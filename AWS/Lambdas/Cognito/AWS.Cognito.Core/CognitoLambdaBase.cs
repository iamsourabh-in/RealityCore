using System;

namespace AWS.Cognito.Core
{
    public class CognitoLambdaBase
    {

        public void InitailizeConfiguration(object request)
        {

        }
        public Exception ThrowCustomException(object context, CognitoActionType actionType, string description, string exMessage, string sourceIp = "")
        {
            var UnixCurrentTimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            // context.Logger.LogLine("Error Log Time: " + UnixCurrentTimeStamp + " - Username : " + username + ", ActionType: " + actionType + ",Error Message: " + description + ", Exception : " + exMessage + ", sourceIp : " + sourceIp);
            throw new Exception(description);
        }
    }
}
