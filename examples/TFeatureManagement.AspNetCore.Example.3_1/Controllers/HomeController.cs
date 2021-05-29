using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System.Diagnostics;
using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Example.ActionConstraints;
using TFeatureManagement.AspNetCore.Example.Models;

namespace TFeatureManagement.AspNetCore.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFeatureManager<Feature> _featureManager;

        public HomeController(ILogger<HomeController> logger, IFeatureManager<Feature> featureManager)
        {
            _logger = logger;
            _featureManager = featureManager;
        }

        public async Task<IActionResult> Index()
        {
            if (await _featureManager.IsEnabledAsync(Feature.Example1))
            {
            }

            return View();
        }

        [FeatureActionConstraint(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionConstraint(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}