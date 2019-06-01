using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reality.Core.Middlewares;
using Reality.Core.Services;
using System;

namespace Reality.Core
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<ITestService, TestService>();
            services.AddScoped<FactoryActivatedMiddleware>();
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            #region Learn Middleware/Pipeline ways

            /*
             * A Middleware is a piece of code that can be pluged in 
             * whenever needed and can be easily replaced.
             * 
             * For Example If you need the any request coming to a route after /api
             * should have a token in the header. You can do this by writing a middleware .
             * 
             * # The advantage is that we can now plug this login into any application we want
             * # and we can also change the logic by only makking a change in one area of the code therefore reducing bugs.
             
             */

            #region Inline Conventional Way (Basic)

            /*This is the conventional inline way of making a middleware work.
              In this approach we use the app.Use method which internally delegates 
              To async (context, next) in which we have written the code to write 
              asyncly to the Response
            */
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello World Inline Conventional Way (Basic) !\n");
                await next();
            });
            #endregion

            #region Inline Conventional Way (Intermidiate)

            /*
             * This is the conventional inline way of making a middleware work.
              In this approach we use the app.Use method which internally delegates 
              To async (context, next) in which we have written the code to write 
              async to the Response.
              The only change from the above middleware is we are now accessing the
              ITestService which we have registered in the ConfigureServices method 
              Above.

            */
            app.Use(async (context, next) =>
            {
                var testService = context.RequestServices.GetRequiredService<ITestService>();
                var username = context.Request.Query["username"].ToString();
                if (!String.IsNullOrEmpty(username))
                {

                    await context.Response.WriteAsync("Hello From Inline Conventional Way (Intermidiate) pipe: Pipe has query string : username -> " + testService.GreetUser(username) + '\n');
                }
                else
                {
                    await context.Response.WriteAsync("Hello From Inline Conventional Way (Intermidiate) pipe \n");
                }

                await next();
            });
            #endregion

            #region Conventional Class Way

            /*This is the conventional inline way of making a middleware work.
              In this approach we use the app.Use method which internally delegates
              the tasks to the InvokeAsync Method in the Class you defined
              which would help to invoke the next middleware in the pipeline.
              
              # The Class Constructor takes a RequestDelegate as a parameter.
              # The InvokeAsync takes a parameter HttpContext on which it 
                writes the Response.
            */



            app.UseConventionalMiddleware();
            #endregion

            #region Factory Activate Class Way

            /*This is the conventional inline way of making a middleware work.
              In this approach we use the app.Use method which internally delegates
              the tasks to the InvokeAsync Method in the Class which you defined
              implementing the IMiddleware interface
              The only change is we will not have to register the middleware in 
              the ConfigureServices method of the StartUp.
              
              # services.AddTransient<FactoryActivatedMiddleware>();

              # The Class Constructor takes a RequestDelegate as a parameter
                which would help to invoke the next middleware in the pipeline.
              # The InvokeAsync takes a parameter HttpContext on which it 
                writes the Response.
                
                Benefits:
                # Activation per client request (injection of scoped services)
                # Strong typing of middleware
            */


            app.UseFactoryActivatedMiddleware(true);
            #endregion

            #region End of PipeLine
            /*This is the conventional inline way of making a middleware work.
            In this approach we use the app.Use method which internally delegates 
            To async (context, next) in which we have written the code to write 
            async to the Response
          */
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!  \n");
            });
            #endregion



            #endregion

            #region Learn Routing 
            app.Map("/foo", (appbuilder) =>
            {
                appbuilder.Run(async (context) =>
                {
                    await context.Response.WriteAsync("at foo now");
                });
            });

            var routeHandler = new RouteHandler(context =>
            {
                var routeValues = context.GetRouteData().Values;
                return context.Response.WriteAsync($"Hello! Route values: {string.Join(", ", routeValues)}");
            });
            var routeBuilder = new RouteBuilder(app, routeHandler);

            routeBuilder.MapRoute("Track Package Route", "package/{name}");

            routeBuilder.MapGet("test/{name}", (context) =>
            {
                var name = context.GetRouteValue("name");
                return context.Response.WriteAsync($"Hi, {name}!");
            });
            var routes = routeBuilder.Build();
            app.UseRouter(routes);

            #endregion

            var options = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4096
            };
            app.UseWebSockets(options);

        }
    }
}
