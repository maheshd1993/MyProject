using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class NotificationCountModel
    {
        public int TotalAllLeadCount    { get; set; }
        public int TodayFollowUpCount { get; set; }
        public int TodayNewLeadCount  { get; set; }
        public int TodayAssignLeadCount  { get; set; }
        public int TodayAssignForm16Count { get; set; }
        public int TodayAssignExpenseCount { get; set; }
    }
    
}