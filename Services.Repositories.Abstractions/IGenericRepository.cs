
namespace Services.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> IsExistByIdAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<T> RemoveAsync(T entity);
    }
}
