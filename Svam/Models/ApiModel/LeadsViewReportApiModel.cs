using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class LeadsViewReportApiModel
    {
        public string FieldLabel { get; set; }
        public string ActiveColumnName { get; set; }
        public string FilterColumnName { get; set; } 
        public bool IsActive { get; set; }
        public bool IsFilter { get; set; }
        public bool IsActiveButtonAction { get; set; }
        public bool IsFilterButtonAction { get; set; }
    }
}