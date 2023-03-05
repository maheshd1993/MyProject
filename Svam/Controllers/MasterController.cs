using Svam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Traders.Models;
using Svam.EF;
using Svam.UtilityManager;

namespace Traders.Controllers
{
    public class MasterController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        #region Manage Lead Status
        public ActionResult ManageLeadStatus(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            LeadStatusModel LSM = new LeadStatusModel();
            try
            {
                if (id != null)
                {

                    var getData = db.crm_leadstatus_tbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        LSM.LeadStatusname = getData.LeadStatusName;
                        LSM.ColorHexValue = getData.ColorHexValue;
                    }
                }
                ViewBag.LeadStatus = db.crm_leadstatus_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(LSM);
        }

        [HttpPost]
        public ActionResult ManageLeadStatus(LeadStatusModel lsm, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!ModelState.IsValid)
            {
                TempData["alert"] = "Lead Status can't be blank";
                return View(lsm);
            }
            try
            {
                if (id != null)
                {
                    var getData = db.crm_leadstatus_tbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.LeadStatusName = lsm.LeadStatusname;
                        getData.ColorHexValue = lsm.ColorHexValue;
                        //getData.BranchID = BranchID;
                        //getData.CompanyID = CompanyID;
                        //getData.Status = true;
                        db.SaveChanges();
                        TempData["success"] = "Lead Status updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry no data found this id";
                    }
                }
                else
                {
                    var checkExist = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName.ToLower() == lsm.LeadStatusname.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_leadstatus_tbl lst = new crm_leadstatus_tbl();
                        lst.LeadStatusName = lsm.LeadStatusname;
                        lst.ColorHexValue = lsm.ColorHexValue;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.Status = true;
                        lst.created_at = Constant.GetBharatTime();
                        db.crm_leadstatus_tbl.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Lead Status added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This Status name is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageLeadstatus");
        }

        public JsonResult ChangeStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_leadstatus_tbl set Status=case when Status=1 then 0 else 1 end where id=" + id);
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

        #region Manage Lead Source
        public ActionResult ManageLeadSource(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            LeadSourceModel LSM = new LeadSourceModel();

            try
            {
                if (id != 0)
                {
                    var getData = db.crm_leadsource_tbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        LSM.LeadsourceName = getData.LeadsourceName;
                    }
                }
                ViewBag.LeadsourceList = db.crm_leadsource_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(LSM);
        }

        [HttpPost]
        public ActionResult ManageLeadSource(LeadSourceModel LSM, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!ModelState.IsValid)
            {
                TempData["alert"] = "Lead Source can't be blank";
                return View(LSM);
            }
            try
            {
                if (id != null)
                {
                    var getData = db.crm_leadsource_tbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.LeadsourceName = LSM.LeadsourceName;
                        getData.BranchID = BranchID;
                        getData.CompanyID = CompanyID;
                        //getData.Status = true;
                        db.SaveChanges();
                        TempData["success"] = "Lead source updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry no data found this id";
                    }
                }
                else
                {
                    var checkExist = db.crm_leadsource_tbl.Where(em => em.LeadsourceName.ToLower() == LSM.LeadsourceName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_leadsource_tbl lst = new crm_leadsource_tbl();
                        lst.LeadsourceName = LSM.LeadsourceName;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.Status = true;
                        lst.Created_at = System.DateTime.Now;
                        db.crm_leadsource_tbl.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Lead source added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This Lead source name is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageLeadSource");
        }

        public JsonResult ChangeLeadSourceStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_leadsource_tbl set Status=case when Status=1 then 0 else 1 end where Id=" + id);
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

        #region  ManageProductType
        public ActionResult ManageProductType(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            ProductTypeModel PTM = new ProductTypeModel();
            try
            {
                if (id != 0)
                {
                    var getData = db.crm_producttypetbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        PTM.ProductTypeName = getData.ProductTypeName;
                    }
                }
                ViewBag.ProductTypeList = db.crm_producttypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(PTM);
        }

        [HttpPost]
        public ActionResult ManageProductType(ProductTypeModel PTM, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!ModelState.IsValid)
            {
                TempData["alert"] = "Product type can't be blank";
                return View(PTM);
            }
            try
            {
                if (id != null)
                {
                    var getData = db.crm_producttypetbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.ProductTypeName = PTM.ProductTypeName;
                        getData.BranchID = BranchID;
                        getData.CompanyID = CompanyID;
                        //getData.Status = true;
                        db.SaveChanges();
                        TempData["success"] = "Product type updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry no data found.";
                    }
                }
                else
                {
                    var checkExist = db.crm_producttypetbl.Where(em => em.ProductTypeName.ToLower() == PTM.ProductTypeName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_producttypetbl lst = new crm_producttypetbl();
                        lst.ProductTypeName = PTM.ProductTypeName;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.Status = true;
                        db.crm_producttypetbl.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Product type added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This Product type name is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageProductType");
        }

        public JsonResult  ChangeProductTypeStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_producttypetbl set Status=case when Status=1 then 0 else 1 end where Id=" + id);
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

        #region ManageItemType
        public ActionResult ManageItemType(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            ItemTypeModel ITM = new ItemTypeModel();
            try
            {
                if (id != 0)
                {
                    var getData = db.crm_itemtypetbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        ITM.ItemTypeName = getData.ItemTypeName;
                    }
                }
                ViewBag.ItemTypeList = db.crm_itemtypetbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(ITM);
        }
        [HttpPost]
        public ActionResult ManageItemType(ItemTypeModel ITM, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!ModelState.IsValid)
            {
                TempData["alert"] = "Item type can't be blank";
                return View(ITM);
            }
            try
            {
                if (id != null)
                {
                    var getData = db.crm_itemtypetbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.ItemTypeName = ITM.ItemTypeName;
                        getData.BranchID = BranchID;
                        getData.CompanyID = CompanyID;
                        //getData.Status = true;
                        db.SaveChanges();
                        TempData["success"] = "Item type updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry no data found.";
                    }
                }
                else
                {
                    var checkExist = db.crm_itemtypetbl.Where(em => em.ItemTypeName.ToLower() == ITM.ItemTypeName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_itemtypetbl lst = new crm_itemtypetbl();
                        lst.ItemTypeName = ITM.ItemTypeName;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.Status = true;
                        db.crm_itemtypetbl.Add(lst);
                        db.SaveChanges();
                        //int returnValue = lst.Id;
                        //var itemtypelist = db.crm_itemtypetbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID).FirstOrDefault();
                        //if (itemtypelist != null)
                        //{
                        //    crm_assignitemtypeuser als = new crm_assignitemtypeuser();
                        //    als.ItemTypeId = returnValue;
                        //    als.CompanyID = CompanyID;
                        //    als.BranchID = BranchID;
                        //    als.UserId = 0;
                        //    als.Estimated_Cost = 0;
                        //    als.Quantity = 0;
                        //    db.crm_assignitemtypeuser.Add(als);
                        //    db.SaveChanges();
                        //}
                        //if (db.SaveChanges() > 0)
                        //{
                            TempData["success"] = "Item type added successfully";
                       // }
                        

                    }
                    else
                    {
                        TempData["alert"] = "This Item type name is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageItemType");
        }

        public JsonResult ChangeItemTypeStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_itemtypetbl set Status=case when Status=1 then 0 else 1 end where Id=" + id);
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

        #region Manage Interview Status
        public ActionResult ManageInterViewStatus(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            InterviewStatusModel ISM = new InterviewStatusModel();
            try
            {
                if (id != 0)
                {
                    var getData = db.crm_intervewstatus.Where(em => em.ID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        ISM.InterviewStatusName = getData.InterViewStatus;
                    }
                }
                ViewBag.InterViewStatusList = db.crm_intervewstatus.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(ISM);
        }

        [HttpPost]
        public ActionResult ManageInterViewStatus(InterviewStatusModel ISM, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!ModelState.IsValid)
            {
                TempData["alert"] = "Interview status can't be blank";
                return View(ISM);
            }
            try
            {
                if (id != null)
                {
                    var getData = db.crm_intervewstatus.Where(em => em.ID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.InterViewStatus = ISM.InterviewStatusName;
                        getData.BranchID = BranchID;
                        getData.CompanyID = CompanyID;
                        //getData.Status = true;
                        db.SaveChanges();
                        TempData["success"] = "Interview status updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry no data found.";
                    }
                }
                else
                {
                    var checkExist = db.crm_intervewstatus.Where(em => em.InterViewStatus.ToLower() == ISM.InterviewStatusName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_intervewstatus lst = new crm_intervewstatus();
                        lst.InterViewStatus = ISM.InterviewStatusName;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.Status = true;
                        db.crm_intervewstatus.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Interview Status added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This Interview status name is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageInterViewStatus");
        }

        public JsonResult ChangeInterViewStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_intervewstatus set Status=case when Status=1 then 0 else 1 end where Id=" + id);
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

        #region Email Setting
        [HttpGet]
        public ActionResult ManageEmailSetting()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            CRMEmailSettingModel CESM = new CRMEmailSettingModel();
            var getEmailSetting = db.crm_emailsetting.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
            if (getEmailSetting != null)
            {
                CESM.EmailSettingID = getEmailSetting.EmailSettingID;
                CESM.EmailAddress = getEmailSetting.EmailAddress;               
                CESM.Port = getEmailSetting.Port;
                CESM.SMTPHost = getEmailSetting.SMTPHost;
                CESM.UserName = getEmailSetting.UserName;
                CESM.Password = getEmailSetting.Password;
                CESM.SSL = getEmailSetting.SSL;
                CESM.CCEmail = getEmailSetting.CCEmail;
                CESM.DisplayName = getEmailSetting.DisplayName;
            }
            return View(CESM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageEmailSetting(CRMEmailSettingModel oCRMEmailSettingModel)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                if (oCRMEmailSettingModel.EmailSettingID > 0)
                {
                    var updateEmailSetting = db.crm_emailsetting.Where(em => em.EmailSettingID == oCRMEmailSettingModel.EmailSettingID && em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
                    if (updateEmailSetting != null)
                    {
                        updateEmailSetting.EmailAddress = oCRMEmailSettingModel.EmailAddress;
                        updateEmailSetting.SMTPHost = oCRMEmailSettingModel.SMTPHost;
                        updateEmailSetting.Port = oCRMEmailSettingModel.Port;
                        updateEmailSetting.UserName = oCRMEmailSettingModel.UserName;
                        updateEmailSetting.Password = oCRMEmailSettingModel.Password;
                        updateEmailSetting.SSL = oCRMEmailSettingModel.SSL;
                        updateEmailSetting.CCEmail = oCRMEmailSettingModel.CCEmail;
                        updateEmailSetting.DisplayName = oCRMEmailSettingModel.DisplayName;
                        updateEmailSetting.CompanyID = CompanyID;
                        updateEmailSetting.BranchCode = BranchID;
                        updateEmailSetting.ModifiedBy= Convert.ToString(Session["UID"]);
                        updateEmailSetting.ModifiedDate = Constant.GetBharatTime();
                        db.SaveChanges();
                        TempData["success"] = "Email setting updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "** Something went wrong,Please try again.";
                    }
                }
                else
                {
                    var emailCheck = db.crm_emailsetting.Where(em => em.EmailAddress == oCRMEmailSettingModel.EmailAddress && em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
                    if (emailCheck == null)
                    {
                        crm_emailsetting emsetting = new crm_emailsetting();
                        emsetting.EmailAddress = oCRMEmailSettingModel.EmailAddress;
                        emsetting.SMTPHost = oCRMEmailSettingModel.SMTPHost;
                        emsetting.Port = oCRMEmailSettingModel.Port;
                        emsetting.UserName = oCRMEmailSettingModel.UserName;
                        emsetting.Password = oCRMEmailSettingModel.Password;
                        emsetting.SSL = oCRMEmailSettingModel.SSL;
                        emsetting.CCEmail = oCRMEmailSettingModel.CCEmail;
                        emsetting.DisplayName = oCRMEmailSettingModel.DisplayName;
                        emsetting.CompanyID = CompanyID;
                        emsetting.BranchCode = BranchID;
                        emsetting.CreatedBy = Convert.ToString(Session["UID"]);
                        emsetting.CreatedDate = Constant.GetBharatTime();
                       
                        db.crm_emailsetting.Add(emsetting);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Email setting added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This email address is already available.";
                    }
                }
            }
            else 
            {
                TempData["alert"] = "** Something went wrong,Please try again.";
            }
            return View(oCRMEmailSettingModel);
        }
        #endregion

        #region Error Type
        public ActionResult ManageErrorType(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            CRMErrorTypeModel LSM = new CRMErrorTypeModel();
            try
            {
                if (id != null)
                {

                    var getData = db.crm_errortype.Where(em => em.ErrrorID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        LSM.ErrorName = getData.ErrorName;
                    }
                }

                var geterrortypeList = db.crm_errortype.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                if (geterrortypeList.Count > 0)
                {
                    List<CRMErrorTypeModel> LSMList = new List<CRMErrorTypeModel>();
                    foreach (var item in geterrortypeList)
                    {
                        CRMErrorTypeModel cError = new CRMErrorTypeModel();
                        cError.ErrorId = item.ErrrorID;
                        cError.ErrorName = item.ErrorName;
                        cError.StatusName = item.IsActive == true ? "Active" : "Deactive";
                        cError.IsActive = (bool)item.IsActive;
                        LSMList.Add(cError);
                    }
                    LSM.CRMErrorTypeModelList = LSMList;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
             return View(LSM);
        }

        [HttpPost]
        public ActionResult ManageErrorType(CRMErrorTypeModel CET, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                if (id != null)
                {
                    var getData = db.crm_errortype.Where(em => em.ErrrorID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.ErrorName = CET.ErrorName;
                        getData.BranchID = BranchID;
                        getData.CompanyID = CompanyID;
                        //getData.IsActive = true;
                        getData.IsDeleted = false;
                        getData.ModifiedOn = System.DateTime.Now;
                        getData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                        db.SaveChanges();
                        TempData["success"] = "Error type updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Something went wrong,please try again";
                    }
                }
                else
                {
                    var checkExist = db.crm_errortype.Where(em => em.ErrorName.ToLower() == CET.ErrorName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_errortype lst = new crm_errortype();
                        lst.ErrorName = CET.ErrorName;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.IsActive = true;
                        lst.IsDeleted = false;
                        lst.CreatedOn = System.DateTime.Now;
                        lst.CreatedBy = Convert.ToInt32(Session["UID"]);
                        db.crm_errortype.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Error type added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This error type is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageErrorType");

        }

        public JsonResult ChangeErrorTypeStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_errortype set IsActive=case when IsActive=1 then 0 else 1 end where ErrrorID=" + id);
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

        #region Urgency
        public ActionResult ManageUrgency(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            CRMUrgencyModel LSM = new CRMUrgencyModel();
            try
            {
                if (id != null)
                {
                    var getData = db.crm_urgency.Where(em => em.urgencyID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        LSM.UrgencyName = getData.urgencyName;
                    }
                }

                var geterrortypeList = db.crm_urgency.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                if (geterrortypeList.Count > 0)
                {
                    List<CRMUrgencyModel> LSMList = new List<CRMUrgencyModel>();
                    foreach (var item in geterrortypeList)
                    {
                        CRMUrgencyModel cError = new CRMUrgencyModel();
                        cError.UrgencyId = item.urgencyID;
                        cError.UrgencyName = item.urgencyName;
                        cError.StatusName = item.IsActive == true ? "Active" : "Deactive";
                        LSMList.Add(cError);
                    }
                    LSM.CRMUrgencyModelList = LSMList;
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(LSM);
        }

        [HttpPost]
        public ActionResult ManageUrgency(CRMUrgencyModel CET, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                if (id != null)
                {
                    var getData = db.crm_urgency.Where(em => em.urgencyID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.urgencyName = CET.UrgencyName;
                        getData.BranchID = BranchID;
                        getData.CompanyID = CompanyID;
                        //getData.IsActive = true;
                        getData.IsDeleted = false;
                        getData.ModifiedOn = System.DateTime.Now;
                        getData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                        db.SaveChanges();
                        TempData["success"] = "Urgency updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Something went wrong,please try again";
                    }
                }
                else
                {
                    var checkExist = db.crm_urgency.Where(em => em.urgencyName.ToLower() == CET.UrgencyName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_urgency lst = new crm_urgency();
                        lst.urgencyName = CET.UrgencyName;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.IsActive = true;
                        lst.IsDeleted = false;
                        lst.CreatedOn = System.DateTime.Now;
                        lst.CreatedBy = Convert.ToInt32(Session["UID"]);
                        db.crm_urgency.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Urgency added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This urgency is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageUrgency");

        }

        public JsonResult ChangeUrgencyStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_urgency set IsActive=case when IsActive=1 then 0 else 1 end where urgencyID=" + id);
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

        #region Leave type name 
        public ActionResult ManageLeaveType(int? id) 
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            CRMLeaveTypeNameModel LSM = new CRMLeaveTypeNameModel();
            try
            {
                if (id != null)
                {

                    var getData = db.crm_leavetypename.Where(em => em.ID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        LSM.LeaveName = getData.LeaveName;
                    }
                }

                var geterrortypeList = db.crm_leavetypename.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                if (geterrortypeList.Count > 0)
                {
                    List<CRMLeaveTypeNameModel> LSMList = new List<CRMLeaveTypeNameModel>();
                    foreach (var item in geterrortypeList)
                    {
                        CRMLeaveTypeNameModel cError = new CRMLeaveTypeNameModel();
                        cError.ID = item.ID;
                        cError.LeaveName = item.LeaveName;
                        cError.StatusName = item.IsActive == true ? "Active" : "Deactive";
                        cError.IsActive = (bool)item.IsActive;
                        LSMList.Add(cError);
                    }
                    LSM.CRMLeaveTypeModelList = LSMList;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(LSM);
        }

        [HttpPost]
        public ActionResult ManageLeaveType(CRMLeaveTypeNameModel CET, int? id) 
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var dt = Constant.GetBharatTime();
            try
            {
                if (id != null)
                {
                    var getData = db.crm_leavetypename.Where(em => em.ID == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.LeaveName = CET.LeaveName;
                        getData.BranchID = BranchID;
                        getData.CompanyID = CompanyID;
                        //getData.IsActive = true;
                        getData.IsDeleted = false;
                        getData.ModifiedOn = dt;
                        getData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                        db.SaveChanges();
                        TempData["success"] = "Leave name updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Something went wrong,please try again";
                    }
                }
                else
                {
                    var checkExist = db.crm_leavetypename.Where(em => em.LeaveName.ToLower() == CET.LeaveName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_leavetypename lst = new crm_leavetypename();
                        lst.LeaveName = CET.LeaveName;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.IsActive = true;
                        lst.IsDeleted = false;
                        lst.CreatedOn = dt;
                        lst.CreatedBy = Convert.ToInt32(Session["UID"]);
                        db.crm_leavetypename.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Leave name added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This leave name is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageLeaveType");

        }

        public JsonResult ChangeLeaveTypeStatus(int id) 
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update Crm_LeaveTypeName set IsActive=case when IsActive=1 then 0 else 1 end where ID=" + id);
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

        #region Manage Project Status
        public ActionResult ManageProjectStatus(int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            ProjectStatusModel PSM = new ProjectStatusModel();
            try
            {
                if (id != null)
                {

                    var getData = db.crm_projectstatus_tbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        PSM.ProjectStatusname = getData.ProjectStatusName;
                        PSM.ColorHexValue = getData.ColorHexValue;
                    }
                }
                ViewBag.ProjectStatus = db.crm_projectstatus_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(PSM);
        }
        [HttpPost]
        public ActionResult ManageProjectStatus(ProjectStatusModel lsm, int? id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            if (!ModelState.IsValid)
            {
                TempData["alert"] = "Project Status can't be blank";
                return View(lsm);
            }
            try
            {
                if (id != null)
                {
                    var getData = db.crm_projectstatus_tbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.ProjectStatusName = lsm.ProjectStatusname;
                        getData.ColorHexValue = lsm.ColorHexValue;
                        //getData.BranchID = BranchID;
                        //getData.CompanyID = CompanyID;
                        //getData.Status = true;
                        db.SaveChanges();
                        TempData["success"] = "Project Status updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry no data found this id";
                    }
                }
                else
                {
                    var checkExist = db.crm_projectstatus_tbl.Where(em => em.ProjectStatusName.ToLower() == lsm.ProjectStatusname.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_projectstatus_tbl lst = new crm_projectstatus_tbl();
                        lst.ProjectStatusName = lsm.ProjectStatusname;
                        lst.ColorHexValue = lsm.ColorHexValue;
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.Status = true;
                        lst.created_at = Constant.GetBharatTime();
                        db.crm_projectstatus_tbl.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            TempData["success"] = "Project Status added successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "This Status name is already available in list";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Master/ManageProjectStatus");
        }

        public JsonResult ChangeProjectStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_leadstatus_tbl set Status=case when Status=1 then 0 else 1 end where id=" + id);
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
    }
}
