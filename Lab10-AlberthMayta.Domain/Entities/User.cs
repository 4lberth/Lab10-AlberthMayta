using System;
using System.Collections.Generic;

namespace Lab10_AlberthMayta.Infrastructure;

public partial class User
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
