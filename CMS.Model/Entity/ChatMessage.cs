using System;

namespace CMS.Model.Entity
{
    public class ChatMessage : BaseModel
    {
        public int ChatId { get; set; }

        public int? UserId { get; set; }
        
        public string Message { get; set; }

        public DateTime InsertDate { get; set; }

        public User User { get; set; }
        
        public Chat Chat { get; set; }
    }
}
