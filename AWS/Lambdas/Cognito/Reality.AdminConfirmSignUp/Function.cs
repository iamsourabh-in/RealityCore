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

namespace Reality.AdminConfirmSignUp
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

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<ICognitoService, CognitoService>();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<AdminConfirmSignUpResponse> FunctionHandler(AdminConfirmSignUpRequest request, ILambdaContext context)
        {

            LambdaLogger.Log(request.ToString<AdminConfirmSignUpRequest>());

            try
            {
                if (request.IsRequestValid())
                {
                    var response = await _cognitoService.AdminConfirmSignUpAsync(request);
                    return new AdminConfirmSignUpResponse() { StatusCode = 200, StatusMessage = "success", Payload = response };
                }
                throw new Exception("Invalid Request");
            }
            catch (Exception ex)
            {
                return new AdminConfirmSignUpResponse() { StatusCode = 400, StatusMessage = "error", Payload = ex.Message };
            }
        }

        
    }
}
