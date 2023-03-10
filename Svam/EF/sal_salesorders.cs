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
    
    public partial class sal_salesorders
    {
        public int ID { get; set; }
        public System.DateTime SalesOrderDate { get; set; }
        public string SOTypeCode { get; set; }
        public string CustomerID { get; set; }
        public string LoyaltyCardNo { get; set; }
        public string StatusCode { get; set; }
        public Nullable<int> DeliveryID { get; set; }
        public string DeliveryAddress { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> PackagingCharges { get; set; }
        public Nullable<decimal> DeliveryCharges { get; set; }
        public string ItemTax { get; set; }
        public Nullable<decimal> ItemDiscount { get; set; }
        public Nullable<decimal> DiscountRuleID { get; set; }
        public Nullable<decimal> DiscountOnTotal { get; set; }
        public Nullable<decimal> Adjustment { get; set; }
        public string Rounding { get; set; }
        public Nullable<decimal> NetReceivable { get; set; }
        public Nullable<decimal> NetReceived { get; set; }
        public Nullable<decimal> CurrentDue { get; set; }
        public string Particulars { get; set; }
        public string Document { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string TableNo { get; set; }
        public Nullable<decimal> Tips { get; set; }
        public Nullable<System.DateTime> DeliveryTime { get; set; }
        public string WaiterNm { get; set; }
        public int BranchCode { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<decimal> TaxRate1 { get; set; }
        public Nullable<int> TaxAccountNumber1 { get; set; }
        public string Tax1 { get; set; }
        public Nullable<decimal> TaxRate2 { get; set; }
        public Nullable<decimal> TaxAccountNumber2 { get; set; }
        public Nullable<decimal> Tax2 { get; set; }
        public Nullable<decimal> TaxRate3 { get; set; }
        public string TaxAccountNumber3 { get; set; }
        public Nullable<decimal> Tax3 { get; set; }
        public Nullable<decimal> RedeemPointsValue { get; set; }
        public Nullable<decimal> TotalTaxable { get; set; }
        public Nullable<int> NoOfGuests { get; set; }
        public string CustomerName { get; set; }
        public string DiscountSalesOrderLineID { get; set; }
        public string SalesOrderNo { get; set; }
        public string Saved { get; set; }
        public Nullable<int> OrderToBranch { get; set; }
        public Nullable<System.DateTime> DeliveryDt { get; set; }
        public string Print1 { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
