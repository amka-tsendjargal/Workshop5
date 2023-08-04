using Microsoft.AspNetCore.Mvc;
using System.Diagnostics; 
using TravelExpertsMVC.Models; 

namespace TravelExpertsMVC.Controllers
{
    // Define the HomeController class, which serves as a controller for handling HTTP requests.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Private field to hold the logger for logging information.

        // Constructor for the HomeController class that takes a logger as a parameter via dependency injection.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger; // Assign the logger provided by the dependency injection to the _logger field.
        }

        // Action method for handling the default route (Home/Index).
        // This method returns a ViewResult representing the Index view.
        public IActionResult Index()
        {
            return View();
        }

        // Action method for handling the Privacy page request.
        // This method returns a ViewResult representing the Privacy view.
        public IActionResult Privacy()
        {
            return View();
        }

        // Action method for handling the Error page request.
        // This method returns a ViewResult representing the Error view, along with an ErrorViewModel containing the error details.
        // It uses ResponseCache attribute to indicate that the response should not be cached (Duration = 0).
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Create an ErrorViewModel with the RequestId to display error details in the view.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
