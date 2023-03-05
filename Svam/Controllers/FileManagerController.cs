using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace Svam.Controllers
{
    public class FileManagerController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        [HttpGet]
        public ActionResult ManageFile(Int32? page)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            CRMFileManagerModel CFM = new CRMFileManagerModel();

            CFM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            var getFileList = db.crm_filemanager.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.IsDeleted == false).OrderByDescending(em=>em.FileID).ToList();
            if (getFileList.Count > 0)
            {
                List<CRMFileManagerModel> CFMList = new List<CRMFileManagerModel>();
                foreach(var item in getFileList)
                {
                    CRMFileManagerModel oCFM = new CRMFileManagerModel();
                    oCFM.FileID = item.FileID;
                    oCFM.FileName = item.FileName;
                    oCFM.FileUpload = item.FileUpload;
                    if (item.ModifledBy > 0)
                    {

                        oCFM.UploadDate = String.Format("{0:" + CFM.DateFormat + "}", item.ModifiedOn); //Convert.ToString(item.ModifiedOn);
                    }
                    else 
                    {
                        oCFM.UploadDate = String.Format("{0:" + CFM.DateFormat + "}", item.CreatedOn); //Convert.ToString(item.CreatedOn);
                    }
                    if (item.IsActive == true)
                    {
                        oCFM.FileStatusName = "Active";
                    }
                    else 
                    {
                        oCFM.FileStatusName = "Deactive";
                    }

                    CFMList.Add(oCFM);
                }
                CFM.oCRMFileManagerModelList = CFMList;

                #region Paging-Start
                int pageNumber = 1;
                int pageSize = 20;
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

                TotalProducts = CFM.oCRMFileManagerModelList.Count();
                pages = (TotalProducts / pageSize);
                var Product = CFM.oCRMFileManagerModelList.Skip(pageSkip).Take(pageSize).ToList();
                CFM.oCRMFileManagerModelList = Product;
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
                #endregion
            }
            return View(CFM);
        }

        [HttpGet]
        public ActionResult EditManageFile(Int32? FileID)
        {
            var getFile = new crm_filemanager();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string Successmsg = string.Empty;
            if (FileID > 0)
            {
                 getFile = db.crm_filemanager.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FileID == FileID).FirstOrDefault();               
            }
            return Json(getFile, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ManageFile()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            string fileErrormsg = string.Empty;
            try
            {
                bool SaveNow = false;
                HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
                String sFileName = Convert.ToString(Request.Form[0]);
                Int64? FileID = Request.Form[1] == "" ? 0 : Convert.ToInt64(Request.Form[1]);
                if (Postfile != null)
                {
                    string FileName = string.Empty;
                    string FileFullName = string.Empty;
                    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp","csv" };
                    var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
                    if (supportedTypes.Contains(fileExt))
                    {
                        string extension = Path.GetExtension(Postfile.FileName);
                        FileName = "FM-" + Convert.ToString(Session["UserName"]).Trim() + "-" + Guid.NewGuid().ToString() + "";
                        FileFullName = FileName + extension;
                        string _path = Server.MapPath("~/FileManager/" + FileName + extension);
                        Postfile.SaveAs(_path);
                        SaveNow = true;

                        
                        DateTime localTime = Constant.GetBharatTime();
                        if (FileID > 0)
                        {
                            var fileExists = db.crm_filemanager.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FileID == FileID).FirstOrDefault();
                            if (fileExists != null)
                            {
                                fileExists.FileName = sFileName;
                                fileExists.FileUpload = FileFullName;
                                fileExists.ModifiedOn = localTime;
                                fileExists.ModifledBy = Convert.ToInt32(Session["UID"]);
                                db.Entry(fileExists).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            crm_filemanager fm = new crm_filemanager();
                            fm.BranchID = BranchID;
                            fm.CompanyID = CompanyID;
                            fm.FileName = sFileName;
                            fm.FileUpload = FileFullName;
                            fm.CreatedBy = Convert.ToInt32(Session["UID"]);
                            fm.CreatedOn = localTime;
                            fm.IsActive = true;
                            fm.IsDeleted = false;
                            db.crm_filemanager.Add(fm);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        fileErrormsg = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                        SaveNow = false;
                    }
                }

                if (SaveNow == true)
                {
                    Successmsg = "File save successfully.";
                }
                else if (SaveNow == false)
                {
                    Errormsg = fileErrormsg;
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
        public ActionResult DeleteFile(Int32? FileID)
        {
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                var getFile = db.crm_filemanager.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FileID == FileID).FirstOrDefault();
                if (getFile != null)
                {
                    getFile.IsDeleted = true;
                    db.Entry(getFile).State = EntityState.Modified;
                    db.SaveChanges();
                    Successmsg = "File deleted successfully";
                }
                else
                {
                    Errormsg = "** Someting went wrong. Please try again.";
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

        public FileResult FileDownload(string PostFile)
        {
            var filepath = System.IO.Path.Combine(Server.MapPath("~/FileManager/"), PostFile);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), PostFile);
        }
    }
}
