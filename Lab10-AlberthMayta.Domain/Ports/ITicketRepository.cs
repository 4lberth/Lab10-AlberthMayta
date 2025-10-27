using Lab10_AlberthMayta.Infrastructure;

namespace Lab10_AlberthMayta.Domain.Ports
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        // Método para el Admin (ver todos)
        Task<IEnumerable<Ticket>> GetOpenTicketsWithUserAsync();
        
        // Método para el User (ver los suyos)
        Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(Guid userId);
        
        // Método para ver un ticket con su creador
        Task<Ticket> GetTicketWithUserByIdAsync(Guid ticketId);
    }
}