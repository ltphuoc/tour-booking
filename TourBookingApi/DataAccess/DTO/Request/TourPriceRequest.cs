using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class TourPriceRequest
    {
        public decimal[,] PriceAdults { get; set; } = new decimal[10, 2];
        public decimal[,] PriceChildren { get; set; } = new decimal[10, 2];
        public decimal[,] PriceInfants { get; set; } = new decimal[10, 2];
    }
}
