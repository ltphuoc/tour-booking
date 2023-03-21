﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class TourResponse
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public int TourDuration { get; set; }
        public int TourCapacity { get; set; }
        public int Status { get; set; }
        public List<TourDetailResponse> TourDetails { get; set; }
        public List<TourPriceResponse> TourPrices { get; set; }

        public List<TourGuideReponse> TourGuides { get; set; }
    }
}
