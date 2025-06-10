using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TesteTecnicoItau.Domain.Interfaces.Infraestructure;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotacoesController : ControllerBase
    {
        private readonly ICotacaoB3ApiManager _cotacaoManager;

        public CotacoesController(ICotacaoB3ApiManager cotacaoManager)
        {
            _cotacaoManager = cotacaoManager;
        }

        /// <summary>
        /// Obtém a cotação atual de um ativo com base no seu ticker.
        /// </summary>
        /// <param name="ticker">Ticker do ativo (ex: PETR4)</param>
        /// <returns>Informações da cotação do ativo</returns>
        [HttpGet("{ticker}")]
        [SwaggerOperation(
            Summary = "Retorna a cotação atual de um ativo",
            Description = "Consulta a API da B3 e retorna o valor atual de mercado do ativo informado via ticker.")]
        [SwaggerResponse(200, "Cotação retornada com sucesso", typeof(CotacaoAtivoDto))]
        [SwaggerResponse(404, "Ticker não encontrado ou não disponível na B3")]
        public async Task<IActionResult> GetCotacao(string ticker)
        {
            var cotacao = await _cotacaoManager.ObterPrecoAtualAsync(ticker);

            if (cotacao == null)
                return NotFound("Ticker não encontrado ou sem dados disponíveis.");

            var response = new CotacaoAtivoDto(cotacao);
            return Ok(response);
        }
    }
}
