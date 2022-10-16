using Microsoft.AspNetCore.Mvc;
using TFeatureManagement.AspNetCore.Example.Models;
using TFeatureManagement.AspNetCore.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Example.Controllers
{
    [ApiController]
    [Route("attributerouting/")]
    public class AttributeRoutingController : ControllerBase
    {
        [HttpGet("featureconstrained", Order = -1)]
        [FeatureActionConstraint<Feature>(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionConstraint<Feature>(RequirementType.Any, Feature.Example3, Feature.Example4)]
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
        [FeatureActionFilter<Feature>(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionFilter<Feature>(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult FeatureFiltered()
        {
            return new OkObjectResult("Only visible if features are enabled.");
        }
    }
}