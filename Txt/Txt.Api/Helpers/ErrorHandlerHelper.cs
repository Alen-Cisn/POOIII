
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Txt.Shared.ErrorModels;

namespace Txt.Api.Helpers;
public static class ErrorHandlerHelper
{
    public static void UseErrors(this IApplicationBuilder app, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.Use(WriteDevelopmentResponse);
        }
        else
        {
            app.Use(WriteProductionResponse);
        }
    }

    private static Task WriteDevelopmentResponse(HttpContext httpContext, Func<Task> next)
        => WriteResponse(httpContext, includeDetails: true);

    private static Task WriteProductionResponse(HttpContext httpContext, Func<Task> next)
        => WriteResponse(httpContext, includeDetails: false);

    private static async Task WriteResponse(HttpContext httpContext, bool includeDetails)
    {
        var exceptionFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
        if (exceptionFeature != null)
        {
            Exception exception = exceptionFeature.Error;

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json";

            Error response = new()
            {
                ErrorCode = httpContext.Response.StatusCode,
                Details = includeDetails ? exception.ToString() : "An unexpected error ocurred."
            };

            string jsonResponse = JsonSerializer.Serialize(response);

            await httpContext.Response.WriteAsync(jsonResponse);
        }
    }
}