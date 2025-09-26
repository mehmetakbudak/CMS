using CMS.Storage.Dtos.Response;
using System.Net;

namespace CMS.Business.Exceptions
{
    public class UnAuthorizedException : ApiExceptionBase
    {
        public UnAuthorizedException() : base()
        {
            Error = new BaseResult { StatusCode = 401, Message = "Kimlik bilgileri doğrulanamadı." };
        }

        public UnAuthorizedException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = 401, Message = message };
        }


        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.Unauthorized;
    }
}
