using System.Collections.Generic;
using System.Net;

namespace CMS.Model.Model
{
    public class BaseResult
    {        
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class ServiceResult : BaseResult
    {
        public object Data { get; set; }
    }
}
