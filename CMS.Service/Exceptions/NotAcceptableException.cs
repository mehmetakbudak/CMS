using CMS.Model.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CMS.Service.Exceptions
{
   public class NotAcceptableException : ApiExceptionBase
    {
        public NotAcceptableException() : base()
        {
            Error = new BaseResult { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = "Kabul Edilmeyen İstek." };
        }

        public NotAcceptableException(string message) : base(message)
        {
            Error = new BaseResult { StatusCode = (int)HttpStatusCode.NotAcceptable, Message = message };
        }

        protected override HttpStatusCode HttpStatusCode => HttpStatusCode.NotAcceptable;
    }
}
