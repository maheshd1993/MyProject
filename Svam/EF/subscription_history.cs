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
    
    public partial class subscription_history
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string BranchCode { get; set; }
        public string TableType { get; set; }
        public string ChalanNo { get; set; }
        public string SubStartDate { get; set; }
        public string SubEndDate { get; set; }
        public string SubByUserId { get; set; }
        public string SubByCompanyId { get; set; }
        public string SubByBranchId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string SyancId { get; set; }
        public Nullable<int> Flag { get; set; }
    }
}
