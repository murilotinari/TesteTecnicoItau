using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Infrastructure.Data.Context;

namespace TesteTecnicoItau.Infrastructure.Data.Repositories
{
    public class CotacaoRepository(AppDbContext context) : ICotacaoRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<decimal> ObterPrecoAtualAsync(int ativoId)
        {
            return await _context.Cotacoes
                .Where(c => c.AtivoId == ativoId)
                .OrderByDescending(c => c.DataHora)
                .Select(c => (decimal)c.PrecoUnit)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExisteCotacaoAsync(int ativoId, DateTime dataHora)
        {
            return await _context.Cotacoes.AnyAsync(c =>
                c.AtivoId == ativoId && c.DataHora == dataHora);
        }

        public async Task SalvarAsync(CotacaoEntity cotacao)
        {
            _context.Cotacoes.Add(cotacao);
            await _context.SaveChangesAsync();
        }
    }
}
