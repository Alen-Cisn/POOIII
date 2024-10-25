using MediatR;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Shared.Commands.Interfaces;

public interface ICommand<TResult> : IRequest<OneOf<TResult, Error>>
{
}