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
    
    public partial class tbl_disperitempercustomer
    {
        public int Id { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public Nullable<int> companyid { get; set; }
        public string customername { get; set; }
        public string Itemsku { get; set; }
        public string discount { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}