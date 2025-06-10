using System.ComponentModel.DataAnnotations.Schema;

namespace TesteTecnicoItau.Domain.Entities;

public class CotacaoEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("ativo_id")]
    public int AtivoId { get; set; }

    [Column("preco_unit")]
    public decimal PrecoUnit { get; set; }

    [Column("data_hora")]
    public DateTime DataHora { get; set; }

    // Navegação
    public AtivoEntity Ativo { get; set; } = null!;
}
