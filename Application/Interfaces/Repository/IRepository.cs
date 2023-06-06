using System.Linq.Expressions;

namespace Application.Repository
{
    public interface IRepository<T> 
    {
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> expression, params string[] includes);
        Task<T?> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity, params string[] includes);
        Task<bool> DeleteAsync(Guid id);
    }
}
