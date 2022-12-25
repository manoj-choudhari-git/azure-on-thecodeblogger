using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Net;

namespace IsolatedFunctionApp
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public Function1(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _configuration = configuration;
        }

        [Function("GetEndpoint")]
        public HttpResponseData GetEndpoint([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            
            _logger.LogInformation($"START: GetEndpoint");
            var configKey = "TestApi:GetEndpoint:Message";
            var response = ProcessRequest(req, configKey);
            _logger.LogInformation($"END: GetEndpoint");
            
            return response;
        }

        [Function("PostEndpoint")]
        public HttpResponseData PostEndpoint([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation($"START: PostEndpoint");
            var configKey = "TestApi:PostEndpoint:Message";
            var response = ProcessRequest(req, configKey);
            _logger.LogInformation($"END: PostEndpoint");

            return response;
        }

        private HttpResponseData ProcessRequest(HttpRequestData req, string configKey)
        {
            // Read Query String
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var name = queryDictionary["name"];

            // Read configuration data
            string configuredValue = _configuration[configKey];

            // Prepare the result
            string result = $"Please create a key-value with the key '{configKey}' in Azure App Configuration.";
            if (configuredValue != null)
            {
                result = $"{configuredValue} - {name}";
            }

            // Prepare and Return Response
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(result);

            return response;
        }
    }
}
