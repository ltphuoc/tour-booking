namespace DataAccess.DTO.Response
{
    public class TourDetailResponse
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int DestinationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Departure { get; set; } = null!;
        public int? TransportationId { get; set; } = null!;
        public DateTime ExpiredDate { get; set; }
        public string TourDescription { get; set; } = null!;

        public DestinationResponse Destination { get; set; }

        public TransportationResponse Transportation { get; set; }

    }
}
