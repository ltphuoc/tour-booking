using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode Code { get; set; }
    }
}
