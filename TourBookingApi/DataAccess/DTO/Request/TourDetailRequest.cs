namespace DataAccess.DTO.Request
{
    public class TourDetailRequest
    {
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Departure { get; set; } = null!;
        public int DestinationId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int? TransportationId { get; set; } = null!;
        public string TourDescription { get; set; } = null!;
    }

    public class TourDetailUpdateRequest
    {
        public DateTime? StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; } = null;
        public string Departure { get; set; } = null!;
        public DateTime? ExpiredDate { get; set; } = null;
        public string TourDescription { get; set; } = null!;
    }

    public class TourDetailCreateRequest
    {
        public int TourId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Departure { get; set; } = null!;
        public int DestinationId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int? TransportationId { get; set; } = null!;
        public string TourDescription { get; set; } = null!;
    }
}
