using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Svam._Classes
{
    public class ecomclasses
    {
        [DataContract]
        public class Login
        {
            [DataMember]
            public string user_id { get; set; }
            [DataMember]
            public string cust_id { get; set; }
            [DataMember]
            public string user_name { get; set; }
            [DataMember]
            public string user_email { get; set; }
            [DataMember]
            public string password { get; set; }
            [DataMember]
            public string CompanyID { get; set; }
            [DataMember]
            public string branch { get; set; }
        }
        [DataContract]
        public class categories
        {
            [DataMember]
            public string Category { get; set; }
            [DataMember]
            public string categoryID { get; set; }
        }
        [DataContract]
        public class products
        {
            [DataMember]
            public int ID { get; set; }
            [DataMember]
            public string ItemName { get; set; }
            [DataMember]
            public string SKU { get; set; }
            [DataMember]
            public string PurchasePrice { get; set; }
            [DataMember]
            public string MRP { get; set; }
            [DataMember]
            public string MrpDiscount { get; set; }
            [DataMember]
            public string SavingAmt { get; set; }
            [DataMember]
            public string ThumbNailImage { get; set; }
            [DataMember]
            public string Quantity { get; set; }
        }
        [DataContract]
        public class Product
        {
            [DataMember]
            public int ID { get; set; }
        }
        [DataContract]
        public class Cartproducts
        {
            [DataMember]
            public string Email { get; set; }
            [DataMember]
            public string CompanyID { get; set; }
            [DataMember]
            public int branchID { get; set; }
            [DataMember]
            public List<Product> products { get; set; }
        }
        [DataContract]
        public class Register
        {
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public string Email { get; set; }
            [DataMember]
            public string Mobile { get; set; }
            [DataMember]
            public string Password { get; set; }
            [DataMember]
            public string Address { get; set; }
            [DataMember]
            public string CompanyID { get; set; }
        }
        [DataContract]
        public class OrderHistory
        {
            [DataMember]
            public string ShipTo { get; set; }
            [DataMember]
            public decimal Total { get; set; }
            [DataMember]
            public string OrderNo { get; set; }
            [DataMember]
            public string OrderPlacedDate { get; set; }
            [DataMember]
            public string Status { get; set; }
            [DataMember]
            public string DeliveryDateTime { get; set; }
            [DataMember]
            public string ItemName { get; set; }
            [DataMember]
            public string SoldBy { get; set; }
            [DataMember]
            public string Price { get; set; }
            [DataMember]
            public string Quantity { get; set; }
            [DataMember]
            public string ThumbNailImage { get; set; }
        }
        [DataContract]
        public class CheckOut
        {
            [DataMember]
            public CheckOutDetails checkoutdetails;

            public CheckOut()
            {
                checkoutdetails = new CheckOutDetails();

            }
        }
        public class TimeSlot
        {
            public int key { get; set; }
            public string value { get; set; }
        }
        [DataContract]
        public class CheckOutDetails
        {
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public string Email { get; set; }
            [DataMember]
            public string Mobile { get; set; }
            [DataMember]
            public string Address { get; set; }
            [DataMember]
            public string Deliverydate { get; set; }
            [DataMember]
            public List<TimeSlot> TimeSlot { get; set; }
            [DataMember]
            public List<products> COproducts { get; set; }
        }
        [DataContract]
        public class PlacedOrder
        {
            [DataMember]
            public string User_Id { get; set; }
            [DataMember]
            public string BranchCode { get; set; }
            [DataMember]
            public string CompanyID { get; set; }
            [DataMember]
            public CheckOutDetails checkoutdetails;
            public PlacedOrder()
            {
                checkoutdetails = new CheckOutDetails();
            }
        }
    }
}