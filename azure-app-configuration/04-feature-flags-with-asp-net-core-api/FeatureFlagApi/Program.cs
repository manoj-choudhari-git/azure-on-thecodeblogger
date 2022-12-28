
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;

namespace FeatureFlagApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Reading from user-secrets locally
            string connectionString = builder.Configuration.GetConnectionString("AppConfig")!;

            // Load configuration from Azure App Configuration
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                // Connect and Load all feature flags with no label
                options
                    .Connect(connectionString)          // To connect to App Configuration
                    .UseFeatureFlags(featureOptions =>  // To load feature flags
                    {   
                        // Flags are cached for 45 seconds
                        featureOptions.CacheExpirationInterval = new TimeSpan(0, 0, 45);

                        // Flags with label = "first" are loaded
                        featureOptions.Label = "first";
                    });
            });

            // To Register - Feature Management Dependencies
            builder.Services
                .AddSingleton<IConfiguration>(builder.Configuration)
                .AddFeatureManagement();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}