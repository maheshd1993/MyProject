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
    
    public partial class com_city_live
    {
        public int ID { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
