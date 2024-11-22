
using System.Data.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using Txt.Shared.Commands.Interfaces;
using Txt.Shared.ErrorModels;
using Txt.Shared.Exceptions;
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
        catch (Exception ex)
        {
            return new(ExceptionHandlerExtension.HandleException(ex, logger));
        }
    }

}

public static class ExceptionHandlerExtension
{
    static public Error HandleException(Exception ex, ILogger logger)
    {
        switch (ex)
        {
            case ValidationException validationException:

                logger.LogInformation(validationException, validationException.Message);
                return new Error
                {
                    ErrorCode = 400,
                    Details = validationException.Message!
                };

            case DbException dbException:

                logger.LogInformation(dbException, dbException.Message);
                return dbException.ErrorCode switch
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
                };

            case ArgumentNullException argNullException:

                logger.LogInformation(argNullException, argNullException.Message);
                return new Error
                {
                    ErrorCode = 400,
                    Details = $"A required argument was null: {argNullException.ParamName}."
                };

            case NotFoundException notFoundException:

                logger.LogInformation(notFoundException, notFoundException.Message);
                return new Error
                {
                    ErrorCode = 404,
                    Details = notFoundException.Message
                };

            case InvalidOperationException invalidOpException:

                logger.LogInformation(invalidOpException, invalidOpException.Message);
                return new Error
                {
                    ErrorCode = 400,
                    Details = $"Invalid operation: {invalidOpException.Message}."
                };

            default:

                logger.LogInformation(ex, ex.Message);
                return new Error
                {
                    ErrorCode = 500,
                    Details = "An unexpected error occurred. Please try again later.",
                };
        }
    }
}