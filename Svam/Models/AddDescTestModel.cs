using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class AddDescTestModel
    {
        public int? LeadID { get; set; }
        public Int32? UID { get; set; }
        public string UserName { get; set; }
        public string CompanyID { get; set; }
        public string BranchID { get; set; }
        public string txtDescription { get; set; }
        public string FollowUpDate { get; set; }
        public int? LeadStatusID { get; set; }
        public string Token { get; set; }
    }
}