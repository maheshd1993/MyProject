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
    
    public partial class crm_usertbl
    {
        public int Id { get; set; }
        public Nullable<int> ByUID { get; set; }
        public string UserName { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TimeZone { get; set; }
        public string ProfileName { get; set; }
        public string ProfileId { get; set; }
        public Nullable<bool> Status { get; set; }
        public string MappedUsers { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<decimal> CasualLeave { get; set; }
        public Nullable<decimal> MedicalLeave { get; set; }
        public string Year { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> BranchID { get; set; }
        public string FatherName { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public Nullable<int> Gender { get; set; }
        public string ContactNumber { get; set; }
        public string AlternateNumber { get; set; }
        public string EmployeeCode { get; set; }
        public string Designation { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string RefName1 { get; set; }
        public string RefPhoneNumber1 { get; set; }
        public string RefEmail1 { get; set; }
        public string RefName2 { get; set; }
        public string RefPhoneNumber2 { get; set; }
        public string RefEmail2 { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<bool> IsExpired { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string KeyVersion { get; set; }
        public Nullable<int> EscalateUserId { get; set; }
        public float EscalateTime { get; set; }
        public Nullable<int> t_LoginId { get; set; }
        public string Aadhar_No { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public Nullable<int> CompanyTypeId { get; set; }
        public string EmployeType { get; set; }
        public string Customergroupid { get; set; }
    }
}
