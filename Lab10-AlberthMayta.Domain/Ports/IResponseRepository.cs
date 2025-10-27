using Lab10_AlberthMayta.Infrastructure;

namespace Lab10_AlberthMayta.Domain.Ports
{
    public interface IResponseRepository : IRepository<Response>
    {
        // Métodos específicos para respuestas
        Task<IEnumerable<Response>> GetResponsesByTicketIdAsync(Guid ticketId);
    }
}