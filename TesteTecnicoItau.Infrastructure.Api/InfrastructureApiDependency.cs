using Microsoft.Extensions.DependencyInjection;
using TesteTecnicoItau.Domain.Interfaces.Infraestructure;

namespace TesteTecnicoItau.Infrastructure.Api
{
    public static class InfrastructureApiDependency
    {
        public static void ConfigureInfrastructureApi(this IServiceCollection services)
        {
            services.AddScoped<ICotacaoB3ApiManager, CotacaoB3ApiManager>();
        }
    }
}
