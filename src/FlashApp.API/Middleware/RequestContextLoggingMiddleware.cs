﻿using Serilog.Context;

namespace FlashApp.API.Middleware;

/// <summary>
/// Middleware to give the flexibility of either taking in the correlation id externally from another service talking
/// with my API allowing me to trace a single request across multiple services in a microservice environment
/// </summary>
public class RequestContextLoggingMiddleware(RequestDelegate requestDelegate)
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public Task Invoke(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
        {
            return requestDelegate(context);
        }
    }

    private static string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out Microsoft.Extensions.Primitives.StringValues correlationId);

        return correlationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}
