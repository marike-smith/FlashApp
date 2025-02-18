namespace FlashApp.API.Middleware;

public class SecurityHeadersMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        // Add headers to the response
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
        
        var csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';";
        context.Response.Headers["Content-Security-Policy"] = csp;
        context.Response.Headers["X-Content-Security-Policy"] = csp;

        context.Response.Headers["Referrer-Policy"] = "no-referrer";
        context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
        context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        
        await next(context);
    }
}