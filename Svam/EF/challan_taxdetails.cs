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
    
    public partial class challan_taxdetails
    {
        public int ID { get; set; }
        public string GRNNo { get; set; }
        public string SystemBillNo { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public Nullable<decimal> TaxPer { get; set; }
        public Nullable<decimal> TaxAmt { get; set; }
        public string CompanyID { get; set; }
        public string BranchCode { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
