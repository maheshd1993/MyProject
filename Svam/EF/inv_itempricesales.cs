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
    
    public partial class inv_itempricesales
    {
        public int ID { get; set; }
        public string SKU { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public decimal UnitPrice { get; set; }
        public string TaxInclusive { get; set; }
        public Nullable<int> TaxID { get; set; }
        public string FixedPrice { get; set; }
        public Nullable<int> DiscountRuleID { get; set; }
        public Nullable<int> LoyaltyTypeID { get; set; }
        public Nullable<int> TableTypeID { get; set; }
        public string WeekDay { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Active { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int BranchCode { get; set; }
        public int SalesPriceID { get; set; }
        public Nullable<int> LocationCode { get; set; }
        public string SyncID { get; set; }
    }
}
