using Amazon.Lambda.Core;
using AWS.Helper.Core;

namespace AWS.Lambda.CoreServices
{
    public class LogService : ILogService
    {
        public void LogInfo<T>(T t)
        {
            LambdaLogger.Log(t.ToJsonString());
        }

        public void LogInfo(string info)
        {
            LambdaLogger.Log(info);
        }

        public void LogError<T>(T t)
        {
            LambdaLogger.Log(t.ToJsonString());
        }
        public void LogError(string errorMessage)
        {
            LambdaLogger.Log(errorMessage);
        }
    }
}
