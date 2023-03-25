namespace DataAccess.DTO.Response
{
    public class DestinationImageResponse
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public string Image { get; set; } = null!;
    }
}
