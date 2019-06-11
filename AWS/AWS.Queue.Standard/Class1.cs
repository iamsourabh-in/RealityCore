using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net;
using System.Threading.Tasks;

namespace AWS.Queue.Standard
{
    public class StandardQueueService
    {
        private AmazonSQSClient sqsClient;

        public StandardQueueService()
        {
            AmazonSQSConfig sqsConfig = new AmazonSQSConfig();
            sqsClient = new AmazonSQSClient(sqsConfig);
        }

        public async Task<HttpStatusCode> SendMessageAsync(string messageBody, string queueURL)
        {
            SendMessageRequest sqsRequest = new SendMessageRequest();
            sqsRequest.QueueUrl = "https://sqs.us-east-1.amazonaws.com/272278775219/s3StandardQueue";

            sqsRequest.MessageBody = messageBody;

            SendMessageResponse sqsResponse = await sqsClient.SendMessageAsync(sqsRequest);

            return sqsResponse.HttpStatusCode;
        }
    }
}
