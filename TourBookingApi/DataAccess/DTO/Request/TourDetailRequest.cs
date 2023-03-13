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
        public List<DestinationRequest> Destination { get; set; }
        public DateOnly? ExpiredDate { get; set; } = null;
        public string TourDescription { get; set; } = null!;
    }
}
