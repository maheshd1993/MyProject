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
    
    public partial class purchase_requisition_tbl
    {
        public long Id { get; set; }
        public Nullable<int> UID { get; set; }
        public string PRDate { get; set; }
        public string TxNo { get; set; }
        public string TxType { get; set; }
        public string Department { get; set; }
        public string Requester { get; set; }
        public string RequestTO { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Remarks { get; set; }
        public string GenerateStatus { get; set; }
        public string SyncID { get; set; }
    }
}