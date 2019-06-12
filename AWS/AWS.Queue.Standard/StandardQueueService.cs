using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net;
using System.Threading.Tasks;

namespace AWS.Queue.Standard
{
    public class StandardQueueService : IStandardQueueService
    {
        private AmazonSQSClient sqsClient;
        private string QueueBaseURL = "https://sqs.us-east-1.amazonaws.com/";
        private string Account_Name = "272278775219";

        public StandardQueueService()
        {
            AmazonSQSConfig sqsConfig = new AmazonSQSConfig();
            sqsClient = new AmazonSQSClient(sqsConfig);
        }

        public async Task<HttpStatusCode> SendMessageAsync(string messageBody, string queueName = "s3StandardQueue", string messageGroupId = "default")
        {
            SendMessageRequest sqsRequest = new SendMessageRequest();
            sqsRequest.QueueUrl = QueueBaseURL + Account_Name + "/" + queueName;

            sqsRequest.MessageBody = messageBody;

            SendMessageResponse sqsResponse = await sqsClient.SendMessageAsync(sqsRequest);

            return sqsResponse.HttpStatusCode;
        }
    }
}
