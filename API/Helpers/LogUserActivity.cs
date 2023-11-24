using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionExecutedContext = await next();

        if (!actionExecutedContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = actionExecutedContext.HttpContext.User.GetUserId();
        var repo = actionExecutedContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var user = await repo.GetUserByIdAsync(userId);
        user.LastActive = DateTime.UtcNow;

        await repo.SaveAllAsync();
    }
}
