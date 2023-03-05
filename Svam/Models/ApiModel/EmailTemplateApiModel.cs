using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models.ApiModel
{
    public class EmailTemplateApiModel
    {
        public long? EmailTemplateID { get; set; }        
        public string EmailTemplateName { get; set; }
        [AllowHtml]
        public string EmailTempleteBody { get; set; }
        public string CreatedOn { get; set; }
        public int? BranchID { get; set; }
        public int? CompanyID { get; set; }
        public int? UID { get; set; }
        public string Token  { get; set; }
    }
}