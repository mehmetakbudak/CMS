using CMS.Storage.Enum;
using System;

namespace CMS.Storage.Model
{
    public class JwtTokenModel
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
