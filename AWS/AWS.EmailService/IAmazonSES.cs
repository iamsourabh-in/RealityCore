using System.Threading.Tasks;

namespace AWS.EmailService
{
    public interface IAmazonSES
    {
        Task SendEmailAsync(string subject, string htmlBody, string textBody);
    }
}
