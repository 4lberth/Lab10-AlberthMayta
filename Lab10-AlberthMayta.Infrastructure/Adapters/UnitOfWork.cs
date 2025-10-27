using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure.Data.Context;

namespace Lab10_AlberthMayta.Infrastructure.Adapters
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lab10ARMCContext _context;

        // Repositorios privados para lazy loading
        private ITicketRepository? _ticketRepository;
        private IUserRepository? _userRepository;
        private IRoleRepository? _roleRepository;
        private IUserRoleRepository? _userRoleRepository; // <-- AÑADIDO
        private IResponseRepository? _responseRepository; // <-- AÑADIDO
        
        public UnitOfWork(Lab10ARMCContext context)
        {
            _context = context;
        }

        // Implementación de las propiedades de la interfaz
        public ITicketRepository TicketRepository => 
            _ticketRepository ??= new TicketRepository(_context);

        public IUserRepository UserRepository => 
            _userRepository ??= new UserRepository(_context);

        public IRoleRepository RoleRepository => 
            _roleRepository ??= new RoleRepository(_context);

        public IUserRoleRepository UserRoleRepository =>  // <-- AÑADIDO
            _userRoleRepository ??= new UserRoleRepository(_context);

        public IResponseRepository ResponseRepository =>  // <-- AÑADIDO
            _responseRepository ??= new ResponseRepository(_context);

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}