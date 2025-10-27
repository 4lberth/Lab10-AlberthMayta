using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab10_AlberthMayta.Infrastructure.Adapters
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(Lab10ARMCContext context) : base(context)
        {
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}