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
    
    public partial class consignee
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public int BranchCode { get; set; }
        public string CustomerID { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeMobileNo { get; set; }
        public string ConsigneeEmailId { get; set; }
        public string ConsigneeAadharNo { get; set; }
        public string ConsigneePanNo { get; set; }
        public string ConsigneeGSTIN { get; set; }
        public string ConsigneeAddress { get; set; }
        public Nullable<int> ConsigneeCountry { get; set; }
        public Nullable<int> ConsigneeCity { get; set; }
        public Nullable<int> ConsigneeState { get; set; }
        public string ConsigneeAddressOther { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Flag { get; set; }
        public string SyncID { get; set; }
        public string pincode { get; set; }
    }
}
