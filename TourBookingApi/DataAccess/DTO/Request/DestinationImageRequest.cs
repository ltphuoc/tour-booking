using System.ComponentModel.DataAnnotations;

namespace DataAccess.DTO.Request
{
    public class DestinationImageRequest
    {
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = null!;
    }

    public class DestinationImageUpdateRequest
    {
        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = null!;
    }

    public class DestinationImageCreateRequest
    {
        [Required(ErrorMessage = "Image is required")]
        public List<string> Image { get; set; } = null!;
        [Required(ErrorMessage = "DestinationId is required")]
        public int DestinationId { get; set; }
    }
}
