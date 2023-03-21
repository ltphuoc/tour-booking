using System.Net;

namespace DataAccess.DTO.Response
{
    public class BaseResponseViewModel<T>
    {
        public StatusViewModel Status { get; set; } = new StatusViewModel();
        public T? Data { get; set; }
    }

    public class StatusViewModel
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
    }
}
