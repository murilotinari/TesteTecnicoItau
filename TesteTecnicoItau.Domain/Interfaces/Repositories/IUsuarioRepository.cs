using TesteTecnicoItau.Domain.Entities;

namespace TesteTecnicoItau.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioEntity>> ListarTodosAsync();
    }
}
