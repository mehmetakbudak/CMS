using CMS.Storage.Dtos.Response;
using System.Net;

namespace CMS.Business.Exceptions
{
    public class NotFoundException : ApiExceptionBase
    {
        public NotFoundException() : base()
        {
            Error = new BaseResult { StatusCode = 404, Message = "Kayıt bulunamadı." };
        }

        public NotFoundException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = 404, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
    }
}
