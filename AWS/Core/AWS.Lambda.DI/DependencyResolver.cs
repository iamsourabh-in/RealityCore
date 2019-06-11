

using AWS.Lambda.Configuration;
using AWS.Lambda.Environment;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AWS.Lambda.DI
{
    public class DependencyResolver
    {
        public IServiceProvider ServiceProvider { get; }
        public string CurrentDirectory { get; set; }
        public Action<IServiceCollection> RegisterServices { get; }

        public DependencyResolver(Action<IServiceCollection> registerServices = null)
        {
            // Set up Dependency Injection
            var serviceCollection = new ServiceCollection();
            RegisterServices = registerServices;
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register env and config services
            services.AddTransient<IEnvironmentService, EnvironmentService>();

            services.AddTransient<IConfigurationService, ConfigurationService>

            (provider => new ConfigurationService(provider.GetService<IEnvironmentService>())
            {
                CurrentDirectory = CurrentDirectory
            });

            // Register other services
            RegisterServices?.Invoke(services);
        }
    }
}

