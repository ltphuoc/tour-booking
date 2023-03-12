using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.Response
{
    public class BaseResponseViewModel<T>
    {
        public StatusViewModel Status { get; set; }
        public T Data { get; set; }
    }

    public class StatusViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }
    }
}
