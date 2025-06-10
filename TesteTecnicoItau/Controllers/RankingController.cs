using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TesteTecnicoItau.Domain.Interfaces.Repositories;

namespace TesteTecnicoItau.Controllers
{
    [ApiController]
    [Route("api/ranking")]
    public class RankingController : ControllerBase
    {
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly IPosicaoRepository _posicaoRepository;

        public RankingController(IOperacaoRepository operacaoRepository, IPosicaoRepository posicaoRepository)
        {
            _operacaoRepository = operacaoRepository;
            _posicaoRepository = posicaoRepository;
        }

        /// <summary>
        /// Retorna os 10 clientes com as maiores posições em ativos.
        /// </summary>
        /// <returns>Lista dos 10 clientes com maiores posições</returns>
        [HttpGet("top10clientesposicao")]
        [SwaggerOperation(
            Summary = "Top 10 clientes por posição",
            Description = "Retorna os 10 clientes com os maiores volumes de posição consolidada em ativos.")]
        [SwaggerResponse(200, "Lista de clientes com maiores posições retornada com sucesso", typeof(IEnumerable<object>))]
        public async Task<IActionResult> GetTop10PosicoesClientes()
        {
            var response = await _posicaoRepository.ObterTop10ClientesComMaioresPosicoesAsync();
            return Ok(response);
        }

        /// <summary>
        /// Retorna os 10 clientes que mais geraram corretagem.
        /// </summary>
        /// <returns>Lista dos 10 clientes com maior volume de corretagem</returns>
        [HttpGet("top10clientescorretagem")]
        [SwaggerOperation(
            Summary = "Top 10 clientes por corretagem",
            Description = "Retorna os 10 clientes que mais geraram receita de corretagem para a corretora.")]
        [SwaggerResponse(200, "Lista de clientes por volume de corretagem retornada com sucesso", typeof(IEnumerable<object>))]
        public async Task<IActionResult> GetTop10ClientesCorretagem()
        {
            var response = await _operacaoRepository.ObterTop10ClientesPorCorretagemAsync();
            return Ok(response);
        }
    }
}
