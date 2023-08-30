using System.Net;
using HotelListing.API.Exceptions;
using Newtonsoft.Json;

namespace HotelListing.API.Middelware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        this._next = next;
        this._logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something Went Wrong while processing {context.Request.Path}");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        var errorDetails = new ErrorDetails
        {
            ErrorType = "Failure",
            ErrorMessage = exception.Message,
        };

        switch (exception)
        {
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorDetails.ErrorType = "Not Found";
                break;
            default:
                break;
        }

        string response = JsonConvert.SerializeObject(errorDetails);
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(response);
    }
}

public class ErrorDetails
{
    public string ErrorType { get; set; }
    public string ErrorMessage { get; set; }
}