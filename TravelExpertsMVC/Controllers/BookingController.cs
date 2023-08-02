using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelExpertsData;
using TravelExpertsMVC.Models;

namespace TravelExpertsMVC.Controllers
{
    /// <summary>
    /// This class is a controller for pages displaying Bookings
    /// </summary>
    public class BookingController : Controller
    {
        // The TravelExperts database context
        private TravelExpertsContext _dbContext;

        // Constructor for dependency injection
        public BookingController(TravelExpertsContext dbContext)
        {
            _dbContext = dbContext;
        }

        // The BookingHistory page, only valid for logged-in users
        // this page displays a list of the past bookings that a customer has made
        [Authorize]
        public IActionResult BookingHistory()
        {
            try
            {
                // Get customer ID from session
                int? customerID = HttpContext.Session.GetInt32("CurrentCustomer");

                if (customerID == null)
                {
                    TempData["ErrorMessage"] = "Your session has expired.";
                    return RedirectToAction("Logout", "Customer");
                }

                // Model is a BookingViewModel (only relevant data from booking, pre-
                // formatted strings for displaying)
                var Model = BookingManager.GetCustomerBookings(_dbContext, (int)customerID);

                return View(Model);
            }
            catch // database error
            {
                TempData["ErrorMessage"] = "There was an error accessing your booking history.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
