using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FeatureFlagIsolatedFunctionsApp
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

        [Function("GetStorageData")]
        public async Task<HttpResponseData> GetStorageData([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {

            _logger.LogInformation($"START: GetStorageData");
            await _configurationRefresher.TryRefreshAsync();
            var isFlagEnabled = await _featureManagerSnapshot.IsEnabledAsync("StorageFeature");
            var responseMessage = isFlagEnabled
                ? "Storage Feature Flag is Enabled"
                : "Storage Feature Flag is Disabled";


            // Prepare and Return Response
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(responseMessage);

            _logger.LogInformation("END: GetStorageData");
            return response;
        }
    }
}
