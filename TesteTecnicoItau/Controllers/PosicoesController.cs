using Microsoft.AspNetCore.Mvc;
using TesteTecnicoItau.Application.Services;
using TesteTecnicoItau.Domain.Interfaces.Application;

namespace TesteTecnicoItau.Controllers
{
    public class PosicoesController : ControllerBase
    {
        private readonly IInvestimentoUsuarioService _investimentoUsuarioService;

        public PosicoesController(IInvestimentoUsuarioService investimentoUsuarioService)
        {
            _investimentoUsuarioService = investimentoUsuarioService;
        }

        [HttpPost("popular-posicoes")]
        public async Task<IActionResult> Popular()
        {
            await _investimentoUsuarioService.PopularPosicoesAsync();
            return Ok("Posições atualizadas com sucesso");
        }
    }
}
