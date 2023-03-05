using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class StackedChartData
    {
        //public static List<StackedChartData> GetData()
        //{
        //    var data = new List<StackedChartData>();

        //    data.Add(new StackedChartData("Sat", 50000, 10500));
        //    data.Add(new StackedChartData("Fri", 50000, 12000));
        //    data.Add(new StackedChartData("Thu", 50000, 13000));
        //    data.Add(new StackedChartData("Wed", 50000, 25000));
        //    data.Add(new StackedChartData("Tue", 50000, 30000));
        //    data.Add(new StackedChartData("Mon", 50000, 45000));
        //    data.Add(new StackedChartData("Sun", 50000, 49000));

        //    return data;
        //}


        public StackedChartData(string label, double value1, double value2)
        {
            this.Label = label;
            this.Value1 = value1;
            this.Value2 = value2;
        }



        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }

    }
}