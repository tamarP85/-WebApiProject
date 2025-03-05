
using System.Diagnostics;

namespace WebApiProject.Middleware;

public class LogMiddleware
{


    private RequestDelegate next;

    public LogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        await next(context);
        System.Console.WriteLine($"{context.Request.Path}.{context.Request.Method} took {stopWatch.ElapsedMilliseconds}ms."+$"success: {context.Items["success"]}");
    }
}
public static partial class MiddlewareExtentions{
    public static WebApplication UseLogMiddleware(this WebApplication app){
        app.UseMiddleware<LogMiddleware>();
        return app;
    }
}
