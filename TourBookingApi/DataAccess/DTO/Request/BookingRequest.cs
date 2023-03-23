namespace DataAccess.DTO.Request
{
    public class BookingRequest
    {
        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
    }

    public class BookingUpdateRequest
    {
        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
    }

    public class BookingCreateRequest
    {
        public int TourId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
    }
}
