using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Domain.Models;
using TesteTecnicoItau.Infrastructure.Data.Context;

namespace TesteTecnicoItau.Infrastructure.Data.Repositories
{
    public class PosicaoRepository : IPosicaoRepository
    {
        private readonly AppDbContext _context;

        public PosicaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AtualizarAsync(PosicaoEntity posicao)
        {
            var existente = await _context.Posicoes
                    .FirstOrDefaultAsync(p => p.UsuarioId == posicao.UsuarioId && p.AtivoId == posicao.AtivoId);

            if (existente == null)
            {
                await _context.Posicoes.AddAsync(posicao);
            }
            else
            {
                existente.Qtd = posicao.Qtd;
                existente.PrecoMedio = posicao.PrecoMedio;
                existente.PL = posicao.PL;
                _context.Posicoes.Update(existente);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<PosicaoEntity> ObterPosicaoPorUsuarioIdEAtivoIdAsync(int usuarioId, int ativoId)
        {
            return await _context.Posicoes
                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.AtivoId == ativoId);
        }

        public async Task<List<TopPosicaoDto>> ObterTop10ClientesComMaioresPosicoesAsync()
        {
            return await _context.Posicoes
                .GroupBy(p => p.UsuarioId)
                .Select(g => new TopPosicaoDto
                {
                    UsuarioId = g.Key,
                    ValorTotal = g.Sum(p => p.Qtd * p.PrecoMedio)
                })
                .OrderByDescending(x => x.ValorTotal)
                .Take(10)
                .ToListAsync();
        }
    }
}
