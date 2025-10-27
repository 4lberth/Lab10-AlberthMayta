using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab10_AlberthMayta.Infrastructure.Adapters
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(Lab10ARMCContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}