namespace DataAccess.DTO.Response
{
    public class DestinationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Status { get; set; }
        public List<DestinationImageResponse> DestinationImages { get; set; }
    }
}
