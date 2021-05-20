using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class ServiceResult
    {
        public string Message { get; set; }

        public object Data { get; set; }

        public int StatusCode { get; set; }
    }
}
