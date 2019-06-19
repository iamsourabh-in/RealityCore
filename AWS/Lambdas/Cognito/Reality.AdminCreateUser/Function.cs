using Amazon.Lambda.Core;
using AWS.Cognito.Core;
using Reality.Cognito.Models;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Reality.AdminCreateUser
{
    public class Function : CognitoLambdaBase
    {

        /// <summary>
        /// Lambda to create user in coognito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<AdminCreateUserResponse> FunctionHandler(AdminCreateUserRequest request, ILambdaContext context)
        {
            try
            {
                if (request.IsRequestValid())
                { 
                    CognitoHelper helper = new CognitoHelper();
                    var response = await helper.AdminCreateUserAsync(request);
                    return new AdminCreateUserResponse() { StatusCode = 200, StatusMessage = "success", Payload = response };
                }
                throw new Exception("Invalid Request");
            }
            catch (Exception ex)
            {
                return new AdminCreateUserResponse() { StatusCode = 400, StatusMessage = "error", Payload = ex.Message };
            }
        }
    }
}
