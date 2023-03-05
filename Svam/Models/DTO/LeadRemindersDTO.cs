using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class LeadRemindersDTO
    {
        public int LeadId { get; set; }
        public int UserId { get; set; }
        public bool IsLeadReminder { get; set; }
    }
}