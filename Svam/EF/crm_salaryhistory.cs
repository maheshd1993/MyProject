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
    
    public partial class crm_salaryhistory
    {
        public int ID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string Month { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HRA { get; set; }
        public Nullable<decimal> TravellingAllowance { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> PerformanceIncentive { get; set; }
        public Nullable<decimal> OtherBenefits { get; set; }
        public Nullable<decimal> PFEmployeeShare { get; set; }
        public Nullable<decimal> PFEmployerShare { get; set; }
        public Nullable<decimal> ESICEmployerEmployee { get; set; }
        public Nullable<decimal> TDS { get; set; }
        public Nullable<decimal> OtherDeduction { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string SalarySlipName { get; set; }
        public Nullable<decimal> PaySalary { get; set; }
        public string IFCSCode { get; set; }
        public string LWF { get; set; }
        public string Security { get; set; }
        public string Advance { get; set; }
        public string LWP { get; set; }
    }
}
