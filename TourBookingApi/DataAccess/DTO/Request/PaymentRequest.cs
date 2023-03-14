using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class PaymentRequest
    {
        public string PaymentMethod { get; set; } = null!;
        public DateTime? PaymentDate { get; set; } = null;
        public decimal[,] PaymentAccount { get; set; } = new decimal[10, 2];
        public List<BookingRequest> Bookings { get; set; }
    }

    public class PaymentUpdateRequest
    {
        public string PaymentMethod { get; set; } = null!;
        public DateTime? PaymentDate { get; set; } = null;
        public decimal[,] PaymentAccount { get; set; } = new decimal[10, 2];
        public List<BookingRequest> Bookings { get; set; }
    }

    public class PaymentCreateRequest
    {
        public string PaymentMethod { get; set; } = null!;
        public DateTime? PaymentDate { get; set; } = null;
        public decimal[,] PaymentAccount { get; set; } = new decimal[10, 2];
        public List<BookingRequest> Bookings { get; set; }
    }
}
