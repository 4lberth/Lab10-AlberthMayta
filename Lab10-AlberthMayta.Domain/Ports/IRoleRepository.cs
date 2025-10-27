using Lab10_AlberthMayta.Infrastructure;

namespace Lab10_AlberthMayta.Domain.Ports
{
    public interface IRoleRepository : IRepository<Role>
    {
        // Aquí puedes agregar métodos específicos para Roles
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}