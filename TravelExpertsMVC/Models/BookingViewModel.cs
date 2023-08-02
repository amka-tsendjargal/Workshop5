using TravelExpertsData;

namespace TravelExpertsMVC.Models
{
    public class BookingViewModel
    {
        public string BookingDate { get; set; }         // The date the booking was made
        public string Travellers { get; set; }          // number of travellers
        public string TripType { get; set; }            // the trip type (business / leisure / group)
        public string TotalPrice { get; set; }          // the total price (either package price or sum of product prices)
        public string ID { get; set; }                  // booking id
        public bool Packaged { get; set; }              // whether the booking is a package

        public string PackageName { get; set; }         // the name of the package (if any)
        public string? Destination { get; set; }        // the booking destination

        public List<BookingViewItem> Items { get; set; }    // a list of products that are a part of the booking

        public BookingViewModel(Booking Booking)
        {
            this.BookingDate    = Booking.BookingDate?.ToString("yyyy-MM-dd") ?? "";

            this.Travellers = "1 Traveller";            // assume 1 traveller by default
            if (Booking.TravelerCount != null)
            {
                if (Booking.TravelerCount > 1)
                {                                       // pluralize for multiple travellers
                    this.Travellers = $"{Booking.TravelerCount} Travellers";
                }
            }
                    
            this.TripType       = Booking.TripType?.Ttname ?? "";
            this.ID             = Booking.BookingId.ToString();

            this.Items = new();

            decimal TotalPrice  = 0;



            foreach (var detail in Booking.BookingDetails)
            {
                decimal ItemPrice = (detail.BasePrice ?? 0) // price of this item, includign all fees and commission
                                  + (detail.Fee?.FeeAmt ?? 0)
                                  + (detail.AgencyCommission ?? 0);

                TotalPrice += ItemPrice;                    // add price to total

                this.Items.Add(new()
                {
                    Price       = ItemPrice.ToString("C0"),
                    Description = detail.Description ?? "",
                    Class       = detail.Class?.ClassName,
                    Product     = detail.ProductSupplier?.Product?.ProdName ?? "",
                    Supplier    = detail.ProductSupplier?.Supplier?.SupName ?? "",
                    ID          = detail.BookingDetailId.ToString(),
                });

                if (this.Destination == null && !string.IsNullOrWhiteSpace(detail.Destination))
                {
                    this.Destination = detail.Destination;
                }
            }

            if (Booking.Package != null)
            {
                this.Packaged = true;
                this.PackageName = Booking.Package.PkgName;
                TotalPrice = Booking.Package.PkgBasePrice
                           + (Booking.Package.PkgAgencyCommission ?? 0);
            }
            else
            {
                this.Packaged = false;
                this.PackageName = "";
            }

            this.TotalPrice = TotalPrice.ToString("C0");
        }
    }
    public class BookingViewItem
    {
        public string Price { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Class { get; set; } = null!;
        public string Product { get; set; } = null!;
        public string Supplier { get; set; } = null!;
        public string ID { get; set; } = null!;
    }
}
