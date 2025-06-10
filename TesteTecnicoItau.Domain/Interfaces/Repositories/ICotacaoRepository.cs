using TesteTecnicoItau.Domain.Entities;

namespace TesteTecnicoItau.Domain.Interfaces.Repositories
{
    public interface ICotacaoRepository
    {
        Task<bool> ExisteCotacaoAsync(int ativoId, DateTime dataHora);
        Task SalvarAsync(CotacaoEntity cotacao);
        Task<decimal> ObterPrecoAtualAsync(int ativoId);
    }
}
