using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class ChatMessage : BaseEntityModel
    {
        public int ChatId { get; set; }

        public Chat Chat { get; set; }

        public int? UserId { get; set; }
        
        public User User { get; set; }
        
        public string Message { get; set; }

        public DateTime InsertedDate { get; set; }        
    }
}
