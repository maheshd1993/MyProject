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
    
    public partial class t_login
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string user_password { get; set; }
        public int role_id { get; set; }
        public string role_name { get; set; }
        public Nullable<int> UserRole { get; set; }
        public string Emp_code { get; set; }
        public string Emp_name { get; set; }
        public Nullable<int> Mob_no { get; set; }
        public string branch { get; set; }
        public int company_id { get; set; }
        public string customer_id { get; set; }
        public string last_login { get; set; }
        public string Subscription_StartDate { get; set; }
        public string Subscription_EndDate { get; set; }
        public string PaymentStatus { get; set; }
        public string connect_to_support { get; set; }
        public string SubscId { get; set; }
        public string txnid { get; set; }
        public string PaymentDate { get; set; }
        public string paymentstate { get; set; }
        public Nullable<decimal> Totalamount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string IsActive { get; set; }
        public string flag { get; set; }
        public string sub_code { get; set; }
        public string SyncID { get; set; }
        public string permissions { get; set; }
        public Nullable<int> mastercompany { get; set; }
        public string KeyVersion { get; set; }
        public string planStatus { get; set; }
        public string crm_token { get; set; }
        public Nullable<System.DateTime> crm_token_cr_time { get; set; }
        public string user_type { get; set; }
        public string customer_groups { get; set; }
    }
}
