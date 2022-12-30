using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using TestApi.Configurations;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingsController : ControllerBase
    {
        private readonly AppSettings appSettings;

        public GreetingsController(IOptionsSnapshot<AppSettings> appSettingsOptions)
        {
            this.appSettings = appSettingsOptions.Value;
        }

        [HttpGet]
        public IActionResult GetGreetings(string name)
        {
            var result = $"{appSettings.GetEndpoint.Message} - {name}";
            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostGreetings(string name)
        {
            var result = $"{appSettings.PostEndpoint.Message} - {name}";
            return Ok(result);
        }
    }
}
