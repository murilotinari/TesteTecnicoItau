using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Domain.Models;
using TesteTecnicoItau.Infrastructure.Data.Context;

namespace TesteTecnicoItau.Infrastructure.Data.Repositories
{
    public class OperacaoRepository : IOperacaoRepository
    {
        private readonly AppDbContext _context;

        public OperacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioEntity>> ObterUsuariosPorAtivoIdAsync(int ativoId)
        {
            return await _context.Operacoes
                .Where(o => o.AtivoId == ativoId)
                .Select(o => o.Usuario)
                .Distinct()
                .ToListAsync();
        }

        // retorna o total investido por ativo de um usuário
        public async Task<decimal> ObterTotalInvestidoPorAtivoAsync(int usuarioId, int ativoId)
        {
            return await _context.Operacoes
                .Where(o => o.UsuarioId == usuarioId && o.AtivoId == ativoId && o.TipoOp == "compra")
                .SumAsync(o => o.Qtd * o.PrecoUnit);
        }

        // retorna todas as operações feitas por um usuário em um determinado ativo
        public async Task<List<OperacaoEntity>> ObterOperacoesPorUsuarioEAtivoAsync(int usuarioId, int ativoId)
        {
            return await _context.Operacoes
            .Where(o => o.UsuarioId == usuarioId && o.AtivoId == ativoId)
            .OrderBy(o => o.DataHora)
            .ToListAsync();
        }

        public async Task<List<AtivoEntity>> ObterAtivosOperadosPorUsuarioAsync(int usuarioId)
        {
            return await _context.Operacoes
           .Where(o => o.UsuarioId == usuarioId)
           .Select(o => o.Ativo)
           .Distinct()
           .ToListAsync();
        }

        public async Task<List<OperacaoEntity>> ObterComprasPorAtivoAsync(int ativoId)
        {
            return await _context.Operacoes
                .Where(o => o.AtivoId == ativoId && o.TipoOp == "compra")
                .ToListAsync();
        }

        public async Task<decimal> ObterTotalCorretagemPorUsuarioAsync(int usuarioId)
        {
            return await _context.Operacoes
                .Where(o => o.UsuarioId == usuarioId)
                .SumAsync(o => o.Corretagem);
        }

        public async Task<decimal> ObterTotalCorretagemAsync()
        {
            return await _context.Operacoes.SumAsync(op => op.Corretagem);
        }

        public async Task<List<TopCorretagemDto>> ObterTop10ClientesPorCorretagemAsync()
        {
            return await _context.Operacoes
                .GroupBy(o => o.UsuarioId)
                .Select(g => new TopCorretagemDto
                {
                    UsuarioId = g.Key,
                    TotalCorretagem = g.Sum(o => o.Corretagem)
                })
                .OrderByDescending(x => x.TotalCorretagem)
                .Take(10)
                .ToListAsync();
        }
    }
}
