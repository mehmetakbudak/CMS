using System.Net;

namespace CMS.Storage.Dtos.Response
{
    public class BaseResult
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
