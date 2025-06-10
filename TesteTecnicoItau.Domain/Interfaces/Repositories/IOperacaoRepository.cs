using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Domain.Interfaces.Repositories
{
    public interface IOperacaoRepository
    {
        Task<decimal> ObterTotalInvestidoPorAtivoAsync(int usuarioId, int ativoId);
        Task<List<OperacaoEntity>> ObterOperacoesPorUsuarioEAtivoAsync(int usuarioId, int ativoId);
        Task<List<AtivoEntity>> ObterAtivosOperadosPorUsuarioAsync(int usuarioId);
        Task<List<OperacaoEntity>> ObterComprasPorAtivoAsync(int ativoId);
        Task<decimal> ObterTotalCorretagemPorUsuarioAsync(int usuarioId);
        Task<decimal> ObterTotalCorretagemAsync();
        Task<List<TopCorretagemDto>> ObterTop10ClientesPorCorretagemAsync();
        Task<List<UsuarioEntity>> ObterUsuariosPorAtivoIdAsync(int ativoId);
    }
}
