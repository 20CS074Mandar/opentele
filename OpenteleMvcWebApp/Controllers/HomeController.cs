using Microsoft.AspNetCore.Mvc;
using OpenteleMvcWebApp.Models;
using System.Diagnostics;

namespace OpenteleMvcWebApp.Controllers
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
            using var activity = DiagnosticsConfig.activitysource.StartActivity("SayHello");
            activity?.SetTag("foo", 1);
            activity?.SetTag("bar", "Hello world");
            activity?.SetTag("Baz", new int[] { 1, 2, 3 });
            return View();
        }

        public IActionResult Privacy()
        {
            DiagnosticsConfig.requestCounter.Add(1,
                new("Action",nameof(Index)),
                new("Controller", nameof(HomeController)));
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}