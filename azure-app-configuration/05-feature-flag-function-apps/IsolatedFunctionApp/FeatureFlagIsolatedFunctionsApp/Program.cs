using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        // Read connection string from environment variable
        string connectionString = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.User)!;

        // Load Azure App Configurations
        builder.AddAzureAppConfiguration(options =>
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

    })
    .ConfigureServices(configureDelegate =>
    {
        configureDelegate.AddAzureAppConfiguration();
        configureDelegate.AddFeatureManagement();
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
