using CMS.Storage.Dtos.Response;
using System.Net;

namespace CMS.Business.Exceptions
{
    public class FoundException : ApiExceptionBase
    {
        public FoundException() : base()
        {
            Error = new BaseResult { StatusCode = 302, Message = "Kayıt Mevcut." };
        }

        public FoundException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = 302, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.Found;
    }
}
