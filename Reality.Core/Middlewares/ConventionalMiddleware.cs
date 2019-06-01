using Microsoft.AspNetCore.Http;
using Reality.Core.Services;
using System.Threading.Tasks;

namespace Reality.Core.Middlewares
{
    public class ConventionalMiddleware
    {
        private readonly RequestDelegate _next;
        private ITestService _testService;
        public ConventionalMiddleware(RequestDelegate next, ITestService testService)
        {
            _next = next;
            _testService = testService;
        }

        public async Task InvokeAsync(HttpContext context)

        {
            var keyValue = context.Request.Query["username"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                await context.Response.WriteAsync(_testService.GetCount().ToString() + "\n");
            }

            await _next(context);
        }
    }
}
