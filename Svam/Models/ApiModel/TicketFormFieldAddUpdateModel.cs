using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class TicketFormFieldAddUpdateModel
    {
        public string TextName { get; set; }
        public string FieldName  { get; set; }
        public bool IsActive { get; set; }
        public bool IsRequired { get; set; }
        public string FieldType { get; set; }  
        public string SaveType { get; set; } 
        public int? UID  { get; set; }
        public int? BranchID { get; set; }
        public int? CompanyID { get; set; }
        public string Token { get; set; }
    }
}