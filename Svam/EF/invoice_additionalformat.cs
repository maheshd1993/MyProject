//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Svam.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class invoice_additionalformat
    {
        public int ID { get; set; }
        public string field1_name { get; set; }
        public string field1_status { get; set; }
        public string field2_name { get; set; }
        public string field2_status { get; set; }
        public string field3_name { get; set; }
        public string field3_status { get; set; }
        public string field4_name { get; set; }
        public string field4_status { get; set; }
        public string field5_name { get; set; }
        public string field5_status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
    }
}
