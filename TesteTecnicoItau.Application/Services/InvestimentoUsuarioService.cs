using TesteTecnicoItau.Application.Helpers;
using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Interfaces.Application;
using TesteTecnicoItau.Domain.Interfaces.Infraestructure;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Domain.Models;

namespace TesteTecnicoItau.Application.Services
{
    public class InvestimentoUsuarioService : IInvestimentoUsuarioService
    {
        private readonly IOperacaoRepository _operacaoRepository;
        private readonly ICotacaoB3ApiManager _cotacaoManager;
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPosicaoRepository _posicaoRepository;

        public InvestimentoUsuarioService(
            IOperacaoRepository operacaoRepository,
            ICotacaoB3ApiManager cotacaoManager,
            IUsuarioRepository usuarioRepository,
            ICotacaoRepository cotacaoRepository,
            IPosicaoRepository posicaoRepository)
        {
            _operacaoRepository = operacaoRepository;
            _cotacaoManager = cotacaoManager;
            _usuarioRepository = usuarioRepository;
            _cotacaoRepository = cotacaoRepository;
            _posicaoRepository = posicaoRepository;
        }


        public async Task<decimal> ObterTotalInvestidoAsync(int usuarioId, int ativoId)
        {
            return await _operacaoRepository.ObterTotalInvestidoPorAtivoAsync(usuarioId, ativoId);
        }

        // todo fazer a logica
        public async Task<PosicaoPapelDto> ObterPosicaoPorPapel(int usuarioId, int ativoId)
        {
            var operacoes = await _operacaoRepository.ObterOperacoesPorUsuarioEAtivoAsync(usuarioId, ativoId);

            if (operacoes == null || !operacoes.Any())
                return null!;

            var compras = operacoes.Where(o => o.TipoOp == "compra").ToList();
            var vendas = operacoes.Where(o => o.TipoOp == "venda").ToList();

            var precoMedio = CalculosHelper.CalcularPrecoMedio(compras);

            int qtdCompras = compras.Sum(o => o.Qtd);
            int qtdVendas = vendas.Sum(o => o.Qtd);
            int quantidadeAtual = qtdCompras - qtdVendas;

            if (quantidadeAtual <= 0)
                return null!;

            var ticker = operacoes.First().Ativo.Codigo;
            var cotacao = await _cotacaoManager.BuscarAtivoAsync(ticker);

            if (cotacao == null)
                return null!;

            var precoAtual = cotacao.Price ?? 0;
            var valorMercado = quantidadeAtual * precoAtual;
            var pl = CalculosHelper.CalcularPL(precoAtual, precoMedio, quantidadeAtual);

            return new PosicaoPapelDto
            {
                CodigoAtivo = ticker,
                Quantidade = quantidadeAtual,
                PrecoMedio = precoMedio,
                PrecoAtual = precoAtual,
                ValorMercado = valorMercado,
                PL = pl
            };
        }

        public async Task<PosicaoGlobalDto> ObterPosicaoGlobal(int usuarioId)
        {
            var ativos = await _operacaoRepository.ObterAtivosOperadosPorUsuarioAsync(usuarioId);

            decimal totalInvestido = 0;
            decimal valorAtualCarteira = 0;
            decimal lucroPrejuizoTotal = 0;

            foreach (var ativo in ativos)
            {
                var posicao = await ObterPosicaoPorPapel(usuarioId, ativo.Id);

                if (posicao == null || posicao.Quantidade <= 0)
                    continue;

                decimal valorInvestido = posicao.PrecoMedio * posicao.Quantidade;
                decimal valorMercado = posicao.PrecoAtual * posicao.Quantidade;
                decimal pl = valorMercado - valorInvestido;

                totalInvestido += valorInvestido;
                valorAtualCarteira += valorMercado;
                lucroPrejuizoTotal += pl;
            }

            return new PosicaoGlobalDto
            {
                UsuarioId = usuarioId,
                TotalInvestido = totalInvestido,
                ValorAtualCarteira = valorAtualCarteira,
                LucroPrejuizoTotal = lucroPrejuizoTotal
            };
        }

        public async Task PopularPosicoesAsync()
        {
            var usuarios = await _usuarioRepository.ListarTodosAsync();
            foreach (var usuario in usuarios)
            {
                var ativos = await _operacaoRepository.ObterAtivosOperadosPorUsuarioAsync(usuario.Id);
                foreach (var ativo in ativos)
                {
                    var operacoes = await _operacaoRepository.ObterOperacoesPorUsuarioEAtivoAsync(usuario.Id, ativo.Id);
                    var precoMedio = CalculosHelper.CalcularPrecoMedio(operacoes);
                    var qtdCompra = operacoes.Where(o => o.TipoOp == "compra").Sum(o => o.Qtd);
                    var qtdVenda = operacoes.Where(o => o.TipoOp == "venda").Sum(o => o.Qtd);
                    var qtd = qtdCompra - qtdVenda;
                    var precoAtual = await _cotacaoRepository.ObterPrecoAtualAsync(ativo.Id);

                    var pl = CalculosHelper.CalcularPL(precoAtual, precoMedio, qtd);

                    var posicao = new PosicaoEntity
                    {
                        UsuarioId = usuario.Id,
                        AtivoId = ativo.Id,
                        Qtd = qtd,
                        PrecoMedio = precoMedio,
                        PL = pl
                    };

                    await _posicaoRepository.AtualizarAsync(posicao);
                }
            }
        }

    }
}
