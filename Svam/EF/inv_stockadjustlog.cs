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
    
    public partial class inv_stockadjustlog
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Item_Sku { get; set; }
        public Nullable<decimal> OldStock { get; set; }
        public Nullable<decimal> AdjustedStock { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public string flag { get; set; }
        public string SyncId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Remark { get; set; }
    }
}
