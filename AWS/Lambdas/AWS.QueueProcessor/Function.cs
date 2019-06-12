using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using AWS.EmailService;
using AWS.Lambda.CoreServices;
using AWS.Lambda.DI;
using AWS.Queue.Standard;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWS.QueueProcessor
{
    public class Function
    {
        private readonly ILogService _loggingService;

        private readonly IStandardQueueService _standardQueue;

        private readonly IAmazonSES _amazonSES;

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            DependencyResolver resolver = new DependencyResolver(ConfigureServices);
            _loggingService = resolver.ServiceProvider.GetRequiredService<ILogService>();
            _standardQueue = resolver.ServiceProvider.GetRequiredService<IStandardQueueService>();
            _amazonSES = resolver.ServiceProvider.GetRequiredService<IAmazonSES>();
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("<html>");
                builder.Append("<head></head>");
                builder.Append("<body>");
                builder.Append($"<p> This email was sent from SES when {message.Body} was uploaded on S3</p>");
                builder.Append("</body>");
                builder.Append("</html>");

                context.Logger.LogLine($"Processed message {builder.ToString()}");

                await _amazonSES.SendEmailAsync("S3 Upload Details", builder.ToString(), message.Body);
            }
            catch (Exception ex)
            {
                context.Logger.Log(ex.Message);
            }

            // TODO: Do interesting work based on the new message
            await Task.CompletedTask;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IStandardQueueService, StandardQueueService>();
            services.AddTransient<IAmazonSES, AmazonSES>();
        }
    }
}
