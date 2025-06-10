using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TesteTecnicoItau.Application.Helpers;
using TesteTecnicoItau.Domain.Interfaces.Application;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IInvestimentoUsuarioService _investimentoAppService;
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly IAtivoRepository _ativoRepository;

        public ClientesController(
            IInvestimentoUsuarioService investimentoAppService,
            IOperacaoRepository operacaoRepository,
            IAtivoRepository ativorepository)
        {
            _investimentoAppService = investimentoAppService;
            _operacaoRepository = operacaoRepository;
            _ativoRepository = ativorepository;
        }

        /// <summary>
        /// Obtém o preço médio de um ativo adquirido por um usuário.
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="ticker">Ticker do ativo (ex: PETR4)</param>
        /// <returns>Preço médio do ativo</returns>
        [HttpGet("{usuarioId}/ativos/{ticker}/preco-medio")]
        [SwaggerOperation(Summary = "Retorna o preço médio de um ativo", Description = "Calcula o preço médio das operações de um ativo específico para um determinado usuário.")]
        [SwaggerResponse(200, "Preço médio calculado com sucesso", typeof(PrecoMedioDto))]
        [SwaggerResponse(404, "Ativo não encontrado ou usuário sem operações")]
        public async Task<IActionResult> GetPrecoMedio(int usuarioId, string ticker)
        {
            var ativoId = await _ativoRepository.ObterIdPorTickerAsync(ticker);

            if (ativoId == 0)
                return NotFound("Ativo não encontrado.");

            var operacoes = await _operacaoRepository.ObterOperacoesPorUsuarioEAtivoAsync(usuarioId, ativoId);

            if (operacoes == null || !operacoes.Any())
                return NotFound("Não foram encontradas operações para este ativo e usuário.");

            var preco = CalculosHelper.CalcularPrecoMedio(operacoes);
            var response = new PrecoMedioDto(preco);

            return Ok(response);
        }

        /// <summary>
        /// Obtém a posição global de um usuário.
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <returns>Posição global com saldo, ativos e lucro/prejuízo</returns>
        [HttpGet("{usuarioId}/posicao")]
        [SwaggerOperation(Summary = "Retorna a posição global do usuário", Description = "Consulta todas as posições do usuário com saldo, valor de mercado e lucro/prejuízo.")]
        [SwaggerResponse(200, "Posição global retornada com sucesso", typeof(PosicaoGlobalDto))]
        [SwaggerResponse(404, "Usuário não encontrado ou sem posição")]
        public async Task<IActionResult> GetPosicao(int usuarioId)
        {
            var response = await _investimentoAppService.ObterPosicaoGlobal(usuarioId);

            if (response == null)
                return NotFound("Posição não encontrada para o usuário.");

            return Ok(response);
        }
    }
}
