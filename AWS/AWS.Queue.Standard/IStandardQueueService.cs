using System.Net;
using System.Threading.Tasks;

namespace AWS.Queue.Standard
{
    public interface IStandardQueueService
    {
        Task<HttpStatusCode> SendMessageAsync(string messageBody, string queueName = "s3StandardQueue", string messageGroupId = "default");
    }
}