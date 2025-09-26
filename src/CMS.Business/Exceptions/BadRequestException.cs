using CMS.Storage.Dtos.Response;
using System.Net;

namespace CMS.Business.Exceptions
{
    public class BadRequestException : ApiExceptionBase
    {
        public BadRequestException() : base()
        {
            Error = new BaseResult { StatusCode = 400, Message = "Geçersiz istek." };
        }

        public BadRequestException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = 400, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
    }
}
