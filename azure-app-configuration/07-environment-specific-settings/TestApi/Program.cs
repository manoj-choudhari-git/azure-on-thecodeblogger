
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

using TestApi.Configurations;

namespace TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

            // Retrieve the connection string from environment variable
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString")!;

            // Load configuration from Azure App Configuration
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options
                    .Connect(connectionString)
                    // Load configuration values with no label
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    // Override with any configuration values specific to current hosting env
                    .Select(KeyFilter.Any, builder.Environment.EnvironmentName);
            });

            // Bind the configuration settings under "TestApi:" namespace
            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("TestApi"));

            // Other settings in Program.cs

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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