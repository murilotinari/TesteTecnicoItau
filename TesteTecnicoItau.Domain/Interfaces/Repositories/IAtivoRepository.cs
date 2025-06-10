namespace TesteTecnicoItau.Domain.Interfaces.Repositories
{
    public interface IAtivoRepository
    {
        Task<int> ObterIdPorTickerAsync(string ticker);
    }
}
