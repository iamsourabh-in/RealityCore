using Amazon.SQS;
using Amazon.SQS.Model;
using System.Net;
using System.Threading.Tasks;

namespace AWS.Queue.Fifo
{
    public class FifoQueueService
    {
        private AmazonSQSClient sqsClient;

        public FifoQueueService()
        {
            AmazonSQSConfig sqsConfig = new AmazonSQSConfig();
            sqsClient = new AmazonSQSClient(sqsConfig);
        }

        public async Task<HttpStatusCode> SendMessageAsync(string messageBody, string queueURL, string messageGroupId = "default")
        {
            SendMessageRequest sqsRequest = new SendMessageRequest();
            sqsRequest.QueueUrl = "https://sqs.us-east-1.amazonaws.com/272278775219/ImageProcessingQueue.fifo";

            sqsRequest.MessageBody = messageBody;

            // Message Group Id is requires in FIfo to maintain first in first out for a particular group.
            sqsRequest.MessageGroupId = messageGroupId;   //"s3BucketEvent"

            SendMessageResponse sqsResponse = await sqsClient.SendMessageAsync(sqsRequest);

            return sqsResponse.HttpStatusCode;
        }
    }
}
