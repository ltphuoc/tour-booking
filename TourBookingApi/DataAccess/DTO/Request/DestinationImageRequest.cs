using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class DestinationImageRequest
    {
        public string Image { get; set; } = null!;
    }

    public class DestinationImageUpdateRequest
    {
        public int Id { get; set; }
        public string Image { get; set; } = null!;
    }
}
