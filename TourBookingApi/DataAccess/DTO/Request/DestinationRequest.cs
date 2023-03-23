using BusinessObject.Models;
using DataAccess.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Request
{
    public class DestinationRequest
    {
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Status { get; set; }
        public List<DestinationImageRequest> DestinationImages { get; set; }
    }

    public class DestinationUpdateRequest
    {
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Status { get; set; }
    }

    public class DestinationCreateRequest
    {
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<DestinationImageRequest> DestinationImages { get; set; }
    }
}
