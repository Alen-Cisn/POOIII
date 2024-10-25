using Txt.Application.Services.Interfaces;
using Txt.Domain.Entities;
using Txt.Infrastructure.Data;

namespace Txt.Infrastructure.Repositories;

public class NotesRepository(ApplicationDbContext context, ICurrentUserService currentUserService)
    : RepositoryBase<Note>(context, currentUserService)
{
}