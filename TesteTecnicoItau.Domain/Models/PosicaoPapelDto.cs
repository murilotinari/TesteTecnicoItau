namespace TesteTecnicoItau.Domain.Models
{
    public class PosicaoPapelDto
    {
        public string CodigoAtivo { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoMedio { get; set; }
        public decimal PrecoAtual { get; set; }
        public decimal ValorMercado { get; set; }
        public decimal PL { get; set; } // Lucro ou Prejuízo
    }

}
