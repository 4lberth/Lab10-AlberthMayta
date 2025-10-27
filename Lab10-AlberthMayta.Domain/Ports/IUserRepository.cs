using Lab10_AlberthMayta.Infrastructure;

namespace Lab10_AlberthMayta.Domain.Ports
{
    public interface IUserRepository : IRepository<User>
    {
        // Métodos específicos para Usuarios
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
    }
}