using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class AddLeadDescrResponseDTO 
    {
        public string  Message  { get; set; }
        public string  ModifiedDate  { get; set; }
        public string  FollowupDateTime  { get; set; }
        public string FollowupTime { get; set; } 
        public string  FollowupDate   { get; set; }
        public int LeadReminder  { get; set; }
    }
}