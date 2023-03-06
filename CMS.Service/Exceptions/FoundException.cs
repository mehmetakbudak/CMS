using CMS.Storage.Model;
using System.Net;

namespace CMS.Service.Exceptions
{
    public class FoundException : ApiExceptionBase
    {
        public FoundException() : base()
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.Found, Message = "Kayıt Mevcut." };
        }

        public FoundException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = HttpStatusCode.Found, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.Found;
    }
}
