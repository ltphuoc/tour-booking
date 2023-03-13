using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class TourRequest
    {
        public string TourName { get; set; } = null!;
        public int TourDuration { get; set; } = 0!;
        public int TourCapacity { get; set; } = 0!;
        public string Status { get; set; } = null!;
        public List<TourGuideRequest> TourGuides { get; set; }
        public List<BookingRequest> Bookings { get; set; }
        public List<TourDetailRequest> TourDetails { get; set; }
        public List<TourPriceRequest> TourPrices { get; set; }

    }

    public class TourUpdateRequest
    {
        public string TourName { get; set; } = null!;
        public int TourDuration { get; set; } = 0!;
        public int TourCapacity { get; set; } = 0!;
        public string Status { get; set; } = null!;
        public List<TourGuideRequest> TourGuides { get; set; }
        public List<BookingRequest> Bookings { get; set; }
        public List<TourDetailRequest> TourDetails { get; set; }
        public List<TourPriceRequest> TourPrices { get; set; }
    }

    public class TourCreateRequest
    {
        public string TourName { get; set; } = null!;
        public int TourDuration { get; set; } = 0!;
        public int TourCapacity { get; set; } = 0!;
        public string Status { get; set; } = null!;
    }
}
