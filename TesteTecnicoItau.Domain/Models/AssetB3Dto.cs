namespace TesteTecnicoItau.Domain.Models;

public class AssetB3Dto
{
    public string? Ticker { get; set; }
    public decimal? Price { get; set; }
    public decimal? Priceopen { get; set; }
    public decimal? High { get; set; }
    public decimal? Low { get; set; }
    public long? Volume { get; set; }
    public decimal? Marketcap { get; set; }
    public DateTime? Tradetime { get; set; }
    public long? Volumeavg { get; set; }
    public decimal? Pe { get; set; }
    public decimal? Eps { get; set; }
    public decimal? High52 { get; set; }
    public decimal? Low52 { get; set; }
    public decimal? Change { get; set; }
    public decimal? Changepct { get; set; }
    public decimal? Closeyest { get; set; }
    public long? Shares { get; set; }
}