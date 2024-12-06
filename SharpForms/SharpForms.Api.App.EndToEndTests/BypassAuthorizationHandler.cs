using Microsoft.AspNetCore.Authorization;

namespace SharpForms.Api.App.EndToEndTests;


public class BypassAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        foreach (var requirement in context.Requirements)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
