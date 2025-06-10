using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Controllers
{
    [ApiController]
    [Route("api/corretora")]
    public class CorretoraController : ControllerBase
    {
        private readonly IOperacaoRepository _operacaoRepository;

        public CorretoraController(IOperacaoRepository operacaoRepository)
        {
            _operacaoRepository = operacaoRepository;
        }

        /// <summary>
        /// Obtém a corretagem total recebida pela corretora com base nas corretagens realizadas.
        /// </summary>
        /// <returns>Valor total de corretagem obtido</returns>
        [HttpGet("lucro")]
        [SwaggerOperation(Summary = "Retorna o lucro total da corretora", Description = "Consulta o valor total arrecadado com taxas de corretagem sobre as operações realizadas.")]
        [SwaggerResponse(200, "Lucro de corretagem obtido com sucesso", typeof(TotalCorretagemDto))]
        public async Task<IActionResult> GetCorretagemCorretora()
        {
            var lucro = await _operacaoRepository.ObterTotalCorretagemAsync();
            var response = new TotalCorretagemDto(lucro);
            return Ok(response);
        }
    }
}
