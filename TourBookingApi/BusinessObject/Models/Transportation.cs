using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Transportation
    {
        public Transportation()
        {
            TourDetails = new HashSet<TourDetail>();
        }

        public int Id { get; set; }
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;

        public virtual ICollection<TourDetail> TourDetails { get; set; }
    }
}
