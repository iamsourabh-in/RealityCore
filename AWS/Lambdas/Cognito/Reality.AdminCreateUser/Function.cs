using Amazon.Lambda.Core;
using AWS.Cognito.Core;
using AWS.Lambda.CoreServices;
using AWS.Lambda.DI;
using Microsoft.Extensions.DependencyInjection;
using Reality.Cognito.Models;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Reality.AdminCreateUser
{
    public class Function : CognitoLambdaBase
    {
        private readonly ILogService _loggingService;
        private readonly ICognitoService _cognitoService;

        public Function()
        {
            DependencyResolver resolver = new DependencyResolver(ConfigureServices);
            _loggingService = resolver.ServiceProvider.GetRequiredService<ILogService>();
            _cognitoService = resolver.ServiceProvider.GetRequiredService<ICognitoService>();
        }

        /// <summary>
        /// Lambda to create user in coognito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<AdminCreateUserResponse> FunctionHandler(AdminCreateUserRequest request, ILambdaContext context)
        {
            LambdaLogger.Log(request.ToString<AdminCreateUserRequest>());
            try
            {
                if (request.IsRequestValid())
                {
                    var response = await _cognitoService.AdminCreateUserAsync(request);
                    return new AdminCreateUserResponse() { StatusCode = 200, StatusMessage = "success", Payload = response };
                }
                throw new Exception("Invalid Request");
            }
            catch (Exception ex)
            {
                return new AdminCreateUserResponse() { StatusCode = 400, StatusMessage = "error", Payload = ex.Message };
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<ICognitoService, CognitoService>();
        }
    }
}
