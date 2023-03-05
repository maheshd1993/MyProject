using System;
using System.Linq;
using System.Web.Mvc;
using Svam.EF;
using Traders.Models;
using Svam.Models;

namespace Traders.Controllers
{
    public class projectController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        #region Add-Project-Name
        public ActionResult add(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (Session["UserName"] != null)
            {
                AddProjectModel APM = new AddProjectModel();
                try
                {
                    if (id != null)
                    { 
                        var getProjectData = db.crm_projecttbl.Where(em => em.ProjectId == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (getProjectData != null)
                        {
                            APM.ProjectName = getProjectData.ProjectName;
                        }
                    }
                    ViewBag.result = db.crm_projecttbl.Where(em => em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(a=>a.ProjectName).ToList();
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                }
                return View(APM);
            }
            else
            {
                Session["ReturnUrl"] = "/project/add";
                return Redirect("/home/login");
            }
        }

        [HttpPost]
        public ActionResult add(AddProjectModel APM,int? id)
        {
            try
            {
                if (Session["UserName"] != null)
                {
                    if (id != null)
                    {
                        Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                        Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                        var getProjectData = db.crm_projecttbl.Where(em => em.ProjectId == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (getProjectData != null)
                        {
                            getProjectData.ProjectName = APM.ProjectName.Trim();
                            getProjectData.BranchID = Convert.ToInt32(Session["BranchID"]);
                            getProjectData.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                            db.SaveChanges();
                            TempData["success"] = "Project Name updated successfully";
                        }
                    }
                    else
                    {
                        crm_projecttbl PT = new crm_projecttbl();
                        PT.ProjectName = APM.ProjectName.Trim();
                        PT.BranchID = Convert.ToInt32(Session["BranchID"]);
                        PT.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                        PT.Status = true;
                        PT.Created_Date = System.DateTime.Now;
                        db.crm_projecttbl.Add(PT);
                        db.SaveChanges();
                        TempData["success"] = "Project Name added successfully";
                    }                    
                }
                else
                {
                    TempData["alert"] = "Sorry There is some problem!";         
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);  
            }
            return Redirect("/project/add");
        }

        #endregion
        #region changestatus
        public JsonResult ChangeStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_projecttbl set IsActive=case when IsActive=1 then 0 else 1 end where ProjectId=" + id);
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

        public JsonResult ChangeStatusModel(string pid,string mid)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_projectmoduletbl set IsActive=case when IsActive=1 then 0 else 1 end where M_Id='" + mid + "' and PId='" + pid + "'");
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
        #region Add-ProjectModule-Name
        public ActionResult AddModule()
        {
            if (Session["UserName"] != null)
            {
                AddProjectModuleModel APMM = new AddProjectModuleModel();
                try
                {
                    if (Request.QueryString["pid"] != null)
                    {
                        Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                        Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                        int Pid = Convert.ToInt32(Request.QueryString["pid"]);
                        #region Get-ProductName
                        var ProjectName = db.crm_projecttbl.Where(em => em.ProjectId == Pid && em.BranchID == BranchID && em.CompanyID == CompanyID).Select(em => em.ProjectName).FirstOrDefault();
                        if (ProjectName != null)
                        {
                            TempData["Project-Name"] = ProjectName;
                            APMM.ProjectId = Pid;
                        }
                        else
                        {
                            return Redirect("/project/add");
                        }

                        #endregion
                        if (Request.QueryString["mid"] != null)
                        {
                            int mid = Convert.ToInt32(Request.QueryString["mid"]);
                            var getProjectData = db.crm_projectmoduletbl.Where(em => em.M_Id == mid && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (getProjectData != null)
                            {
                                APMM.ProjectModuleName = getProjectData.ModuleName;
                                APMM.ModuleId = mid;
                            }
                        }
                        ViewBag.result = db.crm_projectmoduletbl.Where(em => em.PId == Pid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                    }
                    else
                    {
                        return Redirect("/project/add");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);  
                    return Redirect("/project/add");
                }
                return View(APMM);
            }
            else
            {
                Session["ReturnUrl"] = "/project/AddModule";
                return Redirect("/home/login");
            }
        }

        [HttpPost]
        public ActionResult AddModule(AddProjectModuleModel APMM)
        {
            if (Session["UserName"] != null)
            {
                try
                {
                    if (APMM.ProjectId != 0)
                    {
                        if (APMM.ModuleId != 0)
                        {
                            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                            var getProjectData = db.crm_projectmoduletbl.Where(em => em.M_Id == APMM.ModuleId && em.PId == APMM.ProjectId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (getProjectData != null)
                            {
                                getProjectData.ModuleName = APMM.ProjectModuleName.Trim();
                                getProjectData.BranchID = Convert.ToInt32(Session["BranchID"]);
                                getProjectData.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                                db.SaveChanges();
                                TempData["success"] = "Project Modiule updated successfully";
                                return Redirect("/project/AddModule/?pid=" + APMM.ProjectId);
                            }
                            else
                            {
                                return Redirect("/project/add");
                            }
                        }
                        else
                        {
                            crm_projectmoduletbl PMT = new crm_projectmoduletbl();
                            PMT.PId = APMM.ProjectId;
                            PMT.ModuleName = APMM.ProjectModuleName;
                            PMT.BranchID = Convert.ToInt32(Session["BranchID"]);
                            PMT.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                            PMT.Status = true;
                            PMT.Created_at = System.DateTime.Now;
                            db.crm_projectmoduletbl.Add(PMT);
                            db.SaveChanges();
                            TempData["success"] = "Project Modiule added successfully";
                            return Redirect("/project/AddModule/?pid=" + APMM.ProjectId);
                        }
                    }
                    else
                    {
                        return Redirect("/project/add");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    TempData["alert"] = "Sorry There is some problem!";     
                }
                return Redirect("/project/AddModule");
            }
            else
            {
                Session["ReturnUrl"] = "/project/AddModule";
                return Redirect("/home/login");
            }
        }
        #endregion
    }
}
