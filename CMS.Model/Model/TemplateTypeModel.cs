using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Model.Model
{
    public class TemplateResponseModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class ForgotPasswordTemplateModel
    {
        public string FullName { get; set; }
        public string Url { get; set; }
    }
}
