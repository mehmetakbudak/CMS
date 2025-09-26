using CMS.Storage.Dtos.Response;
using System.Net;

namespace CMS.Business.Exceptions
{
    public class NotAcceptableException : ApiExceptionBase
    {
        public NotAcceptableException() : base()
        {
            Error = new BaseResult { StatusCode = 406, Message = "Kabul Edilmeyen İstek." };
        }

        public NotAcceptableException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = 406, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotAcceptable;
    }
}
