using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Destination
    {
        public Destination()
        {
            DestinationImages = new HashSet<DestinationImage>();
            TourDetails = new HashSet<TourDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Status { get; set; }

        public virtual ICollection<DestinationImage> DestinationImages { get; set; }
        public virtual ICollection<TourDetail> TourDetails { get; set; }
    }
}
