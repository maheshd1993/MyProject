using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class CompanyIdBranchIdDTO
    {
        public int Id  { get; set; }
        public int CompanyID { get; set; }
        public int BranchID  { get; set; }
    }
}