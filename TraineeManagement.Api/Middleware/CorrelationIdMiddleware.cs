using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TraineeManagement.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationHeaderKey = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Guid correlationId;

        if (context.Request.Headers.TryGetValue(CorrelationHeaderKey, out var headerValue) 
            && Guid.TryParse(headerValue, out var parsedGuid))
        {
            correlationId = parsedGuid;
        }
        else
        {
            correlationId = Guid.NewGuid();
        }

        context.Items[CorrelationHeaderKey] = correlationId;

        await _next(context);
    }
}
