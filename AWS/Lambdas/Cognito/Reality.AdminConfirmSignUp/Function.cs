using Amazon.Lambda.Core;
using AWS.Cognito.Core;
using Reality.Cognito.Models;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Reality.AdminConfirmSignUp
{
    public class Function : CognitoLambdaBase
    {

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
                    CognitoService helper = new CognitoService();
                    var response = await helper.AdminConfirmSignUpAsync(request);
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
