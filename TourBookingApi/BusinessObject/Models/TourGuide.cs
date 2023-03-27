using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TourGuide
    {
        public TourGuide()
        {
            Tours = new HashSet<Tour>();
        }

        public int Id { get; set; }
        public string TourGuideName { get; set; } = null!;
        public int TourGuideAge { get; set; }
        public string TourGuidePhone { get; set; } = null!;
        public string TourGuideEmail { get; set; } = null!;
        public string TourGuideLanguageSpoken { get; set; } = null!;
        public string TourGuideAva { get; set; } = null!;
        public string TourGuideBio { get; set; } = null!;

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
