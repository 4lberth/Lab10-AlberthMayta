using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab10_AlberthMayta.Infrastructure.Adapters
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(Lab10ARMCContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserRole>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
        }
    }
}