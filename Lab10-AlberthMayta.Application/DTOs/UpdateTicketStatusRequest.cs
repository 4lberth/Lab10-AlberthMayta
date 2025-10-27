namespace Lab10_AlberthMayta.Application.DTOs
{
    public class UpdateTicketStatusRequest
    {
        // Debe ser 'abierto', 'en_proceso' o 'cerrado'
        public string Status { get; set; }
    }
}