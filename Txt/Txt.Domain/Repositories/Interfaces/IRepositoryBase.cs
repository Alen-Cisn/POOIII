using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Txt.Domain.Entities.Abstract;

namespace Txt.Domain.Repositories.Interfaces;

public interface IRepositoryBase<T> where T : Auditable
{
    IQueryable<T> FindAll();
    IQueryable<T> FindWhere(Expression<Func<T, bool>> expression);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    T Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
}