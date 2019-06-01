using Microsoft.AspNetCore.Builder;

namespace Reality.Core.Middlewares
{
    public static class MiddlewareExtentions
    {
        public static IApplicationBuilder UseConventionalMiddleware(
       this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConventionalMiddleware>();
        }

        public static IApplicationBuilder UseFactoryActivatedMiddleware(
            this IApplicationBuilder builder, bool active)
        {
            return builder.UseMiddleware<FactoryActivatedMiddleware>();
        }
    }
}
