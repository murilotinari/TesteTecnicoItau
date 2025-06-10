using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Domain.Interfaces.Application
{
    public interface IInvestimentoUsuarioService
    {
        Task<decimal> ObterTotalInvestidoAsync(int usuarioId, int ativoId);
        Task<PosicaoPapelDto> ObterPosicaoPorPapel(int usuarioId, int ativoId);
        Task<PosicaoGlobalDto> ObterPosicaoGlobal(int usuarioId);
        Task PopularPosicoesAsync();
    }
}
