

using Serilog;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace WebApiProject.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;

        public LogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await _next(context);
            stopWatch.Stop();
            Log.Information("{RequestPath} {RequestMethod} took {ElapsedMilliseconds}ms. success: {Success}",
                context.Request.Path,
                context.Request.Method,
                stopWatch.ElapsedMilliseconds,
                context.Items["success"]);
        }
    }

    public static class MiddlewareExtensions
    {
        public static WebApplication UseLogMiddleware(this WebApplication app)
        {
            app.UseMiddleware<LogMiddleware>();
            return app;
        }
    }
}
