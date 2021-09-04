using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TFeatureManagement.AspNetCore.Example.Models;
using TFeatureManagement.AspNetCore.Example.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Example.Mvc.Filters;

namespace TFeatureManagement.AspNetCore.Example.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [FeatureActionConstraint(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionConstraint(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult FeatureConstrained()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FeatureConstrainedFallback()
        {
            return View();
        }

        [FeatureActionFilter(RequirementType.Any, Feature.Example1, Feature.Example2)]
        [FeatureActionFilter(RequirementType.Any, Feature.Example3, Feature.Example4)]
        public IActionResult FeatureFiltered()
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