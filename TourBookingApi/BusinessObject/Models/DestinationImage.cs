using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class DestinationImage
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public string Image { get; set; } = null!;

        public virtual Destination Destination { get; set; } = null!;
    }
}
