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
    
    public partial class vouchernumberformate
    {
        public int ID { get; set; }
        public string VoucherType { get; set; }
        public string VoucherMethod { get; set; }
        public Nullable<System.DateTime> ApplicableFromDate { get; set; }
        public Nullable<int> StartingNumber { get; set; }
        public string Particular { get; set; }
        public string PrafixDetails { get; set; }
        public string EntryType { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CompanyId { get; set; }
        public int BranchCode { get; set; }
        public string Flag { get; set; }
        public string SyncID { get; set; }
        public string bill_type { get; set; }
    }
}
