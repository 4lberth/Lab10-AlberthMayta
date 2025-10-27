namespace Lab10_AlberthMayta.Application.DTOs
{
    public class TicketSummaryDto
    {
        public Guid TicketId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatorUsername { get; set; }
    }
}