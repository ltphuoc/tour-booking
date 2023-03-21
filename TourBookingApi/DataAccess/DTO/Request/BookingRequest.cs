using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class BookingRequest
    {

        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; } = new DateTime();
        public int NumAdults { get; set; } = 0!;
        public int NumInfants { get; set; } = 0!;
        public decimal[,] TotalPrice { get; set; } = new decimal[10, 2];
        //public List<PaymentRequest> Payments { get; set; }
    }

    public class BookingUpdateRequest
    {
        public DateTime BookingDate { get; set; } = new DateTime();
        public int NumAdults { get; set; } = 0!;
        public int NumInfants { get; set; } = 0!;
        public decimal[,] TotalPrice { get; set; } = new decimal[10, 2];
        /*public TourRequest Tours { get; set; }
        public AccountRequest Customers { get; set; }
        public List<PaymentRequest> Payments { get; set; }*/
    }

    public class BookingCreateRequest
    {
        public DateTime BookingDate { get; set; } = new DateTime();
        public int NumAdults { get; set; } = 0!;
        public int NumInfants { get; set; } = 0!;
        public decimal[,] TotalPrice { get; set; } = new decimal[10, 2];
        /*public TourRequest Tours { get; set; }
        public AccountRequest Customers { get; set; }
        public List<PaymentRequest> Payments { get; set; }*/
    }
}
