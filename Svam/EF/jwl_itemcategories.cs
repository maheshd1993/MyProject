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
    
    public partial class jwl_itemcategories
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Nullable<int> CategoryTypeID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> DiscountRuleID { get; set; }
        public Nullable<int> Photo { get; set; }
        public Nullable<int> Document { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public Nullable<int> NoOfDays { get; set; }
        public string flag { get; set; }
        public string categoryID { get; set; }
        public string SyncID { get; set; }
    }
}
