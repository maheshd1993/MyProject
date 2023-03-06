using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Svam.EF;
using Traders.Models;
using Svam.Models;
using NReco.PdfGenerator;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Svam.UtilityManager;

namespace Traders.Controllers
{
    public class HRController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        public ActionResult Dashboard()
        {
            ViewInterviewSchedule VISM = new ViewInterviewSchedule();
            try
            {
                if (Session["UID"] != null)
                {
                    #region Cal- DateTime.......
                    var CurrentDate = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var TodayDate = CurrentDate.ToString("MM/dd/yyyy");
                    var MStartDate = monthStartDate.ToString("MM/dd/yyyy");
                    var MEndDate = MonthendDate.ToString("MM/dd/yyyy");
                    #endregion

                    var UID = Convert.ToInt32(Session["UID"]);
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        #region By Super-Admin

                        DateTime localTime = Constant.GetBharatTime();
                        var TodatDate = localTime.Date.ToString("MM/dd/yyyy");

                        #region New-Candidate
                        DataTable GetNewCandidates = DataAccessLayer.GetDataTable("call P_getInterViewByHr('" + 0 + "','" + TodatDate + "','" + TodatDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetNewCandidates.Rows.Count > 0)
                        {
                            VISM.NewCandidatemodelList = (from dr in GetNewCandidates.AsEnumerable()
                                                          select new ViewInterviewSchedule()
                                                          {
                                                              Id = Convert.ToInt32(dr["Id"]),
                                                              CandidateName = Convert.ToString(dr["CandidateName"]),
                                                              Mobile = Convert.ToString(dr["MobileNo"]),
                                                              Email = Convert.ToString(dr["Email"]),
                                                              Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                              ProfileName = Convert.ToString(dr["ProfileName"]),
                                                              FollowUpDate = Convert.ToString(dr["FollowUpDate"]),
                                                              InterviewDate = Convert.ToString(dr["InterViewDate"]),
                                                              CurrentStatus = Convert.ToString(dr["Status"]),
                                                          }).ToList();
                        }
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region By HR
                        DateTime utcTime = DateTime.UtcNow;
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                        var TodatDate = localTime.Date.ToString("MM/dd/yyyy");
                        //var TodatDate = System.DateTime.Now.Date.ToString("MM/dd/yyyy");
                        var NextWeekDate = localTime.AddDays(7).Date.ToString("MM/dd/yyyy");

                        #region New-Candidate
                        DataTable GetNewCandidates = DataAccessLayer.GetDataTable(" call P_getInterViewByHr('" + UID + "','" + TodatDate + "','" + TodatDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetNewCandidates.Rows.Count > 0)
                        {
                            VISM.NewCandidatemodelList = (from dr in GetNewCandidates.AsEnumerable()
                                                          select new ViewInterviewSchedule()
                                                          {
                                                              Id = Convert.ToInt32(dr["Id"]),
                                                              CandidateName = Convert.ToString(dr["CandidateName"]),
                                                              Mobile = Convert.ToString(dr["MobileNo"]),
                                                              Email = Convert.ToString(dr["Email"]),
                                                              Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                              ProfileName = Convert.ToString(dr["ProfileName"]),
                                                              FollowUpDate = Convert.ToString(dr["FollowUpDate"]),
                                                              InterviewDate = Convert.ToString(dr["InterViewDate"]),
                                                              CurrentStatus = Convert.ToString(dr["Status"]),
                                                          }).ToList();
                        }
                        #endregion

                        #region Next-Week-Schedule
                        DataTable GetNextWeekInterviewSchedule = DataAccessLayer.GetDataTable(" call CRM_getNextWeekInterviewSchedule('" + UID + "','" + TodatDate + "','" + NextWeekDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetNextWeekInterviewSchedule.Rows.Count > 0)
                        {
                            VISM.NextweekinterviewSchedulemodelList = (from dr in GetNextWeekInterviewSchedule.AsEnumerable()
                                                                       select new ViewInterviewSchedule()
                                                                       {
                                                                           Id = Convert.ToInt32(dr["Id"]),
                                                                           CandidateName = Convert.ToString(dr["CandidateName"]),
                                                                           Mobile = Convert.ToString(dr["MobileNo"]),
                                                                           Email = Convert.ToString(dr["Email"]),
                                                                           Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                                           ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                           FollowUpDate = Convert.ToString(dr["FollowUpDate"]),
                                                                           InterviewDate = Convert.ToString(dr["InterViewDate"]),
                                                                           CurrentStatus = Convert.ToString(dr["Status"]),
                                                                       }).ToList();
                        }
                        #endregion

                        #region Todays-Followup
                        DataTable GetTodayFollowup = DataAccessLayer.GetDataTable(" call CRM_getTodayFollowupInterview('" + UID + "','" + TodatDate + "','" + TodatDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetTodayFollowup.Rows.Count > 0)
                        {
                            VISM.TodaysfollowupmodelList = (from dr in GetTodayFollowup.AsEnumerable()
                                                            select new ViewInterviewSchedule()
                                                            {
                                                                Id = Convert.ToInt32(dr["Id"]),
                                                                CandidateName = Convert.ToString(dr["CandidateName"]),
                                                                Mobile = Convert.ToString(dr["MobileNo"]),
                                                                Email = Convert.ToString(dr["Email"]),
                                                                Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                                ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                FollowUpDate = Convert.ToString(dr["FollowUpDate"]),
                                                                InterviewDate = Convert.ToString(dr["InterViewDate"]),
                                                                CurrentStatus = Convert.ToString(dr["Status"]),
                                                            }).ToList();
                        }
                        #endregion

                        #region Todays-InterView
                        DataTable GetTodayinterview = DataAccessLayer.GetDataTable(" call CRM_getNextWeekInterviewSchedule('" + UID + "','" + TodatDate + "','" + TodatDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetTodayinterview.Rows.Count > 0)
                        {
                            VISM.TodaysInterviewmodelList = (from dr in GetTodayinterview.AsEnumerable()
                                                             select new ViewInterviewSchedule()
                                                             {
                                                                 Id = Convert.ToInt32(dr["Id"]),
                                                                 CandidateName = Convert.ToString(dr["CandidateName"]),
                                                                 Mobile = Convert.ToString(dr["MobileNo"]),
                                                                 Email = Convert.ToString(dr["Email"]),
                                                                 Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                                 ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                 FollowUpDate = Convert.ToString(dr["FollowUpDate"]),
                                                                 InterviewDate = Convert.ToString(dr["InterViewDate"]),
                                                                 CurrentStatus = Convert.ToString(dr["Status"]),
                                                             }).ToList();
                        }

                        #endregion

                        #endregion
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(VISM);
        }

        public ActionResult Index()
        {
            EmployeeLogHistory ELH = new EmployeeLogHistory();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            ELH.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            try
            {
                Int32? UID = 0;
                UID = Convert.ToInt32(Session["UID"]);
                Int32? UserID = Convert.ToInt32(Session["UID"]);

                var AssignList1 = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == UID).OrderBy(em => em.Fname).FirstOrDefault();

                if (AssignList1 != null)
                {
                    if (AssignList1.MappedUsers != null)
                    {
                        List<EmployeeLogHistory> assignToList = new List<EmployeeLogHistory>();
                        var GetMapUser = AssignList1.MappedUsers.Split(',');
                        foreach (var item in GetMapUser)
                        {
                            var mapid = Convert.ToInt32(item);
                            var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (GetMapUserData != null)
                            {
                                //int roleid = Convert.ToInt32(GetMapUserData.ProfileId);
                                //var roledata = db.crm_roleassigntbl.Where(em => em.Id == roleid && em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).FirstOrDefault();
                                //if (roledata.ViewTicket == true)
                                //{
                                    EmployeeLogHistory ouser = new EmployeeLogHistory();
                                    ouser.UserID = mapid;
                                    ouser.UserName = GetMapUserData.Fname + ' ' + GetMapUserData.Lname + '(' + GetMapUserData.EmployeeCode + ')';
                                    assignToList.Add(ouser);
                                //}
                            }
                        }
                        ELH.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
                    }
                    else
                    {
                        var AssignList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
                        if (AssignList != null)
                        {
                            List<EmployeeLogHistory> assignToList = new List<EmployeeLogHistory>();
                            foreach (var item in AssignList)
                            {
                                EmployeeLogHistory CRM = new EmployeeLogHistory();
                                CRM.UserID = item.Id;
                                CRM.UserName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                                assignToList.Add(CRM);
                            }

                            ELH.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
                        }
                    }
                }
                if (Session["UID"] != null)
                {
                    #region Data-time-Formate
                    var getPrevDate = Constant.GetBharatTime().AddDays(0);
                    var getNewPrevDate = getPrevDate.ToString("dd/MM/yyyy");

                    #endregion

                    #region GetLog-History

                    if (CompanyID == 2644)
                    {

                        if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                        {
                            var AssignList12 = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == UID).OrderBy(em => em.Fname).FirstOrDefault();

                            if (AssignList12 != null)
                            {
                                if (AssignList12.MappedUsers != null)
                                {
                                    var GetMapUser = AssignList12.MappedUsers.Split(',');
                                    foreach (var item in GetMapUser)
                                    {
                                        var mapid = Convert.ToInt32(item);
                                        DataTable GetLogHistory = DataAccessLayer.GetDataTable(" call CRM_GetUserLogHistory('" + mapid + "','" + getNewPrevDate + "','" + getNewPrevDate + "'," + BranchID + "," + CompanyID + ")");
                                        if (GetLogHistory.Rows.Count > 0)
                                        {
                                            var TodatDate = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                                            var getloginhistory = (from dr in GetLogHistory.AsEnumerable()
                                                                   select new EmployeeLogHistory()
                                                                   {
                                                                       Id = Convert.ToInt32(dr["ID"]),
                                                                       EmpId = Convert.ToInt32(dr["EmpId"]),
                                                                       EmpName = Convert.ToString(dr["EmpName"]),
                                                                       ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                       LoginDate = dr["L_In_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_In_Date"])) : string.Empty,
                                                                       LoginTime = Convert.ToString(dr["L_In_Time"]),
                                                                       LogoutDate = dr["L_Out_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_Out_Date"])) : string.Empty,
                                                                       LogoutTime = Convert.ToString(dr["L_Out_Time"]),
                                                                       Duration = Convert.ToString(dr["Duration"]),
                                                                       WorkingLateHours = dr["Working_Late_Night"] == DBNull.Value ? false : Convert.ToBoolean(dr["Working_Late_Night"]),
                                                                       ExtraWorking = dr["Extra_working"] == DBNull.Value ? false : Convert.ToBoolean(dr["Extra_working"]),
                                                                       IPAddress = Convert.ToString(dr["IPAddress"]),
                                                                       MacAddress = Convert.ToString(dr["MacAddress"]),
                                                                       TimeZone = Convert.ToString(dr["LogTimeZone"])
                                                                   }).OrderByDescending(em => em.LoginDate).ToList();
                                            ELH.GetEmpLogHistoryModel.AddRange(getloginhistory);
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            DataTable GetLogHistory = DataAccessLayer.GetDataTable(" call CRM_GetUserLogHistory('" + 0 + "','" + getNewPrevDate + "','" + getNewPrevDate + "'," + BranchID + "," + CompanyID + ")");
                            if (GetLogHistory.Rows.Count > 0)
                            {
                                var TodatDate = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                                ELH.GetEmpLogHistoryModel = (from dr in GetLogHistory.AsEnumerable()
                                                             select new EmployeeLogHistory()
                                                             {
                                                                 Id = Convert.ToInt32(dr["ID"]),
                                                                 EmpId = Convert.ToInt32(dr["EmpId"]),
                                                                 EmpName = Convert.ToString(dr["EmpName"]),
                                                                 ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                 LoginDate = dr["L_In_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_In_Date"])) : string.Empty,
                                                                 LoginTime = Convert.ToString(dr["L_In_Time"]),
                                                                 LogoutDate = dr["L_Out_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_Out_Date"])) : string.Empty,
                                                                 LogoutTime = Convert.ToString(dr["L_Out_Time"]),
                                                                 Duration = Convert.ToString(dr["Duration"]),
                                                                 WorkingLateHours = dr["Working_Late_Night"] == DBNull.Value ? false : Convert.ToBoolean(dr["Working_Late_Night"]),
                                                                 ExtraWorking = dr["Extra_working"] == DBNull.Value ? false : Convert.ToBoolean(dr["Extra_working"]),
                                                                 IPAddress = Convert.ToString(dr["IPAddress"]),
                                                                 MacAddress = Convert.ToString(dr["MacAddress"]),
                                                                 TimeZone = Convert.ToString(dr["LogTimeZone"])
                                                             }).OrderByDescending(em => em.LoginDate).ToList();
                            }
                        }
                    }
                    else
                    {
                        DataTable GetLogHistory = DataAccessLayer.GetDataTable(" call CRM_GetUserLogHistory('" + 0 + "','" + getNewPrevDate + "','" + getNewPrevDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetLogHistory.Rows.Count > 0)
                        {
                            var TodatDate = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                            ELH.GetEmpLogHistoryModel = (from dr in GetLogHistory.AsEnumerable()
                                                         select new EmployeeLogHistory()
                                                         {
                                                             Id = Convert.ToInt32(dr["ID"]),
                                                             EmpId = Convert.ToInt32(dr["EmpId"]),
                                                             EmpName = Convert.ToString(dr["EmpName"]),
                                                             ProfileName = Convert.ToString(dr["ProfileName"]),
                                                             LoginDate = dr["L_In_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_In_Date"])) : string.Empty,
                                                             LoginTime = Convert.ToString(dr["L_In_Time"]),
                                                             LogoutDate = dr["L_Out_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_Out_Date"])) : string.Empty,
                                                             LogoutTime = Convert.ToString(dr["L_Out_Time"]),
                                                             Duration = Convert.ToString(dr["Duration"]),
                                                             WorkingLateHours = dr["Working_Late_Night"] == DBNull.Value ? false : Convert.ToBoolean(dr["Working_Late_Night"]),
                                                             ExtraWorking = dr["Extra_working"] == DBNull.Value ? false : Convert.ToBoolean(dr["Extra_working"]),
                                                             IPAddress = Convert.ToString(dr["IPAddress"]),
                                                             MacAddress = Convert.ToString(dr["MacAddress"]),
                                                             TimeZone = Convert.ToString(dr["LogTimeZone"])
                                                         }).OrderByDescending(em => em.LoginDate).ToList();
                        }
                    }

                    #endregion
                    Session["Hr-HoldEmpLogHistory"] = ELH.GetEmpLogHistoryModel.ToList();
                }
                else
                {
                    Session["ReturnUrl"] = "/HR";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
                return Redirect("/home/login");
            }
            return View(ELH);
        }

        #region Date-Wise-filter
        public ActionResult FilterLogHistorybyDate(int? SearchText, string FromDate, string ToDate)
        {
            EmployeeLogHistory ELH = new EmployeeLogHistory();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            ELH.DateFormat = Constant.DateFormat();//get date format by company id
            try
            {
                if (Session["UID"] != null)
                {
                    if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
                    {
                        var MStartDate = string.Empty;
                        var MEndDate = string.Empty;
                        if (ELH.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var fmDate = DateTime.ParseExact(FromDate, ELH.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, ELH.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            MStartDate = FromDate;
                            MEndDate = ToDate;
                        }

                        #region GetLog-History
                        DataTable GetLogHistory = DataAccessLayer.GetDataTable(" call CRM_GetUserLogHistory('" + 0 + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetLogHistory.Rows.Count > 0)
                        {
                            ELH.GetEmpLogHistoryModel = (from dr in GetLogHistory.AsEnumerable()
                                                         select new EmployeeLogHistory()
                                                         {
                                                             Id = Convert.ToInt32(dr["ID"]),
                                                             EmpId = Convert.ToInt32(dr["EmpId"]),
                                                             EmpName = Convert.ToString(dr["EmpName"]),
                                                             ProfileName = Convert.ToString(dr["ProfileName"]),
                                                             LoginDate = dr["L_In_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_In_Date"])) : string.Empty,
                                                             LoginTime = Convert.ToString(dr["L_In_Time"]),
                                                             LogoutDate = dr["L_Out_Date"] != DBNull.Value ? String.Format("{0:" + ELH.DateFormat + "}", Convert.ToDateTime(dr["L_Out_Date"])) : string.Empty,
                                                             LogoutTime = Convert.ToString(dr["L_Out_Time"]),
                                                             Duration = Convert.ToString(dr["Duration"]),
                                                             WorkingLateHours = dr["Working_Late_Night"] == DBNull.Value ? false : Convert.ToBoolean(dr["Working_Late_Night"]),
                                                             ExtraWorking = dr["Extra_working"] == DBNull.Value ? false : Convert.ToBoolean(dr["Extra_working"]),
                                                             IPAddress = Convert.ToString(dr["IPAddress"]),
                                                             MacAddress = Convert.ToString(dr["MacAddress"]),
                                                             TimeZone = Convert.ToString(dr["LogTimeZone"])
                                                         }).ToList();

                        }

                        if (SearchText != 0)
                        {
                            //var filter = ELH.GetEmpLogHistoryModel.Where(em => em.EmpName.ToLower().Contains(SearchText.ToLower().Trim())).ToList();
                            var filter = ELH.GetEmpLogHistoryModel.Where(em => em.EmpId == SearchText).ToList();
                            ELH.GetEmpLogHistoryModel = filter.ToList();
                        }
                        #endregion
                    }
                    else if (SearchText != 0 && string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                    {
                        if (Session["Hr-HoldEmpLogHistory"] != null)
                        {
                            var GetLogHistoryData = (List<EmployeeLogHistory>)Session["Hr-HoldEmpLogHistory"];
                            if (GetLogHistoryData.Count > 0)
                            {
                                //var FilterData = GetLogHistoryData.Where(em => em.EmpName.ToLower().Contains(SearchText.ToLower().Trim())).ToList();
                                var FilterData = GetLogHistoryData.Where(em => em.EmpId == SearchText).ToList();
                                ELH.GetEmpLogHistoryModel = FilterData.ToList();
                            }
                        }
                    }
                    //Session["Hr-HoldEmpLogHistory"] = ELH.GetEmpLogHistoryModel.ToList();
                }
                else
                {
                    Session["ReturnUrl"] = "/HR";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_FilterLoghistoryByUser", ELH);
        }
        #endregion

        #region Search-Users
        public ActionResult SearchNisEmployee(string SearchText)
        {
            string ReturnMsg = "";
            try
            {
                EmployeeLogHistory ELH = new EmployeeLogHistory();
                if (Session["Hr-HoldEmpLogHistory"] != null)
                {
                    var GetLogHistoryData = (List<EmployeeLogHistory>)Session["Hr-HoldEmpLogHistory"];
                    if (GetLogHistoryData.Count > 0)
                    {
                        var FilterData = GetLogHistoryData.Where(em => em.EmpName.ToLower().Contains(SearchText.Trim())).ToList();
                        ELH.GetEmpLogHistoryModel = FilterData.ToList();
                    }
                }
                return PartialView("_FilterLoghistoryByUser", ELH);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ReturnMsg = "error";
                return Json(ReturnMsg, JsonRequestBehavior.AllowGet);
            }
            //return View();
        }
        #endregion

        #region Create-Interview-Schdule
        public ActionResult CreateInterviewSchdule(int? id)
        {
            CreateInterViewSchduleModel CISM = new CreateInterViewSchduleModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            CISM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            try
            {
                #region Get-Profile-List
                var GetProfileList = db.crm_jobprofiletbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).OrderBy(a => a.Profile).ToList();
                if (GetProfileList != null)
                {
                    //List<CreateInterViewSchduleModel> profileList = new List<CreateInterViewSchduleModel>();
                    var profileList = (from item in GetProfileList
                                       select new CreateInterViewSchduleModel
                                       {
                                           ProfileID = item.Id,
                                           Profile = item.Profile
                                       }).ToList();
                    //foreach (var item in GetProfileList)
                    //{
                    //    CreateInterViewSchduleModel profileModel = new CreateInterViewSchduleModel();
                    //    profileModel.ProfileID = item.Id;
                    //    profileModel.Profile = item.Profile;
                    //    profileList.Add(profileModel);
                    //}
                    CISM.ProfileList = profileList;
                }
                #endregion

                #region Get-InterviewStatus List
                var GetInterviewStatusList = db.crm_intervewstatus.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).OrderBy(a => a.InterViewStatus).ToList();
                if (GetInterviewStatusList != null)
                {
                    //List<CreateInterViewSchduleModel> interviewStatusList = new List<CreateInterViewSchduleModel>();
                    var interviewStatusList = (from item in GetInterviewStatusList
                                               select new CreateInterViewSchduleModel
                                               {
                                                   InterviewStatusID = item.ID,
                                                   InterviewStatusName = item.InterViewStatus
                                               }).ToList();
                    //foreach (var item in GetInterviewStatusList)
                    //{
                    //    CreateInterViewSchduleModel iStatusModel = new CreateInterViewSchduleModel();
                    //    iStatusModel.InterviewStatusID = item.ID;
                    //    iStatusModel.InterviewStatusName = item.InterViewStatus;
                    //    interviewStatusList.Add(iStatusModel);
                    //}
                    CISM.InterviewStatusList = interviewStatusList;
                }
                #endregion


                if (id != null)
                {
                    var GetData = db.crm_interviewscheduletbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        CISM.CandidateName = GetData.CandidateName;
                        CISM.Telephone = GetData.Telephone;
                        CISM.ProfileID = GetData.Profile;
                        //ViewBag.Profile = new SelectList(GetProfile, "Id", "Profile", GetData.Profile);
                        if (!string.IsNullOrEmpty(GetData.DateOfBirth))
                        {
                            var dobDate = DateTime.ParseExact(GetData.DateOfBirth, CISM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(GetData.DateOfBirth);
                            CISM.DOB = String.Format("{0:" + CISM.DateFormat + "}", dobDate);
                        }

                        CISM.PostalAddress = GetData.PostalAddress;
                        CISM.ResumeTitle = GetData.ResumeTitle;
                        CISM.PreferredLocation = GetData.PreferredLocation;
                        CISM.CurrentEmployer = GetData.CurrentEmployer;
                        CISM.CurrentDesignation = GetData.CurrentDesignation;
                        CISM.AnnualSalary = GetData.AnnualSalary;
                        CISM.UGCourses = GetData.UGCourses;
                        CISM.PGCourses = GetData.PGCourses;
                        CISM.PPGCourses = GetData.PPGCourses;
                        CISM.Email = GetData.Email;
                        CISM.MobileNo = GetData.MobileNo;
                        CISM.TotalExp = GetData.WorkExperiance;
                        CISM.CurrentLocation = GetData.CurrentLocation;
                        if (!string.IsNullOrEmpty(GetData.FollowUpDate))
                        {
                            var fupDate = DateTime.ParseExact(GetData.FollowUpDate, CISM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(GetData.FollowUpDate);
                            CISM.FollowUpDate = String.Format("{0:" + CISM.DateFormat + "}", fupDate);
                        }
                        if (!string.IsNullOrEmpty(GetData.InterViewDate))
                        {
                            var intrDate = DateTime.ParseExact(GetData.InterViewDate, CISM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(GetData.InterViewDate);
                            CISM.InterviewDate = String.Format("{0:" + CISM.DateFormat + "}", intrDate);
                        }

                        CISM.Remarks = GetData.Remarks;
                        CISM.Status = GetData.Status;
                        if (!string.IsNullOrEmpty(GetData.LastActiveDate))
                        {
                            var actvDate = DateTime.ParseExact(GetData.LastActiveDate, CISM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(GetData.LastActiveDate);
                            CISM.LastActivateDate = String.Format("{0:" + CISM.DateFormat + "}", actvDate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(CISM);
        }

        [HttpPost]
        public ActionResult CreateInterviewSchdule(CreateInterViewSchduleModel CISM, int? id)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                CISM.DateFormat = Constant.DateFormat();//get date format by company id
                if (Session["UID"] != null)
                {
                    if (id != null)
                    {
                        var GetData = db.crm_interviewscheduletbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).SingleOrDefault();
                        if (GetData != null)
                        {
                            GetData.CandidateName = Convert.ToString(CISM.CandidateName);
                            GetData.Profile = Convert.ToInt32(CISM.ProfileID);
                            GetData.Email = CISM.Email;
                            GetData.Telephone = CISM.Telephone;
                            GetData.MobileNo = CISM.MobileNo;
                            GetData.DateOfBirth = CISM.DOB;
                            GetData.PostalAddress = CISM.PostalAddress;
                            GetData.WorkExperiance = CISM.TotalExp;
                            GetData.ResumeTitle = CISM.ResumeTitle;
                            GetData.CurrentLocation = CISM.CurrentLocation;
                            GetData.PreferredLocation = CISM.PreferredLocation;
                            GetData.CurrentEmployer = CISM.CurrentEmployer;
                            GetData.CurrentDesignation = CISM.CurrentDesignation;
                            GetData.AnnualSalary = CISM.AnnualSalary;
                            GetData.UGCourses = CISM.UGCourses;
                            GetData.PGCourses = CISM.PGCourses;
                            GetData.PPGCourses = CISM.PPGCourses;
                            GetData.LastActiveDate = CISM.LastActivateDate;
                            GetData.FollowUpDate = CISM.FollowUpDate;
                            GetData.InterViewDate = CISM.InterviewDate;
                            GetData.Remarks = CISM.Remarks;
                            GetData.Status = CISM.Status;
                            GetData.BranchID = BranchID;
                            GetData.CompanyID = CompanyID;
                            db.SaveChanges();
                            TempData["success"] = "Interview Schedule updated sucessfully";
                        }
                    }
                    else
                    {
                        crm_interviewscheduletbl interviewtbl = new crm_interviewscheduletbl();
                        interviewtbl.UID = Convert.ToInt32(Session["UID"]);
                        if (CISM.CandidateName != null || CISM.ProfileID != null || CISM.MobileNo != null)
                        {
                            interviewtbl.CandidateName = CISM.CandidateName;
                            interviewtbl.Profile = Convert.ToInt32(CISM.ProfileID);
                            interviewtbl.Email = Convert.ToString(CISM.Email);
                            interviewtbl.Telephone = Convert.ToString(CISM.Telephone);
                            interviewtbl.MobileNo = Convert.ToString(CISM.MobileNo);
                            interviewtbl.DateOfBirth = Convert.ToString(CISM.DOB);
                            interviewtbl.PostalAddress = Convert.ToString(CISM.PostalAddress);
                            interviewtbl.WorkExperiance = CISM.TotalExp;
                            interviewtbl.ResumeTitle = Convert.ToString(CISM.ResumeTitle);
                            interviewtbl.CurrentLocation = Convert.ToString(CISM.CurrentLocation);
                            interviewtbl.PreferredLocation = Convert.ToString(CISM.PreferredLocation);
                            interviewtbl.CurrentEmployer = Convert.ToString(CISM.CurrentEmployer);
                            interviewtbl.CurrentDesignation = Convert.ToString(CISM.CurrentDesignation);
                            interviewtbl.AnnualSalary = Convert.ToString(CISM.AnnualSalary);
                            interviewtbl.UGCourses = Convert.ToString(CISM.UGCourses);
                            interviewtbl.PGCourses = Convert.ToString(CISM.PGCourses);
                            interviewtbl.PPGCourses = Convert.ToString(CISM.PPGCourses);
                            interviewtbl.LastActiveDate = Convert.ToString(CISM.LastActivateDate);
                            interviewtbl.FollowUpDate = CISM.FollowUpDate;
                            interviewtbl.InterViewDate = CISM.InterviewDate;
                            interviewtbl.Remarks = CISM.Remarks;
                            interviewtbl.CreateDate = Constant.GetBharatTime().ToString("dd/MM/yyyy");
                            interviewtbl.Created_at = Constant.GetBharatTime();
                            interviewtbl.Status = Convert.ToString(CISM.InterviewStatusID);
                            interviewtbl.BranchID = BranchID;
                            interviewtbl.CompanyID = CompanyID;
                            db.crm_interviewscheduletbl.Add(interviewtbl);
                            if (db.SaveChanges() > 0)
                            {
                                TempData["success"] = "Interview Schedule added sucessfully";
                            }
                        }
                        else
                        {
                            TempData["alert"] = "Fill Candidate Name , Profile and Mobile No";
                            return Redirect("/Hr/CreateInterviewSchdule");
                        }
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/hr/CreateInterviewSchdule";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Hr/ViewInterviewSchedule");
        }

        public ActionResult UploadInterview(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    int UID = Convert.ToInt32(Session["UID"]);
                    if (Request.Files["file"].ContentLength > 0)
                    {
                        string FileName = Path.GetFileName(file.FileName);
                        FileName = "Trd-" + Guid.NewGuid().ToString().Substring(0, 4) + "-" + FileName;
                        string Extension = Path.GetExtension(file.FileName);
                        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                        string FilePath = Server.MapPath(FolderPath + FileName);
                        file.SaveAs(FilePath);
                        var confirm = ExcelImportExport.Import_Interview(FilePath, Extension, "Yes", UID, BranchID, CompanyID);
                        if (confirm != "")
                        {
                            TempData["alert"] = confirm;
                        }
                    }
                    else
                    {
                        TempData["alert"] = "There is some Problem!";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return Redirect("/hr/CreateInterviewSchdule");
        }
        #endregion

        #region View-Interview-Schedule

        public ActionResult ViewInterviewSchedule(string InterviewStatus, string FromDate, string ToDate)
        {
            ViewInterviewSchedule VISM = new ViewInterviewSchedule();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                VISM.DateFormat = Constant.DateFormat();//get date format by company id
                Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                if (Session["UID"] != null)
                {

                    int UID = 0;
                    if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                    {
                        UID = Convert.ToInt32(Session["UID"]);
                    }
                    #region Get-Profile-List
                    var GetProfile = db.crm_jobprofiletbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(a => a.Profile).ToList().Select(em => new crm_jobprofiletbl
                    {
                        Id = em.Id,
                        Profile = em.Profile
                    }).AsQueryable();
                    ViewBag.Profile = new SelectList(GetProfile, "Id", "Profile");
                    #endregion

                    #region Interview Status
                    var isList = db.crm_intervewstatus.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).OrderBy(a => a.InterViewStatus).ToList();
                    VISM.InterviewStatusList = new SelectList(isList, "InterViewStatus", "InterViewStatus", InterviewStatus);
                    #endregion

                    #region Cal- DateTime.......
                    var CurrentDate = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var TodayDate = CurrentDate.ToString("dd/MM/yyyy");
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        MStartDate = FromDate;
                        MEndDate = ToDate;

                        if (VISM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var fmDate = DateTime.ParseExact(FromDate, VISM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, VISM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                        }
                        else
                        {
                            MStartDate = FromDate;
                            MEndDate = ToDate;
                        }
                    }

                    #endregion

                    #region Calculate-Interview

                    DataTable GetData = DataAccessLayer.GetDataTable(" call CRM_getInterViewByHr(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetData.Rows.Count > 0)
                    {
                        VISM.ViewinterviewSchedulemodelList = (from dr in GetData.AsEnumerable()
                                                               select new ViewInterviewSchedule()
                                                               {
                                                                   Id = Convert.ToInt32(dr["Id"]),
                                                                   CandidateName = Convert.ToString(dr["CandidateName"]),
                                                                   Mobile = Convert.ToString(dr["MobileNo"]),
                                                                   Email = Convert.ToString(dr["Email"]),
                                                                   Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                                   ResumeID = Convert.ToString(dr["ResumeId"]),
                                                                   ResumeTitle = Convert.ToString(dr["ResumeTitle"]),
                                                                   CurrentLocation = Convert.ToString(dr["CurrentLocation"]),
                                                                   CurrentDesignation = Convert.ToString(dr["CurrentDesignation"]),
                                                                   CurrentEmployer = Convert.ToString(dr["CurrentEmployer"]),
                                                                   AnnualSalry = Convert.ToString(dr["AnnualSalary"]),
                                                                   UGCourses = Convert.ToString(dr["UGCourses"]),
                                                                   PGCourses = Convert.ToString(dr["PGCourses"]),
                                                                   PPGCourses = Convert.ToString(dr["PPGCourses"]),
                                                                   ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                   ProfileId = Convert.ToInt32(dr["Profile"]),
                                                                   CurrentStatus = Convert.ToString(dr["InterViewStatus"]),
                                                                   Remarks = Convert.ToString(dr["Remarks"]),

                                                                   FollowUpDate = dr["FollowUpDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["FollowUpDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                                   InterviewDate = dr["InterViewDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["InterViewDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                                   CreatedDate = dr["CreateDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["CreateDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty
                                                               }).ToList();
                    }
                    if (InterviewStatus != null && InterviewStatus != "Select" && InterviewStatus != "")
                    {
                        VISM.ViewinterviewSchedulemodelList = VISM.ViewinterviewSchedulemodelList.Where(em => em.CurrentStatus == InterviewStatus).ToList();
                    }

                    #endregion
                }
                else
                {
                    Session["ReturnUrl"] = "/hr/ViewInterviewSchedule";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(VISM);
        }

        public ActionResult addDescription(Int64 Iid, string Description)
        {
            string msg = "";
            try
            {
                crm_interviewdescriptiontbl IDtl = new crm_interviewdescriptiontbl();
                IDtl.Description = Description;
                IDtl.InterviewId = Iid;
                IDtl.Date = Constant.GetBharatTime().ToString("dd/MM/yyyy");
                IDtl.BranchID = Convert.ToInt32(Session["BranchID"]);
                IDtl.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                db.crm_interviewdescriptiontbl.Add(IDtl);
                if (db.SaveChanges() > 0)
                {
                    msg = "done";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "error";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewInterviewDecsription(Int64 Iid)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                ViewBag.result = db.crm_interviewdescriptiontbl.Where(em => em.InterviewId == Iid && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderByDescending(em => em.Id).ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialViewLeadDescription");
        }

        public ActionResult SearchInterViewSchedule(string SearchText)
        {
            ViewInterviewSchedule VISM = new ViewInterviewSchedule();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                VISM.DateFormat = Constant.DateFormat();//get date format by company id
                DataTable GetFilterData = DataAccessLayer.GetDataTable("call CRM_SearchInterviewSchedulebyTeach('" + SearchText + "'," + BranchID + "," + CompanyID + ")");
                if (GetFilterData.Rows.Count > 0)
                {
                    VISM.ViewinterviewSchedulemodelList = (from dr in GetFilterData.AsEnumerable()
                                                           select new ViewInterviewSchedule()
                                                           {
                                                               Id = Convert.ToInt32(dr["Id"]),
                                                               CandidateName = Convert.ToString(dr["CandidateName"]),
                                                               Mobile = Convert.ToString(dr["MobileNo"]),
                                                               Email = Convert.ToString(dr["Email"]),
                                                               Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                               ResumeID = Convert.ToString(dr["ResumeId"]),
                                                               ResumeTitle = Convert.ToString(dr["ResumeTitle"]),
                                                               CurrentLocation = Convert.ToString(dr["CurrentLocation"]),
                                                               CurrentDesignation = Convert.ToString(dr["CurrentDesignation"]),
                                                               CurrentEmployer = Convert.ToString(dr["CurrentEmployer"]),
                                                               AnnualSalry = Convert.ToString(dr["AnnualSalary"]),
                                                               UGCourses = Convert.ToString(dr["UGCourses"]),
                                                               PGCourses = Convert.ToString(dr["PGCourses"]),
                                                               PPGCourses = Convert.ToString(dr["PPGCourses"]),
                                                               ProfileName = Convert.ToString(dr["ProfileName"]),
                                                               ProfileId = Convert.ToInt32(dr["Profile"]),
                                                               CurrentStatus = Convert.ToString(dr["InterViewStatus"]),
                                                               Remarks = Convert.ToString(dr["Remarks"]),

                                                               FollowUpDate = dr["FollowUpDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["FollowUpDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                               InterviewDate = dr["InterViewDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["InterViewDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                               CreatedDate = dr["CreateDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["CreateDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty
                                                           }).ToList();
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_SearchInterviewScheduleByText", VISM);
        }

        public ActionResult FilterData(string DDLSelectedText, string FromDate, string ToDate)
        {
            ViewInterviewSchedule VISM = new ViewInterviewSchedule();
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    VISM.DateFormat = Constant.DateFormat();//get date format by company id
                    int UID = 0;
                    if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                    {
                        UID = Convert.ToInt32(Session["UID"]);
                    }
                    if (!string.IsNullOrEmpty(DDLSelectedText) && string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                    {
                        #region Get-Data-By-DDLSelectedText

                        DataTable GetFilterData = DataAccessLayer.GetDataTable(" call CRM_SearchInterviewSchedulebyTeach('" + DDLSelectedText + "'," + BranchID + "," + CompanyID + ")");
                        if (GetFilterData.Rows.Count > 0)
                        {
                            VISM.ViewinterviewSchedulemodelList = (from dr in GetFilterData.AsEnumerable()
                                                                   select new ViewInterviewSchedule()
                                                                   {
                                                                       Id = Convert.ToInt32(dr["Id"]),
                                                                       CandidateName = Convert.ToString(dr["CandidateName"]),
                                                                       Mobile = Convert.ToString(dr["MobileNo"]),
                                                                       Email = Convert.ToString(dr["Email"]),
                                                                       Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                                       ResumeID = Convert.ToString(dr["ResumeId"]),
                                                                       ResumeTitle = Convert.ToString(dr["ResumeTitle"]),
                                                                       CurrentLocation = Convert.ToString(dr["CurrentLocation"]),
                                                                       CurrentDesignation = Convert.ToString(dr["CurrentDesignation"]),
                                                                       CurrentEmployer = Convert.ToString(dr["CurrentEmployer"]),
                                                                       AnnualSalry = Convert.ToString(dr["AnnualSalary"]),
                                                                       UGCourses = Convert.ToString(dr["UGCourses"]),
                                                                       PGCourses = Convert.ToString(dr["PGCourses"]),
                                                                       PPGCourses = Convert.ToString(dr["PPGCourses"]),
                                                                       ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                       ProfileId = Convert.ToInt32(dr["Profile"]),
                                                                       CurrentStatus = Convert.ToString(dr["InterViewStatus"]),
                                                                       Remarks = Convert.ToString(dr["Remarks"]),

                                                                       FollowUpDate = dr["FollowUpDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["FollowUpDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                                       InterviewDate = dr["InterViewDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["InterViewDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                                       CreatedDate = dr["CreateDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["CreateDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty
                                                                   }).ToList();

                        }
                        #endregion
                    }
                    else if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        var MStartDate = string.Empty;
                        var MEndDate = string.Empty;
                        if (VISM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var fmDate = DateTime.ParseExact(FromDate, VISM.DateFormat, CultureInfo.InvariantCulture); // Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, VISM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            MStartDate = FromDate;
                            MEndDate = ToDate;
                        }
                        #region Calculate-Interview

                        DataTable GetData = DataAccessLayer.GetDataTable(" call CRM_getInterViewByHr('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetData.Rows.Count > 0)
                        {
                            VISM.ViewinterviewSchedulemodelList = (from dr in GetData.AsEnumerable()
                                                                   select new ViewInterviewSchedule()
                                                                   {
                                                                       Id = Convert.ToInt32(dr["Id"]),
                                                                       CandidateName = Convert.ToString(dr["CandidateName"]),
                                                                       Mobile = Convert.ToString(dr["MobileNo"]),
                                                                       Email = Convert.ToString(dr["Email"]),
                                                                       Experiance = Convert.ToString(dr["WorkExperiance"]),
                                                                       ResumeID = Convert.ToString(dr["ResumeId"]),
                                                                       ResumeTitle = Convert.ToString(dr["ResumeTitle"]),
                                                                       CurrentLocation = Convert.ToString(dr["CurrentLocation"]),
                                                                       CurrentDesignation = Convert.ToString(dr["CurrentDesignation"]),
                                                                       CurrentEmployer = Convert.ToString(dr["CurrentEmployer"]),
                                                                       AnnualSalry = Convert.ToString(dr["AnnualSalary"]),
                                                                       UGCourses = Convert.ToString(dr["UGCourses"]),
                                                                       PGCourses = Convert.ToString(dr["PGCourses"]),
                                                                       PPGCourses = Convert.ToString(dr["PPGCourses"]),
                                                                       ProfileName = Convert.ToString(dr["ProfileName"]),
                                                                       ProfileId = Convert.ToInt32(dr["Profile"]),
                                                                       CurrentStatus = Convert.ToString(dr["InterViewStatus"]),
                                                                       Remarks = Convert.ToString(dr["Remarks"]),

                                                                       FollowUpDate = dr["FollowUpDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["FollowUpDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                                       InterviewDate = dr["InterViewDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["InterViewDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty,
                                                                       CreatedDate = dr["CreateDate"] != DBNull.Value ? String.Format("{0:" + VISM.DateFormat + "}", DateTime.ParseExact(dr["CreateDate"].ToString(), VISM.DateFormat, CultureInfo.InvariantCulture)) : string.Empty
                                                                   }).ToList();

                        }

                        if (DDLSelectedText != "")
                        {
                            var filetByDDL = VISM.ViewinterviewSchedulemodelList.Where(em => em.CurrentStatus == DDLSelectedText).ToList();
                            VISM.ViewinterviewSchedulemodelList = filetByDDL.ToList();
                        }
                        #endregion
                    }
                }
                else
                {
                    return Json("Sorry your session has expiry. Please try again later", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_SearchInterviewScheduleByText", VISM);
        }

        #endregion

        #region Manage-Profile
        public ActionResult manage_profile(int? id)
        {
            JobProfileModel JPM = new JobProfileModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (id != null)
                {
                    var getProfileData = db.crm_jobprofiletbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getProfileData != null)
                    {
                        JPM.Profile = getProfileData.Profile;
                    }
                }
                ViewBag.result = db.crm_jobprofiletbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(JPM);
        }

        [HttpPost]
        public ActionResult manage_profile(JobProfileModel JPM, int? id)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (id != null)
                {
                    var GetData = db.crm_jobprofiletbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        GetData.Profile = JPM.Profile.Trim();
                        GetData.BranchID = BranchID;
                        GetData.CompanyID = CompanyID;
                        db.SaveChanges();
                        TempData["success"] = "Profile updated successfully";
                    }
                }
                else
                {
                    crm_jobprofiletbl jpt = new crm_jobprofiletbl();
                    jpt.Profile = JPM.Profile.Trim();
                    jpt.Status = true;
                    jpt.BranchID = BranchID;
                    jpt.CompanyID = CompanyID;
                    db.crm_jobprofiletbl.Add(jpt);
                    if (db.SaveChanges() > 0)
                    {
                        TempData["success"] = "Profile addedd successfully";
                    }
                    else
                    {
                        TempData["alert"] = "There is some problem";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/hr/manage_profile");
        }

        public JsonResult ChangeProfileStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_jobprofiletbl set Status=case when Status=1 then 0 else 1 end where Id=" + id);
                msg = "ok";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public JsonResult GetEmployee()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            //var emp = db.crm_usertbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(em => em.Fname).ThenBy(em => em.Lname).ToList();
            Int32? UID = 0;

            UID = Convert.ToInt32(Session["UID"]);
            Int32? UserID = Convert.ToInt32(Session["UID"]);

            EmployeeLogHistory ELH = new EmployeeLogHistory();
            var AssignList1 = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == UID).OrderBy(em => em.Fname).FirstOrDefault();

            if (AssignList1 != null)
            {
                if (AssignList1.MappedUsers != null)
                {
                    List<EmployeeLogHistory> assignToList = new List<EmployeeLogHistory>();
                    var GetMapUser = AssignList1.MappedUsers.Split(',');
                    foreach (var item in GetMapUser)
                    {
                        var mapid = Convert.ToInt32(item);
                        var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetMapUserData != null)
                        {
                            int roleid = Convert.ToInt32(GetMapUserData.ProfileId);
                            var roledata = db.crm_roleassigntbl.Where(em => em.Id == roleid && em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).FirstOrDefault();
                            if (roledata.ViewTicket == true)
                            {
                                EmployeeLogHistory ouser = new EmployeeLogHistory();
                                ouser.UserID = mapid;
                                ouser.UserName = GetMapUserData.Fname + ' ' + GetMapUserData.Lname + '(' + GetMapUserData.EmployeeCode + ')';
                                assignToList.Add(ouser);
                            }
                        }
                    }
                    ELH.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
                }
                else
                {
                    var AssignList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
                    if (AssignList != null)
                    {
                        List<EmployeeLogHistory> assignToList = new List<EmployeeLogHistory>();
                        foreach (var item in AssignList)
                        {
                            EmployeeLogHistory CRM = new EmployeeLogHistory();
                            CRM.UserID = item.Id;
                            CRM.UserName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                            assignToList.Add(CRM);
                        }

                        ELH.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
                    }
                }
            }

            return Json(ELH, JsonRequestBehavior.AllowGet);
        }

        #region Manage Salary
        public ActionResult HRSalary(int? EmployeeName, string Month)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            SalaryModel SM = new SalaryModel();
            string thisMonth = "";
            if (Month == null)
            {
                DateTime dt = DateTime.Today;
                thisMonth = dt.ToString("MMMM-yyyy");
                Month = thisMonth;
            }

            Session["Myformat"] = thisMonth;

            SM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Status == true).OrderBy(em => em.Fname).ToList();
            if (getEmployeeList != null)
            {
                List<SalaryModel> CUMEmployeeList = new List<SalaryModel>();
                foreach (var item in getEmployeeList)
                {
                    SalaryModel CUMEmployee = new SalaryModel();
                    CUMEmployee.UserID = item.Id;

                    CUMEmployee.FullName = item.Fname + ' ' + ' ' + item.Lname + ' ' + '(' + item.EmployeeCode + ')';
                    CUMEmployeeList.Add(CUMEmployee);
                }
                SM.EmployeeList = CUMEmployeeList;
            }

            DataTable getSalaryHistory = DataAccessLayer.GetDataTable("call CRM_SalaryHistoryEmployeeList(" + BranchID + "," + CompanyID + ",'" + EmployeeName + "','" + Month + "')");
            if (getSalaryHistory.Rows.Count > 0)
            {
                List<SalaryModel> SMList = new List<SalaryModel>();
                for (int i = 0; i < getSalaryHistory.Rows.Count; i++)
                {
                    SalaryModel sModel = new SalaryModel();
                    sModel.UserID = Convert.ToInt32(getSalaryHistory.Rows[i]["EmployeeID"]);
                    sModel.SalaryID = Convert.ToInt32(getSalaryHistory.Rows[i]["SalaryID"]);
                    sModel.FullName = Convert.ToString(getSalaryHistory.Rows[i]["FullName"]);
                    sModel.EmployeeCode = Convert.ToString(getSalaryHistory.Rows[i]["EmployeeCode"]);
                    sModel.BankName = Convert.ToString(getSalaryHistory.Rows[i]["BankName"]);
                    sModel.Month = Convert.ToString(getSalaryHistory.Rows[i]["Month"]);
                    sModel.AccountNumber = Convert.ToString(getSalaryHistory.Rows[i]["AccountNumber"]);
                    sModel.BranchName = Convert.ToString(getSalaryHistory.Rows[i]["BranchName"]);
                    sModel.TotalSalary = getSalaryHistory.Rows[i]["TotalSalary"] == DBNull.Value ? 0 : Convert.ToDecimal(getSalaryHistory.Rows[i]["TotalSalary"]);
                    sModel.SalerySlipName = Convert.ToString(getSalaryHistory.Rows[i]["SalarySlipName"]);
                    SMList.Add(sModel);
                }
                SM.SaleryHistoryList = SMList.OrderByDescending(em => em.Month).ToList();
            }
            return View(SM);
        }

        public ActionResult HRCreateSalary(Int32? SalaryID, Int32? EmployeeID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            SalaryModel SM = new SalaryModel();
            try
            {
                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Status == true).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<SalaryModel> CUMEmployeeList = new List<SalaryModel>();
                    foreach (var item in getEmployeeList)
                    {
                        SalaryModel CUMEmployee = new SalaryModel();
                        CUMEmployee.UserID = item.Id;
                        CUMEmployee.FullName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                        CUMEmployeeList.Add(CUMEmployee);
                    }
                    SM.EmployeeList = CUMEmployeeList;
                }

                if (EmployeeID > 0 && SalaryID > 0)
                {
                    var getEmployeeSalary = db.crm_salaryhistory.Where(em => em.ID == SalaryID && em.EmployeeID == EmployeeID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getEmployeeSalary != null)
                    {
                        SM.SalaryID = getEmployeeSalary.ID;
                        SM.UserID = getEmployeeSalary.EmployeeID;
                        SM.Month = getEmployeeSalary.Month;
                        SM.BankName = getEmployeeSalary.BankName;
                        SM.BranchName = getEmployeeSalary.BranchName;
                        SM.AccountNumber = getEmployeeSalary.AccountNumber;
                        SM.BasicSalary = getEmployeeSalary.BasicSalary;
                        SM.HRA = getEmployeeSalary.HRA;
                        SM.TravellingAllowance = getEmployeeSalary.TravellingAllowance;
                        SM.MedicalAllowance = getEmployeeSalary.MedicalAllowance;
                        SM.PerformanceIncentive = getEmployeeSalary.PerformanceIncentive;
                        SM.OtherBenefits = getEmployeeSalary.OtherBenefits;
                        SM.PFEmployeeShare = getEmployeeSalary.PFEmployeeShare;
                        SM.PFEmployerShare = getEmployeeSalary.PFEmployerShare;
                        SM.ESICEmployerEmployee = getEmployeeSalary.ESICEmployerEmployee;
                        SM.TDS = getEmployeeSalary.TDS;
                        SM.IFSCCode = getEmployeeSalary.IFCSCode;
                        SM.LWF = getEmployeeSalary.LWF;
                        SM.Security = getEmployeeSalary.Security;
                        SM.Advance = getEmployeeSalary.Advance;
                        SM.LWP = getEmployeeSalary.LWP;
                        SM.OtherDeduction = getEmployeeSalary.OtherDeduction;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return RedirectToAction("HRSalary");
            }

            return View(SM);
        }

        [HttpPost]
        public ActionResult HRCreateSalary(SalaryModel SM, Int32? SalaryID)
        {
            var slId = SalaryID;
            try
            {
                string Message = string.Empty;
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                #region Calculate-salary by days of working.......

                var calday = 0;
                var MStartDate = string.Empty;
                var MEndDate = string.Empty;

                if (!string.IsNullOrEmpty(SM.Month))
                {
                    int monthNo = DateTime.ParseExact(SM.Month, "MMMM-yyyy", System.Globalization.CultureInfo.CurrentCulture).Month;
                    int cyear = DateTime.ParseExact(SM.Month, "MMMM-yyyy", System.Globalization.CultureInfo.CurrentCulture).Year;

                    var startdate = new DateTime(cyear, monthNo, 1);
                    var enddate = startdate.AddMonths(1).AddDays(-1);

                    MStartDate = startdate.ToString("dd/MM/yyyy");
                    MEndDate = enddate.ToString("dd/MM/yyyy");
                    calday = (enddate - startdate).Days + 1;

                }


                #region employee attendence
                int Loginontime = 0;
                int Loginofftime = 0;
                int ExtraHoursDay = 0;
                int SatAndSun = 0;
                int Absent = 0;
                int Present = 0;
                decimal PaySalary = 0;
                decimal perDaySalary = 0;
                decimal lossOfPay = 0;

                var getAttandanceByEmp = new List<EmpAttandanceRepotModel>();
                DataTable GetTodayLeads = DataAccessLayer.GetDataTable("call CRM_GetEmpAttandanceByID('" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + "," + SM.UserID + ")");

                if (GetTodayLeads.Rows.Count > 0)
                {
                    getAttandanceByEmp = (from dr in GetTodayLeads.AsEnumerable()
                                          select new EmpAttandanceRepotModel()
                                          {
                                              EmpId = Convert.ToInt32(dr["EmpId"]),
                                              LoginDate = Convert.ToString(dr["L_In_Date"]),
                                              LoginTime = Convert.ToString(dr["L_In_Time"]),
                                              LogoutDate = Convert.ToString(dr["L_Out_Date"]),
                                              LogoutTime = Convert.ToString(dr["L_Out_Time"]),
                                              WorkDuration = Convert.ToString(dr["Duration"]),
                                              WorkingLateNight = Convert.ToBoolean((Convert.ToString(dr["Working_Late_Night"]) == null || Convert.ToString(dr["Working_Late_Night"]) == "") ? 0 : dr["Working_Late_Night"]),
                                              ExtraWorking = Convert.ToBoolean((Convert.ToString(dr["Extra_working"]) == null || Convert.ToString(dr["Extra_working"]) == "") ? 0 : dr["Extra_working"]),
                                              LogZoneTime = Convert.ToString(dr["LogTimeZone"])
                                          }).ToList();
                }


                foreach (var Aitem in getAttandanceByEmp)
                {
                    if (Aitem.LogZoneTime == "IST")
                    {
                        var MatchCondition = Convert.ToDateTime("10:15:00 AM").TimeOfDay;
                        var LogDate = Convert.ToDateTime(Aitem.LoginDate).DayOfWeek;
                        var logt = Convert.ToDateTime(Aitem.LoginTime).TimeOfDay;
                        var duration = Convert.ToString(Aitem.WorkDuration);
                        var spliteDuration = "0";
                        if (!string.IsNullOrEmpty(duration))
                        {
                            spliteDuration = duration.Substring(0, 2);//this code added on 07/12/2020 for correct extra hours get
                        }

                        if (Convert.ToString(LogDate) == "Saturday" || Convert.ToString(LogDate) == "Sunday")
                        {
                            //Start to cal Saturday and sunday.....
                            SatAndSun++;

                            #region code modify on 07/12/2020 for present on time/extra hours perday etc
                            if (logt <= MatchCondition && Convert.ToInt32(spliteDuration) >= 9)
                            {
                                //Start to cal Log on Time And Duration 9Hrs Complete....
                                if (Convert.ToInt32(spliteDuration) > 9)
                                {
                                    ExtraHoursDay++;
                                    Loginontime++;
                                }
                                else
                                {
                                    Loginontime++;
                                }
                            }
                            else if (logt <= MatchCondition)
                            {
                                Loginontime++;
                            }
                            else
                            {
                                Loginofftime++;
                            }
                            #endregion
                        }
                        else if (logt <= MatchCondition && Convert.ToInt32(spliteDuration) >= 9)
                        {
                            //Start to cal Log on Time And Duration 9Hrs Complete....
                            if (Convert.ToInt32(spliteDuration) > 9)
                            {
                                ExtraHoursDay++;
                                Loginontime++;
                            }
                            else
                            {
                                Loginontime++;
                            }
                        }
                        else if (logt <= MatchCondition)//this line added on 07/12/2020 for present on time if extra hours not get
                        {
                            Loginontime++;
                        }
                        else if (logt > MatchCondition)
                        {
                            //Start to calculate Off-time Login
                            Loginofftime++;
                        }
                    }
                }

                Absent = calday - (Loginontime + Loginofftime /*+ SatAndSun*/);
                Present = calday - Absent;

                #region Start to Calculate-salary

                if (SM.BasicSalary != null)
                {
                    decimal basicSal = SM.BasicSalary != null ? SM.BasicSalary ?? 0 : 0;
                    decimal hra = SM.HRA != null ? SM.HRA ?? 0 : 0;
                    decimal TravellingAllowance = SM.TravellingAllowance != null ? SM.TravellingAllowance ?? 0 : 0;
                    decimal MedicalAllowance = SM.MedicalAllowance != null ? SM.MedicalAllowance ?? 0 : 0;
                    decimal PerformanceIncentive = SM.PerformanceIncentive != null ? SM.PerformanceIncentive ?? 0 : 0;
                    decimal OtherBenefits = SM.OtherBenefits != null ? SM.OtherBenefits ?? 0 : 0;
                    decimal PFEmployeeShare = SM.PFEmployeeShare != null ? SM.PFEmployeeShare ?? 0 : 0;
                    decimal PFEmployerShare = SM.PFEmployerShare != null ? SM.PFEmployerShare ?? 0 : 0;
                    decimal ESICEmployerEmployee = SM.ESICEmployerEmployee != null ? SM.ESICEmployerEmployee ?? 0 : 0;
                    decimal TDS = SM.TDS != null ? SM.TDS ?? 0 : 0;
                    decimal OtherDeduction = SM.OtherDeduction != null ? SM.OtherDeduction ?? 0 : 0;

                    Decimal GrossPay = basicSal + hra + TravellingAllowance + MedicalAllowance + PerformanceIncentive + OtherBenefits;

                    perDaySalary = Convert.ToDecimal(GrossPay / calday);
                    //PaySalary = Convert.ToDecimal(perDaySalary * Present);
                    lossOfPay = Convert.ToDecimal(perDaySalary * Absent);

                    Decimal TotalDeducation = PFEmployeeShare + PFEmployerShare + ESICEmployerEmployee + TDS + OtherDeduction + lossOfPay;
                    Decimal NetPayAmount = GrossPay - TotalDeducation;
                    PaySalary = NetPayAmount > 0 ? NetPayAmount : 0;
                }
                #endregion

                #endregion

                #endregion


                if (SalaryID > 0)
                {
                    var GetUpdateRecods = db.crm_salaryhistory.Where(em => em.ID == SalaryID).FirstOrDefault();

                    if (GetUpdateRecods != null)
                    {
                        GetUpdateRecods.BankName = SM.BankName;
                        GetUpdateRecods.BranchName = SM.BranchName;
                        GetUpdateRecods.AccountNumber = SM.AccountNumber;
                        GetUpdateRecods.BasicSalary = SM.BasicSalary;
                        GetUpdateRecods.HRA = SM.HRA;
                        GetUpdateRecods.Month = SM.Month;
                        GetUpdateRecods.TravellingAllowance = SM.TravellingAllowance;
                        GetUpdateRecods.MedicalAllowance = SM.MedicalAllowance;
                        GetUpdateRecods.PerformanceIncentive = SM.PerformanceIncentive;
                        GetUpdateRecods.OtherBenefits = SM.OtherBenefits;
                        GetUpdateRecods.PFEmployeeShare = SM.PFEmployeeShare;
                        GetUpdateRecods.PFEmployerShare = SM.PFEmployerShare;
                        GetUpdateRecods.ESICEmployerEmployee = SM.ESICEmployerEmployee;
                        GetUpdateRecods.TDS = SM.TDS;
                        GetUpdateRecods.OtherDeduction = SM.OtherDeduction;
                        GetUpdateRecods.ModifiedOn = Constant.GetBharatTime();
                        GetUpdateRecods.ModifiedBy = Convert.ToInt32(Session["UID"]);
                        GetUpdateRecods.PaySalary = PaySalary;
                        GetUpdateRecods.IFCSCode = SM.IFSCCode;
                        GetUpdateRecods.LWF = SM.LWF;
                        GetUpdateRecods.Security = SM.Security;
                        GetUpdateRecods.Advance = SM.Advance;
                        GetUpdateRecods.LWP = SM.LWP;
                        db.SaveChanges();
                        TempData["success"] = "Salary updated successfully";
                        Message = "Salary updated successfully";

                    }
                    else
                    {
                        TempData["alert"] = "There is some problem";
                        return Redirect("/HR/HRSalary");
                    }
                }
                else
                {
                    var GetRecods = db.crm_salaryhistory.Where(em => em.EmployeeID == SM.UserID && em.Month == SM.Month).FirstOrDefault();
                    if (GetRecods == null)
                    {
                        crm_salaryhistory CSH = new crm_salaryhistory();
                        CSH.EmployeeID = SM.UserID;
                        CSH.BankName = SM.BankName;
                        CSH.BranchName = SM.BranchName;
                        CSH.AccountNumber = SM.AccountNumber;
                        CSH.BasicSalary = SM.BasicSalary;
                        CSH.HRA = SM.HRA;
                        CSH.Month = SM.Month;
                        CSH.TravellingAllowance = SM.TravellingAllowance;
                        CSH.MedicalAllowance = SM.MedicalAllowance;
                        CSH.PerformanceIncentive = SM.PerformanceIncentive;
                        CSH.OtherBenefits = SM.OtherBenefits;
                        CSH.PFEmployeeShare = SM.PFEmployeeShare;
                        CSH.PFEmployerShare = SM.PFEmployerShare;
                        CSH.ESICEmployerEmployee = SM.ESICEmployerEmployee;
                        CSH.TDS = SM.TDS;
                        CSH.OtherDeduction = SM.OtherDeduction;
                        CSH.CreatedOn = Constant.GetBharatTime();
                        CSH.CreatedBy = Convert.ToInt32(Session["UID"]);
                        CSH.CompanyID = CompanyID;
                        CSH.BranchID = BranchID;
                        CSH.PaySalary = PaySalary;
                        CSH.IFCSCode = SM.IFSCCode;
                        CSH.LWF = SM.LWF;
                        CSH.Security = SM.Security;
                        CSH.Advance = SM.Advance;
                        CSH.LWP = SM.LWP;
                        db.crm_salaryhistory.Add(CSH);
                        db.SaveChanges();
                        TempData["success"] = "Salary added successfully";
                        Message = "Salary added successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Record Already";
                    }
                }

                #region Save Salary Slip in PDF Format
                if (Message != "")
                {
                    string htmlBody = string.Empty;
                    string fileName = string.Empty;
                    string output_path_pdf = string.Empty;
                    string companyName = string.Empty;
                    string companyAddress = string.Empty;
                    string companyWebsite = string.Empty;
                    string companyPhoneNo = string.Empty;
                    string companyLogo = string.Empty;

                    var companyProfile = db.company_profile.Find(CompanyID);
                    if (CompanyID == 296)
                    {
                        companyName = "Nicole Infosoft Pvt.Ltd.";
                        companyAddress = "Plot Number-113, Sector-44,\n Gurgaon, Haryana-122003,";
                        companyPhoneNo = "0124-4223060";
                        companyWebsite = "www.nicoleinfosoft.com";
                        companyLogo = "http://crm.smartcapita.com/img/nislogo.png";
                    }
                    else
                    {
                        if (companyProfile != null)
                        {
                            companyName = companyProfile.Organization;
                            companyAddress = companyProfile.Address;
                            companyPhoneNo = companyProfile.Phoneno;
                            companyWebsite = companyProfile.Website;
                            string filePath = companyProfile.FilePath;
                            string imgPath = "https://www.smartcapita.com/" + filePath;
                            companyLogo = imgPath;
                        }
                    }

                    DataTable GetEmployeeSalary = DataAccessLayer.GetDataTable("call CRM_EmployeeSalaryShip(" + SM.UserID + "," + BranchID + "," + CompanyID + ",'" + SM.Month + "')");
                    if (GetEmployeeSalary.Rows.Count > 0)
                    {
                        SM.FullName = Convert.ToString(GetEmployeeSalary.Rows[0]["FullName"]);
                        SM.EmployeeCode = Convert.ToString(GetEmployeeSalary.Rows[0]["EmployeeCode"]);
                        SM.Designation = Convert.ToString(GetEmployeeSalary.Rows[0]["ProfileName"]);
                        SM.BankName = Convert.ToString(GetEmployeeSalary.Rows[0]["BankName"]);
                        SM.BranchName = Convert.ToString(GetEmployeeSalary.Rows[0]["BranchName"]);
                        SM.AccountNumber = Convert.ToString(GetEmployeeSalary.Rows[0]["AccountNumber"]);
                        SM.Month = Convert.ToString(GetEmployeeSalary.Rows[0]["Month"]);
                        SM.PaySalary = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["PaySalary"]);
                        if (GetEmployeeSalary.Rows[0]["BasicSalary"] != DBNull.Value)
                        {
                            SM.BasicSalary = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["BasicSalary"]);
                        }
                        else
                        {
                            SM.BasicSalary = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["HRA"] != DBNull.Value)
                        {
                            SM.HRA = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["HRA"]);
                        }
                        else
                        {
                            SM.HRA = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["TravellingAllowance"] != DBNull.Value)
                        {
                            SM.TravellingAllowance = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["TravellingAllowance"]);
                        }
                        else
                        {
                            SM.TravellingAllowance = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["MedicalAllowance"] != DBNull.Value)
                        {
                            SM.MedicalAllowance = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["MedicalAllowance"]);
                        }
                        else
                        {
                            SM.MedicalAllowance = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["PerformanceIncentive"] != DBNull.Value)
                        {
                            SM.PerformanceIncentive = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["PerformanceIncentive"]);
                        }
                        else
                        {
                            SM.PerformanceIncentive = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["OtherBenefits"] != DBNull.Value)
                        {
                            SM.OtherBenefits = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["OtherBenefits"]);
                        }
                        else
                        {
                            SM.OtherBenefits = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["PFEmployeeShare"] != DBNull.Value)
                        {
                            SM.PFEmployeeShare = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["PFEmployeeShare"]);
                        }
                        else
                        {
                            SM.PFEmployeeShare = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["PFEmployerShare"] != DBNull.Value)
                        {
                            SM.PFEmployerShare = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["PFEmployerShare"]);
                        }
                        else
                        {
                            SM.PFEmployerShare = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["ESICEmployerEmployee"] != DBNull.Value)
                        {
                            SM.ESICEmployerEmployee = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["ESICEmployerEmployee"]);
                        }
                        else
                        {
                            SM.ESICEmployerEmployee = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["TDS"] != DBNull.Value)
                        {
                            SM.TDS = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["TDS"]);
                        }
                        else
                        {
                            SM.TDS = 0;
                        }
                        if (GetEmployeeSalary.Rows[0]["OtherDeduction"] != DBNull.Value)
                        {
                            SM.OtherDeduction = Convert.ToDecimal(GetEmployeeSalary.Rows[0]["OtherDeduction"]);
                        }
                        else
                        {
                            SM.OtherDeduction = 0;
                        }
                        Decimal? GrossPay = SM.BasicSalary + SM.HRA + SM.TravellingAllowance + SM.MedicalAllowance + SM.PerformanceIncentive + SM.OtherBenefits;
                        Decimal? TotalDeducation = SM.PFEmployeeShare + SM.PFEmployerShare + SM.ESICEmployerEmployee + SM.TDS + SM.OtherDeduction + lossOfPay;
                        Decimal? NetPayAmount = GrossPay - TotalDeducation;
                        NetPayAmount = NetPayAmount > 0 ? NetPayAmount : 0;
                        CurrencyToWordINR CINR = new CurrencyToWordINR();
                        //string ConvertInWord = CINR.changeNumericToWords(Convert.ToDouble(SM.PaySalary));
                        string ConvertInWord = CINR.changeNumericToWords(Math.Ceiling(Convert.ToDouble(NetPayAmount)));
                        htmlBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/MailTemplate/SalarySlip.html"));
                        htmlBody = htmlBody.Replace("##CompanyLogo##", companyLogo);
                        htmlBody = htmlBody.Replace("##CompanyName##", companyName);
                        htmlBody = htmlBody.Replace("##CompanyAddress##", companyAddress);
                        htmlBody = htmlBody.Replace("##CompanyPhoneNo##", companyPhoneNo);
                        htmlBody = htmlBody.Replace("##CompanyWebsite##", companyWebsite);
                        htmlBody = htmlBody.Replace("##Name##", SM.FullName);
                        htmlBody = htmlBody.Replace("##BankName##", SM.BankName);
                        htmlBody = htmlBody.Replace("##Designation##", SM.Designation);
                        htmlBody = htmlBody.Replace("##BranchName##", SM.BranchName);
                        htmlBody = htmlBody.Replace("##EmployeeCode##", SM.EmployeeCode);
                        htmlBody = htmlBody.Replace("##AccountNumber##", SM.AccountNumber);
                        htmlBody = htmlBody.Replace("##Month##", SM.Month.Replace("-", " "));
                        htmlBody = htmlBody.Replace("##BasicSalary##", Convert.ToString(SM.BasicSalary));
                        htmlBody = htmlBody.Replace("##PFEmployeeShare##", Convert.ToString(SM.PFEmployeeShare));
                        htmlBody = htmlBody.Replace("##HRA##", Convert.ToString(SM.HRA));
                        htmlBody = htmlBody.Replace("##PFEmployerShare##", Convert.ToString(SM.PFEmployerShare));
                        htmlBody = htmlBody.Replace("##ESICEmployerEmployee##", Convert.ToString(SM.ESICEmployerEmployee));
                        htmlBody = htmlBody.Replace("##MedicalAllowance##", Convert.ToString(SM.MedicalAllowance));
                        htmlBody = htmlBody.Replace("##TravellingAllowance##", Convert.ToString(SM.TravellingAllowance));
                        htmlBody = htmlBody.Replace("##TDS##", Convert.ToString(SM.TDS));
                        htmlBody = htmlBody.Replace("##PerformanceIncentive##", Convert.ToString(SM.PerformanceIncentive));
                        htmlBody = htmlBody.Replace("##OtherDeduction##", Convert.ToString(SM.OtherDeduction));
                        htmlBody = htmlBody.Replace("##OtherBenefits##", Convert.ToString(SM.OtherBenefits));
                        htmlBody = htmlBody.Replace("##LWF##", Convert.ToString(SM.LWF));
                        htmlBody = htmlBody.Replace("##Advance##", Convert.ToString(SM.Advance));
                        htmlBody = htmlBody.Replace("##AbsentDays##", Convert.ToString(Absent));
                        htmlBody = htmlBody.Replace("##PayLoss##", string.Format("{0:0.00}", lossOfPay));
                        htmlBody = htmlBody.Replace("##GrossPay##", Convert.ToString(GrossPay));
                        htmlBody = htmlBody.Replace("##TotalDeduction##", string.Format("{0:0.00}", TotalDeducation));
                        htmlBody = htmlBody.Replace("##NetPayAmount##", string.Format("{0:0.00}", Math.Ceiling(NetPayAmount ?? 0)));
                        htmlBody = htmlBody.Replace("##NetPayWord##", ConvertInWord);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment; filename=test.pdf");
                        fileName = "SalarySlip-" + SM.EmployeeCode.Trim().Replace("/", "").Replace("\t", "") + "-" + SM.FullName.Trim() + "-" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "";
                        output_path_pdf = HttpContext.Server.MapPath("~/PDF/SalarySlip/" + fileName + ".pdf");
                        HtmlToPdfConverter pdfConverter = new HtmlToPdfConverter();
                        //pdfConverter.PageWidth = 700;
                        //pdfConverter.PageHeight = 800;
                        //pdfConverter.Margins = new PageMargins { Top = 2, Bottom = 2, Left = 2, Right = 2 };
                        pdfConverter.GeneratePdf(htmlBody, null, output_path_pdf);

                        var GetRecordUpdate = db.crm_salaryhistory.Where(em => em.EmployeeID == SM.UserID && em.Month == SM.Month).FirstOrDefault();
                        if (GetRecordUpdate != null)
                        {
                            GetRecordUpdate.SalarySlipName = fileName;
                            GetRecordUpdate.ModifiedOn = Constant.GetBharatTime();
                            GetRecordUpdate.ModifiedBy = Convert.ToInt32(Session["UID"]);
                            db.SaveChanges();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return RedirectToAction("HRCreateSalary", new { SalaryID = slId, EmployeeID = SM.UserID });
            }
            return RedirectToAction("HRSalary");
        }

        public ActionResult HRGetEmployeeSalary(Int32? EmployeeID, String Month)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            SalaryModel SM = new SalaryModel();
            var GetEmployeeSalary = db.crm_salarydetail.Where(em => em.UserID == EmployeeID && em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefault();
            if (GetEmployeeSalary != null)
            {
                SM.SalaryID = GetEmployeeSalary.ID;
                SM.BankName = GetEmployeeSalary.BankName;
                SM.BranchName = GetEmployeeSalary.BranchName;
                SM.AccountNumber = GetEmployeeSalary.AccountNumber;
                SM.BasicSalary = GetEmployeeSalary.BasicSalary;
                SM.HRA = GetEmployeeSalary.HRA;
                SM.TravellingAllowance = GetEmployeeSalary.TravellingAllowance;
                SM.MedicalAllowance = GetEmployeeSalary.MedicalAllowance;
                SM.PerformanceIncentive = GetEmployeeSalary.PerformanceIncentive;
                SM.OtherBenefits = GetEmployeeSalary.OtherBenefits;
                SM.PFEmployeeShare = GetEmployeeSalary.PFEmployeeShare;
                SM.PFEmployerShare = GetEmployeeSalary.PFEmployerShare;
                SM.ESICEmployerEmployee = GetEmployeeSalary.ESICEmployerEmployee;
                SM.TDS = GetEmployeeSalary.TDS;
                SM.IFSCCode = GetEmployeeSalary.IFCSCode;
                SM.LWF = GetEmployeeSalary.LWF;
                SM.Advance = GetEmployeeSalary.Advance;
                SM.Security = GetEmployeeSalary.Security;
                SM.LWP = GetEmployeeSalary.LWP;


                #region DateTime-Format
                String MM = String.Empty;
                String YY = String.Empty;
                int monthIndex = 0;
                if (Month != null)
                {
                    string[] monthOF = Month.Split(new char[] { '-' });
                    if (monthOF != null)
                    {
                        MM = monthOF[0];
                        YY = monthOF[1];
                        string[] MonthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                        monthIndex = Array.IndexOf(MonthNames, MM) + 1;
                    }

                    var dd = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(Convert.ToInt32(YY), monthIndex, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

                    #endregion

                    #region Extra Payment
                    Decimal? ExtraAmount = 0;
                    DateTime sDate = new DateTime();
                    DateTime eDate = new DateTime();
                    sDate = Convert.ToDateTime(MStartDate).Date;
                    eDate = Convert.ToDateTime(MEndDate).Date;

                    var GetExtraPaymentList = db.crm_extrapayment.Where(em => em.Month >= sDate && em.Month <= eDate && em.UserID == EmployeeID).ToList();
                    if (GetExtraPaymentList.Count > 0)
                    {
                        foreach (var item in GetExtraPaymentList)
                        {
                            ExtraAmount += item.AdvanceAmount;
                        }
                    }
                    if (GetEmployeeSalary.OtherDeduction == ExtraAmount)
                    {
                        SM.OtherDeduction = GetEmployeeSalary.OtherDeduction;
                    }
                    else
                    {
                        SM.OtherDeduction = ExtraAmount;
                    }
                }
                #endregion
            }
            return Json(SM, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetExtraPayment(Int32? EmployeeID, String Month)
        //{
        //    #region DateTime-Format
        //    var dd = System.DateTime.Now;
        //    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
        //    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
        //    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
        //    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
        //    #endregion

        //    #region Extra Payment
        //    Decimal? ExtraAmount = 0;
        //    DateTime sDate = new DateTime();
        //    DateTime eDate = new DateTime();
        //    sDate = Convert.ToDateTime(MStartDate).Date;
        //    eDate = Convert.ToDateTime(MEndDate).Date;
        //    var GetExtraPaymentList = db.crm_extrapayment.Where(em => em.Month >= sDate && em.Month <= eDate && em.UserID == EmployeeID).ToList();
        //    if (GetExtraPaymentList.Count > 0)
        //    {
        //        foreach (var item in GetExtraPaymentList)
        //        {
        //            ExtraAmount += item.AdvanceAmount;
        //        }
        //    }
        //    #endregion

        //    return Json(ExtraAmount, JsonRequestBehavior.AllowGet);
        //}

        public FileResult Download(string SalarySlipName)
        {
            //string contentType = "application/pdf";
            //var filepath = System.IO.Path.Combine(Server.MapPath(@"~\PDF\SalarySlip\" + SalarySlipName + ".pdf"));
            //return File(filepath, MimeMapping.GetMimeMapping(filepath), contentType);


            string contentType = "application/pdf";
            return File(@"~\PDF\SalarySlip\" + SalarySlipName + ".pdf", contentType);
        }


        #endregion

        public ActionResult EmployeeForm16Request(Int32? EmployeeID, string p_status)
        {
            Form16RequestModel LRM = new Form16RequestModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            if (p_status == null)
            {
                p_status = "In Process";
            }
            LRM.DateFormat = Constant.DateFormat();//get date format by company id
            //Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            //crm_leaverequest_tbl
            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/).OrderBy(em => em.Fname).ToList();

            var getFormList = db.crm_formrequest_tbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/).ToList();
            if (getEmployeeList != null)
            {
                List<Form16RequestModel> CUMEmployeeList = new List<Form16RequestModel>();
                foreach (var item in getEmployeeList)
                {
                    Form16RequestModel CUMEmployee = new Form16RequestModel();
                    CUMEmployee.EmployeeID = item.Id;
                    CUMEmployee.FullName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                    CUMEmployeeList.Add(CUMEmployee);
                }
                LRM.EmployeeList = CUMEmployeeList;
            }

            //DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_RequestLeaveList(" + BranchID + "," + CompanyID + ")");
            DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_FormRequestLists(" + BranchID + "," + CompanyID + ",'" + p_status + "')");
            if (getEmployeeLeaveRequest.Rows.Count > 0)
            {
                List<Form16RequestModel> LRMList = new List<Form16RequestModel>();
                for (int i = 0; i < getEmployeeLeaveRequest.Rows.Count; i++)
                {
                    Form16RequestModel LModel = new Form16RequestModel();
                    LModel.EmployeeID = Convert.ToInt32(getEmployeeLeaveRequest.Rows[i]["EmployeeID"]);
                    LModel.FormName ="Form16";
                    LModel.RequestID = Convert.ToInt64(getEmployeeLeaveRequest.Rows[i]["RequestID"]);
                   // LModel.Subject = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["Subject"]);
                    LModel.ProcessStatus = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessStatus"]);
                    LModel.FileName= Convert.ToString(getEmployeeLeaveRequest.Rows[i]["FileName"]);
                    LModel.RequestDate = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["RequestDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    if (Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessDate"]) != string.Empty)
                    {
                        LModel.ProcessDate = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessDate"]);
                    }
                    else
                    {
                        LModel.ProcessDate = string.Empty;
                    }
                    LModel.EmployeeCode = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["EmployeeCode"]);
                    LModel.FullName = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["FullName"]);
                    LRMList.Add(LModel);
                }
                LRM.LeaveEmployeeList = LRMList.OrderByDescending(em => em.RequestDate).ToList();
            }
            else
            {
                LRM.LeaveEmployeeList = new List<Form16RequestModel>();
            }

            if (EmployeeID > 0 && (LRM.LeaveEmployeeList.Count > 0 || LRM.LeaveEmployeeList != null))
            {
                LRM.LeaveEmployeeList = LRM.LeaveEmployeeList.Where(em => em.EmployeeID == EmployeeID).ToList();
            }
            return View(LRM);
        }

        public ActionResult EmployeeForm16RequestDetail(Int64? RequestID)
        {
            Form16RequestModel LRM = new Form16RequestModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<Form16RequestModel> CUMEmployeeList = new List<Form16RequestModel>();
                    foreach (var item in getEmployeeList)
                    {
                        Form16RequestModel CUMEmployee = new Form16RequestModel();
                        CUMEmployee.EmployeeID = item.Id;
                        CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                        CUMEmployeeList.Add(CUMEmployee);
                    }
                    LRM.EmployeeList = CUMEmployeeList;
                }

                //Call Only Employee is Login
                if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                {
                    int UID = Convert.ToInt32(Session["UID"]);
                    LRM.EmployeeList = LRM.EmployeeList.Where(em => em.EmployeeID == UID).ToList();
                }

                //List<LeaveRequestModel> LeaveTypeList = new List<LeaveRequestModel>
                //{
                //    new LeaveRequestModel { LeaveTypeID =1, LeaveName = "Casual Leave" },
                //    new LeaveRequestModel { LeaveTypeID =2, LeaveName = "Medical Leave" }
                //};
                //LRM.LeaveTypeList = LeaveTypeList;

                var geterrortypeList = db.crm_leavetypename.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                if (geterrortypeList.Count > 0)
                {
                    List<Form16RequestModel> LeaveTypeList = new List<Form16RequestModel>();
                    foreach (var item in geterrortypeList)
                    {
                        Form16RequestModel cError = new Form16RequestModel();
                        cError.FormTypeID = item.ID;
                        cError.FormName = "Form16";
                        LeaveTypeList.Add(cError);
                    }
                    LRM.FormTypeList = LeaveTypeList;
                }

                if (RequestID > 0)
                {
                    var GetEmployeeLeave = db.crm_formrequest_tbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Id == RequestID).FirstOrDefault();
                    if (GetEmployeeLeave != null)
                    {
                        LRM.RequestID = GetEmployeeLeave.Id;
                        LRM.EmployeeID = GetEmployeeLeave.EmployeeID;
                        LRM.FormTypeID = GetEmployeeLeave.FormTypeID;
                       // LRM.Subject = GetEmployeeLeave.Subject;
                        LRM.Message = GetEmployeeLeave.Message;
                        LRM.ProcessStatus = GetEmployeeLeave.ProcessStatus;
                        if (GetEmployeeLeave.ProcessStatus == "Denied")
                        {
                          //  LRM.Comment = GetEmployeeLeave.Comment;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return Redirect("/FormManagement/Form16Request");
            }
            return View(LRM);
        }

        [HttpPost]
        public ActionResult EmployeForm16Process(Form16RequestModel LRM, Int32? RequestID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                var getUpdateRecord = db.crm_formrequest_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == RequestID).FirstOrDefault();
                if (getUpdateRecord != null)
                {
                    if (LRM.ProcessStatus != "Select Status")
                    {
                        getUpdateRecord.ProcessDate = Constant.GetBharatTime();
                        getUpdateRecord.ProcessStatus = LRM.ProcessStatus;
                        //if (LRM.ProcessStatus == "Denied")
                        //{
                        //    getUpdateRecord.Comment = LRM.Comment;
                        //}
                    }
                    if (LRM.postedFile!=null)
                    {
                        string fileName = string.Empty;
                        string destinationPath = string.Empty;
                        // List<FileUploadModel> uploadFileModel = new List<FileUploadModel>();

                        fileName = Path.GetFileName(LRM.postedFile.FileName);
                        destinationPath = Server.MapPath("~/MyFiles/");
                        if (!Directory.Exists(destinationPath))
                        {
                            Directory.CreateDirectory(destinationPath);
                        }
                        destinationPath = Path.Combine(Server.MapPath("~/MyFiles/"), fileName);
                        LRM.postedFile.SaveAs(destinationPath);
                            getUpdateRecord.FileName = LRM.postedFile?.FileName;
                            getUpdateRecord.FilePath = LRM.postedFile?.FileName;
                        
                    }
                    
                    db.SaveChanges();
                    TempData["success"] = "form16 updated successfully";
                    return Redirect("/HR/Employeeform16Request");
                }
                else
                {
                    TempData["alert"] = "There is some problem";
                    return Redirect("/HR/Employeeform16RequestDetail");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/HR/Employeeform16RequestDetail");
            }
        }
        #region LeaveRequest
        public ActionResult EmployeeLeaveRequest(Int32? EmployeeID, string p_status)
        {
            LeaveRequestModel LRM = new LeaveRequestModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            if (p_status == null)
            {
                p_status = "In Process";
            }
            LRM.DateFormat = Constant.DateFormat();//get date format by company id
            //Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/).OrderBy(em => em.Fname).ToList();
            if (getEmployeeList != null)
            {
                List<LeaveRequestModel> CUMEmployeeList = new List<LeaveRequestModel>();
                foreach (var item in getEmployeeList)
                {
                    LeaveRequestModel CUMEmployee = new LeaveRequestModel();
                    CUMEmployee.EmployeeID = item.Id;
                    CUMEmployee.FullName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                    CUMEmployeeList.Add(CUMEmployee);
                }
                LRM.EmployeeList = CUMEmployeeList;
            }

            //DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_RequestLeaveList(" + BranchID + "," + CompanyID + ")");
            DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_LeaveRequestList(" + BranchID + "," + CompanyID + ",'" + p_status + "')");
            if (getEmployeeLeaveRequest.Rows.Count > 0)
            {
                List<LeaveRequestModel> LRMList = new List<LeaveRequestModel>();
                for (int i = 0; i < getEmployeeLeaveRequest.Rows.Count; i++)
                {
                    LeaveRequestModel LModel = new LeaveRequestModel();
                    LModel.EmployeeID = Convert.ToInt32(getEmployeeLeaveRequest.Rows[i]["EmployeeID"]);
                    LModel.LeaveName = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["LeaveName"]);
                    LModel.RequestID = Convert.ToInt64(getEmployeeLeaveRequest.Rows[i]["RequestID"]);
                    LModel.Subject = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["Subject"]);
                    LModel.ProcessStatus = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessStatus"]);
                    LModel.RequestDate = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["RequestDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    if (Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessDate"]) != string.Empty)
                    {
                        LModel.ProcessDate = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["ProcessDate"]);
                    }
                    else
                    {
                        LModel.ProcessDate = string.Empty;
                    }
                    LModel.EmployeeCode = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["EmployeeCode"]);
                    LModel.FullName = Convert.ToString(getEmployeeLeaveRequest.Rows[i]["FullName"]);
                    LRMList.Add(LModel);
                }
                LRM.LeaveEmployeeList = LRMList.OrderByDescending(em => em.RequestDate).ToList();
            }
            else
            {
                LRM.LeaveEmployeeList = new List<LeaveRequestModel>();
            }

            if (EmployeeID > 0 && (LRM.LeaveEmployeeList.Count > 0 || LRM.LeaveEmployeeList != null))
            {
                LRM.LeaveEmployeeList = LRM.LeaveEmployeeList.Where(em => em.EmployeeID == EmployeeID).ToList();
            }
            return View(LRM);
        }

        public ActionResult EmployeeLeaveRequestDetail(Int64? RequestID)
        {
            LeaveRequestModel LRM = new LeaveRequestModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<LeaveRequestModel> CUMEmployeeList = new List<LeaveRequestModel>();
                    foreach (var item in getEmployeeList)
                    {
                        LeaveRequestModel CUMEmployee = new LeaveRequestModel();
                        CUMEmployee.EmployeeID = item.Id;
                        CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                        CUMEmployeeList.Add(CUMEmployee);
                    }
                    LRM.EmployeeList = CUMEmployeeList;
                }

                //Call Only Employee is Login
                if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                {
                    int UID = Convert.ToInt32(Session["UID"]);
                    LRM.EmployeeList = LRM.EmployeeList.Where(em => em.EmployeeID == UID).ToList();
                }

                //List<LeaveRequestModel> LeaveTypeList = new List<LeaveRequestModel>
                //{
                //    new LeaveRequestModel { LeaveTypeID =1, LeaveName = "Casual Leave" },
                //    new LeaveRequestModel { LeaveTypeID =2, LeaveName = "Medical Leave" }
                //};
                //LRM.LeaveTypeList = LeaveTypeList;

                var geterrortypeList = db.crm_leavetypename.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                if (geterrortypeList.Count > 0)
                {
                    List<LeaveRequestModel> LeaveTypeList = new List<LeaveRequestModel>();
                    foreach (var item in geterrortypeList)
                    {
                        LeaveRequestModel cError = new LeaveRequestModel();
                        cError.LeaveTypeID = item.ID;
                        cError.LeaveName = item.LeaveName;
                        LeaveTypeList.Add(cError);
                    }
                    LRM.LeaveTypeList = LeaveTypeList;
                }

                if (RequestID > 0)
                {
                    var GetEmployeeLeave = db.crm_leaverequest_tbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Id == RequestID).FirstOrDefault();
                    if (GetEmployeeLeave != null)
                    {
                        LRM.RequestID = GetEmployeeLeave.Id;
                        LRM.EmployeeID = GetEmployeeLeave.EmployeeID;
                        LRM.LeaveTypeID = GetEmployeeLeave.LeaveTypeID;
                        LRM.Subject = GetEmployeeLeave.Subject;
                        LRM.Message = GetEmployeeLeave.Message;
                        LRM.ProcessStatus = GetEmployeeLeave.ProcessStatus;
                        if (GetEmployeeLeave.ProcessStatus == "Denied")
                        {
                            LRM.Comment = GetEmployeeLeave.Comment;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return Redirect("/LeaveManagement/LeaveRequest");
            }
            return View(LRM);
        }

        
        [HttpPost]
        public ActionResult EmployeLeaveProcess(LeaveRequestModel LRM, Int32? RequestID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                var getUpdateRecord = db.crm_leaverequest_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == RequestID).FirstOrDefault();
                if (getUpdateRecord != null)
                {
                    if (LRM.ProcessStatus != "Select Status")
                    {
                        getUpdateRecord.ProcessDate = Constant.GetBharatTime();
                        getUpdateRecord.ProcessStatus = LRM.ProcessStatus;
                        if (LRM.ProcessStatus == "Denied")
                        {
                            getUpdateRecord.Comment = LRM.Comment;
                        }
                    }
                    db.SaveChanges();
                    TempData["success"] = "Leave updated successfully";
                    return Redirect("/HR/EmployeeLeaveRequest");
                }
                else
                {
                    TempData["alert"] = "There is some problem";
                    return Redirect("/HR/EmployeeLeaveRequestDetail");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/HR/EmployeeLeaveRequestDetail");
            }
        }
        #endregion

        #region Extra Payment Amount

        public ActionResult ManageAdvance(String EmployeeName, String StartDate, String EndDate)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            AdvanceModel AM = new AdvanceModel();

            AM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/ && em.Status == true).OrderBy(em => em.Fname).ToList();
            if (getEmployeeList != null)
            {
                List<AdvanceModel> AMEmployeeList = new List<AdvanceModel>();
                foreach (var item in getEmployeeList)
                {
                    AdvanceModel CUMEmployee = new AdvanceModel();
                    CUMEmployee.UserID = item.Id;
                    CUMEmployee.FullName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                    AMEmployeeList.Add(CUMEmployee);
                }
                AM.EmployeeList = AMEmployeeList;
            }

            #region DateTime-Format
            var dd = Constant.GetBharatTime();
            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
            #endregion

            if (!string.IsNullOrWhiteSpace(StartDate) && !string.IsNullOrWhiteSpace(EndDate))
            {
                if (AM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    TempData["StartDate"] = StartDate;
                    TempData["EndDate"] = EndDate;

                    var fmDate = Convert.ToDateTime(StartDate);
                    var tDate = Convert.ToDateTime(EndDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                }
                else
                {
                    TempData["StartDate"] = StartDate;
                    TempData["EndDate"] = EndDate;
                    MStartDate = StartDate;
                    MEndDate = EndDate;
                }
            }
            else
            {

                if (AM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    TempData["StartDate"] = monthStartDate.ToString(AM.DateFormat);
                    TempData["EndDate"] = MonthendDate.ToString(AM.DateFormat);
                }
                else
                {
                    Session["VLFltrFrmDt"] = MStartDate;
                    Session["VLFltrToDt"] = MEndDate;
                }
            }
            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();
            List<crm_extrapayment> GetExtraPaymentList = new List<crm_extrapayment>();
            if (!String.IsNullOrWhiteSpace(EmployeeName))
            {
                sDate = Convert.ToDateTime(MStartDate).Date;
                eDate = Convert.ToDateTime(MEndDate).Date;
                GetExtraPaymentList = db.crm_extrapayment.Where(em => em.Month >= sDate && em.Month <= eDate && em.FullName == EmployeeName && em.CompanyID == CompanyID && em.BranchID == BranchID).ToList();
            }
            else
            {
                sDate = Convert.ToDateTime(MStartDate).Date;
                eDate = Convert.ToDateTime(MEndDate).Date;
                GetExtraPaymentList = db.crm_extrapayment.Where(em => em.Month >= sDate && em.Month <= eDate && em.CompanyID == CompanyID && em.BranchID == BranchID).ToList();
            }
            if (GetExtraPaymentList.Count > 0)
            {
                List<AdvanceModel> AMList = new List<AdvanceModel>();
                foreach (var item in GetExtraPaymentList)
                {
                    AdvanceModel AModel = new AdvanceModel();
                    AModel.AdvanceID = item.AdvanceID;
                    AModel.FullName = item.FullName;
                    AModel.EmployeeCode = item.EmployeeCode;
                    AModel.AdvanceAmount = item.AdvanceAmount;
                    AModel.Month = item.Month != null ? String.Format("{0:" + AM.DateFormat + "}", Convert.ToDateTime(item.Month)) : string.Empty;
                    AModel.CreatedOn = item.CreatedOn != null ? String.Format("{0:" + AM.DateFormat + "}", Convert.ToDateTime(item.CreatedOn)) : string.Empty;
                    AModel.ModifiedOn = item.ModifiedOn == null ? String.Empty : String.Format("{0:" + AM.DateFormat + "}", Convert.ToDateTime(item.ModifiedOn));
                    AMList.Add(AModel);
                }
                AM.AdvanceModelList = AMList;
            }
            return View(AM);
        }

        public ActionResult AddModifyManageAdvance(Int32? AdvanceID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            AdvanceModel AM = new AdvanceModel();

            AM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/ && em.Status == true).OrderBy(em => em.Fname).ToList();
            if (getEmployeeList != null)
            {
                List<AdvanceModel> AMEmployeeList = new List<AdvanceModel>();
                foreach (var item in getEmployeeList)
                {
                    AdvanceModel CUMEmployee = new AdvanceModel();
                    CUMEmployee.UserID = item.Id;
                    CUMEmployee.FullName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                    AMEmployeeList.Add(CUMEmployee);
                }
                AM.EmployeeList = AMEmployeeList;
            }

            var GetExptaPayment = db.crm_extrapayment.Where(em => em.AdvanceID == AdvanceID && em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefault();
            if (GetExptaPayment != null)
            {
                AM.AdvanceID = GetExptaPayment.AdvanceID;
                AM.AdvanceAmount = GetExptaPayment.AdvanceAmount;
                AM.Month = GetExptaPayment.Month != null ? String.Format("{0:" + AM.DateFormat + "}", Convert.ToDateTime(GetExptaPayment.Month)) : string.Empty;
                AM.UserID = GetExptaPayment.UserID;
                AM.EmployeeCode = GetExptaPayment.EmployeeCode;
                AM.EmployeeEmail = GetExptaPayment.EmployeeEmail;
            }
            return View(AM);
        }

        [HttpPost]
        public ActionResult AddModifyManageAdvance(AdvanceModel AM)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                Int32? EmpID = Convert.ToInt32(AM.UserID);
                var getEmployee = db.crm_usertbl.Where(em => em.Id == EmpID).FirstOrDefault();
                AM.EmployeeCode = getEmployee.EmployeeCode;
                AM.EmployeeEmail = getEmployee.Email;
                AM.FullName = getEmployee.Fname + " " + getEmployee.Lname;
                AM.UserID = getEmployee.Id;

                Int32? AdvanceID = Convert.ToInt32(AM.AdvanceID);
                var getUpdateRecord = db.crm_extrapayment.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.UserID == EmpID && em.AdvanceID == AdvanceID).FirstOrDefault();
                if (getUpdateRecord != null)
                {
                    getUpdateRecord.AdvanceAmount = AM.AdvanceAmount;
                    getUpdateRecord.Month = Convert.ToDateTime(AM.Month);
                    getUpdateRecord.ModifiedBy = Convert.ToInt32(Session["UID"]);
                    getUpdateRecord.ModifiedOn = Constant.GetBharatTime();
                    getUpdateRecord.BranchID = BranchID;
                    getUpdateRecord.CompanyID = CompanyID;
                    db.SaveChanges();
                }
                else
                {
                    crm_extrapayment ep = new crm_extrapayment();
                    ep.UserID = AM.UserID;
                    ep.FullName = AM.FullName;
                    ep.AdvanceAmount = AM.AdvanceAmount;
                    ep.Month = Convert.ToDateTime(AM.Month);
                    ep.EmployeeCode = AM.EmployeeCode;
                    ep.EmployeeEmail = AM.EmployeeEmail;
                    ep.BranchID = BranchID;
                    ep.CompanyID = CompanyID;
                    ep.CreatedBy = Convert.ToInt32(Session["UID"]);
                    ep.CreatedOn = Constant.GetBharatTime();
                    db.crm_extrapayment.Add(ep);
                    db.SaveChanges();
                    TempData["success"] = "Extra amount save successfully";
                }
                return Redirect("/HR/ManageAdvance");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/HR/AddModifyManageAdvance");
            }
        }
        #endregion

        #region Manual Attendance
        //[HttpGet]
        public ActionResult ManualAttendance(int? SearchEmployeeID, string StartDate, string EndDate)
        {
            ManualAttendanceModel MAM = new ManualAttendanceModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var UID = Convert.ToInt32(Session["UID"]);
            var usertype = Convert.ToString(Session["UserType"]);
            MAM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            #region DateTime-Format
            var dd = Constant.GetBharatTime();
            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
            #endregion

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                if (MAM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    TempData["StartDate"] = StartDate;
                    TempData["EndDate"] = EndDate;

                    var fmDate = DateTime.ParseExact(StartDate, MAM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(StartDate);
                    var tDate = DateTime.ParseExact(EndDate, MAM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(EndDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                }
                else
                {
                    TempData["StartDate"] = StartDate;
                    TempData["EndDate"] = EndDate;
                    MStartDate = StartDate;
                    MEndDate = EndDate;
                }
            }
            else
            {
                if (MAM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    TempData["StartDate"] = monthStartDate.ToString(MAM.DateFormat);
                    TempData["EndDate"] = MonthendDate.ToString(MAM.DateFormat);
                }
                else
                {
                    TempData["StartDate"] = MStartDate;
                    TempData["EndDate"] = MEndDate;
                }

            }
            if (CompanyID == 2644)
            {
                if (usertype == "SuperAdmin")
                {
                    var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/ && em.Status == true).OrderBy(em => em.Fname).ToList();
                    if (getEmployeeList != null)
                    {
                        List<ManualAttendanceModel> AMEmployeeList = new List<ManualAttendanceModel>();
                        AMEmployeeList = (from item in getEmployeeList
                                          select new ManualAttendanceModel
                                          {
                                              EmployeeID = item.Id,
                                              EmployeeName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')',
                                          }).ToList();
                        MAM.EmployeeList = AMEmployeeList;
                    }
                }
                else
                {
                    var getEmployeeList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == UID).OrderBy(em => em.Fname).FirstOrDefault();
                    if (getEmployeeList != null)
                    {
                        if (getEmployeeList.MappedUsers != null)
                        {
                            List<ManualAttendanceModel> AMEmployeeList = new List<ManualAttendanceModel>();
                            var GetMapUser = getEmployeeList.MappedUsers.Split(',');
                            foreach (var item in GetMapUser)
                            {
                                var mapid = Convert.ToInt32(item);
                                var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                if (GetMapUserData != null)
                                {
                                    ManualAttendanceModel MAM1 = new ManualAttendanceModel();
                                    MAM1.EmployeeID = mapid;
                                    MAM1.EmployeeName = GetMapUserData.Fname + ' ' + GetMapUserData.Lname + '(' + GetMapUserData.EmployeeCode + ')';
                                    AMEmployeeList.Add(MAM1);
                                    //AMEmployeeList = (from item in getEmployeeList
                                    //                  select new ManualAttendanceModel
                                    //                  {
                                    //                      EmployeeID = item.Id,
                                    //                      EmployeeName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')',
                                    //                  }).ToList();
                                    MAM.EmployeeList = AMEmployeeList;
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/ && em.Status == true).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<ManualAttendanceModel> AMEmployeeList = new List<ManualAttendanceModel>();
                    AMEmployeeList = (from item in getEmployeeList
                                      select new ManualAttendanceModel
                                      {
                                          EmployeeID = item.Id,
                                          EmployeeName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')',
                                      }).ToList();
                    MAM.EmployeeList = AMEmployeeList;
                }
            }
            if (SearchEmployeeID == null)
            {
                SearchEmployeeID = 0;
            }

            List<crm_tbl_nipl_emp_attendance> getManualAttendanceModelList = new List<crm_tbl_nipl_emp_attendance>();
            DataTable GetRecords = DataAccessLayer.GetDataTable("call crm_manualattendance(" + SearchEmployeeID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            if (GetRecords.Rows.Count > 0)
            {
                List<ManualAttendanceModel> MAList = new List<ManualAttendanceModel>();
                for (int i = 0; i < GetRecords.Rows.Count; i++)
                {
                    ManualAttendanceModel MA = new ManualAttendanceModel();
                    MA.ManualID = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
                    MA.EmployeeName = Convert.ToString(GetRecords.Rows[i]["EmployeeName"]);
                    MA.HRName = Convert.ToString(GetRecords.Rows[i]["HRName"]);
                    MA.AttendanceDate = GetRecords.Rows[i]["AttendanceDate"] != DBNull.Value ? String.Format("{0:" + MAM.DateFormat + "}", Convert.ToDateTime(GetRecords.Rows[i]["AttendanceDate"])) : string.Empty;
                    MA.LoginTime = Convert.ToString(GetRecords.Rows[i]["LogTime"]);
                    MA.LogoutTime = Convert.ToString(GetRecords.Rows[i]["LogOutTime"]);
                    MA.Duration = Convert.ToString(GetRecords.Rows[i]["Duration"]);
                    MAList.Add(MA);
                }
                MAM.manualAttendanceModelList = MAList;
            }
            return View(MAM);
        }

        [HttpGet]
        public ActionResult AddModifyManualAttendance(long? ManualID)
        {
            ManualAttendanceModel AM = new ManualAttendanceModel();

            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var UID = Convert.ToInt32(Session["UID"]);
            var usertype = Convert.ToString(Session["UserType"]);
            AM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            if (CompanyID == 2644)
            {
                if (usertype == "SuperAdmin")
                {
                    var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/ && em.Status == true).OrderBy(em => em.Fname).ToList();
                    if (getEmployeeList != null)
                    {
                        List<ManualAttendanceModel> AMEmployeeList = new List<ManualAttendanceModel>();
                        AMEmployeeList = (from item in getEmployeeList
                                          select new ManualAttendanceModel
                                          {
                                              EmployeeID = item.Id,
                                              EmployeeName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')',
                                          }).ToList();
                        AM.EmployeeList = AMEmployeeList;
                    }
                }
                else
                {
                    var getEmployeeList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == UID).OrderBy(em => em.Fname).FirstOrDefault();
                    if (getEmployeeList != null)
                    {
                        if (getEmployeeList.MappedUsers != null)
                        {
                            List<ManualAttendanceModel> AMEmployeeList = new List<ManualAttendanceModel>();
                            var GetMapUser = getEmployeeList.MappedUsers.Split(',');
                            foreach (var item in GetMapUser)
                            {
                                var mapid = Convert.ToInt32(item);
                                var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                if (GetMapUserData != null)
                                {
                                    ManualAttendanceModel MAM1 = new ManualAttendanceModel();
                                    MAM1.EmployeeID = mapid;
                                    MAM1.EmployeeName = GetMapUserData.Fname + ' ' + GetMapUserData.Lname + '(' + GetMapUserData.EmployeeCode + ')';
                                    AMEmployeeList.Add(MAM1);
                                    //AMEmployeeList = (from item in getEmployeeList
                                    //                  select new ManualAttendanceModel
                                    //                  {
                                    //                      EmployeeID = item.Id,
                                    //                      EmployeeName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')',
                                    //                  }).ToList();
                                    AM.EmployeeList = AMEmployeeList;
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID /*&& em.ProfileId != null*/ && em.Status == true).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<ManualAttendanceModel> AMEmployeeList = new List<ManualAttendanceModel>();
                    AMEmployeeList = (from item in getEmployeeList
                                      select new ManualAttendanceModel
                                      {
                                          EmployeeID = item.Id,
                                          EmployeeName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')',
                                      }).ToList();
                    AM.EmployeeList = AMEmployeeList;
                }
            }

            if (ManualID != null)
            {
                var getAttendance = db.crm_tbl_nipl_emp_attendance.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.ID == ManualID).FirstOrDefault();
                if (getAttendance != null)
                {
                    AM.ManualID = getAttendance.ID;
                    AM.EmployeeID = getAttendance.EmpId;
                    if (!string.IsNullOrEmpty(getAttendance.L_In_Date))
                    {
                        var atDate = DateTime.ParseExact(getAttendance.L_In_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);// Convert.ToDateTime(getAttendance.L_In_Date);
                        AM.AttendanceDate = String.Format("{0:" + AM.DateFormat + "}", atDate);
                    }
                    AM.LoginTime = getAttendance.L_In_Time;
                    AM.LogoutTime = getAttendance.L_Out_Time;
                }
            }

            return View(AM);
        }

        [HttpPost]
        public ActionResult AddModifyManualAttendance(ManualAttendanceModel MAM)
        {
            try
            {
                var mac = GetMACAddress();
                if (mac != string.Empty)
                {
                    mac = Regex.Replace(mac, ".{2}", "$0-").TrimEnd('-');
                }

                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                MAM.DateFormat = Constant.DateFormat();//get date format by company id
                string dFormatwithTime = MAM.DateFormat + " hh:mm tt";//concatinate date format with time in 12 hour format

                if (MAM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy
                {
                    var attendenceDate = DateTime.ParseExact(MAM.AttendanceDate, MAM.DateFormat, CultureInfo.InvariantCulture);

                    MAM.AttendanceDate = String.Format("{0:dd/MM/yyyy}", attendenceDate);//convert to dd/MM/yyyy format    
                }

                var getattendance = db.crm_tbl_nipl_emp_attendance.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.EmpId == MAM.EmployeeID && em.L_In_Date == MAM.AttendanceDate).FirstOrDefault();
                if (getattendance != null)
                {
                    getattendance.L_Out_Date = MAM.AttendanceDate;
                    getattendance.L_Out_Time = MAM.LogoutTime;

                    var lgtime = MAM.AttendanceDate + " " + getattendance.L_In_Time;
                    //DateTime logt = DateTime.ParseExact(lgtime, dFormatwithTime, CultureInfo.InvariantCulture); //Convert.ToDateTime(lgtime);
                    DateTime logt = Convert.ToDateTime(lgtime);

                    if (!string.IsNullOrEmpty(MAM.LogoutTime))
                    {
                        var logOuttime = MAM.AttendanceDate + " " + getattendance.L_Out_Time;
                        DateTime logOut = DateTime.ParseExact(logOuttime, dFormatwithTime, CultureInfo.InvariantCulture);// Convert.ToDateTime(logOuttime);
                        TimeSpan duration = logOut.Subtract(logt);
                        string dur = string.Format("{0:00}:{1:00}:{2:00}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
                        getattendance.Duration = dur;
                    }

                    db.SaveChanges();
                }
                else
                {
                    var checktodayAttendance = db.crm_tbl_nipl_emp_attendance.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.EmpId == MAM.EmployeeID && em.L_In_Date == MAM.AttendanceDate).FirstOrDefault();
                    if (checktodayAttendance == null)
                    {
                        var lgtime = MAM.AttendanceDate + " " + MAM.LoginTime;

                        DateTime logt = DateTime.ParseExact(lgtime, dFormatwithTime, CultureInfo.InvariantCulture); //Convert.ToDateTime(lgtime);
                        //DateTime logt = Convert.ToDateTime(lgtime);
                        string dur = "00:00:00";
                        if (!string.IsNullOrEmpty(MAM.LogoutTime))
                        {
                            var logOuttime = MAM.AttendanceDate + " " + MAM.LogoutTime;
                            DateTime logOut = DateTime.ParseExact(logOuttime, dFormatwithTime, CultureInfo.InvariantCulture);// Convert.ToDateTime(logOuttime);
                            TimeSpan duration = logOut.Subtract(logt);
                            dur = string.Format("{0:00}:{1:00}:{2:00}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
                        }


                        crm_tbl_nipl_emp_attendance ATD = new crm_tbl_nipl_emp_attendance();
                        ATD.EmpId = MAM.EmployeeID;
                        ATD.IPAddress = Convert.ToString(Session["UserName"]);
                        ATD.L_In_Date = MAM.AttendanceDate;
                        ATD.L_In_Time = MAM.LoginTime;
                        ATD.L_Out_Date = MAM.AttendanceDate;
                        ATD.L_Out_Time = MAM.LogoutTime;
                        ATD.Duration = dur;
                        ATD.MacAddress = mac;
                        ATD.BranchID = BranchID;
                        ATD.CompanyID = CompanyID;
                        ATD.Status = "P";
                        ATD.AttendanceType = "M";
                        ATD.LogTimeZone = "IST";
                        db.crm_tbl_nipl_emp_attendance.Add(ATD);
                        db.SaveChanges();
                    }
                }
                return Redirect("/HR/ManualAttendance");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/HR/AddModifyManualAttendance");
            }
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
        #endregion

        #region Employee Management

        public ActionResult users()
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (Session["UserName"] != null)
                {
                    ViewBag.result = db.crm_usertbl.Where(em => em.ProfileName != "SuperAdmin" && em.ProfileId == null && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderByDescending(em => em.Created_at).ToList();
                }
                else
                {
                    Session["ReturnUrl"] = "/HR/users";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/home/login");
            }

            return View();
        }

        public ActionResult createusers(int? id)
        {
            CreateUserModel CUM = new CreateUserModel();
            try
            {
                if (Session["UserName"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    CUM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

                    //if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    //{
                    //    ViewBag.result = db.crm_roleassigntbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //    var userList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //    ViewBag.UserList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //}
                    //else
                    //{
                    //    var UID = id;
                    //    var profiletype = Convert.ToString(Session["UserType"]);
                    //    if (profiletype == "Admin" || profiletype == "admin")
                    //    {
                    //        ViewBag.result = db.crm_roleassigntbl.Where(em => em.ProfileName != profiletype && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //    }
                    //    else
                    //    {
                    //        ViewBag.result = db.crm_roleassigntbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //    }
                    //    ViewBag.UserList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.Id != UID && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //}

                    #region Select-TimeZone
                    var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList().Select(em => new crm_zonetbl
                    {
                        ZoneName = em.ZoneName
                    }).AsQueryable();
                    ViewBag.TimeZoneName = new SelectList(GetZoneName, "ZoneName", "ZoneName");
                    #endregion
                    var getbranchList = db.com_branches.Where(em => em.OrganizationID == CompanyID).ToList();
                    if (getbranchList.Count > 0)
                    {
                        List<CreateUserModel> BranchList = new List<CreateUserModel>();
                        foreach (var item in getbranchList)
                        {
                            CreateUserModel cm = new CreateUserModel();
                            cm.BranchID = item.OrgBranchCode;
                            cm.CompanyBranchName = item.BranchName;
                            BranchList.Add(cm);
                        }
                        CUM.obranchList = BranchList;
                    }
                    else
                    {
                        List<CreateUserModel> BranchList = new List<CreateUserModel>();
                        if (CompanyID == 1)
                        {
                            CreateUserModel cm = new CreateUserModel();
                            cm.BranchID = 1;
                            cm.CompanyBranchName = "SmartCapita";
                            BranchList.Add(cm);
                            CUM.obranchList = BranchList;
                        }
                    }

                    var GetUserData = db.crm_usertbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetUserData != null)
                    {
                        CUM.UserID = GetUserData.Id;
                        CUM.FirstName = GetUserData.Fname;
                        CUM.LastName = GetUserData.Lname;
                        CUM.FatherName = GetUserData.FatherName;
                        if (GetUserData.DateofBirth != null)
                        {
                            CUM.DateofBirth = GetUserData.DateofBirth.Value.Date.ToString(CUM.DateFormat);
                        }
                        CUM.ContactNumber = GetUserData.ContactNumber;
                        CUM.ContactNumber = GetUserData.ContactNumber;
                        CUM.AlternateNumber = GetUserData.AlternateNumber;
                        CUM.Email = GetUserData.Email;
                        CUM.Gender = GetUserData.Gender;
                        CUM.EmployeeCode = GetUserData.EmployeeCode;
                        CUM.UserName = GetUserData.UserName;
                        CUM.Designation = GetUserData.Designation;
                        CUM.BranchID = GetUserData.BranchID;
                        //CUM.UserPassword = GetUserData.Password;
                        //CUM.ConfirmPassword = GetUserData.Password;
                        //if (!string.IsNullOrEmpty(GetUserData.KeyVersion))
                        //{
                        //    #region password decryption
                        //    byte[] iv1;
                        //    byte[] key = EncriptAES.getdcriptkey(out iv1);
                        //    string decryptPwd = EncriptAES.DecryptString(GetUserData.Password, key, iv1);
                        //    #endregion
                        //    CUM.UserPassword = decryptPwd;//show decrypted password
                        //    CUM.ConfirmPassword = decryptPwd;
                        //}
                        //else
                        //{
                        //    CUM.UserPassword = GetUserData.Password;
                        //    CUM.ConfirmPassword = GetUserData.Password;
                        //}
                        CUM.TimeZoneName = GetUserData.TimeZone;
                        CUM.CurrentAddress = GetUserData.CurrentAddress;
                        CUM.PermanentAddress = GetUserData.PermanentAddress;
                        CUM.RefName1 = GetUserData.RefName1;
                        CUM.RefEmail1 = GetUserData.RefEmail1;
                        CUM.RefPhoneNumber1 = GetUserData.RefPhoneNumber1;
                        CUM.RefName2 = GetUserData.RefName2;
                        CUM.RefEmail2 = GetUserData.RefEmail2;
                        CUM.RefPhoneNumber2 = GetUserData.RefPhoneNumber2;
                        CUM.ProfileId = Convert.ToInt32(GetUserData.ProfileId);
                        if (GetUserData.MappedUsers != null)
                        {
                            var GetMapUser = GetUserData.MappedUsers.Split(',');
                            foreach (var item in GetMapUser)
                            {
                                var count = new CreateUserModel
                                {
                                    MapUserId = Convert.ToInt32(item)
                                };
                                CUM.mapUserList.Add(count);
                            }
                        }
                    }


                    var SalaryDetail = db.crm_salarydetail.Where(em => em.UserID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (SalaryDetail != null)
                    {
                        CUM.BankName = SalaryDetail.BankName;
                        CUM.BranchName = SalaryDetail.BranchName;
                        CUM.AccountNumber = SalaryDetail.AccountNumber;
                        CUM.MonthlySalary = SalaryDetail.MonthlySalary;
                        CUM.AnnualSalary = SalaryDetail.AnnualSalary;
                        CUM.BasicSalary = SalaryDetail.BasicSalary;
                        CUM.HRA = SalaryDetail.HRA;
                        CUM.TravellingAllowance = SalaryDetail.TravellingAllowance;
                        CUM.MedicalAllowance = SalaryDetail.MedicalAllowance;
                        CUM.PerformanceIncentive = SalaryDetail.PerformanceIncentive;
                        CUM.OtherBenefits = SalaryDetail.OtherBenefits;
                        CUM.PFEmployeeShare = SalaryDetail.PFEmployeeShare;
                        CUM.PFEmployerShare = SalaryDetail.PFEmployerShare;
                        CUM.ESICEmployerEmployee = SalaryDetail.ESICEmployerEmployee;
                        CUM.TDS = SalaryDetail.TDS;
                        CUM.IFSCCode = SalaryDetail.IFCSCode;
                        CUM.LWF = SalaryDetail.LWF;
                        CUM.Security = SalaryDetail.Security;
                        CUM.Advance = SalaryDetail.Advance;
                        CUM.LWP = SalaryDetail.LWP;
                        CUM.OtherDeduction = SalaryDetail.OtherDeduction;
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/HR/createusers/" + id;
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(CUM);
        }

        [HttpPost]
        public ActionResult createusers(CreateUserModel CUM, int? id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    CUM.DateFormat = Constant.DateFormat();//get date format by company id
                    #region Select-TimeZone
                    var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList().Select(em => new crm_zonetbl
                    {
                        ZoneName = em.ZoneName
                    }).AsQueryable();
                    ViewBag.TimeZoneName = new SelectList(GetZoneName, "ZoneName", "ZoneName");
                    #endregion
                    var getbranchList = db.com_branches.Where(em => em.OrganizationID == CompanyID).ToList();
                    if (getbranchList.Count > 0)
                    {
                        List<CreateUserModel> BranchList = new List<CreateUserModel>();
                        foreach (var item in getbranchList)
                        {
                            CreateUserModel cm = new CreateUserModel();
                            cm.BranchID = item.OrgBranchCode;
                            cm.CompanyBranchName = item.BranchName;
                            BranchList.Add(cm);
                        }
                        CUM.obranchList = BranchList;
                    }
                    else
                    {
                        List<CreateUserModel> BranchList = new List<CreateUserModel>();
                        if (CompanyID == 1)
                        {
                            CreateUserModel cm = new CreateUserModel();
                            cm.BranchID = 1;
                            cm.CompanyBranchName = "SmartCapita";
                            BranchList.Add(cm);
                            CUM.obranchList = BranchList;
                        }
                    }


                    //if (!ModelState.IsValid)
                    //{
                    //    //var message = string.Join(", ", ModelState.Values.SelectMany(a => a.Errors).Select(a => a.ErrorMessage));
                    //    //TempData["alert"] = message;
                    //    return View(CUM);
                    //}



                    var dd = Constant.GetBharatTime();

                    #region No. of User assign as per company Profile
                    ///Total no. of user in company_profile table
                    ///after that you can create the user in crm against a company
                    //Int32? CountNoOfUsers = 0;
                    //Int32? CurentCountNoOfUsers = 0;
                    //var countUsers = db.company_profile.Where(em => em.ID == CompanyID).FirstOrDefault();
                    //if (countUsers != null)
                    //{
                    //    CountNoOfUsers = /*countUsers.No_of_users == null ? 3 : */countUsers.No_of_users;
                    //}

                    //var countcurrentUsers = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileName != "SuperAdmin").ToList();
                    //if (countcurrentUsers.Count > 0)
                    //{
                    //    CurentCountNoOfUsers = countcurrentUsers.Count();
                    //}
                    #endregion

                    #region calculate the Maaped user List
                    string Mappeduserlist = null;
                    if (Request.Form["MapperUserList"] != null)
                    {
                        Mappeduserlist = Request.Form["MapperUserList"].ToString();
                    }
                    #endregion

                    if (Session["UserName"] != null)
                    {
                        //if (CUM.UserProfile != null && CUM.UserProfile != "")
                        //{
                        //var getProfille = CUM.UserProfile.Split(',');
                        if (id != null)
                        {
                            var GetUpdateRecords = db.crm_usertbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (GetUpdateRecords != null)
                            {
                                GetUpdateRecords.Fname = CUM.FirstName;
                                GetUpdateRecords.Lname = CUM.LastName;
                                GetUpdateRecords.FatherName = CUM.FatherName;
                                if (!string.IsNullOrWhiteSpace(CUM.DateofBirth))
                                {
                                    GetUpdateRecords.DateofBirth = Convert.ToDateTime(CUM.DateofBirth); //DateTime.ParseExact(CUM.DateofBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    GetUpdateRecords.DateofBirth = null;
                                }
                                GetUpdateRecords.ContactNumber = CUM.ContactNumber;
                                GetUpdateRecords.ContactNumber = CUM.ContactNumber;
                                GetUpdateRecords.AlternateNumber = CUM.AlternateNumber;
                                GetUpdateRecords.Email = CUM.Email;
                                GetUpdateRecords.Gender = CUM.Gender;
                                GetUpdateRecords.EmployeeCode = CUM.EmployeeCode;
                                GetUpdateRecords.Designation = CUM.Designation;

                                #region Password auto generate and encryption
                                //    string VersionKey = "";
                                //    byte[] iv1;

                                //    VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                                //   CUM.UserPassword= Guid.NewGuid().ToString("n").Substring(0, 6);

                                //byte[] key = EncriptAES.getdcriptkey(out iv1);
                                //    string ecncryptPwd = EncriptAES.EncryptString(CUM.UserPassword, key, iv1);
                                #endregion
                                //GetUpdateRecords.Password = ecncryptPwd;//save ecncrypted password 
                                //GetUpdateRecords.KeyVersion = VersionKey;//save latest key version                               
                                //GetUpdateRecords.Password = CUM.UserPassword;
                                GetUpdateRecords.TimeZone = CUM.TimeZoneName;
                                GetUpdateRecords.CurrentAddress = CUM.CurrentAddress;
                                GetUpdateRecords.PermanentAddress = CUM.PermanentAddress;
                                GetUpdateRecords.RefName1 = CUM.RefName1;
                                GetUpdateRecords.RefEmail1 = CUM.RefEmail1;
                                GetUpdateRecords.RefPhoneNumber1 = CUM.RefPhoneNumber1;
                                GetUpdateRecords.RefName2 = CUM.RefName2;
                                GetUpdateRecords.RefEmail2 = CUM.RefEmail2;
                                GetUpdateRecords.RefPhoneNumber2 = CUM.RefPhoneNumber2;
                                //GetUpdateRecords.ProfileId = getProfille[0];
                                //GetUpdateRecords.ProfileName = getProfille[1];
                                GetUpdateRecords.MappedUsers = Mappeduserlist;
                                GetUpdateRecords.BranchID = CUM.BranchID;
                                GetUpdateRecords.CompanyID = CompanyID;
                                GetUpdateRecords.Modifiedby = Convert.ToInt32(Session["UID"]);
                                GetUpdateRecords.ModifiedOn = Constant.GetBharatTime();
                                //db.SaveChanges();

                                var SalaryDetailUpdated = db.crm_salarydetail.Where(em => em.UserID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                if (SalaryDetailUpdated != null)
                                {
                                    SalaryDetailUpdated.UserID = id;
                                    SalaryDetailUpdated.BankName = CUM.BankName;
                                    SalaryDetailUpdated.BranchName = CUM.BranchName;
                                    SalaryDetailUpdated.AccountNumber = CUM.AccountNumber;
                                    SalaryDetailUpdated.MonthlySalary = CUM.MonthlySalary;
                                    SalaryDetailUpdated.AnnualSalary = CUM.AnnualSalary;
                                    SalaryDetailUpdated.BasicSalary = CUM.BasicSalary;
                                    SalaryDetailUpdated.HRA = CUM.HRA;
                                    SalaryDetailUpdated.TravellingAllowance = CUM.TravellingAllowance;
                                    SalaryDetailUpdated.MedicalAllowance = CUM.MedicalAllowance;
                                    SalaryDetailUpdated.PerformanceIncentive = CUM.PerformanceIncentive;
                                    SalaryDetailUpdated.OtherBenefits = CUM.OtherBenefits;
                                    SalaryDetailUpdated.PFEmployeeShare = CUM.PFEmployeeShare;
                                    SalaryDetailUpdated.PFEmployerShare = CUM.PFEmployerShare;
                                    SalaryDetailUpdated.ESICEmployerEmployee = CUM.ESICEmployerEmployee;
                                    SalaryDetailUpdated.TDS = CUM.TDS;
                                    SalaryDetailUpdated.OtherDeduction = CUM.OtherDeduction;
                                    SalaryDetailUpdated.BranchID = CUM.BranchID;
                                    SalaryDetailUpdated.CompanyID = CompanyID;
                                    SalaryDetailUpdated.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    SalaryDetailUpdated.ModifiedOn = Constant.GetBharatTime();
                                    SalaryDetailUpdated.IFCSCode = CUM.IFSCCode;
                                    SalaryDetailUpdated.LWF = CUM.LWF;
                                    SalaryDetailUpdated.Security = CUM.Security;
                                    SalaryDetailUpdated.Advance = CUM.Advance;
                                    SalaryDetailUpdated.LWP = CUM.LWP;
                                }
                                db.SaveChanges();
                                if (CUM.Documents != null && CUM.Documents.Count() > 0)
                                {
                                    foreach (var file in CUM.Documents)
                                    {

                                        var postedFile = file;
                                        if (postedFile != null && postedFile.ContentLength > 0)
                                        {
                                            int MaxContentLength = 1024 * 1024 * 5; //Size = 1 MB  

                                            IList<string> AllowedFileExtensions = new List<string> { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                                            var ext = Path.GetExtension(postedFile.FileName);
                                            var extension = ext.ToLower();
                                            if (!AllowedFileExtensions.Contains(extension))
                                            {
                                                var message = string.Format("** Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
                                                TempData["alert"] = message;
                                                return View(CUM);
                                            }
                                            else if (postedFile.ContentLength > MaxContentLength)
                                            {
                                                var message = string.Format("Please Upload a file upto 5 mb.");
                                                TempData["alert"] = message;
                                                return View(CUM);
                                            }
                                            else
                                            {
                                                var FileName = CUM.FirstName + "" + CUM.LastName + "-" + postedFile.FileName + "-" + dd.ToString("ddMMyyyyhhmmss") + "";
                                                var FileFullName = FileName + extension;
                                                var filePath = Server.MapPath("~/EmployeeDocuments/" + FileName + extension);
                                                postedFile.SaveAs(filePath);

                                                var docs = new crm_userdocuments
                                                {
                                                    UserId = CUM.UserID,
                                                    FilePath = FileFullName,
                                                    CompanyId = CompanyID,
                                                    BranchId = BranchID,
                                                    CreatedBy = Convert.ToInt32(Session["UID"]),
                                                    CreatedDate = dd
                                                };
                                                db.crm_userdocuments.Add(docs);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }

                                trans.Commit();
                                TempData["success"] = "Employee updated successfully";
                                return Redirect("/HR/createusers/" + id);
                                //return View(CUM);
                            }
                            else
                            {
                                TempData["alert"] = "There is some problem";
                                //return Redirect("/HR/createusers/" + id);
                                return View(CUM);
                            }
                        }
                        else
                        {
                            //if (CurentCountNoOfUsers <= CountNoOfUsers)
                            //{
                            #region Set Expire Date Default SuperAdmin
                            var PresentDate = DateTime.Today.ToString("yyyy-MM-dd");
                            var PresentDatePlusOneYear = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");//PresentDate.AddYears(1).ToString("yyyy-MM-dd");
                            string Adminbase64StartDate = EncodeDecodeForBase64.EncodeBase64(PresentDate);
                            string Adminbase64EndDate = EncodeDecodeForBase64.EncodeBase64(PresentDatePlusOneYear);
                            #endregion

                            #region Password encryption
                            string VersionKey = "";

                            VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                            CUM.UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                            byte[] iv1;
                            byte[] key = EncriptAES.getdcriptkey(out iv1);
                            string ecncryptPwd = EncriptAES.EncryptString(CUM.UserPassword, key, iv1);
                            #endregion
                            crm_usertbl utbl = new crm_usertbl();
                            utbl.ByUID = Convert.ToInt32(Session["UID"]);
                            utbl.Fname = CUM.FirstName;
                            utbl.Lname = CUM.LastName;
                            utbl.FatherName = CUM.FatherName;
                            if (!string.IsNullOrWhiteSpace(CUM.DateofBirth))
                            {
                                utbl.DateofBirth = Convert.ToDateTime(CUM.DateofBirth); //DateTime.ParseExact(CUM.DateofBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                utbl.DateofBirth = null;
                            }
                            utbl.ContactNumber = CUM.ContactNumber;
                            utbl.AlternateNumber = CUM.AlternateNumber;
                            utbl.Email = CUM.Email;
                            utbl.Gender = CUM.Gender;
                            utbl.EmployeeCode = CUM.EmployeeCode;
                            utbl.UserName = CUM.FirstName + "" + CUM.LastName;
                            utbl.Designation = CUM.Designation;
                            utbl.Password = ecncryptPwd;//save ecncrypted password 
                            utbl.KeyVersion = VersionKey;//save latest key version
                                                         //utbl.Password = CUM.UserPassword;
                            utbl.TimeZone = CUM.TimeZoneName;
                            utbl.CurrentAddress = CUM.CurrentAddress;
                            utbl.PermanentAddress = CUM.PermanentAddress;
                            utbl.RefName1 = CUM.RefName1;
                            utbl.RefEmail1 = CUM.RefEmail1;
                            utbl.RefPhoneNumber1 = CUM.RefPhoneNumber1;
                            utbl.RefName2 = CUM.RefName2;
                            utbl.RefEmail2 = CUM.RefEmail2;
                            utbl.RefPhoneNumber2 = CUM.RefPhoneNumber2;
                            //utbl.ProfileId = getProfille[0];
                            //utbl.ProfileName = getProfille[1];
                            utbl.Status = true;
                            utbl.MappedUsers = Mappeduserlist;
                            utbl.Created_at = Constant.GetBharatTime();
                            utbl.BranchID = CUM.BranchID;
                            utbl.CompanyID = CompanyID;
                            utbl.CreatedBy = Convert.ToInt32(Session["UID"]);
                            utbl.IsActive = true;
                            utbl.IsExpired = false;
                            utbl.StartDate = Adminbase64StartDate;
                            utbl.EndDate = Adminbase64EndDate;
                            db.crm_usertbl.Add(utbl);
                            db.SaveChanges();
                            int returnValue = utbl.Id;
                            if (returnValue > 0)
                            {
                                crm_salarydetail csd = new crm_salarydetail();
                                csd.UserID = returnValue;
                                csd.BankName = CUM.BankName;
                                csd.BranchName = CUM.BranchName;
                                csd.AccountNumber = CUM.AccountNumber;
                                csd.MonthlySalary = CUM.MonthlySalary;
                                csd.AnnualSalary = CUM.AnnualSalary;
                                csd.BasicSalary = CUM.BasicSalary;
                                csd.HRA = CUM.HRA;
                                csd.TravellingAllowance = CUM.TravellingAllowance;
                                csd.MedicalAllowance = CUM.MedicalAllowance;
                                csd.PerformanceIncentive = CUM.PerformanceIncentive;
                                csd.OtherBenefits = CUM.OtherBenefits;
                                csd.PFEmployeeShare = CUM.PFEmployeeShare;
                                csd.PFEmployerShare = CUM.PFEmployerShare;
                                csd.ESICEmployerEmployee = CUM.ESICEmployerEmployee;
                                csd.TDS = CUM.TDS;
                                csd.OtherDeduction = CUM.OtherDeduction;
                                csd.CreatedBy = Convert.ToInt32(Session["UID"]);
                                csd.CreatedOn = Constant.GetBharatTime();
                                csd.BranchID = CUM.BranchID;
                                csd.CompanyID = CompanyID;
                                csd.IFCSCode = CUM.IFSCCode;
                                csd.LWF = CUM.LWF;
                                csd.Security = CUM.Security;
                                csd.Advance = CUM.Advance;
                                csd.LWP = CUM.LWP;
                                db.crm_salarydetail.Add(csd);
                                db.SaveChanges();

                                if (CUM.Documents != null && CUM.Documents.Count() > 0)
                                {
                                    foreach (var file in CUM.Documents)
                                    {

                                        var postedFile = file;
                                        if (postedFile != null && postedFile.ContentLength > 0)
                                        {
                                            int MaxContentLength = 1024 * 1024 * 5; //Size = 1 MB  

                                            IList<string> AllowedFileExtensions = new List<string> { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

                                            var ext = Path.GetExtension(postedFile.FileName);
                                            var extension = ext.ToLower();
                                            if (!AllowedFileExtensions.Contains(extension))
                                            {
                                                var message = string.Format("** Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
                                                TempData["alert"] = message;
                                                return View(CUM);
                                            }
                                            else if (postedFile.ContentLength > MaxContentLength)
                                            {
                                                var message = string.Format("Please Upload a file upto 5 mb.");
                                                TempData["alert"] = message;
                                                return View(CUM);
                                            }
                                            else
                                            {
                                                var FileName = CUM.FirstName + "" + CUM.LastName + "-" + postedFile.FileName + "-" + dd.ToString("ddMMyyyyhhmmss") + "";
                                                var FileFullName = FileName + extension;
                                                var filePath = Server.MapPath("~/EmployeeDocuments/" + FileName + extension);
                                                postedFile.SaveAs(filePath);
                                                var docs = new crm_userdocuments
                                                {
                                                    UserId = returnValue,
                                                    FilePath = FileFullName,
                                                    CompanyId = CompanyID,
                                                    BranchId = BranchID,
                                                    CreatedBy = Convert.ToInt32(Session["UID"]),
                                                    CreatedDate = dd
                                                };
                                                db.crm_userdocuments.Add(docs);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }

                                trans.Commit();
                                TempData["success"] = "Employee added successfully";
                            }
                            else
                            {
                                TempData["alert"] = "There is some problem";
                            }
                            //}
                            //else
                            //{
                            //    TempData["alert"] = "** You can create only " + CountNoOfUsers + " users, For more users contact to administrator.";
                            //}
                        }
                        //}
                        //else
                        //{
                        //    TempData["alert"] = "Please select atleast one user profile";
                        //}
                    }
                    else
                    {
                        return Redirect("/home/login");
                    }
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    TempData["alert"] = "There is some problem";
                    return View(CUM);
                }
            }

            return Redirect("/HR/createusers");
        }

        public ActionResult changeuserstatus(int Id)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var GetUserDetails = db.crm_usertbl.Where(em => em.Id == Id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetUserDetails != null)
                {
                    if (GetUserDetails.Status == true)
                    {
                        GetUserDetails.Status = false;
                    }
                    else if (GetUserDetails.Status == false)
                    {
                        GetUserDetails.Status = true;
                    }
                    else
                    {
                        GetUserDetails.Status = true;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return Redirect("/HR/users");
        }

        public ActionResult UserDocuments(int id)
        {
            var data = new List<crm_userdocuments>();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                if (Session["UserName"] != null)
                {
                    data = db.crm_userdocuments.Where(em => em.UserId == id && em.BranchId == BranchID && em.CompanyId == CompanyID).ToList();
                }
                else
                {
                    Session["ReturnUrl"] = "/HR/users";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/home/login");
            }
            return View(data);
        }

        public JsonResult DeleteUserDocument(int id)
        {
            var msg = "";
            try
            {
                var data = db.crm_userdocuments.Where(a => a.Id == id).FirstOrDefault();
                if (data != null)
                {
                    Utility.DeleteFile("EmployeeDocuments", data.FilePath);
                    db.crm_userdocuments.Remove(data);
                    db.SaveChanges();
                    msg = "ok";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "error";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
