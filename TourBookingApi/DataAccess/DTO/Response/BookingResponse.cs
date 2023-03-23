namespace DataAccess.DTO.Response
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
        public decimal TotalPrice { get; set; }



        public TourResponse Tour { get; set; }
        public AccountResponse Customer { get; set; }

        public List<PaymentResponse> Payments { get; set; }
    }
}
