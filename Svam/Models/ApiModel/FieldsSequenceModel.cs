using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class FieldsSequenceModel
    {
        public int CompanyID  { get; set; }
        public int BranchID  { get; set; }
        public int UID  { get; set; }
        public string Token { get; set; }
        public  List<FieldsSequence> Sequences { get; set; }
    }

    public class FieldsSequence
    {
        public string ColumnName { get; set; }
        public int Priority { get; set; }       
    }
}