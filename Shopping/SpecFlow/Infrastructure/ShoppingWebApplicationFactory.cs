using App.Metrics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shopping.DataAccess;

namespace Shopping.SpecFlow.Infrastructure
{
    public sealed class ShoppingWebApplicationFactory
         : WebApplicationFactory<Program>, IDisposable
    {
        public const string BaseAddress = "http://localhost";

        private readonly DbContextOptionsBuilder<ShoppingDbContext> _builder;

        private bool _disposed = false;

        public Dictionary<string, string> RequestParameters { get; set; } = new Dictionary<string, string>();

        public ShoppingWebApplicationFactory()
        {
            _builder = new DbContextOptionsBuilder<ShoppingDbContext>();
            _builder.UseInMemoryDatabase(Global.DataBaseName);
        }

        public HttpClient GetClient() => CreateDefaultClient(new Uri(BaseAddress));

        public ShoppingDbContext GetContext() => new ShoppingDbContext(_builder.Options);

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(serviceCollection => ReplaceContext(serviceCollection));
            builder.ConfigureAppConfiguration(configBuilder => ModifyAppSettigns(configBuilder));
            builder.ConfigureServices(collection => collection.AddSingleton(NSubstitute.Substitute.For<IMetrics>()));
            return base.CreateHost(builder);
        }

        private void ModifyAppSettigns(IConfigurationBuilder configBuilder)
        {
            var configsToRemove = configBuilder.Sources.Where(src => src is JsonConfigurationSource || src is EnvironmentVariablesConfigurationSource).ToArray();
            foreach (var conf in configsToRemove)
            {
                configBuilder.Sources.Remove(conf);
            }
        }

        private void ReplaceContext(IServiceCollection serviceCollection)
        {
            var contextRegistrations = serviceCollection.Where(desc => desc.ImplementationType == typeof(ShoppingDbContext)).ToArray();
            foreach (var item in contextRegistrations)
            {
                serviceCollection.Remove(item);
                serviceCollection.Add(new ServiceDescriptor(item.ServiceType, provider => new ShoppingDbContext(_builder.Options), item.Lifetime));
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && !_disposed)
            {
                _disposed = true;
            }
        }
    }
}