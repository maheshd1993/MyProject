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
    
    public partial class crm_workassigntbl
    {
        public long Id { get; set; }
        public Nullable<int> UID { get; set; }
        public Nullable<System.DateTime> WorkCreateDate { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> ModuleId { get; set; }
        public string ModuleDescription { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public Nullable<int> WorkAssignTo { get; set; }
        public Nullable<System.DateTime> WorkAssignDate { get; set; }
        public Nullable<int> WorkAssignBy { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> WorkStatus { get; set; }
        public Nullable<System.DateTime> WorkCompletedDate { get; set; }
        public Nullable<int> WorkCompletedBy { get; set; }
        public string CustomerName { get; set; }
        public string Remarks { get; set; }
        public string WorkAttachment { get; set; }
        public bool CancelStatus { get; set; }
        public string ManualCustomerName { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string FollowUpTime { get; set; }
        public Nullable<System.DateTime> FollowDate { get; set; }
    }
}
