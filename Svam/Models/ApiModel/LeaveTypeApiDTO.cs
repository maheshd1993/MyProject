using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class LeaveTypeApiDTO
    {
        public int ID { get; set; }
        public string LeaveName { get; set; }
        public bool IsActive { get; set; }     
        public int? UID  { get; set; }       
        public int? CompanyID { get; set; }
        public int? BranchID { get; set; }
        public string Token { get; set; }
    }

    public class LeaveTypeApiVM 
    {
        public int ID { get; set; }
        public string LeaveName { get; set; }
        public bool IsActive { get; set; }
    }
}