using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Tour
    {
        public Tour()
        {
            Bookings = new HashSet<Booking>();
            TourDetails = new HashSet<TourDetail>();
            TourGuides = new HashSet<TourGuide>();
            TourPrices = new HashSet<TourPrice>();
        }

        public int Id { get; set; }
        public string TourName { get; set; } = null!;
        public int TourDuration { get; set; }
        public int TourCapacity { get; set; }
        public string Status { get; set; } = null!;
        public int TourGuideId { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<TourDetail> TourDetails { get; set; }
        public virtual ICollection<TourGuide> TourGuides { get; set; }
        public virtual ICollection<TourPrice> TourPrices { get; set; }
    }
}
