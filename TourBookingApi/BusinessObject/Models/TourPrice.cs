using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TourPrice
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }

        public virtual Tour Tour { get; set; } = null!;
    }
}
