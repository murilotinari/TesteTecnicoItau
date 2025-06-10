using System.ComponentModel.DataAnnotations.Schema;

namespace TesteTecnicoItau.Domain.Entities;

public class OperacaoEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("ativo_id")]
    public int AtivoId { get; set; }

    [Column("qtd")]
    public int Qtd { get; set; }

    [Column("preco_unit")]
    public decimal PrecoUnit { get; set; }

    [Column("tipo_op")]
    public string TipoOp { get; set; } = null!; // compra | venda

    [Column("corretagem")]
    public decimal Corretagem { get; set; }

    [Column("data_hora")]
    public DateTime DataHora { get; set; }

    // Navegação
    public UsuarioEntity Usuario { get; set; } = null!;

    public AtivoEntity Ativo { get; set; } = null!;
}
