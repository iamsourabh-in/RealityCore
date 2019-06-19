
using Amazon.Lambda.Core;
using AWS.Cognito.Core.Models;
using System.Net;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Reality.SignUp
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> FunctionHandler(ApiSignUpRequest request, ILambdaContext context)
        {
            
            return HttpStatusCode.OK;
        }
    }
}
