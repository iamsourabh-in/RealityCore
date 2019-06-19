using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using AWS.Cognito.Core;
using Reality.Cognito.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Reality.AdminConfirmUserWithTempPassword
{
    public class Function : CognitoLambdaBase
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<AdminConfirmUserWithTempPasswordResponse> FunctionHandler(AdminConfirmUserWithTempPasswordRequest request, ILambdaContext context)
        {
            LambdaLogger.Log(request.ToString<AdminCreateUserRequest>());
            try
            {
                if (request.IsRequestValid())
                {
                    CognitoHelper helper = new CognitoHelper();
                    var response = await helper.AdminConfirmUserWithNewPassword(request);
                    return new AdminConfirmUserWithTempPasswordResponse() { StatusCode = 200, StatusMessage = "success", Payload = response };
                }
                throw new Exception("Invalid Request");
            }
            catch (Exception ex)
            {
                return new AdminConfirmUserWithTempPasswordResponse() { StatusCode = 400, StatusMessage = "error", Payload = ex.Message };
            }
        }
    }
}