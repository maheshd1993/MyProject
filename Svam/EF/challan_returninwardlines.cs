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
    
    public partial class challan_returninwardlines
    {
        public int ID { get; set; }
        public string ChalanNo { get; set; }
        public Nullable<int> challanorder_id { get; set; }
        public string ReturnInwardNo { get; set; }
        public string SalesOrderNo { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> OrdQuantity { get; set; }
        public Nullable<decimal> ReturnQty { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public decimal LinePrice { get; set; }
        public Nullable<decimal> LineTax { get; set; }
        public Nullable<decimal> DiscountRuleID { get; set; }
        public decimal LineDiscount { get; set; }
        public string DiscountSalesOrderLineID { get; set; }
        public string ExtendedPrice { get; set; }
        public string Remarks { get; set; }
        public string StatusCode { get; set; }
        public string UpdatedInStock { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int BranchCode { get; set; }
        public string CompanyID { get; set; }
        public string Instructions { get; set; }
        public string CreditNo { get; set; }
        public string CreditDt { get; set; }
        public Nullable<decimal> CessPer { get; set; }
        public Nullable<decimal> CessAmount { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
        public string colorID { get; set; }
        public string SizeID { get; set; }
        public Nullable<decimal> Taxable { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> ReturnInwardDate { get; set; }
        public Nullable<System.DateTime> SalesOrderDate { get; set; }
        public string EntryType { get; set; }
        public string CustomerNm { get; set; }
        public string CustomerID { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public string GSTType { get; set; }
        public string mobileNo { get; set; }
        public string Address { get; set; }
        public string pos { get; set; }
    }
}
