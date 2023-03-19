using DataAccess.DTO.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class BookingResponse
    {
      
        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; } = new DateTime();
        public int NumAdults { get; set; } = 0!;

        public int NumChildren { get; set; }

        public int NumInfants { get; set; } = 0!;

        public decimal[,] TotalPrice { get; set; } = new decimal[10, 2];

        public TourRequest Tours { get; set; }
        public AccountRequest Customers { get; set; }

        public List<PaymentRequest> Payments { get; set; }
    }
}
