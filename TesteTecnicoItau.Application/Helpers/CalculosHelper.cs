using TesteTecnicoItau.Domain.Entities;

namespace TesteTecnicoItau.Application.Helpers
{
    public static class CalculosHelper
    {
        public static decimal CalcularPrecoMedio(List<OperacaoEntity> operacoes)
        {
            if (operacoes == null || operacoes.Count == 0)
                throw new ArgumentException("A lista de operações está vazia ou nula.");

            var comprasValidas = operacoes
                .Where(o => o.TipoOp == "compra" && o.Qtd > 0 && o.PrecoUnit > 0)
                .ToList();

            if (!comprasValidas.Any())
                throw new InvalidOperationException("Nenhuma operação de compra válida encontrada para o cálculo.");


            var ativoIdReferencia = comprasValidas.First().AtivoId;
            bool ativosMistos = comprasValidas.Any(o => o.AtivoId != ativoIdReferencia);

            if (ativosMistos)
                throw new InvalidOperationException("As operações envolvem múltiplos ativos. O cálculo do preço médio deve ser feito por ativo.");

            var totalInvestido = comprasValidas.Sum(o => o.PrecoUnit * o.Qtd);
            var totalQtd = comprasValidas.Sum(o => o.Qtd);

            if (totalQtd == 0)
                throw new InvalidOperationException("A quantidade total comprada é zero. Não é possível calcular o preço médio.");

            return totalInvestido / totalQtd;
        }

        public static decimal CalcularPL(decimal precoAtual, decimal precoMedio, int qtd)
        {
            return (precoAtual - precoMedio) * qtd;
        }
    }
}
