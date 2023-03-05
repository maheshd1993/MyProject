using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class EmailSetupDTO
    {
        public int? EmailSettingID { get; set; }        
        public string EmailAddress { get; set; }        
        public string SMTPHost { get; set; }      
        public int? Port { get; set; }       
        public string UserName { get; set; }       
        public string Password { get; set; }
        public bool? SSL { get; set; }
        public string CCEmail { get; set; }      
        public string DisplayName { get; set; }
        public int? CompanyID  { get; set; }
        public int? BranchID  { get; set; }
        public string UID  { get; set; }
        public string Token  { get; set; }
    }
}