namespace DataAccess.DTO.Response
{
    public class TourGuideReponse
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string TourGuideName { get; set; } = null!;
        public int TourGuideAge { get; set; }
        public string TourGuidePhone { get; set; } = null!;
        public string TourGuideEmail { get; set; } = null!;
        public string TourGuideLanguageSpoken { get; set; } = null!;
        public string TourGuideAva { get; set; } = null!;
        public string TourGuideBio { get; set; } = null!;
        public List<TourResponse> Tours { get; set; }

        /*public List<DestinationResponse> Destination { get; set; }*/
    }
}
