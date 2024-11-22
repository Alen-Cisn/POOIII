using MediatR;
using Txt.Shared.Dtos;

namespace Txt.Shared.Queries;

public class RootFolderQuery : IRequest<FolderDto>;