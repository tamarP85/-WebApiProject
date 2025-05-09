using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using WebApiProject.Services;
namespace WebApiProject.Middlewares
{
    public class ActiveUserMiddleware
    {
        private readonly RequestDelegate _next;

        public ActiveUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ActiveUserService activeUser)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                activeUser.UserId = int.TryParse(
                    context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value,
                    out var userId
                ) ? userId : -1;
                activeUser.Type = context.User.Claims.FirstOrDefault(c => c.Type == "type")?.Value ?? "Agent";
            }
            await _next(context);
        }
    }
}