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
    
    public partial class debitnote
    {
        public int id { get; set; }
        public string DebitNo { get; set; }
        public string DebitDt { get; set; }
        public string SuppNm { get; set; }
        public string SuppCd { get; set; }
        public string Mobile { get; set; }
        public string RefrenceNo { get; set; }
        public string ReferenceDt { get; set; }
        public string Remarks { get; set; }
        public string TerBill { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
