using Microsoft.Extensions.DependencyInjection;
using TesteTecnicoItau.Application.Services;
using TesteTecnicoItau.Domain.Interfaces.Application;

namespace TesteTecnicoItau.Application
{
    public static class ApplicationDependency
    {
        public static void ConfigureApplication(this IServiceCollection services)
        {
            services.AddScoped<IInvestimentoUsuarioService, InvestimentoUsuarioService>();
            services.AddScoped<ICotacaoProcessorService, CotacaoProcessorService>();
        }
    }
}
