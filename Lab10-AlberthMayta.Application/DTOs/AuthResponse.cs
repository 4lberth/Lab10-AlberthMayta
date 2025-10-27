namespace Lab10_AlberthMayta.Application.DTOs
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Token { get; set; } // El JWT
    }
}