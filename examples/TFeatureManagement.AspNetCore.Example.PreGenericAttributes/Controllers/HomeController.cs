﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using TFeatureManagement.AspNetCore.Example.Models;
using TFeatureManagement.AspNetCore.Example.Mvc.ActionConstraints;
using TFeatureManagement.AspNetCore.Example.Mvc.Filters;

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