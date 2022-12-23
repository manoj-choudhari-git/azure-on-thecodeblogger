using System;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]
namespace FunctionApp
{
    class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            string cs = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.User);
            builder.ConfigurationBuilder.AddAzureAppConfiguration(cs);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            
        }
    }
}
