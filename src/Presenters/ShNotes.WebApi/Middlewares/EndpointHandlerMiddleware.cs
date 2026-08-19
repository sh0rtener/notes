using Newtonsoft.Json;
using ShNotes.Core;
using ShNotes.UseCases;
using ShNotes.WebApi.Common;

namespace ShNotes.WebApi.Middlewares;

public sealed class EndpointHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public EndpointHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex) when (ex is CoreException || ex is InvalidDataException || ex is UseCaseException)
        {
            var request = new ApiResponse<object>() { Message = ex.Message.Trim() };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var result = JsonConvert.SerializeObject(request);

            await httpContext.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            var request = new ApiResponse<object>() { };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

#if DEGUB
            request.Message = ex.Message;
#endif

            var result = JsonConvert.SerializeObject(request);

            await httpContext.Response.WriteAsync(result);
        }
#pragma warning restore CS0168 // Variable is declared but never used
    }
}
