using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Infrastructure.Data.Context;

namespace TesteTecnicoItau.Infrastructure.Data.Repositories
{
    public class AtivoRepository(AppDbContext context) : IAtivoRepository
    {

        private readonly AppDbContext _context = context;

        public async Task<int> ObterIdPorTickerAsync(string ticker)
        {
            return await _context.Ativos
                .Where(a => a.Codigo == ticker)
                .Select(a => (int)a.Id)
                .FirstOrDefaultAsync();
        }
    }
}
