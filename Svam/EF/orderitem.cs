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
    
    public partial class orderitem
    {
        public int ID { get; set; }
        public string AmazonOrderID { get; set; }
        public string QuantityOrdered { get; set; }
        public string Title { get; set; }
        public string ShipTaxCurrencyCode { get; set; }
        public string ShipTaxAmount { get; set; }
        public string PromotionDisCurrencyCode { get; set; }
        public string PromotionDisAmount { get; set; }
        public string IsGift { get; set; }
        public string ASIN { get; set; }
        public string SellerSKU { get; set; }
        public string OrderItemId { get; set; }
        public string NumberOfItems { get; set; }
        public string QuantityShipped { get; set; }
        public string ShipPriceCurrencyCode { get; set; }
        public string ShipPriceAmount { get; set; }
        public string ItemPriceCurrencyCode { get; set; }
        public string ItemPriceAmount { get; set; }
        public string ItemTaxCurrencyCode { get; set; }
        public string ItemTaxAmount { get; set; }
        public string ShipDisCurrencyCode { get; set; }
        public string ShipDisAmount { get; set; }
        public int CompanyId { get; set; }
        public int BranchCode { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string IsActive { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}