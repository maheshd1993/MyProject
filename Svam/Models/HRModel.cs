using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Traders.Models
{
    public class HRModel
    {

    }
    public class CreateInterViewSchduleModel
    {
        [Required(ErrorMessage = "* Please Enter Candidate Name")]
        public string CandidateName { get; set; }
        [Required(ErrorMessage = "*  Please Enter e-mail")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "* E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "* Please Contact Number")]
        public string MobileNo { get; set; }
        public string Telephone { get; set; }
        public string DateFormat  { get; set; }
        public string DOB { get; set; }
        public string PostalAddress { get; set; }
        public string TotalExp { get; set; }
        public string CurrentLocation { get; set; }
        public string PreferredLocation { get; set; }
        public string FollowUpDate { get; set; }
        public string InterviewDate { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string ResumeTitle { get; set; }
        public string CurrentDesignation { get; set; }
        public string CurrentEmployer { get; set; }
        public string AnnualSalary { get; set; }
        public string UGCourses { get; set; }
        public string PGCourses { get; set; }
        public string PPGCourses { get; set; }
        public string LastActivateDate { get; set; }

        [Required(ErrorMessage = "* Please Select Profile")]
        public int? ProfileID { get; set; }
        public string Profile { get; set; }
        [Required(ErrorMessage = "* Please Select Status")]
        public int InterviewStatusID { get; set; }
        public string InterviewStatusName { get; set; }
        public List<CreateInterViewSchduleModel> ProfileList { get; set; }
        public List<CreateInterViewSchduleModel> InterviewStatusList { get; set; }
    }

    public class JobProfileModel
    {
        public int Id { get; set; }
        public string Profile { get; set; }
        public bool Status  { get; set; }
    }

    public class ViewInterviewSchedule
    {
        public Int64 Id { get; set; }
        public string CandidateName { get; set; }
        public string DateFormat  { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Experiance { get; set; }
        public string ResumeID { get; set; }
        public string ResumeTitle { get; set; }
        public string CurrentLocation { get; set; }
        public string CurrentDesignation { get; set; }
        public string CurrentEmployer { get; set; }
        public string AnnualSalry { get; set; }
        public string UGCourses { get; set; }
        public string PGCourses { get; set; }
        public string PPGCourses { get; set; }
        public string ProfileName { get; set; }
        public Int32 ProfileId { get; set; }
        public string FollowUpDate { get; set; }
        public string InterviewDate { get; set; }
        public string InterviewStatus { get; set; }
        public string CurrentStatus { get; set; }
        public string CreatedDate { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public SelectList InterviewStatusList { get; set; }
        public List<ViewInterviewSchedule> ViewinterviewSchedulemodelList = new List<ViewInterviewSchedule>();
        public List<ViewInterviewSchedule> NewCandidatemodelList = new List<ViewInterviewSchedule>();
        public List<ViewInterviewSchedule> NextweekinterviewSchedulemodelList = new List<ViewInterviewSchedule>();
        public List<ViewInterviewSchedule> TodaysfollowupmodelList = new List<ViewInterviewSchedule>();
        public List<ViewInterviewSchedule> TodaysInterviewmodelList = new List<ViewInterviewSchedule>();
    }

    public class SalaryModel 
    {      
        public int SalaryID { get; set; }
        public Nullable<int> UserID { get; set; }
        public String BankName { get; set; }
        public String BranchName { get; set; }
        public String AccountNumber { get; set; }
        public string DateFormat { get; set; }
        public String Month { get; set; }
        public Nullable<Decimal> MonthlySalary { get; set; }
        public Nullable<Decimal> AnnualSalary { get; set; }
        public Nullable<Decimal> BasicSalary { get; set; }
        public Nullable<Decimal> HRA { get; set; }
        public Nullable<Decimal> TravellingAllowance { get; set; }
        public Nullable<Decimal> MedicalAllowance { get; set; }
        public Nullable<Decimal> PerformanceIncentive { get; set; }
        public Nullable<Decimal> OtherBenefits { get; set; }
        public Nullable<Decimal> PFEmployeeShare { get; set; }
        public Nullable<Decimal> PFEmployerShare { get; set; }
        public Nullable<Decimal> ESICEmployerEmployee { get; set; }
        public Nullable<Decimal> TDS { get; set; }
        public Nullable<Decimal> OtherDeduction { get; set; }
        public string IFSCCode { get; set; }
        public string LWF { get; set; }
        public string Security { get; set; }
        public string Advance { get; set; }
        public string LWP { get; set; }

        /// <summary>
        /// Logical Field
        /// </summary>
        public String FullName { get; set; }
        public String EmployeeCode { get; set; }
        public String EmployeeEmail { get; set; }
        public Decimal TotalSalary { get; set; }

        public String NetPayAmount { get; set; }
        public String NetPayWord { get; set; }
        public String Designation { get; set; }
        public String SalerySlipName { get; set; }
        public decimal PaySalary { get; set; }
        public List<SalaryModel> EmployeeList { get; set; }
        public List<SalaryModel> SaleryHistoryList { get; set; }
    }

    public class AdvanceModel 
    {
        public Int64? AdvanceID { get; set; }
        public Nullable<Int64> UserID { get; set; }
        public String FullName { get; set; }
        public String EmployeeCode { get; set; }
        public String EmployeeEmail { get; set; }
        public String Month { get; set; }     
        public Decimal? AdvanceAmount { get; set; }
        public String CreatedOn { get; set; }
        public String ModifiedOn { get; set; } 
        public string DateFormat { get; set; }
        public List<AdvanceModel> EmployeeList { get; set; }
        public List<AdvanceModel> AdvanceModelList { get; set; }
    }

    public class ManualAttendanceModel 
    {
        public Int64? ManualID { get; set; }
        public Nullable<Int32> EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public String HRName { get; set; }
        public String AttendanceDate { get; set; }
        public String LoginTime { get; set; }
        public String LogoutTime { get; set; }
        public String Duration { get; set; }
        public string DateFormat  { get; set; }
        public List<ManualAttendanceModel> EmployeeList { get; set; }
        public List<ManualAttendanceModel> manualAttendanceModelList { get; set; }
    }
}