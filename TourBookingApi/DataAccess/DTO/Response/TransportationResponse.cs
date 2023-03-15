using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class TransportationResponse
    {
        public int Id { get; set; }
        public string TransportationType { get; set; } = null!;
        public string TransportationDescription { get; set; } = null!;
    }
}
