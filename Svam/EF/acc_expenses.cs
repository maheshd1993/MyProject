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
    
    public partial class acc_expenses
    {
        public int ID { get; set; }
        public string VoucherNo { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string DR_Account { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string GSTType { get; set; }
        public Nullable<int> GST_Rate { get; set; }
        public Nullable<decimal> GSTAmount { get; set; }
        public string PaymentMode { get; set; }
        public string RefNo { get; set; }
        public string CardNo { get; set; }
        public string BankName { get; set; }
        public string FinalAmount { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string CR_Account { get; set; }
        public string Remarks { get; set; }
        public string ac_no { get; set; }
        public string bank_name { get; set; }
        public string ifsc { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreateionDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
    }
}