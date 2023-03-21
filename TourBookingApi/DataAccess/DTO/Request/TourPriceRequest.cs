using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class TourPriceRequest
    {
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }
    }

    public class TourPriceUpdateRequest
    {
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }
    }

    public class TourPriceCreateRequest
    {
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }
    }
}
