using BusinessObject.Models;

namespace DataAccess.DTO.Request
{
    public class DestinationImageRequest
    {
        public string Image { get; set; } = null!;
    }

    public class DestinationImageUpdateRequest
    {
        public string Image { get; set; } = null!;
    }

    public class DestinationImageCreateRequest
    {
        public List<string> Image { get; set; } = null!;
        public int DestinationId { get; set; }
        //public virtual Destination Destination { get; set; } = null!;
    }
}
