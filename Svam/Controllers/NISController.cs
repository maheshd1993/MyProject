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
using Svam.UtilityManager;
using System.Globalization;

namespace Traders.Controllers
{
    public class NISController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        #region Nis-Developer

        public ActionResult Developer()
        {
            NisDeveloperModel NDM = new NisDeveloperModel();
            if (Session["UserName"] != null)
            {
                try
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    var GetProject = db.crm_projecttbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(a => a.ProjectName).ToList().Select(em => new crm_projecttbl
                    {
                        ProjectId = em.ProjectId,
                        ProjectName = em.ProjectName
                    }).AsQueryable();
                    ViewBag.Project = new SelectList(GetProject, "ProjectId", "ProjectName");
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                }
            }
            else
            {
                Session["ReturnUrl"] = "/Nis/Developer";
                return Redirect("/home/login");
            }
            return View(NDM);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Developer(NisDeveloperModel NDM, HttpPostedFileBase file)
        {
            if (Session["UserName"] != null)
            {
                try
                {
                    if (NDM.Project != null && NDM.ProjectModule != null)
                    {
                        string CFile = null;
                        #region Upload-CodeFile
                        if (Request.Files["file"].ContentLength > 0)
                        {
                            string FileName = Path.GetFileName(file.FileName);
                            FileName = "Trd-" + Guid.NewGuid().ToString().Substring(0, 4) + "-" + FileName;
                            string Extension = Path.GetExtension(file.FileName);
                            string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                            string FilePath = Server.MapPath(FolderPath + FileName);
                            file.SaveAs(FilePath);
                            CFile = FileName;
                        }
                        #endregion

                        crm_tbl_nipldeveloper_dailyreort dailyReport = new crm_tbl_nipldeveloper_dailyreort();
                        dailyReport.UID = Convert.ToInt32(Session["UID"]);
                        dailyReport.Project_ID = Convert.ToInt32(NDM.Project);
                        dailyReport.ProjectModule_ID = Convert.ToInt32(NDM.ProjectModule);
                        dailyReport.GeneralRemark = NDM.GeneralRemark;
                        dailyReport.CodeModuleRemark = NDM.CodeModuleRemark;
                        dailyReport.DBModuleRemark = NDM.DBModuleRemark;
                        dailyReport.JsModuleRemark = NDM.JSCSSModuleRemark;
                        dailyReport.SupportNeeded = NDM.SupportNeeded;
                        dailyReport.CreatedDate = Constant.GetBharatTime().ToString("dd/MM/yyyy");
                        dailyReport.CodeFile = CFile;
                        dailyReport.status = true;
                        dailyReport.BranchID = Convert.ToInt32(Session["BranchID"]);
                        dailyReport.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                        db.crm_tbl_nipldeveloper_dailyreort.Add(dailyReport);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Today report submited successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "Please select the Project && Module Name!";
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    TempData["alert"] = "Sorry There is some problem!";
                }
            }
            else
            {
                Session["ReturnUrl"] = "/Nis/Developer";
                return Redirect("/home/login");
            }
            return View();
        }

        public ActionResult ViewActivityReport()
        {
            DeveloperActivityModel DAM = new DeveloperActivityModel();
            if (Session["UserName"] != null)
            {

                try
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    DAM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

                    #region all developer list for admin
                    //string Qry = @"select Id as uid,CONCAT(Fname ,' ', Lname) as UserName from crm_usertbl where BranchID='"+BranchID+"' and CompanyID='"+CompanyID+ "' and Status=1 and (ProfileName='Software Developer' or ProfileName='Android Developer') order by Fname";
                    //DAM.DeveloperList = db.Database.SqlQuery<Userddl>(Qry).ToList();

                    var udata = (from ur in db.crm_usertbl
                                 where ur.CompanyID == CompanyID && ur.BranchID == BranchID
                                 && ur.Status == true
                                 && (ur.ProfileName.Contains("Developer")
                                 || ur.ProfileName.Contains("Tester")
                                 || ur.ProfileName.Contains("Designer"))
                                 select new Userddl
                                 {
                                     uid = ur.Id,
                                     UserName = ur.Fname + " " + ur.Lname
                                 }).ToList();
                    DAM.DeveloperList = udata;
                    #endregion

                    #region Data-time-Formate
                    var dd = Constant.GetBharatTime();//get india datetime

                    //DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);

                    DateTime monthStartDate = dd.AddDays(-15);
                    DateTime MonthendDate = new DateTime(dd.Year, dd.Month, dd.Day);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    #region To-CheckFilter-Date
                    if (Session["VLFltrFrmDt"] != null && Session["VLFltrToDt"] != null)
                    {

                        if (DAM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            string sdate = Convert.ToString(Session["VLFltrFrmDt"]);
                            string edate = Convert.ToString(Session["VLFltrToDt"]);

                            var fmDate = DateTime.ParseExact(sdate, DAM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(Session["VLFltrFrmDt"]);
                            var tDate = DateTime.ParseExact(edate, DAM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(Session["VLFltrToDt"]);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            MStartDate = Convert.ToString(Session["VLFltrFrmDt"]);
                            MEndDate = Convert.ToString(Session["VLFltrToDt"]);

                        }
                    }
                    else
                    {
                        if (DAM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            Session["VLFltrFrmDt"] = monthStartDate.ToString(DAM.DateFormat);
                            Session["VLFltrToDt"] = MonthendDate.ToString(DAM.DateFormat);
                        }
                        else
                        {
                            Session["VLFltrFrmDt"] = MStartDate;
                            Session["VLFltrToDt"] = MEndDate;
                        }
                    }
                    #endregion

                    #endregion
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        #region Get-Data-By-Admin

                        DataTable GetAllActivityRecord = DataAccessLayer.GetDataTable("call CRM_ViewActivityReportbyUser('" + 0 + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetAllActivityRecord.Rows.Count > 0)
                        {
                            DAM.DeveloperactivityModelList = (from dr in GetAllActivityRecord.AsEnumerable()
                                                              select new DeveloperActivityModel()
                                                              {
                                                                  Id = Convert.ToInt32(dr["Id"]),
                                                                  Uid = Convert.ToInt32(dr["UID"]),
                                                                  Project_ID = Convert.ToInt64(dr["Project_ID"]),
                                                                  FullName = Convert.ToString(dr["FullName"]),
                                                                  EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                                                  ProjectModule_ID = Convert.ToInt32(dr["ProjectModule_ID"]),
                                                                  GeneralRemark = Convert.ToString(dr["GeneralRemark"]),
                                                                  CodeModuleRemark = Convert.ToString(dr["CodeModuleRemark"]),
                                                                  DBModuleRemark = Convert.ToString(dr["DBModuleRemark"]),
                                                                  JsModuleRemark = Convert.ToString(dr["JsModuleRemark"]),
                                                                  SupportNeeded = Convert.ToString(dr["SupportNeeded"]),
                                                                  CodeFile = Convert.ToString(dr["CodeFile"]),
                                                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? String.Format("{0:" + DAM.DateFormat + "}", Convert.ToDateTime(dr["CreatedDate"])) : string.Empty,
                                                                  ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                  ModuleName = Convert.ToString(dr["ModuleName"])
                                                              }).ToList();
                        }
                        #endregion

                    }
                    else
                    {
                        //var GetAllActivityRecord = db.N_ViewActivityReportbyUser(0, MStartDate, MEndDate).ToList();
                        #region Get-Data-By-User
                        var uid = Convert.ToInt32(Session["UID"]);

                        DataTable GetAllActivityRecord = DataAccessLayer.GetDataTable(" call CRM_ViewActivityReportbyUser('" + uid + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                        if (GetAllActivityRecord.Rows.Count > 0)
                        {
                            DAM.DeveloperactivityModelList = (from dr in GetAllActivityRecord.AsEnumerable()
                                                              select new DeveloperActivityModel()
                                                              {
                                                                  Id = Convert.ToInt32(dr["Id"]),
                                                                  Uid = Convert.ToInt32(dr["UID"]),
                                                                  Project_ID = Convert.ToInt64(dr["Project_ID"]),
                                                                  ProjectModule_ID = Convert.ToInt32(dr["ProjectModule_ID"]),
                                                                  FullName = Convert.ToString(dr["FullName"]),
                                                                  EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                                                  GeneralRemark = Convert.ToString(dr["GeneralRemark"]),
                                                                  CodeModuleRemark = Convert.ToString(dr["CodeModuleRemark"]),
                                                                  DBModuleRemark = Convert.ToString(dr["DBModuleRemark"]),
                                                                  JsModuleRemark = Convert.ToString(dr["JsModuleRemark"]),
                                                                  SupportNeeded = Convert.ToString(dr["SupportNeeded"]),
                                                                  CodeFile = Convert.ToString(dr["CodeFile"]),
                                                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? String.Format("{0:" + DAM.DateFormat + "}", Convert.ToDateTime(dr["CreatedDate"])) : string.Empty,
                                                                  ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                  ModuleName = Convert.ToString(dr["ModuleName"])
                                                              }).ToList();
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                }
            }
            else
            {
                return RedirectToAction("login", "home");
            }

            return View(DAM);
        }

        public ActionResult ViewDeveloperActivityRemark(Int64 ActivityId, string Remark, string RemarkColl)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                var text = "";
                if (RemarkColl == "GeneralRemark")
                {
                    text = db.crm_tbl_nipldeveloper_dailyreort.Where(em => em.Id == ActivityId && em.BranchID == BranchID && em.CompanyID == CompanyID).Select(em => em.GeneralRemark).FirstOrDefault();
                }
                else if (RemarkColl == "CodeModuleRemark")
                {
                    text = db.crm_tbl_nipldeveloper_dailyreort.Where(em => em.Id == ActivityId && em.BranchID == BranchID && em.CompanyID == CompanyID).Select(em => em.CodeModuleRemark).FirstOrDefault();
                }
                else if (RemarkColl == "DBModuleRemark")
                {
                    text = db.crm_tbl_nipldeveloper_dailyreort.Where(em => em.Id == ActivityId && em.BranchID == BranchID && em.CompanyID == CompanyID).Select(em => em.DBModuleRemark).FirstOrDefault();
                }
                else if (RemarkColl == "JsModuleRemark")
                {
                    text = db.crm_tbl_nipldeveloper_dailyreort.Where(em => em.Id == ActivityId && em.BranchID == BranchID && em.CompanyID == CompanyID).Select(em => em.JsModuleRemark).FirstOrDefault();
                }
                TempData["Remark-Description"] = text;
                TempData["Remark"] = Remark;
                //ViewBag.result = db.LeadDescriptiontbls.Where(em => em.Id == ActivityId&&em.).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_ViewDeveloperActivityRemark");
        }

        public ActionResult FilterActivityDateWisebyUser(string fromDate, string ToDate, int UserId = 0)
        {
            if (Session["UserName"] != null)
            {
                DeveloperActivityModel DAM = new DeveloperActivityModel();
                try
                {
                    #region Get-Data-By-Filter Date
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    int uid = Convert.ToInt32(Session["UID"]);
                    DAM.DateFormat = Constant.DateFormat();//get date format by company id
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        uid = 0;
                        if (UserId > 0)
                        {
                            uid = UserId;
                        }
                    }
                    var MStartDate = string.Empty;
                    var MEndDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(fromDate) && !string.IsNullOrWhiteSpace(ToDate))
                    {
                        if (DAM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var fmDate = DateTime.ParseExact(fromDate, DAM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(fromDate);
                            var tDate = DateTime.ParseExact(ToDate, DAM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                        }
                        else
                        {
                            MStartDate = fromDate;
                            MEndDate = ToDate;
                        }

                    }
                    DataTable GetAllActivityRecord = DataAccessLayer.GetDataTable(" call CRM_ViewActivityReportbyUser('" + uid + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetAllActivityRecord.Rows.Count > 0)
                    {
                        DAM.DeveloperactivityModelList = (from dr in GetAllActivityRecord.AsEnumerable()
                                                          select new DeveloperActivityModel()
                                                          {
                                                              Id = Convert.ToInt32(dr["Id"]),
                                                              Uid = Convert.ToInt32(dr["UID"]),
                                                              Project_ID = Convert.ToInt32(dr["Project_ID"]),
                                                              FullName = Convert.ToString(dr["FullName"]),
                                                              EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                                              ProjectModule_ID = Convert.ToInt32(dr["ProjectModule_ID"]),
                                                              GeneralRemark = Convert.ToString(dr["GeneralRemark"]),
                                                              CodeModuleRemark = Convert.ToString(dr["CodeModuleRemark"]),
                                                              DBModuleRemark = Convert.ToString(dr["DBModuleRemark"]),
                                                              JsModuleRemark = Convert.ToString(dr["JsModuleRemark"]),
                                                              SupportNeeded = Convert.ToString(dr["SupportNeeded"]),
                                                              CodeFile = Convert.ToString(dr["CodeFile"]),
                                                              CreatedDate = dr["CreatedDate"] != DBNull.Value ? String.Format("{0:" + DAM.DateFormat + "}", Convert.ToDateTime(dr["CreatedDate"])) : string.Empty,
                                                              ProjectName = Convert.ToString(dr["ProjectName"]),
                                                              ModuleName = Convert.ToString(dr["ModuleName"])
                                                          }).ToList();
                    }

                    #endregion

                    return PartialView("_FilterDevloperActivityDatewiseByUser", DAM);
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    return Json("error", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json("expire", JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult BindDDlProject()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            List<AddProjectModel> data = db.crm_projecttbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID && em.IsActive == true)
                                    .Select(em => new AddProjectModel
                                    {
                                        Pid = em.ProjectId,
                                        ProjectName = em.ProjectName
                                    }).OrderBy(a => a.ProjectName).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindDDlModule(int? pid)
        {
            //var data = db.SubCategories.Where(em => em.CatId == catid).ToList();
            //return Json(data, JsonRequestBehavior.AllowGet);

            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            List<AddProjectModuleModel> data = db.crm_projectmoduletbl.Where(em => em.PId == pid && em.BranchID == BranchID && em.CompanyID == CompanyID && em.IsActive == true)
                                    .Select(em => new AddProjectModuleModel
                                    {
                                        ModuleId = em.M_Id,
                                        ProjectModuleName = em.ModuleName
                                    }).OrderBy(a => a.ProjectModuleName).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Nis-Login-History
        public ActionResult LoginHistory()
        {
            EmployeeLogHistory ELH = new EmployeeLogHistory();
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    ELH.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    #region Data-time-Formate
                    var dd = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                    #region To-CheckFilter-Date
                    if (Session["VLFltrFrmDt"] != null && Session["VLFltrToDt"] != null)
                    {

                        if (ELH.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            string sdate = Convert.ToString(Session["VLFltrFrmDt"]);
                            string edate = Convert.ToString(Session["VLFltrToDt"]);

                            var fmDate = DateTime.ParseExact(sdate, ELH.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(Session["VLFltrFrmDt"]);
                            var tDate = DateTime.ParseExact(edate, ELH.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(Session["VLFltrToDt"]);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            MStartDate = Convert.ToString(Session["VLFltrFrmDt"]);
                            MEndDate = Convert.ToString(Session["VLFltrToDt"]);

                        }
                    }
                    #endregion

                    int uid = Convert.ToInt32(Session["UID"]);


                    #endregion
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        uid = 0;
                        DataTable GetLogHistory = DataAccessLayer.GetDataTable(" call CRM_GetUserLogHistory(" + uid + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
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
                                                             WorkingLateHours = Convert.ToBoolean((Convert.ToString(dr["Working_Late_Night"]) == null || Convert.ToString(dr["Working_Late_Night"]) == "") ? 0 : dr["Working_Late_Night"]),
                                                             ExtraWorking = Convert.ToBoolean((Convert.ToString(dr["Extra_working"]) == null || Convert.ToString(dr["Extra_working"]) == "") ? 0 : dr["Extra_working"]),
                                                             IPAddress = Convert.ToString(dr["IPAddress"]),
                                                             MacAddress = Convert.ToString(dr["MacAddress"]),
                                                             TimeZone = Convert.ToString(dr["LogTimeZone"])
                                                         }).OrderBy(em => em.LoginDate).ToList();
                        }

                    }
                    else
                    {
                        DataTable GetLogHistory = DataAccessLayer.GetDataTable(" call CRM_GetUserLogHistory(" + uid + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
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
                                                             WorkingLateHours = Convert.ToBoolean((Convert.ToString(dr["Working_Late_Night"]) == null || Convert.ToString(dr["Working_Late_Night"]) == "") ? 0 : dr["Working_Late_Night"]),
                                                             ExtraWorking = Convert.ToBoolean((Convert.ToString(dr["Extra_working"]) == null || Convert.ToString(dr["Extra_working"]) == "") ? 0 : dr["Extra_working"]),
                                                             IPAddress = Convert.ToString(dr["IPAddress"]),
                                                             MacAddress = Convert.ToString(dr["MacAddress"]),
                                                             TimeZone = Convert.ToString(dr["LogTimeZone"])
                                                         }).OrderByDescending(em => em.LoginDate).ToList();
                        }
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/Nis/LoginHistory";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(ELH);
        }

        public ActionResult FilterLogHistorybyUser(string FromDate, string ToDate)
        {
            EmployeeLogHistory ELH = new EmployeeLogHistory();
            try
            {
                if (Session["UID"] != null)
                {
                    int uid = 0;
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    ELH.DateFormat = Constant.DateFormat();//get date format by company id
                    #region GetLog-History

                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        uid = 0;
                    }
                    else
                    {
                        uid = Convert.ToInt32(Session["UID"]);
                    }

                    var MStartDate = string.Empty;
                    var MEndDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
                    {
                        if (ELH.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var fmDate = DateTime.ParseExact(FromDate, ELH.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, ELH.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                        }
                        else
                        {
                            MStartDate = FromDate;
                            MEndDate = ToDate;
                        }

                    }

                    DataTable GetLogHistory = DataAccessLayer.GetDataTable(" call CRM_GetUserLogHistory('" + uid + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
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
                                                         WorkingLateHours = Convert.ToBoolean((Convert.ToString(dr["Working_Late_Night"]) == null || Convert.ToString(dr["Working_Late_Night"]) == "") ? 0 : dr["Working_Late_Night"]),
                                                         ExtraWorking = Convert.ToBoolean((Convert.ToString(dr["Extra_working"]) == null || Convert.ToString(dr["Extra_working"]) == "") ? 0 : dr["Extra_working"]),
                                                         IPAddress = Convert.ToString(dr["IPAddress"]),
                                                         MacAddress = Convert.ToString(dr["MacAddress"]),
                                                         TimeZone = Convert.ToString(dr["LogTimeZone"])
                                                     }).ToList();

                    }
                    #endregion
                }
                else
                {
                    Session["ReturnUrl"] = "/Nis/LoginHistory";
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

        #region Mobile-User-Not Access
        public ActionResult Alert()
        {
            return View();
        }
        #endregion

        #region Developer-Report
        public ActionResult DeveloperReport(int id, string dt, string toDate)
        {
            DeveloperActivityModel DAM = new DeveloperActivityModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                DAM.DateFormat = Constant.DateFormat();//get date format by company id
                //var dt = Convert.ToString(Request.QueryString["dt"]);
                var MStartDate = string.Empty;
                var MEndDate = string.Empty;
                if (DAM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    var fmDate = DateTime.ParseExact(dt, DAM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(dt);
                    var tDate = DateTime.ParseExact(toDate, DAM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(toDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                }
                else
                {
                    MStartDate = dt;
                    MEndDate = toDate;
                }

                #region Get-Data-By-Admin
                var uid = Convert.ToInt32(id);
                DataTable GetAllActivityRecord = DataAccessLayer.GetDataTable(" call CRM_ViewActivityReportbyUser('" + uid + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
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
                                                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? String.Format("{0:" + DAM.DateFormat + "}", Convert.ToDateTime(dr["CreatedDate"])) : string.Empty,
                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                          ModuleName = Convert.ToString(dr["ModuleName"])
                                                      }).ToList();

                }
                #endregion

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(DAM);
        }

        //public ActionResult ViewEmpReport(int id)
        //{
        //    try
        //    {
        //        var getProfile = (Request.QueryString["po"]);
        //        var name = (Request.QueryString["name"]);
        //        var date = (Request.QueryString["dt"]);
        //        if (getProfile == "Developer")
        //        {
        //            return Redirect("/Nis/DeveloperReport/" + id + "/?dt=" + date);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);  
        //        return Redirect("/HR");
        //    }
        //    return Redirect("/HR");
        //}
        #endregion


        #region CommonUser-Remark
        public ActionResult Remark()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Remark(GeneralRemarkModel GRM)
        {
            try
            {
                if (Session["UID"] != null)
                {
                    crm_commonactivityremarktbl CAR = new crm_commonactivityremarktbl();
                    CAR.Remark = GRM.Remark;
                    CAR.UID = Convert.ToInt32(Session["UID"]);
                    CAR.Created_at = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                    CAR.Status = true;
                    CAR.BranchID = Convert.ToInt32(Session["BranchID"]);
                    CAR.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    db.crm_commonactivityremarktbl.Add(CAR);
                    if (db.SaveChanges() > 0)
                    {
                        TempData["success"] = "Remark added successfully";
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/Nis/Remark";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Nis/ViewRemark");
        }

        public ActionResult ViewRemark(int? id, string fromdate, string toDate)
        {
            CommonActivityRemarkModel CARM = new CommonActivityRemarkModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                CARM.DateFormat = Constant.DateFormat();//get date format by company id
                Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                int uid = Convert.ToInt32(Session["UID"]);
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    if (id != null)
                    {
                        uid = Convert.ToInt32(id);
                    }
                    else
                    {
                        uid = 0;
                    }
                }

                #region Data-time-Formate
                var dd = Constant.GetBharatTime();
                DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

                if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
                {

                    if (CARM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        var fmDate = DateTime.ParseExact(fromdate, CARM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(fromdate);
                        var tDate = DateTime.ParseExact(toDate, CARM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(toDate);

                        MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                        MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                    }
                    else
                    {
                        MStartDate = fromdate;
                        MEndDate = toDate;
                    }
                }
                #endregion

                DataTable getRemark = DataAccessLayer.GetDataTable("call CRM_GetCommonActivityReport('" + uid + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (getRemark.Rows.Count > 0)
                {
                    CARM.commonActivityRemarkList = (from dr in getRemark.AsEnumerable()
                                                     select new CommonActivityRemarkModel()
                                                     {
                                                         Id = Convert.ToInt32(dr["Id"]),
                                                         UID = Convert.ToInt32(dr["UID"]),
                                                         UserName = Convert.ToString(dr["UserName"]),
                                                         Profile = Convert.ToString(dr["ProfileName"]),
                                                         Email = Convert.ToString(dr["Email"]),
                                                         Remarks = Convert.ToString(dr["Remark"]),
                                                         Date = String.Format("{0:" + CARM.DateFormat + "}", Convert.ToDateTime(dr["Created_at"]))
                                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(CARM);
        }

        public ActionResult FilterCommonRemarks(int? id, string fromdate, string toDate)
        {
            CommonActivityRemarkModel CARM = new CommonActivityRemarkModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                int uid = Convert.ToInt32(Session["UID"]);
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    if (id != null)
                    {
                        uid = Convert.ToInt32(id);
                    }
                    else
                    {
                        uid = 0;
                    }
                }
                CARM.DateFormat = Constant.DateFormat();//get date format by company id
                var MStartDate = fromdate;
                var MEndDate = toDate;
                if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(toDate))
                {

                    if (CARM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        var fmDate = DateTime.ParseExact(fromdate, CARM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(fromdate);
                        var tDate = DateTime.ParseExact(toDate, CARM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(toDate);

                        MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                        MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                    }
                    else
                    {
                        MStartDate = fromdate;
                        MEndDate = toDate;
                    }
                }

                DataTable getRemark = DataAccessLayer.GetDataTable(" call CRM_GetCommonActivityReport('" + uid + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (getRemark.Rows.Count > 0)
                {
                    CARM.commonActivityRemarkList = (from dr in getRemark.AsEnumerable()
                                                     select new CommonActivityRemarkModel()
                                                     {
                                                         Id = Convert.ToInt32(dr["Id"]),
                                                         UID = Convert.ToInt32(dr["UID"]),
                                                         UserName = Convert.ToString(dr["UserName"]),
                                                         Profile = Convert.ToString(dr["ProfileName"]),
                                                         Email = Convert.ToString(dr["Email"]),
                                                         Remarks = Convert.ToString(dr["Remark"]),
                                                         Date = String.Format("{0:" + CARM.DateFormat + "}", Convert.ToDateTime(dr["Created_at"]))
                                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_FilterCommonActivityRemark", CARM);
        }
        #endregion

        public JsonResult BindDDlCustomer()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            // List<AddCustomerModel> data = db.crm_createleadstbl.Where(em => em.LeadStatus.Equals("Closed") && em.BranchID == BranchID && em.CompanyID == CompanyID)
            List<AddCustomerModel> data = db.crm_createleadstbl.Where(em => em.LeadStatus == "Closed" && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderByDescending(em=>em.Createddate)
            .Select(em => new AddCustomerModel
            {
                CId = em.Id,
                CustomerName = em.Customer
            }).OrderBy(a => a.CustomerName).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
