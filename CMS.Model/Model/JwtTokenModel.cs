using CMS.Model.Enum;
using System;

namespace CMS.Model.Model
{
    public class JwtTokenModel
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
