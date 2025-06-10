using System.ComponentModel.DataAnnotations.Schema;

namespace TesteTecnicoItau.Domain.Entities;

public class UsuarioEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = null!;

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("pct_corretagem")]
    public decimal PctCorretagem { get; set; }

    // Navegação
    public ICollection<OperacaoEntity> Operacoes { get; set; } = new List<OperacaoEntity>();

    public ICollection<PosicaoEntity> Posicoes { get; set; } = new List<PosicaoEntity>();
}
