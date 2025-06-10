using System.ComponentModel.DataAnnotations.Schema;

namespace TesteTecnicoItau.Domain.Entities;

public class AtivoEntity
{
    [Column("id")]
    public int Id { get; set; }

    [Column("codigo")]
    public string Codigo { get; set; } = null!;

    [Column("nome")]
    public string Nome { get; set; } = null!;

    // Navegação
    public ICollection<OperacaoEntity> Operacoes { get; set; } = new List<OperacaoEntity>();

    public ICollection<CotacaoEntity> Cotacoes { get; set; } = new List<CotacaoEntity>();

    public ICollection<PosicaoEntity> Posicoes { get; set; } = new List<PosicaoEntity>();
}

