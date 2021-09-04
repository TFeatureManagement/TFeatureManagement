using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using TFeatureManagement.AspNetCore.Example.Models;
using TFeatureManagement.AspNetCore.Example.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Example.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("attributerouting/")]
    public class AttributeRoutingController : ControllerBase
    {
        [HttpGet("featureconstrained", Order = -1)]
        [FeatureActionConstraint(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionConstraint(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult FeatureConstrained()
        {
            return new OkObjectResult($"ActionName: {nameof(FeatureConstrained)} - Only visible if features are enabled.");
        }

        [HttpGet("featureconstrained")]
        public IActionResult FeatureConstrainedFallback()
        {
            return new OkObjectResult($"ActionName: {nameof(FeatureConstrainedFallback)} - Only visible if {nameof(FeatureConstrained)} action is not enabled.");
        }

        [HttpGet("featurefiltered")]
        [FeatureActionFilter(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionFilter(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult FeatureFiltered()
        {
            return new OkObjectResult("Only visible if features are enabled.");
        }
    }
}