namespace Lab10_AlberthMayta.Domain.Ports
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id); // T? significa que puede devolver nulo
        Task AddAsync(T entity);
        void Update(T entity); // Update y Delete no suelen ser async
        void Delete(T entity);
    }
}