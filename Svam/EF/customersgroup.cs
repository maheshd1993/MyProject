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
    
    public partial class customersgroup
    {
        public int ID { get; set; }
        public string CustGroupName { get; set; }
        public string Remarks { get; set; }
        public int CompanyId { get; set; }
        public int BranchCode { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Flag { get; set; }
        public string SyncID { get; set; }
    }
}