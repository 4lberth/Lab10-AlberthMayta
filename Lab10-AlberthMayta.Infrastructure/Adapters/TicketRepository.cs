using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore; // <-- Importante

namespace Lab10_AlberthMayta.Infrastructure.Adapters
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(Lab10ARMCContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ticket>> GetOpenTicketsWithUserAsync()
        {
            return await _context.Tickets
                .Include(t => t.User) // Incluye los datos del usuario creador
                .Where(t => t.Status != "cerrado")
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(Guid userId)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketWithUserByIdAsync(Guid ticketId)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }
    }
}