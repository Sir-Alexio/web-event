using System.Linq.Expressions;

namespace WebEvent.API.Repository.Abstract
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IQueryable<T>> GetAll(bool trackChanges);
        Task<IQueryable<T>> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
