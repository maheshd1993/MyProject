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
    
    public partial class inv_addadjustment
    {
        public int id { get; set; }
        public string AdjustNo { get; set; }
        public string AdjustDt { get; set; }
        public string AdjustmentType { get; set; }
        public string StockINOut { get; set; }
        public string ItemCd { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string Barcode { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Location { get; set; }
        public Nullable<long> Qty { get; set; }
        public string Rmk { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<int> BranchID { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
