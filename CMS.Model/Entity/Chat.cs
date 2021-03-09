using CMS.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class Chat : BaseModel
    {
        public Guid ChatGuid { get; set; }
        public string NameSurname { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public ChatStatus ChatStatus { get; set; }

        public DateTime InsertDate { get; set; }

        public bool Deleted { get; set; }

        public virtual List<ChatMessage> ChatMessages { get; set; }
    }
}
