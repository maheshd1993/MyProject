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
    
    public partial class pur_supplierdeposit
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> DepositDate { get; set; }
        public string SupplierID { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<System.DateTime> ReferenceDate { get; set; }
        public string Remarks { get; set; }
        public string TermID { get; set; }
        public Nullable<decimal> DepositAmount { get; set; }
        public Nullable<decimal> NetReceivable { get; set; }
        public Nullable<decimal> NetReceived { get; set; }
        public Nullable<decimal> CurrentDue { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string SyncID { get; set; }
    }
}