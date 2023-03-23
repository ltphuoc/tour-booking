using BusinessObject.Models;
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
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
        //public List<PaymentRequest> Payments { get; set; }
    }

    public class BookingUpdateRequest
    {
        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
        /*public decimal TotalPrice { get; set; }*/
        /*public TourRequest Tours { get; set; }
        public AccountRequest Customers { get; set; }
        public List<PaymentRequest> Payments { get; set; }*/
    }

    public class BookingCreateRequest
    {
        public int TourId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; }
        //public virtual Tour Tour { get; set; } = null!;
        //public virtual Account Customer { get; set; } = null!;

        //public virtual ICollection<Payment> Payments { get; set; }

        //public decimal[,] TotalPrice { get; set; } = new decimal[10, 2];
        /*public TourRequest Tours { get; set; }
        public AccountRequest Customers { get; set; }
        public List<PaymentRequest> Payments { get; set; }*/
    }
}
