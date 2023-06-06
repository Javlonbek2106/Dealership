using System.Linq.Expressions;
using Application.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        IDealershipDbContext context;

        protected Repository(IDealershipDbContext context)
        {
            this.context = context;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid Id)
        {
            var entity = context.Set<T>().Find(Id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public virtual Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> expression, params string[] includes )
        {
            IQueryable<T> source = context.Set<T>();
            if (includes is not null)
                foreach (var item in includes)
                    source = source.Include(item);
            return Task.FromResult(source.Where(expression));
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T?> UpdateAsync(T entity, params string[] includes)
        {
            
            if (entity != null)
            {
                IQueryable<T> source = context.Set<T>();
                if (includes is not null)
                    foreach (var item in includes)
                        source = source.Include(item);
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
            return null;
        }
    }
}
