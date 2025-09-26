using CMS.Storage.Enum;
using System;

namespace CMS.Storage.Dtos.User
{
    public class UserListDto
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        public string UserTypeName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public UserStatus Status { get; set; }
        public string StatusName { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
