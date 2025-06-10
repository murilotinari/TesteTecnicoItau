using System.ComponentModel.DataAnnotations.Schema;

namespace TesteTecnicoItau.Domain.Entities;

public class PosicaoEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("ativo_id")]
    public int AtivoId { get; set; }

    [Column("qtd")]
    public int Qtd { get; set; }

    [Column("preco_medio")]
    public decimal PrecoMedio { get; set; }

    [Column("pl")]
    public decimal PL { get; set; }

    // Navegação
    public UsuarioEntity Usuario { get; set; } = null!;

    public AtivoEntity Ativo { get; set; } = null!;
}

