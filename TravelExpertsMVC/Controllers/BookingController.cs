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

        //[Authorize]
        public IActionResult BookingHistory()
        {
            var Model = BookingManager.GetCustomerBookings(_dbContext, 133);
            return View(Model);
        }
    }
}
