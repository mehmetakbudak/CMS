using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Storage.Model
{
    public class EmailSettingModel
    {
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
    }
}
