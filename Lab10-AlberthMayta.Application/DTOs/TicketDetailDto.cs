namespace Lab10_AlberthMayta.Application.DTOs
{
    public class TicketDetailDto
    {
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }
        public string CreatorUsername { get; set; } // Para saber quién lo creó
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public List<ResponseDto> Responses { get; set; } = new List<ResponseDto>();
    }
}