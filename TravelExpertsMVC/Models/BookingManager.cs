using Microsoft.EntityFrameworkCore;
using TravelExpertsData;

namespace TravelExpertsMVC.Models
{
    public static class BookingManager
    {
        public static List<BookingViewModel> GetCustomerBookings(TravelExpertsContext DB, int CustomerID)
            /*
             * This seems kindof janky, but it's literally the example Microsoft gives for nested
             * includes...
             * https://learn.microsoft.com/en-ca/ef/core/querying/related-data/eager
             */
            => DB
            .Bookings
            .Where(b => b.CustomerId == CustomerID)
            // Include TripType
            .Include(b => b.TripType)
            // Include Class
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Class)
            // Include Fee
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Fee)
            // Include Product
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.ProductSupplier)
            .ThenInclude(ps => ps.Product)
            // Include Supplier
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.ProductSupplier)
            .ThenInclude(ps => ps.Supplier)
            .OrderByDescending(b => b.BookingDate)
            // Convert to model more suitable for view
            .Select(b => new BookingViewModel(b))
            .ToList();
    }
}
