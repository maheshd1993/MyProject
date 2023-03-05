using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam._Class
{
    public class LeadWeeklyChartModel
    {
        public string Seriesname { get; set; }
        public List<LeadData> Data { get; set; }
    }

    public class LeadData
    {
        public int Value { get; set; }

    }

    public class LeadCategory
    {
        public string Label { get; set; }
    }

    public class FollowUpMissedLeadModel
    {

    }
}