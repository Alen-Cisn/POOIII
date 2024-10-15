using MediatR;
using Txt.Domain.Entities;

namespace Txt.Shared.Queries;

public class NoteQuery : IRequest<List<Note>>
{
}