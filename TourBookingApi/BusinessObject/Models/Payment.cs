using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public int? Status { get; set; }
        public string? PaymentCode { get; set; }
        public string? PaymentImage { get; set; }

        public virtual Booking Booking { get; set; } = null!;
    }
}
