using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace FeatureFlagApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureFlagController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;

        public FeatureFlagController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetSomeData()
        {
            if (await _featureManager.IsEnabledAsync("StorageFeature"))
            {
                return Ok("Storage Feature is used.");
            }
            else
            {
                return Ok("Storage Feature is not used.");
            }
        }
    }
}
