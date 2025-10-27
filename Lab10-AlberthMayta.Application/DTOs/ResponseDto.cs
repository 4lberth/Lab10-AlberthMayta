namespace Lab10_AlberthMayta.Application.DTOs
{
    public class ResponseDto
    {
        public Guid ResponseId { get; set; }
        public Guid ResponderId { get; set; }
        public string ResponderUsername { get; set; } // Para saber quién escribió
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}