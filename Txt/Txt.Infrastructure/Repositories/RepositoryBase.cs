
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using Txt.Application.Services.Interfaces;
using Txt.Domain.Entities.Abstract;
using Txt.Domain.Repositories.Interfaces;
using Txt.Infrastructure.Data;

namespace Txt.Infrastructure.Repositories;

public abstract class RepositoryBase<T>(ApplicationDbContext repositoryContext, ICurrentUserService currentUserService) : IRepositoryBase<T> where T : Auditable
{
    protected ApplicationDbContext Context { get; set; } = repositoryContext;

    public IQueryable<T> FindAll() => Context.Set<T>().AsNoTracking();

    public IQueryable<T> FindWhere(Expression<Func<T, bool>> expression) =>
        Context.Set<T>().Where(expression).AsNoTracking();

    public EntityEntry<T> Create(T entity)
    {
        entity.CreatedOn = DateTime.Now;
        entity.CreatedById = currentUserService.UserId ?? throw new Exception("User not found");
        return Context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        entity.ModifiedOn = DateTime.Now;
        entity.ModifiedById = currentUserService.UserId ?? throw new Exception("User not found");
        Context.Set<T>().Update(entity);
    }

    public void Delete(T entity) => Context.Set<T>().Remove(entity);

    public async Task<EntityEntry<T>> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedOn = DateTime.Now;
        entity.CreatedById = currentUserService.UserId ?? throw new Exception("User not found");

        var entry = await Context.Set<T>().AddAsync(entity, cancellationToken);
        return entry;
    }


}
