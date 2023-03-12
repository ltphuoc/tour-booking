using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class BaseResponsePagingViewModel<T>
    {
        public PagingsMetadata Metadata { get; set; }
        public List<T> Data { get; set; }
    }

    public class PagingsMetadata
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
    }
}
