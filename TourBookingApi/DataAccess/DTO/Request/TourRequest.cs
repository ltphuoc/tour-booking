namespace DataAccess.DTO.Request
{
    public class TourRequest
    {
        public string TourName { get; set; } = null!;
        public int TourDuration { get; set; } = 0!;
        public int TourCapacity { get; set; } = 0!;
        public int Status { get; set; }
        public int? TourGuideId { get; set; }
    }

    public class TourUpdateRequest
    {
        public string TourName { get; set; } = null!;
        public int TourDuration { get; set; } = 0!;
        public int TourCapacity { get; set; } = 0!;
        public int Status { get; set; }
        public int? TourGuideId { get; set; }
    }

    public class TourCreateRequest
    {
        public string TourName { get; set; } = null!;
        public int TourDuration { get; set; }
        public int TourCapacity { get; set; }
        public int Status { get; set; }
        public int? TourGuideId { get; set; }
    }
}
