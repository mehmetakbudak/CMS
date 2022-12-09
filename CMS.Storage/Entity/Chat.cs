using CMS.Storage.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class Chat : BaseEntityModel
    {
        public Guid Code { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }

        public ChatStatus Status { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }

        public virtual List<ChatMessage> ChatMessages { get; set; }
    }
}
