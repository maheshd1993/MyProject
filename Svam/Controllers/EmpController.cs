using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Svam.EF;
using System.Data;
using Traders.Models;
using Svam.Models;
using Svam.UtilityManager;
using System.Globalization;
using Svam._Classes;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace Svam.Controllers
{
    public class EmpController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        apiclasses apiclass = new apiclasses();
        DataUtility du = new DataUtility();
        public ActionResult Attandance(string StartDate, string EndDate, int? CompanyTypeID)
        {
            EmployeeAttandaceReportModel EARM = new EmployeeAttandaceReportModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                EARM.DateFormat = Constant.DateFormat();//get date format by company id
                Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

                #region Cal- DateTime.......
                var CurrentDate = Constant.GetBharatTime();
                DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                var calday = (MonthendDate - monthStartDate).Days + 1;
                var TodayDate = CurrentDate.ToString("dd/MM/yyyy");
                var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

                if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))//|| startdate != string.Empty && enddate != string.Empty)
                {

                    if (EARM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        var fmDate = DateTime.ParseExact(StartDate, EARM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(StartDate);
                        var tDate = DateTime.ParseExact(EndDate, EARM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(EndDate);

                        MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                        MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        calday = (Convert.ToDateTime(MEndDate) - Convert.ToDateTime(MStartDate)).Days + 1;
                        EARM.StartDate = MStartDate;
                        EARM.EndDate = MEndDate;
                    }
                    else
                    {
                        MStartDate = StartDate;
                        MEndDate = EndDate;
                        calday = (Convert.ToDateTime(MEndDate) - Convert.ToDateTime(MStartDate)).Days + 1;
                        EARM.StartDate = MStartDate;
                        EARM.EndDate = MEndDate;
                    }
                }
                else
                {
                    if (EARM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        EARM.StartDate = monthStartDate.ToString(EARM.DateFormat);
                        EARM.EndDate = MonthendDate.ToString(EARM.DateFormat);
                        calday = (Convert.ToDateTime(MEndDate) - Convert.ToDateTime(MStartDate)).Days + 1;
                    }
                    else
                    {
                        EARM.StartDate = MStartDate;
                        EARM.EndDate = MEndDate;
                        calday = (Convert.ToDateTime(MEndDate) - Convert.ToDateTime(MStartDate)).Days + 1;
                    }
                }
                #endregion

                #region CompanyType
                string ptQury = @"select Id as CompanyTypeID, CompanyTypeName from crm_UserCompanytypetbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and Status=1";
                var getCompanyType = db.Database.SqlQuery<EmployeeAttandaceReportModel>(ptQury).OrderBy(a => a.CompanyTypeName).ToList();
                if (getCompanyType.Count > 0)
                {
                    EARM.CompanyTypeList = getCompanyType;
                }
                #endregion
                int UID = 0;
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    UID = Convert.ToInt32(Session["UID"]);
                    int companyid = 0;

                    var GetUser = db.crm_usertbl.Where(em => em.Id != UID && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();


                    if (CompanyTypeID != null)
                    {

                        DataTable GetTodayLeads = DataAccessLayer.GetDataTable(" call CRM_AllEmpAttandanceByDateCompanyType('" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + "," + CompanyTypeID + ")");
                        if (GetTodayLeads.Rows.Count > 0)
                        {
                            EARM.EmpAttandanceRepotModelList = (from dr in GetTodayLeads.AsEnumerable()
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
                                                                    LogZoneTime = Convert.ToString(dr["LogTimeZone"]),
                                                                    CompanyName = Convert.ToString(dr["CompanyName"]),
                                                                    CompanyTypeId = Convert.ToInt32(dr["CompanyTypeId"])
                                                                }).ToList();
                        }
                    }
                    else
                    {

                        DataTable GetTodayLeads = DataAccessLayer.GetDataTable(" call CRM_AllEmpAttandanceByDate('" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetTodayLeads.Rows.Count > 0)
                        {
                            EARM.EmpAttandanceRepotModelList = (from dr in GetTodayLeads.AsEnumerable()
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
                                                                    LogZoneTime = Convert.ToString(dr["LogTimeZone"]),
                                                                    CompanyName = Convert.ToString(dr["CompanyName"]),
                                                                    CompanyTypeId = Convert.ToInt32(dr["CompanyTypeId"])
                                                                }).ToList();
                        }
                    }

                    foreach (var item in GetUser.ToList())
                    {
                        var getAttandanceByEmp = EARM.EmpAttandanceRepotModelList.Where(em => em.EmpId == item.Id).ToList();
                        int Loginontime = 0;
                        int Loginofftime = 0;
                        int ExtraHoursDay = 0;
                        int SatAndSun = 0;
                        string Companyname = "";
                        var Getcompanydetails = db.crm_usercompanytypetbl.Where(em => em.Id == item.CompanyTypeId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        foreach (var Aitem in getAttandanceByEmp)
                        {
                            //if (Aitem.LogZoneTime == "IST")
                            //{
                            //    var MatchCondition = Convert.ToDateTime("10:15:00 AM").TimeOfDay;
                            //    var LogDate = Convert.ToDateTime(Aitem.LoginDate).DayOfWeek;
                            //    var logt = Convert.ToDateTime(Aitem.LoginTime).TimeOfDay;
                            //    var duration = Convert.ToString(Aitem.WorkDuration);
                            //    var spliteDuration = "0";
                            //    if (!string.IsNullOrWhiteSpace(duration))
                            //    {
                            //        spliteDuration = duration.Substring(1, 1);
                            //    }

                            //    if (Convert.ToString(LogDate) == "Saturday" || Convert.ToString(LogDate) == "Sunday")
                            //    {
                            //        //Start to cal Saturday and sunday.....
                            //        SatAndSun++;
                            //    }
                            //    else if (logt <= MatchCondition && Convert.ToInt32(spliteDuration) >= 9)
                            //    {
                            //        //Start to cal Log on Time And Duration 9Hrs Complete....
                            //        if (Convert.ToInt32(spliteDuration) > 9)
                            //        {
                            //            ExtraHoursDay++;
                            //        }
                            //        else
                            //        {
                            //            Loginontime++;
                            //        }
                            //    }
                            //    else if (logt > MatchCondition)
                            //    {
                            //        //Start to calculate Off-time Login
                            //        Loginofftime++;
                            //    }

                            //    #region Start to Calculate-Leave

                            //    #endregion
                            //}

                            #region code modify on 07/12/2020 by arun
                            if (Aitem.LogZoneTime == "IST")
                            {
                                var MatchCondition = Convert.ToDateTime("10:15:00 AM").TimeOfDay;
                                var LogDate = Convert.ToDateTime(Aitem.LoginDate).DayOfWeek;
                                var logt = Convert.ToDateTime(Aitem.LoginTime).TimeOfDay;
                                var duration = Convert.ToString(Aitem.WorkDuration);
                                //Companyname = Convert.ToString(Aitem.CompanyName);
                                var spliteDuration = "0";
                                if (!string.IsNullOrWhiteSpace(duration))
                                {
                                    // spliteDuration = duration.Substring(1, 1);
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

                                #region Start to Calculate-Leave

                                #endregion
                            }
                            #endregion
                        }
                        //var getcompanydetails = db.crm_usercompanytypetbl.Where(em => em.Id == item.CompanyTypeId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        //if(getcompanydetails!=null)
                        //{
                        //    EARM.CompanyName = getcompanydetails.CompanyTypeName;
                        //}

                        var cal = new EmployeeAttandaceReportModel
                        {
                            EmpId = item.Id,
                            EmpName = item.Fname + " " + item.Lname,
                            PresentOnTime = Loginontime,
                            PresentOFFTime = Loginofftime,
                            ExtraHoursDay = ExtraHoursDay,
                            SatAndSun = SatAndSun,
                            CompanyTypeID = item.CompanyTypeId,
                            CompanyName = Getcompanydetails.CompanyTypeName,
                            Absent = calday - (Loginontime + Loginofftime + SatAndSun),
                            Total = calday,
                        };
                        EARM.EmployeeAttandaceReportModelList.Add(cal);
                    }
                }
                else
                {
                    UID = Convert.ToInt32(Session["UID"]);
                    //var GetUser = db.crm_usertbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    var GetUser = db.crm_usertbl.Where(em => em.Id == UID && em.Status == true /*&& em.BranchID == BranchID && em.CompanyID == CompanyID*/).FirstOrDefault();

                    //DataTable GetTodayLeads = DataAccessLayer.GetDataTable(" call CRM_AllEmpAttandanceByDate('" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    DataTable GetTodayLeads = DataAccessLayer.GetDataTable("call CRM_GetEmpAttandanceByID('" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + "," + UID + ")");

                    if (GetTodayLeads.Rows.Count > 0)
                    {
                        EARM.EmpAttandanceRepotModelList = (from dr in GetTodayLeads.AsEnumerable()
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
                                                                LogZoneTime = Convert.ToString(dr["LogTimeZone"]),
                                                                CompanyName = Convert.ToString(dr["CompanyName"]),
                                                                CompanyTypeId = Convert.ToInt32(dr["CompanyTypeId"])
                                                            }).ToList();
                    }

                    //foreach (var item in GetUser.ToList())
                    //{
                    //var getAttandanceByEmp = EARM.EmpAttandanceRepotModelList.Where(em => em.EmpId == item.Id).ToList();
                    if (GetUser != null)
                    {
                        var getAttandanceByEmp = EARM.EmpAttandanceRepotModelList;// EARM.EmpAttandanceRepotModelList.Where(em => em.EmpId == GetUser.Id).ToList();
                        int Loginontime = 0;
                        int Loginofftime = 0;
                        int ExtraHoursDay = 0;
                        int SatAndSun = 0;
                        string Companyname = "";
                        foreach (var Aitem in getAttandanceByEmp)
                        {
                            if (Aitem.LogZoneTime == "IST")
                            {
                                var MatchCondition = Convert.ToDateTime("10:15:00 AM").TimeOfDay;
                                var LogDate = Convert.ToDateTime(Aitem.LoginDate).DayOfWeek;
                                var logt = Convert.ToDateTime(Aitem.LoginTime).TimeOfDay;
                                var duration = Convert.ToString(Aitem.WorkDuration);
                                var spliteDuration = "0";
                                Companyname = Convert.ToString(Aitem.CompanyName);
                                if (!string.IsNullOrWhiteSpace(duration))
                                {
                                    // spliteDuration = duration.Substring(1, 1);
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

                                #region Start to Calculate-Leave

                                #endregion
                            }
                        }

                        var cal = new EmployeeAttandaceReportModel
                        {
                            EmpId = GetUser.Id,
                            EmpName = GetUser.Fname + " " + GetUser.Lname,
                            PresentOnTime = Loginontime,
                            PresentOFFTime = Loginofftime,
                            ExtraHoursDay = ExtraHoursDay,
                            SatAndSun = SatAndSun,
                            CompanyName = Companyname,
                            CompanyTypeID = GetUser.CompanyTypeId,
                            Absent = calday - (Loginontime + Loginofftime + SatAndSun),
                            Total = calday,
                        };
                        EARM.EmployeeAttandaceReportModelList.Add(cal);
                    }

                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(EARM);
        }

        public ActionResult GetAttendanceDetails(string startdate, string enddate, string EmpName, int UID = 0)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            EmployeeAttandaceReportModel EARM = new EmployeeAttandaceReportModel();
            EARM.DateFormat = Constant.DateFormat();//get date format by company id
            #region Cal- DateTime.......
            //var CurrentDate = Constant.GetBharatTime();
            //DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            //var calday = (MonthendDate - monthStartDate).Days + 1;
            //var TodayDate = CurrentDate.ToString("dd/MM/yyyy");
            //var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            //var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

            var MStartDate = string.Empty;
            var MEndDate = string.Empty;
            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))//|| startdate != string.Empty && enddate != string.Empty)
            {

                //EARM.StartDate = MStartDate;
                //EARM.EndDate = MEndDate;
                EARM.EmpName = EmpName;

                if (EARM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    var fmDate = DateTime.ParseExact(startdate, EARM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(startdate);
                    var tDate = DateTime.ParseExact(enddate, EARM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(enddate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                    EARM.StartDate = startdate;
                    EARM.EndDate = enddate;
                }
                else
                {
                    MStartDate = startdate;
                    MEndDate = enddate;
                }
                //calday = (Convert.ToDateTime(MEndDate) - Convert.ToDateTime(MStartDate)).Days + 1;
            }
            //else
            //{
            //    startdate = MStartDate;
            //    enddate = MEndDate;
            //    //calday = (Convert.ToDateTime(enddate) - Convert.ToDateTime(startdate)).Days + 1;
            //}
            #endregion


            DataTable GetTodayLeads = DataAccessLayer.GetDataTable("call CRM_GetEmpAttandanceByID('" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + "," + UID + ")");

            if (GetTodayLeads.Rows.Count > 0)
            {
                EARM.EmpAttandanceRepotModelList = (from dr in GetTodayLeads.AsEnumerable()
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
            return PartialView("EmpAttendaceDetail", EARM);
        }

        public ActionResult AddExpense(Int64? ExpenseID)
        {
            ExpenseModel EXP = new ExpenseModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
                if (getEmployeeList != null)
                {
                    List<ExpenseModel> CUMEmployeeList = new List<ExpenseModel>();
                    foreach (var item in getEmployeeList)
                    {
                        ExpenseModel CUMEmployee = new ExpenseModel();
                        CUMEmployee.EmployeeID = item.Id;
                        CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                        CUMEmployeeList.Add(CUMEmployee);
                    }
                    EXP.EmployeeList = CUMEmployeeList;
                }

                //Call Only Employee is Login
                if (Convert.ToString(Session["UserType"]) != "SuperAdmin")
                {
                    int UID = Convert.ToInt32(Session["UID"]);
                    EXP.EmployeeList = EXP.EmployeeList.Where(em => em.EmployeeID == UID).ToList();
                }

                //List<LeaveRequestModel> LeaveTypeList = new List<LeaveRequestModel>
                //{
                //    new LeaveRequestModel { LeaveTypeID =1, LeaveName = "Casual Leave" },
                //    new LeaveRequestModel { LeaveTypeID =2, LeaveName = "Medical Leave" }
                //};
                //LRM.LeaveTypeList = LeaveTypeList;

                //var geterrortypeList = db.crm_leavetypename.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                //if (geterrortypeList.Count > 0)
                //{
                //	List<LeaveRequestModel> LeaveTypeList = new List<LeaveRequestModel>();
                //	foreach (var item in geterrortypeList)
                //	{
                //		LeaveRequestModel cError = new LeaveRequestModel();
                //		cError.LeaveTypeID = item.ID;
                //		cError.LeaveName = item.LeaveName;
                //		LeaveTypeList.Add(cError);
                //	}
                //	LRM.LeaveTypeList = LeaveTypeList;
                //}

                //if (RequestID > 0)
                //{
                //	var GetEmployeeLeave = db.crm_leaverequest_tbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Id == RequestID).FirstOrDefault();
                //	if (GetEmployeeLeave != null)
                //	{
                //		LRM.RequestID = GetEmployeeLeave.Id;
                //		LRM.EmployeeID = GetEmployeeLeave.EmployeeID;
                //		LRM.LeaveTypeID = GetEmployeeLeave.LeaveTypeID;
                //		LRM.Subject = GetEmployeeLeave.Subject;
                //		LRM.Message = GetEmployeeLeave.Message;
                //		LRM.ProcessStatus = GetEmployeeLeave.ProcessStatus;
                //	}
                //}
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return Redirect("/Emp/AddExpense");
            }
            return View(EXP);
        }


        [HttpPost]
        public ActionResult AddExpense(ExpenseModel EXP, Int32? ExpenseID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                if (ExpenseID > 0)
                {
                    //var getUpdateRecord = db.crm_leaverequest_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == RequestID).FirstOrDefault();
                    //if (getUpdateRecord != null)
                    //{
                    //	getUpdateRecord.EmployeeID = LRM.EmployeeID;
                    //	getUpdateRecord.LeaveTypeID = LRM.LeaveTypeID;
                    //	getUpdateRecord.Subject = LRM.Subject;
                    //	getUpdateRecord.Message = LRM.Message;
                    //	getUpdateRecord.RequestDate = Constant.GetBharatTime();
                    //	db.SaveChanges();
                    //	TempData["success"] = "Leave updated successfully";
                    //	return Redirect("/LeaveManagement/LeaveRequestView");
                    //}
                    //else
                    //{
                    //	TempData["alert"] = "There is some problem";
                    return Redirect("/Emp/AddExpense");
                    //}
                }
                else
                {
                    InsertExpense(EXP);

                    //crm_leaverequest_tbl CLM = new crm_leaverequest_tbl();
                    //CLM.EmployeeID = LRM.EmployeeID;
                    //CLM.LeaveTypeID = LRM.LeaveTypeID;
                    //CLM.Subject = LRM.Subject;
                    //CLM.Message = LRM.Message;
                    //CLM.ProcessStatus = "In Process";
                    //CLM.RequestDate = Constant.GetBharatTime();
                    //CLM.CompanyID = CompanyID;
                    //CLM.BranchID = BranchID;
                    //db.crm_leaverequest_tbl.Add(CLM);
                    //db.SaveChanges();
                    //TempData["success"] = "Leave added successfully";
                    return Redirect("/Emp/ExpenseView");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "There is some problem";
                return Redirect("/Emp/AddExpense");
            }
        }

        public string InsertExpense(ExpenseModel expense)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            int res = 0;

            res = du.ExecuteSql("insert into crm_expense_request_tbl (EmployeeId,ExpenseTypeID,travelledKMS,expense,companyId,BranchId,RequestDate,ProcessDate,ProcessStatus) " +
                                    "values('" + expense.EmployeeID + "','" + expense.ExpanseTypeID + "','" + expense.travelledKMS + "','" + expense.expense + "','" + CompanyID + "','" + BranchID + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','In Process')");

            object insertedexpenseId = du.GetScalar("SELECT LAST_INSERT_ID();");

            //	var lastsynciduserlogin = du.GetDataTable("SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as syncid FROM t_userlogin  WHERE CompanyID='" + register.CompanyID + "' and SyncID RLIKE 'O'");
            //int synciduserlogin = Convert.ToInt32(lastsynciduserlogin.Rows[0]["syncid"]);
            //synciduserlogin = synciduserlogin + 1;
            //res = du.ExecuteSql("insert into t_userlogin (user_name,user_email,user_mobile,user_password,user_address,cust_id,CreatedDate,IsActive,CompanyID,flag,SyncID)" +
            //					"values('" + register.Name + "','" + register.Email + "','" + register.Mobile + "','" + register.Password + "','" + register.Address + "'," + insertedcustid.ToString() + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','yes','" + register.CompanyID + "','N','" + "O" + synciduserlogin.ToString() + "')");
            if (res > 0)
            {
                return "Registered Successfully";
            }
            else
            {
                return "Try Again";
            }
        }

        public ActionResult ExpenseView(Int32? EmployeeID)
        {
            ExpenseModel EXP = new ExpenseModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            EXP.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            var getEmployeeList = db.crm_usertbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
            if (getEmployeeList != null)
            {
                List<ExpenseModel> CUMEmployeeList = new List<ExpenseModel>();
                foreach (var item in getEmployeeList)
                {
                    ExpenseModel CUMEmployee = new ExpenseModel();
                    CUMEmployee.EmployeeID = item.Id;
                    CUMEmployee.FullName = item.Fname + ' ' + item.Lname;
                    CUMEmployeeList.Add(CUMEmployee);
                }
                EXP.EmployeeList = CUMEmployeeList;
            }
            //DataTable getEmployeeLeaveRequest = DataAccessLayer.GetDataTable("call CRM_RequestLeaveList(" + BranchID + "," + CompanyID + ")");
            DataTable getExpanse = DataAccessLayer.GetDataTable("call crm_ExpenseView(" + BranchID + "," + CompanyID + ")");

            if (getExpanse.Rows.Count > 0)
            {
                List<ExpenseModel> EXPList = new List<ExpenseModel>();
                for (int i = 0; i < getExpanse.Rows.Count; i++)
                {
                    ExpenseModel EModel = new ExpenseModel();
                    EModel.EmployeeID = Convert.ToInt32(getExpanse.Rows[i]["EmployeeID"]);
                    EModel.ExpenseTypeId = Convert.ToInt32(getExpanse.Rows[i]["ExpenseTypeId"]);
                    EModel.travelledKMS = Convert.ToString(getExpanse.Rows[i]["travelledKMS"]);
                    EModel.expense = Convert.ToString(getExpanse.Rows[i]["expense"]);
                    EModel.ProcessStatus = Convert.ToString(getExpanse.Rows[i]["ProcessStatus"]);
                    EModel.RequestDate = Convert.ToString(getExpanse.Rows[i]["RequestDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                    if (!String.IsNullOrWhiteSpace(Convert.ToString(getExpanse.Rows[i]["ProcessDate"])))
                    {
                        EModel.ProcessDate = Convert.ToString(getExpanse.Rows[i]["ProcessDate"]);
                    }
                    else
                    {
                        EModel.ProcessDate = string.Empty;
                    }
                    //EModel.EmployeeCode = Convert.ToString(getExpanse.Rows[i]["EmployeeCode"]);
                    EModel.FullName = Convert.ToString(getExpanse.Rows[i]["FullName"]);
                    EXPList.Add(EModel);
                }
                EXP.ExpenseEmployeeList = EXPList.OrderByDescending(em => em.RequestDate).ToList();

                if (EXP.ExpenseEmployeeList.Count > 0)
                {
                    if (Convert.ToString(Session["UserType"]) != "SuperAdmin" && Convert.ToString(Session["UserType"]) != "")
                    {
                        Int32 UID = Convert.ToInt32(Session["UID"]);
                        EXP.ExpenseEmployeeList = EXP.ExpenseEmployeeList.Where(em => em.EmployeeID == UID).ToList();
                    }

                    if (EmployeeID > 0 && Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        EXP.ExpenseEmployeeList = EXP.ExpenseEmployeeList.Where(em => em.EmployeeID == EmployeeID).ToList();
                    }
                }
            }


            return View(EXP);
        }

    }
}