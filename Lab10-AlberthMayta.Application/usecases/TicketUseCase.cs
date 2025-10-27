using Lab10_AlberthMayta.Application.DTOs;
using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure;

namespace Lab10_AlberthMayta.Application.usecases
{
    public class TicketUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // --- Caso de Uso: User crea un ticket ---
        public async Task<TicketDetailDto> CreateTicketAsync(CreateTicketRequest request, Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId); // Para obtener el nombre

            var newTicket = new Ticket
            {
                TicketId = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                UserId = userId,
                Status = "abierto",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.TicketRepository.AddAsync(newTicket);
            await _unitOfWork.SaveAsync();

            return MapTicketToDetailDto(newTicket, user, new List<Response>());
        }

        // --- Caso de Uso: Admin ve todos los tickets ---
        public async Task<IEnumerable<TicketSummaryDto>> GetAllTicketsForAdminAsync()
        {
            var tickets = await _unitOfWork.TicketRepository.GetOpenTicketsWithUserAsync();
            return tickets.Select(t => new TicketSummaryDto
            {
                TicketId = t.TicketId,
                Title = t.Title,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                CreatorUsername = t.User.Username // <-- Por esto incluimos al User
            });
        }
        
        // --- Caso de Uso: User ve sus tickets ---
        public async Task<IEnumerable<TicketSummaryDto>> GetMyTicketsAsync(Guid userId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsByUserIdAsync(userId);
            return tickets.Select(t => new TicketSummaryDto
            {
                TicketId = t.TicketId,
                Title = t.Title,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                CreatorUsername = t.User.Username
            });
        }

        // --- Caso de Uso: User o Admin ven un ticket ---
        public async Task<TicketDetailDto> GetTicketByIdAsync(Guid ticketId, Guid userId, bool isAdmin)
        {
            var ticket = await _unitOfWork.TicketRepository.GetTicketWithUserByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket no encontrado");

            // --- REGLA DE ROL (Flow 5) ---
            if (!isAdmin && ticket.UserId != userId)
            {
                throw new AccessViolationException("No tiene permiso para ver este ticket.");
            }

            var responses = await _unitOfWork.ResponseRepository.GetResponsesByTicketIdAsync(ticketId);
            
            // Para obtener los nombres de los que responden
            var responderIds = responses.Select(r => r.ResponderId).Distinct();
            var responders = (await _unitOfWork.UserRepository.GetAllAsync()) // Simplificado
                                .Where(u => responderIds.Contains(u.UserId));

            // Mapeamos
            var ticketDto = MapTicketToDetailDto(ticket, ticket.User, responses);
            ticketDto.Responses = responses.Select(r => MapResponseToDto(r, responders.FirstOrDefault(u => u.UserId == r.ResponderId))).ToList();

            return ticketDto;
        }

        // --- Caso de Uso: User o Admin responden ---
        public async Task<ResponseDto> AddResponseAsync(Guid ticketId, AddResponseRequest request, Guid userId, bool isAdmin)
        {
            var ticket = await _unitOfWork.TicketRepository.GetByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket no encontrado");

            // --- REGLA DE ROL (Flow 5) ---
            if (!isAdmin && ticket.UserId != userId)
            {
                throw new AccessViolationException("No tiene permiso para responder a este ticket.");
            }

            var responder = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var newResponse = new Response
            {
                ResponseId = Guid.NewGuid(),
                TicketId = ticketId,
                ResponderId = userId,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ResponseRepository.AddAsync(newResponse);

            // --- REGLA DE ROL (Flow 4) ---
            if (isAdmin && ticket.Status == "abierto")
            {
                ticket.Status = "en_proceso";
                _unitOfWork.TicketRepository.Update(ticket);
            }

            await _unitOfWork.SaveAsync();
            return MapResponseToDto(newResponse, responder);
        }
        
        // --- Caso de Uso: Admin cierra ticket ---
        public async Task UpdateTicketStatusAsync(Guid ticketId, UpdateTicketStatusRequest request)
        {
            var ticket = await _unitOfWork.TicketRepository.GetByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket no encontrado");

            ticket.Status = request.Status;
            if (request.Status == "cerrado")
            {
                ticket.ClosedAt = DateTime.UtcNow;
            }
            else
            {
                ticket.ClosedAt = null;
            }
            
            _unitOfWork.TicketRepository.Update(ticket);
            await _unitOfWork.SaveAsync();
        }

        // --- Mapeadores Manuales ---
        private TicketDetailDto MapTicketToDetailDto(Ticket t, User creator, IEnumerable<Response> responses)
        {
            return new TicketDetailDto
            {
                TicketId = t.TicketId, UserId = t.UserId, Title = t.Title, Description = t.Description,
                Status = t.Status, CreatedAt = t.CreatedAt, ClosedAt = t.ClosedAt,
                CreatorUsername = creator?.Username ?? "Desconocido"
            };
        }
        
        private ResponseDto MapResponseToDto(Response r, User responder)
        {
            return new ResponseDto
            {
                ResponseId = r.ResponseId, ResponderId = r.ResponderId, Message = r.Message, CreatedAt = r.CreatedAt,
                ResponderUsername = responder?.Username ?? "Desconocido"
            };
        }
    }
}