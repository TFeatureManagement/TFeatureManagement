using FeatureManagement.AspNetCore.Example.ActionConstraints;
using FeatureManagement.AspNetCore.Example.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using System.Diagnostics;

namespace FeatureManagement.AspNetCore.Example.Controllers
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