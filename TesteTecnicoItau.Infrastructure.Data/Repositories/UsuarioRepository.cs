using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Domain.Entities;
using TesteTecnicoItau.Domain.Interfaces.Repositories;
using TesteTecnicoItau.Infrastructure.Data.Context;

namespace TesteTecnicoItau.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioEntity>> ListarTodosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
    }
}
