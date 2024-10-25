using Txt.Application.Services.Interfaces;
using Txt.Domain.Entities;
using Txt.Infrastructure.Data;

namespace Txt.Infrastructure.Repositories;

public class FoldersRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
    : RepositoryBase<Folder>(context, currentUserService)
{
}