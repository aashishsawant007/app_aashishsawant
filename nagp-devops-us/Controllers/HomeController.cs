using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using nagp_devops_us.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace nagp_devops_us.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            InitializeViewBagValues();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public bool TestFunction(bool val)
        {
           //Added for coverage
           Console.WriteLine("coverage");
            return val;
        }

        private void InitializeViewBagValues()
        {
            // Configurations
            ViewBag.AppConfigUrl = Environment.GetEnvironmentVariable("appConfigUrl");
            ViewBag.AppConfigEnvironment = Environment.GetEnvironmentVariable("appConfigEnvironment");

            // Secerts
            ViewBag.Key = Environment.GetEnvironmentVariable("key");
            ViewBag.Vault = Environment.GetEnvironmentVariable("vault");
        }
    }
}
