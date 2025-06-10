namespace TesteTecnicoItau.Domain.Models
{
    public class PosicaoGlobalDto
    {
        public int UsuarioId { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal ValorAtualCarteira { get; set; }
        public decimal LucroPrejuizoTotal { get; set; }
    }

}
