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
    
    public partial class tbl_daywisecashexpense
    {
        public int ID { get; set; }
        public Nullable<decimal> AvailableCash { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string PtclrDaycash { get; set; }
        public string Expenses { get; set; }
        public string ExpenseType { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public string Flag { get; set; }
        public string SyncID { get; set; }
    }
}
