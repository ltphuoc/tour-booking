namespace DataAccess.DTO.Request
{
    public class TransportationRequest
    {
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
    }

    public class TransportationUpdateRequest
    {
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
    }

    public class TransportationCreateRequest
    {
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
    }
}
