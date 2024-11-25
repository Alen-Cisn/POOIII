
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Txt.Application.Services.Interfaces;
using Txt.Domain.Entities.Abstract;
using Txt.Domain.Repositories.Interfaces;
using Txt.Infrastructure.Data;

namespace Txt.Infrastructure.Repositories;

public abstract class RepositoryBase<T>(ApplicationDbContext repositoryContext, ICurrentUserService currentUserService) : IRepositoryBase<T> where T : Auditable
{
    virtual protected ApplicationDbContext Context { get; set; } = repositoryContext;

    virtual public IQueryable<T> FindAll() => Context.Set<T>().Where(a => a.CreatedById == currentUserService.UserId).AsNoTracking();

    virtual public IQueryable<T> FindWhere(Expression<Func<T, bool>> expression) =>
        Context.Set<T>().Where(a => a.CreatedById == currentUserService.UserId).Where(expression).AsNoTracking();

    virtual public T Create(T entity)
    {
        entity.CreatedOn = DateTime.Now;
        entity.CreatedById = currentUserService.UserId ?? throw new Exception("User not found");
        return Context.Set<T>().Add(entity).Entity;
    }

    virtual public void Update(T entity)
    {
        entity.ModifiedOn = DateTime.Now;
        entity.ModifiedById = currentUserService.UserId ?? throw new Exception("User not found");
        Context.Set<T>().Update(entity);
    }

    virtual public void UpdateRange(T[] entities)
    {
        foreach (var entity in entities)
        {
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedById = currentUserService.UserId ?? throw new Exception("User not found");
        }

        Context.Set<T>().UpdateRange(entities);
    }

    virtual public void Delete(T entity) => Context.Set<T>().Remove(entity);

    virtual public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedOn = DateTime.Now;
        entity.CreatedById = currentUserService.UserId ?? throw new Exception("User not found");

        var entry = await Context.Set<T>().AddAsync(entity, cancellationToken);
        return entry.Entity;
    }

    virtual public void DeleteRange(IEnumerable<T> entities) => Context.Set<T>().RemoveRange(entities);

}
