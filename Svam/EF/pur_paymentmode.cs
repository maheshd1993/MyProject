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
    
    public partial class pur_paymentmode
    {
        public int id { get; set; }
        public string GRN { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<long> Transactionamt { get; set; }
        public Nullable<long> Bankchrgs { get; set; }
        public string Particular { get; set; }
        public Nullable<System.DateTime> Dt { get; set; }
        public string BankNm { get; set; }
        public string BranchNm { get; set; }
        public string CardType { get; set; }
        public string CardTransactionNo { get; set; }
        public string E1 { get; set; }
        public string E2 { get; set; }
        public string E3 { get; set; }
        public string E4 { get; set; }
        public string SyncID { get; set; }
    }
}
