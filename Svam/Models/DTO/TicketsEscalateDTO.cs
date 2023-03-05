using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class TicketsEscalateDTO
    {
       public long TicketID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchID { get; set; }
        public int? AssignedTo { get; set; }
        public int? AssignedBy { get; set; }
        public string AssignedDate { get; set; }
    }
}