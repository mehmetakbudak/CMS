using CMS.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Model.Model
{
    public class ChatModel : BaseModel
    {
        public string NameSurname { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
    }
}
