using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Svam.EF;
using Traders.Models;
using System.Globalization;
using Svam.Models;
using System.Web;
using Svam._Class;
using Traders.Mailer;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using Excel;
using System.Data.Entity;
using Svam.UtilityManager;
using AutoMapper;
using Svam.Repository;
using Svam.Models.ViewModel;
using Svam.Models.DTO;
using System.Text;
using System.Dynamic;

namespace Traders.Controllers
{
    [NoCache]
    public class homeController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        CommonRepository cr = new CommonRepository();

        #region DashBoard
        public ActionResult Index()
        {

            DashBoardLeadsModel DBLM = new DashBoardLeadsModel();
            try
            {
                if (Session["UserName"] != null)
                {
                    //Testing Purpose
                    //var executelead = GetIndiaMartLeadAsync();  

                    var chk = Convert.ToString(Session["ProfilePageUrl"]);
                    if (Convert.ToString(Session["ProfilePageUrl"]) == "/")
                    {
                        #region Cal- DateTime.......
                        var CurrentDate = Constant.GetBharatTime();//get bharat time
                        DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                        DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                        var TodayDate = CurrentDate.ToString("dd/MM/yyyy");
                        var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                        var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

                        #endregion
                        Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                        Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                        var UID = Convert.ToInt32(Session["UID"]);
                        if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                        {
                            #region Today Payment
                            CRMPaymentModel CPM = new CRMPaymentModel();
                            DataTable GetpaymentList = DataAccessLayer.GetDataTable("call CRM_CustomerPayment(" + 0 + "," + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "')");
                            if (GetpaymentList.Rows.Count > 0)
                            {
                                List<CRMPaymentModel> CPMList = new List<CRMPaymentModel>();
                                for (int i = 0; i < GetpaymentList.Rows.Count; i++)
                                {
                                    CRMPaymentModel cPM = new CRMPaymentModel();
                                    cPM.BillNumber = Convert.ToString(GetpaymentList.Rows[i]["BillNumber"]);
                                    cPM.CustomerName = Convert.ToString(GetpaymentList.Rows[i]["CustomerName"]);
                                    cPM.BillDate = Convert.ToDateTime(GetpaymentList.Rows[i]["BillDate"]);
                                    cPM.DueDate = Convert.ToDateTime(GetpaymentList.Rows[i]["DueDate"]);
                                    cPM.BalanceAmount = Convert.ToDecimal(GetpaymentList.Rows[i]["BalanceAmt"]);
                                    CPMList.Add(cPM);
                                }
                                CPM.CRMPaymentModelList = CPMList;
                                DBLM.dashboardTodayPaymentList = CPM.CRMPaymentModelList;
                            }
                            #endregion

                            #region DOB and MarriageAnniversary
                            DataTable GetTodayDOBMA = DataAccessLayer.GetDataTable("call CRM_TodayDOBMarriageAnniversary(" + BranchID + "," + CompanyID + "," + 0 + ")");
                            if (GetTodayDOBMA.Rows.Count > 0)
                            {
                                DBLM.dashboardTodayDOBMAList = (from dr in GetTodayDOBMA.AsEnumerable()
                                                                select new DashBoardLeadsModel()
                                                                {
                                                                    Id = Convert.ToInt32(dr["Id"]),
                                                                    LeadName = Convert.ToString(dr["Customer"]),
                                                                    Phone = Convert.ToString(dr["MobileNo"]),
                                                                    Email = Convert.ToString(dr["EmailId"]),
                                                                    Country = Convert.ToString(dr["CountryName"]),
                                                                    CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                    FollowUpDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                                    LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                                    AssignedBy = Convert.ToString(dr["AssinedBy"])
                                                                }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Delivered").ToList();
                            }
                            #endregion

                            #region Today-Leads
                            DataTable GetTodayLeads = DataAccessLayer.GetDataTable("call CRM_TodayLeads(" + BranchID + "," + CompanyID + "," + 0 + ")");
                            if (GetTodayLeads.Rows.Count > 0)
                            {
                                DBLM.dashboardTodayLeadsList = (from dr in GetTodayLeads.AsEnumerable()
                                                                select new DashBoardLeadsModel()
                                                                {
                                                                    Id = Convert.ToInt32(dr["Id"]),
                                                                    LeadName = Convert.ToString(dr["Customer"]),
                                                                    Phone = Convert.ToString(dr["MobileNo"]),
                                                                    Email = Convert.ToString(dr["EmailId"]),
                                                                    Country = Convert.ToString(dr["CountryName"]),
                                                                    CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                    FollowUpDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                                    LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                                    AssignedBy = Convert.ToString(dr["AssinedBy"])
                                                                }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Delivered").ToList();
                            }
                            #endregion

                            #region Get-Today-FollowUp-Leads
                            DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable(" call CRM_TodayFollowupLeads(" + BranchID + "," + CompanyID + "," + 0 + ")");
                            if (TodayFollowupLeads.Rows.Count > 0)
                            {
                                DBLM.dashboardTodayFollowUpLeadsList = (from dr in TodayFollowupLeads.AsEnumerable()
                                                                        select new DashBoardLeadsModel()
                                                                        {
                                                                            Id = Convert.ToInt32(dr["Id"]),
                                                                            LeadName = Convert.ToString(dr["Customer"]),
                                                                            Phone = Convert.ToString(dr["MobileNo"]),
                                                                            Email = Convert.ToString(dr["EmailId"]),
                                                                            Country = Convert.ToString(dr["CountryName"]),
                                                                            CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                            FollowUpDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                                            LeadStatus = Convert.ToString(dr["LeadStatus"]),

                                                                        }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();
                            }
                            #endregion

                            #region Bind-LeftPanel-Data
                            //var Curent_1 = db.P_GetCurrentMoth_1Leads(0).ToArray();
                            //TempData["CurrentMonth-1"] = Curent_1[0];
                            //var Curent_2 = db.P_GetCurrentMoth_2Leads(0).ToArray();
                            //TempData["CurrentMonth-2"] = Curent_2[0];
                            //var Curent_3 = db.P_GetCurrentMoth_3Leads(0).ToList();
                            //TempData["CurrentMonth-3"] = Curent_3[0];
                            //var Curent_4 = db.P_GetCurrentMoth_4Leads(0).ToList();
                            //TempData["CurrentMonth-4"] = Curent_4[0];

                            #endregion

                            #region Call && Free Trial
                            //var GetAllLeads = db.P_GetCurrentMonthLeadsByUser(0).ToList();

                            ////Start FollowUp.............
                            //var todayFollowUp = db.P_getTodayFollowUp(0, TodayDate).ToArray();
                            //TempData["TodayFollowCount"] = todayFollowUp[0];
                            //var GetMonthFollowup = db.P_getMonthFollowUp(0, MStartDate, MEndDate).ToArray();
                            //TempData["MonthFollowUpCount"] = GetMonthFollowup[0];

                            ////Calls Start Here..........
                            //var todayCalls = db.P_TodayCallsByUserId(0).ToArray();
                            //TempData["TodayCalls"] = todayCalls[0];
                            //var GetMonthCalls = db.P_MonthCallsByUserId(0).ToArray();
                            //TempData["MonthCalls"] = GetMonthCalls[0];

                            ////Free Trail Start here.............
                            //var TodayFreeTrail = db.P_getTodayFreeTrialsByUser(0).ToArray();
                            //TempData["TodayFreeTrail"] = TodayFreeTrail[0];
                            //var MonthFreeTrail = db.P_getMonthFreeTrialsByUser(0, MStartDate, MEndDate).ToArray();
                            //TempData["MonthFreeTrail"] = MonthFreeTrail[0];

                            ////Sales Start here.............
                            //var TodaySales = db.P_TodaySalesByUserId(0).ToArray();
                            //TempData["TodaySales"] = TodaySales[0];
                            //var MonthSales = db.P_MonthSalesByUserId(0).ToArray();
                            //TempData["MonthSales"] = MonthSales[0];
                            #endregion
                        }
                        else
                        {
                            #region Today Payment
                            CRMPaymentModel CPM = new CRMPaymentModel();
                            DataTable GetpaymentList = DataAccessLayer.GetDataTable("call CRM_CustomerPayment(" + Convert.ToInt32(Session["UID"]) + "," + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "')");
                            if (GetpaymentList.Rows.Count > 0)
                            {
                                List<CRMPaymentModel> CPMList = new List<CRMPaymentModel>();
                                for (int i = 0; i < GetpaymentList.Rows.Count; i++)
                                {
                                    CRMPaymentModel cPM = new CRMPaymentModel();
                                    cPM.BillNumber = Convert.ToString(GetpaymentList.Rows[i]["BillNumber"]);
                                    cPM.CustomerName = Convert.ToString(GetpaymentList.Rows[i]["CustomerName"]);
                                    cPM.BillDate = Convert.ToDateTime(GetpaymentList.Rows[i]["BillDate"]);
                                    cPM.DueDate = Convert.ToDateTime(GetpaymentList.Rows[i]["DueDate"]);
                                    cPM.BalanceAmount = Convert.ToDecimal(GetpaymentList.Rows[i]["BalanceAmt"]);
                                    CPMList.Add(cPM);
                                }
                                CPM.CRMPaymentModelList = CPMList;
                                DBLM.dashboardTodayPaymentList = CPM.CRMPaymentModelList;
                            }
                            #endregion

                            #region DOB and MarriageAnniversary
                            DataTable GetTodayDOBMA = DataAccessLayer.GetDataTable("call CRM_TodayDOBMarriageAnniversary(" + BranchID + "," + CompanyID + "," + UID + ")");
                            if (GetTodayDOBMA.Rows.Count > 0)
                            {
                                DBLM.dashboardTodayDOBMAList = (from dr in GetTodayDOBMA.AsEnumerable()
                                                                select new DashBoardLeadsModel()
                                                                {
                                                                    Id = Convert.ToInt32(dr["Id"]),
                                                                    LeadName = Convert.ToString(dr["Customer"]),
                                                                    Phone = Convert.ToString(dr["MobileNo"]),
                                                                    Email = Convert.ToString(dr["EmailId"]),
                                                                    Country = Convert.ToString(dr["CountryName"]),
                                                                    CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                    FollowUpDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                                    LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                                    AssignedBy = Convert.ToString(dr["AssinedBy"])
                                                                }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Delivered").ToList();
                            }
                            #endregion

                            #region Today-Leads
                            DataTable GetTodayLeads = DataAccessLayer.GetDataTable(" call CRM_TodayLeads(" + BranchID + "," + CompanyID + "," + UID + ")");
                            if (GetTodayLeads.Rows.Count > 0)
                            {
                                DBLM.dashboardTodayLeadsList = (from dr in GetTodayLeads.AsEnumerable()
                                                                select new DashBoardLeadsModel()
                                                                {
                                                                    Id = Convert.ToInt32(dr["Id"]),
                                                                    LeadName = Convert.ToString(dr["Customer"]),
                                                                    Phone = Convert.ToString(dr["MobileNo"]),
                                                                    Email = Convert.ToString(dr["EmailId"]),
                                                                    Country = Convert.ToString(dr["CountryName"]),
                                                                    CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                    FollowUpDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                                    LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                                    LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
                                                                    AssignTo = Convert.ToInt32((Convert.ToString(dr["AssignTo"]) == null || Convert.ToString(dr["AssignTo"]) == "") ? 0 : dr["AssignTo"]),
                                                                    AssignedBy = Convert.ToString(dr["AssinedBy"])
                                                                }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();

                                List<DashBoardLeadsModel> assignList = DBLM.dashboardTodayLeadsList.Where(em => em.AssignTo == UID).ToList();
                                DBLM.dashboardTodayLeadsList = DBLM.dashboardTodayLeadsList.Where(em => em.LeadOwner == UID).ToList();
                                if (assignList.Count > 0)
                                {
                                    DBLM.dashboardTodayLeadsList.AddRange(assignList);
                                }
                            }
                            #endregion

                            #region Get-Today-FollowUp-Leads
                            DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable(" call CRM_TodayFollowupLeads(" + BranchID + "," + CompanyID + "," + UID + ")");
                            if (TodayFollowupLeads.Rows.Count > 0)
                            {
                                DBLM.dashboardTodayFollowUpLeadsList = (from dr in TodayFollowupLeads.AsEnumerable()
                                                                        select new DashBoardLeadsModel()
                                                                        {
                                                                            Id = Convert.ToInt32(dr["Id"]),
                                                                            LeadName = Convert.ToString(dr["Customer"]),
                                                                            Phone = Convert.ToString(dr["MobileNo"]),
                                                                            Email = Convert.ToString(dr["EmailId"]),
                                                                            Country = Convert.ToString(dr["CountryName"]),
                                                                            CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                            AssignedBy = Convert.ToString(dr["AssignedBy"]),
                                                                            FollowUpDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                                            LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                                            LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
                                                                            AssignTo = Convert.ToInt32((Convert.ToString(dr["AssignTo"]) == null || Convert.ToString(dr["AssignTo"]) == "") ? 0 : dr["AssignTo"])
                                                                        }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();

                                List<DashBoardLeadsModel> assignList = DBLM.dashboardTodayFollowUpLeadsList.Where(em => em.AssignTo == UID).ToList();
                                DBLM.dashboardTodayFollowUpLeadsList = DBLM.dashboardTodayFollowUpLeadsList.Where(em => em.LeadOwner == UID).ToList();
                                if (assignList.Count > 0)
                                {
                                    DBLM.dashboardTodayFollowUpLeadsList.AddRange(assignList);
                                }
                            }
                            #endregion

                            #region Bind-LeftPanel-Data
                            //var Curent_1 = db.P_GetCurrentMoth_1Leads(UID).ToArray();
                            //TempData["CurrentMonth-1"] = Curent_1[0];
                            //var Curent_2 = db.P_GetCurrentMoth_2Leads(UID).ToArray();
                            //TempData["CurrentMonth-2"] = Curent_2[0];
                            //var Curent_3 = db.P_GetCurrentMoth_3Leads(UID).ToList();
                            //TempData["CurrentMonth-3"] = Curent_3[0];
                            //var Curent_4 = db.P_GetCurrentMoth_4Leads(UID).ToList();
                            //TempData["CurrentMonth-4"] = Curent_4[0];

                            #endregion

                            #region Call && Free Trial
                            //var GetAllLeads = db.P_GetCurrentMonthLeadsByUser(0).ToList();

                            ////Start FollowUp.............
                            //var todayFollowUp = db.P_getTodayFollowUp(UID, TodayDate).ToArray();
                            //TempData["TodayFollowCount"] = todayFollowUp[0];
                            //var GetMonthFollowup = db.P_getMonthFollowUp(UID, MStartDate, MEndDate).ToArray();
                            //TempData["MonthFollowUpCount"] = GetMonthFollowup[0];
                            ////Calls Start here................

                            ////Free Trail Start here...................
                            //var TodayFreeTrail = db.P_getTodayFreeTrialsByUser(UID).ToArray();
                            //TempData["TodayFreeTrail"] = TodayFreeTrail[0];
                            //var MonthFreeTrail = db.P_getMonthFreeTrialsByUser(UID, MStartDate, MEndDate).ToArray();
                            //TempData["MonthFreeTrail"] = MonthFreeTrail[0];

                            ////Sales Start here.............
                            //var TodaySales = db.P_TodaySalesByUserId(0).ToArray();
                            //TempData["TodaySales"] = TodaySales[0];
                            //var MonthSales = db.P_MonthSalesByUserId(0).ToArray();
                            //TempData["MonthSales"] = MonthSales[0];
                            #endregion
                        }
                    }
                    else
                    {
                        return Redirect(Convert.ToString(Session["ProfilePageUrl"]));
                    }
                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(DBLM);
        }
        #endregion

        #region decreypt password
        public ActionResult dpwd(string text)
        {
            string pwd = string.Empty;
            #region password decryption
            byte[] iv1;
            byte[] key = EncriptAES.getdcriptkey(out iv1);
            string decryptPwd = EncriptAES.DecryptString(text, key, iv1);
            #endregion
            pwd = decryptPwd;//show decrypted password
            return Content(pwd);
        }
        #endregion

        #region login || Log-out

        public ActionResult login()
        {

            if (Session["UserName"] != null)
            {
                if (Session["ReturnUrl"] != null)
                {
                    return Redirect(Convert.ToString(Session["ReturnUrl"]));
                }
                else
                {
                    return Redirect("/");
                }
            }
            else
            {

                return View();
            }
        }

        // New Login Page Change on 29th May 2019
        [HttpPost]
        public ActionResult login(LoginModel LM)
        {
            UserLogInfoModel ULIM = new UserLogInfoModel();
            try
            {
                #region password decryption
                string password = "";
                //var userData = new List<UserCredential>();
                var userData = (from u in db.crm_usertbl
                                where (u.UserName.ToLower() == LM.UserNameOrEmail.ToLower() ||
                                u.Email.ToLower() == LM.UserNameOrEmail.ToLower()) && u.Status == true
                                select new UserCredential
                                {
                                    UserName = u.UserName,
                                    Email = u.Email,
                                    Password = u.Password,
                                    KeyVersion = u.KeyVersion
                                }).ToList();
                //string query = @"select UserName,Email,Password,KeyVersion from crm_usertbl where (username='" + LM.UserNameOrEmail + "'or Email='" + LM.UserNameOrEmail + "') and Status=1";
                //var userData = db.Database.SqlQuery<UserCredential>(query).ToList();

                if (userData != null && userData.Count > 0)
                {
                    for (int i = 0; i < userData.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(userData[i].KeyVersion))
                        {
                            byte[] iv1;
                            byte[] key = EncriptAES.getdcriptkey(out iv1);
                            string encPwd = EncriptAES.EncryptString(LM.Password, key, iv1);


                            if ((userData[i].UserName.ToLower() == LM.UserNameOrEmail.ToLower() || userData[i].Email.ToLower() == LM.UserNameOrEmail.ToLower()) && userData[i].Password == encPwd)
                            {
                                password = encPwd;
                                break;
                            }
                        }
                        else
                        {
                            if ((userData[i].UserName.ToLower() == LM.UserNameOrEmail.ToLower() || userData[i].Email.ToLower() == LM.UserNameOrEmail.ToLower()) && userData[i].Password.ToLower() == LM.Password.ToLower())
                            {
                                password = LM.Password;
                                break;
                            }

                        }
                    }
                }


                #endregion
                //DataTable GetUserdata = DataAccessLayer.GetDataTable("call CRM_logIn('" + LM.UserNameOrEmail + "','" + LM.Password + "')");
                DataTable GetUserdata = DataAccessLayer.GetDataTable("call CRM_logIn('" + LM.UserNameOrEmail + "','" + password + "')");
                if (GetUserdata.Rows.Count > 0)
                {
                    UserLogInfoModel uLM = new UserLogInfoModel();
                    uLM.Id = Convert.ToInt32(GetUserdata.Rows[0]["Id"]);
                    uLM.Fname = Convert.ToString(GetUserdata.Rows[0]["Fname"]);
                    uLM.Lname = Convert.ToString(GetUserdata.Rows[0]["Lname"]);
                    uLM.Status = Convert.ToBoolean(GetUserdata.Rows[0]["Status"]);
                    uLM.Email = Convert.ToString(GetUserdata.Rows[0]["Email"]);
                    //uLM.ByUID= Convert.ToInt32(GetUserdata.Rows[0]["ByUID"]);
                    //uLM.TimeZone =GetUserdata.Rows[0]["TimeZone"] != DBNull.Value ? Convert.ToString(GetUserdata.Rows[0]["TimeZone"]):"IST";
                    uLM.TimeZone = Convert.ToString(GetUserdata.Rows[0]["TimeZone"]);
                    uLM.ProfileName = Convert.ToString(GetUserdata.Rows[0]["ProfileName"]);
                    uLM.MappedUsers = Convert.ToString(GetUserdata.Rows[0]["MappedUsers"]);
                    uLM.ProfileId = Convert.ToString(GetUserdata.Rows[0]["ProfileId"]);
                    if (GetUserdata.Rows[0]["BranchID"] != DBNull.Value)
                    {
                        uLM.BranchID = Convert.ToInt32(GetUserdata.Rows[0]["BranchID"]);
                    }
                    if (GetUserdata.Rows[0]["CompanyID"] != DBNull.Value)
                    {
                        uLM.CompanyID = Convert.ToInt32(GetUserdata.Rows[0]["CompanyID"]);
                    }
                    uLM.StartDate = Convert.ToString(GetUserdata.Rows[0]["StartDate"]);
                    uLM.EndDate = Convert.ToString(GetUserdata.Rows[0]["EndDate"]);
                    if (Convert.ToString(GetUserdata.Rows[0]["IsExpired"]) != "")
                    {
                        uLM.IsExpired = Convert.ToBoolean(GetUserdata.Rows[0]["IsExpired"]);
                    }
                    else
                    {
                        uLM.IsExpired = false;
                    }
                    if (Convert.ToString(GetUserdata.Rows[0]["IsActive"]) != "")
                    {
                        uLM.IsActive = Convert.ToBoolean(GetUserdata.Rows[0]["IsActive"]);
                    }
                    ULIM = uLM;

                    #region Default Master Entry
                    DefaultMasterDataDump(uLM.CompanyID, uLM.BranchID);//add by default lead status master some status names
                    #endregion

                    #region Contact Us Enquiry Insert and Update
                    DefaultWebsiteEquiry(uLM.CompanyID, uLM.Id);
                    #endregion


                    if (!string.IsNullOrWhiteSpace(uLM.StartDate) && !string.IsNullOrWhiteSpace(uLM.EndDate))
                    {
                        #region GET Expire start and End Date format in 'yyyy-MM-dd'
                        string base64StartDate = EncodeDecodeForBase64.DecodeBase64(uLM.StartDate);
                        string base64EndDate = EncodeDecodeForBase64.DecodeBase64(uLM.EndDate);
                        DateTime ExpireStarDate = DateTime.ParseExact(base64StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime ExpireEndDate = DateTime.ParseExact(base64EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        #endregion

                        #region Set Expire Date Default SuperAdmin
                        //var PresentDate = Convert.ToDateTime(base64StartDate).Date.ToString("yyyy-MM-dd");
                        //var PresentDatePlusOneYear = Convert.ToDateTime(base64StartDate).Date.AddYears(3).ToString("yyyy-MM-dd");
                        //string Adminbase64StartDate = EncodeDecodeForBase64.EncodeBase64(PresentDate);
                        //string Adminbase64EndDate = EncodeDecodeForBase64.EncodeBase64(PresentDatePlusOneYear);
                        #endregion

                        #region Set Expire Date user need to extend for year
                        // PresentDate = ExpireStarDate.ToString("yyyy-MM-dd");
                        // PresentDatePlusOneYear = ExpireStarDate.AddYears(1).ToString("yyyy-MM-dd");
                        // Adminbase64StartDate = EncodeDecodeForBase64.EncodeBase64(PresentDate);
                        // Adminbase64EndDate = EncodeDecodeForBase64.EncodeBase64(PresentDatePlusOneYear);
                        #endregion                  


                        #region Get-MAC-Address and IP Address
                        var mac = GetMACAddress();
                        if (mac != string.Empty)
                        {
                            mac = Regex.Replace(mac, ".{2}", "$0-").TrimEnd('-');
                        }
                        string userIpAddress = this.Request.UserHostAddress;
                        #endregion

                        Session["UserName"] = Convert.ToString(uLM.Fname + " " + uLM.Lname);
                        Session["UID"] = Convert.ToInt32(uLM.Id);
                        //Session["ByUID"] = Convert.ToInt32(uLM.ByUID);
                        Session["UserEmail"] = uLM.Email;
                        Session["UserType"] = uLM.ProfileName;
                        Session["IsMapped"] = uLM.MappedUsers;
                        Session["TimeZone"] = uLM.TimeZone;
                        Session["CompanyID"] = uLM.CompanyID;
                        Session["BranchID"] = uLM.BranchID;
                        Session["UserProfileId"] = uLM.ProfileId == "" ? "0" : uLM.ProfileId;

                        int BranchID = Convert.ToInt32(GetUserdata.Rows[0]["BranchID"]);
                        int CompanyID = Convert.ToInt32(GetUserdata.Rows[0]["CompanyID"]);

                        if (uLM.Status == true)
                        {
                            #region get companyLogo and favicon
                            if (CompanyID != 296)
                            {
                                var companyProfile = db.company_profile.Where(a => a.ID == CompanyID).FirstOrDefault();
                                if (companyProfile != null)
                                {
                                    if (!string.IsNullOrEmpty(companyProfile.ecom_logo))
                                    {
                                        Session["CompanyLogo"] = string.Format("https://www.smartcapita.com/{0}", companyProfile.ecom_logo);
                                    }
                                    if (!string.IsNullOrEmpty(companyProfile.fab_icon))
                                    {
                                        Session["CompanyFavIcon"] = string.Format("https://www.smartcapita.com/{0}", companyProfile.fab_icon);
                                    }
                                }
                            }
                            #endregion

                            #region Company SuperAdmin Login Exclude User
                            if (uLM.ProfileName == "SuperAdmin" && uLM.IsActive == true && uLM.IsExpired == false && ExpireStarDate.Date <= DateTime.Now.Date && ExpireEndDate.Date >= DateTime.Now.Date)
                            {
                                //Session["RightsCreateLeads"] = true;
                                //Session["RightsViewLeads"] = true;
                                //Session["RightsViewSales"] = true;
                                //Session["RightsMessage"] = true;
                                //Session["RightsRoleManagement"] = true;
                                //Session["RightsNotification"] = true;
                                //Session["RightsDeveloperReport"] = true;
                                //Session["RightsLeadNotify"] = true;
                                //Session["RightsProjectManagement"] = true;
                                //Session["RightsCommonActivityRemarks"] = true;
                                //Session["RightsLeadManagement"] = true;
                                //Session["RightsAssignLeadManagement"] = true;
                                //Session["RightsDailyworkSchedule"] = true;
                                //Session["RightsPayments"] = true;

                                #region Insert Login-Time
                                if (ULIM.TimeZone != "IST" && !string.IsNullOrEmpty(ULIM.TimeZone))
                                {
                                    #region Get-Time-By-TimeZone
                                    var GetData = db.crm_zonetbl.Where(em => em.ZoneName == ULIM.TimeZone).FirstOrDefault();
                                    if (GetData != null)
                                    {
                                        var IST = System.DateTime.Now;
                                        var NewtimeZoneDate = IST.AddHours(-5);
                                        NewtimeZoneDate = NewtimeZoneDate.AddMinutes(-30);
                                        if (GetData.ZoneHours > 0)
                                        {
                                            NewtimeZoneDate = NewtimeZoneDate.AddHours(GetData.ZoneHours);
                                        }
                                        else if (GetData.ZoneMin > 0)
                                        {
                                            NewtimeZoneDate = NewtimeZoneDate.AddMinutes(GetData.ZoneMin);
                                        }

                                        var NewZonetime = NewtimeZoneDate.ToString("hh:mm:ss tt");
                                        var NewZoneDate = NewtimeZoneDate.ToString("dd/MM/yyyy");
                                        var CheckExistAttand = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == ULIM.Id && em.L_In_Date == NewZoneDate && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                        if (CheckExistAttand == null)
                                        {
                                            crm_tbl_nipl_emp_attendance LogAtt = new crm_tbl_nipl_emp_attendance();
                                            LogAtt.EmpId = ULIM.Id;
                                            LogAtt.L_In_Date = NewZoneDate;
                                            LogAtt.L_In_Time = NewZonetime;
                                            LogAtt.Status = "P";
                                            LogAtt.IPAddress = userIpAddress;
                                            LogAtt.MacAddress = mac;
                                            LogAtt.LogTimeZone = ULIM.TimeZone;
                                            LogAtt.Working_Late_Night = false;
                                            LogAtt.Extra_working = false;
                                            LogAtt.CompanyID = CompanyID;
                                            LogAtt.BranchID = BranchID;
                                            db.crm_tbl_nipl_emp_attendance.Add(LogAtt);
                                            db.SaveChanges();
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    DateTime utcTime = DateTime.UtcNow;
                                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

                                    var time = localTime.ToString("hh:mm:ss tt");//System.DateTime.Now.ToString("hh:mm:ss tt");
                                    var Date = localTime.ToString("dd/MM/yyyy");//System.DateTime.Now.ToString("MM/dd/yyyy");
                                    var CheckExistAttand = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == ULIM.Id && em.L_In_Date == Date && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                    if (CheckExistAttand == null)
                                    {
                                        crm_tbl_nipl_emp_attendance LogAtt = new crm_tbl_nipl_emp_attendance();
                                        LogAtt.EmpId = ULIM.Id;
                                        LogAtt.L_In_Date = Date;
                                        LogAtt.L_In_Time = time;
                                        LogAtt.Status = "P";
                                        LogAtt.IPAddress = userIpAddress;
                                        LogAtt.MacAddress = mac;
                                        LogAtt.LogTimeZone = ULIM.TimeZone;
                                        LogAtt.Working_Late_Night = false;
                                        LogAtt.Extra_working = false;
                                        LogAtt.CompanyID = CompanyID;
                                        LogAtt.BranchID = BranchID;
                                        db.crm_tbl_nipl_emp_attendance.Add(LogAtt);
                                        db.SaveChanges();
                                    }
                                }
                                #endregion

                                #region Redirect URL
                                if (Session["ReturnUrl"] != null)
                                {
                                    return Redirect(Convert.ToString(Session["ReturnUrl"]));
                                }
                                else
                                {
                                    #region Set-ProfilePage
                                    if (ULIM.ProfileName == "Developer")
                                    {
                                        Session["ProfilePageUrl"] = "/Nis/Developer";
                                        return Redirect("/Nis/Developer");
                                    }
                                    else if (ULIM.ProfileName == "Sales" || ULIM.ProfileName == "SuperAdmin")
                                    {
                                        Session["ProfilePageUrl"] = "/";
                                        return Redirect("/");
                                    }
                                    else if (ULIM.ProfileName == "HR")
                                    {
                                        Session["ProfilePageUrl"] = "/Hr/dashboard";
                                        return Redirect("/Hr/dashboard");
                                    }
                                    else
                                    {
                                        Session["ProfilePageUrl"] = "/Nis/Remark";
                                        return Redirect("/Nis/Remark");
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else if (uLM.ProfileName == "SuperAdmin" && uLM.IsActive == false && uLM.IsExpired == true && ExpireStarDate.Date <= DateTime.Now.Date && ExpireEndDate.Date <= DateTime.Now.Date)
                            {
                                TempData["alert"] = "Subscription is expired, Please contact to administrator.!";
                            }
                            else if (uLM.ProfileName == "SuperAdmin" && uLM.IsActive == true && uLM.IsExpired == false && ExpireStarDate.Date <= DateTime.Now.Date && ExpireEndDate.Date <= DateTime.Now.Date)
                            {
                                TempData["alert"] = "Subscription is expired, Please contact to administrator.!";
                            }
                            #endregion

                            #region Company User Login Exclude SuperAdmin
                            if (uLM.ProfileName != "SuperAdmin" && uLM.IsActive == true && uLM.IsExpired == false && ExpireStarDate.Date <= DateTime.Now.Date && ExpireEndDate.Date >= DateTime.Now.Date)
                            {
                                #region Insert Login-Time
                                if (ULIM.TimeZone != "IST" && ULIM.TimeZone != null)
                                {
                                    #region Get-Time-By-TimeZone
                                    var GetData = db.crm_zonetbl.Where(em => em.ZoneName == ULIM.TimeZone).FirstOrDefault();
                                    if (GetData != null)
                                    {
                                        var IST = System.DateTime.Now;
                                        var NewtimeZoneDate = IST.AddHours(-5);
                                        NewtimeZoneDate = NewtimeZoneDate.AddMinutes(-30);
                                        if (GetData.ZoneHours > 0)
                                        {
                                            NewtimeZoneDate = NewtimeZoneDate.AddHours(GetData.ZoneHours);
                                        }
                                        else if (GetData.ZoneMin > 0)
                                        {
                                            NewtimeZoneDate = NewtimeZoneDate.AddMinutes(GetData.ZoneMin);
                                        }

                                        var NewZonetime = NewtimeZoneDate.ToString("hh:mm:ss tt");
                                        var NewZoneDate = NewtimeZoneDate.ToString("dd/MM/yyyy");
                                        var CheckExistAttand = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == ULIM.Id && em.L_In_Date == NewZoneDate && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                        if (CheckExistAttand == null)
                                        {
                                            crm_tbl_nipl_emp_attendance LogAtt = new crm_tbl_nipl_emp_attendance();
                                            LogAtt.EmpId = ULIM.Id;
                                            LogAtt.L_In_Date = NewZoneDate;
                                            LogAtt.L_In_Time = NewZonetime;
                                            LogAtt.Status = "P";
                                            LogAtt.IPAddress = userIpAddress;
                                            LogAtt.MacAddress = mac;
                                            LogAtt.LogTimeZone = ULIM.TimeZone;
                                            LogAtt.Working_Late_Night = false;
                                            LogAtt.Extra_working = false;
                                            LogAtt.CompanyID = CompanyID;
                                            LogAtt.BranchID = BranchID;
                                            db.crm_tbl_nipl_emp_attendance.Add(LogAtt);
                                            db.SaveChanges();
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    DateTime utcTime = DateTime.UtcNow;
                                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

                                    var time = localTime.ToString("hh:mm:ss tt");//System.DateTime.Now.ToString("hh:mm:ss tt");
                                    var Date = localTime.ToString("dd/MM/yyyy");//System.DateTime.Now.ToString("MM/dd/yyyy");
                                    var CheckExistAttand = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == ULIM.Id && em.L_In_Date == Date && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                    if (CheckExistAttand == null)
                                    {
                                        crm_tbl_nipl_emp_attendance LogAtt = new crm_tbl_nipl_emp_attendance();
                                        LogAtt.EmpId = ULIM.Id;
                                        LogAtt.L_In_Date = Date;
                                        LogAtt.L_In_Time = time;
                                        LogAtt.Status = "P";
                                        LogAtt.IPAddress = userIpAddress;
                                        LogAtt.MacAddress = mac;
                                        LogAtt.LogTimeZone = ULIM.TimeZone;
                                        LogAtt.Working_Late_Night = false;
                                        LogAtt.Extra_working = false;
                                        LogAtt.CompanyID = CompanyID;
                                        LogAtt.BranchID = BranchID;
                                        db.crm_tbl_nipl_emp_attendance.Add(LogAtt);
                                        db.SaveChanges();
                                    }
                                }
                                #endregion

                                //if (ULIM.ProfileId != null && ULIM.ProfileId != "")
                                //{
                                //    var GetProfileId = Convert.ToInt32(ULIM.ProfileId);
                                //    var GetPermission = db.crm_roleassigntbl.Where(em => em.Id == GetProfileId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                //    if (GetPermission != null)
                                //    {
                                //        Session["RightsCreateLeads"] = Convert.ToBoolean(GetPermission.CreateLeads);
                                //        Session["RightsViewLeads"] = Convert.ToBoolean(GetPermission.ViewLeads);
                                //        Session["RightsViewSales"] = Convert.ToBoolean(GetPermission.ViewSales);
                                //        Session["RightsCommonActivityRemarks"] = Convert.ToBoolean(GetPermission.CommonActivityRemark);
                                //        Session["RightsRoleManagement"] = Convert.ToBoolean(GetPermission.RoleManagement);
                                //        Session["RightsNotification"] = Convert.ToBoolean(GetPermission.Notification);
                                //        Session["RightsLeadNotify"] = Convert.ToBoolean(GetPermission.LeadNotify);
                                //        Session["RightsDeveloperReport"] = Convert.ToBoolean(GetPermission.DeveloperReport);
                                //        Session["RightsProjectManagement"] = Convert.ToBoolean(GetPermission.ProjectManagement);
                                //        Session["RightsCommonActivityRemarks"] = Convert.ToBoolean(GetPermission.CommonActivityRemark);
                                //        Session["RightsLeadManagement"] = Convert.ToBoolean(GetPermission.LeadManagement);
                                //        Session["RightsAssignLeadManagement"] = Convert.ToBoolean(GetPermission.AssignLeadManagement);
                                //        Session["RightsDailyworkSchedule"] = Convert.ToBoolean(GetPermission.DailyWorkSchedule);
                                //        Session["RightsPayments"] = Convert.ToBoolean(GetPermission.Viewpayment);
                                //    }
                                //}

                                #region Redirect URL
                                if (Session["ReturnUrl"] != null)
                                {
                                    return Redirect(Convert.ToString(Session["ReturnUrl"]));
                                }
                                else
                                {
                                    //check if user has assign view leads page then show lead report
                                    bool isSalePerson = false;
                                    string query = @"select ifnull(rl.ViewLeads,0) as ViewLeads
                                                    from crm_usertbl ur
                                                    join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                                    Where ur.Id=" + uLM.Id + " and ur.BranchID = " + BranchID + " and ur.CompanyID =" + CompanyID + "  and rl.ViewLeads = 1";
                                    isSalePerson = db.Database.SqlQuery<bool>(query).FirstOrDefault();

                                    #region Set-ProfilePage
                                    if (ULIM.ProfileName == "Developer")
                                    {
                                        Session["ProfilePageUrl"] = "/Nis/Developer";
                                        return Redirect("/Nis/Developer");
                                    }
                                    else if (ULIM.ProfileName == "Sales" || ULIM.ProfileName == "SuperAdmin" || isSalePerson == true)
                                    {
                                        //Session["ProfilePageUrl"] = "/"; index page
                                        Session["ProfilePageUrl"] = "/home/LeadReport";//lead summery page
                                        return Redirect("/home/LeadReport");
                                    }
                                    else if (ULIM.ProfileName == "HR")
                                    {
                                        Session["ProfilePageUrl"] = "/Hr/dashboard";
                                        return Redirect("/Hr/dashboard");
                                    }
                                    else
                                    {
                                        Session["ProfilePageUrl"] = "/Nis/Remark";
                                        return Redirect("/Nis/Remark");
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            else if (uLM.ProfileName != "SuperAdmin" && uLM.IsActive == false && uLM.IsExpired == true && ExpireStarDate.Date <= DateTime.Now.Date && ExpireEndDate.Date <= DateTime.Now.Date)
                            {
                                TempData["alert"] = "Subscription is expired, Please contact to administrator.!";
                            }
                            else if (uLM.ProfileName == "SuperAdmin" && uLM.IsActive == true && uLM.IsExpired == false && ExpireStarDate.Date <= DateTime.Now.Date && ExpireEndDate.Date <= DateTime.Now.Date)
                            {
                                TempData["alert"] = "Subscription is expired, Please contact to administrator.!";
                            }
                            else
                            {
                                TempData["alert"] = "Subscription is expired, Please contact to administrator.!";
                            }
                            #endregion
                        }
                        else
                        {
                            TempData["alert"] = "Username or e-mail is not active, Please contact to administrator.!";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "Username or e-mail is not active, Please contact to administrator.!";
                    }
                }
                else
                {
                    TempData["alert"] = "Username or e-mail and password does not exits,Please contact to administrator.!";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "Something went wrong! please try after some time";

            }
            return View();
        }

        public void DefaultWebsiteEquiry(Int32? CompanyID, Int32? LeadOwnerID)
        {
            try
            {
                var GetContactInfo = db.contactus.Where(em => em.CompanyID == CompanyID).ToList();
                if (GetContactInfo.Count > 0)
                {
                    var GetDefaultBranch = db.com_branches.Where(em => em.OrgBranchCode == CompanyID).FirstOrDefault();
                    var getLeadSource = db.crm_leadsource_tbl.Where(em => em.CompanyID == CompanyID && em.LeadsourceName == "Website Enquiry").FirstOrDefault();
                    if (GetDefaultBranch != null)
                    {
                        foreach (var item in GetContactInfo)
                        {
                            var CheckExists = db.crm_createleadstbl.Where(em => em.CompanyID == CompanyID && em.BranchID == GetDefaultBranch.OrgBranchCode && em.MobileNo == item.PhoneNo && em.EmailId == item.Email).FirstOrDefault();
                            if (CheckExists != null)
                            {
                                crm_leaddescriptiontbl desctbl = new crm_leaddescriptiontbl();
                                desctbl.BranchID = CheckExists.BranchID;
                                desctbl.CompanyID = CheckExists.CompanyID;
                                desctbl.Description = item.Message;
                                db.crm_leaddescriptiontbl.Add(desctbl);
                                db.SaveChanges();
                            }
                            else
                            {
                                crm_createleadstbl clead = new crm_createleadstbl();
                                clead.LeadOwner = LeadOwnerID;
                                clead.LeadsType = "Manual";
                                clead.BranchID = GetDefaultBranch.OrgBranchCode;
                                clead.CompanyID = CompanyID;
                                clead.MobileNo = item.PhoneNo;
                                clead.Description = item.Message;
                                clead.EmailId = item.Email;
                                clead.FollowDate = DateTime.ParseExact(item.CreatedDate.Value.ToShortDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateTime? Date = DateTime.ParseExact(item.CreatedDate.Value.ToShortDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                clead.date = Date.Value.ToString();
                                clead.FollowUpTime = item.CreatedDate.Value.ToShortTimeString();
                                clead.Createddate = item.CreatedDate;
                                clead.LeadResource = "Website Enquiry";
                                clead.LeadSourceID = getLeadSource == null ? 0 : getLeadSource.Id;
                                db.crm_createleadstbl.Add(clead);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
        }

        public void DefaultMasterDataDump(Int32? CompanyID, Int32? BranchID)
        {
            try
            {
                //var getProfiledump = db.crm_jobprofiletbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                //if (getProfiledump.Count == 0)
                //{
                //    var GetDefaultProfileList = db.crm_jobprofiletbl.Where(em => em.BranchID == 1 && em.CompanyID == 1).ToList();
                //    if (GetDefaultProfileList.Count > 0)
                //    {
                //        foreach (var item in GetDefaultProfileList)
                //        {
                //            crm_jobprofiletbl jpt = new crm_jobprofiletbl();
                //            jpt.Profile = item.Profile;
                //            jpt.Status = true;
                //            jpt.CompanyID = CompanyID;
                //            jpt.BranchID = BranchID;
                //            db.crm_jobprofiletbl.Add(jpt);
                //            db.SaveChanges();
                //        }
                //    }
                //}

                //var getleadsourcedump = db.crm_leadsource_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                //if (getleadsourcedump.Count == 0)
                //{
                //    var GetDefaultLeadSourceList = db.crm_leadsource_tbl.Where(em => em.BranchID == 1 && em.CompanyID == 1).ToList();
                //    if (GetDefaultLeadSourceList.Count > 0)
                //    {
                //        foreach (var item in GetDefaultLeadSourceList)
                //        {
                //            crm_leadsource_tbl lspt = new crm_leadsource_tbl();
                //            lspt.LeadsourceName = item.LeadsourceName;
                //            lspt.Status = true;
                //            lspt.Created_at = System.DateTime.Now;
                //            lspt.CompanyID = CompanyID;
                //            lspt.BranchID = BranchID;
                //            db.crm_leadsource_tbl.Add(lspt);
                //            db.SaveChanges();
                //        }
                //    }
                //}

                string[] ststusNames = { "Open", "Closed", "Not Interested", "Archive" };//add by default these lead status 

                for (int i = 0; i < ststusNames.Count(); i++)
                {
                    string ls = ststusNames[i];
                    var GetDefaultLeadStatusList = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == ls && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetDefaultLeadStatusList == null)
                    {
                        var dt = Constant.GetBharatTime();
                        crm_leadstatus_tbl lsst = new crm_leadstatus_tbl();
                        lsst.LeadStatusName = ls;
                        lsst.Status = true;
                        lsst.created_at = dt;
                        lsst.CompanyID = CompanyID;
                        lsst.BranchID = BranchID;
                        db.crm_leadstatus_tbl.Add(lsst);
                        db.SaveChanges();
                    }
                }
                //var getLeadStatusdump = db.crm_leadstatus_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                //if (getLeadStatusdump.Count == 0)
                //{
                //    var GetDefaultLeadStatusList = db.crm_leadstatus_tbl.Where(em => em.BranchID == 1 && em.CompanyID == 1).ToList();
                //    if (GetDefaultLeadStatusList.Count > 0)
                //    {
                //        foreach (var item in GetDefaultLeadStatusList)
                //        {
                //            crm_leadstatus_tbl lsst = new crm_leadstatus_tbl();
                //            lsst.LeadStatusName = item.LeadStatusName;
                //            lsst.Status = true;
                //            lsst.created_at = System.DateTime.Now;
                //            lsst.CompanyID = CompanyID;
                //            lsst.BranchID = BranchID;
                //            db.crm_leadstatus_tbl.Add(lsst);
                //            db.SaveChanges();
                //        }
                //    }
                //}

                //var getProductTypedump = db.crm_producttypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                //if (getProductTypedump.Count == 0)
                //{
                //    var GetDefaultProductTypeList = db.crm_producttypetbl.Where(em => em.BranchID == 1 && em.CompanyID == 1).ToList();
                //    if (GetDefaultProductTypeList.Count > 0)
                //    {
                //        foreach (var item in GetDefaultProductTypeList)
                //        {
                //            crm_producttypetbl ptt = new crm_producttypetbl();
                //            ptt.ProductTypeName = item.ProductTypeName;
                //            ptt.Status = true;
                //            ptt.CompanyID = CompanyID;
                //            ptt.BranchID = BranchID;
                //            db.crm_producttypetbl.Add(ptt);
                //            db.SaveChanges();
                //        }
                //    }
                //}

                //var getIntervewStatusdump = db.crm_intervewstatus.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                //if (getIntervewStatusdump.Count == 0)
                //{
                //    var GetDefaultIntervewStatusList = db.crm_intervewstatus.Where(em => em.BranchID == 1 && em.CompanyID == 1).ToList();
                //    if (GetDefaultIntervewStatusList.Count > 0)
                //    {
                //        foreach (var item in GetDefaultIntervewStatusList)
                //        {
                //            crm_intervewstatus cist = new crm_intervewstatus();
                //            cist.InterViewStatus = item.InterViewStatus;
                //            cist.Status = true;
                //            cist.CompanyID = CompanyID;
                //            cist.BranchID = BranchID;
                //            db.crm_intervewstatus.Add(cist);
                //            db.SaveChanges();
                //        }
                //    }
                //}
            }
            catch (Exception exc)
            {
                ExceptionLogging.SendExcepToDB(exc);
            }
        }


        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();

                    //sMacAddress = adapter.GetIPv4Statistics().ToString();
                    //sMacAddress = adapter.GetIPStatistics().ToString();
                }
            }
            return sMacAddress;
        }

        public ActionResult LogOut()
        {
            if (Session["UID"] != null)
            {
                try
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    var uid = Convert.ToInt32(Session["UID"]);
                    if (Convert.ToString(Session["TimeZone"]) != "IST" && Convert.ToString(Session["TimeZone"]) != null)
                    {
                        #region Get-Time-By-TimeZone
                        var timeZone = Convert.ToString(Session["TimeZone"]);
                        var GetData = db.crm_zonetbl.Where(em => em.ZoneName == timeZone).FirstOrDefault();
                        if (GetData != null)
                        {
                            var IST = System.DateTime.Now;
                            var NewtimeZoneDate = IST.AddHours(-5);
                            NewtimeZoneDate = NewtimeZoneDate.AddMinutes(-30);
                            if (GetData.ZoneHours > 0)
                            {
                                NewtimeZoneDate = NewtimeZoneDate.AddHours(GetData.ZoneHours);
                            }
                            else if (GetData.ZoneMin > 0)
                            {
                                NewtimeZoneDate = NewtimeZoneDate.AddMinutes(GetData.ZoneMin);
                            }

                            var NewZonetime = NewtimeZoneDate.ToString("hh:mm:ss tt");
                            var NewZoneDate = NewtimeZoneDate.ToString("dd/MM/yyyy");
                            var GetAttandData = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == uid && em.L_In_Date == NewZoneDate && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (GetAttandData != null)
                            {
                                //Calculate Login time by Datebase
                                var lgtime = GetAttandData.L_In_Date + " " + GetAttandData.L_In_Time;
                                DateTime logt = Convert.ToDateTime(lgtime);

                                //Calculate Log-Out time 
                                var logOuttime = NewZoneDate + " " + NewZonetime;
                                DateTime logOut = Convert.ToDateTime(logOuttime);
                                TimeSpan duration = (logOut - logt);
                                string dur = string.Format("{0:00}:{1:00}:{2:00}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
                                GetAttandData.L_Out_Date = NewZoneDate;
                                GetAttandData.L_Out_Time = NewZonetime;
                                GetAttandData.Duration = dur;
                                GetAttandData.BranchID = BranchID;
                                GetAttandData.CompanyID = CompanyID;
                                db.SaveChanges();
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        //Start To get The Server time and Convert it to Local Machine....
                        //DateTime utcTime = DateTime.UtcNow;
                        //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                        DateTime localTime = Constant.GetBharatTime();
                        var time = localTime.ToString("hh:mm:ss tt");//System.DateTime.Now.ToString("hh:mm:ss tt");
                        var Date = localTime.ToString("dd/MM/yyyy");//System.DateTime.Now.ToString("MM/dd/yyyy");
                        var GetAttandData = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == uid && em.L_In_Date == Date && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetAttandData != null)
                        {
                            var lgtime = GetAttandData.L_In_Date + " " + GetAttandData.L_In_Time;
                            DateTime logt = Convert.ToDateTime(lgtime);
                            DateTime dt = localTime;//DateTime.Now;
                            TimeSpan duration = (dt - logt);
                            string dur = string.Format("{0:00}:{1:00}:{2:00}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
                            GetAttandData.L_Out_Date = Date;
                            GetAttandData.L_Out_Time = time;
                            GetAttandData.Duration = dur;
                            GetAttandData.BranchID = BranchID;
                            GetAttandData.CompanyID = CompanyID;
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                }

                Session.Abandon();
                Session.Clear();
                string loggedOutPageUrl = "/home/Login";
                Response.Write("<script language='javascript'>");
                Response.Write("function ClearHistory()");
                Response.Write("{");
                Response.Write(" var backlen=history.length;");
                Response.Write(" history.go(-backlen);");
                Response.Write(" window.location.href='" + loggedOutPageUrl + "'; ");
                Response.Write("}");
                Response.Write("</script>");
                return RedirectToAction("login", "home");
                //Redirect("/home/Login");
            }
            else
            {
                return RedirectToAction("login", "home");
            }
        }

        #endregion

        #region Leads-Management

        public ActionResult createleads(string id = "")
        {
            var permission = new CommonRepository();
            var encode = new StringCipher();
            var UserProfileName = Convert.ToString(Session["UserType"]);
            CreateLeadsModel CLM = new CreateLeadsModel();

            var rights = permission.GetUserRights();
            if (rights == null || (!rights.CreateLeads))
            {
                #region redirect-ProfilePage  
                if (UserProfileName == "Developer")
                {
                    TempData["alert"] = "Sorry! you do not have a right to create lead, please contact administrator";

                    return Redirect("/Nis/Developer");
                }
                else if (UserProfileName == "Sales")
                {
                    TempData["alert"] = "Sorry! you do not have a right to create lead, please contact administrator";

                    return Redirect("/");
                }
                else if (UserProfileName == "HR")
                {
                    TempData["alert"] = "Sorry! you do not have a right to create lead, please contact administrator";

                    return Redirect("/Hr/dashboard");
                }
                else
                {
                    TempData["alert"] = "Sorry! you do not have a right to create lead, please contact administrator";

                    return Redirect("/Nis/Remark");
                }
                #endregion              
            }
            try
            {
                if (Session["UserName"] != null)
                {
                    int BranchID = Convert.ToInt32(Session["BranchID"]);
                    int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    int UserId = Convert.ToInt32(Session["UID"]);
                    CLM.LeadOwner = Convert.ToString(Session["UserName"]);
                    CLM.propVal = cr.GetCreateLeadsetting();
                    CLM.CompanyNodaTimeZone = cr.GetCompanyNodaTimeZone(CompanyID);//get company noda time zone name
                    CLM.IsProductTypeAdd = rights == null ? false : rights.IsProductTypes;//get rigts for add product if not in list
                    CLM.IsLeadSourceAdd = rights == null ? false : rights.IsLeadSource;//get rigts for add lead souce if not in list
                    CLM.IsLeadStatusAdd = rights == null ? false : rights.IsLeadStatus;//get rigts for add lead status if not in list

                    CLM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

                    long? Lid = null;
                    int countryId = cr.GetCompanyCountryId();
                    CLM.CountryID = countryId;


                    #region encrypt-decrypt lead id
                    if (!string.IsNullOrEmpty(id))
                    {
                        var regExp = Regex.Matches(id, @"[a-zA-Z=]").Count;

                        if (regExp == 0)
                        {
                            id = encode.Encrypt(id);//encrypt id if not encrypted format
                        }

                        id = HttpUtility.UrlDecode(encode.Decrypt(id));//decrypt id
                        Lid = Convert.ToInt64(id.ToString());//convert string id to integer id
                    }
                    #endregion

                    TempData["LeadID"] = Lid;
                    //string assignQry = @"select Id as UserID,CONCAT(Fname ,' ', Lname) as UserName from crm_usertbl where Id!='"+ Convert.ToInt32(Session["UID"]) + "' and ProfileName= 'Sales' and Status=1 and BranchID='" + BranchID+"' and CompanyID='"+CompanyID+"' order by Fname";
                    //CLM.AssignToList = db.Database.SqlQuery<CreateUserModel>(assignQry).ToList();
                    CLM.AssignToList = new List<CreateUserModel>();
                    string assignQry = @"select ur.Id as UserID,  CONCAT(ur.FName ,' ', ur.Lname, '(' , ur.EmployeeCode,')') AS UserName  
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewLeads = 1";
                    var AssignList = db.Database.SqlQuery<CreateUserModel>(assignQry).OrderBy(a => a.UserName).ToList();
                    var userId = Convert.ToInt32(Session["UID"]);
                    if (AssignList != null && AssignList.Count > 0)
                    {
                        CLM.AssignToList = AssignList.Where(a => a.UserID != userId).ToList();
                    }

                    #region Smartcapita admin add to assign user list
                    if (CompanyID == 296)
                    {
                        CLM.AssignToList.Add(new CreateUserModel { UserID = 50, UserName = "Smart Capita Admin" });
                    }


                    #endregion
                    #region Get other branch user list to add assignTo list
                    DataTable GetAssignToRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + BranchID + "','','')");
                    if (GetAssignToRecords.Rows.Count > 0)
                    {
                        var GetOtherBranchUser = (from dr in GetAssignToRecords.AsEnumerable()
                                                  select new CreateUserModel()
                                                  {
                                                      UserID = Convert.ToInt32(dr["AssignedUserID"]),
                                                      UserName = Convert.ToString(dr["UserName"] + " (" + dr["EmployeeCode"] + ")" + " (Branch: " + dr["BranchName"] + ")"),
                                                      IsActive = Convert.ToBoolean(dr["IsActive"])
                                                  }).Where(a => a.IsActive == true).OrderBy(a => a.UserName).ToList();
                        CLM.AssignToList.AddRange(GetOtherBranchUser);
                        CLM.AssignToList = CLM.AssignToList.OrderBy(a => a.UserName).ToList();

                    }

                    #endregion

                    #region Lead Status
                    string leadStatusQry = @"select Id,LeadStatusName from crm_leadstatus_tbl where BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "'and Status=1";
                    CLM.leadstatusList = db.Database.SqlQuery<LeadStatusModel>(leadStatusQry).OrderBy(a => a.LeadStatusname).ToList();
                    //if (CLM.leadstatusList != null && CLM.leadstatusList.Count > 0)
                    //{
                    //    CLM.leadstatusList.Add(new LeadStatusModel { Id = 0, LeadStatusname = "Archive" });
                    //    CLM.leadstatusList = CLM.leadstatusList.OrderBy(a => a.LeadStatusname).ToList();
                    //}
                    //else
                    //{
                    //    CLM.leadstatusList = new List<LeadStatusModel>();
                    //    CLM.leadstatusList.Add(new LeadStatusModel { Id = 0, LeadStatusname = "Archive" });
                    //}

                    #endregion


                    #region Drop down Source
                    string leaddropdownsource = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol1dropdown'";
                    CLM.leaddropdownlist = db.Database.SqlQuery<Leaddropdownmodel1>(leaddropdownsource).OrderBy(a => a.DropDownItemNamw).ToList();


                    string leaddropdownsource2 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol2dropdown'";
                    CLM.leaddropdownlist2 = db.Database.SqlQuery<Leaddropdownmodel2>(leaddropdownsource2).OrderBy(a => a.DropDownItemNamw).ToList();

                    string leaddropdownsource3 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol3dropdown'";
                    CLM.leaddropdownlist3 = db.Database.SqlQuery<Leaddropdownmodel3>(leaddropdownsource3).OrderBy(a => a.DropDownItemNamw).ToList();

                    string leaddropdownsource4 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol4dropdown'";
                    CLM.leaddropdownlist4 = db.Database.SqlQuery<Leaddropdownmodel4>(leaddropdownsource4).OrderBy(a => a.DropDownItemNamw).ToList();

                    string leaddropdownsource5 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol5dropdown'";
                    CLM.leaddropdownlist5 = db.Database.SqlQuery<Leaddropdownmodel5>(leaddropdownsource5).OrderBy(a => a.DropDownItemNamw).ToList();
                    #endregion

                    #region Lead Source
                    string leadSourceQuery = @"select Id,LeadsourceName from crm_leadsource_tbl Where Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "'";
                    CLM.leadsourceList = db.Database.SqlQuery<LeadSourceModel>(leadSourceQuery).OrderBy(a => a.LeadsourceName).ToList();

                    #endregion

                    #region Product Type
                    string pTypeQry = @" select Id,ProductTypeName from crm_producttypetbl Where Status=1 and BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'";
                    CLM.producttypetblList = db.Database.SqlQuery<ProductTypeModel>(pTypeQry).OrderBy(a => a.ProductTypeName).ToList();

                    #endregion

                    #region Country List
                    string countryQry = @"select id as CountryID,country_name as CountryName,country_code from acc_countries";
                    CLM.countryList = db.Database.SqlQuery<ManageCountryModel>(countryQry).OrderBy(a => a.CountryName).ToList();

                    #endregion


                    if (!string.IsNullOrEmpty(id))
                    {
                        var GetLeadsData = new crm_createleadstbl();

                        if (UserProfileName == "SuperAdmin")
                        {
                            GetLeadsData = db.crm_createleadstbl.Find(Lid);
                        }
                        else
                        {
                            string query = @"select * from `crm_createLeadstbl` Where Id='" + Lid + "' and (LeadOwner = '" + UserId + "' or (AssignTo !='' and AssignTo = '" + UserId + "') or (BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'))";
                            GetLeadsData = db.Database.SqlQuery<crm_createleadstbl>(query).FirstOrDefault();
                        }
                        //var GetLeadsData = db.crm_createleadstbl.Where(em => em.Id == Lid && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetLeadsData != null)
                        {
                            var data = cr.GetUserCompanyBranch(GetLeadsData.LeadOwner ?? 0);

                            //var GetLeadOwnerInfo = db.crm_usertbl.Where(em => em.Id == GetLeadsData.LeadOwner && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            //if (GetLeadOwnerInfo != null)
                            //{

                            CLM.createdDate = GetLeadsData.Createddate.Value.ToString(CLM.DateFormat);
                            CLM.LID = Convert.ToInt64(Lid);
                            CLM.BranchID = GetLeadsData.BranchID;
                            CLM.CompanyID = GetLeadsData.CompanyID;
                            CLM.LeadOwnerID = GetLeadsData.LeadOwner ?? 0;
                            CLM.LeadOwner = string.Format("{0} {1}", data.Fname, data.Lname);
                            CLM.MobileNo = GetLeadsData.MobileNo.Trim();
                            CLM.EmailId = GetLeadsData.EmailId;
                            //CLM.OtherNo = GetLeadsData.OtherNo;
                            CLM.LeadSourceID = GetLeadsData.LeadSourceID;
                            //CLM.Description = GetLeadsData.Description;
                            CLM.LeadStatusID = GetLeadsData.LeadStatusID;
                            CLM.LeadStatus = GetLeadsData.LeadStatus;
                            CLM.Customer = GetLeadsData.Customer;
                            CLM.Designation = GetLeadsData.Designation;
                            CLM.OrganizationName = GetLeadsData.OrganizationName;
                            CLM.FollowDate = GetLeadsData.FollowDate.Value.ToShortDateString();
                            CLM.Country = GetLeadsData.Country;
                            CLM.City = GetLeadsData.City;
                            CLM.State = GetLeadsData.State;
                            CLM.CountryID = GetLeadsData.CountryID != null ? GetLeadsData.CountryID : countryId;
                            CLM.StateID = GetLeadsData.StateID;
                            CLM.CityID = GetLeadsData.CityID;
                            //CLM.Notify = Convert.ToBoolean(GetLeadsData.Notify);
                            CLM.FollowupTime = GetLeadsData.FollowUpTime;
                            CLM.AssignTo = Convert.ToInt32(GetLeadsData.AssignTo);
                            CLM.Address = Convert.ToString(GetLeadsData.Address);
                            CLM.ProductTypeID = GetLeadsData.ProductTypeID;

                            //set company default time zone if time zone empty
                            if (!string.IsNullOrEmpty(GetLeadsData.ZoneName) && !GetLeadsData.ZoneName.ToLower().Contains("Select"))
                            {
                                CLM.TimeZoneName = GetLeadsData.ZoneName /*: Constant.GetCompanyTimeZone(CompanyID)*/;
                            }

                            if (GetLeadsData.ConvertedFupDateTime != null)
                            {
                                CLM.FollowupTimeInTZ = string.Format("{0:MM-dd-yyyy HH:mm:ss}", GetLeadsData.ConvertedFupDateTime);
                                CLM.FollowupTimeIST = string.Format("{0:" + CLM.DateFormat + " hh:mm tt}", GetLeadsData.ConvertedFupDateTime);
                            }
                            //else
                            //{
                            //    if (!string.IsNullOrEmpty(GetLeadsData.FollowUpTime))
                            //    {
                            //        var finalDateTime = string.Format("{0} {1}", CLM.FollowDate, GetLeadsData.FollowUpTime);

                            //        DateTime dateTime = Convert.ToDateTime(finalDateTime);
                            //        CLM.FollowupTimeInTZ = string.Format("{0:MM-dd-yyyy HH:mm:ss}", dateTime);
                            //    }
                            //    else
                            //    {
                            //        DateTime dateTime = Convert.ToDateTime(CLM.FollowDate);
                            //        CLM.FollowupTimeInTZ = string.Format("{0:MM-dd-yyyy HH:mm:ss}", dateTime);
                            //    }
                            //}


                            if (GetLeadsData.ExpectedDate != null)
                            {
                                CLM.ExpectedDate = String.Format("{0:" + CLM.DateFormat + "}", GetLeadsData.ExpectedDate);
                            }
                            CLM.ExpectedProductAmount = GetLeadsData.ExpectedProductAmount;
                            if (!String.IsNullOrWhiteSpace(GetLeadsData.DateofBirth))
                            {
                                //CLM.DateofBirth = Convert.ToString(GetLeadsData.DateofBirth);

                                var dobDate = Convert.ToDateTime(GetLeadsData.DateofBirth);
                                CLM.DateofBirth = String.Format("{0:" + CLM.DateFormat + "}", dobDate);
                            }
                            if (!String.IsNullOrWhiteSpace(GetLeadsData.MarriageAnniversary))
                            {
                                //CLM.MarriageAnniversary = Convert.ToString(GetLeadsData.MarriageAnniversary);
                                var mrgDate = Convert.ToDateTime(GetLeadsData.MarriageAnniversary);
                                CLM.MarriageAnniversary = String.Format("{0:" + CLM.DateFormat + "}", mrgDate);
                            }
                            CLM.ExtraCol1 = GetLeadsData.ExtraCol1;
                            CLM.ExtraCol2 = GetLeadsData.ExtraCol2;
                            CLM.ExtraCol3 = GetLeadsData.ExtraCol3;
                            CLM.ExtraCol4 = GetLeadsData.ExtraCol4;
                            CLM.ExtraCol5 = GetLeadsData.ExtraCol5;
                            CLM.ExtraCol6 = GetLeadsData.ExtraCol6;
                            CLM.ExtraCol7 = GetLeadsData.ExtraCol7;
                            CLM.ExtraCol8 = GetLeadsData.ExtraCol8;

                            if (GetLeadsData.ExtraCol9 != null)
                            {
                                CLM.ExtraCol9 = String.Format("{0:" + CLM.DateFormat + "}", GetLeadsData.ExtraCol9);
                            }
                            if (GetLeadsData.ExtraCol10 != null)
                            {
                                CLM.ExtraCol10 = String.Format("{0:" + CLM.DateFormat + "}", GetLeadsData.ExtraCol10);
                            }
                            CLM.ExtraCol11 = GetLeadsData.ExtraCol11;
                            CLM.ExtraCol12 = GetLeadsData.ExtraCol12;
                            CLM.ExtraCol13 = GetLeadsData.ExtraCol13;
                            CLM.ExtraCol14 = GetLeadsData.ExtraCol14;
                            CLM.ExtraCol15 = GetLeadsData.ExtraCol15;
                            CLM.ExtraCol16 = GetLeadsData.ExtraCol16;
                            CLM.ExtraCol17 = GetLeadsData.ExtraCol17;
                            if (GetLeadsData.ExtraCol18 != null)
                            {
                                CLM.ExtraCol18 = String.Format("{0:" + CLM.DateFormat + "}", GetLeadsData.ExtraCol18);
                            }
                            if (GetLeadsData.ExtraCol19 != null)
                            {
                                CLM.ExtraCol19 = String.Format("{0:" + CLM.DateFormat + "}", GetLeadsData.ExtraCol19);
                            }
                            if (GetLeadsData.ExtraCol20 != null)
                            {
                                CLM.ExtraCol20 = String.Format("{0:" + CLM.DateFormat + "}", GetLeadsData.ExtraCol20);
                            }
                            //CLM.Extracol1dropId = GetLeadsData.Extracol1dropdown1Id;
                            //CLM.Extracol1dropdown = GetLeadsData.Extracol1dropdown1;
                            CLM.Extracol1dropdownId1 = GetLeadsData.Extracol1dropdownId1;
                            CLM.Extracol1dropdownId2 = GetLeadsData.Extracol1dropdownId2;
                            CLM.Extracol1dropdownId3 = GetLeadsData.Extracol1dropdownId3;
                            CLM.Extracol1dropdownId4 = GetLeadsData.Extracol1dropdownId4;
                            CLM.Extracol1dropdownId5 = GetLeadsData.Extracol1dropdownId5;
                            CLM.Extracol1dropdown = GetLeadsData.Dp1name;
                            CLM.Extracol2dropdown = GetLeadsData.Dp2name;
                            CLM.Extracol3dropdown = GetLeadsData.Dp3name;
                            CLM.Extracol4dropdown = GetLeadsData.Dp4name;
                            CLM.Extracol5dropdown = GetLeadsData.Dp5name;
                            CLM.PastPerformance = GetLeadsData.PastPerformance;
                            CLM.ProfitLoss = GetLeadsData.ProfitLoss;
                            CLM.BuySell = GetLeadsData.BuySell;
                            CLM.StockName = GetLeadsData.StockName;
                            CLM.Price = GetLeadsData.Price;
                            CLM.Target = GetLeadsData.Target;
                            CLM.Target2 = GetLeadsData.Target2;
                            CLM.Target3 = GetLeadsData.Target3;
                            CLM.SI = GetLeadsData.SI;
                            CLM.Remark = GetLeadsData.Remark;

                            //}                          
                        }
                        else
                        {
                            #region redirect-ProfilePage  
                            if (UserProfileName == "Developer")
                            {
                                TempData["alert"] = "Sorry! no lead record found";
                                return Redirect("/Nis/Developer");
                            }
                            else if (UserProfileName == "Sales" || UserProfileName == "SuperAdmin")
                            {
                                TempData["alert"] = "Sorry! no lead record found";
                                return Redirect("/");
                            }
                            else if (UserProfileName == "HR")
                            {
                                TempData["alert"] = "Sorry! no lead record found";
                                return Redirect("/Hr/dashboard");
                            }
                            else
                            {
                                TempData["alert"] = "Sorry! no lead record found";
                                return Redirect("/Nis/Remark");
                            }
                            #endregion                            
                        }

                    }
                    else
                    {
                        var dt = Constant.GetBharatTime();
                        CLM.createdDate = dt.ToString(CLM.DateFormat);
                        if (Request.QueryString["newcustomer"] != null)
                        {
                            var leadstatus = Convert.ToString(Request.QueryString["newcustomer"]);
                            CLM.LeadStatus = "Closed";
                            //set company default time zone if time zone empty
                            //CLM.TimeZoneName = Constant.GetCompanyTimeZone(CompanyID);     

                            //CLM.FollowupTimeInTZ = string.Format("{0:MM-dd-yyyy HH:mm:ss}", dt);
                            //CLM.FollowupTime= dt.ToString("hh:mm tt");//get time only
                            //string Dformat = string.Format("{0}", CLM.DateFormat + " hh:mm tt");
                            //CLM.FollowupTimeIST= string.Format("{0:" + Dformat + "}", dt);//get default followup time in timezone

                        }
                    }

                    #region Select-TimeZone
                    //var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList().Select(em => new crm_zonetbl
                    //{
                    //    ZoneName = em.ZoneName
                    //}).AsQueryable();
                    //ViewBag.TimeZoneName1 = new SelectList(GetZoneName, "ZoneName", "ZoneName");
                    //string tzqury = @"select distinct standard_zone_name as StandardTZName from time_zone order by standard_zone_name";
                    //var tzData = db.Database.SqlQuery<TZName>(tzqury).ToList();

                    //CLM.TimeZones = new SelectList(tzData, "StandardTZName", "StandardTZName", CLM.TimeZoneName);
                    #endregion
                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "** Sorry there is some technical problem. please try again.";
            }
            return View(CLM);
        }

        [HttpPost]
        public ActionResult createleads(CreateLeadsModel CLM, HttpPostedFileBase file, string MobileNo, string id = "", string newcustomer = "")
        {

            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            int LoggedBranchID = Convert.ToInt32(Session["BranchID"]);
            int LoggedCompanyID = Convert.ToInt32(Session["CompanyID"]);

            CLM.propVal = cr.GetCreateLeadsetting();
            CLM.CompanyNodaTimeZone = cr.GetCompanyNodaTimeZone(CompanyID);//get company noda time zone name
            CLM.DateFormat = Constant.DateFormat();//get date format by company id
                                                   // CLM.Language = cr.GetCompanyLanguage(0);
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            // string assignQry = @"select Id as UserID,CONCAT(Fname ,' ', Lname) as UserName from crm_usertbl where Id!='" + Convert.ToInt32(Session["UID"]) + "' and Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' order by Fname";
            //string assignQry = @"select Id as UserID,CONCAT(Fname ,' ', Lname) as UserName from crm_usertbl where Id!='" + Convert.ToInt32(Session["UID"]) + "' and ProfileName= 'Sales' and Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' order by Fname";

            //CLM.AssignToList = db.Database.SqlQuery<CreateUserModel>(assignQry).ToList();
            CLM.AssignToList = new List<CreateUserModel>();
            string assignQry = @"select ur.Id as UserID,  CONCAT(ur.FName ,' ', ur.Lname, '(' , ur.EmployeeCode,')') AS UserName 
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewLeads = 1";
            var AssignList = db.Database.SqlQuery<CreateUserModel>(assignQry).OrderBy(a => a.UserName).ToList();
            var userId = Convert.ToInt32(Session["UID"]);
            if (AssignList != null && AssignList.Count > 0)
            {
                CLM.AssignToList = AssignList.Where(a => a.UserID != userId).ToList();
            }

            #region Smartcapita admin add to assign user list
            CLM.AssignToList.Add(new CreateUserModel { UserID = 50, UserName = "Smart Capita Admin" });
            #endregion

            #region Get other branch user list to add assignTo list
            DataTable GetAssignToRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + BranchID + "','','')");
            if (GetAssignToRecords.Rows.Count > 0)
            {
                var GetOtherBranchUser = (from dr in GetAssignToRecords.AsEnumerable()
                                          select new CreateUserModel()
                                          {
                                              UserID = Convert.ToInt32(dr["AssignedUserID"]),
                                              UserName = Convert.ToString(dr["UserName"] + " (" + dr["EmployeeCode"] + ")" + " (Branch: " + dr["BranchName"] + ")"),
                                              IsActive = Convert.ToBoolean(dr["IsActive"])
                                          }).Where(a => a.IsActive == true).OrderBy(a => a.UserName).ToList();
                CLM.AssignToList.AddRange(GetOtherBranchUser);

                CLM.AssignToList = CLM.AssignToList.OrderBy(a => a.UserName).ToList();
            }

            #endregion

            #region Select-TimeZone
            //var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList().Select(em => new crm_zonetbl
            //{
            //    ZoneName = em.ZoneName
            //}).AsQueryable();
            //ViewBag.TimeZoneName1 = new SelectList(GetZoneName, "ZoneName", "ZoneName");
            //string tzqury = @"select distinct standard_zone_name as StandardTZName from time_zone order by standard_zone_name";
            //var tzData = db.Database.SqlQuery<TZName>(tzqury).ToList();

            //CLM.TimeZones = new SelectList(tzData, "StandardTZName", "StandardTZName", CLM.TimeZoneName);
            #endregion

            #region Lead Status
            string leadStatusQry = @"select Id,LeadStatusName from crm_leadstatus_tbl where BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "'and Status=1";
            CLM.leadstatusList = db.Database.SqlQuery<LeadStatusModel>(leadStatusQry).OrderBy(a => a.LeadStatusname).ToList();
            //if (CLM.leadstatusList != null && CLM.leadstatusList.Count > 0)
            //{
            //    CLM.leadstatusList.Add(new LeadStatusModel { Id = 0, LeadStatusname = "Archive" });
            //    CLM.leadstatusList = CLM.leadstatusList.OrderBy(a => a.LeadStatusname).ToList();
            //}
            //else
            //{
            //    CLM.leadstatusList = new List<LeadStatusModel>();
            //    CLM.leadstatusList.Add(new LeadStatusModel { Id = 0, LeadStatusname = "Archive" });
            //}
            #endregion
            #region Drop down Source
            string leaddropdownsource = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol1dropdown'";
            CLM.leaddropdownlist = db.Database.SqlQuery<Leaddropdownmodel1>(leaddropdownsource).OrderBy(a => a.DropDownItemNamw).ToList();


            string leaddropdownsource2 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol2dropdown'";
            CLM.leaddropdownlist2 = db.Database.SqlQuery<Leaddropdownmodel2>(leaddropdownsource2).OrderBy(a => a.DropDownItemNamw).ToList();

            string leaddropdownsource3 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol3dropdown'";
            CLM.leaddropdownlist3 = db.Database.SqlQuery<Leaddropdownmodel3>(leaddropdownsource3).OrderBy(a => a.DropDownItemNamw).ToList();

            string leaddropdownsource4 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol4dropdown'";
            CLM.leaddropdownlist4 = db.Database.SqlQuery<Leaddropdownmodel4>(leaddropdownsource4).OrderBy(a => a.DropDownItemNamw).ToList();

            string leaddropdownsource5 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol5dropdown'";
            CLM.leaddropdownlist5 = db.Database.SqlQuery<Leaddropdownmodel5>(leaddropdownsource5).OrderBy(a => a.DropDownItemNamw).ToList();
            #endregion
            #region Lead Source
            string leadSourceQuery = @"select Id,LeadsourceName from crm_leadsource_tbl Where Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "'";
            CLM.leadsourceList = db.Database.SqlQuery<LeadSourceModel>(leadSourceQuery).OrderBy(a => a.LeadsourceName).ToList();

            #endregion

            #region Product Type
            string pTypeQry = @" select Id,ProductTypeName from crm_producttypetbl Where Status=1 and BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'";
            CLM.producttypetblList = db.Database.SqlQuery<ProductTypeModel>(pTypeQry).OrderBy(a => a.ProductTypeName).ToList();

            #endregion

            #region Country List
            string countryQry = @"select id as CountryID,country_name as CountryName,country_code from acc_countries";
            CLM.countryList = db.Database.SqlQuery<ManageCountryModel>(countryQry).OrderBy(a => a.CountryName).ToList();

            #endregion

            if (!ModelState.IsValid)
            {
                var message = string.Join(", ", ModelState.Values.SelectMany(a => a.Errors).Select(a => a.ErrorMessage));
                TempData["alert"] = message;
                return View(CLM);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    if (Session["UserName"] != null && Session["UID"] != null)
                    {

                        int? CRMCustomerID = 0;

                        DateTime localTime = Constant.GetBharatTime();

                        if (!string.IsNullOrEmpty(id))
                        {
                            var encode = new StringCipher();
                            //string EncryptionCode = HttpUtility.UrlEncode(encode.Encrypt(id));
                            var regExp = Regex.Matches(id, @"[a-zA-Z=]").Count;
                            if (regExp == 0)
                            {
                                id = encode.Encrypt(id);//encrypt id if not encrypted format
                            }
                            id = HttpUtility.UrlDecode(encode.Decrypt(id));//decrypt id
                            long Lid = Convert.ToInt64(id.ToString());// convert into integer

                            #region Update-Leads
                            var GetLeads = db.crm_createleadstbl.Where(em => em.Id == Lid && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (GetLeads != null)
                            {
                                if (CLM.DateFormat != "dd/MM/yyyy")
                                {
                                    var followDate = DateTime.ParseExact(CLM.FollowDate, CLM.DateFormat, CultureInfo.InvariantCulture);
                                    CLM.FollowDate = string.Format("{0:dd/MM/yyyy}", followDate);
                                }
                                if (Convert.ToDateTime(CLM.FollowDate).Date < localTime.Date && CLM.LeadStatus != "Not Interested")
                                {
                                    TempData["alert"] = "Please select followup date greater after the day";
                                    return View(CLM);
                                }
                                if (CompanyID == 2458)
                                {
                                    //if(!string.IsNullOrEmpty(CLM.ExtraCol9))
                                    //{
                                    //    var extracol9date = CLM.ExtraCol9;
                                    //    CLM.ExtraCol9 = string.Format("{0:dd/MM/yyyy}", extracol9date);
                                    //    var checkdate = db.crm_createleadstbl.Any(x => x.ExtraCol9 == Convert.ToDateTime(CLM.ExtraCol9).Date && x.BranchID == LoggedBranchID && x.CompanyID == LoggedCompanyID);

                                    //    if(checkdate!=null)
                                    //    {
                                    //        TempData["alert"] = "This date already booked for venue!";
                                    //        return View(CLM);
                                    //    }
                                    //}
                                    if (CLM.ExtraCol9 != null)
                                    {
                                        DateTime extracol9 = Convert.ToDateTime(CLM.ExtraCol9);
                                        var checkdate = db.crm_createleadstbl.Where(x => x.ExtraCol9 == extracol9 && x.BranchID == LoggedBranchID && x.CompanyID == LoggedCompanyID && x.Dp1name == CLM.Extracol1dropdown).FirstOrDefault();
                                        if (checkdate != null)
                                        {
                                            TempData["alert"] = "This date already booked for venue!";
                                            return View(CLM);
                                        }
                                    }

                                }
                                string date = localTime.ToString("dd/MM/yyyy");

                                var CheckExistDescript = db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid && em.Date == date && em.BranchID == LoggedBranchID && em.CompanyID == LoggedCompanyID).FirstOrDefault();
                                if (!string.IsNullOrEmpty(GetLeads.AssignTo))
                                {
                                    var AssignTo = Convert.ToInt32(GetLeads.AssignTo);
                                    var data = cr.GetUserCompanyBranch(AssignTo);
                                    if (data != null)
                                    {
                                        BranchID = data.BranchID;
                                        CompanyID = data.CompanyID;
                                    }
                                }
                                if (CheckExistDescript != null || !string.IsNullOrEmpty(CLM.Description))
                                {
                                    CRMCustomerID = Convert.ToInt32(id.ToString());
                                    #region Delayed Lead Record
                                    if (!String.IsNullOrEmpty(GetLeads.FollowDate.Value.ToShortDateString()))
                                    {
                                        DateTime PreFolloupDate = Convert.ToDateTime(GetLeads.FollowDate);
                                        var existRecord = db.crm_delayedfollowuprecordtbl.Where(em => em.LeadId == CRMCustomerID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                        //if(CLM.DateFormat!="dd/MM/yyyy")
                                        //{
                                        //    var followDate= DateTime.ParseExact(CLM.FollowDate, CLM.DateFormat, CultureInfo.InvariantCulture);
                                        //    CLM.FollowDate= string.Format("{0:dd/MM/yyyy}",followDate);
                                        //}
                                        if (Convert.ToDateTime(CLM.FollowDate).Date > PreFolloupDate.Date && existRecord != null)
                                        {
                                            existRecord.PreFollowUpDate = PreFolloupDate.Date;
                                            existRecord.CreatedDate = localTime.Date;
                                            existRecord.CreatedDatetime = localTime;
                                            db.SaveChanges();
                                        }
                                        else
                                        {

                                            crm_delayedfollowuprecordtbl dfr = new crm_delayedfollowuprecordtbl();
                                            if (Convert.ToDateTime(CLM.FollowDate).Date > PreFolloupDate.Date)
                                            {
                                                dfr.CreatedBy = Convert.ToInt32(Session["UID"]);
                                                dfr.CreatedDate = localTime.Date;
                                                dfr.CreatedDatetime = localTime;
                                                dfr.PreFollowUpDate = PreFolloupDate.Date;
                                                dfr.LeadId = Convert.ToInt32(GetLeads.Id);
                                                dfr.BranchID = Convert.ToInt32(Session["BranchID"]);
                                                dfr.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                                                db.crm_delayedfollowuprecordtbl.Add(dfr);
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Update-Lead
                                    GetLeads.Customer = CLM.Customer;
                                    GetLeads.MobileNo = CLM.MobileNo.Replace("+91", "").Trim();
                                    GetLeads.Designation = CLM.Designation;
                                    GetLeads.EmailId = CLM.EmailId;
                                    GetLeads.OrganizationName = CLM.OrganizationName;
                                    GetLeads.Description = CLM.Description;

                                    GetLeads.ZoneName = CLM.TimeZoneName;
                                    if (!string.IsNullOrEmpty(GetLeads.ZoneName) && GetLeads.ZoneName.ToLower().Contains("select"))
                                    {
                                        GetLeads.ZoneName = Constant.GetCompanyTimeZone(CompanyID);
                                    }
                                    GetLeads.IsLeadReminder = true;
                                    if (!string.IsNullOrWhiteSpace(CLM.FollowDate))
                                    {
                                        //GetLeads.FollowDate = Convert.ToDateTime(CLM.FollowDate);
                                        GetLeads.FollowDate = DateTime.ParseExact(CLM.FollowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }
                                    GetLeads.ModifiedDate = Constant.GetBharatTime();
                                    GetLeads.FollowUpTime = CLM.FollowupTime;
                                    //if(string.IsNullOrEmpty(GetLeads.FollowUpTime))
                                    //{
                                    //    GetLeads.FollowUpTime = localTime.ToString("hh:mm tt");
                                    //}
                                    //GetLeads.FollowupTimeinIST = CLM.FollowupTimeIST;
                                    GetLeads.CountryID = CLM.CountryID;
                                    GetLeads.Country = CLM.Country;
                                    GetLeads.CityID = CLM.CityID;
                                    GetLeads.City = CLM.City;
                                    GetLeads.StateID = CLM.StateID;
                                    GetLeads.State = CLM.State;
                                    GetLeads.Address = CLM.Address;
                                    GetLeads.BranchID = BranchID;
                                    GetLeads.CompanyID = CompanyID;
                                    if (!string.IsNullOrEmpty(CLM.FollowupTimeInTZ))
                                    {
                                        GetLeads.ConvertedFupDateTime = DateTime.ParseExact(CLM.FollowupTimeInTZ, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); // Convert.ToDateTime(CLM.FollowupTimeInTZ);
                                    }
                                    //else
                                    //{
                                    //    GetLeads.ConvertedFupDateTime = localTime;
                                    //}

                                    if (!string.IsNullOrEmpty(CLM.ExpectedDate))
                                    {
                                        //var ExpectedDate = Convert.ToString(CLM.ExpectedDate);
                                        GetLeads.ExpectedDate = DateTime.ParseExact(CLM.ExpectedDate, CLM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", CLM.ExpectedDate));
                                    }
                                    GetLeads.ExpectedProductAmount = CLM.ExpectedProductAmount;

                                    GetLeads.DateofBirth = CLM.DateofBirth;
                                    GetLeads.MarriageAnniversary = CLM.MarriageAnniversary;
                                    GetLeads.ExtraCol1 = CLM.ExtraCol1;
                                    GetLeads.ExtraCol2 = CLM.ExtraCol2;
                                    GetLeads.ExtraCol3 = CLM.ExtraCol3;
                                    GetLeads.ExtraCol4 = CLM.ExtraCol4;
                                    GetLeads.ExtraCol5 = CLM.ExtraCol5;
                                    GetLeads.ExtraCol6 = CLM.ExtraCol6;
                                    GetLeads.ExtraCol7 = CLM.ExtraCol7;
                                    GetLeads.ExtraCol8 = CLM.ExtraCol8;

                                    if (!string.IsNullOrEmpty(CLM.ExtraCol9))
                                    {
                                        GetLeads.ExtraCol9 = DateTime.ParseExact(CLM.ExtraCol9, CLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol9));
                                    }
                                    else
                                    {
                                        GetLeads.ExtraCol9 = null;
                                    }
                                    if (!string.IsNullOrEmpty(CLM.ExtraCol10))
                                    {
                                        GetLeads.ExtraCol10 = DateTime.ParseExact(CLM.ExtraCol10, CLM.DateFormat, CultureInfo.InvariantCulture); // Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol10));
                                    }
                                    else
                                    {
                                        GetLeads.ExtraCol10 = null;
                                    }
                                    GetLeads.ExtraCol11 = CLM.ExtraCol11;
                                    GetLeads.ExtraCol12 = CLM.ExtraCol12;
                                    GetLeads.ExtraCol13 = CLM.ExtraCol13;
                                    GetLeads.ExtraCol14 = CLM.ExtraCol14;
                                    GetLeads.ExtraCol15 = CLM.ExtraCol15;
                                    GetLeads.ExtraCol16 = CLM.ExtraCol16;
                                    GetLeads.ExtraCol17 = CLM.ExtraCol17;

                                    if (!string.IsNullOrEmpty(CLM.ExtraCol18))
                                    {
                                        GetLeads.ExtraCol18 = DateTime.ParseExact(CLM.ExtraCol18, CLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol18));
                                    }
                                    else
                                    {
                                        GetLeads.ExtraCol18 = null;
                                    }
                                    if (!string.IsNullOrEmpty(CLM.ExtraCol19))
                                    {
                                        GetLeads.ExtraCol19 = DateTime.ParseExact(CLM.ExtraCol19, CLM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol19));
                                    }
                                    else
                                    {
                                        GetLeads.ExtraCol19 = null;
                                    }
                                    if (!string.IsNullOrEmpty(CLM.ExtraCol20))
                                    {
                                        GetLeads.ExtraCol20 = DateTime.ParseExact(CLM.ExtraCol20, CLM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol20));
                                    }
                                    else
                                    {
                                        GetLeads.ExtraCol20 = null;
                                    }
                                    GetLeads.Extracol1dropdownId1 = CLM.dropddownItemId1;
                                    var exdp1 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId1).FirstOrDefault();
                                    if (exdp1 != null)
                                    {
                                        GetLeads.Dp1name = exdp1.DropDownItemNamw;
                                    }
                                    GetLeads.Extracol1dropdownId2 = CLM.dropddownItemId2;
                                    var exdp2 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId2).FirstOrDefault();
                                    if (exdp2 != null)
                                    {
                                        GetLeads.Dp2name = exdp1.DropDownItemNamw;
                                    }
                                    GetLeads.Extracol1dropdownId3 = CLM.dropddownItemId3;
                                    var exdp3 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId3).FirstOrDefault();
                                    if (exdp3 != null)
                                    {
                                        GetLeads.Dp3name = exdp3.DropDownItemNamw;
                                    }
                                    GetLeads.Extracol1dropdownId4 = CLM.dropddownItemId4;
                                    var exdp4 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId4).FirstOrDefault();
                                    if (exdp4 != null)
                                    {
                                        GetLeads.Dp4name = exdp4.DropDownItemNamw;
                                    }
                                    GetLeads.Extracol1dropdownId5 = CLM.dropddownItemId5;
                                    var exdp5 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId5).FirstOrDefault();
                                    if (exdp5 != null)
                                    {
                                        GetLeads.Dp5name = exdp5.DropDownItemNamw;
                                    }
                                    GetLeads.PastPerformance = CLM.PastPerformance;
                                    GetLeads.ProfitLoss = CLM.ProfitLoss;
                                    GetLeads.BuySell = CLM.BuySell;
                                    GetLeads.StockName = CLM.StockName;
                                    GetLeads.Price = CLM.Price;
                                    GetLeads.Target = CLM.Target;
                                    GetLeads.Target2 = CLM.Target2;
                                    GetLeads.Target3 = CLM.Target3;
                                    GetLeads.SI = CLM.SI;
                                    GetLeads.Remark = CLM.Remark;
                                    if (!string.IsNullOrEmpty(CLM.LeadName))
                                    {

                                        crm_leadstatus_tbl lst = new crm_leadstatus_tbl();
                                        lst.LeadStatusName = CLM.LeadName;
                                        lst.ColorHexValue = null;
                                        lst.BranchID = BranchID;
                                        lst.CompanyID = CompanyID;
                                        lst.Status = true;
                                        lst.created_at = Constant.GetBharatTime();
                                        db.crm_leadstatus_tbl.Add(lst);
                                        db.SaveChanges();

                                        GetLeads.LeadStatus = CLM.LeadName;
                                        GetLeads.LeadStatusID = lst.Id;
                                    }
                                    else
                                    {
                                        GetLeads.LeadStatus = CLM.LeadStatus;
                                        GetLeads.LeadStatusID = CLM.LeadStatusID;
                                    }

                                    if (!string.IsNullOrEmpty(CLM.ProdTypeName))
                                    {
                                        crm_producttypetbl ptn = new crm_producttypetbl();
                                        ptn.ProductTypeName = CLM.ProdTypeName;
                                        ptn.BranchID = BranchID;
                                        ptn.CompanyID = CompanyID;
                                        ptn.Status = true;
                                        db.crm_producttypetbl.Add(ptn);
                                        db.SaveChanges();

                                        GetLeads.ProductTypeID = ptn.Id;
                                        GetLeads.ProductTypeName = CLM.ProdTypeName;
                                    }
                                    else
                                    {
                                        GetLeads.ProductTypeID = CLM.ProductTypeID;
                                        GetLeads.ProductTypeName = CLM.ProductTypeName;
                                    }

                                    if (!string.IsNullOrEmpty(CLM.LdSourceName))
                                    {
                                        crm_leadsource_tbl ls = new crm_leadsource_tbl();
                                        ls.LeadsourceName = CLM.LdSourceName;
                                        ls.BranchID = BranchID;
                                        ls.CompanyID = CompanyID;
                                        ls.Status = true;
                                        ls.Created_at = System.DateTime.Now;
                                        db.crm_leadsource_tbl.Add(ls);
                                        db.SaveChanges();

                                        GetLeads.LeadSourceID = ls.Id;
                                        GetLeads.LeadResource = CLM.LdSourceName;
                                    }
                                    else
                                    {
                                        GetLeads.LeadSourceID = CLM.LeadSourceID;
                                        GetLeads.LeadResource = CLM.LeadSource;
                                    }

                                    #endregion

                                    #region Add-Lead-Description
                                    //File Upload with lead
                                    string FileName = string.Empty;
                                    string FileFullName = string.Empty;
                                    if (file != null)
                                    {
                                        if (file.ContentLength > 0)
                                        {
                                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                                            var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                                            if (supportedTypes.Contains(fileExt))
                                            {
                                                string extension = Path.GetExtension(file.FileName);
                                                FileName = "Lead-" + Convert.ToString(Session["UserName"]).Trim() + "-" + CLM.Customer.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                                FileFullName = FileName + extension;
                                                string _path = Server.MapPath("~/LeadAttachment/" + FileName + extension);
                                                file.SaveAs(_path);
                                            }
                                            else
                                            {
                                                TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                                //return Redirect("/home/createleads");
                                                return View(CLM);
                                            }
                                        }
                                    }


                                    if (!string.IsNullOrEmpty(CLM.Description))
                                    {
                                        crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                                        LD.LeadId = Lid;
                                        LD.Date = localTime.ToString("dd/MM/yyyy");
                                        LD.Description = CLM.Description;
                                        LD.ByUID = Convert.ToInt32(Session["UID"]);
                                        LD.ByUserName = Convert.ToString(Session["UserName"]);
                                        LD.CreatedDateTime = localTime;
                                        LD.BranchID = Convert.ToInt32(Session["BranchID"]);
                                        LD.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                                        LD.LeadAttachment = FileFullName;
                                        LD.LeadStatusName = CLM.LeadStatus;
                                        db.crm_leaddescriptiontbl.Add(LD);
                                    }

                                    #endregion

                                    db.SaveChanges();
                                    trans.Commit(); //after data saved then commit transaction
                                    TempData["success"] = "Leads updated successfully";
                                }

                                else
                                {
                                    TempData["alert"] = "Please add a description before Update";
                                    return View(CLM);
                                }
                            }
                            #endregion
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(CLM.MobileNo))
                            {
                                if (CLM.MobileNo.Length < 9)//check mobile no length is less then 9 then it add 0 
                                {
                                    CLM.MobileNo = "0" + CLM.MobileNo;
                                }

                                string MobileNumber = CLM.MobileNo.Substring(CLM.MobileNo.Length - 9, 9);//get last line digits 
                                if (db.crm_createleadstbl.Any(x => (!string.IsNullOrEmpty(x.MobileNo) && x.MobileNo.Substring(x.MobileNo.Length - 9, 9) == MobileNumber) && x.BranchID == LoggedBranchID && x.CompanyID == LoggedCompanyID))
                                {
                                    //TempData["numberexists"] = "Mobile Number Already exists.";
                                    TempData["alert"] = "Mobile Number Already exists.";
                                    return View(CLM);
                                }
                            }
                            else
                            {
                                TempData["alert"] = "Mobile number is empty.";
                                return View(CLM);
                            }

                            if (!string.IsNullOrEmpty(CLM.EmailId) && db.crm_createleadstbl.Any(x => x.EmailId == CLM.EmailId && x.BranchID == LoggedBranchID && x.CompanyID == LoggedCompanyID))
                            {
                                TempData["alert"] = "Email id already exists.";
                                return View(CLM);
                            }
                            if (CompanyID == 2458)
                            {
                                if (CLM.ExtraCol9 != null)
                                {
                                    DateTime extracol9 = DateTime.ParseExact(CLM.ExtraCol9, CLM.DateFormat, CultureInfo.InvariantCulture);
                                    var checkdate = db.crm_createleadstbl.Where(x => x.ExtraCol9 == extracol9 && x.BranchID == LoggedBranchID && x.CompanyID == LoggedCompanyID && x.Dp1name == CLM.Extracol1dropdown).FirstOrDefault();
                                    if (checkdate != null)
                                    {
                                        TempData["alert"] = "This date already booked for venue!";
                                        return View(CLM);
                                    }
                                }

                            }
                            //else
                            //{
                            #region Create-Leads
                            crm_createleadstbl creatleadsTbl = new crm_createleadstbl();
                            creatleadsTbl.LeadOwner = Convert.ToInt32(Session["UID"]);
                            creatleadsTbl.Customer = CLM.Customer;
                            creatleadsTbl.Designation = CLM.Designation;
                            creatleadsTbl.MobileNo = CLM.MobileNo.Replace("+91", "").Trim();
                            creatleadsTbl.EmailId = CLM.EmailId;
                            creatleadsTbl.OrganizationName = CLM.OrganizationName;
                            creatleadsTbl.CountryID = CLM.CountryID;
                            creatleadsTbl.Country = CLM.Country;
                            creatleadsTbl.CityID = CLM.CityID;
                            creatleadsTbl.City = CLM.City;
                            creatleadsTbl.StateID = CLM.StateID;
                            creatleadsTbl.State = CLM.State;
                            creatleadsTbl.FollowDate = DateTime.ParseExact(CLM.FollowDate, CLM.DateFormat, CultureInfo.InvariantCulture);

                            if (!string.IsNullOrEmpty(CLM.TimeZoneName))
                            {
                                creatleadsTbl.ZoneName = Constant.GetCompanyTimeZone(CompanyID);
                            }
                            //else
                            //{
                            //    creatleadsTbl.ZoneName = CLM.TimeZoneName;
                            //}

                            if (!string.IsNullOrEmpty(CLM.FollowupTime))
                            {
                                creatleadsTbl.FollowUpTime = CLM.FollowupTime;
                            }
                            //else
                            //{
                            //creatleadsTbl.FollowUpTime = localTime.ToString("hh:mm tt");
                            //}

                            if (!string.IsNullOrEmpty(CLM.FollowupTimeInTZ))
                            {
                                creatleadsTbl.ConvertedFupDateTime = DateTime.ParseExact(CLM.FollowupTimeInTZ, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                            //else
                            //{
                            //creatleadsTbl.ConvertedFupDateTime = localTime;
                            //}
                            //creatleadsTbl.FollowupTimeinIST = CLM.FollowupTimeIST;

                            creatleadsTbl.Url = CLM.URL;
                            creatleadsTbl.SkypeId = CLM.SkypeId;
                            creatleadsTbl.Description = CLM.Description;
                            creatleadsTbl.Status = true;
                            creatleadsTbl.LeadsType = "Manual";
                            creatleadsTbl.Address = CLM.Address;
                            creatleadsTbl.Createddate = localTime;
                            creatleadsTbl.IsLeadReminder = true;
                            if (!string.IsNullOrWhiteSpace(CLM.DateofBirth))
                            {
                                creatleadsTbl.DateofBirth = Convert.ToString(CLM.DateofBirth);
                            }
                            if (!string.IsNullOrWhiteSpace(CLM.MarriageAnniversary))
                            {
                                creatleadsTbl.MarriageAnniversary = Convert.ToString(CLM.MarriageAnniversary);
                            }
                            creatleadsTbl.date = localTime.ToString("dd/MM/yyyy");
                            creatleadsTbl.BranchID = Convert.ToInt32(Session["BranchID"]);
                            creatleadsTbl.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                            if (!string.IsNullOrEmpty(CLM.ExpectedDate))
                            {
                                creatleadsTbl.ExpectedDate = DateTime.ParseExact(CLM.ExpectedDate, CLM.DateFormat, CultureInfo.InvariantCulture);
                                //Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", CLM.ExpectedDate));                                    
                            }
                            creatleadsTbl.ExpectedProductAmount = CLM.ExpectedProductAmount;
                            if (CLM.ExtraCol1 != null)
                            {
                                creatleadsTbl.ExtraCol1 = CLM.ExtraCol1;
                            }
                            //creatleadsTbl.ExtraCol1 = CLM.ExtraCol1;
                            creatleadsTbl.ExtraCol2 = CLM.ExtraCol2;
                            creatleadsTbl.ExtraCol3 = CLM.ExtraCol3;
                            creatleadsTbl.ExtraCol4 = CLM.ExtraCol4;
                            creatleadsTbl.ExtraCol5 = CLM.ExtraCol5;
                            creatleadsTbl.ExtraCol6 = CLM.ExtraCol6;
                            creatleadsTbl.ExtraCol7 = CLM.ExtraCol7;
                            creatleadsTbl.ExtraCol8 = CLM.ExtraCol8;
                            if (!string.IsNullOrEmpty(CLM.ExtraCol9))
                            {
                                creatleadsTbl.ExtraCol9 = DateTime.ParseExact(CLM.ExtraCol9, CLM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol9));
                            }
                            if (!string.IsNullOrEmpty(CLM.ExtraCol10))
                            {
                                creatleadsTbl.ExtraCol10 = DateTime.ParseExact(CLM.ExtraCol10, CLM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol10));
                            }
                            if (CLM.ExtraCol11 > 0)
                            { creatleadsTbl.ExtraCol11 = CLM.ExtraCol11; }

                            if (CLM.ExtraCol12 > 0)
                            { creatleadsTbl.ExtraCol12 = CLM.ExtraCol12; }
                            if (CLM.ExtraCol13 > 0)
                            { creatleadsTbl.ExtraCol13 = CLM.ExtraCol13; }
                            if (CLM.ExtraCol14 > 0)
                            { creatleadsTbl.ExtraCol14 = CLM.ExtraCol14; }
                            if (CLM.ExtraCol15 > 0)
                            { creatleadsTbl.ExtraCol15 = CLM.ExtraCol15; }
                            if (CLM.ExtraCol16 > 0 && CLM.ExtraCol16 != null)
                            { creatleadsTbl.ExtraCol16 = CLM.ExtraCol16; }
                            if (CLM.ExtraCol17 > 0)
                            { creatleadsTbl.ExtraCol17 = CLM.ExtraCol17; }
                            if (!string.IsNullOrEmpty(CLM.ExtraCol18))
                            {
                                creatleadsTbl.ExtraCol18 = DateTime.ParseExact(CLM.ExtraCol18, CLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol18));
                            }
                            if (!string.IsNullOrEmpty(CLM.ExtraCol19))
                            {
                                creatleadsTbl.ExtraCol19 = DateTime.ParseExact(CLM.ExtraCol19, CLM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol19));
                            }
                            if (!string.IsNullOrEmpty(CLM.ExtraCol20))
                            {
                                creatleadsTbl.ExtraCol20 = DateTime.ParseExact(CLM.ExtraCol20, CLM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(String.Format("{0:dd-MM-yyyy}", CLM.ExtraCol20));
                            }

                            creatleadsTbl.Extracol1dropdownId1 = CLM.dropddownItemId1;
                            var exdp1 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId1).FirstOrDefault();
                            if (exdp1 != null)
                            {
                                creatleadsTbl.Dp1name = exdp1.DropDownItemNamw;
                            }
                            creatleadsTbl.Extracol1dropdownId2 = CLM.dropddownItemId2;
                            var exdp2 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId2).FirstOrDefault();
                            if (exdp2 != null)
                            {
                                creatleadsTbl.Dp2name = exdp2.DropDownItemNamw;
                            }
                            creatleadsTbl.Extracol1dropdownId3 = CLM.dropddownItemId3;
                            var exdp3 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId3).FirstOrDefault();
                            if (exdp3 != null)
                            {
                                creatleadsTbl.Dp3name = exdp3.DropDownItemNamw;
                            }
                            creatleadsTbl.Extracol1dropdownId4 = CLM.dropddownItemId4;
                            var exdp4 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId4).FirstOrDefault();
                            if (exdp4 != null)
                            {
                                creatleadsTbl.Dp4name = exdp4.DropDownItemNamw;
                            }
                            creatleadsTbl.Extracol1dropdownId5 = CLM.dropddownItemId5;
                            var exdp5 = db.crm_leaddropdownlist_tbl.Where(em => em.dropddownItemId == CLM.dropddownItemId5).FirstOrDefault();
                            if (exdp5 != null)
                            {
                                creatleadsTbl.Dp5name = exdp5.DropDownItemNamw;
                            }
                            creatleadsTbl.PastPerformance = CLM.PastPerformance;
                            creatleadsTbl.ProfitLoss = CLM.ProfitLoss;
                            creatleadsTbl.BuySell = CLM.BuySell;
                            creatleadsTbl.StockName = CLM.StockName;
                            creatleadsTbl.Price = CLM.Price;
                            creatleadsTbl.Target = CLM.Target;
                            creatleadsTbl.Target2 = CLM.Target2;
                            creatleadsTbl.Target3 = CLM.Target3;
                            creatleadsTbl.SI = CLM.SI;
                            creatleadsTbl.Remark = CLM.Remark;
                            db.crm_createleadstbl.Add(creatleadsTbl);
                            db.SaveChanges();


                            if (!string.IsNullOrEmpty(CLM.LeadName))
                            {

                                crm_leadstatus_tbl lst = new crm_leadstatus_tbl();
                                lst.LeadStatusName = CLM.LeadName;
                                lst.ColorHexValue = null;
                                lst.BranchID = BranchID;
                                lst.CompanyID = CompanyID;
                                lst.Status = true;
                                lst.created_at = Constant.GetBharatTime();
                                db.crm_leadstatus_tbl.Add(lst);
                                db.SaveChanges();

                                creatleadsTbl.LeadStatus = CLM.LeadName;
                                creatleadsTbl.LeadStatusID = lst.Id;
                            }
                            else
                            {
                                creatleadsTbl.LeadStatus = CLM.LeadStatus;
                                creatleadsTbl.LeadStatusID = CLM.LeadStatusID;
                            }

                            if (!string.IsNullOrEmpty(CLM.ProdTypeName))
                            {
                                crm_producttypetbl ptn = new crm_producttypetbl();
                                ptn.ProductTypeName = CLM.ProdTypeName;
                                ptn.BranchID = BranchID;
                                ptn.CompanyID = CompanyID;
                                ptn.Status = true;
                                db.crm_producttypetbl.Add(ptn);
                                db.SaveChanges();

                                creatleadsTbl.ProductTypeID = ptn.Id;
                                creatleadsTbl.ProductTypeName = CLM.ProdTypeName;
                            }
                            else
                            {
                                creatleadsTbl.ProductTypeID = CLM.ProductTypeID;
                                creatleadsTbl.ProductTypeName = CLM.ProductTypeName;
                            }

                            if (!string.IsNullOrEmpty(CLM.LdSourceName))
                            {
                                crm_leadsource_tbl ls = new crm_leadsource_tbl();
                                ls.LeadsourceName = CLM.LdSourceName;
                                ls.BranchID = BranchID;
                                ls.CompanyID = CompanyID;
                                ls.Status = true;
                                ls.Created_at = System.DateTime.Now;
                                db.crm_leadsource_tbl.Add(ls);
                                db.SaveChanges();

                                creatleadsTbl.LeadSourceID = ls.Id;
                                creatleadsTbl.LeadResource = CLM.LdSourceName;
                            }
                            else
                            {
                                creatleadsTbl.LeadSourceID = CLM.LeadSourceID;
                                creatleadsTbl.LeadResource = CLM.LeadSource;
                            }
                            //creatleadsTbl.LeadStatusID = CLM.LeadStatusID;
                            //creatleadsTbl.LeadStatus = CLM.LeadStatus;
                            //creatleadsTbl.LeadSourceID = CLM.LeadSourceID;
                            //creatleadsTbl.LeadResource = CLM.LeadSource;
                            //creatleadsTbl.ProductTypeID = CLM.ProductTypeID;
                            //creatleadsTbl.ProductTypeName = CLM.ProductTypeName;
                            //if (db.SaveChanges() > 0)
                            //{
                            #region Add-Lead-Description
                            //File Upload with lead
                            string FileName = string.Empty;
                            string FileFullName = string.Empty;
                            if (file != null)
                            {
                                if (file.ContentLength > 0)
                                {
                                    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                                    var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                                    if (supportedTypes.Contains(fileExt))
                                    {
                                        string extension = Path.GetExtension(file.FileName);
                                        FileName = "Lead-" + Convert.ToString(Session["UserName"]).Trim() + "-" + CLM.Customer.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                        FileFullName = FileName + extension;
                                        string _path = Server.MapPath("~/LeadAttachment/" + FileName + extension);
                                        file.SaveAs(_path);
                                    }
                                    else
                                    {
                                        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                        //return Redirect("/home/createleads");
                                        return View(CLM);
                                    }
                                }
                            }

                            var lid = creatleadsTbl.Id;
                            CRMCustomerID = Convert.ToInt32(creatleadsTbl.Id);
                            crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                            LD.LeadId = lid;
                            LD.Date = localTime.ToString("dd/MM/yyyy");
                            LD.Description = CLM.Description;
                            LD.ByUID = Convert.ToInt32(Session["UID"]);
                            LD.ByUserName = Convert.ToString(Session["UserName"]);
                            LD.CreatedDateTime = localTime;
                            LD.BranchID = Convert.ToInt32(Session["BranchID"]);
                            LD.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                            LD.LeadAttachment = FileFullName;
                            LD.LeadStatusName = CLM.LeadStatus;
                            db.crm_leaddescriptiontbl.Add(LD);
                            #endregion


                            db.SaveChanges();

                            #region customer add in customers table

                            int LastSyncID = 0;
                            DataTable dtLastSync = DataAccessLayer.GetDataTable("call CRM_CustomerLastSyncID('" + LoggedCompanyID + "'," + LoggedBranchID + ")");
                            if (Convert.ToInt32(dtLastSync.Rows[0]["syncid"] == DBNull.Value ? 0 : dtLastSync.Rows[0]["syncid"]) > 0)
                            {
                                LastSyncID = Convert.ToInt32(dtLastSync.Rows[0]["syncid"]) + 1;
                            }
                            else
                            {
                                LastSyncID = 1;
                            }
                            //try
                            //{
                            string sCompanyID = Convert.ToString(Session["CompanyID"]);
                            string sMObile = CLM.MobileNo.Replace("+91", "");

                            #region get total record count from customer table
                            string query = @"select count(*) as rowCount from Customers where CompanyId = " + LoggedCompanyID + "";

                            int rowCount = db.Database.SqlQuery<int>(query).FirstOrDefault();
                            string customerId = "CUSCRM-" + (rowCount + 1);
                            #endregion

                            //var checkCustomer = db.customers.Where(em => em.CompanyID == sCompanyID && em.BranchCode == LoggedBranchID && em.MobileNo == sMObile).FirstOrDefault();
                            //if (checkCustomer == null)
                            //{
                            //    customer c = new customer();
                            //    //c.CustomerID = (Convert.ToString(Session["CompanyID"]));
                            //    c.CustomerID = customerId;
                            //    c.CompanyID = Convert.ToString(Session["CompanyID"]);
                            //    c.BranchCode = Convert.ToInt32(Session["BranchID"]);
                            //    c.CrmCustomerID = CRMCustomerID;
                            //    c.CustomerName = CLM.Customer;
                            //    c.BillingAddress = CLM.Address;
                            //    c.DeliveryAddress = CLM.Address;
                            //    c.Country = CLM.CountryID;
                            //    c.State = CLM.StateID;
                            //    c.City = CLM.CityID;
                            //    c.MobileNo = CLM.MobileNo.Replace("+91", "");
                            //    c.EmailID = CLM.EmailId;
                            //    c.CreatedDate = localTime;
                            //    c.CreatedBy = Convert.ToString(Session["UserName"]);
                            //    c.flag = "N";
                            //    c.SyncID = "O" + LastSyncID;
                            //    db.customers.Add(c);
                            //    db.SaveChanges();
                            //}
                            //}
                            //catch (DbEntityValidationException e)
                            //{
                            //    trans.Rollback();                                    
                            //}
                            #endregion

                            trans.Commit();//after data saved success then commit the transaction

                            TempData["success"] = "Leads created successfully";
                            //}
                            #endregion
                        }
                    }
                    //}
                    else
                    {
                        return Redirect("/home/login");
                    }
                    if (string.IsNullOrEmpty(id))
                    {
                        return Redirect("/home/createleads");
                    }
                    else
                    {
                        string lid = id;
                        //string CurrentURL = Request.Url.AbsoluteUri;
                        //return Redirect(CurrentURL);
                        return RedirectToAction("createleads", new { id = lid });
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    TempData["alert"] = "Something went wrong!";
                    return View(CLM);
                }
            }


        }

        public ActionResult viewleads(int? ProductTypeID, int? LeadSourceID, int? LeadDropDown1, int? LeadDropDown2, int? LeadDropDown3, int? LeadDropDown4, int? LeadDropDown5, string FromDate, string ToDate, string UserddlName, string FilterType = "", string filterText = "", string Term = "", string Country = "", int page = 1, string pagetypeType = "")
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            try
            {
                Session["exportdatasmg"] = 0;
                int msgcheck = Convert.ToInt32(Session["exportdatasmg12"]);
                if (msgcheck == 1)
                {
                    Session["exportdatasmg"] = 1;
                    Session["exportdatasmg12"] = 0;

                }
                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);

                int LoggedBranchId = Convert.ToInt32(Session["BranchID"]);
                int LoggedCompanyId = Convert.ToInt32(Session["CompanyID"]);
                ViewBag.LoggedBranchId = LoggedBranchId;
                ViewBag.LoggedCompanyId = LoggedCompanyId;
                VLM.columnVal = cr.GetViewLeadsetting();

                # region after assigned lead to user redirect page  check sessions value not null 
                if (string.IsNullOrEmpty(UserddlName) && TempData["UserddlName"] != null)
                {
                    UserddlName = Convert.ToString(TempData["UserddlName"]);
                }
                if (string.IsNullOrEmpty(filterText) && TempData["filterText"] != null)
                {
                    filterText = Convert.ToString(TempData["filterText"]);
                }
                if (string.IsNullOrEmpty(FilterType) && TempData["VLFltrFilterType"] != null)
                {
                    FilterType = Convert.ToString(TempData["VLFltrFilterType"]);
                }
                if (string.IsNullOrEmpty(Term) && TempData["VLFilterTerm"] != null)
                {
                    Term = Convert.ToString(TempData["VLFilterTerm"]);
                }
                if (string.IsNullOrEmpty(FromDate) && TempData["VLFltrFrmDt"] != null)
                {
                    FromDate = Convert.ToString(TempData["VLFltrFrmDt"]);
                }
                if (string.IsNullOrEmpty(ToDate) && TempData["VLFltrToDt"] != null)
                {
                    ToDate = Convert.ToString(TempData["VLFltrToDt"]);
                }
                if (LeadSourceID != null && TempData["VLFltrLeadSourceID"] != null)
                {
                    LeadSourceID = Convert.ToInt32(TempData["VLFltrLeadSourceID"]);
                }
                if (ProductTypeID != null && TempData["VLFltrProductTypeID"] != null)
                {
                    ProductTypeID = Convert.ToInt32(TempData["VLFltrProductTypeID"]);
                }
                //if (CompanyID != 2066)
                //{
                if (page != 1 && TempData["cpage"] != null)
                {
                    page = Convert.ToInt32(TempData["cpage"]);
                }
                //}
                //else
                //{
                //    if (Page == null)
                //    {
                //        PreNext = "Next";
                //        Page = 1;
                //    }
                //}

                #endregion

                //if (!string.IsNullOrEmpty(FilterType) && FilterType != "Select Date")
                //{
                Session["VLFltrFilterType"] = (!string.IsNullOrEmpty(FilterType) && FilterType != "Select Date") ? FilterType : null;
                //}
                //else { Session["VLFltrFilterType"] = null; }

                //if (!string.IsNullOrEmpty(filterText))
                //{
                Session["filterText"] = !string.IsNullOrEmpty(filterText) ? filterText : null;
                //}
                //else
                //{
                //    Session["filterText"] = null;
                //}

                int UserID = Convert.ToInt32(Session["UID"]);
                ViewBag.LoggedUserId = UserID;
                var UID = 0;
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    UID = 0;
                    if (!string.IsNullOrEmpty(UserddlName) && UserddlName != "0")
                    {
                        UID = Convert.ToInt32(UserddlName);
                    }
                    VLM.UserddlName = Convert.ToString(UID);
                }
                else
                {
                    if (!string.IsNullOrEmpty(UserddlName) /*&& UserddlName != "0"*/ )
                    {
                        UID = Convert.ToInt32(UserddlName);
                    }
                    else
                    {
                        UID = Convert.ToInt32(Session["UID"]);
                    }
                    VLM.UserddlName = Convert.ToString(UID);
                }

                Session["UserddlName"] = VLM.UserddlName;

                if (!string.IsNullOrEmpty(UserddlName) && UserddlName != "0")
                {
                    var data = cr.GetUserCompanyBranch(UID);
                    if (data != null)
                    {
                        BranchID = data.BranchID;
                        CompanyID = data.CompanyID;
                    }
                }

                #region Country List
                if (LoggedCompanyId == 296)
                {
                    string countryQry = @"select id as CountryID,country_name as CountryName,country_code from acc_countries";
                    var countryList = db.Database.SqlQuery<ManageCountryModel>(countryQry).OrderBy(a => a.CountryName).ToList();
                    VLM.CountryList = new SelectList(countryList, "CountryName", "CountryName", Country);
                    VLM.Country = Country;
                }
                if (LoggedCompanyId == 2183)
                {
                    string countryQry = @"select id as CountryID,country_name as CountryName,country_code from acc_countries";
                    var countryList = db.Database.SqlQuery<ManageCountryModel>(countryQry).OrderBy(a => a.CountryName).ToList();
                    VLM.CountryList = new SelectList(countryList, "CountryName", "CountryName", Country);
                    VLM.Country = Country;
                }

                #endregion

                #region Email Template
                //var getEmailTemplate = db.crm_emailtemplate.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId).OrderBy(a => a.EmailTemplateName).ToList();
                //if (getEmailTemplate != null && getEmailTemplate.Count > 0)
                //{
                //    //List<ViewLeadsModel> oETM = new List<ViewLeadsModel>();
                //    var oETM = (from item in getEmailTemplate
                //                select new ViewLeadsModel
                //                {
                //                    EmailTemplateID = item.EmailTemplateID,
                //                    EmailTemplateName = item.EmailTemplateName
                //                }).ToList();
                //    //foreach (var item in getEmailTemplate)
                //    //{
                //    //    VLM.oEmailTemplateModelList.Add(new ViewLeadsModel { EmailTemplateID = item.EmailTemplateID, EmailTemplateName = item.EmailTemplateName });
                //    //    //ViewLeadsModel EMT = new ViewLeadsModel();
                //    //    //EMT.EmailTemplateID = Convert.ToInt32(item.EmailTemplateID);
                //    //    //EMT.EmailTemplateName = Convert.ToString(item.EmailTemplateName);
                //    //    //oETM.Add(EMT);
                //    //}
                //    VLM.oEmailTemplateModelList = oETM;
                //}
                #endregion

                #region View Total Lead Count /Assign User List/ Lead Status List/File Manager List

                VLM.AssignToList = new List<CreateUserModel>();
                //var AssignList = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName.ToLower().Contains("sales") && em.Status == true).OrderBy(em => em.Fname).ToList();
                string assignquery = @"select ur.Id as UserID,  CONCAT(ur.FName ,' ', ur.Lname, '(' , ur.EmployeeCode,')') AS UserName  
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + LoggedBranchId + " and ur.CompanyID = " + LoggedCompanyId + " and ur.Status = 1 and rl.ViewLeads = 1";
                var AssignList = db.Database.SqlQuery<CreateUserModel>(assignquery).ToList();
                if (AssignList != null && AssignList.Count() > 0)
                {
                    //List<CreateUserModel> assignToList = new List<CreateUserModel>();
                    //foreach (var item in AssignList)
                    //{
                    //    CreateUserModel CRM = new CreateUserModel();
                    //    CRM.UserID = item.Id;

                    //    CRM.UserName = item.Fname + ' ' + item.Lname;
                    //    assignToList.Add(CRM);
                    //}
                    VLM.AssignToList = AssignList.Where(em => em.UserID != UID).OrderBy(a => a.UserName).ToList();
                }

                #region Smartcapita admin add to assign user list
                if (LoggedCompanyId == 296)
                {
                    VLM.AssignToList.Add(new CreateUserModel { UserID = 50, UserName = "Smart Capita Admin" });
                }
                #endregion

                #region Get other branch user list to add assignTo list

                DataTable GetAssignToRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + LoggedBranchId + "','','')");
                var GetOtherBranchUser = (from dr in GetAssignToRecords.AsEnumerable()
                                          select new CreateUserModel()
                                          {
                                              UserID = Convert.ToInt32(dr["AssignedUserID"]),
                                              UserName = Convert.ToString(dr["UserName"] + " (" + dr["EmployeeCode"] + ")" + " (Branch: " + dr["BranchName"] + ")"),
                                              IsActive = Convert.ToBoolean(dr["IsActive"]),
                                              AssignToCompanyID = Convert.ToString(dr["AssignToCompanyID"])

                                          }).Where(a => a.IsActive == true).OrderBy(a => a.UserName).ToList();
                if (GetOtherBranchUser.Count > 0)
                {
                    VLM.AssignToList.AddRange(GetOtherBranchUser);

                    VLM.AssignToList = VLM.AssignToList.OrderBy(a => a.UserName).ToList();
                }
                #endregion

                #region Lead Status
                string leadStatusQry = @"select Id,LeadStatusName from crm_leadstatus_tbl where BranchID='" + LoggedBranchId + "' and CompanyID='" + LoggedCompanyId + "'and Status=1";
                VLM.leadstatusList = db.Database.SqlQuery<LeadStatusModel>(leadStatusQry).OrderBy(a => a.LeadStatusname).ToList();

                //if (VLM.leadstatusList!=null && VLM.leadstatusList.Count>0)
                //{
                //    VLM.leadstatusList.Add(new LeadStatusModel { Id = 0, LeadStatusname = "Archive" });
                //    VLM.leadstatusList=VLM.leadstatusList.OrderBy(a => a.LeadStatusname).ToList();
                //}
                //else
                //{
                //    VLM.leadstatusList = new List<LeadStatusModel>();
                //    VLM.leadstatusList.Add(new LeadStatusModel { Id = 0, LeadStatusname = "Archive" });
                //}

                #endregion
                #region Drop down Source
                string leaddropdownsource = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol1dropdown'";
                var LeadDropdown1list = db.Database.SqlQuery<Leaddropdownmodel1>(leaddropdownsource).OrderBy(a => a.dropddownItemId).ToList();
                VLM.LeadDropdown1 = new SelectList(LeadDropdown1list, "dropddownItemId", "DropDownItemNamw", LeadDropDown1);

                string leaddropdownsource2 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol2dropdown'";
                var LeadDropdown2list = db.Database.SqlQuery<Leaddropdownmodel2>(leaddropdownsource2).OrderBy(a => a.dropddownItemId).ToList();
                VLM.LeadDropdown2 = new SelectList(LeadDropdown2list, "dropddownItemId", "DropDownItemNamw", LeadDropDown2);

                string leaddropdownsource3 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol3dropdown'";
                var LeadDropdown3list = db.Database.SqlQuery<Leaddropdownmodel3>(leaddropdownsource3).OrderBy(a => a.dropddownItemId).ToList();
                VLM.LeadDropdown3 = new SelectList(LeadDropdown3list, "dropddownItemId", "DropDownItemNamw", LeadDropDown3);

                string leaddropdownsource4 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol4dropdown'";
                var LeadDropdown4list = db.Database.SqlQuery<Leaddropdownmodel4>(leaddropdownsource4).OrderBy(a => a.dropddownItemId).ToList();
                VLM.LeadDropdown4 = new SelectList(LeadDropdown4list, "dropddownItemId", "DropDownItemNamw", LeadDropDown4);

                string leaddropdownsource5 = @"select dropddownItemId,DropDownItemNamw from crm_Leaddropdownlist_tbl Where Status=0 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' and DropDownField='ExtraCol5dropdown'";
                var LeadDropdown5list = db.Database.SqlQuery<Leaddropdownmodel5>(leaddropdownsource5).OrderBy(a => a.dropddownItemId).ToList();
                VLM.LeadDropdown5 = new SelectList(LeadDropdown5list, "dropddownItemId", "DropDownItemNamw", LeadDropDown5);
                #endregion
                #region Lead Source
                string leadSourceQuery = @"select Id,LeadsourceName from crm_leadsource_tbl Where Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "'";
                var leadsourceList = db.Database.SqlQuery<LeadSourceModel>(leadSourceQuery).OrderBy(a => a.LeadsourceName).ToList();
                VLM.LeadSource = new SelectList(leadsourceList, "Id", "LeadsourceName", LeadSourceID);
                #endregion

                #region Product Type
                string pTypeQry = @" select Id,ProductTypeName from crm_producttypetbl Where Status=1 and BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'";
                var producttypetblList = db.Database.SqlQuery<ProductTypeModel>(pTypeQry).OrderBy(a => a.ProductTypeName).ToList();
                VLM.ProductType = new SelectList(producttypetblList, "Id", "ProductTypeName", ProductTypeID);

                #endregion


                var getFileManager = db.crm_filemanager.Where(em => em.CreatedBy == UserID && em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.IsDeleted == false && em.IsActive == true).OrderBy(a => a.FileName).ToList();
                if (getFileManager != null && getFileManager.Count > 0)
                {
                    //List<FileManger> fmList = new List<FileManger>();
                    var fmList = (from item in getFileManager
                                  select new FileManger
                                  {
                                      FileID = item.FileID,
                                      FileName = item.FileName,
                                      FileUpload = item.FileUpload
                                  }).ToList();
                    //foreach (var item in getFileManager)
                    //{
                    //    FileManger fm = new FileManger();
                    //    fm.FileID = item.FileID;
                    //    fm.FileName = item.FileName;
                    //    fm.FileUpload = item.FileUpload;
                    //    fmList.Add(fm);
                    //}
                    VLM.oFileMangerList = fmList;
                }
                #endregion

                #region Get all User and mapped users

                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    #region Admin View All Users
                    string userquery = @"select ur.Id as UserID,  CONCAT(ur.FName ,' ', ur.Lname, '(' , ur.EmployeeCode,')') AS UserName 
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + LoggedBranchId + " and ur.CompanyID = " + LoggedCompanyId + " and ur.Status = 1 and rl.ViewLeads = 1";
                    var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                    // var q = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName == "Sales" && em.Status == true).OrderBy(em => em.Fname).ToList();
                    VLM.Userddllist = new List<Userddl>();

                    //Userddl u = new Userddl();
                    if (q != null && q.Count > 0)
                    {
                        VLM.Userddllist.Add(new Userddl { uid = 0, UserName = "All" });
                        VLM.Userddllist.Add(new Userddl { uid = UserID, UserName = "Myself" });
                        foreach (var item in q)
                        {
                            var user = new Userddl
                            {
                                uid = Convert.ToInt32(item.UserID),
                                // UserName = item.Fname + " " + item.Lname
                                UserName = item.UserName,
                            };
                            VLM.Userddllist.Add(user);
                        }
                    }
                    else
                    {
                        VLM.Userddllist.Add(new Userddl { uid = UserID, UserName = "Myself" });
                    }

                    #endregion

                    #region Get other branch user list to add mapped user list

                    DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + LoggedBranchId + "','','')");
                    if (GetRecords.Rows.Count > 0)
                    {
                        var GetOtherBranchUserList = (from dr in GetRecords.AsEnumerable()
                                                      select new Userddl()
                                                      {
                                                          uid = Convert.ToInt32(dr["AssignedUserID"]),
                                                          UserName = Convert.ToString(dr["UserName"] + " (" + dr["EmployeeCode"] + ")" + " (Branch: " + dr["BranchName"] + ")"),
                                                          IsActive = Convert.ToBoolean(dr["IsActive"])
                                                      }).Where(a => a.IsActive == true).OrderBy(a => a.UserName).ToList();
                        VLM.Userddllist.AddRange(GetOtherBranchUserList);

                        VLM.OtherBranchMappedUser = GetOtherBranchUserList;

                        VLM.Userddllist = VLM.Userddllist.OrderBy(a => a.UserName).ToList();

                    }

                    #endregion
                }
                else
                {
                    #region Employee will view only mapped user
                    var GetUserData = db.crm_usertbl.Where(em => em.Id == UserID && em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId).FirstOrDefault();
                    if (GetUserData != null && !string.IsNullOrEmpty(GetUserData.MappedUsers))
                    {
                        VLM.MappedUser = GetUserData.MappedUsers.ToString();
                        var GetMapUser = GetUserData.MappedUsers.Split(',');//seperate mapped user id
                        VLM.Userddllist = new List<Userddl>();

                        if (GetMapUser != null && GetMapUser.Count() > 0)
                        {

                            VLM.Userddllist.Add(new Userddl { uid = 0, UserName = "All" });//add all if mapped user not null
                            VLM.Userddllist.Add(new Userddl { uid = UserID, UserName = "Myself" });//add logged user into list

                            int[] MappedUserIds = GetMapUser.Select(int.Parse).ToArray();//convert user ids string to int
                            int LoggedUID = Convert.ToInt32(Session["UID"]);
                            if (LoggedUID != 2155)
                            {
                                var GetAllMappedUser = (from userIds in MappedUserIds
                                                        join em in db.crm_usertbl on userIds equals em.Id
                                                        where em.Status == true && em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId
                                                        select new Userddl
                                                        {
                                                            uid = em.Id,
                                                            UserName = string.Format("{0} {1}", em.Fname, em.Lname)
                                                        }).ToList();
                                VLM.Userddllist.AddRange(GetAllMappedUser);

                            }


                            VLM.Userddllist = VLM.Userddllist.OrderBy(a => a.UserName).ToList();//show list order by username


                            //foreach (var item in GetMapUser)
                            //{
                            //    var mapid = Convert.ToInt32(item);
                            //    var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId).FirstOrDefault();
                            //    if (GetMapUserData != null)
                            //    {
                            //        var user = new Userddl
                            //        {
                            //            uid = mapid,
                            //            UserName = string.Format("{0} {1}", GetMapUserData.Fname, GetMapUserData.Lname)
                            //        };
                            //        VLM.Userddllist.Add(user);
                            //    }
                            //}
                        }
                        else
                        {
                            Userddl u1 = new Userddl();
                            u1.uid = UserID;
                            u1.UserName = "Myself"; //Session["UserName"].ToString();
                            VLM.Userddllist.Add(u1);
                        }


                        //check other branch user mapped to current user
                        string query = @"select muob.MappedUserId as uid,CONCAT(u.FName ,' ', u.Lname, '(Branch: ',IFNULL(cb.BranchName,'N / A'),')') as UserName,aorg.IsActive
                                         from CRM_MappedUserOtherBranch muob
                                         join crm_assignedtootherorganization aorg on muob.MappedUserId = aorg.AssignedUserID
                                         join crm_usertbl u on u.Id = aorg.AssignedUserID
                                         join com_branches cb on u.BranchID = cb.OrgBranchCode
                                         join company_profile cp on cb.OrganizationID = cp.id
                                         Where muob.UserId = '" + UserID + "' and aorg.AssignToCompanyID = '" + LoggedBranchId + "' and aorg.IsActive = 1 order by muob.CreateDate; ";
                        var data = db.Database.SqlQuery<Userddl>(query).ToList();
                        if (data.Count > 0)
                        {
                            VLM.OtherBranchMappedUser = data;
                            VLM.Userddllist.AddRange(data);

                            VLM.Userddllist = VLM.Userddllist.OrderBy(a => a.UserName).ToList();//show list order by username
                        }

                    }
                    #endregion
                }
                #endregion
                string MStartDate = "";
                string MEndDate = "";
                //if (CompanyID != 2066)
                //{
                #region Default Date show of one month
                VLM.DateFormat = Constant.DateFormat();//get date format by company id
                Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                DateTime bharatTime = Constant.GetBharatTime();//get india datetime
                var dd = bharatTime;
                DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                MEndDate = MonthendDate.ToString("dd/MM/yyyy");



                FilterType = Convert.ToString(Session["VLFltrFilterType"]);
                filterText = Convert.ToString(Session["filterText"]);

                if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
                {
                    if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        Session["VLFltrFrmDt"] = FromDate;
                        Session["VLFltrToDt"] = ToDate;

                        var fmDate = DateTime.ParseExact(FromDate, VLM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                        var tDate = DateTime.ParseExact(ToDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                        MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                        MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                    }
                    else
                    {
                        Session["VLFltrFrmDt"] = FromDate;
                        Session["VLFltrToDt"] = ToDate;
                        MStartDate = FromDate;
                        MEndDate = ToDate;
                    }

                }
                else
                {
                    if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        Session["VLFltrFrmDt"] = monthStartDate.ToString(VLM.DateFormat);
                        Session["VLFltrToDt"] = MonthendDate.ToString(VLM.DateFormat);
                    }
                    else
                    {
                        Session["VLFltrFrmDt"] = MStartDate;
                        Session["VLFltrToDt"] = MEndDate;
                    }
                }
                #endregion


                #region user lead reminder code

                List<LeadRemindersDTO> reminders = new List<LeadRemindersDTO>();

                DataTable GetLeadReminders = DataAccessLayer.GetDataTable(" call CRM_GetLeadReminderDataByuserId(" + UserID + ")");
                if (GetLeadReminders.Rows.Count > 0)
                {
                    for (int i = 0; i < GetLeadReminders.Rows.Count; i++)
                    {
                        int leadId = Convert.ToInt32(GetLeadReminders.Rows[i]["LeadId"]);
                        bool isRemind = Convert.ToBoolean(GetLeadReminders.Rows[i]["IsLeadReminder"]);
                        reminders.Add(new LeadRemindersDTO { LeadId = leadId, IsLeadReminder = isRemind });
                    }
                }
                #endregion


                if (FilterType == "Modified Date")
                {
                    #region Modified-Date
                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewLeadsDefaultOrFilterDemo(" + UID + ",'" + MStartDate + "','" + MEndDate + "','" + BranchID + "','" + CompanyID + "','" + filterText + "')");
                    if (GetRecords.Rows.Count > 0)
                    {
                        List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                        for (int i = 0; i < GetRecords.Rows.Count; i++)
                        {
                            ViewLeadsModel vlm = new ViewLeadsModel();
                            vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
                            vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
                            vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
                            vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
                            if (CompanyID == 296)//view reseller detail only for arunaw sir company
                            {

                                vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
                                vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
                                vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
                                vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
                                vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
                                vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);
                                vlm.SmartCapitaPlan = Convert.ToString(GetRecords.Rows[i]["SmartCapitaPlan"]);
                            }

                            vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
                            vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
                            vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
                            //}
                            vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
                            vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
                            vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
                            vlm.LeadStatusColor = Convert.ToString(GetRecords.Rows[i]["ColorHexValue"]);
                            vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
                            vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
                            vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                            vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
                            vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
                            vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
                            vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                            vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
                            vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);
                            vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
                            vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
                            vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
                            vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
                            vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
                            vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
                            vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
                            vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
                            vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
                            vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
                            vlm.IsDOB = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
                            vlm.IsMA = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";
                            vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
                            vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
                            vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
                            vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
                            vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
                            vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
                            vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
                            vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
                            vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
                            vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);
                            vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
                            vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
                            vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
                            vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
                            vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
                            vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
                            vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
                            vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
                            vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
                            vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
                            vlm.Extracol1dropdownId1 = GetRecords.Rows[i]["Extracol1dropdownId1"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId1"]);
                            vlm.Extracol1dropdownId2 = GetRecords.Rows[i]["Extracol1dropdownId2"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId2"]);
                            vlm.Extracol1dropdownId3 = GetRecords.Rows[i]["Extracol1dropdownId3"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId3"]);
                            vlm.Extracol1dropdownId4 = GetRecords.Rows[i]["Extracol1dropdownId4"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId4"]);
                            vlm.Extracol1dropdownId5 = GetRecords.Rows[i]["Extracol1dropdownId5"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId5"]);
                            //vlm.Extracol1dropdown1Id = GetRecords.Rows[i]["Extracol1dropdown1Id"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdown1Id"]);
                            vlm.ExtraCol1Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp1name"]);
                            vlm.ExtraCol2Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp2name"]);
                            vlm.ExtraCol3Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp3name"]);
                            vlm.ExtraCol4Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp4name"]);
                            vlm.ExtraCol5Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp5name"]);

                            if (reminders != null && reminders.Count > 0)
                            {
                                var lData = reminders.Where(a => a.LeadId == vlm.Id).FirstOrDefault();
                                vlm.LeadReminder = lData == null ? 1 : lData.IsLeadReminder == false ? 0 : 1;
                            }
                            else
                            {
                                vlm.LeadReminder = (Convert.ToInt32(vlm.LeadOwner) != UserID && string.IsNullOrEmpty(vlm.AssignTo)) || (!string.IsNullOrEmpty(vlm.AssignTo) && Convert.ToInt32(vlm.AssignTo) != UserID) ? 0 : 1;
                            }
                            vlm.ConvertedFupDateTime = GetRecords.Rows[i]["ConvertedFupDateTime"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(GetRecords.Rows[i]["ConvertedFupDateTime"]);

                            vlmList.Add(vlm);
                        }
                        VLM.viewleadsList = vlmList.ToList();
                        VLM.AllviewleadsList = VLM.viewleadsList;

                        //if (UID > 0)
                        //{
                        //    VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
                        //    VLM.AllviewleadsList = VLM.viewleadsList;
                        //}
                    }
                    #endregion
                }
                //else if ((FilterType == "AssignDate" || FilterType == "DOB" || FilterType == "MarriageAnniversary" || FilterType == "Created Date" || FilterType == "Followup Date" || FilterType == "ExpectedDate" || FilterType == "ExtCol9Date" || FilterType == "ExtCol10Date" || FilterType == "ExtCol18Date" || FilterType == "ExtCol19Date" || FilterType == "ExtCol20Date") && string.IsNullOrEmpty(filterText))
                else
                {
                    #region Assigned-Date
                    DataTable GetRecords = null;
                    GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewLeadsDefaultOrFilter('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + ",'" + CompanyID + "','" + FilterType + "','" + filterText + "')");
                    if (GetRecords.Rows.Count > 0)
                    {
                        List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                        for (int i = 0; i < GetRecords.Rows.Count; i++)
                        {
                            ViewLeadsModel vlm = new ViewLeadsModel();
                            vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
                            vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
                            vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
                            vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
                            if (CompanyID == 296)//view reseller detail only for arunaw sir company
                            {
                                //if (!string.IsNullOrEmpty(vlm.EMail))
                                //{
                                //    var reseller = cr.GetReseller(CompanyID, vlm.EMail);
                                //    if (reseller != null)
                                //    {
                                //        vlm.ResellerId = reseller.ResellerId;
                                //        vlm.ResellerName = reseller.ResellerName;
                                //        vlm.ResellerContactNo = reseller.ResellerContactNo;
                                //        vlm.ResellerStatus = reseller.ResellerStatus;
                                //        vlm.ResellerCode = reseller.ResellerCode;
                                //    }
                                //}
                                vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
                                vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
                                vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
                                vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
                                vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
                                vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);
                                vlm.SmartCapitaPlan = Convert.ToString(GetRecords.Rows[i]["SmartCapitaPlan"]);
                            }


                            vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
                            vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
                            vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
                            vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
                            vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
                            vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
                            vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
                            vlm.LeadStatusColor = Convert.ToString(GetRecords.Rows[i]["ColorHexValue"]);
                            vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
                            vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                            vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
                            vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
                            vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
                            vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                            vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
                            vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

                            vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
                            vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
                            vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
                            vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
                            vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
                            vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
                            vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
                            vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
                            vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
                            vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);

                            vlm.IsDOB = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
                            vlm.IsMA = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";

                            vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
                            vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
                            vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
                            vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
                            vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
                            vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
                            vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
                            vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
                            vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
                            vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

                            vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
                            vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
                            vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
                            vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
                            vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
                            vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
                            vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
                            vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
                            vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
                            vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
                            vlm.Extracol1dropdownId1 = GetRecords.Rows[i]["Extracol1dropdownId1"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId1"]);
                            vlm.Extracol1dropdownId2 = GetRecords.Rows[i]["Extracol1dropdownId2"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId2"]);
                            vlm.Extracol1dropdownId3 = GetRecords.Rows[i]["Extracol1dropdownId3"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId3"]);
                            vlm.Extracol1dropdownId4 = GetRecords.Rows[i]["Extracol1dropdownId4"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId4"]);
                            vlm.Extracol1dropdownId5 = GetRecords.Rows[i]["Extracol1dropdownId5"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId5"]);
                            //vlm.Extracol1dropdown1Id = GetRecords.Rows[i]["Extracol1dropdown1Id"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdown1Id"]);
                            vlm.ExtraCol1Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp1name"]);
                            vlm.ExtraCol2Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp2name"]);
                            vlm.ExtraCol3Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp3name"]);
                            vlm.ExtraCol4Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp4name"]);
                            vlm.ExtraCol5Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp5name"]);
                            if (reminders != null && reminders.Count > 0)
                            {
                                var lData = reminders.Where(a => a.LeadId == vlm.Id).FirstOrDefault();
                                vlm.LeadReminder = lData == null ? 1 : lData.IsLeadReminder == false ? 0 : 1;
                            }
                            else
                            {
                                vlm.LeadReminder = (Convert.ToInt32(vlm.LeadOwner) != UserID && string.IsNullOrEmpty(vlm.AssignTo)) || (!string.IsNullOrEmpty(vlm.AssignTo) && Convert.ToInt32(vlm.AssignTo) != UserID) ? 0 : 1;
                            }
                            vlm.ConvertedFupDateTime = GetRecords.Rows[i]["ConvertedFupDateTime"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(GetRecords.Rows[i]["ConvertedFupDateTime"]);
                            vlmList.Add(vlm);
                        }


                        VLM.viewleadsList = !string.IsNullOrEmpty(Country) ? vlmList.Where(a => a.Country == Country).ToList() : vlmList.ToList();
                        VLM.AllviewleadsList = VLM.viewleadsList;

                        //if (UID > 0)
                        //{
                        //    if (FilterType != "AssignDate")
                        //    {
                        //        VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
                        //        VLM.AllviewleadsList = VLM.viewleadsList;
                        //    }
                        //}
                    }
                    #endregion
                }


                if (Convert.ToString(Session["UserType"]) != "SuperAdmin" && VLM.Userddllist != null && VLM.Userddllist.Count > 0 && !string.IsNullOrEmpty(UserddlName) && UserddlName == "0")
                {
                    VLM.viewleadsList = null;
                    VLM.viewleadsList = new List<ViewLeadsModel>();
                    foreach (var item in VLM.Userddllist)
                    {
                        string leadOwner = Convert.ToString(item.uid);
                        if (leadOwner != "0")
                        {
                            List<ViewLeadsModel> VlieadList = VLM.AllviewleadsList.Where(em => (em.LeadOwner == leadOwner && (em.AssignTo == null || em.AssignTo == "" || em.AssignTo != leadOwner)) || em.AssignTo == leadOwner).ToList();
                            VLM.viewleadsList.AddRange(VlieadList);
                        }
                    }
                }


                #region filter by term and product type and lead source
                if (!string.IsNullOrEmpty(Term))
                {
                    Term = Term.ToLower().Trim();
                    VLM.viewleadsList = VLM.viewleadsList.Where(a => (a.LeadName.ToLower().Trim().Contains(Term)
                    || (!string.IsNullOrEmpty(a.EMail) && a.EMail.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.Mob) && a.Mob == Term)
                    || (!string.IsNullOrEmpty(a.URL) && a.URL.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.SkypeId) && a.SkypeId.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.Address) && a.Address.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.Designation) && a.Designation.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.OrganizationName) && a.OrganizationName.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.ExtraCol1) && a.ExtraCol1.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.ExtraCol2) && a.ExtraCol2.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.ExtraCol3) && a.ExtraCol3.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.ExtraCol4) && a.ExtraCol4.ToLower().Trim().Contains(Term))
                    || (!string.IsNullOrEmpty(a.ExtraCol5) && a.ExtraCol5.ToLower().Trim().Contains(Term))
                    )).ToList();
                    Session["FilterTerm"] = Term;
                }
                else if (ProductTypeID != null)
                {
                    VLM.viewleadsList = VLM.viewleadsList.Where(a => (a.ProductTypeID != null || a.ProductTypeID != 0) && a.ProductTypeID == ProductTypeID).ToList();
                    Session["ProductTypeID"] = ProductTypeID;
                }
                else if (LeadSourceID != null)
                {
                    VLM.viewleadsList = VLM.viewleadsList.Where(a => (a.LeadSourceID != null || a.LeadSourceID != 0) && a.LeadSourceID == LeadSourceID).ToList();
                    Session["LeadSourceID"] = LeadSourceID;
                }
                #endregion




                VLM.TotalLead = VLM.viewleadsList.Count();

                VLM.FromDate = FromDate;
                VLM.ToDate = ToDate;
                VLM.FilterType = FilterType;
                VLM.filterText = filterText;
                VLM.Term = Term;
                VLM.ProductTypeID = ProductTypeID;
                VLM.LeadSourceID = LeadSourceID;
                //////////////////////////////////////////////////////////////export to excel start////////////////////////////////////////////////////////////
                int Id = Convert.ToInt32(Session["UID"]);
                string userquerydata = @"select ur.Id as UserID,  ur.ByUID as SalaryID
                                from crm_usertbl ur Where ur.ByUID =2146";
                var x2 = db.Database.SqlQuery<CreateUserModel>(userquerydata).OrderBy(em => em.UserID).ToList();
                if (x2 != null && x2.Count > 0)
                {
                    var existuser = x2.Where(a => a.UserID == Id).FirstOrDefault();
                    if (existuser != null)
                    {
                        Session["exporttoexcel"] = 1;
                    }
                }



                //////////////////////////////////////////////////////////////export to excel end/////////////////////////////////////////////////////////////////
                #region Pagging-Start


                int pages = 0;
                int TotalProducts = 0;
                int pageSize = 50;
                TotalProducts = VLM.viewleadsList.Count();
                pages = (TotalProducts / pageSize);
                var x = pages * pageSize;
                if (x < TotalProducts)
                {
                    pages += 1;
                }
                int Rem = 0;
                ViewData["nextpagesid"] = 1;
                ViewData["prevpagesid"] = 1;
                if (pagetypeType == "next")
                {
                    var pagecount = page + 10;
                    if (pages <= pagecount)
                    {
                        ViewData["nextpagesid"] = 0;
                        ViewData["NoOfPages"] = pages;
                        ViewData["NoOfstartpagesPages"] = page;
                    }
                    else
                    {
                        ViewData["NoOfPages"] = pagecount;
                        ViewData["NoOfstartpagesPages"] = page;
                    }
                    int pageNumber = page + 1;
                    // int pageSize = 50;
                    // pageNumber = page;
                    int pageSkip = (pageNumber - 1) * pageSize;
                    var Product = VLM.viewleadsList.Skip(pageSkip).Take(pageSize).ToList();
                    VLM.viewleadsList = Product;
                    Rem = (TotalProducts % pageSize);
                    if (Rem < pageSize && Rem != 0)
                    {
                        pages = (pages + 1);
                    }
                    if (page > 1)
                    {
                        var DeclareIndex = (pageSize * (page - 1)) + 1;
                        ViewData["DeclareIndex"] = DeclareIndex;
                    }
                    else
                    {
                        ViewData["DeclareIndex"] = 1;
                    }

                    VLM.page = page;
                    Session["cpage"] = page;
                }
                else if (pagetypeType == "prev")
                {

                    int pageNumber;
                    var pagecount = page - 9;
                    if (pagecount > 9)
                    {
                        ViewData["nextpagesid"] = 1;
                        ViewData["prevpagesid"] = 1;
                        ViewData["NoOfPages"] = pagecount;
                        ViewData["NoOfstartpagesPages"] = pagecount - 9;
                        pageNumber = pagecount - 8;
                    }
                    else
                    {
                        page = 1;
                        if (pages >= 10)
                        {

                            ViewData["NoOfPages"] = 10;
                        }
                        else
                        {

                            ViewData["NoOfPages"] = pages;
                        }
                        ViewData["NoOfstartpagesPages"] = 1;
                        ViewData["prevpagesid"] = 0;
                        //ViewData["NoOfPages"] = page;
                        //ViewData["NoOfstartpagesPages"] = page-8;
                        pageNumber = page;
                    }
                    int pageSkip = (pageNumber - 1) * pageSize;
                    var Product = VLM.viewleadsList.Skip(pageSkip).Take(pageSize).ToList();
                    VLM.viewleadsList = Product;
                    Rem = (TotalProducts % pageSize);
                    if (Rem < pageSize && Rem != 0)
                    {
                        pages = (pages + 1);
                    }
                    if (page > 1)
                    {
                        var DeclareIndex = (pageSize * (pageNumber - 1)) + 1;
                        ViewData["DeclareIndex"] = DeclareIndex;
                    }
                    else
                    {
                        ViewData["DeclareIndex"] = 1;
                    }

                    VLM.page = page;
                    Session["cpage"] = page;
                }
                else
                {
                    int pageNumber;
                    if (page == 1)
                    {
                        page = 1;
                        if (pages <= 10)
                        {
                            ViewData["nextpagesid"] = 0;
                            ViewData["prevpagesid"] = 0;
                            ViewData["NoOfPages"] = pages;
                            ViewData["NoOfstartpagesPages"] = 1;
                        }
                        else
                        {
                            ViewData["nextpagesid"] = 1;
                            ViewData["prevpagesid"] = 0;
                            ViewData["NoOfPages"] = 10;
                            ViewData["NoOfstartpagesPages"] = 1;
                        }

                    }
                    else
                    {
                        var pagecount = page + 10;
                        if (pages <= pagecount)
                        {
                            ViewData["nextpagesid"] = 0;
                            ViewData["NoOfPages"] = pages;
                            ViewData["NoOfstartpagesPages"] = page;
                        }
                        else
                        {
                            ViewData["NoOfPages"] = pagecount;
                            ViewData["NoOfstartpagesPages"] = page;
                        }
                    }


                    pageNumber = page;
                    int pageSkip = (pageNumber - 1) * pageSize;
                    var Product = VLM.viewleadsList.Skip(pageSkip).Take(pageSize).ToList();
                    VLM.viewleadsList = Product;
                    Rem = (TotalProducts % pageSize);
                    if (Rem < pageSize && Rem != 0)
                    {
                        pages = (pages + 1);
                    }
                    if (page > 1)
                    {
                        var DeclareIndex = (pageSize * (page - 1)) + 1;
                        ViewData["DeclareIndex"] = DeclareIndex;
                    }
                    else
                    {
                        ViewData["DeclareIndex"] = 1;
                    }

                    VLM.page = page;
                    Session["cpage"] = page;
                }
                #endregion

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(VLM);
        }
        public ActionResult AssigntoassignBy(string FromDate, string ToDate, int? Assignby, int? Assignto)
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            int UserID = Convert.ToInt32(Session["UID"]);
            ViewBag.LoggedUserId = UserID;

            string MStartDate = "";
            string MEndDate = "";
            //if (CompanyID != 2066)
            //{
            #region Default Date show of one month
            VLM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            DateTime bharatTime = Constant.GetBharatTime();//get india datetime
            var dd = bharatTime;
            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            MEndDate = MonthendDate.ToString("dd/MM/yyyy");

            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
            {
                if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    Session["VLFltrFrmDt"] = FromDate;
                    Session["VLFltrToDt"] = ToDate;

                    var fmDate = DateTime.ParseExact(FromDate, VLM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                    var tDate = DateTime.ParseExact(ToDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                }
                else
                {
                    Session["VLFltrFrmDt"] = FromDate;
                    Session["VLFltrToDt"] = ToDate;
                    MStartDate = FromDate;
                    MEndDate = ToDate;
                }

            }
            else
            {
                if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    Session["VLFltrFrmDt"] = monthStartDate.ToString(VLM.DateFormat);
                    Session["VLFltrToDt"] = MonthendDate.ToString(VLM.DateFormat);
                }
                else
                {
                    Session["VLFltrFrmDt"] = MStartDate;
                    Session["VLFltrToDt"] = MEndDate;
                }
            }
            #endregion

            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {
                #region Admin View All Users
                string userquery = @"select ur.Id as UserID,  CONCAT(ur.FName ,' ', ur.Lname, '(' , ur.EmployeeCode,')') AS UserName 
                                from crm_usertbl ur                                
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1";
                var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                // var q = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName == "Sales" && em.Status == true).OrderBy(em => em.Fname).ToList();
                VLM.Userddllist = new List<Userddl>();

                //Userddl u = new Userddl();
                if (q != null && q.Count > 0)
                {
                    VLM.Userddllist.Add(new Userddl { uid = 0, UserName = "All" });
                    VLM.Userddllist.Add(new Userddl { uid = UserID, UserName = "Myself" });
                    foreach (var item in q)
                    {
                        if (item.UserName != null)
                        {
                            var user = new Userddl
                            {
                                uid = Convert.ToInt32(item.UserID),
                                // UserName = item.Fname + " " + item.Lname
                                UserName = item.UserName,
                            };

                            VLM.Userddllist.Add(user);
                        }
                    }
                }
                else
                {
                    VLM.Userddllist.Add(new Userddl { uid = UserID, UserName = "Myself" });
                }
                #endregion

            }
            else
            {
                #region Employee will view only mapped user
                string userquery = @"select ur.Id as UserID,  CONCAT(ur.FName ,' ', ur.Lname, '(' , ur.EmployeeCode,')') AS UserName 
                                from crm_usertbl ur                                
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and ur.Id=" + UserID + "";
                var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                // var q = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName == "Sales" && em.Status == true).OrderBy(em => em.Fname).ToList();
                VLM.Userddllist = new List<Userddl>();

                //Userddl u = new Userddl();
                if (q != null && q.Count > 0)
                {
                    VLM.Userddllist.Add(new Userddl { uid = 0, UserName = "All" });
                    VLM.Userddllist.Add(new Userddl { uid = UserID, UserName = "Myself" });

                    foreach (var item in q)
                    {
                        if (item.UserName != null)
                        {
                            var user = new Userddl
                            {
                                uid = Convert.ToInt32(item.UserID),
                                // UserName = item.Fname + " " + item.Lname
                                UserName = item.UserName,
                            };
                            VLM.Userddllist.Add(user);
                        }
                    }
                }
                else
                {
                    VLM.Userddllist.Add(new Userddl { UserName = "Myself" });
                }
                #endregion
            }



            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_AssignToAssignByReport(" + UserID + ",'" + Assignby + "','" + Assignto + "','" + MStartDate + "','" + MEndDate + "','" + BranchID + "','" + CompanyID + "')");
            if (GetRecords.Rows.Count > 0)
            {
                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                for (int i = 0; i < GetRecords.Rows.Count; i++)
                {
                    ViewLeadsModel vlm = new ViewLeadsModel();
                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
                    if (CompanyID == 296)//view reseller detail only for arunaw sir company
                    {

                        vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
                        vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
                        vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
                        vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
                        vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
                        vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);
                        vlm.SmartCapitaPlan = Convert.ToString(GetRecords.Rows[i]["SmartCapitaPlan"]);
                    }

                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
                    vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
                    vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
                    //}
                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
                    vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
                    vlm.LeadStatusColor = Convert.ToString(GetRecords.Rows[i]["ColorHexValue"]);
                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);
                    vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
                    vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
                    vlm.IsDOB = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
                    vlm.IsMA = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";
                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);
                    vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
                    vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
                    vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
                    vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
                    vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
                    vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
                    vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
                    vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
                    vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
                    vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
                    vlm.Extracol1dropdownId1 = GetRecords.Rows[i]["Extracol1dropdownId1"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId1"]);
                    vlm.Extracol1dropdownId2 = GetRecords.Rows[i]["Extracol1dropdownId2"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId2"]);
                    vlm.Extracol1dropdownId3 = GetRecords.Rows[i]["Extracol1dropdownId3"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId3"]);
                    vlm.Extracol1dropdownId4 = GetRecords.Rows[i]["Extracol1dropdownId4"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId4"]);
                    vlm.Extracol1dropdownId5 = GetRecords.Rows[i]["Extracol1dropdownId5"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdownId5"]);
                    //vlm.Extracol1dropdown1Id = GetRecords.Rows[i]["Extracol1dropdown1Id"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["Extracol1dropdown1Id"]);
                    vlm.ExtraCol1Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp1name"]);
                    vlm.ExtraCol2Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp2name"]);
                    vlm.ExtraCol3Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp3name"]);
                    vlm.ExtraCol4Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp4name"]);
                    vlm.ExtraCol5Dropdown = Convert.ToString(GetRecords.Rows[i]["Dp5name"]);

                    vlmList.Add(vlm);
                }
                VLM.AssignToAssignByList = vlmList.ToList();

                //if (UID > 0)
                //{
                //    VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
                //    VLM.AllviewleadsList = VLM.viewleadsList;
                //}
            }

            return View(VLM);
        }
        public ActionResult LeadReport(string FromDate, string ToDate, string UserddlName)
        {
            //ViewLeadsModel VLM = new ViewLeadsModel();
            ////VLM.LeadDate = Constant.GetBharatTime();
            //try
            //{

            //    string language = cr.GetCompanyLanguage(0);//get company Language
            //    VLM.Language = language;
            //    if (Session["UID"] != null)
            //    {

            //        Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            //        Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            //        int LoggedBranchId = Convert.ToInt32(Session["BranchID"]);
            //        int LoggedCompanyId = Convert.ToInt32(Session["CompanyID"]);

            //        int LoggedUID = Convert.ToInt32(Session["UID"]);

            //        var LeadRecords = new List<ViewLeadsModel>();


            //        var UID = 0;
            //        if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            //        {
            //            UID = 0;
            //            if (!string.IsNullOrEmpty(UserddlName) && UserddlName != "0")
            //            {
            //                UID = Convert.ToInt32(UserddlName);
            //            }
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(UserddlName) && UserddlName != "0")
            //            {
            //                UID = Convert.ToInt32(UserddlName);
            //            }
            //            else
            //            {
            //                UID = Convert.ToInt32(Session["UID"]);
            //            }
            //        }

            //        if (Session["UserddlName"] == null)
            //        {
            //            VLM.UserddlName = Convert.ToString(UID);
            //            UserddlName = Convert.ToString(UID);
            //        }
            //        else
            //        {
            //            VLM.UserddlName = Convert.ToString(Session["UserddlName"]);
            //            UserddlName = Convert.ToString(Session["UserddlName"]);
            //        }

            //        if (!string.IsNullOrEmpty(UserddlName))
            //        {
            //            var data = cr.GetUserCompanyBranch(UID);
            //            if (data != null)
            //            {
            //                BranchID = data.BranchID;
            //                CompanyID = data.CompanyID;
            //            }
            //        }

            //        #region Data-time-Formate
            //        VLM.DateFormat = Constant.DateFormat();//get date format by company id
            //        Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            //        var dd = Constant.GetBharatTime();//get india datetime

            //        //DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            //        //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);

            //        VLM.TodayDate = dd.ToString("dd/MM/yyyy");//get today date for today followup lead link

            //        DateTime monthStartDate = dd.AddDays(-5);
            //        DateTime MonthendDate = new DateTime(dd.Year, dd.Month, dd.Day);

            //        var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            //        var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
            //        #region To-CheckFilter-Date
            //        if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
            //        {
            //            if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
            //            {
            //                TempData["Fromdate"] = FromDate;
            //                TempData["Todate"] = ToDate;

            //                var fmDate = DateTime.ParseExact(FromDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
            //                var tDate = DateTime.ParseExact(ToDate, VLM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(ToDate);

            //                MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
            //                MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

            //            }
            //            else
            //            {
            //                TempData["Fromdate"] = FromDate;
            //                TempData["Todate"] = ToDate;
            //                MStartDate = FromDate;
            //                MEndDate = ToDate;
            //            }
            //        }
            //        else
            //        {
            //            if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
            //            {
            //                TempData["Fromdate"] = monthStartDate.ToString(VLM.DateFormat);
            //                TempData["Todate"] = MonthendDate.ToString(VLM.DateFormat);
            //            }
            //            else
            //            {
            //                TempData["Fromdate"] = MStartDate;
            //                TempData["Todate"] = MEndDate;
            //            }
            //        }
            //        #endregion

            //        #endregion


            //        if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            //        {
            //            #region Mapped User
            //            string userquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName 
            //                    from crm_usertbl ur
            //                    join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
            //                    Where ur.BranchID = " + LoggedBranchId + " and ur.CompanyID = " + LoggedCompanyId + " and ur.Status = 1 and rl.ViewLeads = 1";
            //            var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
            //            // var q = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName == "Sales" && em.Status == true).OrderBy(em => em.Fname).ToList();
            //            VLM.Userddllist = new List<Userddl>();

            //            if (q != null && q.Count > 0)
            //            {
            //                Userddl u = new Userddl();
            //                u.UserName = "All";
            //                //Userddl u1 = new Userddl();
            //                VLM.Userddllist.Add(u);
            //                foreach (var item in q)
            //                {
            //                    var user = new Userddl
            //                    {
            //                        uid = Convert.ToInt32(item.UserID),
            //                        //UserName = item.Fname + " " + item.Lname
            //                        UserName = item.UserName
            //                    };
            //                    VLM.Userddllist.Add(user);
            //                }
            //            }

            //            #endregion

            //            #region Get other branch user list to add mapped user list

            //            DataTable GetOtherMappedRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + LoggedBranchId + "','','')");
            //            if (GetOtherMappedRecords.Rows.Count > 0)
            //            {
            //                var GetOtherBranchUserList = (from dr in GetOtherMappedRecords.AsEnumerable()
            //                                              select new Userddl()
            //                                              {
            //                                                  uid = Convert.ToInt32(dr["AssignedUserID"]),
            //                                                  UserName = Convert.ToString(dr["UserName"] + " (Branch: " + dr["BranchName"] + ")"),
            //                                                  IsActive = Convert.ToBoolean(dr["IsActive"])
            //                                              }).Where(a => a.IsActive == true).OrderBy(a => a.UserName).ToList();
            //                VLM.Userddllist.AddRange(GetOtherBranchUserList);

            //                VLM.Userddllist = VLM.Userddllist.OrderBy(a => a.UserName).ToList();
            //            }

            //            #endregion
            //        }
            //        else
            //        {
            //            #region Get-MappedUser-Parents
            //            var GetUserData = db.crm_usertbl.Where(em => em.Id == LoggedUID && em.BranchID == LoggedBranchId && em.CompanyID == LoggedCompanyId).FirstOrDefault();
            //            if (GetUserData != null && GetUserData.MappedUsers != null)
            //            {
            //                VLM.MappedUser = GetUserData.MappedUsers.ToString();
            //                var GetMapUser = GetUserData.MappedUsers.Split(',');
            //                VLM.Userddllist = new List<Userddl>();

            //                if (GetMapUser != null && GetMapUser.Count() > 0)
            //                {

            //                    VLM.Userddllist.Add(new Userddl { UserName = "All" });

            //                    string unm = Session["UserName"].ToString();
            //                    VLM.Userddllist.Add(new Userddl { uid = UID, UserName = unm });

            //                    foreach (var item in GetMapUser)
            //                    {
            //                        var mapid = Convert.ToInt32(item);
            //                        var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
            //                        if (GetMapUserData != null)
            //                        {
            //                            var user = new Userddl
            //                            {
            //                                uid = mapid,
            //                                UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
            //                            };
            //                            VLM.Userddllist.Add(user);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    Userddl u1 = new Userddl();
            //                    u1.uid = UID;
            //                    u1.UserName = Session["UserName"].ToString();
            //                    VLM.Userddllist.Add(u1);
            //                }


            //                //check other branch user mapped to current user
            //                string query = @"select muob.MappedUserId as uid,CONCAT(u.FName ,' ', u.Lname, '(Branch: ',IFNULL(cb.BranchName,'N / A'),')') as UserName,aorg.IsActive
            //                             from CRM_MappedUserOtherBranch muob
            //                             join crm_assignedtootherorganization aorg on muob.MappedUserId = aorg.AssignedUserID
            //                             join crm_usertbl u on u.Id = aorg.AssignedUserID
            //                             join com_branches cb on u.BranchID = cb.OrgBranchCode
            //                             join company_profile cp on cb.OrganizationID = cp.id
            //                             Where muob.UserId = '" + LoggedUID + "' and aorg.AssignToCompanyID = '" + LoggedBranchId + "' and aorg.IsActive = 1 order by muob.CreateDate; ";
            //                var data = db.Database.SqlQuery<Userddl>(query).ToList();
            //                if (data.Count > 0)
            //                {
            //                    VLM.OtherBranchMappedUser = data;

            //                    VLM.Userddllist.AddRange(data);
            //                }
            //            }
            //            #endregion
            //        }

            //        if (Convert.ToInt32(Session["CompanyID"]) != 2066 && Convert.ToInt32(Session["BranchID"]) != 1979)
            //        {
            //            #region New-Lead
            //            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetNewLeadSummaryReport('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //            if (GetRecords.Rows.Count > 0)
            //            {
            //                var GetLeadsRecords = (from dr in GetRecords.AsEnumerable()
            //                                       select new ViewLeadsModel()
            //                                       {
            //                                           Id = Convert.ToInt32(dr["ID"]),
            //                                           LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                           AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                           AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                           AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                           LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                           AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                           ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                           LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                           LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                       }).ToList();

            //                LeadRecords.AddRange(GetLeadsRecords);

            //            }

            //            #endregion

            //            #region Followup-Missed-Delayed lead

            //            DataTable GetFMDRecords = DataAccessLayer.GetDataTable("call CRM_GetFollowupMissedDelayedLead('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //            if (GetFMDRecords.Rows.Count > 0)
            //            {
            //                var GetFMDLeadsRecords = (from dr in GetFMDRecords.AsEnumerable()
            //                                          select new ViewLeadsModel()
            //                                          {
            //                                              Id = Convert.ToInt32(dr["ID"]),
            //                                              LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                              AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                              AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                              AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                              LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                              AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                              ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                              LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                              LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                          }).ToList();


            //                LeadRecords.AddRange(GetFMDLeadsRecords);
            //            }



            //            DataTable GetFUpRecords = DataAccessLayer.GetDataTable("call CRM_GetLSReportByFollowupDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //            if (GetFUpRecords.Rows.Count > 0)
            //            {
            //                var GetFUPLeadsRecords = (from dr in GetFUpRecords.AsEnumerable()
            //                                          select new ViewLeadsModel()
            //                                          {
            //                                              Id = Convert.ToInt32(dr["ID"]),
            //                                              LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                              AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                              AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                              AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                              LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                              AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                              ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                              LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                              LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                          }).ToList();



            //                var FUPLead = GetFUPLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
            //                LeadRecords.AddRange(FUPLead);

            //            }

            //            int followup = LeadRecords.Where(a => a.LeadStatusName.Equals("Follow")).ToList().Count();
            //            int Missedfollowup = LeadRecords.Where(a => a.LeadStatusName.Equals("Missed")).ToList().Count();


            //            ViewBag.FollowUp = followup;
            //            ViewBag.MissedFollowUp = Missedfollowup;

            //            int Delayedfollowup = LeadRecords.Where(a => a.LeadStatusName.Equals("Delayed")).ToList().Count();
            //            ViewBag.Delayedfollowup = Delayedfollowup;

            //            #endregion

            //            #region New-Leads                    

            //            var NewLeadsRecords = LeadRecords.Where(a => a.LeadStatusName.Equals("NewLead")).ToList().Count();
            //            int newCount = Convert.ToInt32(NewLeadsRecords);
            //            ViewBag.NewLeadsCount = newCount;

            //            #endregion

            //            #region Not-Interested
            //            int notinterestCount = LeadRecords.Where(a => a.LeadStatus.Equals("Not Interested")).GroupBy(a => a.Id).ToList().Count();
            //            ViewBag.NotInterestedCount = notinterestCount;
            //            #endregion

            //            #region Closed

            //            int ClosedCount = LeadRecords.Where(a => a.LeadStatus.Equals("Closed")).GroupBy(a => a.Id).ToList().Count();
            //            ViewBag.ClosedRecordsCount = ClosedCount;

            //            #endregion

            //            #region Suspect-Leads

            //            int SuspectCount = LeadRecords.Where(a => a.LeadStatus.Equals("Suspect")).GroupBy(a => a.Id).ToList().Count();
            //            ViewBag.SuspectLeadsCount = SuspectCount;
            //            #endregion

            //            #region Prospect-Leads                   
            //            int prospectCount = LeadRecords.Where(a => a.LeadStatus.Equals("Prospect")).GroupBy(a => a.Id).ToList().Count();
            //            ViewBag.ProspectCount = prospectCount;

            //            #endregion
            //            #region pie chart data
            //            var cData = new List<PieChartModel>();

            //            cData.Add(new PieChartModel("New Leads", newCount));
            //            cData.Add(new PieChartModel("Follow Ups", followup));
            //            cData.Add(new PieChartModel("Missed Follow Ups", Missedfollowup));
            //            cData.Add(new PieChartModel("Delayed Follow Ups", Delayedfollowup));
            //            cData.Add(new PieChartModel("Not Interested", notinterestCount));
            //            cData.Add(new PieChartModel("Closed Leads", ClosedCount));
            //            cData.Add(new PieChartModel("Suspect Leads", SuspectCount));
            //            cData.Add(new PieChartModel("Prospect Leads", prospectCount));

            //            //convert to json format data
            //            ViewBag.DataList = JsonConvert.SerializeObject(cData);

            //            #endregion
            //        }
            //        else
            //        {
            //            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetNewLeadSummaryReport1('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //            if (GetRecords.Rows.Count > 0)
            //            {
            //                var GetLeadsRecords = (from dr in GetRecords.AsEnumerable()
            //                                       select new ViewLeadsModel()
            //                                       {
            //                                           Id = Convert.ToInt32(dr["ID"]),
            //                                           LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                           AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                           AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                           AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                           LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                           AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                           ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                           LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                           LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                       }).ToList();

            //                LeadRecords.AddRange(GetLeadsRecords);

            //            }
            //            DataTable GetFMDRecords = DataAccessLayer.GetDataTable("call CRM_GetFollowupMissedDelayedLead1('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //            if (GetFMDRecords.Rows.Count > 0)
            //            {
            //                var GetFMDLeadsRecords = (from dr in GetFMDRecords.AsEnumerable()
            //                                          select new ViewLeadsModel()
            //                                          {
            //                                              Id = Convert.ToInt32(dr["ID"]),
            //                                              LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                              AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                              AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                              AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                              LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                              AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                              ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                              LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                              LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                          }).ToList();


            //                LeadRecords.AddRange(GetFMDLeadsRecords);
            //            }


            //            DataTable GetFUpRecords = DataAccessLayer.GetDataTable("call CRM_GetLSReportByFollowupDate1('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //            if (GetFUpRecords.Rows.Count > 0)
            //            {
            //                var GetFUPLeadsRecords = (from dr in GetFUpRecords.AsEnumerable()
            //                                          select new ViewLeadsModel()
            //                                          {
            //                                              Id = Convert.ToInt32(dr["ID"]),
            //                                              LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                              AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                              AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                              AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                              LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                              AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                              ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                              LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                              LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                          }).ToList();



            //                var FUPLead = GetFUPLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
            //                LeadRecords.AddRange(FUPLead);

            //            }

            //            //LeadRecords.AddRange(getotherleads);
            //            var NewLeadsRecords = LeadRecords.Where(a => a.LeadStatus.Equals("Open")).ToList().Count();
            //            int newCount = Convert.ToInt32(NewLeadsRecords);
            //            ViewBag.NewLeadsCount = newCount;

            //            int Freetrail = LeadRecords.Where(a => a.LeadStatus.Equals("Free Trail")).ToList().Count();
            //            ViewBag.Freetraillead = Freetrail;

            //            int LeadList = LeadRecords.Where(a => a.LeadStatus.Equals("Lead List")).ToList().Count();
            //            ViewBag.LeadListLead = LeadList;

            //            int Followup = LeadRecords.Where(a => a.LeadStatus.Equals("Followup")).ToList().Count();
            //            ViewBag.FollowupLead = Followup;

            //            int Delayedfollowup = LeadRecords.Where(a => a.LeadStatus.Equals("Not Interested")).ToList().Count();
            //            ViewBag.DelayedfollowupLead = Delayedfollowup;

            //            int RequestList = LeadRecords.Where(a => a.LeadStatus.Equals("Request List")).ToList().Count();
            //            ViewBag.RequestListLead = RequestList;

            //            int Subscribe = LeadRecords.Where(a => a.LeadStatus.Equals("Subscribe")).ToList().Count();
            //            ViewBag.SubscribeLead = Subscribe;

            //            int Archive = LeadRecords.Where(a => a.LeadStatus.Equals("Archive")).ToList().Count();
            //            ViewBag.ArchiveLead = Archive;

            //            int Closed = LeadRecords.Where(a => a.LeadStatus.Equals("Closed")).ToList().Count();
            //            ViewBag.ClosedLead = Closed;

            //            var cData = new List<PieChartModel>();
            //            #region pie chart data
            //            cData.Add(new PieChartModel("Open", newCount));
            //            cData.Add(new PieChartModel("Lead List", LeadList));
            //            cData.Add(new PieChartModel("Followup", Followup));
            //            cData.Add(new PieChartModel("Not Interested", Delayedfollowup));
            //            cData.Add(new PieChartModel("Free Trail", Freetrail));
            //            cData.Add(new PieChartModel("Request List", RequestList));
            //            cData.Add(new PieChartModel("Subscribe", Subscribe));
            //            cData.Add(new PieChartModel("Archive", Archive));
            //            cData.Add(new PieChartModel("Closed", Closed));

            //            //convert to json format data
            //            ViewBag.DataList = JsonConvert.SerializeObject(cData);
            //            #endregion
            //        }



            //        #region Get-AssignToOthersLeads Info

            //        //Collect the Manually Created-Leads
            //        DataTable GetAssignedRecords = DataAccessLayer.GetDataTable(" call CRM_GetAssignToOtherLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //        //if (GetAssignedRecords.Rows.Count > 0)
            //        //{
            //        //var AssignedRecord = (from dr in GetAssignedRecords.AsEnumerable()
            //        //                     select new ViewLeadsModel()
            //        //                     {
            //        //                         Id = Convert.ToInt32(dr["Id"]),
            //        //                         LeadName = Convert.ToString(dr["Customer"]),
            //        //                         Mob = Convert.ToString(dr["MobileNo"]),
            //        //                         EMail = Convert.ToString(dr["EmailId"]),
            //        //                         Country = Convert.ToString(dr["Country"]),
            //        //                         State = Convert.ToString(dr["State"]),
            //        //                         City = Convert.ToString(dr["City"]),
            //        //                         FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
            //        //                         Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
            //        //                         LeadStatus = Convert.ToString(dr["LeadStatus"]),
            //        //                         LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //        //                         AssignTo = Convert.ToString(dr["AssignTo"]),
            //        //                         CreatedDate = Convert.ToString(dr["Date"]),
            //        //                     }).ToList();


            //        var AssignToOthers = /*GetAssignedRecords.Rows.Count == 0 ? 0 :*/ GetAssignedRecords.Rows.Count;
            //        ViewBag.AssignToOthers = AssignToOthers;
            //        //}

            //        #endregion








            //        #region daily lead report on chart

            //        var dateList = new List<DateTime>();
            //        var chartData = new List<LeadWeeklyChartModel>();
            //        var dateString = new List<LeadCategory>();

            //        if (LeadRecords.Count > 0)
            //        {
            //            foreach (var item in LeadRecords.OrderBy(a => a.LeadDate))
            //            {
            //                //check for same date not add
            //                if (dateList.Count > 0)
            //                {
            //                    if (!dateList.Any(a => a.Date == item.LeadDate.Date))
            //                    {
            //                        dateList.Add(item.LeadDate.Date);
            //                    }
            //                }
            //                else
            //                {
            //                    dateList.Add(item.LeadDate.Date);
            //                }

            //                if (dateString.Count > 0)
            //                {
            //                    if (!dateString.Any(a => a.Label.Equals(item.LeadDate.Date.ToString(VLM.DateFormat))))
            //                    {
            //                        dateString.Add(new LeadCategory { Label = item.LeadDate.Date.ToString(VLM.DateFormat) });
            //                    }
            //                }
            //                else
            //                {
            //                    dateString.Add(new LeadCategory { Label = Convert.ToDateTime(item.LeadDate).Date.ToString(VLM.DateFormat) });
            //                }

            //            }

            //        }
            //        else
            //        {
            //            // VLM.FilterDate = FilterDate;
            //            dateList = Enumerable.Range(0, 5).Select(i => dd.Date.AddDays(-i)).ToList();

            //            foreach (var day in dateList.OrderBy(a => a.Date))
            //            {
            //                dateString.Add(new LeadCategory { Label = day.ToString(VLM.DateFormat) });

            //                //Console.WriteLine($"{day:yyyy-MM-dd}");
            //            }
            //        }

            //        if (Convert.ToInt32(Session["CompanyID"]) != 2066 && Convert.ToInt32(Session["BranchID"]) != 1979)
            //        {
            //            // Create 4 series of the 3Dcolumn chart view type.
            //            var series1 = new LeadWeeklyChartModel { Seriesname = "New Leads", Data = new List<LeadData> { } };
            //            var series2 = new LeadWeeklyChartModel { Seriesname = "Follow-Ups", Data = new List<LeadData> { } };
            //            var series3 = new LeadWeeklyChartModel { Seriesname = "Missed Follow-Ups", Data = new List<LeadData> { } };
            //            var series4 = new LeadWeeklyChartModel { Seriesname = "Closed", Data = new List<LeadData> { } };

            //            foreach (var dt in dateList.OrderBy(a => a.Date))
            //            {
            //                //new lead count

            //                var newLead = LeadRecords.Where(em => em.LeadStatusName.Equals("NewLead") && em.LeadDate.Date >= dt.Date && em.LeadDate.Date.Date <= dt.Date).ToList().Count();
            //                series1.Data.Add(new LeadData { Value = newLead });


            //                int mLead = 0;
            //                int fLead = 0;

            //                fLead = LeadRecords.Where(a => a.LeadStatusName.Equals("Follow") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList().Count();
            //                mLead = LeadRecords.Where(a => a.LeadStatusName.Equals("Missed") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList().Count();

            //                //follow-up lead data add
            //                series2.Data.Add(new LeadData { Value = fLead });

            //                //missed follow-up lead data add
            //                series3.Data.Add(new LeadData { Value = mLead });


            //                var closedLead = LeadRecords.Where(em => em.LeadStatus.Equals("Closed") && (em.ModifiedDate != "" && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date >= dt.Date && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date <= dt.Date)).ToList().Count();

            //                series4.Data.Add(new LeadData { Value = closedLead });

            //            }
            //            // Add the series to the chart.
            //            chartData.AddRange(new LeadWeeklyChartModel[] { series1, series2, series3, series4 });

            //            //convert to json format dates list
            //            ViewBag.Dates = JsonConvert.SerializeObject(dateString);

            //            //convert to json format data
            //            ViewBag.WeeklyDataList = JsonConvert.SerializeObject(chartData);
            //        }
            //        else
            //        {
            //            var series1 = new LeadWeeklyChartModel { Seriesname = "Open", Data = new List<LeadData> { } };
            //            var series2 = new LeadWeeklyChartModel { Seriesname = "Followup", Data = new List<LeadData> { } };
            //            var series3 = new LeadWeeklyChartModel { Seriesname = "Not Interested", Data = new List<LeadData> { } };
            //            var series4 = new LeadWeeklyChartModel { Seriesname = "Free Trail", Data = new List<LeadData> { } };

            //            foreach (var dt in dateList.OrderBy(a => a.Date))
            //            {
            //                //new lead count

            //                var newLead = LeadRecords.Where(em => em.LeadStatus.Equals("Open")).ToList().Count();
            //                series1.Data.Add(new LeadData { Value = newLead });


            //                int mLead = 0;
            //                int fLead = 0;

            //                fLead = LeadRecords.Where(a => a.LeadStatus.Equals("Followup") ).ToList().Count();
            //                mLead = LeadRecords.Where(a => a.LeadStatus.Equals("Not Interested")).ToList().Count();

            //                //follow-up lead data add
            //                series2.Data.Add(new LeadData { Value = fLead });

            //                //missed follow-up lead data add
            //                series3.Data.Add(new LeadData { Value = mLead });


            //                var closedLead = LeadRecords.Where(em => em.LeadStatus.Equals("Free Trail") && (em.ModifiedDate != "" && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date >= dt.Date && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date <= dt.Date)).ToList().Count();

            //                series4.Data.Add(new LeadData { Value = closedLead });

            //            }
            //            // Add the series to the chart.
            //            chartData.AddRange(new LeadWeeklyChartModel[] { series1, series2, series3, series4 });

            //            //convert to json format dates list
            //            ViewBag.Dates = JsonConvert.SerializeObject(dateString);

            //            //convert to json format data
            //            ViewBag.WeeklyDataList = JsonConvert.SerializeObject(chartData);
            //        }



            //        #endregion


            //        #region Get-Today-FollowUp-Leads
            //        DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable(" call CRM_TodayFollowupLeads(" + BranchID + "," + CompanyID + "," + UID + ")");
            //        if (TodayFollowupLeads.Rows.Count > 0)
            //        {
            //            VLM.TodayFollowUpLeadsList = (from dr in TodayFollowupLeads.AsEnumerable()
            //                                          select new DashBoardLeadsModel()
            //                                          {
            //                                              Id = Convert.ToInt32(dr["Id"]),
            //                                              LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
            //                                              Phone = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
            //                                              Email = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
            //                                              Country = dr["CountryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CountryName"]),
            //                                              CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
            //                                              FollowUpDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
            //                                              LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
            //                                              LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                              AssignedBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                              AssignTo = dr["AssignTo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AssignTo"])
            //                                          }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();

            //            if (UID > 0)
            //            {
            //                List<DashBoardLeadsModel> assignList = VLM.TodayFollowUpLeadsList.Where(em => em.AssignTo == UID).ToList();
            //                VLM.TodayFollowUpLeadsList = VLM.TodayFollowUpLeadsList.Where(em => em.LeadOwner == UID).ToList();
            //                if (assignList.Count > 0)
            //                {
            //                    VLM.TodayFollowUpLeadsList.AddRange(assignList);
            //                }
            //            }
            //        }

            //        #endregion

            //        #region TodayNew-Leads
            //        DataTable GetTodayLeads = DataAccessLayer.GetDataTable(" call CRM_TodayLeads(" + BranchID + "," + CompanyID + "," + UID + ")");
            //        if (GetTodayLeads.Rows.Count > 0)
            //        {
            //            VLM.TodayNewLeadsList = (from dr in GetTodayLeads.AsEnumerable()
            //                                     select new DashBoardLeadsModel()
            //                                     {
            //                                         Id = Convert.ToInt32(dr["Id"]),
            //                                         LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
            //                                         Phone = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
            //                                         Email = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
            //                                         Country = dr["CountryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CountryName"]),
            //                                         CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
            //                                         FollowUpDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
            //                                         LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                         LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
            //                                         AssignTo = Convert.ToInt32((Convert.ToString(dr["AssignTo"]) == null || Convert.ToString(dr["AssignTo"]) == "") ? 0 : dr["AssignTo"]),
            //                                         AssignedBy = dr["AssinedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssinedBy"])
            //                                     }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();

            //            if (UID > 0)
            //            {
            //                List<DashBoardLeadsModel> assignList = VLM.TodayNewLeadsList.Where(em => em.AssignTo == UID).ToList();
            //                VLM.TodayNewLeadsList = VLM.TodayNewLeadsList.Where(em => em.LeadOwner == UID).ToList();
            //                if (assignList.Count > 0)
            //                {
            //                    VLM.TodayNewLeadsList.AddRange(assignList);
            //                }
            //            }

            //        }
            //        #endregion




            //    }

            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendExcepToDB(ex);
            //}

            //return View(VLM);

            ViewLeadsModel VLM = new ViewLeadsModel();
            //VLM.LeadDate = Constant.GetBharatTime();
            try
            {

                string language = cr.GetCompanyLanguage(0);//get company Language
                VLM.Language = language;
                if (Session["UID"] != null)
                {

                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    int LoggedBranchId = Convert.ToInt32(Session["BranchID"]);
                    int LoggedCompanyId = Convert.ToInt32(Session["CompanyID"]);
                    int LoggedUID = Convert.ToInt32(Session["UID"]);

                    var LeadRecords = new List<ViewLeadsModel>();


                    var UID = 0;
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        UID = 0;
                        if (!string.IsNullOrEmpty(UserddlName) && UserddlName != "0")
                        {
                            UID = Convert.ToInt32(UserddlName);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UserddlName) && UserddlName != "0")
                        {
                            UID = Convert.ToInt32(UserddlName);
                        }
                        else
                        {
                            UID = Convert.ToInt32(Session["UID"]);
                        }
                    }

                    if (Session["UserddlName"] == null)
                    {
                        VLM.UserddlName = Convert.ToString(UID);
                        UserddlName = Convert.ToString(UID);
                    }
                    else
                    {
                        VLM.UserddlName = Convert.ToString(Session["UserddlName"]);
                        UserddlName = Convert.ToString(Session["UserddlName"]);
                    }

                    if (!string.IsNullOrEmpty(UserddlName))
                    {
                        var data = cr.GetUserCompanyBranch(UID);
                        if (data != null)
                        {
                            BranchID = data.BranchID;
                            CompanyID = data.CompanyID;
                        }
                    }

                    #region Data-time-Formate
                    VLM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    var dd = Constant.GetBharatTime();//get india datetime

                    //DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);

                    VLM.TodayDate = dd.ToString("dd/MM/yyyy");//get today date for today followup lead link

                    DateTime monthStartDate = dd.AddDays(-5);
                    DateTime MonthendDate = new DateTime(dd.Year, dd.Month, dd.Day);

                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    #region To-CheckFilter-Date
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            TempData["Fromdate"] = FromDate;
                            TempData["Todate"] = ToDate;

                            var fmDate = DateTime.ParseExact(FromDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, VLM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            TempData["Fromdate"] = FromDate;
                            TempData["Todate"] = ToDate;
                            MStartDate = FromDate;
                            MEndDate = ToDate;
                        }
                    }
                    else
                    {
                        if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            TempData["Fromdate"] = monthStartDate.ToString(VLM.DateFormat);
                            TempData["Todate"] = MonthendDate.ToString(VLM.DateFormat);
                        }
                        else
                        {
                            TempData["Fromdate"] = MStartDate;
                            TempData["Todate"] = MEndDate;
                        }
                    }
                    #endregion

                    #endregion


                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        #region Mapped User
                        string userquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName 
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + LoggedBranchId + " and ur.CompanyID = " + LoggedCompanyId + " and ur.Status = 1 and rl.ViewLeads = 1";
                        var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                        // var q = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName == "Sales" && em.Status == true).OrderBy(em => em.Fname).ToList();
                        VLM.Userddllist = new List<Userddl>();

                        if (q != null && q.Count > 0)
                        {
                            Userddl u = new Userddl();
                            u.UserName = "All";
                            //Userddl u1 = new Userddl();
                            VLM.Userddllist.Add(u);
                            foreach (var item in q)
                            {
                                var user = new Userddl
                                {
                                    uid = Convert.ToInt32(item.UserID),
                                    //UserName = item.Fname + " " + item.Lname
                                    UserName = item.UserName
                                };
                                VLM.Userddllist.Add(user);
                            }
                        }

                        #endregion

                        #region Get other branch user list to add mapped user list

                        DataTable GetOtherMappedRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + LoggedBranchId + "','','')");
                        if (GetOtherMappedRecords.Rows.Count > 0)
                        {
                            var GetOtherBranchUserList = (from dr in GetOtherMappedRecords.AsEnumerable()
                                                          select new Userddl()
                                                          {
                                                              uid = Convert.ToInt32(dr["AssignedUserID"]),
                                                              UserName = Convert.ToString(dr["UserName"] + " (Branch: " + dr["BranchName"] + ")"),
                                                              IsActive = Convert.ToBoolean(dr["IsActive"])
                                                          }).Where(a => a.IsActive == true).OrderBy(a => a.UserName).ToList();
                            VLM.Userddllist.AddRange(GetOtherBranchUserList);

                            VLM.Userddllist = VLM.Userddllist.OrderBy(a => a.UserName).ToList();
                        }

                        #endregion
                    }
                    else
                    {
                        #region Get-MappedUser-Parents
                        var GetUserData = db.crm_usertbl.Where(em => em.Id == LoggedUID && em.BranchID == LoggedBranchId && em.CompanyID == LoggedCompanyId).FirstOrDefault();
                        if (GetUserData != null && GetUserData.MappedUsers != null)
                        {
                            VLM.MappedUser = GetUserData.MappedUsers.ToString();
                            var GetMapUser = GetUserData.MappedUsers.Split(',');
                            VLM.Userddllist = new List<Userddl>();

                            if (GetMapUser != null && GetMapUser.Count() > 0)
                            {

                                VLM.Userddllist.Add(new Userddl { UserName = "All" });

                                string unm = Session["UserName"].ToString();
                                VLM.Userddllist.Add(new Userddl { uid = UID, UserName = unm });

                                foreach (var item in GetMapUser)
                                {
                                    var mapid = Convert.ToInt32(item);
                                    var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                    if (GetMapUserData != null)
                                    {
                                        var user = new Userddl
                                        {
                                            uid = mapid,
                                            UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
                                        };
                                        VLM.Userddllist.Add(user);
                                    }
                                }
                            }
                            else
                            {
                                Userddl u1 = new Userddl();
                                u1.uid = UID;
                                u1.UserName = Session["UserName"].ToString();
                                VLM.Userddllist.Add(u1);
                            }


                            //check other branch user mapped to current user
                            string query = @"select muob.MappedUserId as uid,CONCAT(u.FName ,' ', u.Lname, '(Branch: ',IFNULL(cb.BranchName,'N / A'),')') as UserName,aorg.IsActive
                                         from CRM_MappedUserOtherBranch muob
                                         join crm_assignedtootherorganization aorg on muob.MappedUserId = aorg.AssignedUserID
                                         join crm_usertbl u on u.Id = aorg.AssignedUserID
                                         join com_branches cb on u.BranchID = cb.OrgBranchCode
                                         join company_profile cp on cb.OrganizationID = cp.id
                                         Where muob.UserId = '" + LoggedUID + "' and aorg.AssignToCompanyID = '" + LoggedBranchId + "' and aorg.IsActive = 1 order by muob.CreateDate; ";
                            var data = db.Database.SqlQuery<Userddl>(query).ToList();
                            if (data.Count > 0)
                            {
                                VLM.OtherBranchMappedUser = data;

                                VLM.Userddllist.AddRange(data);
                            }
                        }
                        #endregion
                    }


                    #region New-Lead
                    DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetNewLeadSummaryReport('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetRecords.Rows.Count > 0)
                    {
                        var GetLeadsRecords = (from dr in GetRecords.AsEnumerable()
                                               select new ViewLeadsModel()
                                               {
                                                   Id = Convert.ToInt32(dr["ID"]),
                                                   LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                   AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                                   AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                                   AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                   LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                   AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                                   ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                                   LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                                   LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                               }).ToList();

                        LeadRecords.AddRange(GetLeadsRecords);

                    }

                    #endregion

                    #region Followup-Missed-Delayed lead

                    DataTable GetFMDRecords = DataAccessLayer.GetDataTable("call CRM_GetFollowupMissedDelayedLead('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetFMDRecords.Rows.Count > 0)
                    {
                        var GetFMDLeadsRecords = (from dr in GetFMDRecords.AsEnumerable()
                                                  select new ViewLeadsModel()
                                                  {
                                                      Id = Convert.ToInt32(dr["ID"]),
                                                      LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                      AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                                      AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                                      AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                      LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                      AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                                      ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                                      LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                                      LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                                  }).ToList();


                        LeadRecords.AddRange(GetFMDLeadsRecords);
                    }



                    DataTable GetFUpRecords = DataAccessLayer.GetDataTable("call CRM_GetLSReportByFollowupDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetFUpRecords.Rows.Count > 0)
                    {
                        var GetFUPLeadsRecords = (from dr in GetFUpRecords.AsEnumerable()
                                                  select new ViewLeadsModel()
                                                  {
                                                      Id = Convert.ToInt32(dr["ID"]),
                                                      LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                      AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                                      AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                                      AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                      LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                      AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                                      ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                                      LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                                      LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                                  }).ToList();



                        var FUPLead = GetFUPLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
                        LeadRecords.AddRange(FUPLead);

                    }

                    int followup = LeadRecords.Where(a => a.LeadStatusName.Equals("Follow")).ToList().Count();
                    int Missedfollowup = LeadRecords.Where(a => a.LeadStatusName.Equals("Missed")).ToList().Count();


                    ViewBag.FollowUp = followup;
                    ViewBag.MissedFollowUp = Missedfollowup;

                    int Delayedfollowup = LeadRecords.Where(a => a.LeadStatusName.Equals("Delayed")).ToList().Count();
                    ViewBag.Delayedfollowup = Delayedfollowup;

                    #endregion

                    #region New-Leads                    

                    var NewLeadsRecords = LeadRecords.Where(a => a.LeadStatusName.Equals("NewLead")).ToList().Count();
                    int newCount = Convert.ToInt32(NewLeadsRecords);
                    ViewBag.NewLeadsCount = newCount;

                    #endregion

                    #region Not-Interested
                    int notinterestCount = LeadRecords.Where(a => a.LeadStatus.Equals("Not Interested")).GroupBy(a => a.Id).ToList().Count();
                    ViewBag.NotInterestedCount = notinterestCount;
                    #endregion

                    #region Closed

                    int ClosedCount = LeadRecords.Where(a => a.LeadStatus.Equals("Closed")).GroupBy(a => a.Id).ToList().Count();

                    ViewBag.ClosedRecordsCount = ClosedCount;

                    #endregion

                    #region Suspect-Leads

                    int SuspectCount = LeadRecords.Where(a => a.LeadStatus.Equals("Suspect")).GroupBy(a => a.Id).ToList().Count();
                    ViewBag.SuspectLeadsCount = SuspectCount;
                    #endregion

                    #region Prospect-Leads                   
                    int prospectCount = LeadRecords.Where(a => a.LeadStatus.Equals("Prospect")).GroupBy(a => a.Id).ToList().Count();
                    ViewBag.ProspectCount = prospectCount;

                    #endregion

                    #region Get-AssignToOthersLeads Info

                    //Collect the Manually Created-Leads
                    DataTable GetAssignedRecords = DataAccessLayer.GetDataTable(" call CRM_GetAssignToOtherLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    //if (GetAssignedRecords.Rows.Count > 0)
                    //{
                    //var AssignedRecord = (from dr in GetAssignedRecords.AsEnumerable()
                    //                     select new ViewLeadsModel()
                    //                     {
                    //                         Id = Convert.ToInt32(dr["Id"]),
                    //                         LeadName = Convert.ToString(dr["Customer"]),
                    //                         Mob = Convert.ToString(dr["MobileNo"]),
                    //                         EMail = Convert.ToString(dr["EmailId"]),
                    //                         Country = Convert.ToString(dr["Country"]),
                    //                         State = Convert.ToString(dr["State"]),
                    //                         City = Convert.ToString(dr["City"]),
                    //                         FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
                    //                         Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                    //                         LeadStatus = Convert.ToString(dr["LeadStatus"]),
                    //                         LeadOwner = Convert.ToString(dr["LeadOwner"]),
                    //                         AssignTo = Convert.ToString(dr["AssignTo"]),
                    //                         CreatedDate = Convert.ToString(dr["Date"]),
                    //                     }).ToList();


                    var AssignToOthers = /*GetAssignedRecords.Rows.Count == 0 ? 0 :*/ GetAssignedRecords.Rows.Count;
                    ViewBag.AssignToOthers = AssignToOthers;
                    //}



                    #endregion


                    #region pie chart data
                    var cData = new List<PieChartModel>();

                    cData.Add(new PieChartModel("New Leads", newCount));
                    cData.Add(new PieChartModel("Follow Ups", followup));
                    cData.Add(new PieChartModel("Missed Follow Ups", Missedfollowup));
                    cData.Add(new PieChartModel("Delayed Follow Ups", Delayedfollowup));
                    cData.Add(new PieChartModel("Not Interested", notinterestCount));
                    cData.Add(new PieChartModel("Closed Leads", ClosedCount));
                    cData.Add(new PieChartModel("Suspect Leads", SuspectCount));
                    cData.Add(new PieChartModel("Prospect Leads", prospectCount));

                    //convert to json format data
                    ViewBag.DataList = JsonConvert.SerializeObject(cData);
                    #endregion


                    #region daily lead report on chart

                    var dateList = new List<DateTime>();
                    var chartData = new List<LeadWeeklyChartModel>();
                    var dateString = new List<LeadCategory>();

                    if (LeadRecords.Count > 0)
                    {
                        foreach (var item in LeadRecords.OrderBy(a => a.LeadDate))
                        {
                            //check for same date not add
                            if (dateList.Count > 0)
                            {
                                if (!dateList.Any(a => a.Date == item.LeadDate.Date))
                                {
                                    dateList.Add(item.LeadDate.Date);
                                }
                            }
                            else
                            {
                                dateList.Add(item.LeadDate.Date);
                            }

                            if (dateString.Count > 0)
                            {
                                if (!dateString.Any(a => a.Label.Equals(item.LeadDate.Date.ToString(VLM.DateFormat))))
                                {
                                    dateString.Add(new LeadCategory { Label = item.LeadDate.Date.ToString(VLM.DateFormat) });
                                }
                            }
                            else
                            {
                                dateString.Add(new LeadCategory { Label = Convert.ToDateTime(item.LeadDate).Date.ToString(VLM.DateFormat) });
                            }

                        }

                    }
                    else
                    {
                        // VLM.FilterDate = FilterDate;
                        dateList = Enumerable.Range(0, 5).Select(i => dd.Date.AddDays(-i)).ToList();

                        foreach (var day in dateList.OrderBy(a => a.Date))
                        {
                            dateString.Add(new LeadCategory { Label = day.ToString(VLM.DateFormat) });

                            //Console.WriteLine($"{day:yyyy-MM-dd}");
                        }
                    }


                    // Create 4 series of the 3Dcolumn chart view type.
                    var series1 = new LeadWeeklyChartModel { Seriesname = "New Leads", Data = new List<LeadData> { } };
                    var series2 = new LeadWeeklyChartModel { Seriesname = "Follow-Ups", Data = new List<LeadData> { } };
                    var series3 = new LeadWeeklyChartModel { Seriesname = "Missed Follow-Ups", Data = new List<LeadData> { } };
                    var series4 = new LeadWeeklyChartModel { Seriesname = "Closed", Data = new List<LeadData> { } };

                    foreach (var dt in dateList.OrderBy(a => a.Date))
                    {
                        //new lead count

                        var newLead = LeadRecords.Where(em => em.LeadStatusName.Equals("NewLead") && em.LeadDate.Date >= dt.Date && em.LeadDate.Date.Date <= dt.Date).ToList().Count();
                        series1.Data.Add(new LeadData { Value = newLead });


                        int mLead = 0;
                        int fLead = 0;

                        fLead = LeadRecords.Where(a => a.LeadStatusName.Equals("Follow") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList().Count();
                        mLead = LeadRecords.Where(a => a.LeadStatusName.Equals("Missed") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList().Count();

                        //follow-up lead data add
                        series2.Data.Add(new LeadData { Value = fLead });

                        //missed follow-up lead data add
                        series3.Data.Add(new LeadData { Value = mLead });


                        var closedLead = LeadRecords.Where(em => em.LeadStatus.Equals("Closed") && (em.ModifiedDate != "" && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date >= dt.Date && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date <= dt.Date)).ToList().Count();

                        series4.Data.Add(new LeadData { Value = closedLead });

                    }

                    // Add the series to the chart.
                    chartData.AddRange(new LeadWeeklyChartModel[] { series1, series2, series3, series4 });

                    //convert to json format dates list
                    ViewBag.Dates = JsonConvert.SerializeObject(dateString);

                    //convert to json format data
                    ViewBag.WeeklyDataList = JsonConvert.SerializeObject(chartData);

                    #endregion


                    #region Get-Today-FollowUp-Leads
                    DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable(" call CRM_TodayFollowupLeads(" + BranchID + "," + CompanyID + "," + UID + ")");
                    if (TodayFollowupLeads.Rows.Count > 0)
                    {
                        VLM.TodayFollowUpLeadsList = (from dr in TodayFollowupLeads.AsEnumerable()
                                                      select new DashBoardLeadsModel()
                                                      {
                                                          Id = Convert.ToInt32(dr["Id"]),
                                                          LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
                                                          Phone = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
                                                          Email = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
                                                          Country = dr["CountryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CountryName"]),
                                                          CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                          FollowUpDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                          LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
                                                          LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                          AssignedBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                          AssignTo = dr["AssignTo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AssignTo"])
                                                      }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();

                        if (UID > 0)
                        {
                            List<DashBoardLeadsModel> assignList = VLM.TodayFollowUpLeadsList.Where(em => em.AssignTo == UID).ToList();
                            VLM.TodayFollowUpLeadsList = VLM.TodayFollowUpLeadsList.Where(em => em.LeadOwner == UID).ToList();
                            if (assignList.Count > 0)
                            {
                                VLM.TodayFollowUpLeadsList.AddRange(assignList);
                            }
                        }
                    }

                    #endregion

                    #region TodayNew-Leads
                    DataTable GetTodayLeads = DataAccessLayer.GetDataTable(" call CRM_TodayLeads(" + BranchID + "," + CompanyID + "," + UID + ")");
                    if (GetTodayLeads.Rows.Count > 0)
                    {
                        VLM.TodayNewLeadsList = (from dr in GetTodayLeads.AsEnumerable()
                                                 select new DashBoardLeadsModel()
                                                 {
                                                     Id = Convert.ToInt32(dr["Id"]),
                                                     LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
                                                     Phone = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
                                                     Email = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
                                                     Country = dr["CountryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CountryName"]),
                                                     CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                     FollowUpDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                     LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                     LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
                                                     AssignTo = Convert.ToInt32((Convert.ToString(dr["AssignTo"]) == null || Convert.ToString(dr["AssignTo"]) == "") ? 0 : dr["AssignTo"]),
                                                     AssignedBy = dr["AssinedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssinedBy"])
                                                 }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();

                        if (UID > 0)
                        {
                            List<DashBoardLeadsModel> assignList = VLM.TodayNewLeadsList.Where(em => em.AssignTo == UID).ToList();
                            VLM.TodayNewLeadsList = VLM.TodayNewLeadsList.Where(em => em.LeadOwner == UID).ToList();
                            if (assignList.Count > 0)
                            {
                                VLM.TodayNewLeadsList.AddRange(assignList);
                            }
                        }

                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }

            return View(VLM);
        }

        public async Task<ActionResult> ViewBookinglist()
        {
            CreateLeadsModel DModel = new CreateLeadsModel();
            try
            {
                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                List<crm_createleadstbl> bookingList = await db.crm_createleadstbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ExtraCol9 != null).ToListAsync();
                DModel.createlist = bookingList;
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ViewBookinglistforpersonal", DModel);
        }
        public JsonResult Bindbookdate()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            List<bindbookingdate> data = db.crm_createleadstbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.ExtraCol9 != null)
                                        .Select(em => new bindbookingdate
                                        {
                                            extracol9 = em.ExtraCol9.Value.ToString(),

                                        }).OrderBy(a => a.extracol9).ToList();
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.extracol9))
                {
                    DateTime dt = DateTime.Parse(item.extracol9);
                    item.extracol9 = dt.ToString("MM/dd/yyyy");
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookingReport(string id = "")
        {
            CreateLeadsModel DModel = new CreateLeadsModel();
            try
            {
                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                DModel.CompanyNodaTimeZone = cr.GetCompanyNodaTimeZone(CompanyID);//get company noda time zone name
                DModel.DateFormat = Constant.DateFormat();//get date format by company id
                                                          // CLM.Language = cr.GetCompanyLanguage(0);
                Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                List<crm_createleadstbl> bookingList = db.crm_createleadstbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ExtraCol9 != null).ToList();
                DModel.createlist = bookingList;
            }
            catch (Exception ex)
            {

            }
            return View(DModel);
        }

        public ActionResult ViewLeadReportInfo(string FilterText, string FromDate, string ToDate, string UserddlName, string CDate = "")
        {
            //ViewLeadsModel VLM = new ViewLeadsModel();
            //try
            //{
            //    int BranchID = Convert.ToInt32(Session["BranchID"]);
            //    int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            //    var LeadRecords = new List<ViewLeadsModel>();

            //    VLM.DateFormat = Constant.DateFormat();//get date format by company id

            //    var MStartDate = string.Empty;
            //    var MEndDate = string.Empty;

            //    if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
            //    {

            //        var fmDate = DateTime.ParseExact(FromDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
            //        var tDate = DateTime.ParseExact(ToDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

            //        MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
            //        MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
            //    }
            //    else
            //    {
            //        MStartDate = FromDate;
            //        MEndDate = ToDate;
            //    }


            //    var UID = 0;
            //    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            //    {
            //        UID = 0;
            //        if (UserddlName != null && UserddlName != "0")
            //        {
            //            UID = Convert.ToInt32(UserddlName);
            //        }
            //    }
            //    else
            //    {
            //        if (UserddlName != null && UserddlName != "0")
            //        {
            //            UID = Convert.ToInt32(UserddlName);
            //        }
            //        else
            //        {
            //            UID = Convert.ToInt32(Session["UID"]);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(UserddlName))
            //    {
            //        var data = cr.GetUserCompanyBranch(UID);
            //        if (data != null)
            //        {
            //            BranchID = data.BranchID;
            //            CompanyID = data.CompanyID;
            //        }
            //    }


            //    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetNewLeadSummaryReport('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //    if (GetRecords.Rows.Count > 0)
            //    {
            //        var GetLeadsRecords = (from dr in GetRecords.AsEnumerable()
            //                               select new ViewLeadsModel()
            //                               {
            //                                   Id = Convert.ToInt32(dr["ID"]),
            //                                   LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
            //                                   Mob = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
            //                                   EMail = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
            //                                   //Country = dr["Country"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Country"]),
            //                                   //State = dr["State"] == DBNull.Value ? string.Empty : Convert.ToString(dr["State"]),
            //                                   City = dr["City"] == DBNull.Value ? string.Empty : Convert.ToString(dr["City"]),
            //                                   //Date = dr["Date"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Date"]),
            //                                   FollowupDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
            //                                   //Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),

            //                                   LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                   //AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                   //AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                   //AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                   //LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                   //AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                   ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                   LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                   LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                               }).ToList();

            //        LeadRecords.AddRange(GetLeadsRecords);

            //    }


            //    #region Followup-Missed-Delayed lead
            //    if (Convert.ToInt32(Session["CompanyID"]) != 2066 && Convert.ToInt32(Session["BranchID"]) != 1979)
            //    {
            //        DataTable GetFMDRecords = DataAccessLayer.GetDataTable("call CRM_GetFollowupMissedDelayedLead('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //        if (GetFMDRecords.Rows.Count > 0)
            //        {
            //            var GetFMDLeadsRecords = (from dr in GetFMDRecords.AsEnumerable()
            //                                      select new ViewLeadsModel()
            //                                      {
            //                                          Id = Convert.ToInt32(dr["ID"]),
            //                                          LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
            //                                          Mob = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
            //                                          EMail = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
            //                                          City = dr["City"] == DBNull.Value ? string.Empty : Convert.ToString(dr["City"]),
            //                                          LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                          FollowupDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
            //                                          //AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                          //AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                          //AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                          //LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                          //AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                          ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                          LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                          LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                      }).ToList();
            //            LeadRecords.AddRange(GetFMDLeadsRecords);

            //            //if (GetFMDLeadsRecords.Count > 0)
            //            //{
            //            //    //var FMDLead = GetFMDLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
            //            //    //var FMDLead = GetFMDLeadsRecords.Where(x => LeadRecords.Any(y => y.Id == x.Id && y.LeadStatusName != "NewLead")).ToList();
            //            //}
            //        }
            //    }

            //    DataTable GetFUpRecords = DataAccessLayer.GetDataTable("call CRM_GetLSReportByFollowupDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //    if (GetFUpRecords.Rows.Count > 0)
            //    {
            //        var GetFUPLeadsRecords = (from dr in GetFUpRecords.AsEnumerable()
            //                                  select new ViewLeadsModel()
            //                                  {
            //                                      Id = Convert.ToInt32(dr["ID"]),
            //                                      LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
            //                                      Mob = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
            //                                      EMail = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
            //                                      City = dr["City"] == DBNull.Value ? string.Empty : Convert.ToString(dr["City"]),
            //                                      LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
            //                                      FollowupDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
            //                                      //AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
            //                                      //AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
            //                                      //AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
            //                                      //LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                      //AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
            //                                      ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
            //                                      LeadDate = Convert.ToDateTime(dr["LeadDate"]),
            //                                      LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
            //                                  }).ToList();



            //        var FUPLead = GetFUPLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
            //        LeadRecords.AddRange(FUPLead);
            //    }
            //    #endregion

            //    if (!string.IsNullOrEmpty(CDate))
            //    {
            //        var dt = Convert.ToDateTime(CDate);
            //        if (FilterText == "New Leads")
            //        {
            //            VLM.viewleadsList = LeadRecords.Where(em => em.LeadStatusName.Equals("NewLead") && em.LeadDate.Date >= dt.Date && em.LeadDate.Date.Date <= dt.Date).ToList();
            //        }
            //        else if (FilterText == "Follow-Up")
            //        {
            //            VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatusName.Equals("Follow") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList();

            //        }
            //        else if (FilterText == "Missed Follow-Ups")
            //        {
            //            VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatusName.Equals("Missed") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList();

            //        }
            //        else if (FilterText == "Closed")
            //        {
            //            //VLM.viewleadsList = LeadRecords.Where(em => em.LeadStatus.Equals("Closed") && (em.ModifiedDate != "" && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date >= dt.Date && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date <= dt.Date)).ToList();

            //            var groupedData = LeadRecords.Where(em => em.LeadStatus.Equals("Closed") && (em.ModifiedDate != "" && Convert.ToDateTime(string.Format("{0:" + VLM.DateFormat + "}", em.ModifiedDate)).Date >= dt.Date && Convert.ToDateTime(string.Format("{0:" + VLM.DateFormat + "}", em.ModifiedDate)).Date <= dt.Date)).GroupBy(a => a.Id).ToList();

            //            foreach (var gd in groupedData)
            //            {
            //                foreach (var item in gd)
            //                {
            //                    var vl = new ViewLeadsModel
            //                    {
            //                        Id = item.Id,
            //                        LeadName = item.LeadName,
            //                        Mob = item.Mob,
            //                        EMail = item.EMail,
            //                        City = item.City,
            //                        LeadStatus = item.LeadStatus,
            //                        FollowupDate = item.FollowupDate,
            //                        ModifiedDate = item.ModifiedDate,
            //                        LeadDate = item.LeadDate,
            //                        LeadStatusName = item.LeadStatusName
            //                    };

            //                    if (VLM.viewleadsList.Count > 0)
            //                    {
            //                        if (!VLM.viewleadsList.Any(a => a.Id == item.Id))
            //                        {
            //                            VLM.viewleadsList.Add(vl);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        VLM.viewleadsList.Add(vl);
            //                    }
            //                }

            //            }

            //        }
            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(Session["CompanyID"]) != 2066 && Convert.ToInt32(Session["BranchID"]) != 1979)
            //        {
            //            if (FilterText == "NewLead" || FilterText == "Follow" || FilterText == "Missed" || FilterText == "Delayed")
            //            {
            //                VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatusName.Equals(FilterText)).ToList();
            //            }
            //            else if (FilterText == "Not Interested" || FilterText == "Closed" || FilterText == "Suspect" || FilterText == "Prospect")
            //            {
            //                //VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatus.ToLower() == FilterText.ToLower()).ToList();

            //                var groupedData = LeadRecords.Where(a => a.LeadStatus.ToLower() == FilterText.ToLower()).GroupBy(a => a.Id).ToList();

            //                //var vlList = new List<ViewLeadsModel>();

            //                foreach (var gd in groupedData)
            //                {
            //                    foreach (var item in gd)
            //                    {
            //                        var vl = new ViewLeadsModel
            //                        {
            //                            Id = item.Id,
            //                            LeadName = item.LeadName,
            //                            Mob = item.Mob,
            //                            EMail = item.EMail,
            //                            City = item.City,
            //                            LeadStatus = item.LeadStatus,
            //                            FollowupDate = item.FollowupDate,
            //                            ModifiedDate = item.ModifiedDate,
            //                            LeadDate = item.LeadDate,
            //                            LeadStatusName = item.LeadStatusName
            //                        };

            //                        //vlList.Add(vl);
            //                        if (VLM.viewleadsList.Count > 0)
            //                        {
            //                            if (!VLM.viewleadsList.Any(a => a.Id == item.Id))
            //                            {
            //                                VLM.viewleadsList.Add(vl);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            VLM.viewleadsList.Add(vl);
            //                        }
            //                    }

            //                }


            //            }


            //            else if (FilterText == "AssignToOthers")
            //            {
            //                #region Get-AssignToOthersLeads Info
            //                //Collect the Manually Created-Leads
            //                DataTable GetAssignedRecords = DataAccessLayer.GetDataTable(" call CRM_GetAssignToOtherLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
            //                if (GetAssignedRecords.Rows.Count > 0)
            //                {
            //                    VLM.viewleadsList = (from dr in GetAssignedRecords.AsEnumerable()
            //                                         select new ViewLeadsModel()
            //                                         {
            //                                             Id = Convert.ToInt32(dr["Id"]),
            //                                             LeadName = Convert.ToString(dr["Customer"]),
            //                                             Mob = Convert.ToString(dr["MobileNo"]),
            //                                             EMail = Convert.ToString(dr["EmailId"]),
            //                                             Country = Convert.ToString(dr["Country"]),
            //                                             State = Convert.ToString(dr["State"]),
            //                                             City = Convert.ToString(dr["City"]),
            //                                             FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
            //                                             Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
            //                                             LeadStatus = Convert.ToString(dr["LeadStatus"]),
            //                                             LeadOwner = Convert.ToString(dr["LeadOwner"]),
            //                                             AssignTo = Convert.ToString(dr["AssignTo"]),
            //                                             CreatedDate = Convert.ToString(dr["Date"]),
            //                                         }).ToList();

            //                }

            //                #endregion
            //            }
            //        }
            //        else
            //        {
            //            var getotherleads = db.crm_createleadstbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID).ToList();
            //            if (FilterText == "Open" || FilterText == "Lead List" || FilterText == "Followup" || FilterText == "Not Interested" || FilterText == "Free Trail")
            //            {
            //                VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatus.Equals(FilterText)).ToList();
            //            }
            //            else if (FilterText == "Request List" || FilterText == "Subscribe" || FilterText == "Archive" || FilterText == "Closed")
            //            {
            //                //VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatus.ToLower() == FilterText.ToLower()).ToList();

            //                var groupedData = LeadRecords.Where(a => a.LeadStatus.ToLower() == FilterText.ToLower()).GroupBy(a => a.Id).ToList();

            //                //var vlList = new List<ViewLeadsModel>();

            //                foreach (var gd in groupedData)
            //                {
            //                    foreach (var item in gd)
            //                    {
            //                        var vl = new ViewLeadsModel
            //                        {
            //                            Id = item.Id,
            //                            LeadName = item.LeadName,
            //                            Mob = item.Mob,
            //                            EMail = item.EMail,
            //                            City = item.City,
            //                            LeadStatus = item.LeadStatus,
            //                            FollowupDate = item.FollowupDate,
            //                            ModifiedDate = item.ModifiedDate,
            //                            LeadDate = item.LeadDate,
            //                            LeadStatusName = item.LeadStatusName
            //                        };

            //                        //vlList.Add(vl);
            //                        if (VLM.viewleadsList.Count > 0)
            //                        {
            //                            if (!VLM.viewleadsList.Any(a => a.Id == item.Id))
            //                            {
            //                                VLM.viewleadsList.Add(vl);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            VLM.viewleadsList.Add(vl);
            //                        }
            //                    }

            //                }


            //            }
            //        }

            //    }


            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendExcepToDB(ex);
            //}
            //return PartialView("_LeadReportInfoList", VLM);
            ViewLeadsModel VLM = new ViewLeadsModel();
            try
            {
                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var LeadRecords = new List<ViewLeadsModel>();

                VLM.DateFormat = Constant.DateFormat();//get date format by company id

                var MStartDate = string.Empty;
                var MEndDate = string.Empty;

                if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {

                    var fmDate = DateTime.ParseExact(FromDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                    var tDate = DateTime.ParseExact(ToDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                }
                else
                {
                    MStartDate = FromDate;
                    MEndDate = ToDate;
                }

                if (UserddlName == "")
                {
                    UserddlName = "0";
                }
                var UID = 0;
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    UID = 0;
                    if (UserddlName != null && UserddlName != "0")
                    {
                        UID = Convert.ToInt32(UserddlName);
                    }
                }
                else
                {
                    if (UserddlName != null && UserddlName != "0")
                    {
                        UID = Convert.ToInt32(UserddlName);
                    }
                    else
                    {
                        UID = Convert.ToInt32(Session["UID"]);
                    }
                }

                if (!string.IsNullOrEmpty(UserddlName))
                {
                    var data = cr.GetUserCompanyBranch(UID);
                    if (data != null)
                    {
                        BranchID = data.BranchID;
                        CompanyID = data.CompanyID;
                    }
                }


                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetNewLeadSummaryReport('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetRecords.Rows.Count > 0)
                {
                    var GetLeadsRecords = (from dr in GetRecords.AsEnumerable()
                                           select new ViewLeadsModel()
                                           {
                                               Id = Convert.ToInt32(dr["ID"]),
                                               LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
                                               Mob = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
                                               EMail = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
                                               //Country = dr["Country"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Country"]),
                                               //State = dr["State"] == DBNull.Value ? string.Empty : Convert.ToString(dr["State"]),
                                               City = dr["City"] == DBNull.Value ? string.Empty : Convert.ToString(dr["City"]),
                                               //Date = dr["Date"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Date"]),
                                               FollowupDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
                                               //Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                               LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                               //AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                               //AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                               //AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                               //LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                               //AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                               ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                               LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                               LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                           }).ToList();

                    LeadRecords.AddRange(GetLeadsRecords);

                }


                #region Followup-Missed-Delayed lead

                DataTable GetFMDRecords = DataAccessLayer.GetDataTable("call CRM_GetFollowupMissedDelayedLead('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetFMDRecords.Rows.Count > 0)
                {
                    var GetFMDLeadsRecords = (from dr in GetFMDRecords.AsEnumerable()
                                              select new ViewLeadsModel()
                                              {
                                                  Id = Convert.ToInt32(dr["ID"]),
                                                  LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
                                                  Mob = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
                                                  EMail = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
                                                  City = dr["City"] == DBNull.Value ? string.Empty : Convert.ToString(dr["City"]),
                                                  LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                  FollowupDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
                                                  //AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                                  //AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                                  //AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                  //LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                  //AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                                  ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                                  LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                                  LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                              }).ToList();
                    LeadRecords.AddRange(GetFMDLeadsRecords);

                    //if (GetFMDLeadsRecords.Count > 0)
                    //{
                    //    //var FMDLead = GetFMDLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
                    //    //var FMDLead = GetFMDLeadsRecords.Where(x => LeadRecords.Any(y => y.Id == x.Id && y.LeadStatusName != "NewLead")).ToList();
                    //}
                }

                DataTable GetFUpRecords = DataAccessLayer.GetDataTable("call CRM_GetLSReportByFollowupDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetFUpRecords.Rows.Count > 0)
                {
                    var GetFUPLeadsRecords = (from dr in GetFUpRecords.AsEnumerable()
                                              select new ViewLeadsModel()
                                              {
                                                  Id = Convert.ToInt32(dr["ID"]),
                                                  LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
                                                  Mob = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
                                                  EMail = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
                                                  City = dr["City"] == DBNull.Value ? string.Empty : Convert.ToString(dr["City"]),
                                                  LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                  FollowupDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
                                                  //AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                                  //AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                                  //AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                  //LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                  //AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                                  ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                                  LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                                  LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                              }).ToList();



                    var FUPLead = GetFUPLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
                    LeadRecords.AddRange(FUPLead);
                }
                #endregion

                if (!string.IsNullOrEmpty(CDate))
                {
                    var dt = Convert.ToDateTime(CDate);
                    if (FilterText == "New Leads")
                    {
                        VLM.viewleadsList = LeadRecords.Where(em => em.LeadStatusName.Equals("NewLead") && em.LeadDate.Date >= dt.Date && em.LeadDate.Date.Date <= dt.Date).ToList();
                    }
                    else if (FilterText == "Follow-Up")
                    {
                        VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatusName.Equals("Follow") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList();

                    }
                    else if (FilterText == "Missed Follow-Ups")
                    {
                        VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatusName.Equals("Missed") && a.LeadDate.Date >= dt.Date && a.LeadDate.Date <= dt.Date).ToList();

                    }
                    else if (FilterText == "Closed")
                    {
                        //VLM.viewleadsList = LeadRecords.Where(em => em.LeadStatus.Equals("Closed") && (em.ModifiedDate != "" && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date >= dt.Date && Convert.ToDateTime(string.Format("{0:dd/MM/yyyy}", em.ModifiedDate)).Date <= dt.Date)).ToList();

                        var groupedData = LeadRecords.Where(em => em.LeadStatus.Equals("Closed") && (em.ModifiedDate != "" && Convert.ToDateTime(string.Format("{0:" + VLM.DateFormat + "}", em.ModifiedDate)).Date >= dt.Date && Convert.ToDateTime(string.Format("{0:" + VLM.DateFormat + "}", em.ModifiedDate)).Date <= dt.Date)).GroupBy(a => a.Id).ToList();

                        foreach (var gd in groupedData)
                        {
                            foreach (var item in gd)
                            {
                                var vl = new ViewLeadsModel
                                {
                                    Id = item.Id,
                                    LeadName = item.LeadName,
                                    Mob = item.Mob,
                                    EMail = item.EMail,
                                    City = item.City,
                                    LeadStatus = item.LeadStatus,
                                    FollowupDate = item.FollowupDate,
                                    ModifiedDate = item.ModifiedDate,
                                    LeadDate = item.LeadDate,
                                    LeadStatusName = item.LeadStatusName
                                };

                                if (VLM.viewleadsList.Count > 0)
                                {
                                    if (!VLM.viewleadsList.Any(a => a.Id == item.Id))
                                    {
                                        VLM.viewleadsList.Add(vl);
                                    }
                                }
                                else
                                {
                                    VLM.viewleadsList.Add(vl);
                                }
                            }

                        }

                    }
                }
                else
                {
                    if (FilterText == "NewLead" || FilterText == "Follow" || FilterText == "Missed" || FilterText == "Delayed")
                    {
                        VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatusName.Equals(FilterText)).ToList();
                    }
                    else if (FilterText == "Not Interested" || FilterText == "Closed" || FilterText == "Suspect" || FilterText == "Prospect")
                    {
                        //VLM.viewleadsList = LeadRecords.Where(a => a.LeadStatus.ToLower() == FilterText.ToLower()).ToList();

                        var groupedData = LeadRecords.Where(a => a.LeadStatus.ToLower() == FilterText.ToLower()).GroupBy(a => a.Id).ToList();

                        //var vlList = new List<ViewLeadsModel>();

                        foreach (var gd in groupedData)
                        {
                            foreach (var item in gd)
                            {
                                var vl = new ViewLeadsModel
                                {
                                    Id = item.Id,
                                    LeadName = item.LeadName,
                                    Mob = item.Mob,
                                    EMail = item.EMail,
                                    City = item.City,
                                    LeadStatus = item.LeadStatus,
                                    FollowupDate = item.FollowupDate,
                                    ModifiedDate = item.ModifiedDate,
                                    LeadDate = item.LeadDate,
                                    LeadStatusName = item.LeadStatusName
                                };

                                //vlList.Add(vl);
                                if (VLM.viewleadsList.Count > 0)
                                {
                                    if (!VLM.viewleadsList.Any(a => a.Id == item.Id))
                                    {
                                        VLM.viewleadsList.Add(vl);
                                    }
                                }
                                else
                                {
                                    VLM.viewleadsList.Add(vl);
                                }
                            }

                        }


                    }
                    else if (FilterText == "AssignToOthers")
                    {
                        #region Get-AssignToOthersLeads Info
                        //Collect the Manually Created-Leads
                        DataTable GetAssignedRecords = DataAccessLayer.GetDataTable(" call CRM_GetAssignToOtherLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetAssignedRecords.Rows.Count > 0)
                        {
                            VLM.viewleadsList = (from dr in GetAssignedRecords.AsEnumerable()
                                                 select new ViewLeadsModel()
                                                 {
                                                     Id = Convert.ToInt32(dr["Id"]),
                                                     LeadName = Convert.ToString(dr["Customer"]),
                                                     Mob = Convert.ToString(dr["MobileNo"]),
                                                     EMail = Convert.ToString(dr["EmailId"]),
                                                     Country = Convert.ToString(dr["Country"]),
                                                     State = Convert.ToString(dr["State"]),
                                                     City = Convert.ToString(dr["City"]),
                                                     FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
                                                     Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                                     LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                     LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                     AssignTo = Convert.ToString(dr["AssignTo"]),
                                                     CreatedDate = Convert.ToString(dr["Date"]),
                                                 }).ToList();

                        }

                        #endregion
                    }
                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_LeadReportInfoList", VLM);
        }

        #region Today followup lead popup after login
        public ActionResult TodayFollowupLeadPopUp()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            ViewLeadsModel VLM = new ViewLeadsModel();
            //int LoggedBranchId = Convert.ToInt32(Session["BranchID"]);
            //int LoggedCompanyId = Convert.ToInt32(Session["CompanyID"]);
            //int LoggedUID = Convert.ToInt32(Session["UID"]);

            var dd = Constant.GetBharatTime();//get india datetime
            VLM.DateFormat = Constant.DateFormat();//get date format by company id
            VLM.TodayDate = dd.ToString(VLM.DateFormat);//get today date for today followup lead link



            var UID = Convert.ToString(Session["UserType"]) == "SuperAdmin" ? 0 : Convert.ToInt32(Session["UID"]);


            DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable(" call CRM_TodayFollowupLeads(" + BranchID + "," + CompanyID + "," + UID + ")");
            if (TodayFollowupLeads.Rows.Count > 0)
            {
                VLM.TodayFollowUpLeadsList = (from dr in TodayFollowupLeads.AsEnumerable()
                                              select new DashBoardLeadsModel()
                                              {
                                                  Id = Convert.ToInt32(dr["Id"]),
                                                  LeadName = dr["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Customer"]),
                                                  Phone = dr["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MobileNo"]),
                                                  Email = dr["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dr["EmailId"]),
                                                  Country = dr["CountryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CountryName"]),
                                                  CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                  FollowUpDate = dr["FollowDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                  LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
                                                  LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                  AssignedBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                  AssignTo = dr["AssignTo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AssignTo"])
                                              })/*.Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered")*/.ToList();

                //if (UID > 0)
                //{
                //    List<DashBoardLeadsModel> assignList = VLM.TodayFollowUpLeadsList.Where(em => em.AssignTo == UID).ToList();
                //    VLM.TodayFollowUpLeadsList = VLM.TodayFollowUpLeadsList.Where(em => em.LeadOwner == UID).ToList();
                //    if (assignList.Count > 0)
                //    {
                //        VLM.TodayFollowUpLeadsList.AddRange(assignList);
                //    }
                //}
            }

            if (VLM.TodayFollowUpLeadsList.Count > 0)
            {
                return PartialView("_PartialViewTodayFollowupLeadPopUp", VLM);
            }
            else
            {
                return Json("No record found", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult AssignLeadShow(string filterText, int? page, string UserddlName)
        {
            AssignedLeadsModel ALM = new AssignedLeadsModel();
            try
            {
                if (Session["UID"] != null)
                {
                    ALM.Language = cr.GetCompanyLanguage(0);
                    int BranchID = Convert.ToInt32(Session["BranchID"]);
                    int CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    //int LoggedBranchId = Convert.ToInt32(Session["BranchID"]);
                    //int LoggedCompanyId = Convert.ToInt32(Session["CompanyID"]);

                    var UID = Convert.ToInt32(Session["UID"]);
                    //if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    //{
                    //    //UID = 0;
                    //    if (!string.IsNullOrEmpty(UserddlName))
                    //    {
                    //        UID = Convert.ToInt32(UserddlName);
                    //    }
                    //}
                    //else
                    //{
                    //    if (!string.IsNullOrEmpty(UserddlName))
                    //    {
                    //        UID = Convert.ToInt32(UserddlName);
                    //    }
                    //    else
                    //    {
                    //        UID = Convert.ToInt32(Session["UID"]);
                    //    }
                    //}

                    //if (!string.IsNullOrEmpty(UserddlName))
                    //{
                    //    Session["Userddllist"] = UserddlName;
                    //    ALM.UserddlName = Session["Userddllist"].ToString();

                    //    var data = cr.GetUserCompanyBranch(UID);
                    //    if (data != null)
                    //    {
                    //        BranchID = data.BranchID;
                    //        CompanyID = data.CompanyID;
                    //    }
                    //}


                    #region Data-time-Formate
                    ALM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    var dd = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    var FilterType = Convert.ToString(Session["VLFltrFilterType"]);
                    #endregion

                    #region To-CheckFilter-Date
                    if (Session["AssignedFrmDt"] != null && Session["AssignedToDt"] != null)
                    {

                        if (ALM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var FromDate = Convert.ToString(Session["AssignedFrmDt"]);
                            var ToDate = Convert.ToString(Session["AssignedToDt"]);


                            var fmDate = DateTime.ParseExact(FromDate, ALM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, ALM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            MStartDate = Convert.ToString(Session["AssignedFrmDt"]);
                            MEndDate = Convert.ToString(Session["AssignedToDt"]);
                        }
                    }
                    else
                    {
                        if (ALM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            Session["AssignedFrmDt"] = monthStartDate.ToString(ALM.DateFormat);
                            Session["AssignedToDt"] = MonthendDate.ToString(ALM.DateFormat);
                        }
                        else
                        {
                            Session["AssignedFrmDt"] = MStartDate;
                            Session["AssignedToDt"] = MEndDate;
                        }
                    }
                    #endregion

                    //if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    //{
                    //    //var GetUserList = db.crm_usertbl.Where(em => em.BranchID == LoggedBranchId && em.CompanyID == LoggedCompanyId && em.ProfileName.ToLower().Contains("sales") && em.Status==true).ToList();
                    //    //if (GetUserList != null)
                    //    //{
                    //    //    List<CreateUserModel> userList = new List<CreateUserModel>();
                    //    //    foreach (var item in GetUserList)
                    //    //    {
                    //    //        CreateUserModel CRM = new CreateUserModel();
                    //    //        CRM.UserID = item.Id;
                    //    //        CRM.UserName = item.Fname + ' ' + item.Lname;
                    //    //        userList.Add(CRM);
                    //    //    }
                    //    //    ALM.Userddllist = userList;
                    //    //}

                    //    string assignquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName 
                    //            from crm_usertbl ur
                    //            join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                    //            Where ur.BranchID = " + LoggedBranchId + " and ur.CompanyID = " + LoggedCompanyId + " and ur.Status = 1 and rl.ViewLeads = 1";
                    //    var GetUserList = db.Database.SqlQuery<CreateUserModel>(assignquery).OrderBy(a=>a.UserName).ToList();
                    //    if (GetUserList != null && GetUserList.Count() > 0)
                    //    {
                    //        ALM.Userddllist = GetUserList.ToList();
                    //    }

                    //    #region Get other branch user list to add mapped user list

                    //    DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + LoggedBranchId + "','','')");
                    //    if(GetRecords.Rows.Count>0)
                    //    {
                    //        var GetOtherBranchUserList = (from dr in GetRecords.AsEnumerable()
                    //                                      select new CreateUserModel()
                    //                                      {
                    //                                          UserID = Convert.ToInt32(dr["AssignedUserID"]),
                    //                                          UserName = Convert.ToString(dr["UserName"] + " (Branch: " + dr["BranchName"] + ")"),
                    //                                          IsActive = Convert.ToBoolean(dr["IsActive"])
                    //                                      }).Where(a => a.IsActive == true).OrderBy(a => a.UserName).ToList();
                    //        ALM.Userddllist.AddRange(GetOtherBranchUserList);
                    //        ALM.Userddllist = ALM.Userddllist.OrderBy(a => a.UserName).ToList();

                    //    }

                    //    #endregion
                    //}
                    //else
                    //{
                    //    var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID ).FirstOrDefault();
                    //    ALM.Userddllist = new List<CreateUserModel>();
                    //    if (GetUserData != null && GetUserData.MappedUsers != null)
                    //    {
                    //        ALM.MappedUser = GetUserData.MappedUsers.ToString();
                    //        var GetMapUser = GetUserData.MappedUsers.Split(',');
                    //        CreateUserModel u = new CreateUserModel();
                    //        u.UserName = "ALL";
                    //        ALM.Userddllist.Add(u);

                    //        if (GetMapUser != null)
                    //        {
                    //            foreach (var item in GetMapUser)
                    //            {
                    //                var mapid = Convert.ToInt32(item);
                    //                var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).FirstOrDefault();
                    //                if (GetMapUserData != null)
                    //                {
                    //                    var user = new CreateUserModel
                    //                    {
                    //                        UserID = mapid,
                    //                        UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
                    //                    };
                    //                    ALM.Userddllist.Add(user);
                    //                }
                    //            }
                    //        }

                    //        //check other branch user mapped to current user
                    //        string query = @"select muob.MappedUserId as UserID,CONCAT(u.FName ,' ', u.Lname, '(Branch: ',IFNULL(cb.BranchName,'N / A'),')') as UserName,aorg.IsActive
                    //                     from CRM_MappedUserOtherBranch muob
                    //                     join crm_assignedtootherorganization aorg on muob.MappedUserId = aorg.AssignedUserID
                    //                     join crm_usertbl u on u.Id = aorg.AssignedUserID
                    //                     join com_branches cb on u.BranchID = cb.OrgBranchCode
                    //                     join company_profile cp on cb.OrganizationID = cp.id
                    //                     Where muob.UserId = '" + UID + "' and aorg.AssignToCompanyID = '" + BranchID + "' and aorg.IsActive = 1 order by muob.CreateDate; ";
                    //        var data = db.Database.SqlQuery<CreateUserModel>(query).OrderBy(a => a.UserName).ToList();                        
                    //        if (data.Count > 0)
                    //        {
                    //    ALM.Userddllist.AddRange(data);
                    //        }
                    //    }
                    //}

                    var getleadStatus = db.crm_leadstatus_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).OrderBy(a => a.LeadStatusName).ToList();
                    if (getleadStatus != null && getleadStatus.Count > 0)
                    {
                        //List<LeadStatusModel> LSMList = new List<LeadStatusModel>();
                        var LSMList = (from item in getleadStatus
                                       select new LeadStatusModel
                                       {
                                           Id = item.Id,
                                           LeadStatusname = item.LeadStatusName
                                       }).ToList();
                        //foreach (var item in getleadStatus)
                        //{
                        //    LeadStatusModel LSM = new LeadStatusModel();
                        //    LSM.Id = item.Id;
                        //    LSM.LeadStatusname = item.LeadStatusName;
                        //    LSMList.Add(LSM);
                        //}
                        ALM.leadstatusList = LSMList;
                    }


                    #region Assigned-Date
                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_LeadsAssignedToMe(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ",'" + filterText + "' )");
                    if (GetRecords.Rows.Count > 0)
                    {

                        ALM.assignleadsModelList = (from dr in GetRecords.AsEnumerable()
                                                    select new AssignedLeadsModel()
                                                    {
                                                        Id = Convert.ToInt32(dr["ID"]),
                                                        CustomerName = Convert.ToString(dr["Customer"]),
                                                        Phone = Convert.ToString(dr["MobileNo"]),
                                                        Email = Convert.ToString(dr["EmailId"]),
                                                        Country = Convert.ToString(dr["CountryName"]),
                                                        State = Convert.ToString(dr["StateName"]),
                                                        City = Convert.ToString(dr["CityName"]),
                                                        FollowUpDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                        LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                        //AssignToUserName = Convert.ToString(dr["AssignToUserName"]),
                                                        //AssignByUserName = Convert.ToString(dr["AssignByUserName"]),
                                                        //LeadOwner = Convert.ToString(dr["LeadOwnerName"]),
                                                        AssignDate = Convert.ToString(dr["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                        AssignTo = Convert.ToString(dr["AssignedTo"])
                                                    }).ToList();
                    }
                    #endregion


                }
                else
                {
                    return RedirectToAction("Login", "home");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(ALM);
        }

        //[HttpGet]
        //public ActionResult GetEmailTemplateContent(Int32? EmailTemplateID)
        //{
        //    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //    //var getEmailTemplate = db.crm_emailtemplate.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefault();
        //    //return Json(getEmailTemplate.EmailTemplateContent, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetStateList(string CountryID)
        {
            List<ManageStateModel> sList = new List<ManageStateModel>();
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var GetStateList = db.com_state.Where(em => em.Country == CountryID && (em.CompanyID == null || em.CompanyID == 0 || em.CompanyID == CompanyID)).Where(a => a.ID != 0).ToList();
            //var isIndia  = cr.GetCompanyCountry();
            if (GetStateList.Count > 0)
            {
                //var stl = (from st in GetStateList
                //           select new ManageStateModel
                //           {
                //               StateID = st.ID,
                //               StateName = CountryID == "1" ? st.State.Substring(3) : st.State,
                //         }).ToList();
                //sList = stl;
                foreach (var item in GetStateList)
                {
                    ManageStateModel SM = new ManageStateModel();
                    int dashIndex = 0;
                    string stateName = string.Empty;
                    SM.StateID = item.ID;

                    dashIndex = item.State.IndexOf('-');//get index of fist dash
                    stateName = dashIndex > 0 ? item.State.Substring(dashIndex + 1) : item.State;//if dash index >0 then get state name after dash otherwise full state name

                    //SM.StateName = CountryID == "1" ? item.State.Substring(3) : item.State;
                    SM.StateName = stateName;
                    sList.Add(SM);
                }
                sList = sList.OrderBy(a => a.StateName).ToList();
            }

            return Json(sList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCityList(string CountryID, string StateID)
        {
            List<ManageCityModel> cList = new List<ManageCityModel>();
            var GetCityList = db.com_city.Where(em => em.Country == CountryID && em.State == StateID).ToList();
            if (GetCityList.Count > 0)
            {
                var ctl = (from cty in GetCityList
                           select new ManageCityModel
                           {
                               CityID = cty.ID,
                               CityName = cty.City
                           }).ToList();
                cList = ctl.OrderBy(a => a.CityName).ToList();
                //foreach (var item in GetCityList)
                //{
                //    ManageCityModel CM = new ManageCityModel();
                //    CM.CityID = item.ID;
                //    CM.CityName = item.City;
                //    cList.Add(CM);
                //}
            }
            return Json(cList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckMobilenumber(string number)
        {
            string msg = "";
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!string.IsNullOrEmpty(number))
            {
                //if (number.Length > 10)
                //{
                //    if (number.Length > 11)
                //    {
                //        number = number.Substring(2, number.Length - 2);
                //    }
                //    else if (number.Length > 10)
                //    {
                //        number = number.Substring(1, number.Length - 1);
                //    }
                //}
                //if (number.Length >= 9)
                //{
                if (number.Length < 9)//check mobile no length is less then 9 then it add 0 
                {
                    number = "0" + number;
                }
                number = number.Substring(number.Length - 9, 9);//get last line digits 

                if (db.crm_createleadstbl.Any(x => (!string.IsNullOrEmpty(x.MobileNo) && x.MobileNo.Substring(x.MobileNo.Length - 9, 9) == number) && x.BranchID == BranchID && x.CompanyID == CompanyID))
                {
                    msg = "exist";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    msg = "ok";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                //}
                //else
                //{
                //    msg = "error";
                //    return Json(msg, JsonRequestBehavior.AllowGet);
                //}
            }
            else
            {
                msg = "error";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult CheckEmailExist(string emailId)
        {
            string msg = "";
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!string.IsNullOrEmpty(emailId))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(emailId);
                if (match.Success)
                {
                    if (db.crm_createleadstbl.Any(x => x.EmailId == emailId && x.BranchID == BranchID && x.CompanyID == CompanyID))
                    {
                        msg = "exist";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        msg = "ok";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    msg = "invalid";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                msg = "error";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CRMSendNow()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                String SendNow = String.Empty;
                String Issue = String.Empty;
                var CheckEmailSetting = db.crm_emailsetting.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
                if (CheckEmailSetting != null)
                {
                    HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
                    String ToEmailAddress = Convert.ToString(Request.Form[0]);
                    String Subject = Convert.ToString(Request.Form[1]).TrimEnd();
                    String MessageBody = Convert.ToString(Request.Form[2]).TrimEnd();
                    Int32? FileID = Request.Form[3].Count() == 0 ? 0 : Convert.ToInt32(Request.Form[3]);
                    if (Postfile != null)
                    {
                        string fileName = Path.GetFileName(Postfile.FileName);
                        SendNow = EmailUtility.CRMSendEmailAttachment(ToEmailAddress, Subject, MessageBody, Postfile, CompanyID, BranchID, out Issue);
                    }
                    else if (FileID > 0)
                    {
                        var getFile = db.crm_filemanager.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FileID == FileID).FirstOrDefault();
                        if (getFile != null)
                        {
                            var fileName = "~/FileManager/" + Path.GetFileName(getFile.FileUpload);
                            SendNow = EmailUtility.sCRMSendEmailAttachment(ToEmailAddress, Subject, MessageBody, fileName, CompanyID, BranchID, out Issue);
                        }
                    }
                    else
                    {
                        SendNow = EmailUtility.CRMSendEmailAttachment(ToEmailAddress, Subject, MessageBody, null, CompanyID, BranchID, out Issue);
                    }

                    if (SendNow == "True")
                    {
                        Successmsg = "Email send successfully.";
                    }
                    else
                    {
                        Errormsg = Issue; //"** Something went wrong,Please try again.";
                    }
                }
                else
                {
                    Errormsg = "** Email setting is not configure,Please contact to administrator.";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                Errormsg = "error";
            }
            string MsgReturn = string.Empty;
            if (!string.IsNullOrWhiteSpace(Errormsg))
            {
                MsgReturn = Errormsg;
            }
            else if (!string.IsNullOrWhiteSpace(Successmsg))
            {
                MsgReturn = Successmsg;
            }
            return Json(MsgReturn, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SendSMS()
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                string SendNow = string.Empty;
                string Issue = string.Empty;

                //string query = @"select SMSAPI,UserName,Password,SenderID from smsapisetting where companyid=" + CompanyID + " and Branchcode=" + BranchID + "";
                //var data =await db.Database.SqlQuery<SMSApiModel>(query).FirstOrDefaultAsync();


                var smsApiConfig = new SMSApiModel
                {
                    SMSAPI = "https://prpsms.co.in/API/SendMsg.aspx?",
                    UserName = "20201136",
                    Password = "Q9o99eLe",
                    SenderID = "SMSINF"
                };

                //string url = "https://prpsms.co.in/API/SendMsg.aspx?uname=20201136&pass=Q9o99eLe&send=SMSINF&dest="+mobile+"&msg="+sms+"&priority=1";


                if (smsApiConfig != null)
                {

                    string PhoneNumbers = Convert.ToString(Request.Form[0]);
                    string TextMessage = Convert.ToString(Request.Form[1]);

                    var splitedNumbers = PhoneNumbers.Split('|');
                    var sb = new StringBuilder();
                    bool isOneSended = false;
                    foreach (var item in splitedNumbers)
                    {
                        var result = Utility.SendSMS(TextMessage, item, smsApiConfig.SMSAPI, smsApiConfig.UserName, smsApiConfig.Password, smsApiConfig.SenderID);
                        if (result.Length != 19)
                        {
                            sb.AppendFormat("{0}, ", item);
                        }
                        else
                        {
                            isOneSended = true;
                        }
                    }

                    if (sb != null)
                    {
                        if (isOneSended)
                        {
                            var sb1 = new StringBuilder();
                            sb1.Append("These are the numbers that sms has not been sended: ");
                            sb1.Append(sb.ToString());
                            string Msg = sb1.ToString();
                            Errormsg = Msg.TrimEnd(',');
                        }
                        else
                        {
                            Errormsg = "** Something went wrong,Please try again.";
                        }

                    }
                    else
                    {
                        Successmsg = "SMS send successfully.";
                    }
                }
                else
                {
                    Errormsg = "** SMS api is not configure,Please contact to administrator.";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                Errormsg = "error";
            }
            string MsgReturn = string.Empty;
            if (!string.IsNullOrWhiteSpace(Errormsg))
            {
                MsgReturn = Errormsg;
            }
            else if (!string.IsNullOrWhiteSpace(Successmsg))
            {
                MsgReturn = Successmsg;
            }
            return Json(MsgReturn, JsonRequestBehavior.AllowGet);
        }

        // View Lead page add description and update followup date
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> ViewLeadAddDescription()
        {

            //ViewLeadsModel VLM = new ViewLeadsModel();
            AddLeadDescrResponseDTO VLM = new AddLeadDescrResponseDTO();
            var GetLeadsData = new crm_createleadstbl();

            string Successmsg = string.Empty;
            string Errormsg = string.Empty;

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    int BranchID = Convert.ToInt32(Session["BranchID"]);
                    int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    int UserId = Convert.ToInt32(Session["UID"]);

                    long? LeadID = Convert.ToInt32(Request.Form[0]);
                    string Description = Convert.ToString(Request.Form[1]).TrimEnd();
                    string LeadStatusName = Convert.ToString(Request.Form[2]).TrimEnd();
                    int LeadStatusId = Convert.ToInt32(Request.Form[3]);
                    string FollowUpDate = Convert.ToString(Request.Form[4]).TrimEnd();
                    string followUpDateForIst = FollowUpDate;
                    string FollowUpTime = Convert.ToString(Request.Form[5]).TrimEnd();

                    string DateFormat = Constant.DateFormat();//get date format by logged user company id
                    DateTime localTime = Constant.GetBharatTime();// get india datetime
                    var CurrentDate = localTime.ToString("dd/MM/yyyy");

                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        GetLeadsData = await db.crm_createleadstbl.FindAsync(LeadID);
                    }
                    else
                    {
                        var chaUserId = UserId.ToString();
                        GetLeadsData = db.crm_createleadstbl.Where(a => a.Id == LeadID && (a.LeadOwner == UserId || (!string.IsNullOrEmpty(a.AssignTo) && a.AssignTo == chaUserId) || a.BranchID == BranchID && a.CompanyID == CompanyID)).FirstOrDefault();
                        // string query = @"select * from crm_createLeadstbl Where Id='" + LeadID + "' and (LeadOwner = '" + UserId + "' or (AssignTo !='' and AssignTo = '" + UserId + "') or (BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'))";
                        //GetLeadsData = db.Database.SqlQuery<crm_createleadstbl>(query).FirstOrDefault();
                    }
                    string FileName = string.Empty;
                    string FileFullName = string.Empty;
                    HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
                    if (Postfile != null)
                    {
                        int fileSize = Postfile.ContentLength;
                        if (fileSize > 0)
                        {
                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                            var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
                            if (supportedTypes.Contains(fileExt))
                            {
                                //get Customer name CLM.Customer
                                //var CLM1 = await db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                                string extension = Path.GetExtension(Postfile.FileName);
                                FileName = "Lead-" + Convert.ToString(Session["UserName"]) + "-" + GetLeadsData == null ? "NA" : GetLeadsData.Customer + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                FileFullName = FileName + extension;
                                string _path = Server.MapPath("~/LeadAttachment/" + FileName + extension);
                                Postfile.SaveAs(_path);
                            }
                            else
                            {
                                Errormsg = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                VLM.Message = Errormsg;
                            }
                        }
                    }

                    //var cl = await db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                    if (GetLeadsData != null)
                    {

                        DateTime PreFolloupDate = Convert.ToDateTime(GetLeadsData.FollowDate);
                        if (!string.IsNullOrWhiteSpace(FollowUpDate))
                        {
                            if (DateFormat != "dd/MM/yyyy")
                            {
                                //var dateFormates = Constant.DateFormates();
                                //DateTime followDate = DateTime.ParseExact(FollowUpDate, dateFormates, new CultureInfo("en-US"), DateTimeStyles.None); 

                                var followDate = DateTime.ParseExact(FollowUpDate, DateFormat, CultureInfo.InvariantCulture);
                                FollowUpDate = string.Format("{0:dd/MM/yyyy}", followDate);
                            }
                            if (Convert.ToDateTime(FollowUpDate).Date < localTime.Date && LeadStatusName != "Not Interested")
                            {
                                Errormsg = "BackFollowupDate";
                                VLM.Message = Errormsg;
                                return Json(VLM, JsonRequestBehavior.AllowGet);
                            }

                            if (!string.IsNullOrEmpty(FollowUpTime))
                            {
                                GetLeadsData.ZoneName = (!string.IsNullOrEmpty(GetLeadsData.ZoneName) && !GetLeadsData.ZoneName.ToLower().Contains("Select")) ? GetLeadsData.ZoneName : Constant.GetCompanyTimeZone(CompanyID);
                                GetLeadsData.FollowUpTime = FollowUpTime;//set followup time

                                var istTime = Constant.GetFollowupTimeInIST(GetLeadsData.ZoneName, followUpDateForIst, FollowUpTime, CompanyID);
                                if (!string.IsNullOrEmpty(istTime))
                                {
                                    var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                                    //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                                    GetLeadsData.ConvertedFupDateTime = cDate;
                                    // var finalDateTime = string.Format("{0} {1}", FollowUpDate, FollowUpTime);
                                    // DateTime dateTime = Convert.ToDateTime(finalDateTime);
                                    //var followupDateTime = istTime;
                                    VLM.FollowupDateTime = istTime;
                                    VLM.FollowupTime = cDate.ToString("hh:mm tt"); //set followup time                                 
                                }
                                //else
                                //{
                                //    var td = localTime;
                                //    GetLeadsData.ZoneName = (!string.IsNullOrEmpty(GetLeadsData.ZoneName) && !GetLeadsData.ZoneName.ToLower().Contains("Select")) ? GetLeadsData.ZoneName : Constant.GetCompanyTimeZone(CompanyID);
                                //    GetLeadsData.ConvertedFupDateTime = td;
                                //    //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", td);
                                //    //var finalDateTime = string.Format("{0} {1}", FollowUpDate, FollowUpTime);
                                //    GetLeadsData.FollowUpTime = td.ToString("hh:mm tt"); //set followup time
                                //    //DateTime dateTime = Convert.ToDateTime(finalDateTime);
                                //    var followupDateTime = string.Format("{0:MM-dd-yyyy HH:mm:ss}", td);
                                //    VLM.FollowupDateTime = followupDateTime;
                                //    VLM.FollowupTime = GetLeadsData.FollowUpTime;
                                //}
                            }
                            else
                            {
                                GetLeadsData.ConvertedFupDateTime = null;//set null if followuptime isnull
                            }
                            //else
                            //{
                            //    GetLeadsData.ZoneName = (!string.IsNullOrEmpty(GetLeadsData.ZoneName) && !GetLeadsData.ZoneName.ToLower().Contains("Select")) ? GetLeadsData.ZoneName: Constant.GetCompanyTimeZone(CompanyID) ;

                            //    var istTime = Constant.GetFollowupTimeInIST(GetLeadsData.ZoneName, followUpDateForIst, FollowUpTime, CompanyID);
                            //    if (!string.IsNullOrEmpty(istTime))
                            //    {
                            //        var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                            //        //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                            //        GetLeadsData.ConvertedFupDateTime = cDate;
                            //        GetLeadsData.FollowUpTime = cDate.ToString("hh:mm tt"); //set followup time
                            //        VLM.FollowupDateTime = istTime;
                            //        VLM.FollowupTime = GetLeadsData.FollowUpTime;
                            //    }
                            //    else
                            //    {
                            //        var td = localTime;
                            //        //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", td);
                            //        GetLeadsData.ZoneName = (!string.IsNullOrEmpty(GetLeadsData.ZoneName) && !GetLeadsData.ZoneName.ToLower().Contains("Select")) ? GetLeadsData.ZoneName : Constant.GetCompanyTimeZone(CompanyID);
                            //        GetLeadsData.ConvertedFupDateTime = td;
                            //        GetLeadsData.FollowUpTime = td.ToString("hh:mm tt");//set followup time
                            //        var followupDateTime = string.Format("{0:MM-dd-yyyy HH:mm:ss}", td);
                            //        VLM.FollowupDateTime = followupDateTime;
                            //        VLM.FollowupTime = GetLeadsData.FollowUpTime;
                            //    }
                            //}

                            //VLM.FollowupDate = FollowUpDate;
                            //if (Convert.ToDateTime(FollowUpDate).Date > PreFolloupDate.Date)
                            //{
                            //    GetLeadsData.IsLeadReminder = true;
                            //    VLM.LeadReminder = 1;
                            //}
                            //else
                            //{
                            //    VLM.LeadReminder = GetLeadsData.IsLeadReminder == true ? 1 : 0;
                            //}

                            //GetLeadsData.FollowDate = DateTime.ParseExact(FollowUpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            GetLeadsData.FollowDate = Convert.ToDateTime(FollowUpDate);

                        }


                        crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                        LD.Description = Description;
                        LD.LeadId = LeadID;
                        LD.Date = CurrentDate;
                        LD.ByUID = Convert.ToInt32(Session["UID"]);
                        LD.ByUserName = Convert.ToString(Session["UserName"]);
                        LD.CreatedDateTime = localTime;
                        LD.BranchID = BranchID;
                        LD.CompanyID = CompanyID;
                        LD.LeadAttachment = FileFullName;
                        LD.LeadStatusName = LeadStatusName;
                        db.crm_leaddescriptiontbl.Add(LD);


                        GetLeadsData.LeadStatusID = LeadStatusId;
                        GetLeadsData.LeadStatus = LeadStatusName;
                        GetLeadsData.ModifiedDate = localTime;

                        if (!string.IsNullOrEmpty(GetLeadsData.AssignTo))
                        {
                            var data = cr.GetUserCompanyBranch(Convert.ToInt32(GetLeadsData.AssignTo));
                            if (data != null)
                            {
                                BranchID = data.BranchID;
                                CompanyID = data.CompanyID;
                            }
                        }

                        //update lead delayed missed followup detail                        
                        var existRecord = await db.crm_delayedfollowuprecordtbl.Where(em => em.LeadId == GetLeadsData.Id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                        if (Convert.ToDateTime(FollowUpDate).Date > PreFolloupDate.Date && existRecord != null)
                        {
                            existRecord.PreFollowUpDate = PreFolloupDate.Date;
                            existRecord.CreatedDate = localTime.Date;
                            existRecord.CreatedDatetime = localTime;
                            db.SaveChanges();
                        }
                        else
                        {
                            crm_delayedfollowuprecordtbl dfr = new crm_delayedfollowuprecordtbl();
                            if (Convert.ToDateTime(FollowUpDate).Date > PreFolloupDate.Date)
                            {
                                dfr.CreatedBy = Convert.ToInt32(Session["UID"]);
                                dfr.CreatedDate = localTime.Date;
                                dfr.CreatedDatetime = localTime;
                                dfr.PreFollowUpDate = PreFolloupDate.Date;
                                dfr.LeadId = Convert.ToInt32(GetLeadsData.Id);
                                dfr.BranchID = BranchID;
                                dfr.CompanyID = CompanyID;
                                db.crm_delayedfollowuprecordtbl.Add(dfr);
                            }

                        }

                        db.SaveChanges();
                        trans.Commit();

                    }

                    string format = String.Format("{0}", DateFormat + " hh:mm tt");
                    string modifyDate = String.Format("{0:" + format + "}", localTime);
                    Successmsg = "ok";
                    //Successmsg = modifyDate;
                    VLM.Message = Successmsg;
                    VLM.ModifiedDate = modifyDate;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    Errormsg = "error";
                    VLM.Message = Errormsg;
                    return Json(VLM, JsonRequestBehavior.AllowGet);
                }
            }

            //string MsgReturn = string.Empty;
            //if (!string.IsNullOrWhiteSpace(Errormsg))
            //{
            //    MsgReturn = Errormsg;
            //}
            //else if (!string.IsNullOrWhiteSpace(Successmsg))
            //{
            //    MsgReturn = Successmsg;
            //}
            return Json(VLM, JsonRequestBehavior.AllowGet);
        }

        // View all description of a Lead
        public async Task<ActionResult> ViewLeadDecsription(Int64 Lid)
        {
            CreateLeadsModel DModel = new CreateLeadsModel();
            try
            {

                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                int UserId = Convert.ToInt32(Session["UID"]);
                var GetLeadsData = new crm_createleadstbl();
                DModel.DateFormat = Constant.DateFormat();//get date format by company id

                //GetLeadsData = await db.crm_createleadstbl.Where(a=>a.Id==Lid).FirstOrDefaultAsync();

                //if(GetLeadsData!=null)
                //{
                //    if (Convert.ToString(Session["UserType"]) == "SuperAdmin" && GetLeadsData.LeadOwner == UserId)
                //    {
                //        ViewBag.UserName = GetLeadsData.Customer;
                //        List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                //        DModel.descriptionList = descList;
                //    }
                //    else if (Convert.ToString(Session["UserType"]) == "SuperAdmin" && GetLeadsData.LeadOwner != UserId)
                //    {
                //        var userDetail = await db.crm_usertbl.Where(a => a.Id == GetLeadsData.LeadOwner).FirstOrDefaultAsync();
                //        if(userDetail!=null && userDetail.CompanyID==CompanyID && userDetail.BranchID==BranchID)
                //        {
                //            ViewBag.UserName = GetLeadsData.Customer;
                //            List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                //            DModel.descriptionList = descList;
                //        }                       
                //    }
                //    else if (Convert.ToString(Session["UserType"]) != "SuperAdmin" && GetLeadsData.LeadOwner == UserId)
                //    {
                //        ViewBag.UserName = GetLeadsData.Customer;
                //        List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                //        DModel.descriptionList = descList;
                //    }
                //    else if (Convert.ToString(Session["UserType"]) != "SuperAdmin" && GetLeadsData.LeadOwner != UserId && !string.IsNullOrEmpty(GetLeadsData.AssignedBy))
                //    {
                //        int assignById = Convert.ToInt32(GetLeadsData.AssignedBy);
                //        var userDetail = await db.crm_usertbl.Where(a => a.Id == GetLeadsData.LeadOwner).FirstOrDefaultAsync();
                //        var assignByUserDetail = await db.crm_usertbl.Where(a => a.Id == assignById).FirstOrDefaultAsync();
                //        if (userDetail != null && assignByUserDetail!=null && userDetail.CompanyID == assignByUserDetail.CompanyID && userDetail.BranchID == assignByUserDetail.BranchID)
                //        {
                //            ViewBag.UserName = GetLeadsData.Customer;
                //            List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                //            DModel.descriptionList = descList;
                //        }
                //    }
                //    else
                //    {

                //        ViewBag.UserName = GetLeadsData.Customer;
                //        List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid && em.CompanyID == CompanyID && em.BranchID == BranchID).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                //        DModel.descriptionList = descList;
                //    }
                //}

                //if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                //{
                //    GetLeadsData = await db.crm_createleadstbl.FindAsync(Lid);
                //}
                //else
                //{
                //    string query = @"select * from `crm_createLeadstbl` Where Id='" + Lid + "' and (LeadOwner = '" + UserId + "' or (AssignTo !='' and AssignTo = '" + UserId + "') or (BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'))";
                //    GetLeadsData = db.Database.SqlQuery<crm_createleadstbl>(query).FirstOrDefault();
                //}

                //if(GetLeadsData!=null)
                //{
                //    ViewBag.UserName = GetLeadsData.Customer;
                //    List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid && em.CompanyID == CompanyID && em.BranchID == BranchID).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                //    DModel.descriptionList = descList;
                //}

                if (CompanyID == 296)//view lead description for smart capita admin or users
                {
                    GetLeadsData = await db.crm_createleadstbl.FindAsync(Lid);
                    ViewBag.UserName = GetLeadsData.Customer;
                    List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                    DModel.descriptionList = descList;
                }
                else
                {
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        GetLeadsData = await db.crm_createleadstbl.FindAsync(Lid);
                    }
                    else
                    {
                        string query = @"select * from `crm_createLeadstbl` Where Id='" + Lid + "' and (LeadOwner = '" + UserId + "' or (AssignTo !='' and AssignTo = '" + UserId + "') or (BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'))";
                        GetLeadsData = db.Database.SqlQuery<crm_createleadstbl>(query).FirstOrDefault();
                    }

                    if (GetLeadsData != null)
                    {
                        ViewBag.UserName = GetLeadsData.Customer;
                        List<crm_leaddescriptiontbl> descList = await db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid && em.CompanyID == CompanyID && em.BranchID == BranchID).OrderByDescending(em => em.CreatedDateTime).ToListAsync();
                        DModel.descriptionList = descList;
                    }

                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialViewLeadDescription", DModel);
        }

        public FileResult LeadDownload(string PostFile)
        {
            var filepath = System.IO.Path.Combine(Server.MapPath("~/LeadAttachment/"), PostFile);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), PostFile);
        }

        public ActionResult FilterLeadsByDates(string FilterType, string filterText, string FromDate, string ToDate, string UserddlName)
        {
            try
            {
                Session["VLFltrFilterType"] = FilterType;
                Session["filterText"] = filterText;
                Session["VLFltrFrmDt"] = FromDate;
                Session["VLFltrToDt"] = ToDate;
                Session["UserddlName"] = UserddlName;
                return Redirect("/home/viewleads/?page=1");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["Alert"] = "";
                return View();
            }
        }

        public ActionResult ResetLeadsFilter()
        {
            Session["VLFltrFilterType"] = null;
            Session["VLFltrFrmDt"] = null;
            Session["VLFltrToDt"] = null;
            Session["UserddlName"] = "0";
            Session["filterText"] = null;
            Session["DDLFilterText"] = null;
            return Redirect("/home/viewleads/?page=1");
        }

        public ActionResult ExportViewLeads(string SelectedLead123, int? Selecteduserid)
        {
            try
            {
                Session["exportdatasmg12"] = 0;
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                var ViewSalesLeads = (List<ViewLeadsModel>)TempData["ExportViewLeads"];
                string selectlist = string.Empty;
                //if (Request.Form["MapperUserLeads"] != null)
                //{
                //    selectlist = Request.Form["MapperUserLeads"].ToString();
                //}
                if (SelectedLead123 == "on|on|on|on" || SelectedLead123 == "" || string.IsNullOrEmpty(SelectedLead123))
                {
                    Session["exportdatasmg"] = 1;
                    Session["exportdatasmg12"] = 1;
                    // ViewData["Message"] = String.Format("Please Select the lead checkbox ");
                    return Redirect("/home/viewleads/?page=1");
                }
                else
                //if (!string.IsNullOrEmpty(SelectedLead123))
                {
                    selectlist = SelectedLead123;
                    Session["exportdatasmg"] = 0;
                }

                if (selectlist != string.Empty)
                {
                    var SpliteselectLeads = selectlist.Split('|');
                    if (SpliteselectLeads[0] == "on")
                    {
                        SpliteselectLeads = SpliteselectLeads.Where((item, index) => index != 0).ToArray();
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Customer Name", typeof(string));
                    dt.Columns.Add("Organization Name", typeof(string));
                    dt.Columns.Add("Mobile No", typeof(string));
                    dt.Columns.Add("WhatsApp Number", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("Country", typeof(string));
                    dt.Columns.Add("State", typeof(string));
                    dt.Columns.Add("City", typeof(string));
                    dt.Columns.Add("Followup Date", typeof(string));
                    dt.Columns.Add("Lead Status", typeof(string));
                    dt.Columns.Add("Created By", typeof(string));
                    dt.Columns.Add("Assign By", typeof(string));
                    dt.Columns.Add("Assign Date", typeof(string));
                    dt.Columns.Add("Description", typeof(string));

                    //foreach (var Ditem in ViewSalesLeads)
                    // {

                    foreach (var item1 in SpliteselectLeads)
                    {
                        //var SpliteItem = selectlist.Split('/');
                        //string Value = Convert.ToString(SpliteItem[0]);
                        var SpliteItem = item1.Split('/');
                        string Value = Convert.ToString(SpliteItem[0]);
                        var isNumber = Value.All(char.IsNumber);//to check all charcter is numeric
                        if (isNumber)
                        {
                            var leadid = Convert.ToInt64(SpliteItem[0]);

                            if (Selecteduserid != 0 && Selecteduserid != null)
                            {
                                var assigntootherbranch = db.crm_assignedtootherorganization.Where(a => a.AssignedUserID == Selecteduserid).ToList();
                                if (assigntootherbranch != null)
                                {
                                    var otherusercomid = db.crm_usertbl.Where(em => em.Id == Selecteduserid).FirstOrDefault();
                                    if (otherusercomid != null)
                                    {
                                        int comid = Convert.ToInt32(otherusercomid.CompanyID);
                                        int branchid = Convert.ToInt32(otherusercomid.BranchID);

                                        string description = "";
                                        var GetLeadsDescription = db.crm_leaddescriptiontbl.Where(em => em.LeadId == leadid && em.CompanyID == comid && em.BranchID == branchid).ToList();
                                        if (GetLeadsDescription != null)
                                        {
                                            foreach (var item in GetLeadsDescription)
                                            {
                                                description += item.Date + "-" + item.Description + "\r\n";
                                            }
                                        }
                                        //if (GetLeadsDescription != null)
                                        //{
                                        var getleadtbl = db.crm_createleadstbl.Where(em => em.Id == leadid && em.CompanyID == comid && em.BranchID == branchid).ToList();
                                        if (getleadtbl != null)
                                        {
                                            foreach (var item in getleadtbl)
                                            {
                                                DataRow dr = dt.NewRow();
                                                dr["Customer Name"] = item.Customer;
                                                dr["Organization Name"] = item.OrganizationName;
                                                dr["Mobile No"] = item.MobileNo;
                                                dr["WhatsApp Number"] = item.ExtraCol1;
                                                dr["Email"] = item.EmailId;
                                                int countryid = Convert.ToInt32(item.CountryID);
                                                var getcountry = db.acc_countries.Where(em => em.id == countryid).FirstOrDefault();
                                                if (getcountry != null)
                                                {
                                                    dr["Country"] = getcountry.country_name;
                                                }
                                                else
                                                {
                                                    dr["Country"] = item.Country;
                                                }
                                                int stateid = Convert.ToInt32(item.StateID);
                                                var getstate = db.com_state.Where(em => em.ID == stateid).FirstOrDefault();
                                                if (getstate != null)
                                                {
                                                    dr["State"] = getstate.State;
                                                }
                                                else
                                                {

                                                    dr["State"] = item.State;
                                                }
                                                int Cityid = Convert.ToInt32(item.CityID);
                                                var getcity = db.com_city.Where(em => em.ID == Cityid).FirstOrDefault();
                                                if (getcity != null)
                                                {
                                                    dr["City"] = getcity.City;
                                                }
                                                else
                                                {

                                                    dr["City"] = item.City;
                                                }
                                                dr["Followup Date"] = item.FollowDate;
                                                dr["Lead Status"] = item.LeadStatus;
                                                dr["Created By"] = item.CreatedBy;
                                                dr["Assign By"] = item.AssignedBy;
                                                dr["Assign Date"] = item.AssignedDate;
                                                dr["Description"] = description;

                                                dt.Rows.Add(dr);

                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                string description = "";
                                var GetLeadsDescription = db.crm_leaddescriptiontbl.Where(em => em.LeadId == leadid && em.CompanyID == CompanyID && em.BranchID == BranchID).ToList();
                                if (GetLeadsDescription != null)
                                {
                                    foreach (var item in GetLeadsDescription)
                                    {
                                        description += item.Date + "-" + item.Description + "\r\n";
                                    }
                                }
                                //if (GetLeadsDescription != null)
                                //{
                                var getleadtbl = db.crm_createleadstbl.Where(em => em.Id == leadid && em.CompanyID == CompanyID && em.BranchID == BranchID).ToList();
                                if (getleadtbl != null)
                                {
                                    foreach (var item in getleadtbl)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["Customer Name"] = item.Customer;
                                        dr["Organization Name"] = item.OrganizationName;
                                        dr["Mobile No"] = item.MobileNo;
                                        dr["WhatsApp Number"] = item.ExtraCol1;
                                        dr["Email"] = item.EmailId;
                                        int countryid = Convert.ToInt32(item.CountryID);
                                        var getcountry = db.acc_countries.Where(em => em.id == countryid).FirstOrDefault();
                                        if (getcountry != null)
                                        {
                                            dr["Country"] = getcountry.country_name;
                                        }
                                        else
                                        {
                                            dr["Country"] = item.Country;
                                        }
                                        int stateid = Convert.ToInt32(item.StateID);
                                        var getstate = db.com_state.Where(em => em.ID == stateid).FirstOrDefault();
                                        if (getstate != null)
                                        {
                                            dr["State"] = getstate.State;
                                        }
                                        else
                                        {

                                            dr["State"] = item.State;
                                        }
                                        int Cityid = Convert.ToInt32(item.CityID);
                                        var getcity = db.com_city.Where(em => em.ID == Cityid).FirstOrDefault();
                                        if (getcity != null)
                                        {
                                            dr["City"] = getcity.City;
                                        }
                                        else
                                        {

                                            dr["City"] = item.City;
                                        }
                                        dr["Followup Date"] = item.FollowDate;
                                        dr["Lead Status"] = item.LeadStatus;
                                        dr["Created By"] = item.CreatedBy;
                                        dr["Assign By"] = item.AssignedBy;
                                        dr["Assign Date"] = item.AssignedDate;
                                        dr["Description"] = description;

                                        dt.Rows.Add(dr);

                                    }
                                }
                                //}
                                //}
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "Report");
                            if (wb != null)
                            {
                                //Response.ClearHeaders();
                                Response.Clear();
                                string attach = "attachment;filename=Report(" + System.DateTime.Now + ").xlsx";
                                Response.ClearContent();
                                Response.Buffer = true;
                                Response.AddHeader("content-disposition", attach);
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }

                        }
                    }
                }
                Session["exportdatasmg12"] = 1;
                return Redirect("/home/viewleads/?page=1");

                //return Redirect("/home/OurMappeduserSales/"+"17");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
                //return Redirect("/home/OurMappeduserSales/" + "17");
            }
        }


        public ActionResult filterleadsReport(string FromDate, string ToDate)
        {
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    var UID = Convert.ToInt32(Session["UID"]);

                    #region Data-time-Formate
                    var dd = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    #region To-CheckFilter-Date
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        MStartDate = FromDate;
                        MEndDate = ToDate;
                        ViewBag.Fromdate = FromDate;
                        ViewBag.Todate = ToDate;
                    }
                    #endregion

                    #endregion

                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    var GetLeadsRecords = (from dr in GetRecords.AsEnumerable()
                                           select new ViewLeadsModel()
                                           {
                                               Id = Convert.ToInt32(dr["ID"]),
                                               LeadName = Convert.ToString(dr["Customer"]),
                                               Mob = Convert.ToString(dr["MobileNo"]),
                                               EMail = Convert.ToString(dr["EmailId"]),
                                               Country = Convert.ToString(dr["Country"]),
                                               State = Convert.ToString(dr["State"]),
                                               City = Convert.ToString(dr["City"]),
                                               FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
                                               Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                               LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                               AssinedTo = Convert.ToString(dr["AssinedTo"]),
                                               AssignBy = Convert.ToString(dr["AssinedBy"]),
                                           }).ToList();




                    var GetLeadDescription = db.crm_leaddescriptiontbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    #region FollowUp
                    int followup = 0;
                    int Missedfollowup = 0;

                    #region Our-Lead
                    foreach (var item in GetLeadsRecords)
                    {

                        var FilterDiscriptionData = GetLeadDescription.Where(em => em.LeadId == item.Id && em.Date == item.FollowupDate).FirstOrDefault();
                        if (FilterDiscriptionData != null)
                        {
                            followup++;
                        }
                        else
                        {
                            Missedfollowup++;
                        }
                    }
                    #endregion


                    ViewBag.FollowUp = followup;
                    ViewBag.MissedFollowUp = Missedfollowup;

                    #endregion

                    #region New-Leads
                    var CurrentDate = dd.ToString("dd/MM/yyyy");
                    var NewLeadsRecords = GetLeadsRecords.ToList().Count();//GetLeadsRecords.Where(em => em.date == CurrentDate).ToList().Count();
                    int newCount = Convert.ToInt32(NewLeadsRecords);
                    ViewBag.NewLeadsCount = newCount;

                    #endregion

                    #region Not-Interested
                    var NotInterestedRecords = GetLeadsRecords.Where(em => em.LeadStatus == "Not Interested").ToList().Count();
                    int notinterestCount = Convert.ToInt32(NotInterestedRecords);
                    ViewBag.NotInterestedCount = notinterestCount;
                    #endregion

                    #region Closed
                    var ClosedRecords = GetLeadsRecords.Where(em => em.LeadStatus == "Closed").ToList().Count();
                    int ClosedCount = Convert.ToInt32(ClosedRecords); ;
                    ViewBag.ClosedRecordsCount = ClosedCount;

                    #endregion

                    #region Suspect-Leads
                    var SuspectLeadsRecords = GetLeadsRecords.Where(em => em.LeadStatus == "Suspect").ToList().Count();
                    int SuspectCount = Convert.ToInt32(SuspectLeadsRecords);
                    ViewBag.SuspectLeadsCount = SuspectCount;
                    #endregion

                    #region Prospect-Leads
                    var ProspectRecords = GetLeadsRecords.Where(em => em.LeadStatus == "Prospect").ToList().Count();
                    int prospectCount = Convert.ToInt32(ProspectRecords);
                    ViewBag.ProspectCount = prospectCount;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_LeadReportPartial");
        }

        [HttpPost]
        public async Task<ActionResult> SingleAssignLeadToUser(Int64 LeadID, String LeadStatus, int? UserAssignTo)
        {
            var encode = new StringCipher();
            try
            {
                int LoggedBranchID = Convert.ToInt32(Session["BranchID"]);
                int LoggedCompanyID = Convert.ToInt32(Session["CompanyID"]);

                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                if (UserAssignTo != null)
                {
                    var data = cr.GetUserCompanyBranch(UserAssignTo ?? 0);
                    if (data != null)
                    {
                        BranchID = data.BranchID;
                        CompanyID = data.CompanyID;
                    }
                }
                DateTime utcTime = DateTime.UtcNow;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                var time = localTime.ToString("hh:mm:ss tt");
                var Date = localTime.ToString("dd/MM/yyyy");
                if (UserAssignTo > 0)
                {
                    crm_leadassignhistorytbl LAS = new crm_leadassignhistorytbl();
                    LAS.LeadId = LeadID;
                    LAS.LeadAssignBy = Convert.ToInt32(Session["UID"]);
                    LAS.LeadAssignTo = UserAssignTo;
                    LAS.LeadAssignDate = localTime.Date.ToString("dd/MM/yyyy");
                    LAS.LeadStatus = Convert.ToString(LeadStatus);
                    LAS.CreatedDate = localTime;
                    LAS.BranchID = BranchID;
                    LAS.CompanyID = CompanyID;
                    db.crm_leadassignhistorytbl.Add(LAS);

                    var createLead = await db.crm_createleadstbl.Where(em => em.Id == LAS.LeadId && em.BranchID == LoggedBranchID && em.CompanyID == LoggedCompanyID).FirstOrDefaultAsync();
                    if (createLead != null)
                    {
                        createLead.AssignTo = Convert.ToString(UserAssignTo);
                        createLead.BranchID = BranchID;
                        createLead.CompanyID = CompanyID;
                        createLead.AssignedDate = localTime;
                        createLead.AssignedBy = Convert.ToString(Session["UID"]);
                        createLead.IsLeadReminder = true;
                    }
                    await db.SaveChangesAsync();

                    TempData["success"] = "Lead assigned successfully";
                }
                else
                {
                    TempData["alert"] = "** Please select user to assign";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["Alert"] = ex.Message.ToString();
            }
            return RedirectToAction("createleads", new { id = encode.Encrypt(LeadID.ToString()) });
            //return Redirect("/home/createleads/" + LeadID + "");
        }

        [HttpPost]
        public async Task<ActionResult> AssignLeadToUser(ViewLeadsModel LMM, int? UserAssignTo, string SelectedLead)
        {
            try
            {
                if (Session["UID"] != null)
                {
                    int LoggedBranchID = Convert.ToInt32(Session["BranchID"]);
                    int LoggedCompanyID = Convert.ToInt32(Session["CompanyID"]);

                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    if (UserAssignTo != null)
                    {
                        var data = cr.GetUserCompanyBranch(UserAssignTo ?? 0);
                        if (data != null)
                        {
                            BranchID = data.BranchID;
                            CompanyID = data.CompanyID;
                        }
                    }

                    #region calculate the Assigned Leads
                    string Assignedlist = string.Empty;

                    if (!string.IsNullOrEmpty(SelectedLead))
                    {
                        Assignedlist = SelectedLead;
                    }
                    #endregion
                    //Int32 CountAssign = 0;
                    LMM.AssignTo = Convert.ToString(UserAssignTo);

                    if (Assignedlist != string.Empty && LMM.AssignTo != null)
                    {
                        var SpliteAssignedLeads = Assignedlist.Split('|');
                        if (SpliteAssignedLeads[0] == "on")
                        {
                            SpliteAssignedLeads = SpliteAssignedLeads.Where((item, index) => index != 0).ToArray();
                        }
                        var AssignedId = Convert.ToInt32(LMM.AssignTo);

                        DateTime localTime = Constant.GetBharatTime();

                        var time = localTime.ToString("hh:mm:ss tt");
                        var Date = localTime.ToString("dd/MM/yyyy");

                        crm_leadassignhistorytbl LAS = new crm_leadassignhistorytbl();

                        foreach (var item in SpliteAssignedLeads)
                        {
                            var SpliteItem = item.Split('/');
                            string Value = Convert.ToString(SpliteItem[0]);
                            var isNumber = Value.All(char.IsNumber);//to check all charcter is numeric
                            if (isNumber)
                            {
                                LAS.LeadId = Convert.ToInt64(SpliteItem[0]);
                                LAS.LeadAssignBy = Convert.ToInt32(Session["UID"]);
                                LAS.LeadAssignTo = AssignedId;
                                LAS.LeadAssignDate = localTime.Date.ToString("dd/MM/yyyy");
                                LAS.LeadStatus = Convert.ToString(SpliteItem[1]);
                                LAS.CreatedDate = localTime;
                                LAS.BranchID = BranchID;
                                LAS.CompanyID = CompanyID;
                                db.crm_leadassignhistorytbl.Add(LAS);

                                var createLead = await db.crm_createleadstbl.Where(em => em.Id == LAS.LeadId && em.BranchID == LoggedBranchID && em.CompanyID == LoggedCompanyID).FirstOrDefaultAsync();
                                if (createLead != null)
                                {
                                    createLead.AssignTo = AssignedId.ToString();
                                    createLead.BranchID = BranchID;
                                    createLead.CompanyID = CompanyID;
                                    createLead.AssignedDate = localTime;
                                    createLead.AssignedBy = Convert.ToString(Session["UID"]);
                                    createLead.IsLeadReminder = true;
                                }
                                db.SaveChanges();
                                //CountAssign++;
                            }
                        }

                        if (LAS.Id > 0)
                            TempData["success"] = "Lead assigned successfully";

                    }
                    else
                    {
                        TempData["alert"] = "Please select the Leads and User to Assign";
                    }

                    TempData["UserddlName"] = Session["UserddlName"];
                    TempData["filterText"] = Session["filterText"];
                    TempData["VLFltrFilterType"] = Session["VLFltrFilterType"];
                    TempData["VLFltrFrmDt"] = Session["VLFltrFrmDt"];
                    TempData["VLFltrToDt"] = Session["VLFltrToDt"];
                    TempData["cpage"] = LMM.page;
                    TempData["VLFilterTerm"] = Session["FilterTerm"];
                    TempData["VLFltrProductTypeID"] = Session["ProductTypeID"];
                    TempData["VLFltrLeadSourceID"] = Session["LeadSourceID"];

                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/home/viewleads");
            //return Redirect("/home/viewleads/?page=1&UserddlName=" + Session["UserddlName"] + "&filterText=" + Session["filterText"] + "&FilterType=" + Session["VLFltrFilterType"] + "&FromDate=" + Session["VLFltrFrmDt"] + "&ToDate=" + Session["VLFltrToDt"] + "");
        }

        public ActionResult FilterAssignedByDates(string FilterType, string FromDate, string ToDate)
        {
            try
            {
                Session["VLFltrFilterType"] = FilterType;
                Session["AssignedFrmDt"] = FromDate;
                Session["AssignedToDt"] = ToDate;
                return Redirect("/home/AssignLeadShow/?page=1");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["Alert"] = "";
                return View();
            }
        }

        public ActionResult ResetAssignLeadsFilter()
        {
            Session["VLFltrFilterType"] = null;
            Session["AssignedFrmDt"] = null;
            Session["AssignedToDt"] = null;
            Session["Userddllist"] = "";
            Session["DDLFilterText"] = null;
            return Redirect("/home/AssignLeadShow/?page=1");
        }

        #region Export Count of leads
        public ActionResult ExportCountOfLeads()
        {
            try
            {
                //var ViewSalesLeads = (List<ViewLeadsModel>)TempData["MappedUser-Leads"];

                DataTable dt = new DataTable();
                dt.Columns.Add("Row Labels", typeof(string));
                dt.Columns.Add("Count", typeof(Int32));
                #region Add Row
                DataRow dr = dt.NewRow();
                dr["Row Labels"] = "New Leads";
                dr["Count"] = TempData["NewLeadsCount"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Follow up";
                dr["Count"] = TempData["FollowUp"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Missed follow up";
                dr["Count"] = TempData["MissedFollowUp"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Delayed follow up";
                dr["Count"] = TempData["Delayedfollowup"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Not interested";
                dr["Count"] = TempData["NotInterestedCount"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Closed Leads";
                dr["Count"] = TempData["ClosedRecordsCount"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Suspect Leads";
                dr["Count"] = TempData["SuspectLeadsCount"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Prospect Leads";
                dr["Count"] = TempData["ProspectCount"].ToString();
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["Row Labels"] = "Assign To Others";
                dr["Count"] = TempData["AssignToOthers"].ToString();
                dt.Rows.Add(dr);
                #endregion
                if (dt.Rows.Count > 0)
                {

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Lead-Report");
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=Lead-Report.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                return Json("done", JsonRequestBehavior.AllowGet);
                //return Redirect("/home/OurMappeduserSales/"+"17");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
                //return Redirect("/home/OurMappeduserSales/" + "17");
            }
        }
        #endregion

        #endregion

        #region Sales-Management
        public ActionResult viewsales(int? page)
        {
            ViewSalesModel VSM = new ViewSalesModel();
            try
            {
                if (Session["UserName"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    VSM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    var dd = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

                    if (Session["SalesFltrFrmDt"] != null && Session["SalesFltrToDt"] != null)
                    {

                        if (VSM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {

                            var FromDate = Convert.ToString(Session["SalesFltrFrmDt"]);
                            var ToDate = Convert.ToString(Session["SalesFltrToDt"]);

                            var fmDate = DateTime.ParseExact(FromDate, VSM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, VSM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            MStartDate = Convert.ToString(Session["SalesFltrFrmDt"]);
                            MEndDate = Convert.ToString(Session["SalesFltrToDt"]);
                        }
                    }
                    else
                    {
                        if (VSM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            Session["SalesFltrFrmDt"] = monthStartDate.ToString(VSM.DateFormat);
                            Session["SalesFltrToDt"] = MonthendDate.ToString(VSM.DateFormat);
                        }
                        else
                        {
                            Session["SalesFltrFrmDt"] = MStartDate;
                            Session["SalesFltrToDt"] = MEndDate;
                        }
                    }

                    int UID = 0;
                    int LoginUserId = Convert.ToInt32(Session["UID"]);
                    if (Session["dllUserName"] != null || Session["dllUserName"] != "")
                    {
                        Session["dllUserName"] = 0;
                        UID = Convert.ToInt32(Session["dllUserName"]);
                    }

                    if (Convert.ToString(Session["UserType"]) != "SuperAdmin" && Session["dllUserName"] == null)
                    {
                        UID = Convert.ToInt32(Session["UID"]);
                    }
                    //if (Session["dllUserName"] == "")
                    //{
                    //    UID = Convert.ToInt32(Session["UID"]);
                    //}

                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        //var q = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true && em.ProfileName.ToLower().Contains("sales")).OrderBy(em => em.Fname).ToList();
                        string userquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName , ur.EmployeeCode
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewLeads = 1";
                        var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                        VSM.Userddllist = new List<Userddl>();
                        //Userddl u = new Userddl();
                        if (q != null && q.Count > 0)
                        {
                            foreach (var item in q)
                            {
                                var user = new Userddl
                                {
                                    uid = Convert.ToInt32(item.UserID),
                                    // UserName = item.Fname + " " + item.Lname
                                    UserName = item.UserName + "    (" + item.EmployeeCode + ")",
                                };
                                VSM.Userddllist.Add(user);
                            }
                        }
                    }
                    else
                    {
                        var GetUserData = db.crm_usertbl.Where(em => em.Id == LoginUserId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetUserData != null && GetUserData.MappedUsers != null)
                        {
                            var GetMapUser = GetUserData.MappedUsers.Split(',');
                            VSM.Userddllist = new List<Userddl>();
                            Userddl u1 = new Userddl();
                            u1.uid = UID;
                            u1.UserName = Session["UserName"].ToString();
                            VSM.Userddllist.Add(u1);

                            foreach (var item in GetMapUser)
                            {
                                var mapid = Convert.ToInt32(item);
                                var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                if (GetMapUserData != null)
                                {
                                    var user = new Userddl
                                    {
                                        uid = mapid,
                                        UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
                                    };
                                    VSM.Userddllist.Add(user);
                                }
                            }
                        }
                        else
                        {
                            VSM.Userddllist = new List<Userddl>();
                            Userddl u1 = new Userddl();
                            u1.uid = UID;
                            u1.UserName = Session["UserName"].ToString();
                            VSM.Userddllist.Add(u1);
                        }
                    }

                    //DataTable GetSales = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    DataTable GetSales = DataAccessLayer.GetDataTable(" call CRM_GetViewSales('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetSales.Rows.Count > 0)
                    {
                        VSM.viewsalesList = (from dr in GetSales.AsEnumerable()
                                             select new ViewSalesModel()
                                             {
                                                 Id = Convert.ToInt32(dr["Id"]),
                                                 LeadName = Convert.ToString(dr["Customer"]),
                                                 FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                 LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                 Mob = Convert.ToString(dr["MobileNo"]),
                                                 ClosedBy = Convert.ToString(dr["SalePersonName"]),
                                                 AdvancePayment = Convert.ToString(dr["ADVANCEPAYMENT"]),
                                                 ProjectValue = Convert.ToString(dr["PROJECTVALUE"]),
                                             })/*.Where(em => em.LeadStatus == "Closed")*/.ToList();

                    }

                    #region Pagging-Start
                    int pageNumber = 1;
                    int pageSize = 100;
                    int pages = 0;
                    if (page == null)
                    {
                        pageNumber = 1;
                    }
                    else
                    {
                        pageNumber = Convert.ToInt32(page);
                    }
                    int TotalProducts = 0;
                    int Rem = 0;
                    int pageSkip = (pageNumber - 1) * pageSize;

                    TotalProducts = VSM.viewsalesList.Count();
                    pages = (TotalProducts / pageSize);
                    var Product = VSM.viewsalesList.Skip(pageSkip).Take(pageSize).ToList();
                    VSM.viewsalesList = Product;
                    pages = (TotalProducts / pageSize);
                    Rem = (TotalProducts % pageSize);
                    if (Rem < pageSize && Rem != 0)
                    {
                        pages = (pages + 1);
                    }
                    ViewData["NoOfPages"] = pages;

                    //For Page Index Count.......
                    if (page > 1)
                    {
                        var DeclareIndex = (pageSize * (page - 1)) + 1;
                        ViewData["DeclareIndex"] = DeclareIndex;
                    }
                    else
                    {
                        ViewData["DeclareIndex"] = 1;
                    }
                    #endregion
                }
                else
                {
                    Session["ReturnPath"] = "/home/viewsales";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(VSM);
        }

        public ActionResult FilterSalesByDates(string UserddlName, string FromDate, string ToDate)
        {
            try
            {
                Session["dllUserName"] = UserddlName;
                Session["SalesFltrFrmDt"] = FromDate;
                Session["SalesFltrToDt"] = ToDate;
                return Redirect("/home/viewsales/?page=1");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["Alert"] = "";
                return View();
            }
        }

        public ActionResult ResetSalesFilter()
        {
            Session["UserddlName"] = null;
            Session["SalesFltrFrmDt"] = null;
            Session["SalesFltrToDt"] = null;
            return Redirect("/home/viewsales/?page=1");
        }

        public ActionResult ViewSalesPaymentdetails(int Leadid)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var GetSaleDetail = db.crm_saledetailtbl.Where(em => em.FK_LEADID == Leadid && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
            PaymentModel PM = new PaymentModel();
            if (GetSaleDetail != null)
            {
                //PM.BankName = getData.BankName;
                //PM.AccountDetails = getData.AccountDetails;
                //PM.ProductDetails = getData.ProductDetails;
                //PM.ValidFrom = getData.ValidFrom;
                //PM.ValidTo = getData.ValidTo;
                //PM.Amount = Convert.ToDecimal(getData.Amount);
                //PM.Discount = Convert.ToDecimal(getData.Discount);
                //PM.PayComment = Convert.ToString(getData.PaymentComment);

            }

            return PartialView("_PartialViewPaymentDetails", PM);
        }

        #endregion

        #region Archives-Management
        public ActionResult viewmonthly(string StartDate, string EndDate)
        {
            ViewArchivesModel VAM = new ViewArchivesModel();
            try
            {
                if (Session["UserName"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    var UID = Convert.ToInt32(Session["UID"]);

                    #region Get the Start-Date && End-date of Current Month
                    var MonthStartDate = "";
                    var MonthEndDate = "";
                    if (StartDate != null && EndDate != null)
                    {
                        MonthStartDate = StartDate;
                        MonthEndDate = EndDate;
                    }
                    else
                    {
                        var GetPrevMonthDate = DateTime.Now.AddMonths(-1);
                        var dd = Convert.ToDateTime(GetPrevMonthDate);
                        DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                        DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                        MonthStartDate = monthStartDate.ToString("dd/MM/yyyy");
                        MonthEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    }
                    TempData["DisplayDate"] = MonthStartDate + " - " + MonthEndDate;
                    #endregion

                    DataTable GetData = DataAccessLayer.GetDataTable(" call CRM_LeadsBetwenDates('" + MonthStartDate + "','" + MonthEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetData.Rows.Count > 0)
                    {
                        VAM.viewarchivesList = (from dr in GetData.AsEnumerable()
                                                select new ViewArchivesModel()
                                                {
                                                    LeadOwnerID = Convert.ToInt32(dr["LeadOwner"]),
                                                    LeadOwner = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                                    FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                                    ProductName = Convert.ToString(dr["ProductName"]),
                                                    LeadStatus = Convert.ToString(dr["LeadStatusName"])
                                                }).Where(em => em.LeadOwnerID == UID).ToList();

                    }
                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(VAM);
        }

        public ActionResult ourUsersArchives()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    MapUserModel MUM = new MapUserModel();
                    var UID = Convert.ToInt32(Session["UID"]);
                    var GetData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        var MapUserList = GetData.MappedUsers.Split(',');
                        foreach (var item in MapUserList)
                        {
                            var ID = Convert.ToInt32(item);
                            var GetUserDetails = db.crm_usertbl.Where(em => em.Id == ID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            var cal = new MapUserModel
                            {
                                Id = GetUserDetails.Id,
                                UserName = GetUserDetails.Fname + " " + GetUserDetails.Lname
                            };
                            MUM.mapuserlist.Add(cal);

                        }
                        ViewBag.userMappedList = new SelectList(MUM.mapuserlist, "Id", "UserName");
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/home/ourUsersArchives";
                    return Redirect("/home/login");
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View();
        }

        #endregion

        #region Message-Management
        public ActionResult message()
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    //var GetProduct = db.Producttbls.Where(em => em.Status == true).ToList().Select(em => new Producttbl
                    //{
                    //    Id = em.Id,
                    //    ProductName = em.ProductName
                    //}).AsQueryable();
                    //ViewBag.product = new SelectList(GetProduct, "Id", "ProductName");

                }
                else
                {
                    Session["ReturnUrl"] = "/home/message";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View();
        }

        [HttpPost]
        public ActionResult message(string calUserList, string txtMessage)
        {
            string Mappeduserlist = null;
            if (Request.Form["calUserList"] != null)
            {
                Mappeduserlist = Request.Form["calUserList"].ToString();
            }
            if (Mappeduserlist != null && txtMessage != null)
            {
                TempData["successAlert"] = "Message sent successfully";
            }
            else
            {
                TempData["erroralert"] = "Please provide the proper information";
            }
            return Redirect("/home/message");
        }

        public ActionResult GetLeadsByProdyct(string product)
        {
            try
            {
                // ViewBag.reult = db.P_GetLeadsbyProduct(product).Where(em => em.LeadStatus == "Close won").ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialGetLeadsbyProduct");
        }

        public ActionResult MessagePageProductNameListDisplay(string ProductList)
        {
            try
            {
                var SpliteData = ProductList.Split(',');
                ViewBag.result = SpliteData;
                //List<string> st = new List<string>();
                //foreach (var item in SpliteData)
                //{
                //    st.Add(item);
                //    //var Top = new FirmDataModelInfo
                //    //{
                //    //    FarmName = item
                //    //};
                //    //DTM.firmDatamodelinfoList.Add(Top);

                //}
                //var get = st;
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialMessagePageViewproduct");
        }

        #endregion

        #region Role-Management || User-Management
        public ActionResult users(string Empcode, int? FilterBranchId, int? companytypeid)
        {
            var model = new ViewUserModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (Session["UserName"] != null)
                {
                    if (FilterBranchId > 0)
                    {
                        BranchID = FilterBranchId ?? 0;
                    }
                    //ViewBag.result = db.crm_usertbl.Where(em => em.ProfileName != "SuperAdmin" && em.ProfileId!=null && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(em => em.Fname).ThenBy(em => em.Lname).ToList();
                    if (CompanyID == 2644)
                    {
                        if (companytypeid != null)
                        {
                            var data1 = "select CU.Id,CU.Fname,CU.Lname,CU.ContactNumber,CU.EmployeeCode,CU.Email,CU.Status,CU.UserName,CU.ProfileName,CUM.CompanyTypeName,CU.Designation from crm_usertbl CU join crm_UserCompanytypetbl CUM on CU.CompanyTypeId=CUM.Id where CU.BranchID='" + BranchID + "' and CU.CompanyTypeId='" + companytypeid + "'";
                            var data = db.Database.SqlQuery<UsersModel>(data1).ToList();
                            if (data != null && data.Count > 0)
                            {
                                model.UsersList = (from item in data
                                                   select new UsersModel
                                                   {
                                                       Id = item.Id,
                                                       Fname = item.Fname,
                                                       Lname = item.Lname,
                                                       ContactNumber = item.ContactNumber,
                                                       EmployeeCode = item.EmployeeCode,
                                                       Email = item.Email,
                                                       Status = (bool)item.Status,
                                                       UserName = item.UserName,
                                                       ProfileName = item.ProfileName,
                                                       CompanyTypeName = item.CompanyTypeName,
                                                       Designation = item.Designation
                                                   }
                                                 ).ToList();
                            }
                        }
                        if (Empcode != null)
                        {
                            var data1 = "select CU.Id,CU.Fname,CU.Lname,CU.ContactNumber,CU.EmployeeCode,CU.Email,CU.Status,CU.UserName,CU.ProfileName,CUM.CompanyTypeName,CU.Designation from crm_usertbl CU join crm_UserCompanytypetbl CUM on CU.CompanyTypeId=CUM.Id where CU.BranchID='" + BranchID + "' and CU.EmployeeCode='" + Empcode + "'";
                            var data = db.Database.SqlQuery<UsersModel>(data1).ToList();
                            if (data != null && data.Count > 0)
                            {
                                model.UsersList = (from item in data
                                                   select new UsersModel
                                                   {
                                                       Id = item.Id,
                                                       Fname = item.Fname,
                                                       Lname = item.Lname,
                                                       ContactNumber = item.ContactNumber,
                                                       EmployeeCode = item.EmployeeCode,
                                                       Email = item.Email,
                                                       Status = (bool)item.Status,
                                                       UserName = item.UserName,
                                                       ProfileName = item.ProfileName,
                                                       CompanyTypeName = item.CompanyTypeName,
                                                       Designation = item.Designation
                                                   }
                                                 ).ToList();
                            }
                        }
                        else
                        {
                            var data1 = "select CU.Id,CU.Fname,CU.Lname,CU.ContactNumber,CU.EmployeeCode,CU.Email,CU.Status,CU.UserName,CU.ProfileName,CUM.CompanyTypeName,CU.Designation from crm_usertbl CU join crm_UserCompanytypetbl CUM on CU.CompanyTypeId=CUM.Id where CU.BranchID='" + BranchID + "'";

                            var data = db.Database.SqlQuery<UsersModel>(data1).ToList();
                            if (data != null && data.Count > 0)
                            {
                                model.UsersList = (from item in data
                                                   select new UsersModel
                                                   {
                                                       Id = item.Id,
                                                       Fname = item.Fname,
                                                       Lname = item.Lname,
                                                       ContactNumber = item.ContactNumber,
                                                       EmployeeCode = item.EmployeeCode,
                                                       Email = item.Email,
                                                       Status = (bool)item.Status,
                                                       UserName = item.UserName,
                                                       ProfileName = item.ProfileName,
                                                       CompanyTypeName = item.CompanyTypeName,
                                                       Designation = item.Designation
                                                   }
                                                 ).ToList();
                            }
                        }

                    }


                    else
                    {
                        var data = db.crm_usertbl.Where(em => em.ProfileName != "SuperAdmin" && em.ProfileId != null && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(em => em.Fname).ThenBy(em => em.Lname).ToList();
                        if (data != null && data.Count > 0)
                        {
                            model.UsersList = (from item in data
                                               select new UsersModel
                                               {
                                                   Id = item.Id,
                                                   Fname = item.Fname,
                                                   Lname = item.Lname,
                                                   ContactNumber = item.ContactNumber,
                                                   EmployeeCode = item.EmployeeCode,
                                                   Email = item.Email,
                                                   Status = (bool)item.Status,
                                                   UserName = item.UserName,
                                                   ProfileName = item.ProfileName,
                                                   Designation = item.Designation
                                               }
                                             ).ToList();
                        }
                    }
                    var getbranchList = db.com_branches.Where(em => em.OrganizationID == CompanyID).ToList();

                    model.BranchList = new SelectList(getbranchList, "OrgBranchCode", "BranchName", BranchID);

                    #region CompanyType
                    string ptQury = @"select Id as CompanyTypeID, CompanyTypeName from crm_UserCompanytypetbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and Status=1";
                    var getCompanyType = db.Database.SqlQuery<ViewUserModel>(ptQury).OrderBy(a => a.CompanyTypeName).ToList();
                    if (getCompanyType.Count > 0)
                    {
                        model.CompanyTypeList = getCompanyType;
                    }
                    #endregion
                }
                else
                {
                    Session["ReturnUrl"] = "/home/users";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/home/login");
            }

            return View(model);
        }

        public ActionResult roleinformation()
        {
            return View();
        }


        #region Create-Users
        public ActionResult createusers(int? id)
        {
            CreateUserModel CUM = new CreateUserModel();
            try
            {
                if (Session["UserName"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    //var permission = new CommonRepository();

                    var userList = new List<crm_usertbl>();
                    #region Select-TimeZone
                    var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList().Select(em => new crm_zonetbl
                    {
                        ZoneName = em.ZoneName
                    }).AsQueryable();
                    ViewBag.TimeZoneName = new SelectList(GetZoneName, "ZoneName", "ZoneName");
                    #endregion
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        ViewBag.result = db.crm_roleassigntbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        //userList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        ViewBag.UserList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.ProfileId != null && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    }
                    else
                    {
                        var UID = id;
                        var profiletype = Convert.ToString(Session["UserType"]);
                        //if (profiletype == "Admin" || profiletype == "admin")
                        if (profiletype == "SuperAdmin" || profiletype == "superddmin")
                        {
                            ViewBag.result = db.crm_roleassigntbl.Where(em => em.ProfileName != profiletype && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        }
                        else
                        {
                            ViewBag.result = db.crm_roleassigntbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        }
                        userList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.Id != UID && em.ProfileId != null && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        ViewBag.UserList = userList;
                    }
                    #region CompanyType
                    string ptQury = @"select Id as CompanyTypeID, CompanyTypeName from crm_UserCompanytypetbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and Status=1";
                    var getCompanyType = db.Database.SqlQuery<CreateUserModel>(ptQury).OrderBy(a => a.CompanyTypeName).ToList();
                    if (getCompanyType.Count > 0)
                    {
                        CUM.CompanyTypeList = getCompanyType;
                    }
                    #endregion
                    #region Get other branch user list assigned by sa
                    var FromDate = string.Empty;
                    var ToDate = string.Empty;
                    DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + BranchID + "','" + FromDate + "','" + ToDate + "')");
                    var GetOtherBranchUserList = (from dr in GetRecords.AsEnumerable()
                                                  select new ManageOrganizationVM()
                                                  {
                                                      //Id = Convert.ToInt32(dr["Id"]),
                                                      //AssignToCompanyID = Convert.ToInt32(dr["AssignToCompanyID"]),
                                                      Id = Convert.ToInt32(dr["AssignedUserID"]),
                                                      BranchName = Convert.ToString(dr["BranchName"]),
                                                      //AssignedBranchName = Convert.ToString(dr["AssignedBranchName"]),                                              
                                                      //AssignedDate = Convert.ToString(dr["AssignedDate"]),
                                                      UserName = Convert.ToString(dr["UserName"]),
                                                      EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                                      ProfileName = Convert.ToString(dr["ProfileName"]),
                                                      EmailID = Convert.ToString(dr["EmailID"]),
                                                      IsActive = Convert.ToBoolean(dr["IsActive"])
                                                  }).Where(a => a.IsActive == true).ToList();
                    if (GetOtherBranchUserList.Count > 0)
                    {
                        CUM.OtherBranchUserList = GetOtherBranchUserList;
                    }
                    #endregion

                    #region AssignItem

                    if (id != null)
                    {
                        var data1 = @"select ITM.Id, ITM.ItemTypeName , IAM.ItemTypeId,IAM.Quantity,IAM.Estimated_Cost,IAM.Expirydate,IAM.CheckStatus,IAM.SerialNo,IAM.UserId,IAM.Assigndate from 
                                crm_itemtypetbl ITM join crm_assignitemtypeuser IAM
                                on ITM.Id = IAM.ItemTypeId where ITM.BranchID = '" + BranchID + "' and ITM.CompanyID = '" + CompanyID + "' and IAM.UserId='" + id + "'";

                        Session["ItemId"] = Convert.ToInt32(id);

                        var itemdata = db.Database.SqlQuery<ItemAssignlist>(data1).ToList();
                        if (itemdata != null && itemdata.Count > 0)
                        {
                            CUM.ItemTypeList = (from item in itemdata
                                                select new ItemAssignlist
                                                {
                                                    Id = item.Id,
                                                    ItemTypeId = item.ItemTypeId,
                                                    UserId = item.UserId,
                                                    ItemTypeName = item.ItemTypeName,
                                                    Quantity = item.Quantity,
                                                    Estimated_Cost = item.Estimated_Cost,
                                                    Expirydate = item.Expirydate,
                                                    CheckStatus = item.CheckStatus,
                                                    SerialNo = item.SerialNo,
                                                    Assigndate = item.Assigndate
                                                }
                                             ).ToList();
                        }
                    }


                    var dataall = @"select Id,ItemTypename from  crm_itemtypetbl where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'";

                    var allitemdata = db.Database.SqlQuery<ItemAssignlist>(dataall).ToList();
                    if (allitemdata != null && allitemdata.Count > 0)
                    {
                        CUM.AllItemTypeList = (from item in allitemdata
                                               select new ItemAssignlist
                                               {
                                                   Id = item.Id,
                                                   ItemTypeName = item.ItemTypeName,

                                               }
                                         ).ToList();
                    }

                    if (CompanyID == 1153 || CompanyID == 2682)
                    {
                        var Customegroupdataall = @"select ID,CustGroupName from  customersgroup where BranchCode = '" + BranchID + "' and CompanyId = '" + CompanyID + "'";

                        var allcustomgrpdata = db.Database.SqlQuery<Customergrouplist>(Customegroupdataall).ToList();
                        if (allcustomgrpdata != null && allcustomgrpdata.Count > 0)
                        {
                            CUM.Allcustgrouplist = (from item in allcustomgrpdata
                                                    select new Customergrouplist
                                                    {
                                                        ID = item.ID,
                                                        CustGroupName = item.CustGroupName,

                                                    }
                                             ).ToList();
                        }
                    }
                    //var GetItemlist = db.crm_itemtypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //var GetItemlist = db.crm_itemtypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //var GetItemlistuser = db.crm_assignitemtypeuser.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    //var data1 = (from sl in GetItemlistuser
                    //             join ul in GetItemlist on sl.ItemTypeId equals ul.Id
                    //             join tl in GetItemlistuser on sl.UserId equals id into slUl
                    //             from d in slUl.DefaultIfEmpty()
                    //             select new CreateUserModel()
                    //             {
                    //                 ItemId = ul.Id,
                    //                 ItemName = ul.ItemTypeName,
                    //                 Quantity = sl.Quantity,
                    //                 Estimated_Cost = sl.Estimated_Cost,
                    //                 Expiry_Date = sl.Expirydate,
                    //                 itemSerialno = sl.SerialNo,
                    //                 itemcheck = sl.CheckStatus
                    //             }).ToList();
                    //CUM.ItemTypeList = data1;
                    //if (GetItemlist.Count > 0)
                    //{
                    //    List<CreateUserModel> ItemList = new List<CreateUserModel>();
                    //    foreach (var item in GetItemlist)
                    //    {
                    //        CreateUserModel Icm = new CreateUserModel();
                    //        Icm.itemuid = item.UserId;
                    //        Icm.itemSerialno = item.Serialno;
                    //        Icm.ItemId = item.Id;
                    //        Icm.ItemName = item.ItemTypeName;
                    //        Icm.Quantity = item.Quantity;
                    //        Icm.Estimated_Cost = item.Estimated_Cost;
                    //        Icm.Expiry_Date = Convert.ToDateTime(item.Expirydate);
                    //        ItemList.Add(Icm);
                    //    }
                    //    CUM.ItemTypeList = ItemList;

                    //}


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

                    var DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    var GetUserData = db.crm_usertbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    //set branchid default if null
                    CUM.BranchID = GetUserData != null && GetUserData.BranchID > 0 ? GetUserData.BranchID : BranchID;
                    if (GetUserData != null)
                    {
                        CUM.UserID = GetUserData.Id;
                        CUM.FirstName = GetUserData.Fname;
                        CUM.LastName = GetUserData.Lname;
                        CUM.FatherName = GetUserData.FatherName;
                        if (GetUserData.DateofBirth != null)
                        {
                            CUM.DateofBirth = String.Format("{0:" + DateFormat + "}", GetUserData.DateofBirth);//GetUserData.DateofBirth.Value.Date.ToString("dd/MM/yyyy");
                        }
                        CUM.ContactNumber = GetUserData.ContactNumber;
                        CUM.ContactNumber = GetUserData.ContactNumber;
                        CUM.AlternateNumber = GetUserData.AlternateNumber;
                        CUM.Email = GetUserData.Email;
                        CUM.Gender = GetUserData.Gender;
                        CUM.EmployeeCode = GetUserData.EmployeeCode;
                        CUM.UserName = GetUserData.UserName;
                        CUM.Designation = GetUserData.Designation;

                        if (GetUserData.EscalateUserId != null)
                        {
                            CUM.EscalateUserId = GetUserData.EscalateUserId;
                            CUM.TempEscalateUserId = GetUserData.EscalateUserId;
                            var reportingMgrId = db.crm_usertbl.Where(a => a.Id == GetUserData.EscalateUserId && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault().EscalateUserId;
                            if (reportingMgrId != null)
                            {
                                CUM.ReportingManagerId = reportingMgrId;
                                CUM.TempReportingManagerId = reportingMgrId;
                            }
                            CUM.EscalateTime = GetUserData.EscalateTime;
                        }

                        if (!string.IsNullOrEmpty(GetUserData.KeyVersion))
                        {
                            #region password decryption
                            byte[] iv1;
                            byte[] key = EncriptAES.getdcriptkey(out iv1);
                            string decryptPwd = EncriptAES.DecryptString(GetUserData.Password, key, iv1);
                            #endregion
                            CUM.UserPassword = decryptPwd;//show decrypted password
                            CUM.ConfirmPassword = decryptPwd;
                        }
                        else
                        {
                            CUM.UserPassword = GetUserData.Password;
                            CUM.ConfirmPassword = GetUserData.Password;
                        }

                        CUM.TimeZoneName = GetUserData.TimeZone;
                        CUM.CurrentAddress = GetUserData.CurrentAddress;
                        CUM.PermanentAddress = GetUserData.PermanentAddress;
                        CUM.RefName1 = GetUserData.RefName1;
                        CUM.RefEmail1 = GetUserData.RefEmail1;
                        CUM.RefPhoneNumber1 = GetUserData.RefPhoneNumber1;
                        CUM.RefName2 = GetUserData.RefName2;
                        CUM.RefEmail2 = GetUserData.RefEmail2;
                        CUM.RefPhoneNumber2 = GetUserData.RefPhoneNumber2;
                        CUM.AadharNumber = GetUserData.Aadhar_No;
                        CUM.Employetype = GetUserData.EmployeType;
                        if (CompanyID == 1153 || CompanyID == 2682)
                        {
                            CUM.Customergroupid = Convert.ToInt32(GetUserData.Customergroupid);

                            //customersgroup pdt = new customersgroup();
                            //var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
                            //for (int i = 0; i < comptype.Count; i++)
                            //{
                            //    int multicompany = Convert.ToInt32(comptype[i].ID);

                            //    var multibranch = db.com_branches.Where(em => em.OrganizationID == multicompany).FirstOrDefault();
                            //    {
                            //        if (multibranch != null)
                            //        {
                            //            int multib = Convert.ToInt32(multibranch.OrgBranchCode);


                            //            var data1 = db.crm_usertbl.Where(a => a.Id == id).FirstOrDefault();
                            //            if (data1.t_LoginId == null)//check if t_login id is null then save it to table
                            //            {
                            //                var dt = Constant.GetBharatTime();
                            //                string uniqUserId = data1.Fname + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                            //                #region Password encryption
                            //                string VersionKey = "";

                            //                VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                            //                                                                                                           //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                            //                byte[] iv1;
                            //                byte[] key = EncriptAES.getdcriptkey(out iv1);
                            //                string ecncryptPwd = EncriptAES.EncryptString(uniqUserId, key, iv1);
                            //                #endregion

                            //                #region get syncid from t_login
                            //                string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                            //                int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                            //                string synId = "O" + (syncid + 1);
                            //                #endregion
                            //                //var tl = new t_login
                            //                //{
                            //                t_login tl = new t_login();
                            //                tl.user_id = uniqUserId;
                            //                tl.user_password = ecncryptPwd;//save encrypted password
                            //                tl.role_id = 7;
                            //                tl.role_name = "Admin";
                            //                tl.UserRole = 7;
                            //                tl.Emp_code = "CRM";
                            //                tl.branch = multib.ToString();
                            //                tl.company_id = multicompany;
                            //                tl.Subscription_StartDate = GetUserData.StartDate;
                            //                tl.Subscription_EndDate = GetUserData.EndDate;
                            //                tl.PaymentStatus = "no";
                            //                tl.CreatedDate = dt;
                            //                tl.IsActive = "yes";
                            //                tl.flag = "N";
                            //                tl.SyncID = synId;
                            //                tl.mastercompany = 0;
                            //                tl.user_type = "crm_user";
                            //                tl.customer_id = "";
                            //                tl.permissions = "";
                            //                tl.KeyVersion = VersionKey;//save latest key version
                            //                                           //};
                            //                db.t_login.Add(tl);
                            //                db.SaveChanges();
                            //            }
                            //                //var checkExist = db.customersgroups.Where(em => em.CustGroupName == CUM.Customergroupid && em.BranchCode == multib && em.CompanyId == multicompany).FirstOrDefault();
                            //                //if (checkExist == null)
                            //                //{
                            //                //    pdt.CustGroupName = CUM.Customergroupid;
                            //                //    pdt.BranchCode = multib;
                            //                //    pdt.CompanyId = multicompany;

                            //                //    db.customersgroups.Add(pdt);
                            //                //    db.SaveChanges();


                            //                //    //return Json(expando, JsonRequestBehavior.AllowGet);

                            //                //}
                            //                //else
                            //                //{

                            //                //    //expando.Msg = "Cus type already exist";
                            //                //    //return Json(expando, JsonRequestBehavior.AllowGet);
                            //                //}
                            //            }
                            //    }

                            //}
                            //var data = db.crm_usertbl.Where(a => a.Id == id && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                            //if (data.t_LoginId == null)//check if t_login id is null then save it to table
                            //{
                            //    var dt = Constant.GetBharatTime();
                            //    string uniqUserId = data.Fname + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                            //    #region Password encryption
                            //    string VersionKey = "";

                            //    VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                            //                                                                                               //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                            //    byte[] iv1;
                            //    byte[] key = EncriptAES.getdcriptkey(out iv1);
                            //    string ecncryptPwd = EncriptAES.EncryptString(uniqUserId, key, iv1);
                            //    #endregion

                            //    #region get syncid from t_login
                            //    string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                            //    int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                            //    string synId = "O" + (syncid + 1);
                            //    #endregion
                            //    //var tl = new t_login
                            //    //{
                            //    t_login tl = new t_login();
                            //    tl.user_id = uniqUserId;
                            //    tl.user_password = ecncryptPwd;//save encrypted password
                            //    tl.role_id = 7;
                            //    tl.role_name = "Admin";
                            //    tl.UserRole = 7;
                            //    tl.Emp_code = "CRM";
                            //    tl.branch = BranchID.ToString();
                            //    tl.company_id = CompanyID;
                            //    tl.Subscription_StartDate = GetUserData.StartDate;
                            //    tl.Subscription_EndDate = GetUserData.EndDate;
                            //    tl.PaymentStatus = "no";
                            //    tl.CreatedDate = dt;
                            //    tl.IsActive = "yes";
                            //    tl.flag = "N";
                            //    tl.SyncID = synId;
                            //    tl.mastercompany = 0;
                            //    tl.user_type = "crm_user";
                            //    tl.customer_id = "";
                            //    tl.permissions = "";
                            //    tl.KeyVersion = VersionKey;//save latest key version
                            //                               //};
                            //    db.t_login.Add(tl);
                            //    db.SaveChanges();
                            //}
                            //var mancompany = db.customersgroups.Where(em => em.CustGroupName == CUM.Customergroupid && em.BranchCode == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                            //if (mancompany == null)
                            //{
                            //    pdt.CustGroupName = CUM.Customergroupid;
                            //    pdt.BranchCode = BranchID;
                            //    pdt.CompanyId = CompanyID;

                            //    db.customersgroups.Add(pdt);
                            //    db.SaveChanges();


                            //    //return Json(expando, JsonRequestBehavior.AllowGet);

                            //}



                        }
                        if (GetUserData.CompanyTypeId != null)
                        {
                            CUM.CompanyTypeID = GetUserData.CompanyTypeId;
                        }
                        if (GetUserData.DOJ != null)
                        {
                            CUM.DateofJoining = String.Format("{0:" + DateFormat + "}", GetUserData.DOJ);//GetUserData.DateOfJOining.Value.Date.ToString("dd/MM/yyyy");
                        }
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

                        //check other branch mapped user list is not null and check which user mapped to current user
                        if (CUM.OtherBranchUserList.Count > 0)
                        {
                            var userLinks = db.crm_mappeduserotherbranch.Where(a => a.UserId == id).ToList();
                            var data = (from sl in CUM.OtherBranchUserList
                                        join ul in userLinks on sl.Id equals ul.MappedUserId into slUl
                                        from d in slUl.DefaultIfEmpty()
                                        select new ManageOrganizationVM()
                                        {
                                            IsChecked = d == null ? false : true,
                                            Id = sl.Id,
                                            UserName = sl.UserName,
                                            EmployeeCode = sl.EmployeeCode,
                                            ProfileName = sl.ProfileName,
                                            EmailID = sl.EmailID,
                                            IsActive = sl.IsActive,
                                            BranchName = sl.BranchName
                                        }).ToList();
                            CUM.OtherBranchUserList = data;
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
                        CUM.OtherDeduction = SalaryDetail.OtherDeduction;
                        CUM.IFSCCode = SalaryDetail.IFCSCode;
                        CUM.LWF = SalaryDetail.LWF;
                        CUM.Security = SalaryDetail.Security;
                        CUM.Advance = SalaryDetail.Advance;
                        CUM.LWP = SalaryDetail.LWP;
                    }

                    //var esclateUser = db.crm_ticketescalationmaster.Where(em => em.UserId == id && em.BranchId == BranchID && em.CompanyId == CompanyID).ToList();
                    //var esclateUser = db.crm_ticketescalationmaster.Where(em => em.UserId == id && em.BranchId == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                    //if(esclateUser!=null)
                    //{
                    //    CUM.EscalateUserId = esclateUser.EscalateUserId;
                    //    CUM.EscalateLevel = esclateUser.EscalateLevel;
                    //    CUM.EscalateTime = esclateUser.EscalateTime;
                    //}
                    string assignQry = @"select Id as uid,CONCAT(Fname ,' ', Lname,' ','(',Email,')') as UserName from crm_usertbl where Id!='" + Convert.ToInt32(Session["UID"]) + "' and Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' order by Fname";
                    CUM.UsersList = db.Database.SqlQuery<Userddl>(assignQry).ToList();


                    //CUM.UsersList = new SelectList(CUM.mapUserList, "UserID", "UserName");
                    //if (esclateUser!=null && esclateUser.Count()>0)
                    //{
                    //    CUM.EscalateUsers = (from usr in esclateUser
                    //                         select new TicketEscalationMasterDTO
                    //                         {
                    //                              Id=usr.Id,
                    //                              UserId=usr.UserId,
                    //                              EscalateUserId= usr.EscalateUserId,
                    //                              EscalateLevel=usr.EscalateLevel,
                    //                              EscalateTime=usr.EscalateTime,
                    //                              //CreatedDate=usr.CreatedDate,
                    //                              //CreatedBy=usr.CreatedBy,
                    //                              //ModifiedBy=usr.ModifiedBy,
                    //                              //ModifiedDate=usr.ModifiedDate,
                    //                              CompanyId=usr.CompanyId,
                    //                              BranchId=usr.BranchId
                    //                         }).ToList();
                    //}
                    //else
                    //{
                    //    CUM.EscalateUsers = new List<TicketEscalationMasterDTO> {
                    //        new TicketEscalationMasterDTO { Id = 0, UserId = 0, EscalateUserId = 0, EscalateLevel = 0, EscalateTime = 0, CompanyId = CompanyID, BranchId = BranchID },
                    //     new TicketEscalationMasterDTO { Id = 0, UserId = 0, EscalateUserId = 0, EscalateLevel = 0, EscalateTime = 0, CompanyId = CompanyID, BranchId = BranchID },
                    //     new TicketEscalationMasterDTO { Id = 0, UserId = 0, EscalateUserId = 0, EscalateLevel = 0, EscalateTime = 0, CompanyId = CompanyID, BranchId = BranchID },
                    //     new TicketEscalationMasterDTO { Id = 0, UserId = 0, EscalateUserId = 0, EscalateLevel = 0, EscalateTime = 0, CompanyId = CompanyID, BranchId = BranchID },
                    //     new TicketEscalationMasterDTO { Id = 0, UserId = 0, EscalateUserId = 0, EscalateLevel = 0, EscalateTime = 0, CompanyId = CompanyID, BranchId = BranchID }};
                    //}
                }
                else
                {
                    Session["ReturnUrl"] = "/home/createusers/" + id;
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


                    int BranchID = Convert.ToInt32(Session["BranchID"]);
                    int CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    var dd = Constant.GetBharatTime();

                    var DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

                    var userList = new List<crm_usertbl>();
                    #region Select-TimeZone
                    var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList().Select(em => new crm_zonetbl
                    {
                        ZoneName = em.ZoneName
                    }).AsQueryable();
                    ViewBag.TimeZoneName = new SelectList(GetZoneName, "ZoneName", "ZoneName");
                    #endregion
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        ViewBag.result = db.crm_roleassigntbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        //userList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        ViewBag.UserList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.ProfileId != null && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    }
                    else
                    {
                        var UID = id;
                        var profiletype = Convert.ToString(Session["UserType"]);
                        //if (profiletype == "Admin" || profiletype == "admin")
                        if (profiletype == "SuperAdmin" || profiletype == "superddmin")
                        {
                            ViewBag.result = db.crm_roleassigntbl.Where(em => em.ProfileName != profiletype && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        }
                        else
                        {
                            ViewBag.result = db.crm_roleassigntbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        }
                        userList = db.crm_usertbl.Where(em => em.Status == true && em.ProfileName.ToLower() != "SuperAdmin" && em.Id != UID && em.ProfileId != null && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        ViewBag.UserList = userList;
                    }

                    #region CompanyType
                    string ptQury = @"select Id as CompanyTypeID, CompanyTypeName from crm_UserCompanytypetbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and Status=1";
                    var getCompanyType = db.Database.SqlQuery<CreateUserModel>(ptQury).OrderBy(a => a.CompanyTypeName).ToList();
                    if (getCompanyType.Count > 0)
                    {
                        CUM.CompanyTypeList = getCompanyType;
                    }
                    #endregion
                    #region Get other branch user list assigned by sa
                    var FromDate = string.Empty;
                    var ToDate = string.Empty;
                    DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + BranchID + "','" + FromDate + "','" + ToDate + "')");
                    var GetOtherBranchUserList = (from dr in GetRecords.AsEnumerable()
                                                  select new ManageOrganizationVM()
                                                  {
                                                      //Id = Convert.ToInt32(dr["Id"]),
                                                      //AssignToCompanyID = Convert.ToInt32(dr["AssignToCompanyID"]),
                                                      Id = Convert.ToInt32(dr["AssignedUserID"]),
                                                      BranchName = Convert.ToString(dr["BranchName"]),
                                                      //AssignedBranchName = Convert.ToString(dr["AssignedBranchName"]),                                              
                                                      //AssignedDate = Convert.ToString(dr["AssignedDate"]),
                                                      UserName = Convert.ToString(dr["UserName"]),
                                                      EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                                      ProfileName = Convert.ToString(dr["ProfileName"]),
                                                      EmailID = Convert.ToString(dr["EmailID"]),
                                                      IsActive = Convert.ToBoolean(dr["IsActive"])
                                                  }).Where(a => a.IsActive == true).ToList();
                    if (GetOtherBranchUserList.Count > 0)
                    {
                        CUM.OtherBranchUserList = GetOtherBranchUserList;
                    }
                    #endregion
                    #region AssignItem

                    //var GetItemlist = db.crm_itemtypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    var data1 = @"select ITM.ItemTypeName , IAM.ItemTypeId,IAM.Quantity,IAM.Estimated_Cost,IAM.Expirydate,IAM.CheckStatus,IAM.SerialNo,IAM.UserId,IAM.Assigndate from 
                                crm_itemtypetbl ITM join crm_assignitemtypeuser IAM
                                on ITM.Id = IAM.ItemTypeId where ITM.BranchID = '" + BranchID + "' and ITM.CompanyID = '" + CompanyID + "'";

                    var itemdata = db.Database.SqlQuery<ItemAssignlist>(data1).ToList();
                    if (itemdata != null && itemdata.Count > 0)
                    {
                        CUM.ItemTypeList = (from item in itemdata
                                            select new ItemAssignlist
                                            {
                                                ItemTypeId = item.ItemTypeId,
                                                UserId = item.UserId,
                                                ItemTypeName = item.ItemTypeName,
                                                Quantity = item.Quantity,
                                                Estimated_Cost = item.Estimated_Cost,
                                                Expirydate = item.Expirydate,
                                                CheckStatus = item.CheckStatus,
                                                SerialNo = item.SerialNo,
                                                Assigndate = item.Assigndate
                                            }
                                         ).ToList();
                    }
                    //if (GetItemlist.Count > 0)
                    //{
                    //    List<CreateUserModel> ItemList = new List<CreateUserModel>();
                    //    foreach (var item in GetItemlist)
                    //    {
                    //        CreateUserModel Icm = new CreateUserModel();
                    //        Icm.itemuid = item.UserId;
                    //        Icm.itemSerialno = item.Serialno;
                    //        Icm.ItemId = item.Id;
                    //        Icm.ItemName = item.ItemTypeName;
                    //        Icm.Quantity = item.Quantity;
                    //        Icm.Estimated_Cost = item.Estimated_Cost;
                    //        Icm.Expiry_Date = Convert.ToDateTime(item.Expirydate);
                    //        ItemList.Add(Icm);
                    //    }
                    //    CUM.ItemTypeList = ItemList;

                    //}


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
                    if (CompanyID == 1153 || CompanyID == 2682)
                    {
                        var Customegroupdataall = @"select ID,CustGroupName from  customersgroup where BranchCode = '" + BranchID + "' and CompanyId = '" + CompanyID + "'";

                        var allcustomgrpdata = db.Database.SqlQuery<Customergrouplist>(Customegroupdataall).ToList();
                        if (allcustomgrpdata != null && allcustomgrpdata.Count > 0)
                        {
                            CUM.Allcustgrouplist = (from item in allcustomgrpdata
                                                    select new Customergrouplist
                                                    {
                                                        ID = item.ID,
                                                        CustGroupName = item.CustGroupName,

                                                    }
                                             ).ToList();
                        }
                    }
                    string assignQry = @"select Id as uid,CONCAT(Fname ,' ', Lname,' ','(',Email,')') as UserName from crm_usertbl where Id!='" + Convert.ToInt32(Session["UID"]) + "' and Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "' order by Fname";
                    CUM.UsersList = db.Database.SqlQuery<Userddl>(assignQry).ToList();
                    #region No. of User assign as per company Profile
                    ///Total no. of user in company_profile table
                    ///after that you can create the user in crm against a company
                    int? CountNoOfUsers = 0;
                    int? CurentCountNoOfUsers = 0;
                    var countUsers = db.company_profile.Where(em => em.ID == CompanyID).FirstOrDefault();
                    if (countUsers != null)
                    {
                        CountNoOfUsers = /*countUsers.No_of_users == 0 ? 3 :*/ countUsers.No_of_users;
                    }
                    if (CompanyID == 2644)
                    {
                        var countcurrentUsers = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileName != "SuperAdmin" && em.ProfileId != null && em.Status == true).ToList();
                        if (countcurrentUsers.Count > 0)
                        {
                            CurentCountNoOfUsers = countcurrentUsers.Count();
                        }
                    }
                    if (CompanyID != 296 && CompanyID != 2644)
                    {
                        var countcurrentUsers = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileName != "SuperAdmin" && em.ProfileId != null).ToList();
                        if (countcurrentUsers.Count > 0)
                        {
                            CurentCountNoOfUsers = countcurrentUsers.Count();
                        }
                    }

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
                        if (!string.IsNullOrEmpty(CUM.UserProfile) || !string.IsNullOrEmpty(CUM.AddProfileName))
                        {

                            if (id != null)
                            {
                                var GetUpdateRecords = db.crm_usertbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                if (GetUpdateRecords != null)
                                {
                                    #region Password encryption
                                    string VersionKey = "";
                                    byte[] iv1;

                                    VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version

                                    byte[] key = EncriptAES.getdcriptkey(out iv1);
                                    string ecncryptPwd = EncriptAES.EncryptString(CUM.UserPassword, key, iv1);
                                    #endregion
                                    GetUpdateRecords.Fname = CUM.FirstName;
                                    GetUpdateRecords.Lname = CUM.LastName;
                                    GetUpdateRecords.FatherName = CUM.FatherName;
                                    if (!string.IsNullOrWhiteSpace(CUM.DateofBirth))
                                    {
                                        GetUpdateRecords.DateofBirth = DateTime.ParseExact(CUM.DateofBirth, DateFormat, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        GetUpdateRecords.DateofBirth = null;
                                    }
                                    GetUpdateRecords.Aadhar_No = CUM.AadharNumber;
                                    if (!string.IsNullOrWhiteSpace(CUM.DateofJoining))
                                    {
                                        GetUpdateRecords.DOJ = DateTime.ParseExact(CUM.DateofJoining, DateFormat, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        GetUpdateRecords.DOJ = null;
                                    }
                                    GetUpdateRecords.ContactNumber = CUM.ContactNumber;
                                    GetUpdateRecords.ContactNumber = CUM.ContactNumber;
                                    GetUpdateRecords.AlternateNumber = CUM.AlternateNumber;
                                    GetUpdateRecords.Email = CUM.Email;
                                    GetUpdateRecords.Gender = CUM.Gender;
                                    GetUpdateRecords.UserName = CUM.UserName;
                                    GetUpdateRecords.EmployeeCode = CUM.EmployeeCode;
                                    GetUpdateRecords.Designation = CUM.Designation;
                                    GetUpdateRecords.Password = ecncryptPwd;//save ecncrypted password 
                                    GetUpdateRecords.KeyVersion = VersionKey;//save latest key version                               
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
                                    if (CompanyID == 1153 || CompanyID == 2682)
                                    {
                                        GetUpdateRecords.Customergroupid = CUM.Customergroupid.ToString();

                                        customersgroup pdt = new customersgroup();
                                        var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
                                        for (int i = 0; i < comptype.Count; i++)
                                        {
                                            int multicompany = Convert.ToInt32(comptype[i].ID);
                                            string returnmultigrpid = "";
                                            string csutomergropname = "";
                                            var multibranch = db.com_branches.Where(em => em.OrganizationID == multicompany).FirstOrDefault();
                                            {
                                                if (multibranch != null)
                                                {
                                                    int multib = Convert.ToInt32(multibranch.OrgBranchCode);
                                                    var checkExist = db.customersgroups.Where(em => em.ID == CUM.Customergroupid && em.BranchCode == multib && em.CompanyId == multicompany).FirstOrDefault();
                                                    if (checkExist == null)
                                                    {
                                                        var checkExist1 = db.customersgroups.Where(em => em.BranchCode == BranchID && em.CompanyId == CompanyID && em.ID == CUM.Customergroupid).FirstOrDefault();
                                                        if (checkExist1 != null)
                                                        {
                                                            csutomergropname = checkExist1.CustGroupName.ToString();
                                                        }
                                                        var samegropname = db.customersgroups.Where(em => em.CustGroupName == csutomergropname && em.BranchCode == multib && em.CompanyId == multicompany).FirstOrDefault();
                                                        if (samegropname == null)
                                                        {
                                                            pdt.CustGroupName = csutomergropname;
                                                            pdt.BranchCode = multib;
                                                            pdt.CompanyId = multicompany;
                                                            pdt.CreatedBy = Convert.ToString(Session["UserName"]);
                                                            pdt.CreatedDate = Constant.GetBharatTime();
                                                            pdt.ModifiedBy = Convert.ToString(Session["UserName"]);
                                                            pdt.ModifiedDate = Constant.GetBharatTime();
                                                            string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM customersgroup";
                                                            int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                                            string synId = "O" + (syncid + 1);
                                                            pdt.SyncID = synId;
                                                            pdt.Flag = "N";
                                                            db.customersgroups.Add(pdt);
                                                            db.SaveChanges();
                                                            returnmultigrpid = pdt.ID.ToString();
                                                        }
                                                        else
                                                        {
                                                            returnmultigrpid = samegropname.ID.ToString();
                                                        }

                                                        //return Json(expando, JsonRequestBehavior.AllowGet);

                                                    }
                                                    else
                                                    {
                                                        var checkExist1 = db.customersgroups.Where(em => em.BranchCode == BranchID && em.CompanyId == CompanyID && em.ID == CUM.Customergroupid).FirstOrDefault();
                                                        if (checkExist1 != null)
                                                        {
                                                            returnmultigrpid = checkExist1.ID.ToString();
                                                        }
                                                    }

                                                    var t_data = db.crm_usertbl.Where(a => a.Id == id).FirstOrDefault();
                                                    if (t_data.t_LoginId == null)//check if t_login id is null then save it to table
                                                    {
                                                        var dt = Constant.GetBharatTime();
                                                        string uniqUserId = t_data.UserName + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                                                        string multibranch123 = Convert.ToString(multib);
                                                        var updatet_login = db.t_login.Where(a => a.branch == multibranch123 && a.company_id == multicompany && a.user_id.Substring(0, 5) == uniqUserId.Substring(0, 5)).FirstOrDefault();

                                                        if (updatet_login != null)
                                                        {
                                                            updatet_login.customer_groups = returnmultigrpid;
                                                            db.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            #region Password encryption
                                                            VersionKey = "";

                                                            VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                                                                                                                                                       //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                                                            byte[] iv2;
                                                            byte[] key2 = EncriptAES.getdcriptkey(out iv2);
                                                            string ecncryptPwd1 = EncriptAES.EncryptString(uniqUserId, key2, iv1);
                                                            #endregion

                                                            #region get syncid from t_login
                                                            string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                                                            int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                                            string synId = "O" + (syncid + 1);
                                                            #endregion
                                                            //var tl = new t_login
                                                            //{
                                                            t_login tl = new t_login();
                                                            tl.user_id = uniqUserId;
                                                            tl.user_password = ecncryptPwd1;//save encrypted password
                                                            tl.role_id = 7;
                                                            tl.role_name = "Admin";
                                                            tl.UserRole = 7;
                                                            tl.Emp_code = "CRM";
                                                            tl.branch = multib.ToString();
                                                            tl.company_id = multicompany;
                                                            tl.Subscription_StartDate = t_data.StartDate;
                                                            tl.Subscription_EndDate = t_data.EndDate;
                                                            tl.PaymentStatus = "no";
                                                            tl.CreatedDate = dt;
                                                            tl.IsActive = "yes";
                                                            tl.flag = "N";
                                                            tl.SyncID = synId;
                                                            tl.mastercompany = CompanyID;
                                                            //tl.user_type = "crm_user";
                                                            tl.customer_id = "";
                                                            tl.permissions = "";
                                                            tl.KeyVersion = VersionKey;//save latest key version
                                                            tl.customer_groups = returnmultigrpid;                           //};
                                                            db.t_login.Add(tl);
                                                            db.SaveChanges();
                                                        }
                                                    }

                                                }
                                            }

                                        }
                                        string returnsingleid = "";
                                        var mancompany = db.customersgroups.Where(em => em.ID == CUM.Customergroupid && em.BranchCode == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                                        if (mancompany != null)
                                        {
                                            //pdt.CustGroupName = mancompany.CustGroupName;
                                            //pdt.BranchCode = BranchID;
                                            //pdt.CompanyId = CompanyID;
                                            //pdt.CreatedBy = Convert.ToString(Session["UserName"]);
                                            //pdt.CreatedDate = Constant.GetBharatTime();
                                            //pdt.ModifiedBy = Convert.ToString(Session["UserName"]);
                                            //pdt.ModifiedDate = Constant.GetBharatTime();
                                            //string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM customersgroup";
                                            //int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                            //string synId = "O" + (syncid + 1);
                                            //pdt.SyncID = synId;
                                            //pdt.Flag = "N";
                                            //db.customersgroups.Add(pdt);
                                            //db.SaveChanges();
                                            returnsingleid = mancompany.ID.ToString();

                                            //return Json(expando, JsonRequestBehavior.AllowGet);

                                        }
                                        var data = db.crm_usertbl.Where(a => a.Id == id && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        var dt1 = Constant.GetBharatTime();
                                        string uniqUserId1 = data.UserName + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                                        string BranchID1 = Convert.ToString(BranchID);
                                        var updatet_login1 = db.t_login.Where(a => a.branch == BranchID1 && a.company_id == CompanyID && a.user_id.Substring(0, 5) == uniqUserId1.Substring(0, 5)).FirstOrDefault();

                                        if (updatet_login1 != null)
                                        {
                                            updatet_login1.customer_groups = returnsingleid;
                                            db.SaveChanges();
                                        }

                                        else
                                        {

                                            #region Password encryption
                                            string VersionKey2 = "";

                                            VersionKey2 = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                                                                                                                                        //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                                            byte[] iv2;
                                            byte[] key2 = EncriptAES.getdcriptkey(out iv2);
                                            string ecncryptPwd1 = EncriptAES.EncryptString(uniqUserId1, key2, iv2);
                                            #endregion

                                            #region get syncid from t_login
                                            string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                                            int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                            string synId = "O" + (syncid + 1);
                                            #endregion
                                            //var tl = new t_login
                                            //{
                                            t_login tl = new t_login();
                                            tl.user_id = uniqUserId1;
                                            tl.user_password = ecncryptPwd1;//save encrypted password
                                            tl.role_id = 7;
                                            tl.role_name = "Admin";
                                            tl.UserRole = 7;
                                            tl.Emp_code = "CRM";
                                            tl.branch = BranchID.ToString();
                                            tl.company_id = CompanyID;
                                            tl.Subscription_StartDate = data.StartDate;
                                            tl.Subscription_EndDate = data.EndDate;
                                            tl.PaymentStatus = "no";
                                            tl.CreatedDate = dt1;
                                            tl.IsActive = "yes";
                                            tl.flag = "N";
                                            tl.SyncID = synId;
                                            tl.mastercompany = CompanyID;
                                            //tl.user_type = "crm_user";
                                            tl.customer_id = "";
                                            tl.permissions = "";
                                            tl.KeyVersion = VersionKey;//save latest key version
                                            tl.customer_groups = returnsingleid;                     //};
                                            db.t_login.Add(tl);
                                            db.SaveChanges();

                                        }

                                    }
                                    if (CUM.CompanyTypeID != null)
                                    {
                                        GetUpdateRecords.CompanyTypeId = CUM.CompanyTypeID;
                                    }
                                    if (!string.IsNullOrEmpty(CUM.AddProfileName))
                                    {
                                        var role = new crm_roleassigntbl
                                        {
                                            created_at = Constant.GetBharatTime(),
                                            ProfileName = CUM.AddProfileName,
                                            BranchID = BranchID,
                                            CompanyID = CompanyID,
                                            Status = true
                                        };
                                        db.crm_roleassigntbl.Add(role);
                                        db.SaveChanges();

                                        GetUpdateRecords.ProfileId = Convert.ToString(role.Id);
                                        GetUpdateRecords.ProfileName = role.ProfileName;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(CUM.UserProfile))
                                        {
                                            var getProfille = CUM.UserProfile.Split(',');
                                            GetUpdateRecords.ProfileId = getProfille[0];
                                            GetUpdateRecords.ProfileName = getProfille[1];
                                        }

                                    }
                                    GetUpdateRecords.MappedUsers = Mappeduserlist;
                                    GetUpdateRecords.BranchID = CUM.BranchID;
                                    GetUpdateRecords.CompanyID = CompanyID;
                                    GetUpdateRecords.Modifiedby = Convert.ToInt32(Session["UID"]);
                                    GetUpdateRecords.ModifiedOn = Constant.GetBharatTime();
                                    GetUpdateRecords.EmployeType = CUM.Employetype;
                                    #region ticket escalation user entry

                                    if (CUM.TempEscalateUserId != null && CUM.TempReportingManagerId != null && CUM.TempEscalateUserId != CUM.EscalateUserId && CUM.ReportingManagerId != null && CUM.TempReportingManagerId != CUM.ReportingManagerId)
                                    {
                                        var previousMgrId = db.crm_usertbl.Where(a => a.Id == CUM.TempEscalateUserId && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        if (previousMgrId != null)
                                        {
                                            previousMgrId.EscalateUserId = null;
                                            previousMgrId.EscalateTime = 0;
                                            db.SaveChanges();
                                        }
                                        var newRepMgr = db.crm_usertbl.Where(a => a.Id == CUM.EscalateUserId && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        if (newRepMgr != null)
                                        {
                                            newRepMgr.EscalateUserId = CUM.ReportingManagerId;
                                            newRepMgr.EscalateTime = CUM.EscalateTime;
                                            db.SaveChanges();
                                        }
                                    }
                                    else if (CUM.TempEscalateUserId != null && CUM.TempReportingManagerId != null && CUM.TempEscalateUserId == CUM.EscalateUserId && CUM.ReportingManagerId != null && CUM.TempReportingManagerId != CUM.ReportingManagerId)
                                    {
                                        //var previousMgrId = db.crm_usertbl.Where(a => a.Id == CUM.EscalateUserId && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        //if (previousMgrId != null)
                                        //{
                                        //    previousMgrId.EscalateUserId = null;
                                        //    previousMgrId.EscalateTime = 0;
                                        //    db.SaveChanges();
                                        //}
                                        var newRepMgr = db.crm_usertbl.Where(a => a.Id == CUM.EscalateUserId && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        if (newRepMgr != null)
                                        {
                                            newRepMgr.EscalateUserId = CUM.ReportingManagerId;
                                            newRepMgr.EscalateTime = CUM.EscalateTime;
                                            db.SaveChanges();
                                        }
                                    }
                                    else if (CUM.TempEscalateUserId == null && CUM.TempReportingManagerId == null && CUM.EscalateUserId != null && CUM.ReportingManagerId != null && CUM.EscalateTime > 0)
                                    {
                                        GetUpdateRecords.EscalateUserId = CUM.EscalateUserId;
                                        GetUpdateRecords.EscalateTime = CUM.EscalateTime;
                                        var reportingMgrId = db.crm_usertbl.Where(a => a.Id == CUM.EscalateUserId && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        if (reportingMgrId != null)
                                        {
                                            reportingMgrId.EscalateUserId = CUM.ReportingManagerId;
                                            reportingMgrId.EscalateTime = CUM.EscalateTime;
                                            db.SaveChanges();
                                        }
                                    }
                                    else if (CUM.TempEscalateUserId == null && CUM.TempReportingManagerId == null && CUM.EscalateUserId != null && CUM.ReportingManagerId == null && CUM.EscalateTime > 0)
                                    {
                                        GetUpdateRecords.EscalateUserId = CUM.EscalateUserId;
                                        GetUpdateRecords.EscalateTime = CUM.EscalateTime;
                                    }

                                    #endregion
                                    db.SaveChanges();

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
                                        db.SaveChanges();
                                    }
                                    #region Upload user Document
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
                                                    var FileName = CUM.UserName + "_" + CUM.FirstName + "" + CUM.LastName + "-" + postedFile.FileName + "-" + dd.ToString("ddMMyyyyhhmmss") + "";
                                                    var FileFullName = FileName + extension;
                                                    var filePath = Server.MapPath("~/EmployeeDocuments/" + FileName + extension);
                                                    postedFile.SaveAs(filePath);

                                                    var docs = new crm_userdocuments
                                                    {
                                                        UserId = id,
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
                                    #endregion
                                    #region user profile document
                                    if (CUM.Userprofilepic != null && CUM.Userprofilepic.Count() > 0)
                                    {
                                        foreach (var file in CUM.Userprofilepic)
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
                                                    var FileName = postedFile.FileName;
                                                    var FileFullName = FileName;
                                                    var filePath = Server.MapPath("/EmployeeDocuments/" + FileName);
                                                    postedFile.SaveAs(filePath);

                                                    var docs = new crm_userprofile
                                                    {
                                                        //UserId = id,
                                                        ProfilePath = FileFullName,
                                                        CompanyId = CompanyID,
                                                        BranchId = BranchID,
                                                        CreatedBy = Convert.ToInt32(Session["UID"]),
                                                        CreatedDate = dd
                                                    };
                                                    db.crm_userprofile.Add(docs);
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    //var esclateUser = db.crm_ticketescalationmaster.Where(em => em.UserId == id && em.BranchId == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                                    //if (esclateUser != null)
                                    //{
                                    //    esclateUser.EscalateUserId = CUM.EscalateUserId;
                                    //    esclateUser.EscalateLevel = CUM.EscalateLevel;
                                    //    esclateUser.EscalateTime = CUM.EscalateTime;
                                    //    esclateUser.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    //    esclateUser.ModifiedDate = dd;
                                    //}

                                    //delete exixting map other branch user for updated new one
                                    db.Database.ExecuteSqlCommand(@"SET SQL_SAFE_UPDATES = 0;delete from CRM_MappedUserOtherBranch where UserId=" + id);


                                    //////// map other branch user updated ////////////
                                    if (CUM.IsChecked != null && CUM.IsChecked.Count() > 0)
                                    {
                                        foreach (var item in CUM.IsChecked)
                                        {
                                            var usl = new crm_mappeduserotherbranch
                                            {
                                                UserId = id ?? 0,
                                                MappedUserId = item,
                                                CreateDate = dd,
                                                IsActive = true
                                            };
                                            db.crm_mappeduserotherbranch.Add(usl);
                                        }
                                        db.SaveChanges();
                                    }
                                    trans.Commit();

                                    TempData["success"] = "User updated successfully";
                                    return Redirect("/home/createusers/" + id);
                                }
                                else
                                {
                                    trans.Rollback();
                                    TempData["alert"] = "There is some problem";
                                    return Redirect("/home/createusers/" + id);
                                }
                            }
                            else
                            {
                                if (CurentCountNoOfUsers <= CountNoOfUsers)
                                {
                                    #region Set Expire Date Default SuperAdmin
                                    var PresentDate = DateTime.Today.ToString("yyyy-MM-dd");
                                    var PresentDatePlusOneYear = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");//PresentDate.AddYears(1).ToString("yyyy-MM-dd");
                                    string Adminbase64StartDate = EncodeDecodeForBase64.EncodeBase64(PresentDate);
                                    string Adminbase64EndDate = EncodeDecodeForBase64.EncodeBase64(PresentDatePlusOneYear);
                                    #endregion

                                    #region Password encryption
                                    string VersionKey = "";

                                    VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version

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
                                        utbl.DateofBirth = DateTime.ParseExact(CUM.DateofBirth, DateFormat, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        utbl.DateofBirth = null;
                                    }
                                    utbl.Aadhar_No = CUM.AadharNumber;
                                    if (!string.IsNullOrWhiteSpace(CUM.DateofJoining))
                                    {
                                        utbl.DOJ = DateTime.ParseExact(CUM.DateofJoining, DateFormat, CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        utbl.DOJ = null;
                                    }
                                    utbl.ContactNumber = CUM.ContactNumber;
                                    utbl.AlternateNumber = CUM.AlternateNumber;
                                    utbl.Email = CUM.Email;
                                    utbl.Gender = CUM.Gender;
                                    utbl.EmployeeCode = CUM.EmployeeCode;
                                    utbl.UserName = CUM.UserName;
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
                                    utbl.EmployeType = CUM.Employetype;
                                    //if (CompanyID == 1153)
                                    //{
                                    //    utbl.Customergroupid = CUM.Customergroupid;

                                    //    customersgroup pdt = new customersgroup();
                                    //    var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
                                    //    for (int i = 0; i < comptype.Count; i++)
                                    //    {
                                    //        int multicompany = Convert.ToInt32(comptype[i].ID);
                                    //        var multibranch = db.com_branches.Where(em => em.OrganizationID == multicompany).FirstOrDefault();
                                    //        {
                                    //            if (multibranch != null)
                                    //            {
                                    //                int multib = Convert.ToInt32(multibranch.OrgBranchCode);
                                    //                var checkExist = db.customersgroups.Where(em => em.CustGroupName == utbl.Customergroupid && em.BranchCode == multib && em.CompanyId == multicompany).FirstOrDefault();
                                    //                if (checkExist == null)
                                    //                {
                                    //                    pdt.CustGroupName = utbl.Customergroupid;
                                    //                    pdt.BranchCode = multib;
                                    //                    pdt.CompanyId = multicompany;

                                    //                    db.customersgroups.Add(pdt);
                                    //                    db.SaveChanges();


                                    //                    //return Json(expando, JsonRequestBehavior.AllowGet);

                                    //                }
                                    //                else
                                    //                {

                                    //                    //expando.Msg = "Cus type already exist";
                                    //                    //return Json(expando, JsonRequestBehavior.AllowGet);
                                    //                }
                                    //            }
                                    //        }

                                    //    }
                                    //    var mancompany = db.customersgroups.Where(em => em.CustGroupName == utbl.Customergroupid && em.BranchCode == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                                    //    if (mancompany == null)
                                    //    {
                                    //        pdt.CustGroupName = utbl.Customergroupid;
                                    //        pdt.BranchCode = BranchID;
                                    //        pdt.CompanyId = CompanyID;

                                    //        db.customersgroups.Add(pdt);
                                    //        db.SaveChanges();


                                    //        //return Json(expando, JsonRequestBehavior.AllowGet);

                                    //    }
                                    //}
                                    if (CUM.CompanyTypeID != null)
                                    {
                                        utbl.CompanyTypeId = Convert.ToInt32(CUM.CompanyTypeID);
                                    }
                                    if (!string.IsNullOrEmpty(CUM.AddProfileName))
                                    {
                                        var role = new crm_roleassigntbl
                                        {
                                            created_at = Constant.GetBharatTime(),
                                            ProfileName = CUM.AddProfileName,
                                            BranchID = BranchID,
                                            CompanyID = CompanyID,
                                            Status = true
                                        };
                                        db.crm_roleassigntbl.Add(role);
                                        db.SaveChanges();

                                        utbl.ProfileId = Convert.ToString(role.Id);
                                        utbl.ProfileName = role.ProfileName;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(CUM.UserProfile))
                                        {
                                            var getProfille = CUM.UserProfile.Split(',');
                                            utbl.ProfileId = getProfille[0];
                                            utbl.ProfileName = getProfille[1];
                                        }
                                    }


                                    #region ticket escalation user entry
                                    if (CUM.EscalateUserId != null && CUM.ReportingManagerId != null && CUM.EscalateTime > 0)
                                    {
                                        utbl.EscalateUserId = CUM.EscalateUserId;
                                        utbl.EscalateTime = CUM.EscalateTime;

                                        var reportingMgrId = db.crm_usertbl.Where(a => a.Id == CUM.EscalateUserId && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        if (reportingMgrId != null)
                                        {
                                            reportingMgrId.EscalateUserId = CUM.ReportingManagerId;
                                            reportingMgrId.EscalateTime = CUM.EscalateTime;
                                            //db.SaveChanges();
                                        }
                                    }
                                    else if (CUM.EscalateUserId != null && CUM.ReportingManagerId == null && CUM.EscalateTime > 0)
                                    {
                                        utbl.EscalateUserId = CUM.EscalateUserId;
                                        utbl.EscalateTime = CUM.EscalateTime;
                                    }
                                    #endregion


                                    db.crm_usertbl.Add(utbl);
                                    db.SaveChanges();
                                    int returnValue = utbl.Id;
                                    Session["UserID"] = returnValue;
                                    if (CompanyID == 1153 || CompanyID == 2682)
                                    {
                                        utbl.Customergroupid = CUM.Customergroupid.ToString();

                                        customersgroup pdt = new customersgroup();
                                        var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
                                        for (int i = 0; i < comptype.Count; i++)
                                        {
                                            int multicompany = Convert.ToInt32(comptype[i].ID);
                                            string returnmultigrpid = "";
                                            string csutomergropname = "";
                                            var multibranch = db.com_branches.Where(em => em.OrganizationID == multicompany).FirstOrDefault();
                                            {
                                                if (multibranch != null)
                                                {
                                                    int multib = Convert.ToInt32(multibranch.OrgBranchCode);
                                                    var checkExist = db.customersgroups.Where(em => em.ID == CUM.Customergroupid && em.BranchCode == multib && em.CompanyId == multicompany).FirstOrDefault();
                                                    if (checkExist == null)
                                                    {
                                                        var checkExist1 = db.customersgroups.Where(em => em.BranchCode == BranchID && em.CompanyId == CompanyID && em.ID == CUM.Customergroupid).FirstOrDefault();
                                                        if (checkExist1 != null)
                                                        {
                                                            csutomergropname = checkExist1.CustGroupName.ToString();
                                                        }
                                                        var samegropname = db.customersgroups.Where(em => em.CustGroupName == csutomergropname && em.BranchCode == multib && em.CompanyId == multicompany).FirstOrDefault();
                                                        if (samegropname == null)
                                                        {
                                                            pdt.CustGroupName = csutomergropname;
                                                            pdt.BranchCode = multib;
                                                            pdt.CompanyId = multicompany;
                                                            pdt.CreatedBy = Convert.ToString(Session["UserName"]);
                                                            pdt.CreatedDate = Constant.GetBharatTime();
                                                            pdt.ModifiedBy = Convert.ToString(Session["UserName"]);
                                                            pdt.ModifiedDate = Constant.GetBharatTime();
                                                            string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM customersgroup";
                                                            int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                                            string synId = "O" + (syncid + 1);
                                                            pdt.SyncID = synId;
                                                            pdt.Flag = "N";
                                                            db.customersgroups.Add(pdt);
                                                            db.SaveChanges();
                                                            returnmultigrpid = pdt.ID.ToString();
                                                        }
                                                        else
                                                        {
                                                            returnmultigrpid = samegropname.ID.ToString();
                                                        }

                                                        //return Json(expando, JsonRequestBehavior.AllowGet);

                                                    }
                                                    else
                                                    {
                                                        var checkExist1 = db.customersgroups.Where(em => em.BranchCode == BranchID && em.CompanyId == CompanyID && em.ID == CUM.Customergroupid).FirstOrDefault();
                                                        if (checkExist1 != null)
                                                        {
                                                            returnmultigrpid = checkExist1.ID.ToString();
                                                        }
                                                    }

                                                    var t_data = db.crm_usertbl.Where(a => a.Id == returnValue).FirstOrDefault();
                                                    if (t_data.t_LoginId == null)//check if t_login id is null then save it to table
                                                    {
                                                        var dt = Constant.GetBharatTime();
                                                        string uniqUserId = t_data.UserName + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                                                        #region Password encryption
                                                        VersionKey = "";

                                                        VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                                                                                                                                                   //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                                                        byte[] iv2;
                                                        byte[] key2 = EncriptAES.getdcriptkey(out iv2);
                                                        string ecncryptPwd1 = EncriptAES.EncryptString(uniqUserId, key2, iv1);
                                                        #endregion

                                                        #region get syncid from t_login
                                                        string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                                                        int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                                        string synId = "O" + (syncid + 1);
                                                        #endregion
                                                        //var tl = new t_login
                                                        //{
                                                        t_login tl = new t_login();
                                                        tl.user_id = uniqUserId;
                                                        tl.user_password = ecncryptPwd1;//save encrypted password
                                                        tl.role_id = 7;
                                                        tl.role_name = "Admin";
                                                        tl.UserRole = 7;
                                                        tl.Emp_code = "CRM";
                                                        tl.branch = multib.ToString();
                                                        tl.company_id = multicompany;
                                                        tl.Subscription_StartDate = t_data.StartDate;
                                                        tl.Subscription_EndDate = t_data.EndDate;
                                                        tl.PaymentStatus = "no";
                                                        tl.CreatedDate = dt;
                                                        tl.IsActive = "yes";
                                                        tl.flag = "N";
                                                        tl.SyncID = synId;
                                                        tl.mastercompany = CompanyID;
                                                        //tl.user_type = "crm_user";
                                                        tl.customer_id = "";
                                                        tl.permissions = "";
                                                        tl.KeyVersion = VersionKey;//save latest key version
                                                        tl.customer_groups = returnmultigrpid;                           //};
                                                        db.t_login.Add(tl);
                                                        db.SaveChanges();
                                                    }

                                                }
                                            }

                                        }
                                        string returnsingleid = "";
                                        var mancompany = db.customersgroups.Where(em => em.ID == CUM.Customergroupid && em.BranchCode == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                                        if (mancompany != null)
                                        {
                                            //pdt.CustGroupName = mancompany.CustGroupName;
                                            //pdt.BranchCode = BranchID;
                                            //pdt.CompanyId = CompanyID;
                                            //pdt.CreatedBy = Convert.ToString(Session["UserName"]);
                                            //pdt.CreatedDate = Constant.GetBharatTime();
                                            //pdt.ModifiedBy = Convert.ToString(Session["UserName"]);
                                            //pdt.ModifiedDate = Constant.GetBharatTime();
                                            //string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM customersgroup";
                                            //int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                            //string synId = "O" + (syncid + 1);
                                            //pdt.SyncID = synId;
                                            //pdt.Flag = "N";
                                            //db.customersgroups.Add(pdt);
                                            //db.SaveChanges();
                                            returnsingleid = mancompany.ID.ToString();

                                            //return Json(expando, JsonRequestBehavior.AllowGet);

                                        }
                                        var data = db.crm_usertbl.Where(a => a.Id == returnValue && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                                        if (data.t_LoginId == null)//check if t_login id is null then save it to table
                                        {
                                            var dt = Constant.GetBharatTime();
                                            string uniqUserId = data.UserName + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                                            #region Password encryption
                                            string VersionKey2 = "";

                                            VersionKey2 = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                                                                                                                                        //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                                            byte[] iv2;
                                            byte[] key2 = EncriptAES.getdcriptkey(out iv2);
                                            string ecncryptPwd1 = EncriptAES.EncryptString(uniqUserId, key2, iv2);
                                            #endregion

                                            #region get syncid from t_login
                                            string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                                            int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                            string synId = "O" + (syncid + 1);
                                            #endregion
                                            //var tl = new t_login
                                            //{
                                            t_login tl = new t_login();
                                            tl.user_id = uniqUserId;
                                            tl.user_password = ecncryptPwd1;//save encrypted password
                                            tl.role_id = 7;
                                            tl.role_name = "Admin";
                                            tl.UserRole = 7;
                                            tl.Emp_code = "CRM";
                                            tl.branch = BranchID.ToString();
                                            tl.company_id = CompanyID;
                                            tl.Subscription_StartDate = data.StartDate;
                                            tl.Subscription_EndDate = data.EndDate;
                                            tl.PaymentStatus = "no";
                                            tl.CreatedDate = dt;
                                            tl.IsActive = "yes";
                                            tl.flag = "N";
                                            tl.SyncID = synId;
                                            tl.mastercompany = CompanyID;
                                            //tl.user_type = "crm_user";
                                            tl.customer_id = "";
                                            tl.permissions = "";
                                            tl.KeyVersion = VersionKey;//save latest key version
                                            tl.customer_groups = returnsingleid;                     //};
                                            db.t_login.Add(tl);
                                            db.SaveChanges();
                                        }




                                    }
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
                                                        var FileName = CUM.UserName + "_" + CUM.FirstName + "" + CUM.LastName + "-" + postedFile.FileName + "-" + dd.ToString("ddMMyyyyhhmmss") + "";
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

                                        //delete exixting software link for new link add
                                        //db.Database.ExecuteSqlCommand(@"delete from CRM_MappedUserOtherBranch where UserId=" + id);

                                        //////// add assign rights to user ////////////
                                        if (CUM.IsChecked != null && CUM.IsChecked.Count() > 0)
                                        {
                                            foreach (var item in CUM.IsChecked)
                                            {
                                                var usl = new crm_mappeduserotherbranch
                                                {
                                                    UserId = id ?? 0,
                                                    MappedUserId = item,
                                                    CreateDate = Constant.GetBharatTime(),
                                                    IsActive = true
                                                };
                                                db.crm_mappeduserotherbranch.Add(usl);
                                            }
                                            db.SaveChanges();
                                        }

                                        //if (CUM.EscalateUserId > 0 && CUM.EscalateTime > 0 && CUM.EscalateLevel > 0)
                                        //{
                                        //    var esc = new crm_ticketescalationmaster
                                        //    {
                                        //        UserId = returnValue,
                                        //        EscalateTime = CUM.EscalateTime,
                                        //        EscalateLevel = CUM.EscalateLevel,
                                        //        CompanyId = CompanyID,
                                        //        BranchId = BranchID,
                                        //        CreatedDate = dd,
                                        //        CreatedBy = Convert.ToInt32(Session["UID"])
                                        //    };
                                        //    db.crm_ticketescalationmaster.Add(esc);
                                        //    db.SaveChanges();
                                        //}
                                        trans.Commit();
                                        TempData["success"] = "User added successfully";
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        TempData["alert"] = "There is some problem";
                                        return View(CUM);
                                    }
                                }
                                else
                                {
                                    trans.Rollback();
                                    TempData["alert"] = "** You can create only " + CountNoOfUsers + " users, For more users contact to administrator.";
                                    return View(CUM);
                                }
                            }
                        }
                        else
                        {
                            trans.Rollback();
                            TempData["alert"] = "Please select atleast one User Role";
                            return View(CUM);
                        }
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

            return Redirect("/home/createusers");
        }


        public ActionResult ViewAssignedItem(CreateUserModel CUM, String SearchUserID, int? userid, int? itemid)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            Int32? UserID = Convert.ToInt32(Session["UID"]);

            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {

                if (!String.IsNullOrWhiteSpace(SearchUserID))
                {
                    UserID = Convert.ToInt32(SearchUserID);
                    Session["SearchUserID"] = SearchUserID;
                }
            }
            else
            {
                UserID = Convert.ToInt32(Session["UID"]);
                Session["SearchUserID"] = string.Empty;
            }
            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {
                #region Admin View All Users
                //var userlist = (from asUser in db.crm_tickets
                //                join user in db.crm_usertbl on asUser.AssignedTo equals user.Id
                //                join user2 in db.crm_usertbl on asUser.CreatedBy equals user2.Id
                //                where (asUser.CreatedBy != UserID || asUser.AssignedTo != null) &&
                //                 asUser.BranchID == BranchID
                //                && asUser.CompanyId == CompanyID
                //                && user.Status == true && user2.Status == true
                //                orderby user2.Fname
                //                select new CreateUserModel
                //                {
                //                    UserID = user.Id,
                //                    UserName = user.Fname + " " + user.Lname + "(" + user.EmployeeCode + ")"
                //                }
                //               ).Distinct().ToList();
                var Userlist = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).OrderBy(a => a.Fname).ToList();
                if (Userlist != null && Userlist.Count > 0)
                {
                    List<CreateUserModel> UserList = new List<CreateUserModel>();
                    foreach (var item in Userlist)
                    {
                        CreateUserModel oUrgency = new CreateUserModel();
                        oUrgency.UserID = item.Id;
                        oUrgency.UserName = item.Fname + " " + item.Lname + "(" + item.EmployeeCode + ")";
                        UserList.Add(oUrgency);
                    }
                    CUM.UserList = UserList;
                }
                //db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(em => em.Fname).ToList();
                //if (userlist != null && userlist.Count > 0)
                //{
                //    //List<CRMTicketModel> ouserList = new List<CRMTicketModel>();
                //    //foreach (var item in userlist)
                //    //{
                //    //    CRMTicketModel ouser = new CRMTicketModel();
                //    //    ouser.UserID = item.Id;
                //    //    ouser.UserName = item.Fname + " " + item.Lname;
                //    //    ouserList.Add(ouser);
                //    //}
                //    CUM.UserList = userlist.OrderBy(a => a.UserName).ToList();
                //}
                #endregion
            }
            var itemList = db.crm_itemtypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).OrderBy(a => a.ItemTypeName).ToList();
            if (itemList != null && itemList.Count > 0)
            {
                List<CreateUserModel> itemtypeList = new List<CreateUserModel>();
                foreach (var item in itemList)
                {
                    CreateUserModel oUrgency = new CreateUserModel();
                    oUrgency.ItemTypeId = item.Id;
                    oUrgency.ItemTypeName = item.ItemTypeName;
                    itemtypeList.Add(oUrgency);
                }
                CUM.itemtypelistt = itemtypeList;
            }
            else
            {
                CUM.itemtypelistt = new List<CreateUserModel>();
            }





            #region AssignItem

            //var GetItemlist = db.crm_itemtypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
            if (userid != null && itemid != null)
            {
                var data1 = @"select distinct ITM.ItemTypeName , IAM.ItemTypeId,IAM.Quantity,IAM.Estimated_Cost,IAM.Expirydate,IAM.CheckStatus,IAM.SerialNo,IAM.UserId,IAM.Assigndate,ut.Fname,ut.Lname,ut.EmployeeCode from 
                        crm_itemtypetbl ITM  right join crm_assignitemtypeuser IAM on ITM.Id = IAM.ItemTypeId 
                        join crm_usertbl ut on IAM.UserId=ut.Id where ITM.BranchID = '" + BranchID + "' and ITM.CompanyID = '" + CompanyID + "' and IAM.UserId != 0 and IAM.UserId='" + userid + "' and ItemTypeId='" + itemid + "'";

                var itemdata = db.Database.SqlQuery<ItemAssignlist>(data1).ToList();
                if (itemdata != null && itemdata.Count > 0)
                {
                    CUM.ItemTypeList = (from item in itemdata
                                        select new ItemAssignlist
                                        {
                                            ItemTypeId = item.ItemTypeId,
                                            UserId = item.UserId,
                                            ItemTypeName = item.ItemTypeName,
                                            Quantity = item.Quantity,
                                            Estimated_Cost = item.Estimated_Cost,
                                            Expirydate = item.Expirydate,
                                            CheckStatus = item.CheckStatus,
                                            SerialNo = item.SerialNo,
                                            Assigndate = item.Assigndate,
                                            Fname = item.Fname + " " + item.Lname + "(" + item.EmployeeCode + ")",
                                        }
                                     ).ToList();
                }
            }

            else if (itemid != null)
            {
                var data1 = @"select distinct ITM.ItemTypeName , IAM.ItemTypeId,IAM.Quantity,IAM.Estimated_Cost,IAM.Expirydate,IAM.CheckStatus,IAM.SerialNo,IAM.UserId,IAM.Assigndate,ut.Fname,ut.Lname,ut.EmployeeCode from 
                        crm_itemtypetbl ITM  right join crm_assignitemtypeuser IAM on ITM.Id = IAM.ItemTypeId 
                        join crm_usertbl ut on IAM.UserId=ut.Id where ITM.BranchID = '" + BranchID + "' and ITM.CompanyID = '" + CompanyID + "' and IAM.UserId != 0 and ItemTypeId='" + itemid + "'";

                var itemdata = db.Database.SqlQuery<ItemAssignlist>(data1).ToList();
                if (itemdata != null && itemdata.Count > 0)
                {
                    CUM.ItemTypeList = (from item in itemdata
                                        select new ItemAssignlist
                                        {
                                            ItemTypeId = item.ItemTypeId,
                                            UserId = item.UserId,
                                            ItemTypeName = item.ItemTypeName,
                                            Quantity = item.Quantity,
                                            Estimated_Cost = item.Estimated_Cost,
                                            Expirydate = item.Expirydate,
                                            CheckStatus = item.CheckStatus,
                                            SerialNo = item.SerialNo,
                                            Assigndate = item.Assigndate,
                                            Fname = item.Fname + " " + item.Lname + "(" + item.EmployeeCode + ")",
                                        }
                                     ).ToList();
                }
            }
            else if (userid != null)
            {
                var data1 = @"select distinct ITM.ItemTypeName , IAM.ItemTypeId,IAM.Quantity,IAM.Estimated_Cost,IAM.Expirydate,IAM.CheckStatus,IAM.SerialNo,IAM.UserId,IAM.Assigndate,ut.Fname,ut.Lname,ut.EmployeeCode from 
                        crm_itemtypetbl ITM  right join crm_assignitemtypeuser IAM on ITM.Id = IAM.ItemTypeId 
                        join crm_usertbl ut on IAM.UserId=ut.Id where ITM.BranchID = '" + BranchID + "' and ITM.CompanyID = '" + CompanyID + "' and IAM.UserId != 0 and IAM.UserId='" + userid + "'";

                var itemdata = db.Database.SqlQuery<ItemAssignlist>(data1).ToList();
                if (itemdata != null && itemdata.Count > 0)
                {
                    CUM.ItemTypeList = (from item in itemdata
                                        select new ItemAssignlist
                                        {
                                            ItemTypeId = item.ItemTypeId,
                                            UserId = item.UserId,
                                            ItemTypeName = item.ItemTypeName,
                                            Quantity = item.Quantity,
                                            Estimated_Cost = item.Estimated_Cost,
                                            Expirydate = item.Expirydate,
                                            CheckStatus = item.CheckStatus,
                                            SerialNo = item.SerialNo,
                                            Assigndate = item.Assigndate,
                                            Fname = item.Fname + " " + item.Lname + "(" + item.EmployeeCode + ")",
                                        }
                                     ).ToList();
                }
            }

            else
            {
                var data1 = @"select distinct ITM.ItemTypeName , IAM.ItemTypeId,IAM.Quantity,IAM.Estimated_Cost,IAM.Expirydate,IAM.CheckStatus,IAM.SerialNo,IAM.UserId,IAM.Assigndate,ut.Fname,ut.Lname,ut.EmployeeCode from 
                        crm_itemtypetbl ITM  right join crm_assignitemtypeuser IAM on ITM.Id = IAM.ItemTypeId 
                        join crm_usertbl ut on IAM.UserId=ut.Id where ITM.BranchID = '" + BranchID + "' and ITM.CompanyID = '" + CompanyID + "' and IAM.UserId != 0";

                var itemdata = db.Database.SqlQuery<ItemAssignlist>(data1).ToList();
                if (itemdata != null && itemdata.Count > 0)
                {
                    CUM.ItemTypeList = (from item in itemdata
                                        select new ItemAssignlist
                                        {
                                            ItemTypeId = item.ItemTypeId,
                                            UserId = item.UserId,
                                            ItemTypeName = item.ItemTypeName,
                                            Quantity = item.Quantity,
                                            Estimated_Cost = item.Estimated_Cost,
                                            Expirydate = item.Expirydate,
                                            CheckStatus = item.CheckStatus,
                                            SerialNo = item.SerialNo,
                                            Assigndate = item.Assigndate,
                                            Fname = item.Fname + " " + item.Lname + "(" + item.EmployeeCode + ")",
                                        }
                                     ).ToList();
                }
            }
            #endregion
            return View(CUM);
        }

        public ActionResult Removedcheckeditemtypedata(int id, int uid)
        {
            string msg = "";
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                var GetitemDetails = db.crm_assignitemtypeuser.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.ItemTypeId == id && em.UserId == uid).FirstOrDefault();
                {
                    GetitemDetails.CheckStatus = "unchk";
                    GetitemDetails.UserId = 0;
                    GetitemDetails.SerialNo = "";
                    GetitemDetails.Quantity = 0;
                    GetitemDetails.Estimated_Cost = 0;
                    GetitemDetails.Expirydate = null;
                    db.SaveChanges();
                    TempData["success"] = "Assign Item Remove successfully";

                }

            }
            catch (Exception ex)
            {

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult saveitemtypedata(int id, int? QTY, decimal EST, string EXD, int uid, string SNO, string AssignDate)
        {
            string msg = "";
            try
            {
                var DateFormat = Constant.DateFormat();
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                int UID = 0;
                if (uid == 0)
                {
                    UID = Convert.ToInt32(Session["UserID"]);
                }
                else
                {
                    UID = uid;
                }

                var GetitemDetails = db.crm_assignitemtypeuser.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.ItemTypeId == id && em.UserId == uid).FirstOrDefault();
                if (GetitemDetails != null)
                {
                    GetitemDetails.SerialNo = SNO;
                    GetitemDetails.UserId = UID;
                    GetitemDetails.Quantity = QTY;
                    GetitemDetails.Estimated_Cost = EST;
                    GetitemDetails.CheckStatus = "chk";
                    if (!string.IsNullOrWhiteSpace(EXD))
                    {
                        GetitemDetails.Expirydate = DateTime.ParseExact(EXD, DateFormat, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        GetitemDetails.Expirydate = null;
                    }
                    if (!string.IsNullOrWhiteSpace(AssignDate))
                    {
                        GetitemDetails.Assigndate = DateTime.ParseExact(AssignDate, DateFormat, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        GetitemDetails.Assigndate = null;
                    }
                    db.SaveChanges();
                    TempData["success"] = "Assign Item Updated successfully";
                }
                else
                {
                    crm_assignitemtypeuser als = new crm_assignitemtypeuser();
                    als.ItemTypeId = id;
                    als.CompanyID = CompanyID;
                    als.BranchID = BranchID;
                    als.UserId = uid;
                    als.Estimated_Cost = EST;
                    als.Quantity = QTY;
                    als.SerialNo = SNO;
                    als.CheckStatus = "chk";
                    if (!string.IsNullOrWhiteSpace(EXD))
                    {
                        als.Expirydate = DateTime.ParseExact(EXD, DateFormat, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        als.Expirydate = null;
                    }
                    if (!string.IsNullOrWhiteSpace(AssignDate))
                    {
                        als.Assigndate = DateTime.ParseExact(AssignDate, DateFormat, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        als.Assigndate = null;
                    }
                    db.crm_assignitemtypeuser.Add(als);
                    db.SaveChanges();
                    TempData["success"] = "Assign Item successfully";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> AddComapnyDDLItem(string ItemName)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            dynamic expando = new ExpandoObject();

            if (Session["CompanyID"] != null)
            {
                try
                {
                    var checkExist = await db.crm_usercompanytypetbl.Where(em => em.CompanyTypeName.ToLower() == ItemName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                    if (checkExist == null)
                    {
                        crm_usercompanytypetbl pdt = new crm_usercompanytypetbl();
                        pdt.CompanyTypeName = ItemName;
                        pdt.BranchID = BranchID;
                        pdt.CompanyID = CompanyID;
                        pdt.Status = true;
                        db.crm_usercompanytypetbl.Add(pdt);
                        await db.SaveChangesAsync();

                        expando.Msg = "ok";
                        expando.AddedItem = ItemName;
                        expando.ItemId = pdt.Id;
                        //return Json(expando, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        expando.Msg = "Company type already exist";
                        //return Json(expando, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    expando.Msg = "Something went wrong! please try again";
                }
            }
            else
            {
                expando.Msg = "expire";
            }
            var json = JsonConvert.SerializeObject(expando);//convert to json object
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> AddCustomergroupname(string customgrpname)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            dynamic expando = new ExpandoObject();

            if (Session["CompanyID"] != null)
            {
                try
                {
                    customersgroup pdt = new customersgroup();
                    var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
                    for (int i = 0; i < comptype.Count; i++)
                    {
                        int multicompany = Convert.ToInt32(comptype[i].ID);

                        var multibranch = await db.com_branches.Where(em => em.OrganizationID == multicompany).FirstOrDefaultAsync();
                        {
                            if (multibranch != null)
                            {
                                int multib = Convert.ToInt32(multibranch.OrgBranchCode);
                                var checkExist = await db.customersgroups.Where(em => em.CustGroupName == customgrpname && em.BranchCode == multib && em.CompanyId == multicompany).FirstOrDefaultAsync();
                                if (checkExist == null)
                                {
                                    pdt.CustGroupName = customgrpname;
                                    pdt.BranchCode = multib;
                                    pdt.CompanyId = multicompany;
                                    pdt.CreatedBy = Convert.ToString(Session["UserName"]);
                                    pdt.CreatedDate = Constant.GetBharatTime();
                                    pdt.ModifiedBy = Convert.ToString(Session["UserName"]);
                                    pdt.ModifiedDate = Constant.GetBharatTime();
                                    string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM customersgroup";
                                    int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                                    string synId = "O" + (syncid + 1);
                                    pdt.SyncID = synId;
                                    pdt.Flag = "N";
                                    db.customersgroups.Add(pdt);
                                    await db.SaveChangesAsync();


                                    //return Json(expando, JsonRequestBehavior.AllowGet);

                                }
                                else
                                {

                                    //expando.Msg = "Cus type already exist";
                                    //return Json(expando, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }

                    }
                    var mancompany = await db.customersgroups.Where(em => em.CustGroupName == customgrpname && em.BranchCode == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();
                    if (mancompany == null)
                    {
                        pdt.CustGroupName = customgrpname;
                        pdt.BranchCode = BranchID;
                        pdt.CompanyId = CompanyID;

                        db.customersgroups.Add(pdt);
                        await db.SaveChangesAsync();


                        //return Json(expando, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        //expando.Msg = "Cus type already exist";
                        //return Json(expando, JsonRequestBehavior.AllowGet);
                    }
                    expando.Msg = "ok";
                    expando.CustGroupName = customgrpname;
                    expando.ID = pdt.ID;


                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    expando.Msg = "Something went wrong! please try again";
                }
            }
            else
            {
                expando.Msg = "expire";
            }
            var json = JsonConvert.SerializeObject(expando);//convert to json object
            return Json(json, JsonRequestBehavior.AllowGet);
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
            return Redirect("/home/users");
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
                    Session["ReturnUrl"] = "/home/users";
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


        public ActionResult GetEmpCodeWithAutoComplete(string reqstr)
        {
            Empcode FNDM = new Empcode();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            try
            {

                var allempcode = (from item in db.crm_usertbl
                                  where item.CompanyID == CompanyID && item.BranchID == BranchID && item.ByUID != null
                                  //&& item.Status == true
                                  select new Empcode
                                  {
                                      EmployeeCode = item.EmployeeCode,
                                  }).ToList();


                allempcode = allempcode.Where(em => em.EmployeeCode.ToLower().Contains(reqstr.ToLower())).ToList();
                if (allempcode != null && allempcode.Count > 0)
                {
                    for (int i = 0; i < allempcode.Count; i++)
                    {
                        var empcode = allempcode[i].EmployeeCode;
                        var ex = FNDM.empcodeList.Any(a => a.EmployeeCode == empcode);
                        if (ex == false)
                        {
                            var cal = new Empcode
                            {
                                EmployeeCode = empcode,
                            };
                            FNDM.empcodeList.Add(cal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["alertMsg"] = ex.Message.ToString();
                return Json("/admin/users");
            }

            return Json(FNDM.empcodeList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Assign-Role
        public ActionResult assignrole()
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                ViewBag.result = db.crm_roleassigntbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(a => a.ProfileName).ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View();
        }
        public ActionResult createrole(int? id)
        {
            //var rights = CommonRepository.GetUserRights();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            CreatRoleModel CreateroleModel = new CreatRoleModel();
            CreateroleModel.IsIndia = cr.GetCompanyCountry();//get company coutry is india for show indiamart Settings?
            //var GetData = db.crm_roleassigntbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
            if (id > 0)
            {
                var GetData = db.crm_roleassigntbl.Find(id);
                if (GetData != null)
                {
                    //CreateroleModel.ProfileName = GetData.ProfileName;
                    //CreateroleModel.CreateLeads = Convert.ToBoolean(GetData.CreateLeads);
                    //CreateroleModel.ViewLeads = Convert.ToBoolean(GetData.ViewLeads);
                    //CreateroleModel.ViewSales = Convert.ToBoolean(GetData.ViewSales);
                    //CreateroleModel.CommonAvtivityRemarks = Convert.ToBoolean(GetData.CommonActivityRemark);
                    //CreateroleModel.RoleManagement = Convert.ToBoolean(GetData.RoleManagement);
                    //CreateroleModel.Notification = Convert.ToBoolean(GetData.Notification);
                    //CreateroleModel.LeadNotify = Convert.ToBoolean(GetData.LeadNotify);
                    //CreateroleModel.DeveloperReport = Convert.ToBoolean(GetData.DeveloperReport);
                    //CreateroleModel.ProjectManagement = Convert.ToBoolean(GetData.ProjectManagement);
                    //CreateroleModel.LeadManagement = Convert.ToBoolean(GetData.LeadManagement);
                    //CreateroleModel.AssignLeadManagement = Convert.ToBoolean(GetData.AssignLeadManagement);
                    //CreateroleModel.DailyworkSchedule = Convert.ToBoolean(GetData.DailyWorkSchedule);
                    //CreateroleModel.ViewPayment = Convert.ToBoolean(GetData.Viewpayment);
                    var data = Mapper.Map<CreatRoleModel>(GetData);
                    CreateroleModel = data;
                }
            }

            return View(CreateroleModel);
        }

        [HttpPost]
        public ActionResult createrole(CreatRoleModel CRM, int? id)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (id > 0)
                {
                    //var GetData = db.crm_roleassigntbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();

                    var data = Mapper.Map<crm_roleassigntbl>(CRM);
                    data.BranchID = BranchID;
                    data.CompanyID = CompanyID;
                    db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["success"] = "Data updated successfully";
                    //if (GetData != null)
                    //{

                    //GetData.ProfileName = CRM.ProfileName;
                    //GetData.CreateLeads = Convert.ToBoolean(CRM.CreateLeads);
                    //GetData.ViewLeads = Convert.ToBoolean(CRM.ViewLeads);
                    //GetData.ViewSales = Convert.ToBoolean(CRM.ViewSales);
                    //GetData.DeveloperReport = Convert.ToBoolean(CRM.DeveloperReport);
                    //GetData.CommonActivityRemark = Convert.ToBoolean(CRM.CommonAvtivityRemarks);
                    //GetData.RoleManagement = Convert.ToBoolean(CRM.RoleManagement);
                    //GetData.Notification = Convert.ToBoolean(CRM.Notification);
                    //GetData.LeadNotify = Convert.ToBoolean(CRM.LeadNotify);
                    //GetData.ProjectManagement = Convert.ToBoolean(CRM.ProjectManagement);
                    //GetData.LeadManagement = Convert.ToBoolean(CRM.LeadManagement);
                    //GetData.AssignLeadManagement = Convert.ToBoolean(CRM.AssignLeadManagement);
                    //GetData.DailyWorkSchedule = Convert.ToBoolean(CRM.DailyworkSchedule);
                    //GetData.Viewpayment = Convert.ToBoolean(CRM.ViewPayment);
                    //GetData.BranchID = BranchID;
                    //GetData.CompanyID = CompanyID;
                    //db.SaveChanges();
                    //TempData["success"] = "Data updated successfully";
                    //}
                }
                else
                {
                    //crm_roleassigntbl RA = new crm_roleassigntbl();
                    //RA.ProfileName = CRM.ProfileName;
                    //RA.CreateLeads = CRM.CreateLeads;
                    //RA.ViewLeads = CRM.ViewLeads;
                    //RA.ViewSales = CRM.ViewSales;
                    //RA.DeveloperReport = CRM.DeveloperReport;
                    //RA.CommonActivityRemark = CRM.CommonAvtivityRemarks;
                    //RA.RoleManagement = CRM.RoleManagement;
                    //RA.Notification = CRM.Notification;
                    //RA.LeadNotify = CRM.LeadNotify;
                    //RA.ProjectManagement = CRM.ProjectManagement;
                    //RA.LeadManagement = CRM.LeadManagement;
                    //RA.AssignLeadManagement = CRM.AssignLeadManagement;
                    //RA.DailyWorkSchedule = CRM.DailyworkSchedule;
                    //RA.Viewpayment = CRM.ViewPayment;
                    //RA.Status = true;
                    //RA.created_at = System.DateTime.Now;
                    //RA.BranchID = BranchID;
                    //RA.CompanyID = CompanyID;
                    //db.crm_roleassigntbl.Add(RA);
                    //int i = db.SaveChanges();

                    var data = Mapper.Map<crm_roleassigntbl>(CRM);
                    data.created_at = Constant.GetBharatTime();
                    data.BranchID = BranchID;
                    data.CompanyID = CompanyID;
                    data.Status = true;
                    db.crm_roleassigntbl.Add(data);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        id = data.Id;
                        TempData["success"] = "Data added successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry there is some problem!";
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "Sorry there is some problem!";
            }
            return RedirectToAction("createrole", new { id = id });
            //return Redirect("/home/createrole");
        }
        #endregion

        #endregion

        #region Notification
        public ActionResult Notification()
        {
            NotificationModel NM = new NotificationModel();
            try
            {

                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    //var GetNotificationList = db.P_GetRequestLeads(false).ToList();
                    //foreach (var item in GetNotificationList)
                    //{
                    //    var cal = new NotificationModel
                    //    {
                    //        Id = item.Id,
                    //        UserName = item.Fname + " " + item.Lname,
                    //        Date = Convert.ToDateTime(item.Created_at),
                    //        Leadstype = item.LeadsType,
                    //        NoofLeads = Convert.ToInt32(item.NoOfLeads),
                    //        State = item.State
                    //    };
                    //    NM.notificationList.Add(cal);
                    //}
                }
                else
                {
                    int Uid = Convert.ToInt32(Session["UID"]);
                    //var GetMapUser = db.usertbls.Where(em => em.Id == Uid).Select(em => em.MappedUsers).SingleOrDefault();
                    //if (GetMapUser != null)
                    //{
                    //    var spliteMapUser = GetMapUser.Split(',');
                    //    foreach (var item in spliteMapUser)
                    //    {
                    //        int MapUid = Convert.ToInt32(item);
                    //        var GetNotificationList = db.P_GetRequestLeads(false).Where(em => em.UID == MapUid).ToList();
                    //        foreach (var Nitem in GetNotificationList)
                    //        {
                    //            var cal = new NotificationModel
                    //            {
                    //                Id = Nitem.Id,
                    //                UserName = Nitem.Fname + " " + Nitem.Lname,
                    //                Date = Convert.ToDateTime(Nitem.Created_at),
                    //                Leadstype = Nitem.LeadsType,
                    //                NoofLeads = Convert.ToInt32(Nitem.NoOfLeads),
                    //                State = Nitem.State
                    //            };
                    //            NM.notificationList.Add(cal);
                    //        }
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
            }
            return View(NM);
        }

        public ActionResult ApprovedLeads(int Id)
        {
            string retMsg = string.Empty;
            if (Session["UserName"] != null)
            {
                try
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    var UID = Convert.ToInt32(Session["UID"]);
                    var GetLeads = db.crm_leadrequesttbl.Where(em => em.Id == Id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetLeads != null)
                    {
                        GetLeads.Status = true;
                        GetLeads.ApprovedBy = UID;
                        GetLeads.ApprovedDate = System.DateTime.Now;
                        GetLeads.BranchID = BranchID;
                        GetLeads.CompanyID = CompanyID;
                        db.SaveChanges();
                        retMsg = "success";
                    }
                    else
                    {
                        retMsg = "problem";
                    }

                }
                catch (Exception ex)
                {
                    retMsg = "problem";
                    ExceptionLogging.SendExcepToDB(ex);
                }
            }
            else
            {
                Session["ReturnUrl"] = "/home/ApprovedLeads";
                return Redirect("/home/login");
            }
            return Json(retMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Our-MappedUser
        public ActionResult OurMappeduser()
        {
            MappedUserModel MUM = new MappedUserModel();
            try
            {
                if (Session["UserName"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    var UID = Convert.ToInt32(Session["UID"]);

                    var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetUserData != null && GetUserData.MappedUsers != null)
                    {
                        var GetMapUser = GetUserData.MappedUsers.Split(',');
                        foreach (var item in GetMapUser)
                        {
                            var mapid = Convert.ToInt32(item);
                            var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (GetMapUserData != null)
                            {
                                var count = new MappedUserModel
                                {
                                    MapUserId = mapid,
                                    EMpName = GetMapUserData.Fname + " " + GetMapUserData.Lname,
                                    Email = GetMapUserData.Email,
                                    Profile = GetMapUserData.ProfileName
                                };
                                MUM.mappeduserList.Add(count);
                            }
                        }
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/home/OurMappeduser";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/home/login");
            }
            return View(MUM);
        }

        #region Mapped-User-Developer

        public ActionResult OurMappeduserDeveloper(int? id)
        {
            DeveloperActivityModel DAM = new DeveloperActivityModel();
            try
            {
                if (Session["UserName"] != null)
                {
                    if (id != null)
                    {
                        Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                        Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                        #region Data-time-Formate
                        var dd = System.DateTime.Now;
                        DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                        DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                        var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                        var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                        #endregion
                        var GetMapUserName = db.crm_usertbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).Select(em => new { Name = em.Fname + " " + em.Lname }).FirstOrDefault();
                        TempData["MapUserName"] = GetMapUserName.Name;

                        DataTable GetAllActivityRecord = DataAccessLayer.GetDataTable(" call CRM_ViewActivityReportbyUser('" + id + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetAllActivityRecord.Rows.Count > 0)
                        {
                            DAM.DeveloperactivityModelList = (from dr in GetAllActivityRecord.AsEnumerable()
                                                              select new DeveloperActivityModel()
                                                              {
                                                                  Id = Convert.ToInt32(dr["Id"]),
                                                                  Uid = Convert.ToInt32(dr["UID"]),
                                                                  Project_ID = Convert.ToInt64(dr["Project_ID"]),
                                                                  ProjectModule_ID = Convert.ToInt32(dr["ProjectModule_ID"]),
                                                                  GeneralRemark = Convert.ToString(dr["GeneralRemark"]),
                                                                  CodeModuleRemark = Convert.ToString(dr["CodeModuleRemark"]),
                                                                  DBModuleRemark = Convert.ToString(dr["DBModuleRemark"]),
                                                                  JsModuleRemark = Convert.ToString(dr["JsModuleRemark"]),
                                                                  SupportNeeded = Convert.ToString(dr["SupportNeeded"]),
                                                                  CodeFile = Convert.ToString(dr["CodeFile"]),
                                                                  CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                                                  ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                  ModuleName = Convert.ToString(dr["ModuleName"])
                                                              }).ToList();

                        }

                    }
                    else
                    {
                        return Redirect("/home/OurMappeduser");
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/home/OurMappeduserDeveloper";
                    return Redirect("/home/login");
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(DAM);
        }

        public ActionResult FilterMappedDeveloper(int EmpId, string FromDate, string ToDate)
        {
            DeveloperActivityModel DAM = new DeveloperActivityModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                DataTable GetAllActivityRecord = DataAccessLayer.GetDataTable(" call CRM_ViewActivityReportbyUser('" + EmpId + "','" + FromDate + "','" + ToDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetAllActivityRecord.Rows.Count > 0)
                {
                    DAM.DeveloperactivityModelList = (from dr in GetAllActivityRecord.AsEnumerable()
                                                      select new DeveloperActivityModel()
                                                      {
                                                          Id = Convert.ToInt32(dr["Id"]),
                                                          Uid = Convert.ToInt32(dr["UID"]),
                                                          Project_ID = Convert.ToInt64(dr["Project_ID"]),
                                                          ProjectModule_ID = Convert.ToInt32(dr["ProjectModule_ID"]),
                                                          GeneralRemark = Convert.ToString(dr["GeneralRemark"]),
                                                          CodeModuleRemark = Convert.ToString(dr["CodeModuleRemark"]),
                                                          DBModuleRemark = Convert.ToString(dr["DBModuleRemark"]),
                                                          JsModuleRemark = Convert.ToString(dr["JsModuleRemark"]),
                                                          SupportNeeded = Convert.ToString(dr["SupportNeeded"]),
                                                          CodeFile = Convert.ToString(dr["CodeFile"]),
                                                          CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                          ModuleName = Convert.ToString(dr["ModuleName"])
                                                      }).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_FilterDevloperActivityDatewiseByUser", DAM);
        }
        #endregion

        #region Mapped-Sales-User
        public ActionResult OurMappeduserSales(int? id)
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            try
            {
                if (id != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    #region Lead Status
                    string leadStatusQry = @"select Id,LeadStatusName from crm_leadstatus_tbl where BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "'and Status=1";
                    VLM.leadstatusList = db.Database.SqlQuery<LeadStatusModel>(leadStatusQry).OrderBy(a => a.LeadStatusname).ToList();

                    #endregion

                    #region Data-time-Formate
                    var dd = Constant.GetBharatTime();
                    VLM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd-MM-yy");
                    var MEndDate = MonthendDate.ToString("dd-MM-yy");

                    if (VLM.DateFormat != "dd-MM-yy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        var fmDate = DateTime.ParseExact(MStartDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                        var tDate = DateTime.ParseExact(MEndDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                        MStartDate = String.Format("{0:dd-MM-yy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                        MEndDate = String.Format("{0:dd-MM-yy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                    }

                    #endregion

                    var GetMapUserName = db.crm_usertbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).Select(em => new { Name = em.Fname + " " + em.Lname }).FirstOrDefault();
                    TempData["MapUserName"] = GetMapUserName.Name;
                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate('" + id + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetRecords.Rows.Count > 0)
                    {
                        VLM.viewleadsList = (from dr in GetRecords.AsEnumerable()
                                             select new ViewLeadsModel()
                                             {
                                                 Id = Convert.ToInt32(dr["Id"]),
                                                 LeadName = Convert.ToString(dr["Customer"]),
                                                 Mob = Convert.ToString(dr["MobileNo"]),
                                                 FollowupDate = Convert.ToString(dr["FollowDate"]),
                                                 LeadStatus = Convert.ToString(dr["LeadStatus"])
                                             }).ToList();
                    }
                }
                else
                {
                    return Redirect("/home/OurMappeduser");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Redirect("/home/login");
            }
            return View(VLM);
        }

        public ActionResult FilterMappeduserLeads(int EmpId, string FromDate, string ToDate, string leadstatus)
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate('" + EmpId + "','" + FromDate + "','" + ToDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetRecords.Rows.Count > 0)
                {
                    VLM.viewleadsList = (from dr in GetRecords.AsEnumerable()
                                         select new ViewLeadsModel()
                                         {
                                             Id = Convert.ToInt32(dr["Id"]),
                                             LeadName = Convert.ToString(dr["Customer"]),
                                             Mob = Convert.ToString(dr["MobileNo"]),
                                             FollowupDate = Convert.ToString(dr["FollowDate"]),
                                             LeadStatus = Convert.ToString(dr["LeadStatus"])
                                         }).ToList();
                }

                if (!string.IsNullOrEmpty(leadstatus) && leadstatus != "None")
                {
                    var SearchByLeadsData = VLM.viewleadsList.Where(em => em.LeadStatus == leadstatus).ToList();
                    VLM.viewleadsList = SearchByLeadsData.ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialViewMappeduserLeads", VLM);
        }

        public ActionResult ExportMappeduserSalesLeads(int? id)
        {
            try
            {
                var ViewSalesLeads = (List<ViewLeadsModel>)TempData["MappedUser-Leads"];
                DataTable dt = new DataTable();
                dt.Columns.Add("Customer Name", typeof(string));
                dt.Columns.Add("Mobile No", typeof(string));
                dt.Columns.Add("Followup Date", typeof(string));
                dt.Columns.Add("Lead Status", typeof(string));
                foreach (var Ditem in ViewSalesLeads)
                {
                    DataRow dr = dt.NewRow();
                    dr["Customer Name"] = Ditem.LeadName;
                    dr["Mobile No"] = Ditem.Mob;
                    dr["Followup Date"] = Ditem.FollowupDate;
                    dr["Lead Status"] = Ditem.LeadStatus;
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Customers");
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=MappedUserSalesR-Report.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                return Json("done", JsonRequestBehavior.AllowGet);
                //return Redirect("/home/OurMappeduserSales/"+"17");
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Json("error", JsonRequestBehavior.AllowGet);
                //return Redirect("/home/OurMappeduserSales/" + "17");
            }
        }

        #endregion

        #endregion

        #region News-Events
        public ActionResult NewsEvent(int? id)
        {
            NewsEventsModel NEM = new NewsEventsModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (id != null)
                {
                    var Getdata = db.crm_newseventtbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (Getdata != null)
                    {
                        NEM.Events = Getdata.News;
                    }
                }
                ViewBag.result = db.crm_newseventtbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(NEM);
        }

        [HttpPost]
        public ActionResult NewsEvent(NewsEventsModel NEM, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (id != null)
            {
                var GetEvents = db.crm_newseventtbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetEvents != null)
                {
                    GetEvents.News = NEM.Events;
                    GetEvents.BranchID = BranchID;
                    GetEvents.CompanyID = CompanyID;
                    db.SaveChanges();
                }
            }
            else
            {
                crm_newseventtbl NE = new crm_newseventtbl();
                NE.News = NEM.Events;
                NE.Status = true;
                NE.BranchID = BranchID;
                NE.CompanyID = CompanyID;
                db.crm_newseventtbl.Add(NE);
                db.SaveChanges();
            }
            return Redirect("/home/NewsEvent");
        }

        public ActionResult DeleteNewsEvents(int id)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var GetDeletedData = db.crm_newseventtbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetDeletedData != null)
                {
                    db.crm_newseventtbl.Remove(GetDeletedData);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return Redirect("/home/NewsEvent");
        }
        #endregion

        #region Lead-Notify
        public ActionResult leadnotify()
        {
            LeadNotifyModel LNM = new LeadNotifyModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    var GetNotificationList = db.crm_createleadstbl.Where(em => em.Notify == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    foreach (var item in GetNotificationList)
                    {
                        var cal = new LeadNotifyModel
                        {
                            Id = Convert.ToInt32(item.Id),
                            LeadName = item.Customer,
                            MobileNo = item.MobileNo,
                            Date = Convert.ToDateTime(item.Createddate),
                            Leadstype = item.LeadsType,
                            State = item.State,
                            NotifyByUser = item.NotifybyUser
                        };
                        LNM.LeadNotifyList.Add(cal);
                    }
                }
                else
                {
                    int Uid = Convert.ToInt32(Session["UID"]);
                    var GetNotificationList = db.crm_createleadstbl.Where(em => em.LeadOwner == Uid && em.Notify == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    if (GetNotificationList != null)
                    {
                        foreach (var Nitem in GetNotificationList)
                        {
                            var cal = new LeadNotifyModel
                            {
                                Id = Convert.ToInt32(Nitem.Id),
                                LeadName = Nitem.Customer,
                                MobileNo = Nitem.MobileNo,
                                Date = Convert.ToDateTime(Nitem.Createddate),
                                Leadstype = Nitem.LeadsType,
                                State = Nitem.State
                            };
                            LNM.LeadNotifyList.Add(cal);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
            }
            return View(LNM);
        }

        public ActionResult ApprovedNotifyLeads(int Id)
        {
            string retMsg = "";
            if (Session["UserName"] != null)
            {
                try
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    var UID = Convert.ToInt32(Session["UID"]);
                    var GetLeads = db.crm_createleadstbl.Where(em => em.Id == Id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetLeads != null)
                    {
                        GetLeads.Notify = false;
                        GetLeads.CompanyID = CompanyID;
                        GetLeads.BranchID = BranchID;
                        //GetLeads.ApprovedBy = UID;
                        //GetLeads.ApprovedDate = System.DateTime.Now;
                        db.SaveChanges();
                        retMsg = "success";
                    }
                    else
                    {
                        retMsg = "problem";
                    }

                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    retMsg = "problem";
                }
            }
            else
            {
                Session["ReturnUrl"] = "/home/leadnotify";
                return Redirect("/home/login");
            }
            return Json(retMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Assign Leads Management
        public ActionResult Assign_Leads_Management(string filterText, string FromDate, string ToDate, string UserddlName)
        {
            LeadManagementModel LMM = new LeadManagementModel();
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    int UID = Convert.ToInt32(Session["UID"]);

                    #region Data-time-Formate
                    var dd = Constant.GetBharatTime().AddDays(-30);
                    var dd2 = Constant.GetBharatTime();
                    //DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = dd.ToString("dd/MM/yyyy");
                    var MEndDate = dd2.ToString("dd/MM/yyyy");
                    if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
                    {
                        MStartDate = FromDate;
                        MEndDate = ToDate;
                    }
                    #endregion

                    #region Users

                    //var GetRecords = db.P_GetLeadsbyDateToAssign(UID, MStartDate, MEndDate).Where(em => em.LeadStatus != "Closed").ToList();    //New
                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetLeadsbyDateToAssign('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    var data = (from dr in GetRecords.AsEnumerable()
                                select new
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    FName = Convert.ToString(dr["Fname"]),
                                    LName = Convert.ToString(dr["Lname"]),
                                    Status = Convert.ToBoolean(dr["Status"]),
                                    Customer = Convert.ToString(dr["Customer"]),
                                    Email = dr.Field<string>("EmailId"),
                                    MobileNo = Convert.ToString(dr["MobileNo"]),
                                    FollowDate = Convert.ToString(dr["FollowDate"]),
                                    LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                    Country = Convert.ToString(dr["Country"]),
                                    State = Convert.ToString(dr["State"]),
                                    City = Convert.ToString(dr["City"]),
                                    date = Convert.ToString(dr["date"]),
                                }).Where(em => em.LeadStatus != "Not Interested").ToList();

                    foreach (var item in data)
                    {
                        //var checkAssigned = db.leadassignhistorytbls.Where(em => em.LeadId == item.Id && em.LeadAssignBy == UID).SingleOrDefault();
                        //if (checkAssigned == null)
                        //{
                        var cal = new LeadManagementModel
                        {
                            Id = Convert.ToInt32(item.Id),
                            CustomerName = item.Customer,
                            PrimaryPhNo = item.MobileNo,
                            Email = item.Email,
                            FollowUpDate = item.FollowDate,
                            CreatedBy = item.FName + " " + item.LName,
                            LeadStatus = item.LeadStatus,
                            Country = item.Country,
                            State = item.State,
                            City = item.City,
                            Date = item.date
                        };
                        LMM.leadManagementmodelList.Add(cal);
                        //}
                    }
                    //if (!string.IsNullOrEmpty(filterText))
                    //{
                    //    if (filterText != "All")
                    //    {
                    //        LMM.leadManagementmodelList = LMM.leadManagementmodelList.Where(em => em.LeadStatus == filterText).ToList();
                    //    }
                    //}

                    //if (filterText != null)
                    //{
                    //    var FilterData = VLM.viewleadsList.Where(em => em.LeadStatus.Contains(filterText)).ToList();
                    //    VLM.viewleadsList = FilterData;
                    //}
                    #endregion
                }
                else
                {
                    Session["ReturnUrl"] = "/home/Assign_Leads_Management";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["Alert"] = ex.Message.ToString();
            }
            return View(LMM);
        }

        [HttpPost]
        public ActionResult ManageAssign_Leads_Management(LeadManagementModel LMM)
        {
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    #region calculate the Assigned Leads
                    string Assignedlist = null;
                    if (Request.Form["MapperUserLeads"] != null)
                    {
                        Assignedlist = Request.Form["MapperUserLeads"].ToString();
                    }
                    #endregion


                    if (Assignedlist != null && LMM.AssignTo != null)
                    {
                        var SpliteAssignedLeads = Assignedlist.Split(',');
                        var AssignedId = Convert.ToInt32(LMM.AssignTo);

                        DateTime utcTime = DateTime.UtcNow;
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

                        var time = localTime.ToString("hh:mm:ss tt");//System.DateTime.Now.ToString("hh:mm:ss tt");
                        var Date = localTime.ToString("dd/MM/yyyy");//System.DateTime.Now.ToString("MM/dd/yyyy");

                        foreach (var item in SpliteAssignedLeads)
                        {
                            var SpliteItem = item.Split('-');
                            crm_leadassignhistorytbl LAS = new crm_leadassignhistorytbl();
                            LAS.LeadId = Convert.ToInt64(SpliteItem[0]);
                            LAS.LeadAssignBy = Convert.ToInt32(Session["UID"]);
                            LAS.LeadAssignTo = AssignedId;
                            LAS.LeadAssignDate = localTime.Date.ToString("dd/MM/yyyy");
                            LAS.LeadStatus = Convert.ToString(SpliteItem[1]);
                            LAS.CreatedDate = localTime;
                            LAS.BranchID = BranchID;
                            LAS.CompanyID = CompanyID;
                            db.crm_leadassignhistorytbl.Add(LAS);

                            var createLead = db.crm_createleadstbl.Where(em => em.Id == LAS.LeadId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (createLead != null)
                            {
                                createLead.AssignTo = AssignedId.ToString();
                                createLead.BranchID = BranchID;
                                createLead.CompanyID = CompanyID;
                                createLead.AssignedDate = localTime;
                                createLead.AssignedBy = Convert.ToString(Session["UID"]);
                            }
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        TempData["Alert"] = "Please select the Leads and User to Assign";
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/home/Assign_Leads_Management";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["Alert"] = ex.Message.ToString();
            }
            return Redirect("/home/Assign_Leads_Management");
        }

        public JsonResult GeMappeduserList()
        {
            LeadManagementModel LMM = new LeadManagementModel();
            int UID = Convert.ToInt32(Session["UID"]);
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            #region Get-MappedUser-Parents
            var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
            if (GetUserData != null && GetUserData.MappedUsers != null)
            {
                var GetMapUser = GetUserData.MappedUsers.Split(',');
                foreach (var item in GetMapUser)
                {
                    var mapid = Convert.ToInt32(item);
                    var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetMapUserData != null)
                    {
                        var count = new MapUserModel
                        {
                            Id = mapid,
                            UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
                        };
                        LMM.mapUserList.Add(count);
                    }
                    //ViewBag.AssignTo = new SelectList(LMM.mapUserList, "Id", "UserName");
                }
            }
            //var GetUserDataDepart = db.usertbls.Where(em => em.ProfileName == GetUserData.ProfileName && em.Status == true).ToList();
            //if (GetUserDataDepart != null)
            //{
            //    foreach (var item in GetUserDataDepart)
            //    {
            //        var data = LMM.mapUserList.Where(em => em.Id == item.Id).SingleOrDefault();
            //        if (data == null)
            //        {
            //            var count = new MapUserModel
            //            {
            //                Id = item.Id,
            //                UserName = item.Fname + " " + item.Lname
            //            };
            //            LMM.mapUserList.Add(count);
            //        }
            //    }
            //}
            #endregion

            return Json(LMM.mapUserList.OrderBy(em => em.UserName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartmentuserList()
        {
            LeadManagementModel LMM = new LeadManagementModel();
            int UID = Convert.ToInt32(Session["UID"]);
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            #region Get-MappedUser-Parents
            var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
            if (GetUserData != null && GetUserData.MappedUsers != null)
            {
                var GetMapUser = GetUserData.MappedUsers.Split(',');
                foreach (var item in GetMapUser)
                {
                    var mapid = Convert.ToInt32(item);
                    var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetMapUserData != null)
                    {
                        var count = new MapUserModel
                        {
                            Id = mapid,
                            UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
                        };
                        LMM.mapUserList.Add(count);
                    }
                    //ViewBag.AssignTo = new SelectList(LMM.mapUserList, "Id", "UserName");
                }
            }
            var GetUserDataDepart = db.crm_usertbl.Where(em => em.ProfileName == GetUserData.ProfileName && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
            if (GetUserDataDepart != null)
            {
                foreach (var item in GetUserDataDepart)
                {
                    var data = LMM.mapUserList.Where(em => em.Id == item.Id).FirstOrDefault();
                    if (data == null)
                    {
                        var count = new MapUserModel
                        {
                            Id = item.Id,
                            UserName = item.Fname + " " + item.Lname
                        };
                        LMM.mapUserList.Add(count);
                    }
                }
            }
            #endregion

            return Json(LMM.mapUserList.OrderBy(em => em.UserName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created On : 10/01/2019
        /// Bind User List of Sales
        /// </summary>
        /// <returns></returns>
        public JsonResult BindSaleUser()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            List<MapUserModel> userList = new List<MapUserModel>();
            DataTable GetUserDataDepart = DataAccessLayer.GetDataTable("call CRM_GetSalesUserList(" + BranchID + "," + CompanyID + ")");
            if (GetUserDataDepart != null)
            {
                userList = (from dr in GetUserDataDepart.AsEnumerable()
                            select new MapUserModel()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                UserName = Convert.ToString(dr["UserName"])
                            }).ToList();
            }
            return Json(userList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Upload and Download Facebook Sample File
        public ActionResult FBDownloadFile(string filename)
        {
            string FullPath = Path.Combine(Server.MapPath("~/Sample/"), filename);
            return File(FullPath, "application/vnd.openxmlformats - officedocument.spreadsheetml.sheet", "FacebookLeadSample.xlsx");
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> CMRFBUploadLead()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            int UID = Convert.ToInt32(Session["UID"]);
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
                if (Postfile.ContentLength > 0)
                {
                    string FileFullName = string.Empty;
                    string conString = string.Empty;
                    var supportedTypes = new[] { "xls", "xlsx", "csv" };
                    var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
                    if (supportedTypes.Contains(fileExt))
                    {
                        #region Old Code Excel Upload
                        DataTable dts = new DataTable();
                        string extension = Path.GetExtension(Postfile.FileName);
                        if (extension == ".xlsx")
                        {

                            dts = ReadExcelFile(Postfile);
                        }
                        else if (extension == ".xls")
                        {
                            Stream stream = Postfile.InputStream;
                            IExcelDataReader reader = null;
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            reader.IsFirstRowAsColumnNames = true;
                            DataSet result = reader.AsDataSet();
                            dts = result.Tables[0];
                            reader.Close();
                        }
                        else if (extension == ".csv")
                        {
                            string FileName = Path.GetFileName(Postfile.FileName);
                            FileName = "CRM-FBUploadLead-" + BranchID + "-" + CompanyID + "-" + Guid.NewGuid().ToString().Substring(0, 4) + "";
                            string _path = Server.MapPath("~/ExcelFileUpload/" + FileName + extension);
                            Postfile.SaveAs(_path);
                            dts = ProcessCSV(_path);
                        }
                        var confirm = await ExcelImportExport.FBImportLeads(dts, UID, BranchID, CompanyID);
                        if (confirm == "Facebook uploaded successfully")
                        {
                            Successmsg = confirm;
                        }
                        else
                        {
                            Successmsg = confirm;
                        }
                        //if (System.IO.File.Exists(path))
                        //    System.IO.File.Delete(path);
                        #endregion



                        #region New Upload not work in server
                        //string extension = Path.GetExtension(Postfile.FileName);
                        //string _path = Server.MapPath("~/ExcelFileUpload/" + Path.GetFileName(Postfile.FileName));
                        //Postfile.SaveAs(_path);
                        //DataTable DTExcel = new DataTable();
                        //ImportExcelToDataTable IEData = new ImportExcelToDataTable();
                        //DTExcel = IEData.XLStoDTusingInterOp(_path);
                        //var confirm = ExcelImportExport.FBImportLeads(DTExcel, UID, BranchID, CompanyID);
                        //if (confirm == "Facebook uploaded successfully")
                        //{
                        //    Successmsg = confirm;
                        //}
                        //else
                        //{
                        //    Successmsg = confirm;
                        //}
                        #endregion
                    }
                    else
                    {
                        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please try file extension like .xls, .xlsx,.csv only.";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                Errormsg = "** Sorry there is some technical problem. please try again.";
                //if (System.IO.File.Exists(path))
                //    System.IO.File.Delete(path);
            }
            string MsgReturn = string.Empty;
            if (!string.IsNullOrWhiteSpace(Errormsg))
            {
                MsgReturn = Errormsg;
            }
            else if (!string.IsNullOrWhiteSpace(Successmsg))
            {
                MsgReturn = Successmsg;
            }
            return Json(MsgReturn, JsonRequestBehavior.AllowGet);
        }

        private static DataTable ProcessCSV(string fileName)
        {

            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            StreamReader sr = new StreamReader(fileName);
            line = sr.ReadLine();
            strArray = r.Split(line);
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();
                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);
            }
            sr.Dispose();
            return dt;
        }

        public DataTable ReadExcelFile(HttpPostedFileBase Postfile)
        {
            //Create a new DataTable.
            DataTable dt = new DataTable();
            using (XLWorkbook workBook = new XLWorkbook(Postfile.InputStream))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);
                //Loop through the Worksheet rows.
                bool firstRow = true;

                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            if (!string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;

                        //iterate loop for datatable columns count
                        for (int j = 1; j <= dt.Columns.Count; j++)
                        {
                            object cellValue = row.Cell(j).Value;
                            if (string.IsNullOrEmpty(row.Cell(j).Value.ToString()))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = "";
                            }
                            else
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cellValue;

                                //if (cellValue is DateTime result)
                                //{
                                //    dt.Rows[dt.Rows.Count - 1][i] = cellValue;
                                //}
                                //else
                                //{
                                //    dt.Rows[dt.Rows.Count - 1][i] = cellValue; /*row.Cell(j).Value.ToString();*/
                                //}
                            }
                            i++;
                        }
                    }
                }
            }
            return dt;
        }

        private string GetValue(DocumentFormat.OpenXml.Packaging.SpreadsheetDocument doc, DocumentFormat.OpenXml.Spreadsheet.Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == DocumentFormat.OpenXml.Spreadsheet.CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        public static string ConvertXLS_XLSX(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            var app = new Microsoft.Office.Interop.Excel.Application();
            var xlsFile = file.FullName;
            var wb = app.Workbooks.Open(xlsFile);
            var xlsxFile = xlsFile + "x";
            wb.SaveAs(Filename: xlsxFile, FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
            wb.Close();
            app.Quit();
            return xlsxFile;
        }


        //Not In Used Now
        //public ActionResult FBUploadLeads(HttpPostedFileBase fbfile)
        //{
        //    try
        //    {
        //        if (Session["UserName"] != null)
        //        {
        //            if (fbfile != null)
        //            {
        //                int UID = Convert.ToInt32(Session["UID"]);
        //                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //                if (Request.Files["fbfile"].ContentLength > 0)
        //                {
        //                    string FileFullName = string.Empty;
        //                    var supportedTypes = new[] { "xls", "xlsx" };
        //                    var fileExt = System.IO.Path.GetExtension(fbfile.FileName).Substring(1);
        //                    if (supportedTypes.Contains(fileExt))
        //                    {
        //                        string FileName = Path.GetFileName(fbfile.FileName);
        //                        string extension = Path.GetExtension(fbfile.FileName);
        //                        FileName = "CRM-FBUploadLead-" + BranchID + "-" + CompanyID + "-" + Guid.NewGuid().ToString().Substring(0, 4) + "";
        //                        FileFullName = FileName + extension;
        //                        string _path = Server.MapPath("~/ExcelFileUpload/" + FileName + extension);
        //                        fbfile.SaveAs(_path);

        //                        var confirm = ExcelImportExport.CRMFB_Import_To_Grid(_path, extension, "Yes", UID, BranchID, CompanyID);
        //                        if (confirm == "Uploaded Successfully")
        //                        {
        //                            TempData["success"] = confirm;
        //                        }
        //                        else
        //                        {
        //                            TempData["alert"] = confirm;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .xls, .xlsx only";
        //                        return Redirect("/home/viewleads/?page=1");
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["alert"] = "There is some Problem!";
        //                }
        //            }
        //            else
        //            {
        //                TempData["alert"] = "** Please select file";
        //            }
        //            return Redirect("/home/viewleads/?page=1");
        //        }
        //        else
        //        {
        //            return Redirect("/home/login");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["alert"] = ex.Message.ToString();
        //        ExceptionLogging.SendExcepToDB(ex);
        //        return Redirect("/home/viewleads/?page=1");
        //    }
        //}
        #endregion

        #region Upload and Download Sample File
        public ActionResult DownloadFile(string filename)
        {
            string FullPath = Path.Combine(Server.MapPath("~/Sample/"), filename);
            return File(FullPath, "application/vnd.openxmlformats - officedocument.spreadsheetml.sheet", "LeadUpload.xlsx");
        }

        public async Task<ActionResult> CRMNormalUploadLead()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            int UID = Convert.ToInt32(Session["UID"]);
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
                if (Postfile.ContentLength > 0)
                {
                    string FileFullName = string.Empty;
                    var supportedTypes = new[] { "xls", "xlsx" };
                    var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
                    if (supportedTypes.Contains(fileExt))
                    {
                        DataTable dts = new DataTable();
                        string extension = Path.GetExtension(Postfile.FileName);
                        if (extension == ".xlsx")
                        {
                            dts = ReadExcelFile(Postfile);
                        }
                        else if (extension == ".xls")
                        {
                            Stream stream = Postfile.InputStream;
                            IExcelDataReader reader = null;
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            reader.IsFirstRowAsColumnNames = true;
                            DataSet result = reader.AsDataSet();
                            dts = result.Tables[0];
                            reader.Close();
                        }

                        var confirm = await ExcelImportExport.ImportLeads(dts, UID, BranchID, CompanyID);
                        if (confirm == "Upload Successfully")
                        {
                            Successmsg = confirm;
                        }
                        else
                        {
                            Successmsg = confirm;
                        }
                    }
                    else
                    {
                        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please try file extension like .xls, .xlsx only.";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                Errormsg = "** Something went wrong, please try again..";
            }
            string MsgReturn = string.Empty;
            if (!string.IsNullOrWhiteSpace(Errormsg))
            {
                MsgReturn = Errormsg;
            }
            else if (!string.IsNullOrWhiteSpace(Successmsg))
            {
                MsgReturn = Successmsg;
            }
            return Json(MsgReturn, JsonRequestBehavior.AllowGet);
        }

        //Not In Used Now
        //public ActionResult UploadLeads(HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        if (Session["UserName"] != null)
        //        {
        //            if (file != null)
        //            {
        //                int UID = Convert.ToInt32(Session["UID"]);
        //                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //                if (Request.Files["file"].ContentLength > 0)
        //                {
        //                    string FileName = Path.GetFileName(file.FileName);
        //                    FileName = "CRM-UploadLead-" + BranchID + "-" + CompanyID + "-" + Guid.NewGuid().ToString().Substring(0, 4) + "-" + FileName;
        //                    string Extension = Path.GetExtension(file.FileName);
        //                    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
        //                    string FilePath = Server.MapPath(FolderPath + FileName);
        //                    file.SaveAs(FilePath);
        //                    var confirm = ExcelImportExport.CRM_Import_To_Grid(FilePath, Extension, "Yes", UID, BranchID, CompanyID);
        //                    if (confirm == "Uploaded Successfully")
        //                    {
        //                        TempData["success"] = confirm;
        //                    }
        //                    else
        //                    {
        //                        TempData["alert"] = confirm;
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["alert"] = "There is some Problem!";
        //                }
        //            }
        //            else
        //            {
        //                TempData["alert"] = "** Please select file";
        //            }

        //            return Redirect("/home/viewleads/?page=1");
        //        }
        //        else
        //        {
        //            return Redirect("/home/login");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["alert"] = ex.Message.ToString();
        //        ExceptionLogging.SendExcepToDB(ex);
        //        return Redirect("/home/viewleads/?page=1");
        //    }
        //}
        #endregion

        #region IndiaMart Leads Get
        public async Task<String> GetIndiaMartLeadAsync()
        {
            String ErrorMessage = String.Empty;
            String SuccessMessage = String.Empty;
            String ReturnMessage = String.Empty;
            Root getLeadList = new Root();
            try
            {
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    DateTime utcTime = DateTime.UtcNow;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    var LeadOwner = await db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileName == "SuperAdmin").FirstOrDefaultAsync();
                    var getleadSourceStatus = await db.crm_leadsource_tbl.Where(em => em.LeadsourceName == "India Mart" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                    var getStatus = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Open" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                    var getproductType = await db.crm_producttypetbl.Where(em => em.ProductTypeName == "Billing" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                    var getSetting = await db.crm_indiamartsetting.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefaultAsync();
                    if (getSetting != null)
                    {
                        DateTime date1 = localTime;
                        DateTime date2 = localTime;
                        string StartDate = string.Format("{0:dd-MMM-yyyy}", date1.AddDays(-1));
                        string EndDate = string.Format("{0:dd-MMM-yyyy}", date2);



                        //string Baseurl = "https://mapi.indiamart.com/wservce/enquiry/listing/GLUSR_MOBILE/" + getSetting.MobileNumber + "/GLUSR_MOBILE_KEY/" + getSetting.IndiaMartCRMKey + "/Start_Time/" + StartDate + "/End_Time/" + EndDate + "/";
                        string Baseurl = "https://mapi.indiamart.com/wservce/crm/crmListing/v2/?glusr_crm_key=" + getSetting.IndiaMartCRMKey + "&start_time=" + StartDate + "&end_time=" + EndDate + "";

                        using (HttpClient client = new HttpClient())
                        {
                            //Root IMLeadList = new Root();
                            List<Root> IMLeadList = new List<Root>();

                            //Passing service base url  
                            client.BaseAddress = new Uri(Baseurl);

                            client.DefaultRequestHeaders.Clear();
                            //Define request data format  
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Baseurl);
                            //Sending request to find web api REST service resource GetAllLeads using HttpClient  
                            HttpResponseMessage responseMessage = await client.SendAsync(request);

                            //Checking the response is successful or not which is sent using HttpClient  
                            if (responseMessage.IsSuccessStatusCode == true)
                            {
                                //Storing the response details recieved from web api   
                                var IndiamartResponse = responseMessage.Content.ReadAsStringAsync().Result;
                                string[] FinalOutput = IndiamartResponse.Replace("[", "").Replace("]", "").Replace("}", "").Replace("{", "").Replace("Error_Message", "").Replace("\"", "").Replace("\n", "").Replace("    ", "").Split(':').ToArray();

                                string msg1 = " It is advised to hit this API once in every 15 minutes,but it seems that you have crossed this limit. please try again after 15 minutes.";
                                string msg2 = " There are no leads in the given time duration.please try for a different duration.";
                                string msg3 = " CRM key that you are using is expired as it was no longer in use. Kindly generate the new CRM key in API URL.";
                                if (FinalOutput[1] != msg1 || FinalOutput[1] != msg2 || FinalOutput[1] != msg3)
                                {
                                    //Deserializing the response recieved from web api and storing into the Employee list 
                                    Root rt1 = new Root();
                                    rt1 = JsonConvert.DeserializeObject<Root>(IndiamartResponse);
                                    IMLeadList.Add(rt1);
                                    //IMLeadList= JsonConvert.DeserializeObject<Root>(IndiamartResponse);
                                    // IMLeadList = IMLeadList.Where(a => !string.IsNullOrEmpty(a.SENDER_MOBILE)).ToList();

                                    if (IMLeadList.Count > 0)
                                    {
                                        foreach (var item in IMLeadList[0].RESPONSE)
                                        {
                                            //if (Convert.ToString(item.MOB).Contains("+91"))
                                            //{
                                            //    item.MOB = item.MOB.Replace("+91", "").Replace("-", "");
                                            //}

                                            if (Convert.ToString(item.SENDER_MOBILE).Length < 9)//check mobile no length is less then 9 then it add 0 
                                            {
                                                item.SENDER_MOBILE = "0" + item.SENDER_MOBILE;
                                            }
                                            string Mobno1 = item.SENDER_MOBILE.Substring(item.SENDER_MOBILE.Length - 9, 9);//get last line digits 

                                            var Getexists = await db.crm_createleadstbl.Where(em => (em.MobileNo.Substring(em.MobileNo.Length - 9, 9) == Mobno1 || (!string.IsNullOrEmpty(item.SENDER_EMAIL) && em.EmailId == item.SENDER_EMAIL)) && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                                            if (Getexists != null)
                                            {
                                                var getLDStatus = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Reinquiry" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();

                                                Getexists.LeadStatusID = getLDStatus != null ? getLDStatus.Id : getStatus != null ? getStatus.Id : Getexists.LeadStatusID;
                                                Getexists.LeadStatus = getLDStatus != null ? getLDStatus.LeadStatusName : getStatus != null ? getStatus.LeadStatusName : Getexists.LeadStatus;
                                                Getexists.ModifiedDate = localTime;
                                                Getexists.FollowDate = DateTime.ParseExact(localTime.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                Getexists.ZoneName = !string.IsNullOrEmpty(Getexists.ZoneName) && !Getexists.ZoneName.ToLower().Contains("select") ? Getexists.ZoneName : Constant.GetCompanyTimeZone(CompanyID);
                                                Getexists.date = localTime.ToString("dd/MM/yyyy");
                                                Getexists.Createddate = localTime;
                                                //Getexists.ConvertedFupDateTime = localTime;
                                                Getexists.Status = true;
                                                db.SaveChanges();
                                            }
                                            else
                                            {
                                                Int32? CRMCustomerID = 0;
                                                crm_createleadstbl ctl = new crm_createleadstbl();
                                                ctl.LeadOwner = LeadOwner.Id;
                                                ctl.Customer = string.IsNullOrEmpty(item.SENDER_NAME) ? "" : item.SENDER_NAME;
                                                ctl.EmailId = string.IsNullOrEmpty(item.SENDER_EMAIL) ? "" : item.SENDER_EMAIL;
                                                ctl.MobileNo = string.IsNullOrEmpty(item.SENDER_MOBILE) ? "" : item.SENDER_MOBILE.Trim();
                                                ctl.LeadStatusID = getStatus != null ? getStatus.Id : 0;
                                                ctl.LeadStatus = getStatus != null ? getStatus.LeadStatusName : "";
                                                ctl.LeadSourceID = getleadSourceStatus != null ? getleadSourceStatus.Id : 0;
                                                ctl.LeadResource = getleadSourceStatus != null ? getleadSourceStatus.LeadsourceName : "";
                                                ctl.ProductTypeID = getproductType != null ? getproductType.Id : 0;
                                                ctl.ProductTypeName = getproductType != null ? getproductType.ProductTypeName : "";
                                                ctl.FollowDate = DateTime.ParseExact(localTime.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                ctl.date = localTime.ToString("dd/MM/yyyy");
                                                ctl.Createddate = localTime;
                                                ctl.Status = true;
                                                //ctl.FollowUpTime = localTime.ToString("hh:mm tt"); //set followup time
                                                //ctl.ZoneName = Constant.GetCompanyTimeZone(CompanyID);
                                                //ctl.ConvertedFupDateTime = localTime;
                                                ctl.CompanyID = CompanyID;
                                                ctl.BranchID = BranchID;
                                                db.crm_createleadstbl.Add(ctl);

                                                if (db.SaveChanges() > 0)
                                                {
                                                    var lid = ctl.Id;
                                                    CRMCustomerID = Convert.ToInt32(ctl.Id);
                                                    crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                                                    LD.LeadId = lid;
                                                    LD.Date = localTime.ToString("dd/MM/yyyy");
                                                    LD.Description = item.QUERY_MESSAGE;
                                                    LD.ByUID = Convert.ToInt32(LeadOwner.Id);
                                                    LD.ByUserName = Convert.ToString(LeadOwner.UserName);
                                                    LD.CreatedDateTime = localTime;
                                                    LD.BranchID = BranchID;
                                                    LD.CompanyID = CompanyID;
                                                    db.crm_leaddescriptiontbl.Add(LD);
                                                    db.SaveChanges();
                                                }
                                                SuccessMessage = Convert.ToString(responseMessage.StatusCode);
                                            }
                                            //else
                                            //{
                                            //    ErrorMessage = "** Indiamart lead is already exists.";
                                            //}
                                        }
                                    }
                                    else
                                    {
                                        SuccessMessage = "** No Record Found **";
                                    }
                                }
                                else
                                {
                                    ErrorMessage = IndiamartResponse;
                                }
                            }
                        }
                    }
                    else
                    {
                        ReturnMessage = "Please configure setting of indiamart";
                    }
                }
                else
                {
                    ReturnMessage = "";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            if (ErrorMessage != string.Empty)
            {
                ReturnMessage = ErrorMessage;
            }
            else
            {
                ReturnMessage = "Lead add successfully.";
            }
            //await Task.Delay(900000);
            //Thread.Sleep(5000);
            return ReturnMessage;
        }
        #endregion


        #region View lead old code not in use 18_02_2021

        //public ActionResult viewleads(int? ProductTypeID, int? LeadSourceID, string FromDate, string ToDate, string UserddlName, string FilterType = "", string filterText = "", string Term = "", int page = 1)
        //{

        //    ViewLeadsModel VLM = new ViewLeadsModel();
        //    try
        //    {

        //        Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //        Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

        //        int LoggedBranchId = Convert.ToInt32(Session["BranchID"]);
        //        int LoggedCompanyId = Convert.ToInt32(Session["CompanyID"]);

        //        VLM.columnVal = cr.GetViewLeadsetting();

        //        # region after assigned lead to user redirect page  check sessions value not null 
        //        if (string.IsNullOrEmpty(UserddlName) && TempData["UserddlName"] != null)
        //        {
        //            UserddlName = Convert.ToString(TempData["UserddlName"]);
        //        }
        //        if (string.IsNullOrEmpty(filterText) && TempData["filterText"] != null)
        //        {
        //            filterText = Convert.ToString(TempData["filterText"]);
        //        }
        //        if (string.IsNullOrEmpty(FilterType) && TempData["VLFltrFilterType"] != null)
        //        {
        //            FilterType = Convert.ToString(TempData["VLFltrFilterType"]);
        //        }
        //        if (string.IsNullOrEmpty(Term) && TempData["VLFilterTerm"] != null)
        //        {
        //            Term = Convert.ToString(TempData["VLFilterTerm"]);
        //        }
        //        if (string.IsNullOrEmpty(FromDate) && TempData["VLFltrFrmDt"] != null)
        //        {
        //            FromDate = Convert.ToString(TempData["VLFltrFrmDt"]);
        //        }
        //        if (string.IsNullOrEmpty(ToDate) && TempData["VLFltrToDt"] != null)
        //        {
        //            ToDate = Convert.ToString(TempData["VLFltrToDt"]);
        //        }
        //        if (LeadSourceID != null && TempData["VLFltrLeadSourceID"] != null)
        //        {
        //            LeadSourceID = Convert.ToInt32(TempData["VLFltrLeadSourceID"]);
        //        }
        //        if (ProductTypeID != null && TempData["VLFltrProductTypeID"] != null)
        //        {
        //            ProductTypeID = Convert.ToInt32(TempData["VLFltrProductTypeID"]);
        //        }
        //        if (page != 1 && TempData["cpage"] != null)
        //        {
        //            page = Convert.ToInt32(TempData["cpage"]);
        //        }

        //        #endregion

        //        if (FilterType != "Select Date" && FilterType != "" && FilterType != null)
        //        {
        //            Session["VLFltrFilterType"] = FilterType;
        //        }
        //        else { Session["VLFltrFilterType"] = null; }

        //        if (filterText != "" && filterText != null)
        //        {
        //            Session["filterText"] = filterText;
        //        }
        //        else
        //        {
        //            Session["filterText"] = null;
        //        }

        //        var UID = 0;
        //        if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
        //        {
        //            UID = 0;
        //            if (UserddlName != null && UserddlName != "0" && UserddlName != string.Empty)
        //            {
        //                //string atest = "9586-202-10072";
        //                //int indexOfHyphen = UserddlName.LastIndexOf("-");
        //                //if (indexOfHyphen >= 0)
        //                //{
        //                //    UID = Convert.ToInt32(UserddlName.Substring(indexOfHyphen + 1));                      
        //                //}
        //                UID = Convert.ToInt32(UserddlName);
        //                VLM.UserddlName = Convert.ToString(UID);
        //            }
        //        }
        //        else
        //        {
        //            if (UserddlName != null && UserddlName != "0" && UserddlName != string.Empty)
        //            {
        //                UID = Convert.ToInt32(UserddlName);
        //            }
        //            else
        //            {
        //                UID = Convert.ToInt32(Session["UID"]);
        //            }
        //        }

        //        Session["UserddlName"] = VLM.UserddlName;

        //        if (!string.IsNullOrEmpty(UserddlName))
        //        {
        //            var data = cr.GetUserCompanyBranch(UID);
        //            if (data != null)
        //            {
        //                BranchID = data.BranchID;
        //                CompanyID = data.CompanyID;
        //            }
        //        }


        //        #region Email Template
        //        var getEmailTemplate = db.crm_emailtemplate.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId).ToList();
        //        if (getEmailTemplate != null && getEmailTemplate.Count > 0)
        //        {
        //            List<ViewLeadsModel> oETM = new List<ViewLeadsModel>();
        //            foreach (var item in getEmailTemplate)
        //            {
        //                ViewLeadsModel EMT = new ViewLeadsModel();
        //                EMT.EmailTemplateID = Convert.ToInt32(item.EmailTemplateID);
        //                EMT.EmailTemplateName = Convert.ToString(item.EmailTemplateName);
        //                oETM.Add(EMT);
        //            }
        //            VLM.oEmailTemplateModelList = oETM;
        //        }
        //        #endregion

        //        #region View Total Lead Count /Assign User List/ Lead Status List/File Manager List


        //        var AssignList = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName.ToLower().Contains("sales") && em.Status == true).OrderBy(em => em.Fname).ToList();
        //        if (AssignList != null)
        //        {
        //            List<CreateUserModel> assignToList = new List<CreateUserModel>();
        //            foreach (var item in AssignList)
        //            {
        //                CreateUserModel CRM = new CreateUserModel();
        //                CRM.UserID = item.Id;

        //                CRM.UserName = item.Fname + ' ' + item.Lname;
        //                assignToList.Add(CRM);
        //            }
        //            VLM.AssignToList = assignToList.Where(em => em.UserID != UID).ToList();
        //        }

        //        #region Get other branch user list to add assignTo list

        //        DataTable GetAssignToRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + LoggedBranchId + "','','')");
        //        var GetOtherBranchUser = (from dr in GetAssignToRecords.AsEnumerable()
        //                                  select new CreateUserModel()
        //                                  {
        //                                      UserID = Convert.ToInt32(dr["AssignedUserID"]),
        //                                      UserName = Convert.ToString(dr["UserName"] + " (Branch: " + dr["BranchName"] + ")"),
        //                                      IsActive = Convert.ToBoolean(dr["IsActive"])
        //                                  }).Where(a => a.IsActive == true).ToList();
        //        if (GetOtherBranchUser.Count > 0)
        //        {
        //            VLM.AssignToList.AddRange(GetOtherBranchUser);
        //        }
        //        #endregion

        //        var getleadStatus = db.crm_leadstatus_tbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.Status == true).ToList();
        //        if (getleadStatus != null)
        //        {
        //            List<LeadStatusModel> LSMList = new List<LeadStatusModel>();
        //            foreach (var item in getleadStatus)
        //            {
        //                LeadStatusModel LSM = new LeadStatusModel();
        //                LSM.Id = item.Id;
        //                LSM.LeadStatusname = item.LeadStatusName;
        //                LSMList.Add(LSM);
        //            }
        //            VLM.leadstatusList = LSMList;
        //        }

        //        #region Lead Source
        //        string leadSourceQuery = @"select Id,LeadsourceName from crm_leadsource_tbl Where Status=1 and BranchID='" + BranchID + "' and CompanyID='" + CompanyID + "'";
        //        var leadsourceList = db.Database.SqlQuery<LeadSourceModel>(leadSourceQuery).ToList();
        //        VLM.LeadSource = new SelectList(leadsourceList, "Id", "LeadsourceName", LeadSourceID);
        //        #endregion

        //        #region Product Type
        //        string pTypeQry = @" select Id,ProductTypeName from crm_producttypetbl Where Status=1 and BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'";
        //        var producttypetblList = db.Database.SqlQuery<ProductTypeModel>(pTypeQry).ToList();
        //        VLM.ProductType = new SelectList(producttypetblList, "Id", "ProductTypeName", ProductTypeID);

        //        #endregion
        //        Int32? UserID = Convert.ToInt32(Session["UID"]);
        //        var getFileManager = db.crm_filemanager.Where(em => em.CreatedBy == UserID && em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.IsDeleted == false).ToList();
        //        if (getFileManager.Count > 0)
        //        {
        //            List<FileManger> fmList = new List<FileManger>();
        //            foreach (var item in getFileManager)
        //            {
        //                FileManger fm = new FileManger();
        //                fm.FileID = item.FileID;
        //                fm.FileName = item.FileName;
        //                fm.FileUpload = item.FileUpload;
        //                fmList.Add(fm);
        //            }
        //            VLM.oFileMangerList = fmList;
        //        }
        //        #endregion

        //        #region Get all User and mapped users
        //        if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
        //        {
        //            #region Admin View All Users
        //            var q = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName == "Sales" && em.Status == true).OrderBy(em => em.Fname).ToList();
        //            VLM.Userddllist = new List<Userddl>();
        //            Userddl u = new Userddl();
        //            foreach (var item in q)
        //            {
        //                var user = new Userddl
        //                {
        //                    uid = item.Id,
        //                    UserName = item.Fname + " " + item.Lname
        //                };
        //                VLM.Userddllist.Add(user);
        //            }
        //            #endregion

        //            #region Get other branch user list to add mapped user list

        //            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + LoggedBranchId + "','','')");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                var GetOtherBranchUserList = (from dr in GetRecords.AsEnumerable()
        //                                              select new Userddl()
        //                                              {
        //                                                  uid = Convert.ToInt32(dr["AssignedUserID"]),
        //                                                  UserName = Convert.ToString(dr["UserName"] + " (Branch: " + dr["BranchName"] + ")"),
        //                                                  IsActive = Convert.ToBoolean(dr["IsActive"])
        //                                              }).Where(a => a.IsActive == true).ToList();
        //                VLM.Userddllist.AddRange(GetOtherBranchUserList);

        //            }

        //            #endregion
        //        }
        //        else
        //        {
        //            #region Employee will view only mapped user
        //            var GetUserData = db.crm_usertbl.Where(em => em.Id == UserID && em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId).FirstOrDefault();
        //            if (GetUserData != null && GetUserData.MappedUsers != null)
        //            {
        //                VLM.MappedUser = GetUserData.MappedUsers.ToString();
        //                var GetMapUser = GetUserData.MappedUsers.Split(',');
        //                VLM.Userddllist = new List<Userddl>();
        //                Userddl u1 = new Userddl();
        //                u1.uid = UID;
        //                u1.UserName = Session["UserName"].ToString();
        //                VLM.Userddllist.Add(u1);

        //                foreach (var item in GetMapUser)
        //                {
        //                    var mapid = Convert.ToInt32(item);
        //                    var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId).FirstOrDefault();
        //                    if (GetMapUserData != null)
        //                    {
        //                        var user = new Userddl
        //                        {
        //                            uid = mapid,
        //                            UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
        //                        };
        //                        VLM.Userddllist.Add(user);
        //                    }
        //                }

        //                //check other branch user mapped to current user
        //                string query = @"select muob.MappedUserId as uid,CONCAT(u.FName ,' ', u.Lname, '(Branch: ',IFNULL(cb.BranchName,'N / A'),')') as UserName,aorg.IsActive
        //                                 from CRM_MappedUserOtherBranch muob
        //                                 join crm_assignedtootherorganization aorg on muob.MappedUserId = aorg.AssignedUserID
        //                                 join crm_usertbl u on u.Id = aorg.AssignedUserID
        //                                 join com_branches cb on u.BranchID = cb.OrgBranchCode
        //                                 join company_profile cp on cb.OrganizationID = cp.id
        //                                 Where muob.UserId = '" + UserID + "' and aorg.AssignToCompanyID = '" + LoggedBranchId + "' and aorg.IsActive = 1 order by muob.CreateDate; ";
        //                var data = db.Database.SqlQuery<Userddl>(query).ToList();
        //                if (data.Count > 0)
        //                {
        //                    VLM.OtherBranchMappedUser = data;
        //                    VLM.Userddllist.AddRange(data);
        //                }

        //            }
        //            #endregion
        //        }
        //        #endregion


        //        #region Default Date show of one month

        //        DateTime bharatTime = Constant.GetBharatTime();//get india datetime
        //        var dd = bharatTime;
        //        DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
        //        DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
        //        var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
        //        var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
        //        FilterType = Convert.ToString(Session["VLFltrFilterType"]);
        //        filterText = Convert.ToString(Session["filterText"]);

        //        if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
        //        {
        //            Session["VLFltrFrmDt"] = FromDate;
        //            Session["VLFltrToDt"] = ToDate;
        //            MStartDate = FromDate;
        //            MEndDate = ToDate;
        //        }
        //        else
        //        {
        //            Session["VLFltrToDt"] = MEndDate;
        //            Session["VLFltrFrmDt"] = MStartDate;
        //        }
        //        #endregion



        //        if (!string.IsNullOrEmpty(FilterType) && !string.IsNullOrEmpty(filterText))
        //        {
        //            #region Modified-Date and filter-Text
        //            if (FilterType == "Modified Date")
        //            {
        //                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewModifiedLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
        //                if (GetRecords.Rows.Count > 0)
        //                {
        //                    List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                    for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                    {
        //                        ViewLeadsModel vlm = new ViewLeadsModel();
        //                        vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                        vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                        vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                        vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                        //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                        //{
        //                        //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                        //    {
        //                        //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                        //        if (reseller != null)
        //                        //        {
        //                        //            vlm.ResellerId = reseller.ResellerId;
        //                        //            vlm.ResellerName = reseller.ResellerName;
        //                        //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                        //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                        //            vlm.ResellerCode = reseller.ResellerCode;
        //                        //        }
        //                        //    }
        //                        //}

        //                        vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                        vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                        vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                        vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                        vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                        vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                        vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
        //                        vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
        //                        vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
        //                        vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                        vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                        vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                        vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                        vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                        vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                        vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                        vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                        vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                        vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                        vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                        vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                        vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                        vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                        vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                        vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                        vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                        vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                        vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                        vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                        vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                        vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                        vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                        vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                        vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                        vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                        vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                        vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                        vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                        vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                        vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                        vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                        vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                        vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                        vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                        vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                        vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                        vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                        vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                        vlmList.Add(vlm);
        //                    }
        //                    VLM.viewleadsList = vlmList.Where(em => em.LeadStatus == filterText).ToList();
        //                    VLM.AllviewleadsList = VLM.viewleadsList;

        //                    if (UID > 0)
        //                    {
        //                        VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                        VLM.AllviewleadsList = VLM.viewleadsList;
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region Assigned-Date or DOB or MarriageAnniversory or ExpectedClosing-date
        //            else if (FilterType == "AssignDate" || FilterType == "DOB" || FilterType == "MarriageAnniversary" || FilterType == "ExpectedDate" || FilterType == "ExtCol9Date" || FilterType == "ExtCol10Date" || FilterType == "ExtCol18Date" || FilterType == "ExtCol19Date" || FilterType == "ExtCol20Date")
        //            {

        //                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewLeadByAssignedDate_DOB_MrgAnnivers(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ",'" + FilterType + "' )");
        //                if (GetRecords.Rows.Count > 0)
        //                {
        //                    List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                    for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                    {
        //                        ViewLeadsModel vlm = new ViewLeadsModel();
        //                        vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                        vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                        vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                        vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                        //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                        //{
        //                        //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                        //    {
        //                        //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                        //        if (reseller != null)
        //                        //        {
        //                        //            vlm.ResellerId = reseller.ResellerId;
        //                        //            vlm.ResellerName = reseller.ResellerName;
        //                        //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                        //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                        //            vlm.ResellerCode = reseller.ResellerCode;
        //                        //        }
        //                        //    }
        //                        //}

        //                        vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                        vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                        vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                        vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                        vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                        vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                        vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
        //                        vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
        //                        vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
        //                        vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                        vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                        vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                        vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                        vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                        vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                        vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                        vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                        vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                        vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                        vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                        vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                        vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                        vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                        vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                        vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                        vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                        vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                        vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                        vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                        vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                        vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                        vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                        vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                        vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                        vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                        vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                        vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                        vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                        vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                        vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                        vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                        vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                        vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                        vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                        vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                        vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                        vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                        vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                        vlmList.Add(vlm);
        //                    }


        //                    VLM.viewleadsList = vlmList.Where(em => em.LeadStatus == filterText).ToList();
        //                    VLM.AllviewleadsList = VLM.viewleadsList;

        //                    if (UID > 0)
        //                    {
        //                        if (FilterType != "AssignDate")
        //                        {
        //                            VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                            VLM.AllviewleadsList = VLM.viewleadsList;
        //                        }
        //                    }
        //                }

        //            }
        //            #endregion

        //            #region Created-Date
        //            else if (FilterType == "Created Date")
        //            {
        //                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByCreateDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
        //                if (GetRecords.Rows.Count > 0)
        //                {
        //                    List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                    for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                    {
        //                        ViewLeadsModel vlm = new ViewLeadsModel();
        //                        vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                        vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                        vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                        vlm.EMail = GetRecords.Rows[i]["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                        //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                        //{
        //                        //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                        //    {
        //                        //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                        //        if (reseller != null)
        //                        //        {
        //                        //            vlm.ResellerId = reseller.ResellerId;
        //                        //            vlm.ResellerName = reseller.ResellerName;
        //                        //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                        //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                        //            vlm.ResellerCode = reseller.ResellerCode;
        //                        //        }
        //                        //    }
        //                        //}

        //                        vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                        vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                        vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                        vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                        vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                        vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                        vlm.Country = GetRecords.Rows[i]["Country"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["Country"]);
        //                        vlm.State = GetRecords.Rows[i]["State"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["State"]);
        //                        vlm.City = GetRecords.Rows[i]["City"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["City"]);
        //                        vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.FollowUpTime = GetRecords.Rows[i]["FollowUpTime"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                        vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                        vlm.LeadStatus = GetRecords.Rows[i]["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                        vlm.AssinedTo = GetRecords.Rows[i]["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                        vlm.AssignBy = GetRecords.Rows[i]["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.Address = GetRecords.Rows[i]["Address"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                        vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                        vlm.AssignTo = GetRecords.Rows[i]["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                        vlm.AssignedBy = GetRecords.Rows[i]["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.AssignDate = GetRecords.Rows[i]["AssignedDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.ModifiedDate = GetRecords.Rows[i]["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.LeadDate = Convert.ToDateTime(GetRecords.Rows[i]["date"]);
        //                        vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                        vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                        vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                        vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                        vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                        vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                        vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                        vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                        vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                        vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                        vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                        vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                        vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                        vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                        vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                        vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                        vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                        vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                        vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                        vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                        vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                        vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                        vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                        vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                        vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                        vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                        vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                        vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                        vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                        vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                        vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                        vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                        vlmList.Add(vlm);
        //                    }
        //                    //vlmList = vlmList.Where(em => em.Createddate >= Convert.ToDateTime(MStartDate) && em.Createddate <= Convert.ToDateTime(MEndDate)).ToList();

        //                    VLM.viewleadsList = vlmList.Where(em => em.LeadStatus == filterText).ToList();
        //                    VLM.AllviewleadsList = VLM.viewleadsList;
        //                    if (UID > 0)
        //                    {
        //                        VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                        VLM.AllviewleadsList = VLM.viewleadsList;
        //                    }

        //                }
        //            }
        //            #endregion

        //            #region Followup-Date
        //            else if (FilterType == "Followup Date")
        //            {
        //                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewFollowUpLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
        //                if (GetRecords.Rows.Count > 0)
        //                {
        //                    List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                    for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                    {
        //                        ViewLeadsModel vlm = new ViewLeadsModel();
        //                        vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                        vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                        vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                        vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                        //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                        //{
        //                        //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                        //    {
        //                        //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                        //        if (reseller != null)
        //                        //        {
        //                        //            vlm.ResellerId = reseller.ResellerId;
        //                        //            vlm.ResellerName = reseller.ResellerName;
        //                        //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                        //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                        //            vlm.ResellerCode = reseller.ResellerCode;
        //                        //        }
        //                        //    }
        //                        //}

        //                        vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                        vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                        vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                        vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                        vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                        vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                        vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
        //                        vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
        //                        vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
        //                        vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                        vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                        vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                        vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                        vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                        vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                        vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                        vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                        vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                        vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace("12:00:00 AM", "");
        //                        vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                        vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                        vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                        vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                        vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                        vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                        vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                        vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                        vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                        vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                        vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                        vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                        vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                        vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                        vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                        vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                        vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                        vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                        vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                        vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                        vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                        vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                        vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                        vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                        vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                        vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                        vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                        vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                        vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                        vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                        vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                        vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                        vlmList.Add(vlm);
        //                    }

        //                    VLM.viewleadsList = vlmList.Where(em => em.LeadStatus == filterText).ToList();
        //                    VLM.AllviewleadsList = VLM.viewleadsList;
        //                    if (UID > 0)
        //                    {
        //                        VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                        VLM.AllviewleadsList = VLM.viewleadsList;
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        else if (FilterType == "Modified Date" && string.IsNullOrEmpty(filterText))
        //        {
        //            #region Modified-Date
        //            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewModifiedLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    ViewLeadsModel vlm = new ViewLeadsModel();
        //                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                    //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                    //    {
        //                    //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                    //        if (reseller != null)
        //                    //        {
        //                    //            vlm.ResellerId = reseller.ResellerId;
        //                    //            vlm.ResellerName = reseller.ResellerName;
        //                    //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                    //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                    //            vlm.ResellerCode = reseller.ResellerCode;
        //                    //        }
        //                    //    }
        //                    //}

        //                    vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                    vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                    vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                    vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                    vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                    vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
        //                    vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
        //                    vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                    vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                    vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                    vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                    vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                    vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                    vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                    vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                    vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                    vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                    vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                    vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                    vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                    vlmList.Add(vlm);
        //                }
        //                VLM.viewleadsList = vlmList.Where(em => em.LeadStatus != "Not Interested").ToList();
        //                VLM.AllviewleadsList = VLM.viewleadsList;

        //                if (UID > 0)
        //                {
        //                    VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                    VLM.AllviewleadsList = VLM.viewleadsList;
        //                }
        //            }
        //            #endregion
        //        }
        //        else if ((FilterType == "AssignDate" || FilterType == "DOB" || FilterType == "MarriageAnniversary" || FilterType == "ExpectedDate" || FilterType == "ExtCol9Date" || FilterType == "ExtCol10Date" || FilterType == "ExtCol18Date" || FilterType == "ExtCol19Date" || FilterType == "ExtCol20Date") && string.IsNullOrEmpty(filterText))
        //        {
        //            #region Assigned-Date
        //            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewLeadByAssignedDate_DOB_MrgAnnivers(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ",'" + FilterType + "' )");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    ViewLeadsModel vlm = new ViewLeadsModel();
        //                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                    //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                    //    {
        //                    //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                    //        if (reseller != null)
        //                    //        {
        //                    //            vlm.ResellerId = reseller.ResellerId;
        //                    //            vlm.ResellerName = reseller.ResellerName;
        //                    //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                    //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                    //            vlm.ResellerCode = reseller.ResellerCode;
        //                    //        }
        //                    //    }
        //                    //}

        //                    vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                    vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                    vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                    vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                    vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                    vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
        //                    vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
        //                    vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                    vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                    vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                    vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                    vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                    vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                    vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                    vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                    vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                    vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                    vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                    vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                    vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                    vlmList.Add(vlm);
        //                }


        //                VLM.viewleadsList = vlmList.Where(em => em.LeadStatus != "Not Interested").ToList();
        //                VLM.AllviewleadsList = VLM.viewleadsList;

        //                if (UID > 0)
        //                {
        //                    if (FilterType != "AssignDate")
        //                    {
        //                        VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                        VLM.AllviewleadsList = VLM.viewleadsList;
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        else if (FilterType == "Created Date" && string.IsNullOrEmpty(filterText))
        //        {
        //            #region Created-Date

        //            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByCreateDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    ViewLeadsModel vlm = new ViewLeadsModel();
        //                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                    vlm.EMail = GetRecords.Rows[i]["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                    //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                    //    {
        //                    //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                    //        if (reseller != null)
        //                    //        {
        //                    //            vlm.ResellerId = reseller.ResellerId;
        //                    //            vlm.ResellerName = reseller.ResellerName;
        //                    //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                    //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                    //            vlm.ResellerCode = reseller.ResellerCode;
        //                    //        }
        //                    //    }
        //                    //}

        //                    vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                    vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                    vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                    vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                    vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                    vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                    vlm.Country = GetRecords.Rows[i]["Country"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["Country"]);
        //                    vlm.State = GetRecords.Rows[i]["State"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["State"]);
        //                    vlm.City = GetRecords.Rows[i]["City"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["City"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.FollowUpTime = GetRecords.Rows[i]["FollowUpTime"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatus = GetRecords.Rows[i]["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                    vlm.AssinedTo = GetRecords.Rows[i]["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                    vlm.AssignBy = GetRecords.Rows[i]["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = GetRecords.Rows[i]["Address"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    vlm.AssignTo = GetRecords.Rows[i]["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                    vlm.AssignedBy = GetRecords.Rows[i]["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = GetRecords.Rows[i]["AssignedDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ModifiedDate = GetRecords.Rows[i]["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.LeadDate = Convert.ToDateTime(GetRecords.Rows[i]["date"]);
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                    vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                    vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                    vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                    vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                    vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                    vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                    vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                    vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                    vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                    vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                    vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                    vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                    vlmList.Add(vlm);
        //                }
        //                //vlmList = vlmList.Where(em => em.Createddate >= Convert.ToDateTime(MStartDate) && em.Createddate <= Convert.ToDateTime(MEndDate)).ToList();

        //                VLM.viewleadsList = vlmList.Where(em => em.LeadStatus != "Not Interested").ToList();
        //                VLM.AllviewleadsList = VLM.viewleadsList;
        //                if (UID > 0)
        //                {
        //                    VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID) || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                    VLM.AllviewleadsList = VLM.viewleadsList;
        //                }

        //            }
        //            #endregion
        //        }
        //        else if (FilterType == "Followup Date" && string.IsNullOrEmpty(filterText))
        //        {
        //            #region Followup-Date 
        //            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewFollowUpLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    ViewLeadsModel vlm = new ViewLeadsModel();
        //                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                    //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                    //    {
        //                    //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                    //        if (reseller != null)
        //                    //        {
        //                    //            vlm.ResellerId = reseller.ResellerId;
        //                    //            vlm.ResellerName = reseller.ResellerName;
        //                    //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                    //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                    //            vlm.ResellerCode = reseller.ResellerCode;
        //                    //        }
        //                    //    }
        //                    //}

        //                    vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                    vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                    vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                    vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                    vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                    vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
        //                    vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
        //                    vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace("12:00:00 AM", "");
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                    vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                    vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                    vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                    vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                    vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                    vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                    vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                    vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                    vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                    vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                    vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                    vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                    vlmList.Add(vlm);
        //                }

        //                VLM.viewleadsList = vlmList.Where(em => em.LeadStatus != "Not Interested").ToList();
        //                VLM.AllviewleadsList = VLM.viewleadsList;
        //                if (UID > 0)
        //                {
        //                    VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID) || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                    VLM.AllviewleadsList = VLM.viewleadsList;
        //                }
        //            }
        //            #endregion
        //        }
        //        else if (!string.IsNullOrEmpty(filterText) && string.IsNullOrEmpty(FilterType))
        //        {

        //            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    ViewLeadsModel vlm = new ViewLeadsModel();
        //                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                    //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                    //    {
        //                    //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                    //        if (reseller != null)
        //                    //        {
        //                    //            vlm.ResellerId = reseller.ResellerId;
        //                    //            vlm.ResellerName = reseller.ResellerName;
        //                    //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                    //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                    //            vlm.ResellerCode = reseller.ResellerCode;
        //                    //        }
        //                    //    }
        //                    //}

        //                    vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                    vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                    vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                    vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                    vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                    vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["Country"]);
        //                    vlm.State = Convert.ToString(GetRecords.Rows[i]["State"]);
        //                    vlm.City = Convert.ToString(GetRecords.Rows[i]["City"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatus = GetRecords.Rows[i]["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                    vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
        //                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"])/*.Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "")*/;
        //                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"])/*.Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "")*/;
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "")/*.Replace(" 12:00:00 AM", "")*/;
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                    vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                    vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);
        //                    vlm.IsDOB = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
        //                    vlm.IsMA = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";
        //                    vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                    vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                    vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                    vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                    vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                    vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                    vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                    vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                    vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                    vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                    vlmList.Add(vlm);
        //                }
        //                VLM.viewleadsList = vlmList.Where(em => em.LeadStatus.Equals(filterText)).ToList();
        //                VLM.AllviewleadsList = VLM.viewleadsList;


        //                //if (UID > 0)
        //                //{
        //                //    VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID) || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                //    VLM.AllviewleadsList = VLM.viewleadsList;
        //                //}
        //            }

        //        }
        //        else
        //        {
        //            #region Default
        //            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");

        //            //DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewLeadsBySelectDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ",'" + FilterType + "' )");

        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    ViewLeadsModel vlm = new ViewLeadsModel();
        //                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
        //                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);

        //                    //if(CompanyID==296)//view reseller detail only for arunaw sir company
        //                    //{
        //                    //    if (!string.IsNullOrEmpty(vlm.EMail))
        //                    //    {
        //                    //        var reseller = cr.GetReseller(CompanyID, vlm.EMail);
        //                    //        if (reseller != null)
        //                    //        {
        //                    //            vlm.ResellerId = reseller.ResellerId;
        //                    //            vlm.ResellerName = reseller.ResellerName;
        //                    //            vlm.ResellerContactNo = reseller.ResellerContactNo;
        //                    //            vlm.ResellerStatus = reseller.ResellerStatus;
        //                    //            vlm.ResellerCode = reseller.ResellerCode;
        //                    //        }
        //                    //    }
        //                    //}

        //                    vlm.ResellerId = Convert.ToInt32(GetRecords.Rows[i]["ResellerId"]);
        //                    vlm.ResellerName = Convert.ToString(GetRecords.Rows[i]["ResellerName"]);
        //                    vlm.ResellerContactNo = Convert.ToString(GetRecords.Rows[i]["ResellerContactNo"]);
        //                    vlm.ResellerStatus = Convert.ToString(GetRecords.Rows[i]["ResellerStatus"]);
        //                    vlm.ResellerCode = Convert.ToString(GetRecords.Rows[i]["ResellerCode"]);
        //                    vlm.ResellerDocStatus = Convert.ToString(GetRecords.Rows[i]["ResellerDocStatus"]);

        //                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["Country"]);
        //                    vlm.State = Convert.ToString(GetRecords.Rows[i]["State"]);
        //                    vlm.City = Convert.ToString(GetRecords.Rows[i]["City"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.FollowUpTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatus = GetRecords.Rows[i]["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                    vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
        //                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
        //                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"])/*.Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "")*/;
        //                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"])/*.Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "")*/;
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "")/*.Replace(" 12:00:00 AM", "")*/;
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]));
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);

        //                    vlm.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
        //                    vlm.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);
        //                    vlm.IsDOB = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
        //                    vlm.IsMA = !string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";
        //                    vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
        //                    vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
        //                    vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
        //                    vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
        //                    vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
        //                    vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
        //                    vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
        //                    vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
        //                    vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
        //                    vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
        //                    vlmList.Add(vlm);
        //                }
        //                VLM.viewleadsList = vlmList.Where(em => em.LeadStatus != "Not Interested").ToList();
        //                VLM.AllviewleadsList = VLM.viewleadsList;

        //                //if (UID > 0)
        //                //{
        //                //    VLM.viewleadsList = VLM.viewleadsList.Where(em => (Convert.ToInt32(em.LeadOwner) == UID && em.AssignTo == "") || (em.AssignTo != "" && Int32.Parse(em.AssignTo) == UID)).ToList();
        //                //    VLM.AllviewleadsList = VLM.viewleadsList;
        //                //}


        //            }
        //            #endregion
        //        }

        //        #region filter by term and product type and lead source
        //        if (!string.IsNullOrEmpty(Term))
        //        {
        //            Term = Term.ToLower().Trim();
        //            VLM.viewleadsList = VLM.viewleadsList.Where(a => (a.LeadName.ToLower().Trim().Contains(Term)
        //            || (!string.IsNullOrEmpty(a.EMail) && a.EMail.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.Mob) && a.Mob == Term)
        //            || (!string.IsNullOrEmpty(a.URL) && a.URL.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.SkypeId) && a.SkypeId.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.Address) && a.Address.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.Designation) && a.Designation.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.OrganizationName) && a.OrganizationName.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.ExtraCol1) && a.ExtraCol1.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.ExtraCol2) && a.ExtraCol2.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.ExtraCol3) && a.ExtraCol3.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.ExtraCol4) && a.ExtraCol4.ToLower().Trim().Contains(Term))
        //            || (!string.IsNullOrEmpty(a.ExtraCol5) && a.ExtraCol5.ToLower().Trim().Contains(Term))
        //            )).ToList();
        //            Session["FilterTerm"] = Term;
        //        }
        //        else if (ProductTypeID != null)
        //        {
        //            VLM.viewleadsList = VLM.viewleadsList.Where(a => (a.ProductTypeID != null || a.ProductTypeID != 0) && a.ProductTypeID == ProductTypeID).ToList();
        //            Session["ProductTypeID"] = ProductTypeID;
        //        }
        //        else if (LeadSourceID != null)
        //        {
        //            VLM.viewleadsList = VLM.viewleadsList.Where(a => (a.LeadSourceID != null || a.LeadSourceID != 0) && a.LeadSourceID == LeadSourceID).ToList();
        //            Session["LeadSourceID"] = LeadSourceID;
        //        }
        //        #endregion

        //        if (!string.IsNullOrEmpty(VLM.UserddlName) && VLM.UserddlName != "0")
        //        {

        //            if ((VLM.MappedUser != String.Empty && VLM.MappedUser != null || (VLM.OtherBranchMappedUser != null && VLM.OtherBranchMappedUser.Count > 0)) && UserddlName == null && UserddlName == String.Empty)
        //            {
        //                List<Userddl> userlist = VLM.Userddllist;
        //                foreach (var item in userlist)
        //                {
        //                    List<ViewLeadsModel> VlieadList = VLM.AllviewleadsList.Where(em => (em.LeadOwner == Convert.ToString(item.uid) && (em.AssignTo == null || em.AssignTo == "" || em.AssignTo != Convert.ToString(item.uid))) || em.AssignTo == Convert.ToString(item.uid)).ToList();
        //                    VLM.viewleadsList.AddRange(VlieadList);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if ((VLM.MappedUser != String.Empty && VLM.MappedUser != null || (VLM.OtherBranchMappedUser != null && VLM.OtherBranchMappedUser.Count > 0)) && UserddlName == null && UserddlName == String.Empty)
        //            {
        //                List<Userddl> userlist = VLM.Userddllist;
        //                foreach (var item in userlist)
        //                {
        //                    List<ViewLeadsModel> VlieadList = VLM.AllviewleadsList.Where(em => (em.LeadOwner == Convert.ToString(item.uid) && (em.AssignTo == null || em.AssignTo == "" || em.AssignTo != Convert.ToString(item.uid))) || em.AssignTo == Convert.ToString(item.uid)).ToList();
        //                    VLM.viewleadsList.AddRange(VlieadList);
        //                }
        //            }
        //        }

        //        VLM.TotalLead = VLM.viewleadsList.Count();

        //        VLM.FromDate = FromDate;
        //        VLM.ToDate = ToDate;
        //        VLM.FilterType = FilterType;
        //        VLM.filterText = filterText;
        //        VLM.Term = Term;
        //        VLM.ProductTypeID = ProductTypeID;
        //        VLM.LeadSourceID = LeadSourceID;


        //        #region Pagging-Start
        //        int pageNumber = 1;
        //        int pageSize = 100;
        //        int pages = 0;
        //        pageNumber = page;
        //        //if (page == null)
        //        //{
        //        //    pageNumber = 1;
        //        //}
        //        //else
        //        //{
        //        //    pageNumber = Convert.ToInt32(page);
        //        //}
        //        int TotalProducts = 0;
        //        int Rem = 0;
        //        int pageSkip = (pageNumber - 1) * pageSize;

        //        TotalProducts = VLM.viewleadsList.Count();
        //        pages = (TotalProducts / pageSize);
        //        var Product = VLM.viewleadsList.Skip(pageSkip).Take(pageSize).ToList();
        //        VLM.viewleadsList = Product;
        //        pages = (TotalProducts / pageSize);
        //        Rem = (TotalProducts % pageSize);
        //        if (Rem < pageSize && Rem != 0)
        //        {
        //            pages = (pages + 1);
        //        }
        //        ViewData["NoOfPages"] = pages;

        //        if (page > 1)
        //        {
        //            var DeclareIndex = (pageSize * (page - 1)) + 1;
        //            ViewData["DeclareIndex"] = DeclareIndex;
        //        }
        //        else
        //        {
        //            ViewData["DeclareIndex"] = 1;
        //        }

        //        VLM.page = page;
        //        Session["cpage"] = page;
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //    }
        //    return View(VLM);
        //}


        public ActionResult oldviewleads(int? page, string FilterType, string filterText, string FromDate, string ToDate, string UserddlName)
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            try
            {
                if (FilterType != "Select Date")
                {
                    TempData["VLFltrFilterType"] = FilterType;
                }

                if (filterText != "Select Lead Status")
                {
                    TempData["filterText"] = filterText;
                }

                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                var UID = 0;
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    UID = 0;
                    if (UserddlName != null && UserddlName != "0" && UserddlName != string.Empty)
                    {
                        UID = Convert.ToInt32(UserddlName);
                    }
                }
                else
                {
                    if (UserddlName != null && UserddlName != "0" && UserddlName != string.Empty)
                    {
                        UID = Convert.ToInt32(UserddlName);
                    }
                    else
                    {
                        UID = Convert.ToInt32(Session["UID"]);
                    }
                }

                if (TempData["UserddlName"] == null)
                {
                    VLM.UserddlName = Convert.ToString(UID);
                    TempData["UserddlName"] = VLM.UserddlName;
                }
                else
                {
                    VLM.UserddlName = Convert.ToString(TempData["UserddlName"]);
                    TempData["UserddlName"] = VLM.UserddlName;
                }

                DataTable GetAllLeadCount = DataAccessLayer.GetDataTable("call CRM_GetLeadCount(" + CompanyID + "," + BranchID + "," + UID + ")");
                if (GetAllLeadCount.Rows.Count > 0)
                {
                    VLM.TotalLead = Convert.ToInt32(GetAllLeadCount.Rows[0]["TotalLead"]);
                }
                else
                {
                    VLM.TotalLead = 0;
                }

                var AssignList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(em => em.Fname).ToList();
                if (AssignList != null)
                {
                    List<CreateUserModel> assignToList = new List<CreateUserModel>();
                    foreach (var item in AssignList)
                    {
                        CreateUserModel CRM = new CreateUserModel();
                        CRM.UserID = item.Id;
                        CRM.UserName = item.Fname + ' ' + item.Lname;
                        assignToList.Add(CRM);
                    }
                    VLM.AssignToList = assignToList.Where(em => em.UserID != UID).ToList();
                }

                var getleadStatus = db.crm_leadstatus_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                if (getleadStatus != null)
                {
                    List<LeadStatusModel> LSMList = new List<LeadStatusModel>();
                    foreach (var item in getleadStatus)
                    {
                        LeadStatusModel LSM = new LeadStatusModel();
                        LSM.Id = item.Id;
                        LSM.LeadStatusname = item.LeadStatusName;
                        LSMList.Add(LSM);
                    }
                    VLM.leadstatusList = LSMList;
                }

                Int32? UserID = Convert.ToInt32(Session["UID"]);
                var getFileManager = db.crm_filemanager.Where(em => em.CreatedBy == UserID && em.CompanyID == CompanyID && em.BranchID == BranchID && em.IsDeleted == false).ToList();
                if (getFileManager.Count > 0)
                {
                    List<FileManger> fmList = new List<FileManger>();
                    foreach (var item in getFileManager)
                    {
                        FileManger fm = new FileManger();
                        fm.FileID = item.FileID;
                        fm.FileName = item.FileName;
                        fm.FileUpload = item.FileUpload;
                        fmList.Add(fm);
                    }
                    VLM.oFileMangerList = fmList;
                }

                if (Session["UserName"] != null)
                {
                    #region DateTime-Format
                    var dd = System.DateTime.Now;
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    FilterType = Convert.ToString(TempData["VLFltrFilterType"]);
                    filterText = Convert.ToString(TempData["filterText"]);
                    #endregion

                    #region To-CheckFilter-Date
                    if ((!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate)) || (TempData["VLFltrFrmDt"] != null && TempData["VLFltrToDt"] != null))
                    {
                        TempData["VLFltrFrmDt"] = FromDate;
                        TempData["VLFltrToDt"] = ToDate;
                        MStartDate = FromDate;
                        MEndDate = ToDate;
                    }
                    else
                    {
                        TempData["VLFltrToDt"] = MEndDate;
                        TempData["VLFltrFrmDt"] = MStartDate;
                    }
                    #endregion

                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        var q = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(em => em.Fname).ToList();
                        VLM.Userddllist = new List<Userddl>();
                        Userddl u = new Userddl();
                        foreach (var item in q)
                        {
                            var user = new Userddl
                            {
                                uid = item.Id,
                                UserName = item.Fname + " " + item.Lname
                            };
                            VLM.Userddllist.Add(user);
                        }

                        #region Super-Admin

                        if (FilterType == "Modified Date")
                        {
                            #region Modified-Date
                            DataTable P_ViewModifiedLeadbyDate = DataAccessLayer.GetDataTable(" call CRM_ViewModifiedLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (P_ViewModifiedLeadbyDate.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < P_ViewModifiedLeadbyDate.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(P_ViewModifiedLeadbyDate.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["CountryName"]);
                                    vlm.State = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["StateName"]);
                                    vlm.City = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["CityName"]);
                                    vlm.FollowupDate = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Fname"]) + " " + Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Address"]);
                                    vlm.LeadOwner = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;

                            }
                            #endregion
                        }
                        else if (FilterType == "Created Date")
                        {
                            #region Created-Date
                            DataTable T_GetViewLeadsByDate = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (T_GetViewLeadsByDate.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < T_GetViewLeadsByDate.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(T_GetViewLeadsByDate.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Country"]);
                                    vlm.State = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["State"]);
                                    vlm.City = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["City"]);
                                    vlm.FollowupDate = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Fname"]) + " " + Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Address"]);
                                    vlm.LeadOwner = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;
                            }
                            #endregion
                        }
                        else if (FilterType == "Followup Date")
                        {
                            #region Followup-Date
                            DataTable P_ViewFollowUpLeadbyDate = DataAccessLayer.GetDataTable(" call CRM_ViewFollowUpLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (P_ViewFollowUpLeadbyDate.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < P_ViewFollowUpLeadbyDate.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(P_ViewFollowUpLeadbyDate.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["CountryName"]);
                                    vlm.State = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["StateName"]);
                                    vlm.City = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["CityName"]);
                                    vlm.FollowupDate = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Fname"]) + " " + Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Address"]);
                                    //vlm.LeadOwner = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace("12:00:00 AM", "");
                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Default
                            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (GetRecords.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < GetRecords.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["Country"]);
                                    vlm.State = Convert.ToString(GetRecords.Rows[i]["State"]);
                                    vlm.City = Convert.ToString(GetRecords.Rows[i]["City"]);
                                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
                                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    if (!string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsDOB"])))
                                    {
                                        vlm.IsDOB = Convert.ToString(GetRecords.Rows[i]["IsDOB"]);
                                    }
                                    else
                                    {
                                        vlm.IsDOB = "0";
                                    }

                                    if (!string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsMA"])))
                                    {
                                        vlm.IsMA = Convert.ToString(GetRecords.Rows[i]["IsMA"]);
                                    }
                                    else
                                    {
                                        vlm.IsMA = "0";
                                    }

                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;
                            }
                            #endregion
                        }

                        if (!string.IsNullOrEmpty(VLM.UserddlName) && VLM.UserddlName != "0")
                        {
                            var FilterData = VLM.viewleadsList.Where(em => (em.LeadOwner == VLM.UserddlName && (em.AssignTo == null || em.AssignTo != VLM.UserddlName)) || em.AssignTo == VLM.UserddlName).ToList();
                            VLM.viewleadsList = FilterData;
                        }
                        if (filterText != null && filterText != "" && filterText != "Select Lead Status")
                        {
                            var FilterData = VLM.viewleadsList.Where(em => em.LeadStatus.Contains(filterText)).ToList();
                            VLM.viewleadsList = FilterData;
                        }

                        if (filterText != "Not Interested")
                        {
                            var FilterData = VLM.viewleadsList.Where(em => em.LeadStatus != "Not Interested").ToList();
                            VLM.viewleadsList = FilterData;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Users
                        #region Get-MappedUser-Parents
                        var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetUserData != null && GetUserData.MappedUsers != null)
                        {
                            VLM.MappedUser = GetUserData.MappedUsers.ToString();
                            var GetMapUser = GetUserData.MappedUsers.Split(',');
                            VLM.Userddllist = new List<Userddl>();
                            Userddl u1 = new Userddl();
                            u1.uid = UID;
                            u1.UserName = Session["UserName"].ToString();
                            VLM.Userddllist.Add(u1);

                            foreach (var item in GetMapUser)
                            {
                                var mapid = Convert.ToInt32(item);
                                var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                if (GetMapUserData != null)
                                {
                                    var user = new Userddl
                                    {
                                        uid = mapid,
                                        UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
                                    };
                                    VLM.Userddllist.Add(user);
                                }
                            }
                        }
                        #endregion

                        if (FilterType == "Modified Date")
                        {
                            #region Modified-Date
                            DataTable P_ViewModifiedLeadbyDate = DataAccessLayer.GetDataTable(" call CRM_ViewModifiedLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (P_ViewModifiedLeadbyDate.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < P_ViewModifiedLeadbyDate.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(P_ViewModifiedLeadbyDate.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["CountryName"]);
                                    vlm.State = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["StateName"]);
                                    vlm.City = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["CityName"]);
                                    vlm.FollowupDate = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Fname"]) + " " + Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["Address"]);
                                    vlm.LeadOwner = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(P_ViewModifiedLeadbyDate.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;

                            }
                            #endregion
                        }
                        else if (FilterType == "Created Date")
                        {
                            #region Created-Date
                            DataTable T_GetViewLeadsByDate = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (T_GetViewLeadsByDate.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < T_GetViewLeadsByDate.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(T_GetViewLeadsByDate.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Country"]);
                                    vlm.State = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["State"]);
                                    vlm.City = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["City"]);
                                    vlm.FollowupDate = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Fname"]) + " " + Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["Address"]);
                                    vlm.LeadOwner = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(T_GetViewLeadsByDate.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;
                            }
                            #endregion
                        }
                        else if (FilterType == "Followup Date")
                        {
                            #region Followup-Date
                            DataTable P_ViewFollowUpLeadbyDate = DataAccessLayer.GetDataTable(" call CRM_ViewFollowUpLeadbyDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (P_ViewFollowUpLeadbyDate.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < P_ViewFollowUpLeadbyDate.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(P_ViewFollowUpLeadbyDate.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["CountryName"]);
                                    vlm.State = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["StateName"]);
                                    vlm.City = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["CityName"]);
                                    vlm.FollowupDate = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Fname"]) + " " + Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["Address"]);
                                    //vlm.LeadOwner = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(P_ViewFollowUpLeadbyDate.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Default Load First
                            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                            if (GetRecords.Rows.Count > 0)
                            {
                                List<ViewLeadsModel> vlmList = new List<ViewLeadsModel>();
                                for (int i = 0; i < GetRecords.Rows.Count; i++)
                                {
                                    ViewLeadsModel vlm = new ViewLeadsModel();
                                    vlm.Id = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
                                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);
                                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
                                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
                                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["Country"]);
                                    vlm.State = Convert.ToString(GetRecords.Rows[i]["State"]);
                                    vlm.City = Convert.ToString(GetRecords.Rows[i]["City"]);
                                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
                                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
                                    vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
                                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
                                    //vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
                                    vlm.AssignTo = Convert.ToString(GetRecords.Rows[i]["AssignTo"]);
                                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                                    if (!string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsDOB"])))
                                    {
                                        vlm.IsDOB = Convert.ToString(GetRecords.Rows[i]["IsDOB"]);
                                    }
                                    else
                                    {
                                        vlm.IsDOB = "0";
                                    }

                                    if (!string.IsNullOrWhiteSpace(Convert.ToString(GetRecords.Rows[i]["IsMA"])))
                                    {
                                        vlm.IsMA = Convert.ToString(GetRecords.Rows[i]["IsMA"]);
                                    }
                                    else
                                    {
                                        vlm.IsMA = "0";
                                    }
                                    vlmList.Add(vlm);
                                }
                                VLM.viewleadsList = vlmList;
                            }
                            #endregion
                        }

                        if (!string.IsNullOrEmpty(VLM.UserddlName) && VLM.UserddlName != "0")
                        {
                            var FilterData = VLM.viewleadsList.Where(em => (em.LeadOwner == VLM.UserddlName && (em.AssignTo == null || em.AssignTo == string.Empty || em.AssignTo != VLM.UserddlName)) || em.AssignTo == VLM.UserddlName).ToList();
                            VLM.viewleadsList = FilterData;
                        }

                        if (filterText != null && filterText != "" && filterText != "Select Lead Status")
                        {
                            var FilterData = VLM.viewleadsList.Where(em => em.LeadStatus.Contains(filterText)).ToList();
                            VLM.viewleadsList = FilterData;
                        }

                        if (filterText != "Not Interested")
                        {
                            var FilterData = VLM.viewleadsList.Where(em => em.LeadStatus != "Not Interested").ToList();
                            VLM.viewleadsList = FilterData;
                        }
                        #endregion
                    }

                    #region Pagging-Start
                    int pageNumber = 1;
                    int pageSize = 100;
                    int pages = 0;
                    if (page == null)
                    {
                        pageNumber = 1;
                    }
                    else
                    {
                        pageNumber = Convert.ToInt32(page);
                    }
                    int TotalProducts = 0;
                    int Rem = 0;
                    int pageSkip = (pageNumber - 1) * pageSize;

                    TotalProducts = VLM.viewleadsList.Count();
                    pages = (TotalProducts / pageSize);
                    var Product = VLM.viewleadsList.Skip(pageSkip).Take(pageSize).ToList();
                    VLM.viewleadsList = Product;
                    pages = (TotalProducts / pageSize);
                    Rem = (TotalProducts % pageSize);
                    if (Rem < pageSize && Rem != 0)
                    {
                        pages = (pages + 1);
                    }
                    ViewData["NoOfPages"] = pages;

                    //For Page Index Count.......
                    if (page > 1)
                    {
                        var DeclareIndex = (pageSize * (page - 1)) + 1;
                        ViewData["DeclareIndex"] = DeclareIndex;
                    }
                    else
                    {
                        ViewData["DeclareIndex"] = 1;
                    }
                    #endregion
                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(VLM);
        }

        #endregion
        public ActionResult UserWorkDetails(string FromDate, string ToDate)
        {
            ViewSalesModel uwd = new ViewSalesModel();
            try
            {

                if (Session["UserName"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    int UserId = Convert.ToInt32(Session["UID"]);
                    var dd = System.DateTime.Now;
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    DateTime Leadfromdateformat1 = DateTime.Parse(MStartDate).Date;
                    MStartDate = Leadfromdateformat1.ToString("dd/MM/yyyy");
                    DateTime Leadtodateformat1 = DateTime.Parse(MEndDate).Date;
                    MEndDate = Leadtodateformat1.ToString("dd/MM/yyyy");
                    var MStartDate1 = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate1 = MonthendDate.ToString("dd/MM/yyyy");
                    if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
                    {
                        if (uwd.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            Session["VLFltrFrmDt"] = FromDate;
                            Session["VLFltrToDt"] = ToDate;

                            //var fmDate = DateTime.ParseExact(FromDate, uwd.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                            //var tDate = DateTime.ParseExact(ToDate, uwd.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                            // MStartDate = String.Format("{0:dd/MM/yyyy}", FromDate);//convert to dd/MM/yyyy format for stored procedure
                            //    MEndDate = String.Format("{0:dd/MM/yyyy}", ToDate);//convert to dd/MM/yyyy format for stored procedure
                            DateTime Leadfromdateformat = DateTime.Parse(FromDate).Date;
                            MStartDate = Leadfromdateformat.ToString("dd/MM/yyyy");
                            DateTime Leadtodateformat = DateTime.Parse(ToDate).Date;
                            MEndDate = Leadtodateformat.ToString("dd/MM/yyyy");
                            MStartDate1 = FromDate;
                            MEndDate1 = ToDate;
                        }
                        else
                        {
                            Session["VLFltrFrmDt"] = FromDate;
                            Session["VLFltrToDt"] = ToDate;
                            //  MStartDate = FromDate;
                            //   MEndDate = ToDate;
                            DateTime Leadfromdateformat = DateTime.Parse(FromDate).Date;
                            MStartDate = Leadfromdateformat.ToString("dd/MM/yyyy");
                            DateTime Leadtodateformat = DateTime.Parse(ToDate).Date;
                            MEndDate = Leadtodateformat.ToString("dd/MM/yyyy");
                            MStartDate1 = FromDate;
                            MEndDate1 = ToDate;
                        }

                    }
                    else
                    {
                        if (uwd.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            Session["VLFltrFrmDt"] = monthStartDate.ToString("dd/MM/yyyy");
                            Session["VLFltrToDt"] = MonthendDate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            Session["VLFltrFrmDt"] = MStartDate;
                            Session["VLFltrToDt"] = MEndDate;

                        }
                    }


                    DataTable P_Viewuserworkdetails = DataAccessLayer.GetDataTable(" call CRM_UserWorkDetails(" + UserId + ")");
                    if (P_Viewuserworkdetails.Rows.Count > 0)
                    {
                        List<userworkdetails> vlmList = new List<userworkdetails>();

                        for (int i = 0; i < P_Viewuserworkdetails.Rows.Count; i++)
                        {
                            var id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
                            //var userrecord = db.crm_usertbl.Where(x => x.Id == id).FirstOrDefault();
                            //var UID = userrecord.Id;
                            //var BranchID = userrecord.BranchID;
                            //var CompanyID = userrecord.CompanyID;
                            var filterText = "";

                            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewLeadByModifiedDateWithFilter(" + id + ",'" + MStartDate1 + "','" + MEndDate1 + "','" + BranchID + "','" + CompanyID + "','" + filterText + "')");

                            //var UserddlName = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
                            userworkdetails vlm = new userworkdetails();
                            vlm.Id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
                            vlm.ByUID = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["ByUID"]);
                            // vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["UserName"]);
                            vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Fname"]) + Convert.ToString(" ") + Convert.ToString(P_Viewuserworkdetails.Rows[i]["Lname"]);
                            //  vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[0]["ModifiedDate"]);
                            vlm.ModifiedDate = Convert.ToString(GetRecords.Rows.Count);
                            // vlm.ModifiedDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ModifiedDate"]);
                            //vlm.LeadStatus = Convert.ToString(P_Viewuserworkdetails.Rows[i]["LeadStatus"]);
                            //vlm.PastPerformance = Convert.ToString(P_Viewuserworkdetails.Rows[i]["PastPerformance"]);
                            vlmList.Add(vlm);
                        }
                        uwd.userworklist = vlmList;
                    }


                    //DataTable P_closeduserworkdetails = DataAccessLayer.GetDataTable(" call CRM_UserWorkDetails(" + UserId + ",'" + MStartDate + "','" + MEndDate + "')");
                    //if (P_closeduserworkdetails.Rows.Count > 0)
                    //{
                    //    List<userworkdetails> vlmList = new List<userworkdetails>();
                    //    for (int i = 0; i < P_closeduserworkdetails.Rows.Count; i++)
                    //    {
                    //        var xid = Convert.ToInt32(P_closeduserworkdetails.Rows[i]["Id"]);
                    //        DataTable P_closeduserworkdetails1 = DataAccessLayer.GetDataTable(" call Userclosedlead(" + xid + ",'" + MStartDate + "','" + MEndDate + "')");
                    //        userworkdetails vlm = new userworkdetails();

                    //        vlm.Id = Convert.ToInt32(P_closeduserworkdetails.Rows[i]["Id"]);
                    //        vlm.userName = Convert.ToString(P_closeduserworkdetails.Rows[i]["UserName"]);
                    //        vlm.LeadStatus = "Closed";
                    //        if (P_closeduserworkdetails1.Rows.Count > 0)
                    //        {
                    //            vlm.closedlead = Convert.ToString(P_closeduserworkdetails1.Rows.Count); ;

                    //        }
                    //        else
                    //        {
                    //            vlm.closedlead = "0";

                    //        }
                    //        vlmList.Add(vlm);
                    //    }
                    //    uwd.userworkclosedlist = vlmList;

                    //}



                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(uwd);
        }

        public ActionResult Customerworkdetails(string UserddlName, string LeadStatus, string Leadfromdate, string Leadtodate)
        {
            ViewSalesModel VSM = new ViewSalesModel();
            try
            {

                if (Session["UserName"] != null)
                {
                    int UserId = Convert.ToInt32(Session["UID"]);
                    #region DateTime-Format
                    var dd = System.DateTime.Now;
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    DateTime Leadfromdateformat = DateTime.Parse(Leadfromdate).Date;
                    var Leadfromdatedatapick = Leadfromdateformat.ToString("yyyy-MM-dd");
                    DateTime Leadtodateformat = DateTime.Parse(Leadtodate).Date;
                    var Leadtodateformatdatapick = Leadtodateformat.ToString("yyyy-MM-dd");
                    #endregion


                    if (LeadStatus != "Closed")
                    {
                        var id1 = int.Parse(UserddlName);
                        var userrecord = db.crm_usertbl.Where(x => x.Id == id1).FirstOrDefault();
                        var UID = userrecord.Id;
                        var BranchID = userrecord.BranchID;
                        var CompanyID = userrecord.CompanyID;
                        var filterText = "";
                        DataTable P_Viewuserworkdetails = DataAccessLayer.GetDataTable(" call CRM_ViewLeadByModifiedDateWithFilter(" + UID + ",'" + Leadfromdate + "','" + Leadtodate + "','" + BranchID + "','" + CompanyID + "','" + filterText + "')");

                        // DataTable P_Viewuserworkdetails = DataAccessLayer.GetDataTable(" call CRM_CustomerWorkDetails(" + UserddlName + ", '" + Leadfromdatedatapick + "','" + Leadtodateformatdatapick + "')");
                        if (P_Viewuserworkdetails.Rows.Count > 0)
                        {
                            List<userworkdetails> vlmList = new List<userworkdetails>();
                            for (int i = 0; i < P_Viewuserworkdetails.Rows.Count; i++)
                            {
                                userworkdetails vlm = new userworkdetails();

                                vlm.Id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
                                vlm.ByUID = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["ByUID"]);
                                vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["AssignedTo"]);
                                vlm.ModifiedDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ModifiedDate"]);
                                vlm.LeadStatus = Convert.ToString(P_Viewuserworkdetails.Rows[i]["LeadStatus"]);
                                vlm.CustomerName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Customer"]);
                                vlm.FollowDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["FollowDate"]);
                                vlm.ProfitLoss = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ProfitLoss"]);
                                vlm.BuySell = Convert.ToString(P_Viewuserworkdetails.Rows[i]["BuySell"]);
                                vlm.StockName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["StockName"]);
                                vlm.Price = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Price"]);
                                vlm.Target = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target"]);
                                vlm.Target2 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target2"]);
                                vlm.Target3 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target3"]);
                                vlm.SI = Convert.ToString(P_Viewuserworkdetails.Rows[i]["SI"]);
                                vlm.Remark = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Remark"]);
                                vlmList.Add(vlm);
                            }
                            VSM.userworklist = vlmList;

                        }
                    }
                    else
                    {
                        DataTable P_Viewuserworkdetails = DataAccessLayer.GetDataTable(" call showUserclosedleadData(" + UserddlName + ",'" + Leadfromdate + "','" + Leadtodate + "')");
                        if (P_Viewuserworkdetails.Rows.Count > 0)
                        {
                            List<userworkdetails> vlmList = new List<userworkdetails>();
                            for (int i = 0; i < P_Viewuserworkdetails.Rows.Count; i++)
                            {
                                userworkdetails vlm = new userworkdetails();

                                vlm.Id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
                                vlm.ByUID = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["ByUID"]);
                                vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["AssignedTo"]);
                                vlm.ModifiedDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ModifiedDate"]);
                                vlm.LeadStatus = Convert.ToString(P_Viewuserworkdetails.Rows[i]["LeadStatus"]);
                                vlm.CustomerName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Customer"]);
                                vlm.FollowDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["FollowDate"]);
                                vlm.ProfitLoss = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ProfitLoss"]);
                                vlm.BuySell = Convert.ToString(P_Viewuserworkdetails.Rows[i]["BuySell"]);
                                vlm.StockName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["StockName"]);
                                vlm.Price = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Price"]);
                                vlm.Target = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target"]);
                                vlm.Target2 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target2"]);
                                vlm.Target3 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target3"]);
                                vlm.SI = Convert.ToString(P_Viewuserworkdetails.Rows[i]["SI"]);
                                vlm.Remark = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Remark"]);
                                vlmList.Add(vlm);
                            }
                            VSM.userworkclosedlist = vlmList;

                        }
                    }

                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View("_CustomerWorkLeadReport", VSM);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Deleteleadsrecord(string id)
        {
            string Successmsg = string.Empty;
            string msg = string.Empty;
            string OTP = string.Empty;
            string Data = "";
            try
            {

                ViewLeadsModel VLM = new ViewLeadsModel();
                int LoggedBranchID = Convert.ToInt32(Session["BranchID"]);
                int LoggedCompanyID = Convert.ToInt32(Session["CompanyID"]);
                string deletelist = string.Empty;

                var data = new List<ViewLeadsModel>();
                //string Value = "";
                Random generate = new Random();
                int r = generate.Next(1, 1000000);
                OTP = r.ToString().PadLeft(6, '0');

                Session["OTP"] = OTP;
                Session.Timeout = 10;

                Data = OTP;
                String ToEmailAddress = Convert.ToString(Session["UserEmail"]);
                String Subject = "Smart Capita CRM OTP";
                String MessageBody = "Please Use this OTP '" + OTP + "' to Delete Smart Capita CRM Leads <br> It is valid only for 10 Minutes";

                bool SendNow = EmailUtility.DeleteleadSendEmailOTP(ToEmailAddress, Subject, MessageBody, LoggedCompanyID, LoggedBranchID);

                if (SendNow)
                {
                    msg = "ok";
                }
                else
                {
                    msg = "Something went wrong! Please try after some time";
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "error";
            }
            return Json(Data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubDeleteleadsrecord(string id, string otp)
        {
            string msg = string.Empty;
            int LoggedBranchID = Convert.ToInt32(Session["BranchID"]);
            int LoggedCompanyID = Convert.ToInt32(Session["CompanyID"]);
            string deletelist = string.Empty;
            var sOTP = Convert.ToString(Session["OTP"]);
            if (!string.IsNullOrEmpty(id))
            {
                deletelist = id;
                if (deletelist != string.Empty)
                {
                    var SplitedeleteLeads = deletelist.Split('|');
                    if (SplitedeleteLeads[0] == "on")
                    {
                        SplitedeleteLeads = SplitedeleteLeads.Where((item, index) => index != 0).ToArray();
                    }
                    foreach (var item in SplitedeleteLeads)
                    {
                        var SpliteItem = item.Split('/');
                        string Value = "";
                        Value = Convert.ToString(SpliteItem[0]);
                        var isNumber = Value.All(char.IsNumber);//to check all charcter is numeric
                        if (isNumber)
                        {
                            if (sOTP == otp)
                            {
                                DataTable getlead = DataAccessLayer.GetDataTable(" call CRM_DeleteRecord(" + LoggedCompanyID + "," + LoggedBranchID + "," + Value + ")");
                            }
                        }
                        //if (getlead.Rows.Count > 0)
                        //{
                        //    msg = "Delete Leads Successfully";
                        //}
                        msg = "Delete Leads Successfully";
                    }
                }
                else
                {
                    msg = "OTP is expired try again";
                }

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult UserWorkDetails(string FromDate, string ToDate)
        //{
        //    ViewSalesModel uwd = new ViewSalesModel();
        //    try
        //    {

        //        if (Session["UserName"] != null)
        //        {
        //            int UserId = Convert.ToInt32(Session["UID"]);                    
        //            var dd = System.DateTime.Now;
        //            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
        //            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
        //            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
        //            var MEndDate = MonthendDate.ToString("dd/MM/yyyy");                    

        //            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
        //            {
        //                if (uwd.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
        //                {
        //                    Session["VLFltrFrmDt"] = FromDate;
        //                    Session["VLFltrToDt"] = ToDate;

        //                    //var fmDate = DateTime.ParseExact(FromDate, uwd.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
        //                    //var tDate = DateTime.ParseExact(ToDate, uwd.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

        //                    MStartDate = String.Format("{0:dd/MM/yyyy}", FromDate);//convert to dd/MM/yyyy format for stored procedure
        //                    MEndDate = String.Format("{0:dd/MM/yyyy}", ToDate);//convert to dd/MM/yyyy format for stored procedure

        //                }
        //                else
        //                {
        //                    Session["VLFltrFrmDt"] = FromDate;
        //                    Session["VLFltrToDt"] = ToDate;
        //                    MStartDate = FromDate;
        //                    MEndDate = ToDate;
        //                }

        //            }
        //            else
        //            {
        //                if (uwd.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
        //                {
        //                    Session["VLFltrFrmDt"] = monthStartDate.ToString("dd/MM/yyyy");
        //                    Session["VLFltrToDt"] = MonthendDate.ToString("dd/MM/yyyy");
        //                }
        //                else
        //                {
        //                    Session["VLFltrFrmDt"] = MStartDate;
        //                    Session["VLFltrToDt"] = MEndDate;
        //                }
        //            }


        //            DataTable P_Viewuserworkdetails = DataAccessLayer.GetDataTable(" call CRM_UserWorkDetails(" + UserId + ",'"+ MStartDate + "','"+ MEndDate + "')");
        //            if (P_Viewuserworkdetails.Rows.Count > 0)
        //            {
        //                List<userworkdetails> vlmList = new List<userworkdetails>();
        //                for (int i = 0; i < P_Viewuserworkdetails.Rows.Count; i++)
        //                {
        //                    userworkdetails vlm = new userworkdetails();

        //                    vlm.Id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
        //                    vlm.ByUID = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["ByUID"]);
        //                    vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["UserName"]);
        //                    vlm.ModifiedDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ModifiedDate"]);
        //                    vlm.LeadStatus = Convert.ToString(P_Viewuserworkdetails.Rows[i]["LeadStatus"]);
        //                    vlm.PastPerformance = Convert.ToString(P_Viewuserworkdetails.Rows[i]["PastPerformance"]);
        //                    vlmList.Add(vlm);
        //                }
        //                uwd.userworklist = vlmList;

        //            }

        //            DataTable P_closeduserworkdetails = DataAccessLayer.GetDataTable(" call Userclosedlead(" + UserId + ",'" + MStartDate + "','" + MEndDate + "')");
        //            if (P_closeduserworkdetails.Rows.Count > 0)
        //            {
        //                List<userworkdetails> vlmList = new List<userworkdetails>();
        //                for (int i = 0; i < P_closeduserworkdetails.Rows.Count; i++)
        //                {
        //                    userworkdetails vlm = new userworkdetails();

        //                    vlm.Id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
        //                    vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["UserName"]);
        //                    vlm.LeadStatus= Convert.ToString(P_closeduserworkdetails.Rows[i]["LeadStatus"]);
        //                    vlm.closedlead = Convert.ToString(P_closeduserworkdetails.Rows[i]["closedlead"]);
        //                    vlmList.Add(vlm);
        //                }
        //                uwd.userworkclosedlist = vlmList;

        //            }



        //        }
        //        else
        //        {
        //            return Redirect("/home/login");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //    }
        //    return View(uwd);
        //}

        //public ActionResult Customerworkdetails(string UserddlName , string LeadStatus)
        //{
        //    ViewSalesModel VSM = new ViewSalesModel();
        //    try
        //    {

        //        if (Session["UserName"] != null)
        //        {
        //            int UserId = Convert.ToInt32(Session["UID"]);
        //            #region DateTime-Format
        //            var dd = System.DateTime.Now;
        //            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
        //            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
        //            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
        //            var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

        //            #endregion


        //            if (LeadStatus != "Closed")
        //            {
        //                DataTable P_Viewuserworkdetails = DataAccessLayer.GetDataTable(" call CRM_CustomerWorkDetails(" + UserddlName + ")");
        //                if (P_Viewuserworkdetails.Rows.Count > 0)
        //                {
        //                    List<userworkdetails> vlmList = new List<userworkdetails>();
        //                    for (int i = 0; i < P_Viewuserworkdetails.Rows.Count; i++)
        //                    {
        //                        userworkdetails vlm = new userworkdetails();

        //                        vlm.Id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
        //                        vlm.ByUID = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["ByUID"]);
        //                        vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["UserName"]);
        //                        vlm.ModifiedDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ModifiedDate"]);
        //                        vlm.LeadStatus = Convert.ToString(P_Viewuserworkdetails.Rows[i]["LeadStatus"]);
        //                        vlm.CustomerName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Customer"]);
        //                        vlm.FollowDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["FollowDate"]);
        //                        vlm.ProfitLoss = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ProfitLoss"]);
        //                        vlm.BuySell = Convert.ToString(P_Viewuserworkdetails.Rows[i]["BuySell"]);
        //                        vlm.StockName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["StockName"]);
        //                        vlm.Price = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Price"]);
        //                        vlm.Target = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target"]);
        //                        vlm.Target2 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target2"]);
        //                        vlm.Target3 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target3"]);
        //                        vlm.SI = Convert.ToString(P_Viewuserworkdetails.Rows[i]["SI"]);
        //                        vlm.Remark = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Remark"]);
        //                        vlmList.Add(vlm);
        //                    }
        //                    VSM.userworklist = vlmList;

        //                }
        //            }
        //            else
        //            {
        //                DataTable P_Viewuserworkdetails = DataAccessLayer.GetDataTable(" call CRM_CustomerClosedleadwork(" + UserddlName + ")");
        //                if (P_Viewuserworkdetails.Rows.Count > 0)
        //                {
        //                    List<userworkdetails> vlmList = new List<userworkdetails>();
        //                    for (int i = 0; i < P_Viewuserworkdetails.Rows.Count; i++)
        //                    {
        //                        userworkdetails vlm = new userworkdetails();

        //                        vlm.Id = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["Id"]);
        //                        vlm.ByUID = Convert.ToInt32(P_Viewuserworkdetails.Rows[i]["ByUID"]);
        //                        vlm.userName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["UserName"]);
        //                        vlm.ModifiedDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ModifiedDate"]);
        //                        vlm.LeadStatus = Convert.ToString(P_Viewuserworkdetails.Rows[i]["LeadStatus"]);
        //                        vlm.CustomerName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Customer"]);
        //                        vlm.FollowDate = Convert.ToString(P_Viewuserworkdetails.Rows[i]["FollowDate"]);
        //                        vlm.ProfitLoss = Convert.ToString(P_Viewuserworkdetails.Rows[i]["ProfitLoss"]);
        //                        vlm.BuySell = Convert.ToString(P_Viewuserworkdetails.Rows[i]["BuySell"]);
        //                        vlm.StockName = Convert.ToString(P_Viewuserworkdetails.Rows[i]["StockName"]);
        //                        vlm.Price = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Price"]);
        //                        vlm.Target = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target"]);
        //                        vlm.Target2 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target2"]);
        //                        vlm.Target3 = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Target3"]);
        //                        vlm.SI = Convert.ToString(P_Viewuserworkdetails.Rows[i]["SI"]);
        //                        vlm.Remark = Convert.ToString(P_Viewuserworkdetails.Rows[i]["Remark"]);
        //                        vlmList.Add(vlm);
        //                    }
        //                    VSM.userworkclosedlist = vlmList;

        //                }
        //            }

        //        }
        //        else
        //        {
        //            return Redirect("/home/login");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //    }
        //    return View("_CustomerWorkLeadReport", VSM);
        //}

        // Update Create Lead page add description
        //[HttpPost]
        //[ValidateInput(false)]
        //public async Task<ActionResult> AddDescriptionLead()
        //{
        //    string Successmsg = string.Empty;
        //    string Errormsg = string.Empty;

        //    using (var trans = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            if (Session["UID"] != null)
        //            {
        //                var GetLeadsData = new crm_createleadstbl();

        //                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //                int UserId = Convert.ToInt32(Session["UID"]);

        //                DateTime localTime = Constant.GetBharatTime();// get india datetime
        //                var CurrentDate = localTime.ToString("dd/MM/yyyy");

        //                HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
        //                Int64? LeadID = Convert.ToInt32(Request.Form[0]);
        //                String Description = Convert.ToString(Request.Form[1]).TrimEnd();
        //                String LeadStatusName = Convert.ToString(Request.Form[2]).TrimEnd();
        //                int LeadStatusId = Convert.ToInt32(Request.Form[3]);
        //                //String FollowUpDate = Convert.ToString(Request.Form[4]).TrimEnd();
        //                string FileName = string.Empty;
        //                string FileFullName = string.Empty;


        //                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
        //                {
        //                    GetLeadsData = await db.crm_createleadstbl.FindAsync(LeadID);
        //                }
        //                else
        //                {
        //                    string query = @"select * from crm_createLeadstbl Where Id='" + LeadID + "' and (LeadOwner = '" + UserId + "' or (AssignTo !='' and AssignTo = '" + UserId + "') or (BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'))";
        //                    GetLeadsData = db.Database.SqlQuery<crm_createleadstbl>(query).FirstOrDefault();
        //                }

        //                //if(GetLeadsData!=null && !string.IsNullOrEmpty(GetLeadsData.AssignTo))
        //                //{
        //                //    var data = cr.GetUserCompanyBranch(Convert.ToInt32(GetLeadsData.AssignTo));
        //                //}



        //                if (GetLeadsData != null)
        //                {

        //                    if (Postfile != null)
        //                    {
        //                        int fileSize = Postfile.ContentLength;

        //                        if (fileSize > 0)
        //                        {
        //                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
        //                            var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
        //                            if (supportedTypes.Contains(fileExt))
        //                            {

        //                                //var CLM = await db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
        //                                string extension = Path.GetExtension(Postfile.FileName);
        //                                FileName = "Lead-" + Convert.ToString(Session["UserName"]) + "-" + GetLeadsData.Customer == null ? "NA" : GetLeadsData.Customer + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
        //                                FileFullName = FileName + extension;
        //                                string _path = Server.MapPath("~/LeadAttachment/" + FileName + extension);
        //                                Postfile.SaveAs(_path);
        //                            }
        //                            else
        //                            {
        //                                TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
        //                            }
        //                        }
        //                    }



        //                    crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
        //                    LD.Description = Description;
        //                    LD.LeadId = LeadID;
        //                    LD.Date = CurrentDate;
        //                    LD.ByUID = Convert.ToInt32(Session["UID"]);
        //                    LD.ByUserName = Convert.ToString(Session["UserName"]);
        //                    LD.CreatedDateTime = localTime;
        //                    LD.BranchID = BranchID;
        //                    LD.CompanyID = CompanyID;
        //                    LD.LeadAttachment = FileFullName;
        //                    LD.LeadStatusName = LeadStatusName;

        //                    db.crm_leaddescriptiontbl.Add(LD);


        //                    //if (!string.IsNullOrWhiteSpace(FollowUpDate))
        //                    //{
        //                    //    //GetLeadsData.FollowDate = DateTime.ParseExact(FollowUpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                    //}
        //                    //if (GetLeadsData.LeadStatusID != LeadStatusId)
        //                    //{
        //                    //    GetLeadsData.LeadStatusID = LeadStatusId;
        //                    //    GetLeadsData.LeadStatus = LeadStatusName;
        //                    //}
        //                    GetLeadsData.ModifiedDate = localTime;

        //                    db.SaveChanges();

        //                        trans.Commit();
        //                    Successmsg = "Description added succesfully.";

        //                }
        //                else
        //                {
        //                    Errormsg = "No lead record found";
        //                }

        //            }
        //            else
        //            {
        //                trans.Rollback();
        //                Errormsg = "Session has expire. Please login again";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            trans.Rollback();
        //            ExceptionLogging.SendExcepToDB(ex);
        //            Errormsg = "error";
        //        }
        //    }

        //    string MsgReturn = string.Empty;
        //    if (!string.IsNullOrWhiteSpace(Errormsg))
        //    {
        //        MsgReturn = Errormsg;
        //    }
        //    else if (!string.IsNullOrWhiteSpace(Successmsg))
        //    {
        //        MsgReturn = Successmsg;
        //    }
        //    return Json(MsgReturn, JsonRequestBehavior.AllowGet);
        //}



        //public ActionResult addDescription(Int64 LID, String Description)
        //{
        //    string msg = "";
        //    try
        //    {
        //        if (Session["UID"] != null)
        //        {
        //            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

        //            DateTime utcTime = DateTime.UtcNow;
        //            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
        //            var CurrentDate = localTime.ToString("dd/MM/yyyy");
        //            crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
        //            LD.Description = Description;
        //            LD.LeadId = LID;
        //            LD.Date = CurrentDate;
        //            LD.ByUID = Convert.ToInt32(Session["UID"]);
        //            LD.ByUserName = Convert.ToString(Session["UserName"]);
        //            LD.CreatedDateTime = localTime;
        //            LD.BranchID = BranchID;
        //            LD.CompanyID = CompanyID;
        //            db.crm_leaddescriptiontbl.Add(LD);
        //            if (db.SaveChanges() > 0)
        //            {
        //                var cl = db.crm_createleadstbl.Where(em => em.Id == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //                if (cl != null)
        //                {
        //                    cl.ModifiedDate = localTime;
        //                    db.SaveChanges();
        //                }
        //                msg = "done";
        //            }

        //        }
        //        else
        //        {
        //            msg = "Session has expire. Please Login again";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //        msg = "error";
        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}



        /// <summary>
        /// Old Login
        ///
        /// Login based on email or username and password
        /// Include CompanyID ,BranchID
        /// </summary>
        /// <param name="LM"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult login(LoginModel LM)
        //{
        //    UserLogInfoModel ULIM = new UserLogInfoModel();
        //    try
        //    {

        //        //var ac = db.crm_tbl_nipl_emp_attendance.ToList();
        //        DataTable GetUserdata = DataAccessLayer.GetDataTable("call CRM_logIn('" + LM.UserNameOrEmail + "','" + LM.Password + "')");
        //        if (GetUserdata.Rows.Count > 0)
        //        {
        //            UserLogInfoModel uLM = new UserLogInfoModel();
        //            uLM.Id = Convert.ToInt32(GetUserdata.Rows[0]["Id"]);
        //            uLM.Fname = Convert.ToString(GetUserdata.Rows[0]["Fname"]);
        //            uLM.Lname = Convert.ToString(GetUserdata.Rows[0]["Lname"]);
        //            uLM.Status = Convert.ToBoolean(GetUserdata.Rows[0]["Status"]);
        //            uLM.Email = Convert.ToString(GetUserdata.Rows[0]["Email"]);
        //            uLM.TimeZone = Convert.ToString(GetUserdata.Rows[0]["TimeZone"]);
        //            uLM.ProfileName = Convert.ToString(GetUserdata.Rows[0]["ProfileName"]);
        //            uLM.MappedUsers = Convert.ToString(GetUserdata.Rows[0]["MappedUsers"]);
        //            uLM.ProfileId = Convert.ToString(GetUserdata.Rows[0]["ProfileId"]);
        //            if (GetUserdata.Rows[0]["BranchID"] != DBNull.Value)
        //            {
        //                uLM.BranchID = Convert.ToInt32(GetUserdata.Rows[0]["BranchID"]);
        //            }
        //            if (GetUserdata.Rows[0]["CompanyID"] != DBNull.Value)
        //            {
        //                uLM.CompanyID = Convert.ToInt32(GetUserdata.Rows[0]["CompanyID"]);
        //            }
        //            uLM.StartDate = Convert.ToString(GetUserdata.Rows[0]["StartDate"]);
        //            uLM.EndDate = Convert.ToString(GetUserdata.Rows[0]["EndDate"]);
        //            if (Convert.ToString(GetUserdata.Rows[0]["IsExpired"]) != "")
        //            {
        //                uLM.IsExpired = Convert.ToBoolean(GetUserdata.Rows[0]["IsExpired"]);
        //            }
        //            if (Convert.ToString(GetUserdata.Rows[0]["IsActive"]) != "")
        //            {
        //                uLM.IsActive = Convert.ToBoolean(GetUserdata.Rows[0]["IsActive"]);
        //            }
        //            ULIM = uLM;

        //            string base64StartDate = EncodeDecodeForBase64.DecodeBase64(uLM.StartDate);
        //            string base64EndDate = EncodeDecodeForBase64.DecodeBase64(uLM.EndDate);

        //            Int32 BranchID = Convert.ToInt32(GetUserdata.Rows[0]["BranchID"]);
        //            Int32 CompanyID = Convert.ToInt32(GetUserdata.Rows[0]["CompanyID"]);


        //            if (ULIM != null && ULIM.Status == true)
        //            {
        //                #region Get-MAC-Address
        //                var mac = GetMACAddress();
        //                if (mac != string.Empty)
        //                {
        //                    mac = Regex.Replace(mac, ".{2}", "$0-").TrimEnd('-');
        //                }
        //                #endregion

        //                #region Manage-Log-Info
        //                string userIpAddress = this.Request.UserHostAddress;

        //                #region Insert Login-Time
        //                if (ULIM.TimeZone != "IST" && ULIM.TimeZone != null)
        //                {
        //                    #region Get-Time-By-TimeZone
        //                    var GetData = db.crm_zonetbl.Where(em => em.ZoneName == ULIM.TimeZone).SingleOrDefault();
        //                    if (GetData != null)
        //                    {
        //                        var IST = System.DateTime.Now;
        //                        var NewtimeZoneDate = IST.AddHours(-5);
        //                        NewtimeZoneDate = NewtimeZoneDate.AddMinutes(-30);
        //                        if (GetData.ZoneHours > 0)
        //                        {
        //                            NewtimeZoneDate = NewtimeZoneDate.AddHours(GetData.ZoneHours);
        //                        }
        //                        else if (GetData.ZoneMin > 0)
        //                        {
        //                            NewtimeZoneDate = NewtimeZoneDate.AddMinutes(GetData.ZoneMin);
        //                        }

        //                        var NewZonetime = NewtimeZoneDate.ToString("hh:mm:ss tt");
        //                        var NewZoneDate = NewtimeZoneDate.ToString("dd/MM/yyyy");
        //                        var CheckExistAttand = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == ULIM.Id && em.L_In_Date == NewZoneDate && em.BranchID == BranchID && em.CompanyID == CompanyID).SingleOrDefault();
        //                        if (CheckExistAttand == null)
        //                        {
        //                            crm_tbl_nipl_emp_attendance LogAtt = new crm_tbl_nipl_emp_attendance();
        //                            LogAtt.EmpId = ULIM.Id;
        //                            LogAtt.L_In_Date = NewZoneDate;
        //                            LogAtt.L_In_Time = NewZonetime;
        //                            LogAtt.Status = "P";
        //                            LogAtt.IPAddress = userIpAddress;
        //                            LogAtt.MacAddress = mac;
        //                            LogAtt.LogTimeZone = ULIM.TimeZone;
        //                            LogAtt.Working_Late_Night = false;
        //                            LogAtt.Extra_working = false;
        //                            LogAtt.CompanyID = CompanyID;
        //                            LogAtt.BranchID = BranchID;
        //                            db.crm_tbl_nipl_emp_attendance.Add(LogAtt);
        //                            db.SaveChanges();
        //                        }
        //                    }
        //                    #endregion
        //                }
        //                else
        //                {
        //                    DateTime utcTime = DateTime.UtcNow;
        //                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //                    DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

        //                    var time = localTime.ToString("hh:mm:ss tt");//System.DateTime.Now.ToString("hh:mm:ss tt");
        //                    var Date = localTime.ToString("dd/MM/yyyy");//System.DateTime.Now.ToString("MM/dd/yyyy");
        //                    var CheckExistAttand = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == ULIM.Id && em.L_In_Date == Date && em.BranchID == BranchID && em.CompanyID == CompanyID).SingleOrDefault();
        //                    if (CheckExistAttand == null)
        //                    {
        //                        crm_tbl_nipl_emp_attendance LogAtt = new crm_tbl_nipl_emp_attendance();
        //                        LogAtt.EmpId = ULIM.Id;
        //                        LogAtt.L_In_Date = Date;
        //                        LogAtt.L_In_Time = time;
        //                        LogAtt.Status = "P";
        //                        LogAtt.IPAddress = userIpAddress;
        //                        LogAtt.MacAddress = mac;
        //                        LogAtt.LogTimeZone = ULIM.TimeZone;
        //                        LogAtt.Working_Late_Night = false;
        //                        LogAtt.Extra_working = false;
        //                        LogAtt.CompanyID = CompanyID;
        //                        LogAtt.BranchID = BranchID;
        //                        db.crm_tbl_nipl_emp_attendance.Add(LogAtt);
        //                        db.SaveChanges();
        //                    }
        //                }
        //                #endregion

        //                Session["UserName"] = Convert.ToString(ULIM.Fname + " " + ULIM.Lname);
        //                Session["UID"] = Convert.ToInt32(ULIM.Id);
        //                Session["UserEmail"] = ULIM.Email;
        //                Session["UserType"] = ULIM.ProfileName;
        //                Session["IsMapped"] = ULIM.MappedUsers;
        //                Session["TimeZone"] = ULIM.TimeZone;
        //                Session["CompanyID"] = ULIM.CompanyID;
        //                Session["BranchID"] = ULIM.BranchID;

        //                if (ULIM.ProfileName == "SuperAdmin")
        //                {
        //                    Session["RightsCreateLeads"] = true;
        //                    Session["RightsViewLeads"] = true;
        //                    Session["RightsViewSales"] = true;
        //                    Session["RightsMessage"] = true;
        //                    Session["RightsRoleManagement"] = true;
        //                    Session["RightsNotification"] = true;
        //                    Session["RightsDeveloperReport"] = true;
        //                    Session["RightsLeadNotify"] = true;
        //                    Session["RightsProjectManagement"] = true;
        //                    Session["RightsCommonActivityRemarks"] = true;
        //                    Session["RightsLeadManagement"] = true;
        //                    Session["RightsAssignLeadManagement"] = true;
        //                    Session["RightsDailyworkSchedule"] = true;
        //                }
        //                else
        //                {
        //                    if (ULIM.ProfileId != null && ULIM.ProfileId != "")
        //                    {
        //                        var GetProfileId = Convert.ToInt32(ULIM.ProfileId);
        //                        var GetPermission = db.crm_roleassigntbl.Where(em => em.Id == GetProfileId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //                        if (GetPermission != null)
        //                        {
        //                            Session["RightsCreateLeads"] = Convert.ToBoolean(GetPermission.CreateLeads);
        //                            Session["RightsViewLeads"] = Convert.ToBoolean(GetPermission.ViewLeads);
        //                            Session["RightsViewSales"] = Convert.ToBoolean(GetPermission.ViewSales);
        //                            Session["RightsCommonActivityRemarks"] = Convert.ToBoolean(GetPermission.CommonActivityRemark);
        //                            Session["RightsRoleManagement"] = Convert.ToBoolean(GetPermission.RoleManagement);
        //                            Session["RightsNotification"] = Convert.ToBoolean(GetPermission.Notification);
        //                            Session["RightsLeadNotify"] = Convert.ToBoolean(GetPermission.LeadNotify);
        //                            Session["RightsDeveloperReport"] = Convert.ToBoolean(GetPermission.DeveloperReport);
        //                            Session["RightsProjectManagement"] = Convert.ToBoolean(GetPermission.ProjectManagement);
        //                            Session["RightsCommonActivityRemarks"] = Convert.ToBoolean(GetPermission.CommonActivityRemark);
        //                            Session["RightsLeadManagement"] = Convert.ToBoolean(GetPermission.LeadManagement);
        //                            Session["RightsAssignLeadManagement"] = Convert.ToBoolean(GetPermission.AssignLeadManagement);
        //                            Session["RightsDailyworkSchedule"] = Convert.ToBoolean(GetPermission.DailyWorkSchedule);
        //                        }
        //                    }
        //                }
        //                if (Session["ReturnUrl"] != null)
        //                {
        //                    return Redirect(Convert.ToString(Session["ReturnUrl"]));
        //                }
        //                else
        //                {
        //                    #region Set-ProfilePage
        //                    if (ULIM.ProfileName == "Developer")
        //                    {
        //                        Session["ProfilePageUrl"] = "/Nis/Developer";
        //                        return Redirect("/Nis/Developer");
        //                    }
        //                    else if (ULIM.ProfileName == "Sales" || ULIM.ProfileName == "SuperAdmin")
        //                    {
        //                        Session["ProfilePageUrl"] = "/";
        //                        return Redirect("/");
        //                    }
        //                    else if (ULIM.ProfileName == "HR")
        //                    {
        //                        Session["ProfilePageUrl"] = "/Hr/dashboard";
        //                        return Redirect("/Hr/dashboard");
        //                    }
        //                    else
        //                    {
        //                        Session["ProfilePageUrl"] = "/Nis/Remark";
        //                        return Redirect("/Nis/Remark");
        //                    }
        //                    #endregion
        //                }
        //                #endregion
        //            }
        //            else
        //            {
        //                TempData["alert"] = "Incorrect username or email and password !";
        //            }
        //        }
        //        else
        //        {
        //            TempData["alert"] = "** Username or e-mail and Password does not exits,please contact to administrator !";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //        TempData["alert"] = ex.Message.ToString();//"There is some problem!";
        //    }
        //    return View();
        //}


        /// <summary>
        /// Not in used
        /// </summary>
        /// <param name="LID"></param>
        /// <param name="txtDescription"></param>
        /// <param name="FollowUpDate"></param>
        /// <param name="hdnLeadStatusName"></param>
        /// <param name="Postfile"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult addDescriptionMVC(Int64 LID, String txtDescription, String FollowUpDate, String hdnLeadStatusName, HttpPostedFileBase Postfile)
        //{
        //    string msg = string.Empty;
        //    try
        //    {
        //        if (Session["UID"] != null)
        //        {
        //            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

        //            DateTime utcTime = DateTime.UtcNow;
        //            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
        //            CreateLeadsModel CLM1 = new CreateLeadsModel();
        //            var getleadStatus = db.crm_leadstatus_tbl.Where(em => em.Id == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
        //            if (getleadStatus != null)
        //            {
        //                List<LeadStatusModel> LSMList = new List<LeadStatusModel>();
        //                foreach (var item in getleadStatus)
        //                {
        //                    LeadStatusModel LSM = new LeadStatusModel();
        //                    LSM.Id = item.Id;
        //                    LSM.LeadStatusname = item.LeadStatusName;
        //                    LSMList.Add(LSM);
        //                }
        //                CLM1.leadstatusList = LSMList;
        //            }



        //            String FileName = String.Empty;
        //            String FileFullName = String.Empty;
        //            if (Postfile != null)
        //            {
        //                if (Postfile.ContentLength > 0)
        //                {
        //                    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
        //                    var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
        //                    if (supportedTypes.Contains(fileExt))
        //                    {
        //                        //get Customer name CLM.Customer
        //                        var CLM = db.crm_createleadstbl.Where(em => em.Id == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();

        //                        String extension = Path.GetExtension(Postfile.FileName);
        //                        FileName = "Lead-" + Convert.ToString(Session["UserName"]) + "-" + CLM.Customer + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
        //                        FileFullName = FileName + extension;
        //                        String _path = Server.MapPath("~/LeadAttachment/" + FileName + extension);
        //                        Postfile.SaveAs(_path);
        //                    }
        //                    else
        //                    {
        //                        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
        //                    }
        //                }
        //            }


        //            var CurrentDate = localTime.ToString("dd/MM/yyyy");
        //            crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
        //            LD.Description = txtDescription;
        //            LD.LeadId = LID;
        //            LD.Date = CurrentDate;
        //            LD.ByUID = Convert.ToInt32(Session["UID"]);
        //            LD.ByUserName = Convert.ToString(Session["UserName"]);
        //            LD.CreatedDateTime = localTime;
        //            LD.BranchID = BranchID;
        //            LD.CompanyID = CompanyID;
        //            LD.LeadAttachment = FileFullName;
        //            LD.LeadStatusName = hdnLeadStatusName;
        //            db.crm_leaddescriptiontbl.Add(LD);
        //            if (db.SaveChanges() > 0)
        //            {
        //                var cl = db.crm_createleadstbl.Where(em => em.Id == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //                if (cl != null)
        //                {
        //                    cl.ModifiedDate = System.DateTime.Now.Date;
        //                    if (!string.IsNullOrWhiteSpace(FollowUpDate))
        //                    {
        //                        cl.FollowDate = DateTime.ParseExact(FollowUpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //                    }
        //                    db.SaveChanges();

        //                    var lid = LD.Id;
        //                    var descupdate = db.crm_leaddescriptiontbl.Where(em => em.LeadId == LID && em.Id == lid).FirstOrDefault();
        //                    if (descupdate != null)
        //                    {
        //                        descupdate.LeadStatusName = cl.LeadStatus;
        //                        db.SaveChanges();
        //                    }
        //                }
        //                msg = "done";
        //            }
        //        }
        //        else
        //        {
        //            msg = "Session has expire. Please Login again";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "error";
        //        ExceptionLogging.SendExcepToDB(ex);
        //    }
        //    return Redirect("/home/viewleads/?page=1&UserddlName=" + Session["UserddlName"] + "&filterText=" + Session["filterText"] + "&FilterType=" + Session["VLFltrFilterType"] + "&FromDate=" + Session["VLFltrFrmDt"] + "&ToDate=" + Session["VLFltrToDt"] + "");
        //    //return Redirect("/home/viewleads/?page=1");
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public JsonResult addDescriptionCreateLead(Int64 LID, string txtDescription, HttpPostedFileBase Postfile)
        //{
        //    string Successmsg = string.Empty;
        //    string Errormsg = string.Empty;
        //    var url = Request.Url.PathAndQuery;
        //    try
        //    {
        //        if (Session["UID"] != null)
        //        {
        //            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

        //            DateTime utcTime = DateTime.UtcNow;
        //            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

        //            string FileName = string.Empty;
        //            string FileFullName = string.Empty;
        //            if (Postfile != null)
        //            {
        //                if (Postfile.ContentLength > 0)
        //                {
        //                    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
        //                    var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
        //                    if (supportedTypes.Contains(fileExt))
        //                    {
        //                        //get Customer name CLM.Customer
        //                        var CLM = db.crm_createleadstbl.Where(em => em.Id == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();

        //                        string extension = Path.GetExtension(Postfile.FileName);
        //                        FileName = "Lead-" + Convert.ToString(Session["UserName"]) + "-" + CLM.Customer + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
        //                        FileFullName = FileName + extension;
        //                        string _path = Server.MapPath("~/LeadAttachment/" + FileName + extension);
        //                        Postfile.SaveAs(_path);
        //                    }
        //                    else
        //                    {
        //                        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
        //                    }
        //                }
        //            }


        //            var CurrentDate = localTime.ToString("dd/MM/yyyy");
        //            crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
        //            LD.Description = txtDescription;
        //            LD.LeadId = LID;
        //            LD.Date = CurrentDate;
        //            LD.ByUID = Convert.ToInt32(Session["UID"]);
        //            LD.ByUserName = Convert.ToString(Session["UserName"]);
        //            LD.CreatedDateTime = localTime;
        //            LD.BranchID = BranchID;
        //            LD.CompanyID = CompanyID;
        //            LD.LeadAttachment = FileFullName;
        //            db.crm_leaddescriptiontbl.Add(LD);
        //            if (db.SaveChanges() > 0)
        //            {
        //                var cl = db.crm_createleadstbl.Where(em => em.Id == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //                if (cl != null)
        //                {
        //                    cl.ModifiedDate = localTime;
        //                    db.SaveChanges();
        //                }
        //                Successmsg = "Description add succesfully.";
        //            }
        //        }
        //        else
        //        {
        //            Errormsg = "Session has expire. Please login again";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //        Errormsg = "error";
        //    }

        //    string MsgReturn = string.Empty;
        //    if (!string.IsNullOrWhiteSpace(Errormsg))
        //    {
        //        MsgReturn = Errormsg;
        //    }
        //    else if (!string.IsNullOrWhiteSpace(Successmsg))
        //    {
        //        MsgReturn = Successmsg;
        //    }
        //    return Json(MsgReturn, JsonRequestBehavior.AllowGet);
        //}


        ///<summary>Not in use get customer id</summary>
        ///
        ///private string GetCustomerID(String companyID)
        ///{
        ///    String getID = String.Empty;
        ///    //try
        ///    //{
        ///       Int32 ID = Convert.ToInt32(companyID);
        ///        var CompanyProfile = db.company_profile.Where(em => em.ID == ID).FirstOrDefault();
        ///        var getCustomerID = db.customers.Where(em => em.CompanyID == companyID).ToList();
        ///        if (getCustomerID.Count > 0)
        ///        {
        ///            if (getCustomerID.Count == 0)
        ///            {
        ///                getID = CompanyProfile.Organization.Substring(0, 3).ToUpper() + "-CUSL-" + 1;
        ///            }
        ///            else
        ///            {
        ///                getID = CompanyProfile.Organization.Substring(0, 3).ToUpper() + "-CUSL-" + (getCustomerID.Count + 1);
        ///            }
        ///        }
        ///        return getID;
        ///    //}
        ///    //catch (Exception ex)
        ///    //{
        ///    //    ExceptionLogging.SendExcepToDB(ex);
        ///    //    throw ex;
        ///    //}
        ///}
        ///<returns></returns>


        //public ActionResult ViewDelayedLeadReportInfo(string FilterText, string FromDate, string ToDate, string UserddlName)
        //{
        //    ViewLeadsModel VLM = new ViewLeadsModel();
        //    try
        //    {
        //        #region Data-time-Formate

        //        var dd = Constant.GetBharatTime();
        //        DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
        //        DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
        //        var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
        //        var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
        //        #region To-CheckFilter-Date
        //        if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
        //        {
        //            MStartDate = FromDate;
        //            MEndDate = ToDate;
        //        }
        //        #endregion

        //        #endregion

        //        var UID = 0;
        //        if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
        //        {
        //            UID = 0;
        //            if (UserddlName != null && UserddlName != "0")
        //            {
        //                UID = Convert.ToInt32(UserddlName);
        //            }
        //        }
        //        else
        //        {
        //            if (UserddlName != null && UserddlName != "0")
        //            {
        //                UID = Convert.ToInt32(UserddlName);
        //            }
        //            else
        //            {
        //                UID = Convert.ToInt32(Session["UID"]);
        //            }
        //        }

        //        if (FilterText == "delayedfollowup")
        //        {
        //            #region Get-delayedfollowup Info
        //            //Collect the Manually Created-Leads
        //            DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetDelayedLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + Convert.ToInt32(Session["BranchID"]) + "," + Convert.ToInt32(Session["CompanyID"]) + ")");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                VLM.viewleadsList = (from dr in GetRecords.AsEnumerable()
        //                                     select new ViewLeadsModel()
        //                                     {
        //                                         Id = Convert.ToInt32(dr["Id"]),
        //                                         LeadName = Convert.ToString(dr["Customer"]),
        //                                         Mob = Convert.ToString(dr["MobileNo"]),
        //                                         EMail = Convert.ToString(dr["EmailId"]),
        //                                         Country = Convert.ToString(dr["Country"]),
        //                                         State = Convert.ToString(dr["State"]),
        //                                         City = Convert.ToString(dr["City"]),
        //                                         FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
        //                                         Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
        //                                         LeadStatus = Convert.ToString(dr["LeadStatus"]),
        //                                         LeadOwner = Convert.ToString(dr["LeadOwner"]),
        //                                         AssignTo = Convert.ToString(dr["AssignTo"]),
        //                                         PreFollowUpDate = Convert.ToString(dr["PreFollowUpDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace(" 12:00:00 AM", ""),
        //                                         CreatedDate = Convert.ToString(dr["CreatedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "").Replace("12:00:00 AM", ""),
        //                                     }).ToList();

        //            }

        //            if (!string.IsNullOrEmpty(UserddlName) && UserddlName != "0")
        //            {
        //                VLM.viewleadsList = VLM.viewleadsList.Where(em => em.LeadOwner == UserddlName || em.AssignTo == UserddlName).ToList();
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //    }
        //    return PartialView("_DelayedLeadReportInfoList", VLM);
        //}

    }
}