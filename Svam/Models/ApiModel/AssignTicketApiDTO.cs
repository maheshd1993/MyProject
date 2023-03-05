using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class AssignTicketApiDTO
    {
        public int UID { get; set; }
        public int AssignToUserId  { get; set; }
        public int CompanyID { get; set; }
        public int BranchID  { get; set; }
        public string AssignTicketIds { get; set; }
        public string Token  { get; set; }
    }
}