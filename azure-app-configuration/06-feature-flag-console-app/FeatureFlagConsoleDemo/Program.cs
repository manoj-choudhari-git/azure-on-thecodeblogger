using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

namespace FeatureFlagConsoleDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            IConfigurationRoot configuration = 
                new ConfigurationBuilder()
                .AddAzureAppConfiguration(options =>
                {
                    options.Connect(connectionString)   // To connect with App Config store
                      .Select("_")                      // To load nothing other than feature flags
                      .UseFeatureFlags(featureOptions =>
                      {
                          // Modify default caching internval
                          featureOptions.CacheExpirationInterval = new TimeSpan(0, 0, 45);

                          // Load feature flags with label = "first"
                          featureOptions.Label = "first";
                      });
                })
                .Build();

            // Initialize Feature Management
            IServiceCollection services = new ServiceCollection();
            services
                .AddSingleton<IConfiguration>(configuration)
                .AddFeatureManagement()
                .AddFeatureFilter<PercentageFilter>();


            // To resolve the dependency from DI container
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IFeatureManager featureManager = serviceProvider.GetRequiredService<IFeatureManager>();

                var flagEnabledCount = 0;
                for (int attempt = 1; attempt <= 20; attempt++)
                {

                    // If feature is enabled
                    if (await featureManager.IsEnabledAsync("StorageFeature"))
                    {
                        Console.WriteLine($"{attempt} : Storage Feature is Enabled.");
                        flagEnabledCount++;
                    }
                    else
                    {
                        Console.WriteLine($"{attempt} : Storage Feature is NOT Enabled.");
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Total Attempts = 20, Enabled was returned {flagEnabledCount} times.");
            }

        }
    }
}