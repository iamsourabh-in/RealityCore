using AWS.Lambda.Environment;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AWS.Lambda.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        public IEnvironmentService EnvService { get; }
        public string CurrentDirectory { get; set; }

        private readonly string _environmentName;

        public ConfigurationService(IEnvironmentService envService)
        {
            EnvService = envService;
            _environmentName = envService.EnvironmentName;
        }

        public ConfigurationService(string environmentName)
        {
            _environmentName = environmentName;
        }

        public IConfiguration GetConfiguration()
        {
            CurrentDirectory = CurrentDirectory ?? Directory.GetCurrentDirectory();
            return new ConfigurationBuilder()
                .SetBasePath(CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
