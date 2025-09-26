using CMS.Storage.Enum;
using System.Collections.Generic;

namespace CMS.Storage.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public UserType UserType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public UserStatus? Status { get; set; }
        public List<int> RoleIds { get; set; }
    }        
}