using TravelExpertsData;

namespace TravelExpertsMVC.Models
{
    public class BookingViewModel
    {
        public string BookingDate { get; set; }
        public string Identifier { get; set; }
        public string Travellers { get; set; }
        public string TripType { get; set; }
        public string TotalPrice { get; set; }
        public string ID { get; set; }
        public bool Packaged { get; set; }

        public string PackageName { get; set; }

        public List<BookingViewItem> Items { get; set; }

        public BookingViewModel(Booking Booking)
        {
            this.BookingDate    = Booking.BookingDate?.ToString("yyyy-MM-dd") ?? "";
            this.Identifier     = Booking.BookingNo ?? "";
            this.Travellers     = Booking.TravelerCount?.ToString() ?? "1";
            this.TripType       = Booking.TripType?.Ttname ?? "";
            this.ID             = Booking.BookingId.ToString();

            this.Items = new();

            decimal TotalPrice  = 0;



            foreach (var detail in Booking.BookingDetails)
            {
                decimal ItemPrice = (detail.BasePrice ?? 0)
                                  + (detail.Fee?.FeeAmt ?? 0)
                                  + (detail.AgencyCommission ?? 0);

                TotalPrice += ItemPrice;

                this.Items.Add(new()
                {
                    Price       = ItemPrice.ToString("C0"),
                    Description = detail.Description ?? "",
                    Destination = detail.Destination ?? "",
                    Class       = detail.Class?.ClassName,
                    Product     = detail.ProductSupplier?.Product?.ProdName ?? "",
                    Supplier    = detail.ProductSupplier?.Supplier?.SupName ?? "",
                    ID          = detail.BookingDetailId.ToString(),
                });
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
        public string Destination { get; set; } = null!;
        public string? Class { get; set; } = null!;
        public string Product { get; set; } = null!;
        public string Supplier { get; set; } = null!;
        public string ID { get; set; } = null!;
    }
}
