using System;
using System.Linq;
using System.Web.Mvc;
using Traders.Models;
using Svam.EF;
using System.Data;
using Svam.Models;
using System.Collections.Generic;
using Svam.UtilityManager;
using System.Globalization;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Traders.Controllers
{
    public class DailyworkController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        #region Assing-Work
        public ActionResult Assign(string FromDate, string ToDate, string assignProject, string assignProjectModule, string assignCustomer)
        {
            DailyworkAssignDeveloperModel DWADM = new DailyworkAssignDeveloperModel();
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    DWADM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

                    #region Get-Today-Work
                    string PWeekDate = "";
                    string TodayDate = "";
                    DateTime bharatTime = Constant.GetBharatTime();//get india datetime
                    var dd = bharatTime;
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    PWeekDate = monthStartDate.ToString("dd/MM/yyyy");
                    TodayDate = MonthendDate.ToString("dd/MM/yyyy");
                    Session["VLFltrFrmDt"] = PWeekDate;
                    Session["VLFltrToDt"] = TodayDate;
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        //PWeekDate = FromDate;
                        //TodayDate = ToDate;

                        if (DWADM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var fmDate = DateTime.ParseExact(FromDate, DWADM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, DWADM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                            PWeekDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            TodayDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                            Session["VLFltrFrmDt"] = PWeekDate;
                            Session["VLFltrToDt"] = TodayDate;
                        }
                        else
                        {
                            Session["VLFltrFrmDt"] = FromDate;
                            Session["VLFltrToDt"] = ToDate;
                            PWeekDate = FromDate;
                            TodayDate = ToDate;
                        }
                    }

                    int UID = 0;
                    int projectId = Convert.ToInt32(Session["ProjectId"]);
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        UID = 0;


                    }
                    else
                    {
                        // UID = 0;
                        UID = Convert.ToInt32(Session["UID"]);

                    }



                    string assignquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName ,ur.EmployeeCode
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ProjectManagement = 1";
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
                        DWADM.AssignToList = AssignList.Where(em => em.UserID != UID).OrderBy(a => a.UserName).ToList();
                    }


                    if (assignProject == null && assignProjectModule == null  && assignCustomer == null )
                    {
                        DataTable getWork = DataAccessLayer.GetDataTable(" call CRM_GetAddWork(" + UID + ",'" + PWeekDate + "','" + TodayDate + "'," + BranchID + "," + CompanyID + ",'" + assignProject + "','" + assignProjectModule + "','" + assignCustomer + "')");
                        if (getWork.Rows.Count > 0)
                        {
                            DWADM.dailyworkAssignDelevelopermodelList = (from dr in getWork.AsEnumerable()
                                                                         select new DailyworkAssignDeveloperModel()
                                                                         {
                                                                             Id = Convert.ToInt32(dr["Id"]),
                                                                             WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                             ProjectId = Convert.ToInt32(dr["ProjectId"]),
                                                                             ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                             ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                                                             ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                             ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                             FinishingDate = Convert.ToDateTime(dr["FinishDate"]).ToShortDateString(),
                                                                             UserName = Convert.ToString(dr["UserName"]),
                                                                             CustomerName = Convert.ToString(dr["Customer"]),
                                                                             CustomerId = Convert.ToString(dr["CustomerName"]),
                                                                             //AssignedTo = Convert.ToString(dr["WorkAssignUser"]),
                                                                             AssignedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                         }).ToList();
                        }
                    }
                   else if (assignProject == "Select Project" && assignCustomer == "Select Customer")
                    {
                        DataTable getWork = DataAccessLayer.GetDataTable(" call CRM_GetAddWork(" + UID + ",'" + PWeekDate + "','" + TodayDate + "'," + BranchID + "," + CompanyID + ",'" + assignProject + "','" + assignProjectModule + "','" + assignCustomer + "')");
                        if (getWork.Rows.Count > 0)
                        {
                            DWADM.dailyworkAssignDelevelopermodelList = (from dr in getWork.AsEnumerable()
                                                                         select new DailyworkAssignDeveloperModel()
                                                                         {
                                                                             Id = Convert.ToInt32(dr["Id"]),
                                                                             WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                             ProjectId = Convert.ToInt32(dr["ProjectId"]),
                                                                             ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                             ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                                                             ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                             ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                             FinishingDate = Convert.ToDateTime(dr["FinishDate"]).ToShortDateString(),
                                                                             UserName = Convert.ToString(dr["UserName"]),
                                                                             CustomerName = Convert.ToString(dr["Customer"]),
                                                                             CustomerId = Convert.ToString(dr["CustomerName"]),
                                                                             //AssignedTo = Convert.ToString(dr["WorkAssignUser"]),
                                                                             AssignedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                         }).ToList();
                        }
                    }
                    else if (assignProject == "" && assignCustomer == "")
                    {
                        DataTable getWork = DataAccessLayer.GetDataTable(" call CRM_GetAddWork(" + UID + ",'" + PWeekDate + "','" + TodayDate + "'," + BranchID + "," + CompanyID + ",'" + assignProject + "','" + assignProjectModule + "','" + assignCustomer + "')");
                        if (getWork.Rows.Count > 0)
                        {
                            DWADM.dailyworkAssignDelevelopermodelList = (from dr in getWork.AsEnumerable()
                                                                         select new DailyworkAssignDeveloperModel()
                                                                         {
                                                                             Id = Convert.ToInt32(dr["Id"]),
                                                                             WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                             ProjectId = Convert.ToInt32(dr["ProjectId"]),
                                                                             ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                             ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                                                             ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                             ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                             FinishingDate = Convert.ToDateTime(dr["FinishDate"]).ToShortDateString(),
                                                                             UserName = Convert.ToString(dr["UserName"]),
                                                                             CustomerName = Convert.ToString(dr["Customer"]),
                                                                             CustomerId = Convert.ToString(dr["CustomerName"]),
                                                                             //AssignedTo = Convert.ToString(dr["WorkAssignUser"]),
                                                                             AssignedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                         }).ToList();
                        }
                    }
                    else if (assignCustomer != null && assignCustomer != "")
                    {
                        DataTable getWork = DataAccessLayer.GetDataTable(" call CRM_GetAddWorkCustomer('" + assignCustomer + "')");
                        if (getWork.Rows.Count > 0)
                        {
                            DWADM.dailyworkAssignDelevelopermodelList = (from dr in getWork.AsEnumerable()
                                                                         select new DailyworkAssignDeveloperModel()
                                                                         {
                                                                             Id = Convert.ToInt32(dr["Id"]),
                                                                             WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                             ProjectId = Convert.ToInt32(dr["ProjectId"]),
                                                                             ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                             ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                                                             ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                             ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                             FinishingDate = Convert.ToDateTime(dr["FinishDate"]).ToShortDateString(),
                                                                             UserName = Convert.ToString(dr["UserName"]),
                                                                             CustomerName = Convert.ToString(dr["Customer"]),
                                                                             CustomerId = Convert.ToString(dr["CustomerName"]),
                                                                             //AssignedTo = Convert.ToString(dr["WorkAssignUser"]),
                                                                             AssignedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                         }).ToList();
                        }
                    }

                    else if (assignProject != null && assignProject != "")
                    {
                        DataTable getWork = DataAccessLayer.GetDataTable(" call CRM_GetAddWorkproject('" + assignProject + "')");
                        if (getWork.Rows.Count > 0)
                        {
                            DWADM.dailyworkAssignDelevelopermodelList = (from dr in getWork.AsEnumerable()
                                                                         select new DailyworkAssignDeveloperModel()
                                                                         {
                                                                             Id = Convert.ToInt32(dr["Id"]),
                                                                             WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                             ProjectId = Convert.ToInt32(dr["ProjectId"]),
                                                                             ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                             ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                                                             ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                             ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                             FinishingDate = Convert.ToDateTime(dr["FinishDate"]).ToShortDateString(),
                                                                             UserName = Convert.ToString(dr["UserName"]),
                                                                             CustomerName = Convert.ToString(dr["Customer"]),
                                                                             CustomerId = Convert.ToString(dr["CustomerName"]),
                                                                             //AssignedTo = Convert.ToString(dr["WorkAssignUser"]),
                                                                             AssignedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                         }).ToList();
                        }
                    }
                    else if (assignProject != null && assignProjectModule != null)
                    {
                        DataTable getWork = DataAccessLayer.GetDataTable(" call CRM_GetAddWorkModule('" + assignProject + "','" + assignProjectModule + "')");
                        if (getWork.Rows.Count > 0)
                        {
                            DWADM.dailyworkAssignDelevelopermodelList = (from dr in getWork.AsEnumerable()
                                                                         select new DailyworkAssignDeveloperModel()
                                                                         {
                                                                             Id = Convert.ToInt32(dr["Id"]),
                                                                             WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                             ProjectId = Convert.ToInt32(dr["ProjectId"]),
                                                                             ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                             ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                                                             ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                             ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                             FinishingDate = Convert.ToDateTime(dr["FinishDate"]).ToShortDateString(),
                                                                             UserName = Convert.ToString(dr["UserName"]),
                                                                             CustomerName = Convert.ToString(dr["Customer"]),
                                                                             CustomerId = Convert.ToString(dr["CustomerName"]),
                                                                             //AssignedTo = Convert.ToString(dr["WorkAssignUser"]),
                                                                             AssignedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                         }).ToList();
                        }
                    }
                    #endregion
                }
                else
                {
                    Session["ReturnUrl"] = "/Dailywork/Assign";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return View(DWADM);
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

        public JsonResult assignDDlCustomer()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            //List<AddCustomerModel> data = db.crm_createleadstbl.Where(em => em.LeadStatus.Equals("Closed") && em.BranchID == BranchID && em.CompanyID == CompanyID)
            List<AddCustomerModel> data = db.crm_createleadstbl.Where(em => em.LeadStatus == "Closed" && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderByDescending(em => em.Createddate)
            .Select(em => new AddCustomerModel
            {
                CId = em.Id,
                CustomerName = em.Customer
            }).OrderBy(a => a.CustomerName).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AssignToUser(int? AssignTo)
        {
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    if (AssignTo > 0)
                    {
                        #region calculate the Assigned Works
                        string AssignedWorklist = null;
                        if (Request.Form["CalUserWorks"] != null)
                        {
                            int AssignCount = 0;
                            AssignedWorklist = Request.Form["CalUserWorks"].ToString();
                            var spliteAssignedWorklist = AssignedWorklist.Split(',');
                            foreach (var item in spliteAssignedWorklist)
                            {
                                int id = Convert.ToInt32(item);
                                var getdata = db.crm_workassigntbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                if (getdata != null)
                                {
                                    getdata.WorkAssignTo = AssignTo;
                                    getdata.WorkAssignDate = Constant.GetBharatTime().Date;
                                    getdata.WorkAssignBy = Convert.ToInt32(Session["UID"]);
                                    db.SaveChanges();
                                    AssignCount++;
                                }
                            }

                            if (AssignCount > 0)
                            {
                                TempData["success"] = "Work Assigned Succesfully";
                            }
                        }
                        else
                        {
                            TempData["alert"] = "Please select the work to assign the user";
                        }
                        #endregion
                    }
                    else
                    {
                        TempData["alert"] = "Please select the user to assign the work";
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/Dailywork/Assign";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Dailywork/ViewAssigned");
        }

        public ActionResult Addwork(string WorkDate, string Project, string Module, string ModuleDescription, string FinishDate, string CustomerName)
        {
            string msg = "";
            try
            {
                if (Session["UID"] != null)
                {
                    var DateFormat = Constant.DateFormat();//get date format by company id
                    crm_workassigntbl wat = new crm_workassigntbl();

                    wat.UID = Convert.ToInt32(Session["UID"]);
                    wat.WorkCreateDate = DateTime.ParseExact(WorkDate, DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(WorkDate);                   
                    wat.ProjectId = Convert.ToInt32(Project);
                    wat.ModuleId = Convert.ToInt32(Module);
                    wat.ModuleDescription = ModuleDescription.Trim();
                    wat.FinishDate = DateTime.ParseExact(FinishDate, DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(WorkDate);                                                         
                    wat.Status = true;
                    wat.BranchID = Convert.ToInt32(Session["BranchID"]);
                    wat.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    wat.CustomerName = CustomerName;
                    // wat.ManualCustomerName = ManualCustomer;

                    db.crm_workassigntbl.Add(wat);
                    if (db.SaveChanges() > 0)
                    {
                        msg = "Work added successfully";
                    }
                    else
                    {
                        msg = "Sorry there is some problem. Try again later.";
                    }
                }
                else
                {
                    msg = "Sorry! your session has expire. Please try again after login.";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "Sorry there is some problem. Try again later.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult BindDDlAssignto()
        //{
        //DailyworkAssignDeveloperModel DAD = new DailyworkAssignDeveloperModel();

        //Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //Int32 UID = Convert.ToInt32(Session["UID"]);

        //if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
        //{
        //    DAD.AssignToList = new List<CreateUserModel>();
        //    //var AssignList = db.crm_usertbl.Where(em => em.CompanyID == LoggedCompanyId && em.BranchID == LoggedBranchId && em.ProfileName.ToLower().Contains("sales") && em.Status == true).OrderBy(em => em.Fname).ToList();
        //    string assignquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName 
        //                    from crm_usertbl ur
        //                    join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
        //                    Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ProjectManagement = 1";
        //    var AssignList = db.Database.SqlQuery<CreateUserModel>(assignquery).ToList();
        //    if (AssignList != null && AssignList.Count() > 0)
        //    {
        //        //List<CreateUserModel> assignToList = new List<CreateUserModel>();
        //        //foreach (var item in AssignList)
        //        //{
        //        //    CreateUserModel CRM = new CreateUserModel();
        //        //    CRM.UserID = item.Id;

        //        //    CRM.UserName = item.Fname + ' ' + item.Lname;
        //        //    assignToList.Add(CRM);
        //        //}
        //        DAD.AssignToList = AssignList.Where(em => em.UserID != UID).OrderBy(a => a.UserName).ToList();
        //    }

        //    //var GetuserList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id != UID && em.Status == true).ToList();
        //    //if (GetuserList.Count > 0)
        //    //{
        //    //    List<MapUserModel> mumList = new List<MapUserModel>();
        //    //    foreach (var item in GetuserList)
        //    //    {
        //    //        MapUserModel mUM = new MapUserModel();
        //    //        mUM.Id = item.Id;
        //    //        mUM.UserName = item.Fname + " " + item.Lname;
        //    //        mumList.Add(mUM);
        //    //    }
        //    //    MUM.mapuserlist = mumList;
        //    //}
        //}
        //else
        //{
        //    var GetData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //    if (GetData != null)
        //    {
        //        if (GetData.MappedUsers != null)
        //        {
        //            var MapUserList = GetData.MappedUsers.Split(',');
        //            foreach (var item in MapUserList)
        //            {
        //                var ID = Convert.ToInt32(item);
        //                var GetUserDetails = db.crm_usertbl.Where(em => em.Id == ID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //                var cal = new MapUserModel
        //                {
        //                    Id = GetUserDetails.Id,
        //                    UserName = GetUserDetails.Fname + " " + GetUserDetails.Lname
        //                };
        //                MUM.mapuserlist.Add(cal);
        //            }
        //        }
        //    }
        //}

        ////var GetUser = db.Usertbls.Where(em => em.Id == UID).FirstOrDefault();
        ////List<MapUserModel> data = db.Usertbls.Where(em => em.Id == UID).Where(em => em.Status == true)
        ////                        .Select(em => new MapUserModel
        ////                        {
        ////                            Pid = em.ProjectId,
        ////                            ProjectName = em.ProjectName
        ////                        }).ToList();

        //return Json(MUM.mapuserlist.ToList(), JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region View-Assigned-Work

        public ActionResult ViewAssigned(string FromDate, string ToDate, string assignProject, string assignProjectModule, string assignCustomer)
        {
            ViewworkAssignDeveloperModel VAwDM = new ViewworkAssignDeveloperModel();
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    VAwDM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    string PWeekDate = "";
                    string TodayDate = "";
                    DateTime bharatTime = Constant.GetBharatTime();//get india datetime
                    var dd = bharatTime;
                    DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                    DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                    PWeekDate = monthStartDate.ToString("dd/MM/yyyy");
                    TodayDate = MonthendDate.ToString("dd/MM/yyyy");
                    Session["VLFltrFrmDt"] = PWeekDate;
                    Session["VLFltrToDt"] = TodayDate;
                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        //PWeekDate = FromDate;
                        //TodayDate = ToDate;

                        if (VAwDM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            var fmDate = DateTime.ParseExact(FromDate, VAwDM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, VAwDM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                            PWeekDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            TodayDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                            Session["VLFltrFrmDt"] = PWeekDate;
                            Session["VLFltrToDt"] = TodayDate;
                        }
                        else
                        {
                            Session["VLFltrFrmDt"] = FromDate;
                            Session["VLFltrToDt"] = ToDate;
                            PWeekDate = FromDate;
                            TodayDate = ToDate;
                        }
                    }
                    int UID = 0;
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        UID = 0;
                        //UID = Convert.ToInt32(Session["UID"]);

                    }
                    else
                    {
                        //UID = 0;
                        UID = Convert.ToInt32(Session["UID"]);
                    }

                    //if (FromDate != null && ToDate != null)
                    //{
                    //    DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_ViewAssignedfilterdate(" + FromDate + ",'" + ToDate + "')");
                    //    if (getAssignedWork.Rows.Count > 0)
                    //    {
                    //        var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                    //        VAwDM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                    //                                                  select new ViewworkAssignDeveloperModel()
                    //                                                  {
                    //                                                      Id = Convert.ToInt32(dr["Id"]),
                    //                                                      WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                    //                                                      ProjectName = Convert.ToString(dr["ProjectName"]),
                    //                                                      ModuleName = Convert.ToString(dr["ModuleName"]),
                    //                                                      CustomerName = Convert.ToString(dr["Customer"]),
                    //                                                      ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                    //                                                      FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                    //                                                      WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                    //                                                      WorkCreatedBy = Convert.ToString(dr["WorkCreatedBy"]),
                    //                                                      AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                    //                                                      WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                    //                                                      TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                    //                                                  }).ToList();
                    //    }
                    //}

                    if (assignProject == null && assignProjectModule == null && assignCustomer == null)
                    {
                        DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_ViewAssignedwork(" + UID + ",'" + PWeekDate + "','" + TodayDate + "'," + BranchID + "," + CompanyID + ",'" + assignProject + "','" + assignProjectModule + "','" + assignCustomer + "')");
                        if (getAssignedWork.Rows.Count > 0)
                        {
                            var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                            VAwDM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                                                                      select new ViewworkAssignDeveloperModel()
                                                                      {
                                                                          Id = Convert.ToInt32(dr["Id"]),
                                                                          WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                          ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                          CustomerName = Convert.ToString(dr["Customer"]),
                                                                          ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                          FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                                                                          WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                                                                          WorkCreatedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                          AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                                                                          WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                                                                          TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                                                                      }).ToList();
                        }
                    }
                    else if (assignProject == ""  && assignCustomer == "")
                    {
                        DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_ViewAssignedwork(" + UID + ",'" + PWeekDate + "','" + TodayDate + "'," + BranchID + "," + CompanyID + ",'" + assignProject + "','" + assignProjectModule + "','" + assignCustomer + "')");
                        if (getAssignedWork.Rows.Count > 0)
                        {
                            var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                            VAwDM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                                                                      select new ViewworkAssignDeveloperModel()
                                                                      {
                                                                          Id = Convert.ToInt32(dr["Id"]),
                                                                          WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                          ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                          CustomerName = Convert.ToString(dr["Customer"]),
                                                                          ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                          FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                                                                          WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                                                                          WorkCreatedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                          AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                                                                          WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                                                                          TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                                                                      }).ToList();
                        }
                    }
                    else if (assignProject == "Select Project" && assignCustomer == "Select Customer")
                    {
                        DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_ViewAssignedwork(" + UID + ",'" + PWeekDate + "','" + TodayDate + "'," + BranchID + "," + CompanyID + ",'" + assignProject + "','" + assignProjectModule + "','" + assignCustomer + "')");
                        if (getAssignedWork.Rows.Count > 0)
                        {
                            var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                            VAwDM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                                                                      select new ViewworkAssignDeveloperModel()
                                                                      {
                                                                          Id = Convert.ToInt32(dr["Id"]),
                                                                          WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                          ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                          CustomerName = Convert.ToString(dr["Customer"]),
                                                                          ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                          FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                                                                          WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                                                                          WorkCreatedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                          AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                                                                          WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                                                                          TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                                                                      }).ToList();
                        }
                    }
                    else if (assignCustomer != null && assignCustomer != "")
                    {
                        DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_ViewAssignedCutomer('" + assignCustomer + "')");
                        if (getAssignedWork.Rows.Count > 0)
                        {
                            var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                            VAwDM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                                                                      select new ViewworkAssignDeveloperModel()
                                                                      {
                                                                          Id = Convert.ToInt32(dr["Id"]),
                                                                          WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                          ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                          CustomerName = Convert.ToString(dr["Customer"]),
                                                                          ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                          FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                                                                          WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                                                                          WorkCreatedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                          AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                                                                          WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                                                                          TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                                                                      }).ToList();
                        }
                    }
                    else if (assignProject != null && assignProject != "")
                    {
                        DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_ViewAssignedProject('" + assignProject + "')");
                        if (getAssignedWork.Rows.Count > 0)
                        {
                            var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                            VAwDM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                                                                      select new ViewworkAssignDeveloperModel()
                                                                      {
                                                                          Id = Convert.ToInt32(dr["Id"]),
                                                                          WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                          ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                          CustomerName = Convert.ToString(dr["Customer"]),
                                                                          ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                          FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                                                                          WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                                                                          WorkCreatedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                          AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                                                                          WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                                                                          TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                                                                      }).ToList();
                        }
                    }
                    else if (assignProject != null && assignProjectModule != null)
                    {
                        DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_ViewAssignedModule('" + assignProject + "','" + assignProjectModule + "')");
                        if (getAssignedWork.Rows.Count > 0)
                        {
                            var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                            VAwDM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                                                                      select new ViewworkAssignDeveloperModel()
                                                                      {
                                                                          Id = Convert.ToInt32(dr["Id"]),
                                                                          WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                          ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                          ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                          CustomerName = Convert.ToString(dr["Customer"]),
                                                                          ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                          FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                                                                          WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                                                                          WorkCreatedBy = Convert.ToString(dr["WorkCreatedBy"]),
                                                                          AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                                                                          WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                                                                          TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VAwDM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                                                                      }).ToList();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return View(VAwDM);
        }

        public ActionResult GetDailyWork(Int32? ID)
        {
            ViewworkAssignDeveloperModel VADM = new ViewworkAssignDeveloperModel();
            VADM.DateFormat = Constant.DateFormat();//get date format by company id
            DataTable getAssignedWork = DataAccessLayer.GetDataTable("call CRM_ViewWorkAssigned(" + ID + ")");
            if (getAssignedWork.Rows.Count > 0)
            {
                VADM.Id = Convert.ToInt32(getAssignedWork.Rows[0]["ID"]);
                VADM.ProjectName = Convert.ToString(getAssignedWork.Rows[0]["ProjectName"]);
                VADM.ModuleName = Convert.ToString(getAssignedWork.Rows[0]["ModuleName"]);
                VADM.ModuleDescription = Convert.ToString(getAssignedWork.Rows[0]["ModuleDescription"]);
                VADM.WorkCreatedDate = getAssignedWork.Rows[0]["WorkCreateDate"] != DBNull.Value ? String.Format("{0:" + VADM.DateFormat + "}", Convert.ToDateTime(getAssignedWork.Rows[0]["WorkCreateDate"])) : null;
                VADM.FinishingDate = getAssignedWork.Rows[0]["FinishDate"] != DBNull.Value ? String.Format("{0:" + VADM.DateFormat + "}", Convert.ToDateTime(getAssignedWork.Rows[0]["FinishDate"])) : null;
                VADM.WorkAssignTo = Convert.ToString(getAssignedWork.Rows[0]["WorkAssignTo"]);
                VADM.OtherUserID = Convert.ToInt32(Session["UID"]);
                if (getAssignedWork.Rows[0]["WorkStatus"] != DBNull.Value)
                {
                    VADM.WorkStatus = Convert.ToInt32(getAssignedWork.Rows[0]["WorkStatus"]);
                }
                else
                {
                    VADM.WorkStatus = 0;
                }
            }
            return Json(VADM, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult UpdateWork(string workID, string WorkStatus, string Des, string workattachment)
        //{
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateWork()
        {
            WorkDescrpitonModel DWAA = new WorkDescrpitonModel();
            var updatework = new crm_workdescriptiontbl();

            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            DateTime localTime = Constant.GetBharatTime();
            long? workID = Convert.ToInt64(Request.Form[0]);
            string WorkStatus = Convert.ToString(Request.Form[1]).TrimEnd();
            string Description = Convert.ToString(Request.Form[2]).TrimEnd();
            string msg = "";


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
                        FileName = "Work-" + Convert.ToString(Session["UserName"]) + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                        FileFullName = FileName + extension;
                        string _path = Server.MapPath("~/ProjectAttachment/" + FileName + extension);
                        Postfile.SaveAs(_path);
                    }
                    else
                    {
                        msg = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";

                    }
                }
            }
            try
            {
                if (WorkStatus != string.Empty)
                {
                    //int ID = Convert.ToInt32(workID);
                    updatework = await db.crm_workdescriptiontbl.FindAsync(workID);
                    updatework = db.crm_workdescriptiontbl.Where(em => em.BranchId == BranchID && em.CompanyId == CompanyID && em.Id == workID).FirstOrDefault();

                    crm_workdescriptiontbl WD = new crm_workdescriptiontbl();
                    WD.WorkId = workID;
                    WD.WorkStatus = Convert.ToInt32(WorkStatus);
                    WD.Description = Description;
                    WD.WorkAttachment = FileFullName;
                    WD.BranchId = BranchID;
                    WD.CompanyId = CompanyID;
                    db.crm_workdescriptiontbl.Add(WD);



                    Int32? ID = Convert.ToInt32(workID);
                    var updateWork = db.crm_workassigntbl.Where(em => em.Id == ID).FirstOrDefault();
                    if (updateWork != null)
                    {
                        updateWork.WorkStatus = Convert.ToInt32(WorkStatus);
                        if (Convert.ToInt32(WorkStatus) == 1)
                        {
                            updateWork.WorkCompletedDate = Constant.GetBharatTime();
                            updateWork.WorkCompletedBy = Convert.ToInt32(Session["UID"]);
                        }
                        db.SaveChanges();

                    }
                    msg = "Work updated successfully";

                }
                else
                {
                    msg = "Sorry there is some problem. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        #endregion
        #region View all description of a work
        public async Task<ActionResult> ViewWorkDescription(Int64 wid)
        {
            WorkdescModel DModel = new WorkdescModel();
            try
            {

                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                int UserId = Convert.ToInt32(Session["UID"]);
                var GetLeadsData = new crm_workdescriptiontbl();
                //DModel.DateFormat = Constant.DateFormat();//get date format by company id                

                //if (CompanyID == 296)//view lead description for smart capita admin or users
                //{
                GetLeadsData = await db.crm_workdescriptiontbl.FindAsync(wid);
                //ViewBag.UserName = GetLeadsData.Customer;
                List<crm_workdescriptiontbl> descList = await db.crm_workdescriptiontbl.Where(em => em.WorkId == wid).OrderByDescending(em => em.Id).ToListAsync();
                DModel.desclist = descList;
                //}
                //else
                //{
                //if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                //{
                //    GetLeadsData = await db.crm_workdescriptiontbl.FindAsync(wid);
                //}                 

                //if (GetLeadsData != null)
                //{
                //    //ViewBag.UserName = GetLeadsData.Customer;
                //    List<crm_workdescriptiontbl> descList = await db.crm_workdescriptiontbl.Where(em => em.WorkId == wid && em.CompanyId == CompanyID && em.BranchId == BranchID).OrderByDescending(em => em.Id).ToListAsync();
                //    DModel.desclist = descList;
                //}

                //}

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialworkDescription", DModel);
        }
        #endregion


        #region Project Assign
        public ActionResult ProjectAssign(string AssignTo, string assignProject, string assignProjectModule, string assignCustomer)
        {
            ViewworkAssignDeveloperModel VADM = new ViewworkAssignDeveloperModel();
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    //int AssignToUser = Convert.ToInt32(AssignTo);
                    int UID = 0;
                    int projectId = Convert.ToInt32(Session["ProjectId"]);
                    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                    {
                        UID = 0;

                    }
                    else
                    {
                        UID = Convert.ToInt32(Session["UID"]);
                    }

                    DataTable getAssignedWork = DataAccessLayer.GetDataTable(" call CRM_AssignToUser('" + AssignTo + "','" + assignProject + "','" + assignProjectModule + "','" + assignCustomer + "')");
                    if (getAssignedWork.Rows.Count > 0)
                    {
                        var TodatDate = Constant.GetBharatTime().Date.ToString("dd/MM/yyyy");
                        VADM.ViewworkAssignDevelopermodelList = (from dr in getAssignedWork.AsEnumerable()
                                                                 select new ViewworkAssignDeveloperModel()
                                                                 {
                                                                     Id = Convert.ToInt32(dr["Id"]),
                                                                     WorkCreatedDate = Convert.ToDateTime(dr["WorkCreateDate"]).ToShortDateString(),
                                                                     ProjectName = Convert.ToString(dr["ProjectName"]),
                                                                     ModuleName = Convert.ToString(dr["ModuleName"]),
                                                                     CustomerName = Convert.ToString(dr["Customer"]),
                                                                     ModuleDescription = Convert.ToString(dr["ModuleDescription"]),
                                                                     //FinishingDate = dr["FinishDate"] != DBNull.Value ? String.Format("{0:" + VADM.DateFormat + "}", Convert.ToDateTime(dr["FinishDate"])) : null,
                                                                     FinishingDate = Convert.ToDateTime(dr["FinishDate"]).ToShortDateString(),
                                                                     WorkAssignedUser = Convert.ToString(dr["WorkAssignUser"]),
                                                                     //AssignDate = dr["WorkAssignDate"] != DBNull.Value ? String.Format("{0:" + VADM.DateFormat + "}", Convert.ToDateTime(dr["WorkAssignDate"])) : null,
                                                                     AssignDate = Convert.ToDateTime(dr["WorkAssignDate"]).ToShortDateString(),
                                                                     //WorkStatusName = Convert.ToString(dr["WorkStatusName"]),
                                                                     CancelStatus = Convert.ToInt32(dr["CancelStatus"]),
                                                                     TaskCompletedDate = dr["WorkCompletedDate"] != DBNull.Value ? String.Format("{0:" + VADM.DateFormat + "}", Convert.ToDateTime(dr["WorkCompletedDate"])) : null
                                                                 }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return View(VADM);
        }
        #endregion

        #region changeCancelstatus
        public JsonResult changeCancelstatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_workassigntbl set CancelStatus=case when CancelStatus=1 then 0 else 1 end where Id=" + id);
                msg = "ok";
            }
            catch (Exception ex)
            {
                //Models.ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public FileResult ProjectDownload(string PostFile)
        {
            var filepath = System.IO.Path.Combine(Server.MapPath("~/ProjectAttachment/"), PostFile);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), PostFile);
        }

        #region Our-Work

        public ActionResult ourwork()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View();
        }

        #endregion
    }
}
