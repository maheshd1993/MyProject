using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class UserCredential
    {
        public string UserName { get; set; }
        public string Email  { get; set; }
        public string Password { get; set; }
        public string KeyVersion { get; set; }
    }
}