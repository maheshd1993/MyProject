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
    
    public partial class crm_fbleadadsmaster
    {
        public long Id { get; set; }
        public string Page_Name { get; set; }
        public string Page_Id { get; set; }
        public string Access_Token { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> Createddate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public bool IsActive { get; set; }
    }
}
