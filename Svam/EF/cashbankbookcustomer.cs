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
    
    public partial class cashbankbookcustomer
    {
        public int id { get; set; }
        public string CustomerID { get; set; }
        public string EntryNo { get; set; }
        public string CustomerName { get; set; }
        public System.DateTime entrydate { get; set; }
        public string paymentmode { get; set; }
        public string chequeno { get; set; }
        public string chequedate { get; set; }
        public string bankname { get; set; }
        public string narration { get; set; }
        public decimal Pbalance { get; set; }
        public decimal amountpaid { get; set; }
        public decimal currentbalance { get; set; }
        public System.DateTime createddate { get; set; }
        public string createdBy { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
