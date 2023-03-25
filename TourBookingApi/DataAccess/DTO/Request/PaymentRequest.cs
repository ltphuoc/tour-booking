namespace DataAccess.DTO.Request
{
    public class PaymentRequest
    {
        public string PaymentMethod { get; set; } = null!;
        public DateTime? PaymentDate { get; set; } = null;
        public decimal PaymentAmount { get; set; }
        /*public List<BookingRequest> Bookings { get; set; }*/
    }

    public class PaymentUpdateAdminRequest
    {
        public int? Status { get; set; }
    }

    public class PaymentUpdateUserRequest
    {
        public string? PaymentImage { get; set; }
    }

    public class PaymentCreateRequest
    {
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public int? Status { get; set; }
        public string? PaymentCode { get; set; }
        public string? PaymentImage { get; set; }

        //public List<BookingRequest> Bookings { get; set; }
    }
}
