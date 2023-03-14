using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class TourDetailRequest
    {
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set;} = null;
        public string Departure { get; set; } = null!;
        public DateTime? ExpiredDate { get; set; } = null;
        public string TourDescription { get; set; } = null!;
        public List<DestinationRequest> Destination { get; set; }
        public List<TourRequest> Tour { get; set; }
        public List<TransportationRequest> Transport { get; set; }
    }

    public class TourDetailUpdateRequest
    {
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; } = null;
        public string Departure { get; set; } = null!;
        public DateTime? ExpiredDate { get; set; } = null;
        public string TourDescription { get; set; } = null!;
        public List<DestinationRequest> Destination { get; set; }
        public List<TourRequest> Tour { get; set; }
        public List<TransportationRequest> Transport { get; set; }
    }

    public class TourDetailCreateRequest
    {
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; } = null;
        public string Departure { get; set; } = null!;
        public DateTime? ExpiredDate { get; set; } = null;
        public string TourDescription { get; set; } = null!;
        public List<DestinationRequest> Destination { get; set; }
        public List<TourRequest> Tour { get; set; }
        public List<TransportationRequest> Transport { get; set; }
    }
}
