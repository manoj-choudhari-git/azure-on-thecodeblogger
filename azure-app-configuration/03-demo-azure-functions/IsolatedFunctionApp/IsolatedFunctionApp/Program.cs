using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        string cs = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.User)!;
        builder.AddAzureAppConfiguration(cs);
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
