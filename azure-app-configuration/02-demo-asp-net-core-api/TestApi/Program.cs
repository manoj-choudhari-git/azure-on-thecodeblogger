
using TestApi.Configurations;

namespace TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Retrieve the connection string
            string connectionString = builder.Configuration.GetConnectionString("AppConfig")!;

            // Load configuration from Azure App Configuration
            builder.Configuration.AddAzureAppConfiguration(connectionString);

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