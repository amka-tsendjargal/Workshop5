using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelExpertsData;
using TravelExpertsMVC.Models;

namespace TravelExpertsMVC.Controllers
{
    public class BookingController : Controller
    {
        private TravelExpertsContext _dbContext;

        public BookingController(TravelExpertsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult BookingHistory()
        {
            try
            {
                int customerID = (int)HttpContext.Session.GetInt32("CurrentCustomer")!;
                var Model = BookingManager.GetCustomerBookings(_dbContext, customerID);

                return View(Model);
            }
            catch
            {
                TempData["ErrorMessage"] = "There was an error accessing your account information.";
                return RedirectToAction("Logout", "Customer");
            }
        }
    }
}
