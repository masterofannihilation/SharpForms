using SharpForms.Api.App.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace SharpForms.Api.App.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
