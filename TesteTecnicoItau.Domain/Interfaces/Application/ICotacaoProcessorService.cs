using TesteTecnicoItau.Domain.Entities;

namespace TesteTecnicoItau.Domain.Interfaces.Application
{
    public interface ICotacaoProcessorService
    {
        Task ProcessarCotacaoAsync(CotacaoEntity cotacao);
    }
}
