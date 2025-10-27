using Lab10_AlberthMayta.Domain.Ports;
using Lab10_AlberthMayta.Infrastructure.Data.Context; // Tu DbContext
using Microsoft.EntityFrameworkCore;

namespace Lab10_AlberthMayta.Infrastructure.Adapters
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Lab10ARMCContext _context;

        public Repository(Lab10ARMCContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}