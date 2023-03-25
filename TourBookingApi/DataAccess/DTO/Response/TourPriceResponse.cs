namespace DataAccess.DTO.Response
{
    public class TourPriceResponse
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public decimal PriceAdults { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceInfants { get; set; }
    }
}
