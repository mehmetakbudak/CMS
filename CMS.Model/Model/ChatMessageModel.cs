using System;

namespace CMS.Model.Model
{
    public class ChatMessageModel
    {
        public Guid Code { get; set; }
        public int? UserId { get; set; }
        public string NameSurname { get; set; }
        public string Message { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
