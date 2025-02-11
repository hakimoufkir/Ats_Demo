using System.Linq.Expressions;

namespace Ats_Demo.GenericRepo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>?> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
