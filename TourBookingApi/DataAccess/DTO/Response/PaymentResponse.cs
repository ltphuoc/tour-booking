using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class PaymentResponse
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public int? Status { get; set; }
        public string PaymentCode { get; set; }
        public string PaymentImage { get; set; }

    }
}
