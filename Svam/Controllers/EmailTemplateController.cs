using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Svam.EF;
using Svam.Models;
using Traders.Models;

namespace Svam.Controllers
{
    [NoCache]
    public class EmailTemplateController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        [HttpGet]
        public ActionResult ManageTemplate(int? page)
        {
            EmailTemplateModel ETMModel = new EmailTemplateModel();
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                DataTable dtETM = DataAccessLayer.GetDataTable("Call CRM_GetEmailTemplate(" + CompanyID + "," + BranchID + ")");
                if (dtETM.Rows.Count > 0)
                {
                    List<EmailTemplateModel> etmList = new List<EmailTemplateModel>();
                     for (int i = 0; i < dtETM.Rows.Count; i++)
                     {
                         EmailTemplateModel oETMModel = new EmailTemplateModel();
                         oETMModel.EmailTemplateID = Convert.ToInt32(dtETM.Rows[i]["EmailTemplateID"]);
                         oETMModel.EmailTemplateName = Convert.ToString(dtETM.Rows[i]["EmailTemplateName"]);
                         oETMModel.CreatedOn = Convert.ToString(dtETM.Rows[i]["CreatedOn"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                         etmList.Add(oETMModel);
                     }
                     ETMModel.oEmailTemplateModelList = etmList;
                }

                #region Paging-Start
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

                if(ETMModel.oEmailTemplateModelList!=null && ETMModel.oEmailTemplateModelList.Count>0)
                {
                    TotalProducts = ETMModel.oEmailTemplateModelList.Count();
                    pages = (TotalProducts / pageSize);
                    var etm = ETMModel.oEmailTemplateModelList.Skip(pageSkip).Take(pageSize).ToList();
                    ETMModel.oEmailTemplateModelList = etm;
                    pages = (TotalProducts / pageSize);
                    Rem = (TotalProducts % pageSize);
                    if (Rem < pageSize && Rem != 0)
                    {
                        pages = (pages + 1);
                    }
                    ViewData["NoOfPages"] = pages;
                    if (page > 1)
                    {
                        var DeclareIndex = (pageSize * (page - 1)) + 1;
                        ViewData["DeclareIndex"] = DeclareIndex;
                    }
                    else
                    {
                        ViewData["DeclareIndex"] = 1;
                    }
                }
                
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(ETMModel);
        }

        [HttpGet]
        public ActionResult CreateManageTemplate(Int32? EmailTemplateID)
        { 
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            EmailTemplateModel ETM = new EmailTemplateModel();
            if (EmailTemplateID > 0)
            {
                //var getTemplatedetail = db.crm_emailtemplate.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.EmailTemplateID == EmailTemplateID).FirstOrDefault();
                //if (getTemplatedetail != null)
                //{
                //    ETM.EmailTemplateID = getTemplatedetail.EmailTemplateID;
                //    ETM.EmailTemplateName = getTemplatedetail.EmailTemplateName;
                //    ETM.EmailTempleteBody = getTemplatedetail.EmailTemplateContent;
                //}
            }
            return View(ETM);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateManageTemplate(EmailTemplateModel ETM)
        {
            if (ModelState.IsValid)
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                try
                {
                    if (ETM.EmailTemplateID > 0)
                    {
                        //var getTemplete = db.crm_emailtemplate.Where(em => em.EmailTemplateID == ETM.EmailTemplateID && em.BranchID==BranchID && em.CompanyID==CompanyID).FirstOrDefault();
                        //if (getTemplete != null)
                        //{
                        //    getTemplete.EmailTemplateName = ETM.EmailTemplateName;
                        //    getTemplete.EmailTemplateContent = ETM.EmailTempleteBody;
                        //    getTemplete.ModifiedBy = Convert.ToInt32(Session["UID"]);
                        //    getTemplete.ModifiedOn = System.DateTime.Now;
                        //    getTemplete.BranchID = BranchID;
                        //    getTemplete.CompanyID = CompanyID;
                        //    db.SaveChanges();
                        //    TempData["success"] = "Email template updated successfully";
                        //    return Redirect("/EmailTemplate/CreateManageTemplate/?EmailTemplateID=" + ETM.EmailTemplateID);
                        //}
                        //else
                        //{
                        //    TempData["alert"] = "There is some problem";                        
                        //}
                    }
                    else 
                    {
                        //crm_emailtemplate etemplete = new crm_emailtemplate();
                        //etemplete.EmailTemplateName = ETM.EmailTemplateName;
                        //etemplete.EmailTemplateContent = ETM.EmailTempleteBody;
                        //etemplete.CreatedBy = Convert.ToInt32(Session["UID"]);
                        //etemplete.CreatedOn = System.DateTime.Now;
                        //etemplete.BranchID = BranchID;
                        //etemplete.CompanyID = CompanyID;
                        //db.crm_emailtemplate.Add(etemplete);
                        db.SaveChanges();
                        TempData["success"] = "Email template added successfully";
                        return Redirect("/EmailTemplate/ManageTemplate");
                    }

                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    TempData["alert"] = "Sorry there is some problem. Please try again";
                }
            }
            else 
            {
                TempData["alert"] = "Sorry there is some problem. Please try again.";
            }
            return Redirect("/EmailTemplate/CreateManageTemplate");
        }
    }
}
