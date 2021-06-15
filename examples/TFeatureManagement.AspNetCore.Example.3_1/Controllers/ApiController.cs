using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using TFeatureManagement.AspNetCore.Example.ActionConstraints;
using TFeatureManagement.AspNetCore.Example.Filters;
using TFeatureManagement.AspNetCore.Example.Models;

namespace TFeatureManagement.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class ApiController : ControllerBase
    {
        [HttpGet("featurefiltered")]
        [FeatureActionFilter(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionFilter(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult FeatureFiltered()
        {
            return new OkObjectResult("Only visible if features are enabled.");
        }

        [HttpGet("featureconstrained")]
        [FeatureActionConstraint(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionConstraint(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult FeatureConstrained()
        {
            return new OkObjectResult("Only visible if features are enabled.");
        }

        [HttpGet("featureconstrained")]
        public IActionResult FeatureConstrainedFallback()
        {
            return new OkObjectResult("Only visible if FeatureConstrained action is not enabled.");
        }
    }
}