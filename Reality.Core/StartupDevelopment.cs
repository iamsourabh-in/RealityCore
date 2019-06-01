using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Reality.Core.Middlewares;
using Reality.Core.Services;
using System;

namespace Reality.Core
{
    public class StartupDevelopment
    {


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<ITestService, TestService>();
            services.AddScoped<FactoryActivatedMiddleware>();
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}
