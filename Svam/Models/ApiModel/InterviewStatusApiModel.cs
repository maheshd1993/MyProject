using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class InterviewStatusApiModel
    {
        public int Id  { get; set; }
        public string InterViewStatusName  { get; set; }
        public bool Status { get; set; }
        public int? CompanyID { get; set; }
        public int? BranchID { get; set; }
        public string Token { get; set; }

    }
}