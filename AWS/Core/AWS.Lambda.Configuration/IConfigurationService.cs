using Microsoft.Extensions.Configuration;

namespace AWS.Lambda.Configuration
{
    public interface IConfigurationService
    {
        IConfiguration GetConfiguration();
    }
}
