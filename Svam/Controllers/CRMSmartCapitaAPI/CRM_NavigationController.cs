using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_NavigationController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        public async Task<HttpResponseMessage> GetMenuPermissions(int? CompanyID, int? BranchID, int? UID, string ProfileName)
        {
            var model = new List<MenuPermissionsModel>();
            string message = string.Empty;
            try
            {
                if (CompanyID == null || CompanyID == 0)
                {
                    message = "Please pass company id parameter value";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                if (BranchID == null || BranchID == 0)
                {
                    message = "Please pass branch id parameter value";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                if (UID == null || UID == 0)
                {
                    message = "Please pass uid parameter value";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                if (string.IsNullOrEmpty(ProfileName))
                {
                    message = "Please pass profile name parameter value";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                var GetData = new MenuPermissionsModel();
                if (ProfileName == "SuperAdmin")
                {
                    //model.Add(new MenuPermissionsModel { MenuName = "Masters", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Products/Services", IsVisible = true });                    
                    //model.Add(new MenuPermissionsModel { MenuName = "Lead Management", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Lead Form", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Leads View Report", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Lead Status", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Lead Source", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Ticket Management", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Ticket Form", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Tickets View", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Error Types", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Urgency Types", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "HR Management", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Employee Profiles", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Leave Names", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Interview Status", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "E-mail", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Templates", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "E-mail setup", IsVisible = true });
                    GetData.Masters = true;
                    GetData.ProductsServices = true;
                    GetData.LeadManagement = true;
                    GetData.LeadForm = true;
                    GetData.LeadsViewReport = true;
                    GetData.LeadStatus = true;
                    GetData.LeadSource = true;
                    GetData.TicketManagement = true;
                    GetData.TicketForm = true;
                    GetData.TicketsView = true;
                    GetData.ErrorTypes = true;
                    GetData.UrgencyTypes = true;
                    GetData.HRManagement = true;
                    GetData.EmployeeProfiles = true;
                    GetData.LeaveNames = true;
                    GetData.InterviewStatus = true;
                    GetData.Email = true;
                    GetData.Templates = true;
                    GetData.Emailsetup = true;
                    GetData.ProjectStatus = true;
                    if (db.company_profile.Any(a => a.ID == CompanyID && a.Country == "1"))//check if country id 1 of that company then india mart setting
                    {
                        //model.Add(new MenuPermissionsModel { MenuName = "IndiaMart Setting", IsVisible = true });
                        GetData.IndiaMartSetting = true;
                    }
                    else
                    {
                        //model.Add(new MenuPermissionsModel { MenuName = "IndiaMart Setting", IsVisible = false });
                        GetData.IndiaMartSetting = false;
                    }
                    //model.Add(new MenuPermissionsModel { MenuName = "Leads", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Create New Lead", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Leads", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Lead Summary", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Leads assigned to me", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "File Manager", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Sales", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Sales", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Sales Target And Performance", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Track Sales Team", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Map view–Sales Team", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Payments", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "General", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Create Daily Remark", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Daily Remark", IsVisible = true });
                    GetData.Leads = true;
                    GetData.CreateNewLead = true;
                    GetData.ViewLeads = true;
                    GetData.LeadSummary = true;
                    GetData.Leadsassignedtome = true;
                    GetData.FileManager = true;
                    GetData.Sales = true;
                    GetData.ViewSales = true;
                    GetData.SalesTargetAndPerformance = true;
                    GetData.TrackSalesTeam = true;
                    GetData.MapviewSalesTeam = true;
                    GetData.Payments = true;
                    GetData.General = true;
                    GetData.CreateDailyRemark = true;
                    GetData.ViewDailyRemark = true;
                    if (CompanyID == 296)//show these menus for smart capita Arunaw sir company only
                    {
                        //model.Add(new MenuPermissionsModel { MenuName = "Developers", IsVisible = true });
                        //model.Add(new MenuPermissionsModel { MenuName = "Create Activity", IsVisible = true });
                        //model.Add(new MenuPermissionsModel { MenuName = "View Activity", IsVisible = true });
                        GetData.Developers = true;
                        GetData.CreateActivity = true;
                        GetData.ViewActivity = true;
                    }
                    else
                    {
                        //model.Add(new MenuPermissionsModel { MenuName = "Developers", IsVisible = false });
                        //model.Add(new MenuPermissionsModel { MenuName = "Create Activity", IsVisible = false });
                        //model.Add(new MenuPermissionsModel { MenuName = "View Activity", IsVisible = false });
                        GetData.Developers = false;
                        GetData.CreateActivity = false;
                        GetData.ViewActivity = false;
                    }
                    //model.Add(new MenuPermissionsModel { MenuName = "Projects", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Project", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Create and Assign Work", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Assigned Work", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Users", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Manage Users", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Manage Roles", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "HR", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Manage Employees", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Manage Salary", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Manage Extra Payment", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Employee Login History", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Manual Attendance", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Employee Leave Requests", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Create Interview Schedule", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Interview Schedule", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Tickets", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Create Ticket", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Tickets", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "My Space", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Attendance", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "Apply Leave", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Leaves", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "My Team", IsVisible = true });
                    //model.Add(new MenuPermissionsModel { MenuName = "View Assigned Work", IsVisible = true });

                    GetData.Projects = true;
                    GetData.Project = true;
                    GetData.CreateandAssignWork = true;
                    GetData.ViewAssignedWork = true;
                    GetData.Users = true;
                    GetData.ManageUsers = true;
                    GetData.ManageRoles = true;
                    GetData.HR = true;
                    GetData.ManageEmployees = true;
                    GetData.ManageSalary = true;
                    GetData.ManageExtraPayment = true;
                    GetData.EmployeeLoginHistory = true;
                    GetData.ManualAttendance = true;
                    GetData.EmployeeLeaveRequests = true;
                    GetData.CreateInterviewSchedule = true;
                    GetData.ViewInterviewSchedule = true;
                    GetData.Tickets = true;
                    GetData.CreateTicket = true;
                    GetData.ViewTickets = true;
                    GetData.MySpace = true;
                    GetData.Attendance = true;
                    GetData.ApplyLeave = true;
                    GetData.ViewLeaves = true;
                    GetData.ViewLeaves = true;
                    GetData.MyTeam = true;
                    GetData.ViewAssignedWork = true;

                    model.Add(GetData);//add all the menu permissions
                }
                else
                {
                    var userdata = await db.crm_usertbl.Where(a => a.Id == UID).FirstOrDefaultAsync();
                    if (userdata != null && !string.IsNullOrEmpty(userdata.ProfileId))
                    {
                        int ProfileId = Convert.ToInt32(userdata.ProfileId);
                        var Rights = await db.crm_roleassigntbl.Where(em => em.Id == ProfileId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                        if (Rights != null)
                        {
                            if ((Rights.IsEmailSetup == true) || (Rights.IsEmailTemplates == true) || (Rights.IsEmployeeProfiles == true) || (Rights.IsErrorTypes == true)
                                 || (Rights.IsIndiaMartSetting == true) || (Rights.IsInterviewStatus == true) || (Rights.IsLeadForm == true) || (Rights.IsLeadSource == true) || (Rights.IsProjectStatus == true)
                                 || (Rights.IsLeadStatus == true) || (Rights.IsLeadsView == true) || (Rights.IsLeaveName == true) || (Rights.IsProductTypes == true) || (Rights.IsTicketForm == true) || (Rights.IsTicketsView == true) || (Rights.IsUrgencyTypes == true))
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Masters", IsVisible = true });
                                GetData.Masters = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Masters", IsVisible = false });
                                GetData.Masters = false;
                            }

                            //model.Add(new MenuPermissionsModel { MenuName = "Products/Services", IsVisible = Rights.IsProductTypes == true ? true : false });
                            GetData.ProductsServices = Rights.IsProductTypes == true ? true : false;
                            if (Rights.IsLeadForm == true || Rights.IsLeadsView == true || Rights.IsLeadSource == true || Rights.IsLeadStatus == true || Rights.IsProjectStatus == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Lead Management", IsVisible = true });
                                GetData.LeadManagement = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Lead Management", IsVisible = false });
                                GetData.LeadManagement = true;
                            }


                            //model.Add(new MenuPermissionsModel { MenuName = "Lead Form", IsVisible = Rights.IsLeadForm == true ? true : false });
                            GetData.LeadForm = Rights.IsLeadForm == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Leads View Report", IsVisible = Rights.IsLeadsView == true ? true : false });
                            GetData.LeadsViewReport = Rights.IsLeadsView == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Lead Status", IsVisible = Rights.IsLeadStatus == true ? true : false });
                            GetData.LeadStatus = Rights.IsLeadStatus == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Lead Source", IsVisible = Rights.IsLeadSource == true ? true : false });
                            GetData.LeadSource = Rights.IsLeadSource == true ? true : false;
                            GetData.ProjectStatus = Rights.IsProjectStatus== true ? true : false;
                            if (Rights.IsTicketForm == true || Rights.IsTicketsView == true || Rights.IsUrgencyTypes == true || Rights.IsErrorTypes == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Ticket Management", IsVisible = true });
                                GetData.TicketManagement = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Ticket Management", IsVisible = false });
                                GetData.TicketManagement = false;
                            }

                            //model.Add(new MenuPermissionsModel { MenuName = "Ticket Form", IsVisible = Rights.IsTicketForm == true? true:false });
                            GetData.TicketForm = Rights.IsTicketForm == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Tickets View", IsVisible = Rights.IsTicketsView == true? true:false });
                            GetData.TicketsView = Rights.IsTicketsView == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Error Types", IsVisible = Rights.IsErrorTypes == true? true:false });
                            GetData.ErrorTypes = Rights.IsErrorTypes == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Urgency Types", IsVisible = Rights.IsUrgencyTypes == true? true:false });
                            GetData.UrgencyTypes = Rights.IsUrgencyTypes == true ? true : false;
                            if (Rights.IsEmployeeProfiles == true || Rights.IsLeaveName == true || Rights.IsInterviewStatus == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "HR Management", IsVisible = true });
                                GetData.HRManagement = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "HR Management", IsVisible = false });
                                GetData.HRManagement = true;
                            }

                            //model.Add(new MenuPermissionsModel { MenuName = "Employee Profiles", IsVisible = Rights.IsEmployeeProfiles == true? true:false });
                            GetData.EmployeeProfiles = Rights.IsEmployeeProfiles == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Leave Names", IsVisible = Rights.IsLeaveName == true? true:false });
                            GetData.LeaveNames = Rights.IsLeaveName == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Interview Status", IsVisible = Rights.IsInterviewStatus == true? true:false });
                            GetData.InterviewStatus = Rights.IsInterviewStatus == true ? true : false;
                            if (Rights.IsEmailTemplates == true || Rights.IsEmailSetup == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "E-mail", IsVisible = true });
                                GetData.Email = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "E-mail", IsVisible = false });
                                GetData.Email = false;
                            }
                            // model.Add(new MenuPermissionsModel { MenuName = "Templates", IsVisible = Rights.IsEmailTemplates == true? true:false });
                            GetData.Templates = Rights.IsEmailTemplates == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "E-mail setup", IsVisible = Rights.IsEmailSetup == true? true:false });
                            GetData.Emailsetup = Rights.IsEmailSetup == true ? true : false;
                            if (db.company_profile.Any(a => a.ID == CompanyID && a.Country == "1"))//check if country id 1 of that company then india mart setting
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "IndiaMart Setting", IsVisible = Rights.IsIndiaMartSetting == true? true:false });
                                GetData.IndiaMartSetting = Rights.IsIndiaMartSetting == true ? true : false;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "IndiaMart Setting", IsVisible = false });
                                GetData.IndiaMartSetting = false;
                            }
                            if ((Rights.ViewLeads == true) || (Rights.CreateLeads == true) || (Rights.LeadNotify == true) || (Rights.AssignLeadManagement == true) || (Rights.FileManager == true))
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Leads", IsVisible = true });
                                GetData.Leads = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Leads", IsVisible = false });
                                GetData.Leads = false;
                            }

                            //model.Add(new MenuPermissionsModel { MenuName = "Create New Lead", IsVisible = Rights.CreateLeads==true? true:false });
                            GetData.CreateNewLead = Rights.CreateLeads == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Leads", IsVisible = Rights.ViewLeads == true? true:false });
                            GetData.ViewLeads = Rights.ViewLeads == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Lead Summary", IsVisible = Rights.LeadNotify == true? true:false });
                            GetData.LeadSummary = Rights.LeadNotify == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Leads assigned to me", IsVisible = Rights.AssignLeadManagement == true? true:false });
                            GetData.Leadsassignedtome = Rights.AssignLeadManagement == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "File Manager", IsVisible = Rights.FileManager == true? true:false });
                            GetData.FileManager = Rights.FileManager == true ? true : false;
                            if (Rights.SaleTarget == true || (Rights.ViewSales == true) || (Rights.TrackSalePerson == true) || (Rights.ViewMapSalePerson == true))
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Sales", IsVisible = true });
                                GetData.Sales = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Sales", IsVisible = false });
                                GetData.Sales = false;
                            }
                            //model.Add(new MenuPermissionsModel { MenuName = "View Sales", IsVisible = Rights.ViewSales == true? true:false });
                            GetData.ViewSales = Rights.ViewSales == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Sales Target And Performance", IsVisible = Rights.SaleTarget == true? true:false });
                            GetData.SalesTargetAndPerformance = Rights.SaleTarget == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Track Sales Team", IsVisible = false });
                            GetData.TrackSalesTeam = false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Map view–Sales Team", IsVisible = false });
                            GetData.MapviewSalesTeam = false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Payments", IsVisible = Rights.Viewpayment == true? true:false });
                            GetData.Payments = Rights.Viewpayment == true ? true : false;
                            if (Rights.CommonActivityRemark == true || Rights.CreateDailyRemark == true || Rights.ViewDailyRemark == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "General", IsVisible = true });
                                GetData.General = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "General", IsVisible = false });
                                GetData.General = false;
                            }


                            //model.Add(new MenuPermissionsModel { MenuName = "Create Daily Remark", IsVisible =(Rights.CommonActivityRemark == true || Rights.CreateDailyRemark == true)? true :false});
                            GetData.CreateDailyRemark = (Rights.CommonActivityRemark == true || Rights.CreateDailyRemark == true) ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Daily Remark", IsVisible = (Rights.CommonActivityRemark == true || Rights.ViewDailyRemark == true) ? true : false });
                            GetData.ViewDailyRemark = (Rights.CommonActivityRemark == true || Rights.ViewDailyRemark == true) ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Developers", IsVisible = Rights.DeveloperReport == true && CompanyID == 296? true:false });
                            GetData.Developers = Rights.DeveloperReport == true && CompanyID == 296 ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Create Activity", IsVisible = true && CompanyID == 296 ? true : false });
                            GetData.CreateActivity = CompanyID == 296 && Rights.CommonActivityRemark == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Activity", IsVisible = true && CompanyID == 296 ? true : false });
                            GetData.ViewActivity = CompanyID == 296 && Rights.ViewDailyRemark == true ? true : false;
                            if (Rights.ProjectManagement == true || Rights.DailyWorkSchedule == true || Rights.CreateAssignWork == true || Rights.ViewAssignedWorks == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Projects", IsVisible = true });
                                GetData.Projects = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Projects", IsVisible = false });
                                GetData.Projects = false;
                            }
                            //model.Add(new MenuPermissionsModel { MenuName = "Project", IsVisible = Rights.ProjectManagement == true? true:false });
                            GetData.Project = Rights.ProjectManagement == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Create and Assign Work", IsVisible = (Rights.DailyWorkSchedule == true || Rights.CreateAssignWork == true)? true:false });
                            GetData.CreateandAssignWork = (Rights.DailyWorkSchedule == true || Rights.CreateAssignWork == true) ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Assigned Work", IsVisible = (Rights.DailyWorkSchedule == true || Rights.ViewAssignedWorks == true)? true:false });
                            GetData.ViewAssignedWork = (Rights.DailyWorkSchedule == true || Rights.ViewAssignedWorks == true) ? true : false;
                            if (Rights.RoleManagement == true || Rights.ManageUser == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Users", IsVisible = true });
                                GetData.Users = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Users", IsVisible = false });
                                GetData.Users = false;
                            }
                            // model.Add(new MenuPermissionsModel { MenuName = "Manage Users", IsVisible = Rights.ManageUser == true? true:false });
                            GetData.ManageUsers = Rights.ManageUser == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Manage Roles", IsVisible = Rights.RoleManagement == true ? true : false });
                            GetData.ManageRoles = Rights.RoleManagement == true ? true : false;
                            if (ProfileName == "HR" || (Rights.IsManageEmployees == true) || (Rights.ManageSalary == true) || (Rights.ManageExtraPayment == true)
                               || (Rights.EmpLoginHistory == true) || (Rights.ManualAttandence == true) || (Rights.EmpLeaveRequest == true) || (Rights.CreateInterviewSchedule == true) || (Rights.ViewInterviewSchedule == true))
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "HR", IsVisible = true });
                                GetData.HR = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "HR", IsVisible = false });
                                GetData.HR = false;
                            }

                            //model.Add(new MenuPermissionsModel { MenuName = "Manage Employees", IsVisible = Rights.IsManageEmployees == true? true:false });
                            GetData.ManageEmployees = Rights.IsManageEmployees == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Manage Salary", IsVisible = Rights.ManageSalary == true? true:false });
                            GetData.ManageSalary = Rights.ManageSalary == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Manage Extra Payment", IsVisible = Rights.ManageExtraPayment == true? true:false });
                            GetData.ManageExtraPayment = Rights.ManageExtraPayment == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Employee Login History", IsVisible = Rights.EmpLoginHistory == true? true:false });
                            GetData.EmployeeLoginHistory = Rights.EmpLoginHistory == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Manual Attendance", IsVisible = Rights.ManualAttandence == true? true:false });
                            GetData.ManualAttendance = Rights.ManualAttandence == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Employee Leave Requests", IsVisible = Rights.EmpLeaveRequest == true?true:false });
                            GetData.EmployeeLeaveRequests = Rights.EmpLeaveRequest == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Create Interview Schedule", IsVisible = Rights.CreateInterviewSchedule == true? true:false });
                            GetData.CreateInterviewSchedule = Rights.CreateInterviewSchedule == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Interview Schedule", IsVisible = Rights.ViewInterviewSchedule == true? true:false });
                            GetData.ViewInterviewSchedule = Rights.ViewInterviewSchedule == true ? true : false;
                            if (Rights.CreateTicket == true || Rights.ViewTicket == true /*|| (UID == 661 || UID == 662 || UID == 664)*/)//661,662,664 is rohit singh,deepak kumar, prashant kumar id to show ticket management
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Tickets", IsVisible = true });
                                GetData.Tickets = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "Tickets", IsVisible = false });
                                GetData.Tickets = false;
                            }

                            //model.Add(new MenuPermissionsModel { MenuName = "Create Ticket", IsVisible = (Rights.CreateTicket == true || (UID == 661 || UID == 662 || UID == 664))? true:false });
                            GetData.CreateTicket = Rights.CreateTicket == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Tickets", IsVisible = (Rights.ViewTicket == true || (UID == 661 || UID == 662 || UID == 664)) ? true : false });
                            GetData.ViewTickets = Rights.ViewTicket == true ? true : false;
                            if (Rights.EmpAttendence == true || Rights.NewLeave == true || Rights.ViewLeave == true || Rights.MappedUser == true || Rights.ViewAssignedWorks == true)
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "My Space", IsVisible = true });
                                GetData.MySpace = true;
                            }
                            else
                            {
                                //model.Add(new MenuPermissionsModel { MenuName = "My Space", IsVisible = false });
                                GetData.MySpace = false;
                            }

                            //model.Add(new MenuPermissionsModel { MenuName = "Attendance", IsVisible = Rights.EmpAttendence == true? true:false });
                            GetData.Attendance = Rights.EmpAttendence == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "Apply Leave", IsVisible = Rights.NewLeave == true?true:false });
                            GetData.ApplyLeave = Rights.NewLeave == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Leaves", IsVisible = Rights.ViewLeave == true? true:false });
                            GetData.ViewLeaves = Rights.ViewLeave == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "My Team", IsVisible = Rights.MappedUser == true? true:false });
                            GetData.MyTeam = Rights.MappedUser == true ? true : false;
                            //model.Add(new MenuPermissionsModel { MenuName = "View Assigned Work", IsVisible = Rights.ViewAssignedWorks == true? true:false });
                            GetData.ViewAssignedWork = Rights.ViewAssignedWorks == true ? true : false;

                            model.Add(GetData);//add all the menu permissions
                        }
                        else
                        {
                            message = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                            HttpError err = new HttpError(message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                        }
                    }
                    else
                    {
                        message = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                        HttpError err = new HttpError(message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);

                message = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            if (!string.IsNullOrEmpty(message))
            {
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
        }
    }
}
