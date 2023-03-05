using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMEmailSettingModel
    {
        public Int32? EmailSettingID { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        public String EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter smtp host")]
        public String SMTPHost { get; set; }
        [Required(ErrorMessage = "Please enter port no.")]
        public Int32? Port { get; set; }
        [Required(ErrorMessage = "Please enter username")]
        public String UserName { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public String Password { get; set; }        
        public Boolean? SSL { get; set; }
        public String CCEmail { get; set; }
        [Required(ErrorMessage = "Please enter display name")]
        public String DisplayName { get; set; }
    }

    public class CRMSendEmailModel 
    {
        [Required(ErrorMessage = "Please enter email address")]
        public String ToEmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter subject")]
        public String Subject { get; set; }
        //[Required(ErrorMessage = "Please enter subject")]
        public String MessageBody { get; set; }
      
        public String AttachmentFile { get; set; }    
    }
}