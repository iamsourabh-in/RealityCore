using System.Net;
using System.Threading.Tasks;

namespace AWS.Queue.Fifo
{
    public interface IFifoQueueService
    {
        Task<HttpStatusCode> SendMessageAsync(string messageBody, string queueURL, string messageGroupId = "default");
    }
}