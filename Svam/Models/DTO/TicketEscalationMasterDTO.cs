using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class TicketEscalationMasterDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? EscalateUserId { get; set; }
        public int EscalateLevel { get; set; }
        public float EscalateTime { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
    }
}