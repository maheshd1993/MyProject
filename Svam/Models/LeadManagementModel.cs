using Svam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Traders.Models
{
    public class LeadManagementModel
    {
        public Int64 Id { get; set; }
        public string LeadSource { get; set; }
        [Required(ErrorMessage = "*")]
        public string Country { get; set; }
        [Required(ErrorMessage = "*")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "*")]
        public string PrimaryPhNo { get; set; }
        public string SecondaryPhNo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string SkypeId { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string FollowUpDate { get; set; }
        public string TimeZoneName { get; set; }
        public string OtherRemark { get; set; }
        public string LeadStatus { get; set; }
        public string AssignTo { get; set; }
        public string CreatedBy { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public string Created_at { get; set; }

        public List<LeadManagementModel> leadManagementmodelList = new List<LeadManagementModel>();
        public List<MappedUserParentsModel> mapuserParentsModel = new List<MappedUserParentsModel>();
        public List<MapUserModel> mapUserList = new List<MapUserModel>();
    }
    public class MappedUserParentsModel
    {
        public int Id { get; set; }
        public string ParentsName { get; set; }
    }

   
    public class ViewAssignedLeadsModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string FollowUpDate { get; set; }
        public string LeadStatus { get; set; }
        public string AssignToUserName { get; set; }
        public string AssignToUID { get; set; }
        public string AssignByUserName { get; set; }
        public Int32 AssignByUID { get; set; }
        public string AssignDate { get; set; }
        public string LeadOwner { get; set; }        
        public string MappedUser { get; set; }
        public string UserddlName { get; set; }
        public string AssignTo { get; set; }
        public string AssignedBy { get; set; }

        public List<ViewAssignedLeadsModel> ViewassignleadsModelList = new List<ViewAssignedLeadsModel>();
        public List<CreateUserModel> Userddllist { get; set; }
       
    }
   
    public class DashBoardLeadsModel
    {
        public Int64 Id { get; set; }
        public string LeadName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public string FollowUpDate { get; set; }
        public string LeadStatus { get; set; }
        public Int32 LeadOwner { get; set; }
        public Int32 AssignTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<DashBoardLeadsModel> dashboardTodayLeadsList = new List<DashBoardLeadsModel>();
        public List<DashBoardLeadsModel> dashboardWeekLeadsList = new List<DashBoardLeadsModel>();
        public List<DashBoardLeadsModel> dashboardPriorityLeadsList = new List<DashBoardLeadsModel>();
        public List<DashBoardLeadsModel> dashboardTodayFollowUpLeadsList = new List<DashBoardLeadsModel>();
        public List<DashBoardLeadsModel> dashboardTodayDOBMAList = new List<DashBoardLeadsModel>();
        public List<CRMPaymentModel> dashboardTodayPaymentList = new List<CRMPaymentModel>();
    }

    public class DOBModel
    {
        public Int64 Id { get; set; }
        public string LeadName { get; set; }        
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public string FollowUpDate { get; set; }
        public string LeadStatus { get; set; }
        public Int32 LeadOwner { get; set; }
        public Int32 AssignTo { get; set; }
        public string DateofBirth { get; set; }
        public string MarriageAnniversary { get; set; }
        public string NickName { get; set; }
    }

    public class DashBoardLeadsApiModel
    {
        public Int64 Id { get; set; }
        public string LeadName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public string FollowUpDate { get; set; }
        public string LeadStatus { get; set; }
        public Int32 LeadOwner { get; set; }
        public Int32 AssignTo { get; set; }
    }

    public class DashBoardData
    {
        public List<DashBoardLeadsApiModel> dashboardDataList = new List<DashBoardLeadsApiModel>();
    }
}