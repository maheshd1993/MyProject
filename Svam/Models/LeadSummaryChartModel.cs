using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Svam.Models
{
    [DataContract]
    public class LeadSummaryChartModel
    {
        //public LeadSummaryChartModel(string label, double y)
        //{
        //    this.Label = label;
        //    this.Y = y;
        //}

        //Explicitly setting the name to be used while serializing to JSON.
        //[DataMember(Name = "label")]
        //public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        //[DataMember(Name = "y")]
        //public Nullable<double> Y = null;

        public String LabelName { get; set; }
        public Nullable<int> DataCount { get; set; }
    }
}