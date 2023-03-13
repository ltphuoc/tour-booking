using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class BookingRequest
    {
        public int CustomerId { get; set; } = 0!;
        public DateOnly BookingDate { get; set; } = new DateOnly();
        public int NumAdults { get; set; } = 0!;
        public int NumInfants { get; set; } = 0!;
        public decimal[,] TotalPrice { get; set; } = new decimal[10, 2];
    }
}
