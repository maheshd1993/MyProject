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
    
    public partial class acc_journalvoucherlines
    {
        public int ID { get; set; }
        public string JournalVoucherNo { get; set; }
        public int AccountNumber { get; set; }
        public Nullable<decimal> Dr { get; set; }
        public Nullable<decimal> Cr { get; set; }
        public string Particulars { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int BranchCode { get; set; }
        public int JournalVoucherLineID { get; set; }
        public string SLGroupCode { get; set; }
        public string SubLedger { get; set; }
        public Nullable<int> ChequeID { get; set; }
        public Nullable<System.DateTime> ChqClearDate { get; set; }
        public string OpBaltype { get; set; }
        public Nullable<decimal> OpBalCrDr { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string Narration { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
    }
}
