using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class TransportationRequest
    {
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
        public List<TourDetailRequest> TourDetail { get; set; }
    }

    public class TransportationUpdateRequest
    {
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
        public List<TourDetailRequest> TourDetail { get; set; }
    }

    public class TransportationCreateRequest
    {
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
        public List<TourDetailRequest> TourDetail { get; set; }
    }
}
