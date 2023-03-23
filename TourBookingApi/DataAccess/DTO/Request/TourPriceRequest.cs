namespace DataAccess.DTO.Request
{
    public class TourPriceRequest
    {
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }
    }

    public class TourPriceUpdateRequest
    {
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }
    }

    public class TourPriceCreateRequest
    {
        public int TourId { get; set; }
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }
    }
}
