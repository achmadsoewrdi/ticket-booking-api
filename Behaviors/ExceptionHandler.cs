using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace TikectingBooking.Api.Behaviors;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var(statusCode, message) = exception switch
        {
            ValidationException ex => (400, ex.Message),
            DbUpdateConcurrencyException => (409, "Terjadi kesalahan konflik data. Silahkan coba lagi. "), _ => (500, "Terjadi Kesalahan Pada Server")
        };
        
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new {error = message}, cancellationToken);
        return true;
    }
}