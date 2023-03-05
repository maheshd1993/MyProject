using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class PieChartModel
    {
        public PieChartModel(string label, int value)
        {
             this.Label=label;
            this.Value=value;
        }

        public string Label { get; set; }
        public int Value   { get; set; }
    }
}