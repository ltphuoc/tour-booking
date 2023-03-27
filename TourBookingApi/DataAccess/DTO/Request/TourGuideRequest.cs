namespace DataAccess.DTO.Request
{
    public class TourGuideRequest
    {
        public string TourGuideName { get; set; } = null!;
        public int TourGuideAge { get; set; } = 0!;
        public string TourGuidePhone { get; set; } = null!;
        public string TourGuideEmail { get; set; } = null!;
        public string TourGuideLanguageSpoken { get; set; } = null!;
        public string TourGuideAva { get; set; } = null!;
        public string TourGuideBio { get; set; } = null!;

        //public List<TourRequest> Tour { get; set; }
    }

    public class TourGuideUpdateRequest
    {
        public string TourGuideName { get; set; } = null!;
        public int TourGuideAge { get; set; } = 0!;
        public string TourGuidePhone { get; set; } = null!;
        public string TourGuideEmail { get; set; } = null!;
        public string TourGuideLanguageSpoken { get; set; } = null!;
        public string TourGuideAva { get; set; } = null!;
        public string TourGuideBio { get; set; } = null!;

        //public int TourId { get; set; }

       /* public List<TourRequest> Tour { get; set; }*/
    }

    public class TourGuideCreateRequest
    {
        public string TourGuideName { get; set; } = null!;
        public int TourGuideAge { get; set; } = 0!;
        public string TourGuidePhone { get; set; } = null!;
        public string TourGuideEmail { get; set; } = null!;
        public string TourGuideLanguageSpoken { get; set; } = null!;
        public string TourGuideAva { get; set; } = null!;
        public string TourGuideBio { get; set; } = null!;

        //public int TourId { get; set; }

       
       /* public List<TourRequest>? Tour { get; set; }*/
    }
}
