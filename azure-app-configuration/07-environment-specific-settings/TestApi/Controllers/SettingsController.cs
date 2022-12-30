using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Cryptography.X509Certificates;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SettingsController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetSettings()
        {
            var logLevel = _configuration["TestApi:LogLevel"];
            var apiEndpointUrl = _configuration["TestApi:ApiEndpointUrl"];

            var result = $"logLevel={logLevel}, apiEndpointUrl={apiEndpointUrl}";
            return Ok(result);
        }
    }
}
