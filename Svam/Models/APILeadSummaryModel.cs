using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class APILeadSummaryModel
    {
        public Int32 NewLeadCount { get; set; }
        public Int32 MissedFollowUpCount { get; set; }
        public Int32 FollowUpCount { get; set; }
        public Int32 DelayedFollowUpCount { get; set; }
        public Int32 NotInterestedCount { get; set; }
        public Int32 ClosedLeadsUpCount { get; set; }
        public Int32 SuspectLeadsCount { get; set; }
        public Int32 ProspectLeadsCount { get; set; }
    }
}