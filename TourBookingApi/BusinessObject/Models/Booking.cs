using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual Account Customer { get; set; } = null!;
        public virtual Tour Tour { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
