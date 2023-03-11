using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Traders.Models
{
    public class EmployeeAttandaceReportModel
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int PresentOnTime { get; set; }
        public int PresentOFFTime { get; set; }
        public int ExtraHoursDay { get; set; }
        public int ExtraWorkingDayWith9hours { get; set; }
        public int ExtraWorkingDayLess9hours { get; set; }
        public int SatAndSun { get; set; }
        public int CasualLeave { get; set; }
        public int MedicalLeave { get; set; }
        public int PaidLeave { get; set; }
        public int Absent { get; set; }
        public int Total { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DateFormat { get; set; }
        public Nullable<Int32> CompanyTypeID { get; set; }
        public string CompanyTypeName { get; set; }
        public string CompanyName { get; set; }


        public List<EmployeeAttandaceReportModel> EmployeeAttandaceReportModelList = new List<EmployeeAttandaceReportModel>();
        public List<EmpAttandanceRepotModel> EmpAttandanceRepotModelList = new List<EmpAttandanceRepotModel>();
        public List<EmployeeAttandaceReportModel> CompanyTypeList { get; set; }

    }

    public class EmpAttandanceRepotModel
    {
        public Int32 EmpId { get; set; }
        public string LoginDate { get; set; }
        public string LoginTime { get; set; }
        public string LogoutDate { get; set; }
        public string LogoutTime { get; set; }
        public string WorkDuration { get; set; }
        public bool WorkingLateNight { get; set; }
        public bool ExtraWorking { get; set; }
        public string LogZoneTime { get; set; }
        public string CompanyName { get; set; }
        public Int32 CompanyTypeId { get; set; }
    }

    public class LeaveRequestModel
    {
        public Int64 RequestID { get; set; }
        [Required(ErrorMessage = "Please Select Employee")]
        public Int32? EmployeeID { get; set; }
      
        [Required(ErrorMessage="* Please Enter Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "* Please Enter Message")]
        [AllowHtml]
        public string Message { get; set; }
        public string DateFormat  { get; set; }
        public string RequestDate { get; set; }
        public string ProcessStatus { get; set; }
        public string ProcessDate { get; set; }
        [AllowHtml]
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Please Select Leave Type")]
        public int? LeaveTypeID { get; set; }
        public string LeaveName { get; set; }

        public List<LeaveRequestModel> LeaveEmployeeList { get; set; }
        public List<LeaveRequestModel> EmployeeList { get; set; }
        public List<LeaveRequestModel> LeaveTypeList { get; set; }
    }

    public class Form16RequestModel
    {
        public Int64 RequestID { get; set; }
        [Required(ErrorMessage = "Please Select Employee")]
        public Int32? EmployeeID { get; set; }

        //[Required(ErrorMessage = "* Please Enter Subject")]
        //public string Subject { get; set; }
        //[Required(ErrorMessage = "* Please Enter Message")]
        //[AllowHtml]
        public string Message { get; set; }
        public string DateFormat { get; set; }
        public string RequestDate { get; set; }
        public string ProcessStatus { get; set; }
        public string ProcessDate { get; set; }
        [AllowHtml]
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
       public HttpPostedFileBase postedFile { get; set; }
        [Required(ErrorMessage = "Please Select Form Type")]
        public int? FormTypeID { get; set; }
        public string FormName { get; set; }

        public List<Form16RequestModel> LeaveEmployeeList { get; set; }
        public List<Form16RequestModel> EmployeeList { get; set; }
        public List<Form16RequestModel> FormTypeList { get; set; }
    }

    public class ExpenseModel
    {
        public Int64 ExpenseID { get; set; }
        [Required(ErrorMessage = "Please Select Employee")]
        public Int32? EmployeeID { get; set; }

        public string DateFormat { get; set; }
        [Required(ErrorMessage = "Please Select Expense Type")]
        public Int64 ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public string travelledKMS { get; set; }
        public string expense { get; set; }
        public string RequestDate { get; set; }
        public string ProcessStatus { get; set; }
        public string ProcessDate { get; set; }
        [AllowHtml]
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        //public string Comment { get; set; }
        public string Receipt { get; set; }
        [Required(ErrorMessage = "Please Select Leave Type")]
        public int? ExpanseTypeID { get; set; }
        public string ExpanseTypeName { get; set; }
        public long TotalExpance { get; set; }
        public List<ExpenseModel> ExpenseEmployeeList { get; set; }
        public List<ExpenseModel> EmployeeList { get; set; }
        public List<ExpenseModel> ExpenseTypeList { get; set; }

    }
}