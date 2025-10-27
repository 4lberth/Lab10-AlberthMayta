namespace Lab10_AlberthMayta.Domain.Ports
{
    public interface IUnitOfWork : IDisposable
    {
        // Propiedades para tus repositorios específicos
        ITicketRepository TicketRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; } // <-- AÑADIDO
        IResponseRepository ResponseRepository { get; } // <-- AÑADIDO

        Task<int> SaveAsync(); 
    }
}