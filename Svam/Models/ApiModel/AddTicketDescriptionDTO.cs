using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class AddTicketDescriptionDTO
    {
        public long? TicketID  { get; set; }
        public string Description { get; set; }
        public string TicketStatusName { get; set; }
        public int? BranchID  { get; set; }
        public int? CompanyID { get; set; }
        public int? UID { get; set; }
        public string Token { get; set; }
    }
}