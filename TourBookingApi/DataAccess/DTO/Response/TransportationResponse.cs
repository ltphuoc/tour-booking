namespace DataAccess.DTO.Response
{
    public class TransportationResponse
    {
        public int Id { get; set; }
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
    }
}
