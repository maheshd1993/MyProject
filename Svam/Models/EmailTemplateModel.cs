using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models
{
    public class EmailTemplateModel
    {
        public Nullable<Int64> EmailTemplateID { get; set; }

        [Required(ErrorMessage = "* Please email template name")]
        public String EmailTemplateName { get; set; }

        [AllowHtml]
        public String EmailTempleteBody { get; set; }

        public String CreatedOn { get; set; }

        public List<EmailTemplateModel> oEmailTemplateModelList { get; set; }
    }
}