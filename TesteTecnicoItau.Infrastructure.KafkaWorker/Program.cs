using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Infrastructure.Data.Context;
using TesteTecnicoItau.Infrastructure.Data.Repositories;
using TesteTecnicoItau.Infrastructure.KafkaWorker;



Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<CotacaoConsumerWorker>();
        services.AddDbContext<AppDbContext>(); // Substitua pelo seu contexto real
        services.AddScoped<ICotacaoRepository, CotacaoRepository>(); // Repositório de persistência
    })
    .Build()
    .Run();
