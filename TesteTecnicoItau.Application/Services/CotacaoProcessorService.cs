using Microsoft.Extensions.Logging;
using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Interfaces.Application;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Application.Services
{
    public class CotacaoProcessorService : ICotacaoProcessorService
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly IPosicaoRepository _posicaoRepository;
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly IInvestimentoUsuarioService _investimentoUsuarioService;
        private readonly ILogger<CotacaoProcessorService> _logger;

        public CotacaoProcessorService(
            ICotacaoRepository cotacaoRepository,
            IPosicaoRepository posicaoRepository,
            IInvestimentoUsuarioService investimentoUsuarioService,
            IOperacaoRepository operacaoRepository,
            ILogger<CotacaoProcessorService> logger)
        {
            _cotacaoRepository = cotacaoRepository;
            _posicaoRepository = posicaoRepository;
            _investimentoUsuarioService = investimentoUsuarioService;
            _operacaoRepository = operacaoRepository;
            _logger = logger;
        }

        public async Task ProcessarCotacaoAsync(CotacaoEntity cotacao)
        {
            if (await CotacaoJaExiste(cotacao)) return;

            await SalvarCotacaoAsync(cotacao);

            var usuarios = await ObterUsuariosComOperacoesNoAtivo(cotacao.AtivoId);

            foreach (var usuario in usuarios)
            {
                await AtualizarPosicaoDoUsuarioAsync(usuario.Id, cotacao.AtivoId);
            }
        }

        // Método para Idempotencia
        private async Task<bool> CotacaoJaExiste(CotacaoEntity cotacao)
        {
            return await _cotacaoRepository.ExisteCotacaoAsync(cotacao.AtivoId, cotacao.DataHora);
        }

        private async Task SalvarCotacaoAsync(CotacaoEntity cotacao)
        {
            _logger.LogInformation("Salvando cotação para Ativo {AtivoId}", cotacao.AtivoId);
            await _cotacaoRepository.SalvarAsync(cotacao);
        }

        private async Task<List<UsuarioEntity>> ObterUsuariosComOperacoesNoAtivo(int ativoId)
        {
            return await _operacaoRepository.ObterUsuariosPorAtivoIdAsync(ativoId);
        }

        private async Task AtualizarPosicaoDoUsuarioAsync(int usuarioId, int ativoId)
        {
            try
            {
                var papel = await _investimentoUsuarioService.ObterPosicaoPorPapel(usuarioId, ativoId);
                
                _logger.LogInformation("Atualizando posição para usuário {UsuarioId} e ativo {AtivoId}", usuarioId, ativoId);

                var novaPosicao = new PosicaoEntity
                {
                    UsuarioId = usuarioId,
                    AtivoId = ativoId,
                    Qtd = papel.Quantidade,
                    PrecoMedio = papel.PrecoMedio,
                    PL = papel.PL
                };

                await _posicaoRepository.AtualizarAsync(novaPosicao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar posição para usuário {UsuarioId}", usuarioId);
            }
        }
    }

}
