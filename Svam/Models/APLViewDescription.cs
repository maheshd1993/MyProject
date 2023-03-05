using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class APLViewDescription
    {
        public String UserName { get; set; }
        public String Description { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public String LeadAttachment { get; set; }
    }
}