

using MediatR;
using Txt.Shared.Commands.Interfaces;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Application.Commands.Interfaces;

public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, OneOf<TResponse, Error>> where TRequest : ICommand<TResponse>
{
}