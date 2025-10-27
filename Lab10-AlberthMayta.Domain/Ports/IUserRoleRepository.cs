using Lab10_AlberthMayta.Domain;
using Lab10_AlberthMayta.Infrastructure;

namespace Lab10_AlberthMayta.Domain.Ports
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        // Puedes agregar métodos específicos si los necesitas
        Task<IEnumerable<UserRole>> GetRolesByUserIdAsync(Guid userId);
    }
}