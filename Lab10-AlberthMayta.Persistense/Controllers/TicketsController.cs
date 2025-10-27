using Lab10_AlberthMayta.Application.DTOs;
using Lab10_AlberthMayta.Application.usecases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lab10_AlberthMayta.Persistence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Todos los endpoints requieren login
    public class TicketsController : ControllerBase
    {
        private readonly TicketUseCase _ticketUseCase;

        public TicketsController(TicketUseCase ticketUseCase)
        {
            _ticketUseCase = ticketUseCase;
        }

        // Helpers para leer el Token
        private Guid GetUserIdFromToken() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private bool IsUserAdmin() => User.IsInRole("Admin");

        // --- Endpoints para User ---

        [HttpPost]
        [Authorize(Roles = "User")] // (Flow 2)
        public async Task<IActionResult> CreateTicket(CreateTicketRequest request)
        {
            var userId = GetUserIdFromToken();
            var ticketDto = await _ticketUseCase.CreateTicketAsync(request, userId);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticketDto.TicketId }, ticketDto);
        }

        [HttpGet("my-tickets")]
        [Authorize(Roles = "User")] // (Flow 5)
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = GetUserIdFromToken();
            var tickets = await _ticketUseCase.GetMyTicketsAsync(userId);
            return Ok(tickets);
        }

        // --- Endpoints para Admin ---

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")] // (Flow 3)
        public async Task<IActionResult> GetAllTicketsForAdmin()
        {
            var tickets = await _ticketUseCase.GetAllTicketsForAdminAsync();
            return Ok(tickets);
        }

        [HttpPatch("{id:guid}/status")]
        [Authorize(Roles = "Admin")] // (Flow 6)
        public async Task<IActionResult> UpdateStatus(Guid id, UpdateTicketStatusRequest request)
        {
            await _ticketUseCase.UpdateTicketStatusAsync(id, request);
            return NoContent();
        }

        // --- Endpoints Comunes (User y Admin) ---

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "User,Admin")] // (Flow 4 y 5)
        public async Task<IActionResult> GetTicketById(Guid id)
        {
            try
            {
                var ticketDto = await _ticketUseCase.GetTicketByIdAsync(id, GetUserIdFromToken(), IsUserAdmin());
                return Ok(ticketDto);
            }
            catch (AccessViolationException ex) { return StatusCode(StatusCodes.Status403Forbidden, ex.Message); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
        
        [HttpPost("{id:guid}/responses")]
        [Authorize(Roles = "User,Admin")] // (Flow 4 y 5)
        public async Task<IActionResult> AddResponse(Guid id, AddResponseRequest request)
        {
            try
            {
                var responseDto = await _ticketUseCase.AddResponseAsync(id, request, GetUserIdFromToken(), IsUserAdmin());
                return Ok(responseDto);
            }
            catch (AccessViolationException ex) { return Forbid(ex.Message); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
    }
}