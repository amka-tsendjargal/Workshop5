using Microsoft.EntityFrameworkCore;
using TravelExpertsData;

namespace TravelExpertsMVC.Models
{
    public static class BookingManager
    {
        public static List<Booking> GetCustomerBookings(TravelExpertsContext DB, int CustomerID)
            /*
             * This seems kindof janky, but it's literally the example Microsoft gives for nested
             * includes...
             * https://learn.microsoft.com/en-ca/ef/core/querying/related-data/eager
             */
            => DB
            .Bookings
            .Where(b => b.CustomerId == CustomerID)
            // Include Package
            .Include(b => b.Package)
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
            .ToList();

        public static List<ProductsSupplier> GetPackageProductSuppliers(TravelExpertsContext DB, int PackageID)
            => DB.PackagesProductsSuppliers
            .Where(pps => pps.PackageId == PackageID)
            .Include(pps => pps.ProductSupplier)
            .ThenInclude(ps => ps.Supplier)
            .Include(pps => pps.ProductSupplier)
            .ThenInclude(ps => ps.Product)
            .Select(pps => pps.ProductSupplier)
            .ToList();
    }
}
