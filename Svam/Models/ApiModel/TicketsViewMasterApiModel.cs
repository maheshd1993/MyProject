using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class TicketsViewMasterApiModel
    {   
        public string FieldLabel  { get; set; }
        public string ColumnName   { get; set; }
        public bool IsActive  { get; set; }
        public bool IsFilter     { get; set; }
        public bool IsActiveButtonAction { get; set; }
        public bool IsFilterButtonAction  { get; set; }
    }
}