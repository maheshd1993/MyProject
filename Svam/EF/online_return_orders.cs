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
    
    public partial class online_return_orders
    {
        public int order_id { get; set; }
        public Nullable<int> Item_id { get; set; }
        public string Sku_ID { get; set; }
        public string PurchasePrice { get; set; }
        public Nullable<float> MRP { get; set; }
        public Nullable<float> MrpDiscount { get; set; }
        public Nullable<float> SavingAmt { get; set; }
        public Nullable<int> Item_qun { get; set; }
        public Nullable<float> Amount { get; set; }
        public string payment_type { get; set; }
        public Nullable<int> ratings { get; set; }
        public string sales_type { get; set; }
        public string User_Id { get; set; }
        public string BranchCode { get; set; }
        public string CompanyID { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_mobile { get; set; }
        public string review { get; set; }
        public Nullable<int> review_status { get; set; }
        public string reviewDate { get; set; }
        public Nullable<int> voucherCode { get; set; }
        public string voucherType { get; set; }
        public string voucherVal { get; set; }
        public string CompletedDate { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<int> isactive { get; set; }
        public string flag { get; set; }
        public string reason { get; set; }
        public string SyncID { get; set; }
        public string user_address { get; set; }
        public string OrderId { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<int> DeliveryCharge { get; set; }
        public Nullable<int> Order_Status { get; set; }
    }
}