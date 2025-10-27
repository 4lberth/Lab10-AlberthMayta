using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab10_AlberthMayta.Infrastructure.Adapters
{
    public class ResponseRepository : Repository<Response>, IResponseRepository
    {
        public ResponseRepository(Lab10ARMCContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Response>> GetResponsesByTicketIdAsync(Guid ticketId)
        {
            return await _context.Responses
                .Where(r => r.TicketId == ticketId)
                .OrderBy(r => r.CreatedAt) // Ordenar por fecha
                .ToListAsync();
        }
    }
}