using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Domain.Interfaces.Infraestructure
{
    public interface ICotacaoB3ApiManager
    {
        Task<AssetB3Dto?> BuscarAtivoAsync(string ticker);
        Task<List<AssetB3Dto>> BuscarTodosAtivosAsync();
        Task<decimal> ObterPrecoAtualAsync(string ticker);
    }
}
