using Svam.EF;
using Svam.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Svam.Controllers
{
    [NoCache]
    public class IndiaMartSettingController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        [HttpGet]
        public ActionResult ManageIndiaMartSetting()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            CRMIndiaMartModel CIM = new CRMIndiaMartModel();
            try
            {
                var getSetting = db.crm_indiamartsetting.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefault();
                if (getSetting != null)
                {
                    CIM.IndiaMartID = getSetting.IndiaMartID;
                    CIM.MobileNumber = getSetting.MobileNumber;
                    CIM.IndiaMartCRMKey = getSetting.IndiaMartCRMKey;
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(CIM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageIndiaMartSetting(CRMIndiaMartModel CIM)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (ModelState.IsValid)
                {
                    if (CIM.IndiaMartID > 0)
                    {
                        var updateSetting = db.crm_indiamartsetting.Where(em => em.IndiaMartID == CIM.IndiaMartID && em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefault();
                        if (updateSetting != null)
                        {
                            updateSetting.MobileNumber = CIM.MobileNumber;
                            updateSetting.IndiaMartCRMKey = CIM.IndiaMartCRMKey;
                            updateSetting.BranchID = BranchID;
                            updateSetting.CompanyID = CompanyID;
                            db.SaveChanges();
                            TempData["success"] = "IndiaMart setting updated successfully";
                        }
                        else
                        {
                            TempData["alert"] = "** Something went wrong,Please try again.";
                        }
                    }
                    else
                    {
                        var settingCheck = db.crm_indiamartsetting.Where(em => em.MobileNumber == CIM.MobileNumber && em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefault();
                        if (settingCheck == null)
                        {
                            crm_indiamartsetting cims = new crm_indiamartsetting();
                            cims.MobileNumber = CIM.MobileNumber;
                            cims.IndiaMartCRMKey = CIM.IndiaMartCRMKey;
                            cims.BranchID = BranchID;
                            cims.CompanyID = CompanyID;
                            db.crm_indiamartsetting.Add(cims);
                            if (db.SaveChanges() > 0)
                            {
                                TempData["success"] = "IndiaMart setting added successfully";
                            }
                        }
                        else
                        {
                            TempData["alert"] = "Registered mobile number is already available.";
                        }
                    }
                }
                else
                {
                    TempData["alert"] = "** Something went wrong,Please try again.";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(CIM);
        }       
    }
}
