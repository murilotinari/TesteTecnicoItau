using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Infrastructure.Data.Context;
using TesteTecnicoItau.Infrastructure.Data.Repositories;

namespace TesteTecnicoItau.Infrastructure.Data
{
    public static class InfrastructureDataDependency
    {
        public static IServiceCollection ConfigureInfrastructureData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IAtivoRepository, AtivoRepository>();
            services.AddScoped<ICotacaoRepository, CotacaoRepository>();
            services.AddScoped<IOperacaoRepository, OperacaoRepository>();
            services.AddScoped<IPosicaoRepository, PosicaoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();


            return services;
        }
    }
}
