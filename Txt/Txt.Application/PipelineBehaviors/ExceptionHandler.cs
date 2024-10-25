
using System.Data.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Txt.Shared.Commands.Interfaces;
using Txt.Shared.ErrorModels;
using Txt.Shared.Result;

namespace Txt.Application.PipelineBehaviors;

public sealed class ExceptionHandler<TRequest, TResponse>(ILogger<ExceptionHandler<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, OneOf<TResponse, Error>>
    where TRequest : ICommand<TResponse>
{
    public async Task<OneOf<TResponse, Error>> Handle(TRequest request, RequestHandlerDelegate<OneOf<TResponse, Error>> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DbException dbException)
        {
            logger.LogInformation(dbException, dbException.Message);
            return new(dbException.ErrorCode switch
            {
                2627 => new Error
                {
                    ErrorCode = 400,
                    Details = "A record with the same unique key already exists."
                },
                547 => new Error
                {
                    ErrorCode = 400,
                    Details = "The operation violates a foreign key constraint."
                },
                53 => new Error
                {
                    ErrorCode = 503,
                    Details = "Unable to connect to the database. Please try again later."
                },
                _ => new Error
                {
                    ErrorCode = 500,
                    Details = "A database error occurred. Please contact support."
                }
            });
        }
        catch (ArgumentNullException argNullException)
        {
            logger.LogInformation(argNullException, argNullException.Message);
            return new(new Error
            {
                ErrorCode = 400,
                Details = $"A required argument was null: {argNullException.ParamName}."
            });
        }
        catch (InvalidOperationException invalidOpException)
        {
            logger.LogInformation(invalidOpException, invalidOpException.Message);
            return new(new Error
            {
                ErrorCode = 400,
                Details = $"Invalid operation: {invalidOpException.Message}."
            });
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, ex.Message);
            return new(new Error
            {
                ErrorCode = 500,
                Details = "An unexpected error occurred. Please try again later.",
            });
        }
    }
}