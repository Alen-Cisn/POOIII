using Txt.Application.Services.Interfaces;
using Txt.Domain.Entities;
using Txt.Infrastructure.Data;

namespace Txt.Infrastructure.Repositories;

public class NoteLinesRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
    : RepositoryBase<NoteLine>(context, currentUserService)
{
}