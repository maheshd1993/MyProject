using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Traders.Models
{
    public class NisDeveloperModel
    {
        [Required(ErrorMessage="*")]
        public string Project { get; set; }
        public string ProjectModule { get; set; }
        [AllowHtml]
        public string GeneralRemark { get; set; }
        [AllowHtml]
        public string CodeModuleRemark { get; set; }
        [AllowHtml]
        public string DBModuleRemark { get; set; }
        [AllowHtml]
        public string JSCSSModuleRemark { get; set; }
        public string SupportNeeded { get; set; }

    }

    public class AddProjectModel
    {
        [Required(ErrorMessage="*")]
        public Int64 Pid { get; set; }
        public string ProjectName { get; set; }
        public string IsActive { get; set; }

    }

    public class AddCustomerModel
    {
        public long CId { get; set; }
        public string CustomerName { get; set; }
        public List<AddCustomerModel> customerinfolist = new List<AddCustomerModel>();
    }

    public class AddProjectStatusModel
    {
        public long Id { get; set; }
        public string ProjectStatusName { get; set; }
    }

    public class AddProjectModuleModel
    {
        [Required(ErrorMessage = "*")]
        public string ProjectModuleName { get; set; }
        public int ProjectId { get; set; }
        public int ModuleId { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public class DeveloperActivityModel
    {
        public Int64 Id { get; set; }
        public int Uid { get; set; }
        public Int64 Project_ID { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode  { get; set; }
        [AllowHtml]
        public string GeneralRemark { get; set; }
        public int ProjectModule_ID { get; set; }
        [AllowHtml]
        public string CodeModuleRemark { get; set; }
        [AllowHtml]
        public string DBModuleRemark { get; set; }
        [AllowHtml]
        public string JsModuleRemark { get; set; }
        public string SupportNeeded { get; set; }
        public string CodeFile { get; set; }
        public string CreatedDate { get; set; }
        public string ProjectName { get; set; }
        public string ModuleName { get; set; }
        public string DateFormat { get; set; }
        public List<Userddl> DeveloperList { get; set; }
        public List<DeveloperActivityModel> DeveloperactivityModelList = new List<DeveloperActivityModel>();
    }

    public class GeneralRemarkModel
    {
        [Required(ErrorMessage="*")]
        [AllowHtml]
        public string Remark { get; set; }
    }
    public class CommonActivityRemarkModel
    {
        public int Id { get; set; }
        public int UID { get; set; }
        public string UserName { get; set; }
        public string Profile { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public string Date { get; set; }
        public string DateFormat { get; set; }
        public string UserPicture { get; set; }
        public List<CommonActivityRemarkModel> commonActivityRemarkList = new List<CommonActivityRemarkModel>();
    }

    public class DailyworkAssignDeveloperModel
    {
        public Int64 Id { get; set; }
        public string AssignDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ModuleId { get; set; }
        public string DateFormat { get; set; }
        public string ModuleName { get; set; }
        [AllowHtml]
        public string ModuleDescription { get; set; }
        public string FinishingDate { get; set; }
        public string UserName { get; set; }
        public string WorkCreatedDate { get; set; }
        public string WorkCreatedBy { get; set; }
        public string WorkAssignTo { get; set; }
        public string WorkAssignedUser { get; set; }
        public string TaskCompletedDate { get; set; }
        public int WorkStatus { get; set; }
        public string WorkStatusName { get; set; }
        public int OtherUserID { get; set; }
        public string CustomerName { get; set; }
        public string CID { get; set; }
        public int CancelStatus { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string CustomerId { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public List<CreateUserModel> AssignToList { get; set; }

        public List<DailyworkAssignDeveloperModel> dailyworkAssignDelevelopermodelList = new List<DailyworkAssignDeveloperModel>();
    }
    public class WorkDescrpitonModel
    {
        public Int64 Id { get; set; }
        public Int64 WorkId { get; set; }
        public int WorkStatus { get; set; }
        public string Description { get; set; }
        public string WorkAttachment { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
    }

    public class ViewworkAssignDeveloperModel
    {
        public Int64 Id { get; set; }
        public string AssignDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ModuleId { get; set; }
        public string DateFormat { get; set; }
        public string ModuleName { get; set; }
        [AllowHtml]
        public string ModuleDescription { get; set; }
        public string FinishingDate { get; set; }
        public string UserName { get; set; }
        public string WorkCreatedDate { get; set; }
        public string WorkCreatedBy { get; set; }
        public string WorkAssignTo { get; set; }
        public string WorkAssignedUser { get; set; }
        public string TaskCompletedDate { get; set; }
        public int WorkStatus { get; set; }
        public string WorkStatusName { get; set; }
        public int OtherUserID { get; set; }
        public string CustomerName { get; set; }
        public int CancelStatus { get; set; }

        public List<ViewworkAssignDeveloperModel> ViewworkAssignDevelopermodelList = new List<ViewworkAssignDeveloperModel>();
        public List<projectddl> projectddlist;
    }
}