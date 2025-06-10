using Confluent.Kafka;
using Polly;
using System.Text.Json;
using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Interfaces.Application;

namespace TesteTecnicoItau.Infrastructure.KafkaWorker
{
    public class CotacaoConsumerWorker : BackgroundService
    {
        private readonly ILogger<CotacaoConsumerWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private IConsumer<Ignore, string>? _consumer;
        private IAsyncPolicy _resiliencePolicy;

        public CotacaoConsumerWorker(
            IServiceProvider serviceProvider, 
            IConfiguration configuration, 
            ILogger<CotacaoConsumerWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _resiliencePolicy = BuildPolicy();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConfigureConsumer();
            _consumer!.Subscribe("cotacoes-novas");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = _consumer.Consume(stoppingToken);
                    var cotacao = JsonSerializer.Deserialize<CotacaoEntity>(result.Message.Value);

                    await _resiliencePolicy.ExecuteAsync(() => ProcessarCotacaoAsync(cotacao!));
                }
                catch (ConsumeException ce)
                {
                    _logger.LogError(ce, "Erro no Kafka: {Reason}", ce.Error.Reason);
                    await Task.Delay(3000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado: {Reason}", ex.Message);
                    await Task.Delay(3000, stoppingToken);
                }
            }
        }

        private void ConfigureConsumer()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "cotacao-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        private IAsyncPolicy BuildPolicy()
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (exception, _) =>
                    {
                        _logger.LogWarning(exception, "Retry após erro");
                    });

            var fallbackPolicy = Policy
                .Handle<Exception>()
                .FallbackAsync(async ct =>
                {
                    _logger.LogError("Falha no processamento da cotação. Pulando mensagem.");
                    await Task.CompletedTask;
                });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (ex, breakDelay) =>
                    {
                        _logger.LogError($"Circuito aberto por {breakDelay.TotalSeconds} segundos devido a: {ex.Message}");
                    },
                    onReset: () => _logger.LogError("Circuito fechado. Execuções retomadas."),
                    onHalfOpen: () => _logger.LogError("Circuito em teste. Tentando nova execução.")
                );


            return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, fallbackPolicy);
        }

        private async Task ProcessarCotacaoAsync(CotacaoEntity cotacao)
        {
            using var scope = _serviceProvider.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<ICotacaoProcessorService>();

            await processor.ProcessarCotacaoAsync(cotacao);
        }
    }

}
