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
    
    public partial class promotions_coupon
    {
        public int POID { get; set; }
        public string Pro_Code { get; set; }
        public string Pro_Description { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public decimal pro_discount { get; set; }
        public Nullable<long> max_user { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string IsActive { get; set; }
        public string SyncID { get; set; }
    }
}