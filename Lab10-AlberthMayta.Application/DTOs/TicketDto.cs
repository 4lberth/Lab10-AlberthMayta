namespace Lab10_AlberthMayta.Application.DTOs
{
    public class TicketDto
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Añadimos las respuestas
        public List<ResponseDto> Responses { get; set; } = new List<ResponseDto>();
    }
}