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
    
    public partial class pur_returnlines
    {
        public int ID { get; set; }
        public string ReturnNo { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public Nullable<decimal> ReturnQty { get; set; }
        public Nullable<long> BalanceQty { get; set; }
        public string Barcode { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> LinePrice { get; set; }
        public Nullable<decimal> DisPer { get; set; }
        public Nullable<decimal> DisAmt { get; set; }
        public Nullable<decimal> LineSubTotal { get; set; }
        public decimal SerTPer { get; set; }
        public Nullable<decimal> SerTAmt { get; set; }
        public Nullable<decimal> ExciseDPer { get; set; }
        public Nullable<decimal> ExciseDAmt { get; set; }
        public Nullable<decimal> VATPer { get; set; }
        public Nullable<decimal> VATAmt { get; set; }
        public Nullable<decimal> SATPer { get; set; }
        public Nullable<decimal> SATamt { get; set; }
        public Nullable<decimal> PkgChrg { get; set; }
        public Nullable<decimal> DlvChrg { get; set; }
        public Nullable<decimal> ExtendedPrice { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Location { get; set; }
        public string UpdatedInStock { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int BranchCode { get; set; }
        public string Remarks { get; set; }
        public string StatusCode { get; set; }
        public int CompanyId { get; set; }
        public Nullable<decimal> PurCessPer { get; set; }
        public Nullable<decimal> PurCessAmount { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
        public string colorID { get; set; }
        public string SizeID { get; set; }
    }
}
