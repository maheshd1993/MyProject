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
    
    public partial class subscriptions_payment_history
    {
        public int SPH { get; set; }
        public string CompanyId { get; set; }
        public string UserId { get; set; }
        public string plans { get; set; }
        public string Amount { get; set; }
        public string txnid { get; set; }
        public string PaymentMode { get; set; }
        public string name_on_card { get; set; }
        public string issuing_bank { get; set; }
        public string bank_ref_num { get; set; }
        public string GST_IndentificationNo { get; set; }
        public string Organization { get; set; }
        public string Address { get; set; }
        public string AadharNo { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string bankcode { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string Subscription_StartDate { get; set; }
        public string Subscription_EndDate { get; set; }
        public string response { get; set; }
        public string Status { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
