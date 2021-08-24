using CMS.Model.Model;
using System;
using System.Net;

namespace CMS.Service.Exceptions
{
    public class NotFoundException : ApiExceptionBase
    {
        public NotFoundException() : base()
        {
            Error = new BaseResult { StatusCode = (int)HttpStatusCode.NotFound, Message = "Kayıt bulunamadı." };
        }

        public NotFoundException(String message) : base(message)
        {
            Error = new BaseResult { StatusCode = (int)HttpStatusCode.NotFound, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
    }
}
