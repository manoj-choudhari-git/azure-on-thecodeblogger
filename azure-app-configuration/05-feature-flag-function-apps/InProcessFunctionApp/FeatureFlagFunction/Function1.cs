using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;

using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FeatureFlagFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;
        private readonly IConfigurationRefresher _configurationRefresher;

        public Function1(
            ILogger<Function1> log, 
            IFeatureManagerSnapshot featureManagerSnapshot, 
            IConfigurationRefresherProvider refresherProvider)
        {
            _featureManagerSnapshot = featureManagerSnapshot;
            _configurationRefresher = refresherProvider.Refreshers.First();
            _logger = log;
        }

        [FunctionName("GetStorageData")]
        [OpenApiOperation(operationId: "GetStorageData", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetStorageData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("START: GetStorageData");
            await _configurationRefresher.TryRefreshAsync();
            var isFlagEnabled = await _featureManagerSnapshot.IsEnabledAsync("StorageFeature");
            var response = isFlagEnabled 
                ? "Storage Feature Flag is Enabled"
                : "Storage Feature Flag is Disabled";

            _logger.LogInformation("END: GetStorageData");
            return new OkObjectResult(response);
        }
    }
}

