using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Domain.Interfaces.Repositories
{
    public interface IPosicaoRepository
    {
        Task AtualizarAsync(PosicaoEntity posicao);
        Task<PosicaoEntity> ObterPosicaoPorUsuarioIdEAtivoIdAsync(int usuarioId, int ativoId);
        Task<List<TopPosicaoDto>> ObterTop10ClientesComMaioresPosicoesAsync();

    }
}
