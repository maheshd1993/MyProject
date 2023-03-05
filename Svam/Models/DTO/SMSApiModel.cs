using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class SMSApiModel
    {
       public string SMSAPI { get; set; }
       public string UserName { get; set; }
       public string Password { get; set; }
       public string SenderID  { get; set; }
    }
}