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
    
    public partial class acc_otherincomes
    {
        public int ID { get; set; }
        public System.DateTime IncomeVoucherDate { get; set; }
        public System.DateTime IncomeDate { get; set; }
        public string IncomeFrom { get; set; }
        public string IncomeDetails { get; set; }
        public decimal IncomeAmount { get; set; }
        public Nullable<decimal> TaxPayable { get; set; }
        public Nullable<decimal> NetReceivable { get; set; }
        public Nullable<decimal> NetReceived { get; set; }
        public Nullable<decimal> CurrentDue { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int BranchCode { get; set; }
        public Nullable<int> Document { get; set; }
        public int IncomeVoucherNo { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
