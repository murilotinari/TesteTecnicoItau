using TesteTecnicoItau.Application.Helpers;
using TesteTecnicoItau.Domain.Entities;
using Xunit;

namespace TesteTecnicoItau.Tests
{
    public class InvestimentoHelperTests
    {
        public static IEnumerable<object[]> ListasInvalidas()
        {
            yield return new object[] { null! };
            yield return new object[] { new List<OperacaoEntity>() };
        }

        [Theory]
        [MemberData(nameof(ListasInvalidas))]
        public void CalcularPrecoMedio_DeveLancarExcecao_SeListaNulaOuVazia(List<OperacaoEntity> operacoes)
        {
            Assert.Throws<ArgumentException>(() =>
                CalculosHelper.CalcularPrecoMedio(operacoes));
        }


        [Fact]
        public void CalcularPrecoMedio_DeveLancarExcecao_SeTodasOperacoesForemVenda()
        {
            var operacoes = new List<OperacaoEntity>
            {
                new OperacaoEntity { AtivoId = 1, Qtd = 10, PrecoUnit = 20, TipoOp = "venda" }
            };

            Assert.Throws<InvalidOperationException>(() =>
                CalculosHelper.CalcularPrecoMedio(operacoes));
        }

        [Fact]
        public void CalcularPrecoMedio_DeveLancarExcecao_SeOperacoesDeAtivosDiferentes()
        {
            var operacoes = new List<OperacaoEntity>
            {
                new OperacaoEntity { AtivoId = 1, Qtd = 10, PrecoUnit = 20, TipoOp = "compra" },
                new OperacaoEntity { AtivoId = 2, Qtd = 5, PrecoUnit = 30, TipoOp = "compra" }
            };

            Assert.Throws<InvalidOperationException>(() =>
                CalculosHelper.CalcularPrecoMedio(operacoes));
        }

        [Theory]
        [InlineData(100, 6, 200, 4, 140)]    // (100*10 + 200*4) / (10+5) = 12
        [InlineData(51, 2, 150, 6, 125.25)]    // (50*2 + 150*6) / (2+6) = 17.5
        [InlineData(300, 3, 0, 0, 300)]      // Apenas 1 compra: (300*3) / 3 = 100
        public void CalcularPrecoMedio_DeveCalcularCorretamente(
            decimal preco1, int qtd1,
            decimal preco2, int qtd2,
            decimal resultadoEsperado)
        {
            // Arrange
            var operacoes = new List<OperacaoEntity>
            {
                new OperacaoEntity { TipoOp = "compra", PrecoUnit = preco1, Qtd = qtd1 },
                new OperacaoEntity { TipoOp = "compra", PrecoUnit = preco2, Qtd = qtd2 }
            };

            // Act
            var precoMedio = CalculosHelper.CalcularPrecoMedio(operacoes);

            // Assert
            Assert.Equal(resultadoEsperado, precoMedio);
        }

        [Fact]
        public void CalcularPrecoMedio_DeveLancarExcecao_SeQuantidadeForZero()
        {
            // Arrange
            var operacoes = new List<OperacaoEntity>
            {
                new OperacaoEntity { TipoOp = "compra", PrecoUnit = 100, Qtd = 0 },
                new OperacaoEntity { TipoOp = "compra", PrecoUnit = 200, Qtd = 0 }
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                CalculosHelper.CalcularPrecoMedio(operacoes));
        }
    }
}

