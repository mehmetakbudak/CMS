using System.Collections.Generic;
using System.Net;

namespace CMS.Model.Model
{
    public class ServiceResult : ServiceReturnModel
    {

        public int IntStatusCode
        {
            get
            {
                return (int)StatusCode;
            }
        }
    }

    public class ServiceReturnModel
    {
        public List<string> Exceptions { get; set; }

        public object Data { get; set; }

        public ServiceReturnModel()
        {
            Exceptions = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }


    }
}
