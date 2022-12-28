using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

using System;

[assembly: FunctionsStartup(typeof(FeatureFlagFunction.Startup))]

namespace FeatureFlagFunction
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            // Load connection string from environment variable
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString")!;

            // Load Azure App Configurations
            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString)   // To connect with App Config store
                       .Select("_")                 // To load nothing other than feature flags
                       .UseFeatureFlags(featureOptions =>
                       {
                           // Modify default caching internval
                           featureOptions.CacheExpirationInterval = new TimeSpan(0, 0, 45);

                           // Load feature flags with label = "first"
                           featureOptions.Label = "first";
                       });
            });
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAzureAppConfiguration();
            builder.Services.AddFeatureManagement();
        }
    }

}
