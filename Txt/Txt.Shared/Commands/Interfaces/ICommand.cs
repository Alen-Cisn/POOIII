using MediatR;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Shared.Commands.Interfaces;

public interface ICommand<TResponse> : IRequest<OneOf<TResponse, Error>>
{
}