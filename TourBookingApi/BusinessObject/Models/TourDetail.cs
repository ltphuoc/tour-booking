using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TourDetail
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Departure { get; set; } = null!;
        public int DestinationId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int TransportationId { get; set; }
        public string TourDescription { get; set; } = null!;

        public virtual Destination Destination { get; set; } = null!;
        public virtual Tour Tour { get; set; } = null!;
        public virtual Transportation Transportation { get; set; } = null!;
    }
}
