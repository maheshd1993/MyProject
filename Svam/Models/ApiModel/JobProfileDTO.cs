using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class JobProfileDTO
    {
        public int ProfileID  { get; set; }
        public int CompanyID   { get; set; }
        public int BranchID  { get; set; }
        public string ProfileName  { get; set; }
        public string Token   { get; set; }
    }
}