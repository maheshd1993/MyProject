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
    
    public partial class pur_purchasegrnorder
    {
        public int ID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string ponumber { get; set; }
        public Nullable<System.DateTime> PurchaseOrderDate { get; set; }
        public string POTypeCode { get; set; }
        public string SupplierNM { get; set; }
        public string SupplierCode { get; set; }
        public string MobileNo { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<System.DateTime> ReferenceDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> ItemTotal { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public string StatusCode { get; set; }
        public string Address { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
        public string SystemVoucherNo { get; set; }
        public string pincode { get; set; }
        public Nullable<int> status { get; set; }
        public string remarks { get; set; }
        public string mode_dispatch { get; set; }
        public Nullable<decimal> discountamt { get; set; }
        public Nullable<decimal> freight_charges { get; set; }
        public Nullable<decimal> packing_charges { get; set; }
        public Nullable<decimal> forwarding_charges { get; set; }
        public Nullable<decimal> other_charges { get; set; }
        public Nullable<decimal> insurance_charges { get; set; }
        public Nullable<decimal> tot_gst { get; set; }
        public Nullable<decimal> DiscountPer { get; set; }
        public string item_centre { get; set; }
        public string centre_address { get; set; }
    }
}
