using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using Svam.EF;
using Traders.Mailer;
using Traders.Models;
using Svam.Models;
using Svam.Models.DTO;
using Svam.UtilityManager;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Dynamic;
using Newtonsoft.Json;

namespace Traders.Controllers
{
    public class commonController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        #region Validate-User
        public ActionResult ValidateUserName(string UserName)
        {
            //Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            //Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            bool existCoupon = true;
            bool GetCoupon = db.crm_usertbl.Any(x => x.UserName == UserName /*&& x.BranchID==BranchID && x.CompanyID==CompanyID*/);
            if (GetCoupon == true)
            {
                existCoupon = !GetCoupon;
                return Json(existCoupon, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(existCoupon, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

        #region Validate-EmailId
        public ActionResult ValidateEmail(string Email)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            bool existCoupon = true;
            bool GetCoupon = db.crm_usertbl.Any(x => x.Email == Email && x.BranchID == BranchID && x.CompanyID == CompanyID && x.Created_at == null);
            if (GetCoupon == true)
            {
                existCoupon = !GetCoupon;
                return Json(existCoupon, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(existCoupon, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

        #region User-ChangePassword
        public ActionResult userchangepswd(string NewPassword, int UserId)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            var returnMsg = "";
            try
            {
                var GetDatabyUId = db.crm_usertbl.Where(em => em.Id == UserId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetDatabyUId != null)
                {
                    #region Password encryption
                    string VersionKey = "";
                    byte[] iv1;

                    VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version

                    byte[] key = EncriptAES.getdcriptkey(out iv1);
                    string ecncryptPwd = EncriptAES.EncryptString(NewPassword, key, iv1);
                    #endregion

                    //GetDatabyUId.Password = NewPassword;
                    GetDatabyUId.Password = ecncryptPwd;//save ecncrypted password 
                    GetDatabyUId.KeyVersion = VersionKey;//save latest key version   
                    db.SaveChanges();
                    returnMsg = "done";
                }
                else
                {
                    returnMsg = "fail";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                returnMsg = "fail";
            }

            return Json(returnMsg, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Display-ProductNAme
        public ActionResult ProductNameListDisplay(string firmList)
        {
            try
            {
                var SpliteData = firmList.Split(',');
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
            return PartialView("_PartialProductName");
        }
        #endregion

        #region ViewMappeduserlist
        public ActionResult ViewMappeduserlist(int Id)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            ViewLeadsModel VLM = new ViewLeadsModel();
            DataTable Geleads = DataAccessLayer.GetDataTable(" call CRM_ViewLeads('" + Id + "'," + BranchID + "," + CompanyID + ")");
            if (Geleads.Rows.Count > 0)
            {
                VLM.viewleadsList = (from dr in Geleads.AsEnumerable()
                                     select new ViewLeadsModel()
                                     {

                                         LeadName = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                         FollowupDate = Convert.ToString(dr["FollowDate"]),
                                         LeadStatus = Convert.ToString(dr["LeadStatus"])
                                     }).ToList();
            }

            return PartialView("_PartialViewMappeduserLeads", VLM);
        }

        #endregion

        #region View-OurUserArchivesData
        public ActionResult ViewOuruserArchives(int UId, string StartDate, string EndDate)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            ViewArchivesModel VAM = new ViewArchivesModel();
            try
            {
                DataTable Getleads = DataAccessLayer.GetDataTable(" call CRM_LeadsBetwenDates('" + StartDate + "','" + EndDate + "'," + BranchID + "," + CompanyID + ")");
                if (Getleads.Rows.Count > 0)
                {
                    VAM.viewarchivesList = (from dr in Getleads.AsEnumerable()
                                            select new ViewArchivesModel()
                                            {
                                                LeadOwnerID = Convert.ToInt32(dr["LeadOwner"]),
                                                LeadOwner = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                FollowupDate = Convert.ToString(dr["FollowDate"]),
                                                LeadStatus = Convert.ToString(dr["LeadStatus"])
                                            }).Where(em => em.LeadOwnerID == UId).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialUserArchivesData", VAM);
        }

        #endregion

        // Common Search .............................
        public ActionResult CommonSearch(string SearchTxt, string sType)
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            if (Session["UserName"] != null)
            {
                try
                {
                    int BranchID = Convert.ToInt32(Session["BranchID"]);
                    int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    int uid = Convert.ToInt32(Session["UID"]);

                    if (!String.IsNullOrEmpty(SearchTxt))
                    {
                        if (!string.IsNullOrEmpty(sType))
                        {
                            SearchTxt = SearchTxt.Substring(SearchTxt.Length - 9, 9);//get last line digits  SearchTxt.Replace("+", "").Replace("-", "");
                        }

                        //var GetData = db.crm_createleadstbl.Where(em => em.Customer.ToLower().Contains(SearchTxt.ToLower()) || em.MobileNo.ToLower().Contains(SearchTxt.ToLower()) && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();
                        DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetSearchLeads('" + SearchTxt.Trim() + "'," + BranchID + "," + CompanyID + ")");
                        var getNewData = (from dr in GetRecords.AsEnumerable()
                                          select new ViewLeadsModel()
                                          {
                                              Id = Convert.ToInt32(dr["ID"]),
                                              LeadName = Convert.ToString(dr["Customer"]),
                                              Mob = Convert.ToString(dr["MobileNo"]),
                                              EMail = Convert.ToString(dr["EmailId"]),
                                              Country = Convert.ToString(dr["Country"]),
                                              State = Convert.ToString(dr["State"]),
                                              City = Convert.ToString(dr["City"]),
                                              FollowupDate = Convert.ToString(dr["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                              Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                              LeadStatus = Convert.ToString(dr["LeadStatusName"]),
                                              AssinedTo = Convert.ToString(dr["AssinedTo"]),
                                              AssignBy = Convert.ToString(dr["AssinedBy"]),
                                              Address = Convert.ToString(dr["Address"]),
                                              LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                              AssignTo = Convert.ToString(dr["AssignTo"]),
                                              AssignedBy = Convert.ToString(dr["AssignedBy"]),
                                              AssignDate = Convert.ToString(dr["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", ""),
                                              ResellerId = Convert.ToInt32(dr["ResellerId"]),
                                              ResellerName = Convert.ToString(dr["ResellerName"]),
                                              ResellerContactNo = Convert.ToString(dr["ResellerContactNo"]),
                                              ResellerStatus = Convert.ToString(dr["ResellerStatus"]),
                                              ResellerCode = Convert.ToString(dr["ResellerCode"]),
                                              ResellerDocStatus = Convert.ToString(dr["ResellerDocStatus"]),
                                              CompanyName = Convert.ToString(dr["CompanyName"])
                                          }).ToList();
                        //ViewBag.result = getNewData.OrderByDescending(em => em.FollowupDate).ToList();
                        VLM.viewleadsList = getNewData.OrderByDescending(em => em.FollowupDate).ToList();
                    }
                    return PartialView("_PartialCommonSearch", VLM);
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    return PartialView("_PartialCommonSearch", VLM);
                }
            }
            else
            {
                return Redirect("/home/login");
            }
        }

        //Leads Request...........
        public ActionResult LeadRequest(string Leadtype, int NoOfLeads, string State)
        {
            string retMsg = "";
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                crm_leadrequesttbl LR = new crm_leadrequesttbl();
                LR.UID = Convert.ToInt32(Session["UID"]);
                LR.NoOfLeads = NoOfLeads;
                LR.State = State;
                LR.LeadsType = "NEW";
                LR.Created_at = System.DateTime.Now;
                LR.Status = false;
                LR.BranchID = BranchID;
                LR.CompanyID = CompanyID;
                db.crm_leadrequesttbl.Add(LR);
                if (db.SaveChanges() > 0)
                {
                    retMsg = "success";
                }
                else
                {
                    retMsg = "fail";
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                retMsg = "problem";
            }
            return Json(retMsg, JsonRequestBehavior.AllowGet);
        }

        //Get Collect The Notification....
        public ActionResult CalulateNotification()
        {
            int totalLeads = 0;
            string returnMsg = "";
            try
            {
                //if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                //{
                //    #region If-SuperAdmin
                //    var Count = db.P_GetRequestLeads(false).ToList().Count();
                //    if (Count > 0)
                //    {
                //        totalLeads += Count;
                //        returnMsg = "success";
                //    }

                //    #endregion

                //}
                //else
                //{
                //    #region If-User
                //    var UID = Convert.ToInt32(Session["UID"]);
                //    var GetData = db.usertbls.Where(em => em.Id == UID).FirstOrDefault();
                //    if (GetData != null)
                //    {
                //        var GetMapuser = GetData.MappedUsers;
                //        var spliteMapUser = GetMapuser.Split(',');
                //        if (spliteMapUser.Count() > 0)
                //        {
                //            foreach (var item in spliteMapUser)
                //            {
                //                int mapuid = Convert.ToInt32(item);
                //                var CountLeads = db.leadrequesttbls.Where(em => em.UID == mapuid && em.Status == false).ToList().Count();
                //                if (CountLeads > 0)
                //                {
                //                    totalLeads += CountLeads;
                //                }
                //            }
                //            returnMsg = "success";
                //        }
                //        else
                //        {
                //            returnMsg = "no";
                //        }

                //    }
                //    else
                //    {
                //        returnMsg = "no";
                //    }
                //    #endregion
                //}
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                returnMsg = "problem";
            }
            return Json(totalLeads, returnMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageLeadStatusCloseWon(Int64 LID, string projectDescription, string projectValue, string advancePayment, string advancePayDate, string chequeNo, string expiryDate, string license, int LeadClosedId, string completionDate)
        {
            string msg = "";
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            try
            {
                // BY chetna
                if (LeadClosedId == 0)
                {
                    var getData = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Closed" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        LeadClosedId = getData.Id;
                    }
                }
                //  End chetna
                //else
                //{
                int uid = Convert.ToInt32(Session["UID"]);
                var GetLeadsInfo = db.crm_createleadstbl.Where(em => em.Id == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                //DateTime utcTime = DateTime.UtcNow;
                //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime localTime = Constant.GetBharatTime();
                if (GetLeadsInfo != null)
                {
                    GetLeadsInfo.LeadStatus = "Closed";
                    db.SaveChanges();
                    var GetSaleDetail = db.crm_saledetailtbl.Where(em => em.FK_LEADID == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (GetSaleDetail != null)
                    {
                        GetSaleDetail.PROJECTDESCRIPTION = projectDescription;
                        GetSaleDetail.PROJECTVALUE = Convert.ToDecimal(string.IsNullOrEmpty(projectValue) ? "0" : projectValue);
                        GetSaleDetail.ADVANCEPAYMENT = Convert.ToDecimal(string.IsNullOrEmpty(advancePayment) ? "0" : advancePayment); 
                        GetSaleDetail.ADVANCEPAYMENTDATE = advancePayDate;
                        GetSaleDetail.CHEQUENO = chequeNo;
                        GetSaleDetail.PROJECTCOMPLETIONDATE = completionDate;
                        GetSaleDetail.EXPIRYDATE = expiryDate;
                        GetSaleDetail.LICENSE = license;
                        GetSaleDetail.FK_CREATEDBY = !string.IsNullOrEmpty(GetLeadsInfo.AssignTo) ? Convert.ToInt32(GetLeadsInfo.AssignTo) : GetLeadsInfo.LeadOwner;
                        GetSaleDetail.CREATEDDATE = localTime.Date;
                        GetSaleDetail.CREATEDDATETIME = localTime;
                        GetSaleDetail.BranchID = BranchID;
                        GetSaleDetail.CompanyID = CompanyID;
                        GetLeadsInfo.LeadStatusID = LeadClosedId;
                        GetLeadsInfo.LeadStatus = "Closed";//update lead status
                        db.SaveChanges();
                        msg = "success";
                    }
                    else
                    {
                        crm_saledetailtbl sd = new crm_saledetailtbl();
                        sd.PROJECTDESCRIPTION = projectDescription;
                        sd.PROJECTVALUE = Convert.ToDecimal(string.IsNullOrEmpty(projectValue) ? "0" : projectValue);
                        sd.ADVANCEPAYMENT = Convert.ToDecimal(string.IsNullOrEmpty(advancePayment) ? "0" : advancePayment); ;
                        sd.ADVANCEPAYMENTDATE = advancePayDate;
                        sd.CHEQUENO = chequeNo;
                        sd.PROJECTCOMPLETIONDATE = completionDate;
                        sd.EXPIRYDATE = expiryDate;
                        sd.LICENSE = license;
                        sd.FK_LEADID = GetLeadsInfo.Id;
                        sd.FK_CREATEDBY = !string.IsNullOrEmpty(GetLeadsInfo.AssignTo) ? Convert.ToInt32(GetLeadsInfo.AssignTo) : GetLeadsInfo.LeadOwner;
                        sd.CREATEDDATE = localTime.Date;
                        sd.CREATEDDATETIME = localTime;
                        sd.BranchID = BranchID;
                        sd.CompanyID = CompanyID;
                        GetLeadsInfo.LeadStatusID = LeadClosedId;//update lead status
                        db.crm_saledetailtbl.Add(sd);
                        db.SaveChanges();
                        msg = "success";
                        //sd.PROJECTCOMPLETIONDATE = 
                    }
                    //GetLeadsInfo.BankName = BankName.Trim();
                    //GetLeadsInfo.AccountDetails = AccountDetails.Trim();
                    //GetLeadsInfo.ProductDetails = ProductDetails.Trim();
                    //GetLeadsInfo.ValidFrom = ValidFrom.Trim();
                    //GetLeadsInfo.ValidTo = ValidTo.Trim();
                    //GetLeadsInfo.LeadStatus = "Close won";
                    //GetLeadsInfo.PaymentDate = System.DateTime.Now;
                    //GetLeadsInfo.Amount = amount;
                    //GetLeadsInfo.Discount = discount;
                    //GetLeadsInfo.PaymentComment = Comment;
                    //if (db.SaveChanges() > 0)
                    //{
                    //    msg = "success";
                    //}
                    //else
                    //{
                    //    msg = "error";
                    //}
                }
                else
                {
                    msg = "error";
                }
                //}
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "error";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSaleDetail(int LID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var GetSaleDetail = new crm_saledetailtbl();
            var DateFormat = Constant.DateFormat();//get date format by company id
            try
            {
                var data = db.crm_saledetailtbl.Where(em => em.FK_LEADID == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (data != null)
                {
                    GetSaleDetail.PROJECTDESCRIPTION = data.PROJECTDESCRIPTION;
                    GetSaleDetail.PROJECTVALUE = Convert.ToDecimal(data.PROJECTVALUE);
                    GetSaleDetail.ADVANCEPAYMENT = Convert.ToDecimal(data.ADVANCEPAYMENT);
                    if (!string.IsNullOrEmpty(data.ADVANCEPAYMENTDATE))
                    {
                        var apdt = Convert.ToDateTime(data.ADVANCEPAYMENTDATE);
                        var advancePayDate = String.Format("{0:" + DateFormat + "}", apdt);
                        GetSaleDetail.ADVANCEPAYMENTDATE = advancePayDate;
                    }

                    GetSaleDetail.CHEQUENO = data.CHEQUENO;

                    if (!string.IsNullOrEmpty(data.PROJECTCOMPLETIONDATE))
                    {
                        var cpdt = Convert.ToDateTime(data.PROJECTCOMPLETIONDATE);
                        var completionDate = String.Format("{0:" + DateFormat + "}", cpdt);
                        GetSaleDetail.PROJECTCOMPLETIONDATE = completionDate;
                    }
                    if (!string.IsNullOrEmpty(data.EXPIRYDATE))
                    {
                        var exprydt = Convert.ToDateTime(data.EXPIRYDATE);
                        var expiryDate = String.Format("{0:" + DateFormat + "}", exprydt);
                        GetSaleDetail.EXPIRYDATE = expiryDate;
                    }

                    GetSaleDetail.LICENSE = data.LICENSE;
                    GetSaleDetail.FK_LEADID = data.FK_LEADID;
                    GetSaleDetail.FK_CREATEDBY = data.FK_CREATEDBY;
                    GetSaleDetail.CREATEDDATE = data.CREATEDDATE;
                    GetSaleDetail.CREATEDDATETIME = data.CREATEDDATETIME;
                    GetSaleDetail.BranchID = data.BranchID;
                    GetSaleDetail.CompanyID = data.CompanyID;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }


            return Json(GetSaleDetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSaleDetail(int LID, string projectDescription, string projectValue, string advancePayment, string advancePayDate, string chequeNo, string completionDate, string expiryDate, string license)
        {
            string msg = "";
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var data = new crm_saledetailtbl();
            var DateFormat = Constant.DateFormat();//get date format by company id
            try
            {
                data = db.crm_saledetailtbl.Where(em => em.FK_LEADID == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (data != null)
                {

                    data.PROJECTDESCRIPTION = projectDescription;
                    data.PROJECTVALUE = Convert.ToDecimal(projectValue);
                    data.ADVANCEPAYMENT = Convert.ToDecimal(advancePayment);
                    if (!string.IsNullOrEmpty(advancePayDate))
                    {
                        var apdt = Convert.ToDateTime(advancePayDate);
                        var advancePDate = String.Format("{0:" + DateFormat + "}", apdt);
                        data.ADVANCEPAYMENTDATE = advancePDate;
                    }
                    data.CHEQUENO = chequeNo;
                    if (!string.IsNullOrEmpty(completionDate))
                    {
                        var comdt = Convert.ToDateTime(completionDate);
                        var comDate = String.Format("{0:" + DateFormat + "}", comdt);
                        data.PROJECTCOMPLETIONDATE = comDate;
                    }
                    if (!string.IsNullOrEmpty(expiryDate))
                    {
                        var Exdt = Convert.ToDateTime(expiryDate);
                        var ExDate = String.Format("{0:" + DateFormat + "}", Exdt);
                        data.EXPIRYDATE = ExDate;
                    }
                    data.LICENSE = license;
                    db.SaveChanges();
                    msg = "success";


                }
                else
                {
                    msg = "error";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "error";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
            //return Redirect("/home/viewsales/?page=1");
        }
        public ActionResult EditSaleDetailuser(int LID, string projectDescription, string projectValue, string advancePayment, string advancePayDate, string chequeNo, string completionDate, string expiryDate, string license)
        {
            string msg = "";
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var data = new crm_saledetailtbl();
            var DateFormat = Constant.DateFormat();//get date format by company id
            try
            {
                data = db.crm_saledetailtbl.Where(em => em.FK_LEADID == LID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (data != null)
                {

                    data.PROJECTDESCRIPTION = projectDescription;
                    data.PROJECTVALUE = Convert.ToDecimal(projectValue);
                    data.ADVANCEPAYMENT = Convert.ToDecimal(advancePayment);
                    if (!string.IsNullOrEmpty(advancePayDate))
                    {
                        var apdt = Convert.ToDateTime(advancePayDate);
                        var advancePDate = String.Format("{0:" + DateFormat + "}", apdt);
                        data.ADVANCEPAYMENTDATE = advancePDate;
                    }
                    data.CHEQUENO = chequeNo;
                    if (!string.IsNullOrEmpty(completionDate))
                    {
                        var comdt = Convert.ToDateTime(completionDate);
                        var comDate = String.Format("{0:" + DateFormat + "}", comdt);
                        data.PROJECTCOMPLETIONDATE = comDate;
                    }
                    if (!string.IsNullOrEmpty(expiryDate))
                    {
                        var Exdt = Convert.ToDateTime(expiryDate);
                        var ExDate = String.Format("{0:" + DateFormat + "}", Exdt);
                        data.EXPIRYDATE = ExDate;
                    }
                    data.LICENSE = license;
                    db.SaveChanges();
                    msg = "success";


                }
                else
                {
                    msg = "error";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "error";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #region Manage-LeadNotify
        public ActionResult LeadNotify(int LeadID)
        {
            string ReturnMsg = "";
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                var GetLeadData = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetLeadData != null)
                {
                    if (GetLeadData.Notify != true)
                    {
                        var NotifyByUser = "Super Admin";
                        if (Session["UserName"] != null)
                        {
                            NotifyByUser = Convert.ToString(Session["UserName"]);
                        }
                        GetLeadData.Notify = true;
                        GetLeadData.NotifybyUser = NotifyByUser;
                        GetLeadData.NotifyDate = System.DateTime.Now;
                        db.SaveChanges();
                        ReturnMsg = "This lead successfully notify to lead owner ";
                    }
                    else
                    {
                        ReturnMsg = "This is already notify to lead owner ";
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return Json(ReturnMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalulateLeadNotify()
        {
            int totalLeads = 0;
            string returnMsg = "";
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    #region If-SuperAdmin
                    var Count = db.crm_createleadstbl.Where(em => em.Notify == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList().Count();
                    if (Count > 0)
                    {
                        totalLeads += Count;
                        returnMsg = "success";
                    }
                    #endregion
                }
                else
                {
                    #region If-User
                    var UID = Convert.ToInt32(Session["UID"]);
                    var GetCountData = db.crm_createleadstbl.Where(em => em.LeadOwner == UID && em.Notify == true && em.BranchID == BranchID && em.CompanyID == CompanyID).ToList().Count();
                    if (GetCountData > 0)
                    {
                        totalLeads += GetCountData;
                        returnMsg = "success";
                    }
                    else
                    {
                        returnMsg = "no";
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                returnMsg = "problem";
            }
            return Json(totalLeads, returnMsg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Show notifcation of leads
        public JsonResult TodayFollowUpLeadCount()
        {

            var model = new NotificationCountModel();

            try
            {
                int TotalLeads = 0;
                int TdFollowupCount = 0;
                int TdLeadCount = 0;
                int TdAssignLeadCount = 0;

                var dd = Constant.GetBharatTime();//get india datetime
                var TodayDate = Convert.ToDateTime(dd.ToString("dd/MM/yyyy"));//get today date for today followup lead link
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
                {
                    #region If-SuperAdmin
                    TdFollowupCount = db.crm_createleadstbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FollowDate == TodayDate && em.LeadStatus != "Not Interested").ToList().Count();
                    //  TdLeadCount = db.crm_createleadstbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && (em.Createddate == TodayDate || em.AssignedDate == TodayDate) && em.LeadStatus != "Not Interested").ToList().Count();
                    // TdAssignLeadCount = db.crm_createleadstbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && (em.AssignedDate == TodayDate) && em.LeadStatus != "Not Interested").ToList().Count();
                    var TdLead = db.crm_createleadstbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.LeadStatus != "Not Interested").ToList();
                    var tdleadlist = TdLead.Where(a => Convert.ToDateTime(Convert.ToDateTime(a.Createddate).ToString("dd/MM/yyyy")) == TodayDate.Date).ToList();
                    TdLeadCount = tdleadlist.Count();
                    var TdAssignLead = db.crm_createleadstbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.LeadStatus != "Not Interested").ToList();
                    var TdAssignLeadlist = TdAssignLead.Where(a => Convert.ToDateTime(Convert.ToDateTime(a.AssignedDate).ToString("dd/MM/yyyy")) == TodayDate.Date).ToList();
                    TdAssignLeadCount = TdAssignLeadlist.Count();
                    TotalLeads = TdFollowupCount + TdLeadCount + TdAssignLeadCount;

                    var TdAssignForm16list = db.crm_formrequest_tbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();

                    model.TotalAllLeadCount = TotalLeads;
                    model.TodayFollowUpCount = TdFollowupCount;
                    model.TodayNewLeadCount = TdLeadCount;
                    model.TodayAssignLeadCount = TdAssignLeadCount;
                    model.TodayAssignExpenseCount = 1;
                    model.TodayAssignForm16Count = TdAssignForm16list.Count();
                    #endregion
                }
                else
                {
                    #region If-User
                    int UID = Convert.ToInt32(Session["UID"]);
                    string userId = UID.ToString();
                    TdFollowupCount = db.crm_createleadstbl.Where(em => (em.LeadOwner == UID || !string.IsNullOrEmpty(em.AssignTo) && em.AssignTo == userId) && em.BranchID == BranchID && em.CompanyID == CompanyID && em.FollowDate == TodayDate && em.LeadStatus != "Not Interested").ToList().Count();
                    // TdLeadCount = db.crm_createleadstbl.Where(em => (em.LeadOwner == UID || (!string.IsNullOrEmpty(em.AssignTo) && em.AssignTo == userId)) && em.BranchID == BranchID && em.CompanyID == CompanyID && (em.Createddate == TodayDate || em.AssignedDate == TodayDate.Date) && em.LeadStatus != "Not Interested").ToList().Count();
                    // TdAssignLeadCount = db.crm_createleadstbl.Where(em => (em.LeadOwner == UID || !string.IsNullOrEmpty(em.AssignTo) && em.AssignTo == userId) && em.BranchID == BranchID && em.CompanyID == CompanyID && (em.AssignedDate == TodayDate) && em.LeadStatus != "Not Interested").ToList().Count();
                    var TdLead = db.crm_createleadstbl.Where(em => (em.LeadOwner == UID || (!string.IsNullOrEmpty(em.AssignTo) && em.AssignTo == userId)) && em.BranchID == BranchID && em.CompanyID == CompanyID && em.LeadStatus != "Not Interested").ToList();
                    var tdleadlist = TdLead.Where(a => Convert.ToDateTime(Convert.ToDateTime(a.Createddate).ToString("dd/MM/yyyy")) == TodayDate.Date).ToList();
                    TdLeadCount = tdleadlist.Count();
                    var TdAssignLead = db.crm_createleadstbl.Where(em => (em.LeadOwner == UID || (!string.IsNullOrEmpty(em.AssignTo) && em.AssignTo == userId)) && em.BranchID == BranchID && em.CompanyID == CompanyID && em.LeadStatus != "Not Interested").ToList();
                    var TdAssignLeadlist = TdAssignLead.Where(a => Convert.ToDateTime(Convert.ToDateTime(a.AssignedDate).ToString("dd/MM/yyyy")) == TodayDate.Date).ToList();
                    TdAssignLeadCount = TdAssignLeadlist.Count();
                    TotalLeads = TdFollowupCount + TdLeadCount + TdAssignLeadCount;
                    TotalLeads = (TdFollowupCount + TdLeadCount + TdAssignLeadCount);

                    model.TotalAllLeadCount = TotalLeads;
                    model.TodayFollowUpCount = TdFollowupCount;
                    model.TodayNewLeadCount = TdLeadCount;
                    model.TodayAssignLeadCount = TdAssignLeadCount;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Send-Message-of-Product
        public ActionResult sendmessage(string SelectedValues, string Message)
        {
            string msg = "";
            try
            {
                string result = apicall("http://198.24.149.4/API/pushsms.aspx?loginID=navigo&password=123456789&mobile=9873555188,7042027474&text=HI HOW ARE YOU&senderid=DEMOOO&route_id=2&Unicode=0");
                //var i=http://198.24.149.4/API/pushsms.aspx?loginID=navigo&password=123456789&mobile=9716603046,9652138542&text=HI HOW ARE YOU&senderid=DEMOOO&route_id=2&Unicode=0;
                if (!result.StartsWith("Wrong Username or Password"))
                {
                    msg = "Sent";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('Message Sent')", true);
                }
                else
                {
                    msg = "Failed";
                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "success", "alert('Message Sending Failed')", true);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = ex.Message.ToString();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public string apicall(string url)
        {
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(url);

            //try
            //{
            httpreq.UseDefaultCredentials = true;
            HttpWebResponse httpres = (HttpWebResponse)httpreq.GetResponse();
            StreamReader sr = new StreamReader(httpres.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
            return results;
            //}
            //catch
            //{
            //    return "0";
            //}
        }

        #endregion

        //  Common for  NIS..................................................................
        public JsonResult ExtraworkingOrLateNightMark(bool ExtraWorking, bool LateNightWorking)
        {
            string ReturnMsg = "";
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    int uid = Convert.ToInt32(Session["UID"]);
                    var CheckUserData = db.crm_usertbl.Where(em => em.Id == uid && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    if (CheckUserData != null)
                    {
                        string UserName = CheckUserData.Fname + " " + CheckUserData.Lname;   //User Full Name
                        string UserEmail = CheckUserData.Email;
                        DateTime dt = Constant.GetBharatTime();
                        String WorkingDayAndDate = "";
                        // var date = dt.ToString("MM/dd/yyyy");
                        var date = dt.ToString("dd/MM/yyyy");
                        WorkingDayAndDate = dt.ToString("dddd, dd MMMM yyyy");   //Saturday, 21 July 2007

                        if (ExtraWorking == true)
                        {
                            #region Extra-Working
                            var getRecords = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == uid && em.L_In_Date == date && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (getRecords != null)
                            {
                                if (getRecords.Extra_working == true)
                                {
                                    ReturnMsg = "You have already marked as extra-working on Today.";
                                }
                                else
                                {
                                    getRecords.Extra_working = true;
                                    int i = db.SaveChanges();
                                    {
                                        TemplateGenerator.ExtraWorkingTemplate(UserName, UserEmail, WorkingDayAndDate, getRecords.L_In_Time, Constant.GetBharatTime().ToShortTimeString(), CompanyID, BranchID);
                                        Thread.Sleep(1000);
                                        ReturnMsg = "Mail sent successfully";
                                    }
                                }
                            }
                            #endregion
                        }
                        if (LateNightWorking == true)
                        {
                            #region LateNight-Working
                            var getRecords = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == uid && em.L_In_Date == date && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (getRecords != null)
                            {
                                if (getRecords.Working_Late_Night == true)
                                {
                                    ReturnMsg = "You have already marked as Late-Night-Working on Today.";
                                }
                                else
                                {
                                    getRecords.Working_Late_Night = true;
                                    db.SaveChanges();
                                    {
                                        TemplateGenerator.LateNightWorkingTemplate(UserName, UserEmail, WorkingDayAndDate, Constant.GetBharatTime().ToShortTimeString(), CompanyID, BranchID);
                                        Thread.Sleep(1000);
                                        ReturnMsg = "Mail sent successfully";
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        ReturnMsg = "Sorry, Your information is not matching. please login again to continue!";
                    }
                }
                else
                {
                    ReturnMsg = "Your session has expire please login again to continue!";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ReturnMsg = ex.Message.ToString();
            }
            return Json(ReturnMsg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDcDates()
        {
            string start = "Q9FnM5mf1Qf/BR9fcc9OeA==";//2020-05-25;
            string end = "gnod5wVZ57mps7LP5nmtbw==";//2021-05-25
            var date = StringCipher.DecryptDate(start, end);
            return Content(date);
        }

        public ActionResult GetToken(int id)
        {
            var token = Utility.TokenGenerator(id);
            return Content(token);
        }

        [HttpPost]
        public JsonResult SaveCurrentLocation(int UserId, string CurrLat, string CurrLong, string CurrAddr, string CurrCity, string CurrState, string CurrCountry)
        {
            var dd = Constant.GetBharatTime();//get india datetime

            try
            {
                if (UserId > 0 && !string.IsNullOrEmpty(CurrLat) && !string.IsNullOrEmpty(CurrLong))
                {

                    var exist = db.crm_tracksaleperson.Where(a => a.UserID == UserId && a.Latitude == CurrLat && a.Longitude == CurrLong && a.TrackDatetime.Value.Year == dd.Year && a.TrackDatetime.Value.Month == dd.Month && a.TrackDatetime.Value.Day == dd.Day).OrderByDescending(a => a.TrackDatetime).FirstOrDefault();
                    if (exist != null)
                    {
                        exist.Latitude = CurrLat;
                        exist.Longitude = CurrLong;
                        exist.TrackDatetime = dd;
                        db.SaveChanges();
                    }
                    else
                    {
                        crm_tracksaleperson tsp = new crm_tracksaleperson();
                        tsp.UserID = UserId;
                        tsp.Address = CurrAddr;
                        tsp.Country = CurrCountry;
                        tsp.StateName = CurrState;
                        tsp.CityName = CurrCity;
                        tsp.Latitude = CurrLat;
                        tsp.Longitude = CurrLong;
                        tsp.TrackDatetime = dd;
                        db.crm_tracksaleperson.Add(tsp);
                        db.SaveChanges();
                    }


                    //string constr = ConfigurationManager.ConnectionStrings["_ConnectionString"].ToString();
                    //var con = new MySqlConnection(constr);
                    //con.Open();
                    ////var sql = "call CRM_SaveTSPLocation(@CurrUserId,@CurrLat,@CurrLong,@CurrAddr,@CurrCity,@CurrState,@CurrCountry,@CurrDate)";
                    //MySqlCommand com = new MySqlCommand("CRM_SaveTSPLocation", con);
                    //com.CommandType = CommandType.StoredProcedure;//System.Data.CommandType.Text;
                    //com.Parameters.Add(new MySqlParameter("CurrUserId", UserId));
                    //com.Parameters.Add(new MySqlParameter("CurrLat", CurrLat.ToString()));
                    //com.Parameters.Add(new MySqlParameter("CurrLong", CurrLong.ToString()));
                    //com.Parameters.Add(new MySqlParameter("CurrAddr", CurrAddr.ToString()));
                    //com.Parameters.Add(new MySqlParameter("CurrCity", CurrCity.ToString()));
                    //com.Parameters.Add(new MySqlParameter("CurrState", CurrState.ToString()));
                    //com.Parameters.Add(new MySqlParameter("CurrCountry", CurrCountry.ToString()));
                    //com.Parameters.Add(new MySqlParameter("CurrDate", dd.ToString()));
                    //var result = com.ExecuteNonQuery();
                    //com.Connection.Close();
                }

            }
            catch (Exception ex)
            {

            }
            return Json("ok");
        }



        [HttpPost]
        public async Task<JsonResult> AddDDLItem(string ItemName, string DropDownType)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            dynamic expando = new ExpandoObject();


            if (Session["CompanyID"] != null)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(ItemName))
                    {
                        expando.Msg = "empty";
                    }
                    else
                    {
                        #region lead source add
                        if (DropDownType == "LeadSource")
                        {
                            var checkExist = await db.crm_leadsource_tbl.Where(em => em.LeadsourceName.ToLower() == ItemName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            if (checkExist == null)
                            {
                                crm_leadsource_tbl lsr = new crm_leadsource_tbl();
                                lsr.LeadsourceName = ItemName;
                                lsr.BranchID = BranchID;
                                lsr.CompanyID = CompanyID;
                                lsr.Status = true;
                                lsr.Created_at = Constant.GetBharatTime();
                                db.crm_leadsource_tbl.Add(lsr);
                                await db.SaveChangesAsync();

                                expando.Msg = "ok";
                                expando.AddedItem = ItemName;
                                expando.ItemId = lsr.Id;
                                //return Json(expando, JsonRequestBehavior.AllowGet);                         
                            }
                            else
                            {
                                expando.Msg = "Lead source already exist";
                                //return Json(expando, JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion

                        #region product type add
                        else if (DropDownType == "ProductType")
                        {
                            var checkExist = await db.crm_producttypetbl.Where(em => em.ProductTypeName.ToLower() == ItemName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            if (checkExist == null)
                            {
                                crm_producttypetbl pdt = new crm_producttypetbl();
                                pdt.ProductTypeName = ItemName;
                                pdt.BranchID = BranchID;
                                pdt.CompanyID = CompanyID;
                                pdt.Status = true;
                                db.crm_producttypetbl.Add(pdt);
                                await db.SaveChangesAsync();

                                expando.Msg = "ok";
                                expando.AddedItem = ItemName;
                                expando.ItemId = pdt.Id;
                                //return Json(expando, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {

                                expando.Msg = "Product type already exist";
                                //return Json(expando, JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion

                        #region lead status add
                        else if (DropDownType == "LeadStatus")
                        {
                            var checkExist = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName.ToLower() == ItemName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            if (checkExist == null)
                            {
                                crm_leadstatus_tbl lst = new crm_leadstatus_tbl();
                                lst.LeadStatusName = ItemName;
                                lst.ColorHexValue = null;
                                lst.BranchID = BranchID;
                                lst.CompanyID = CompanyID;
                                lst.Status = true;
                                lst.created_at = Constant.GetBharatTime();
                                db.crm_leadstatus_tbl.Add(lst);
                                await db.SaveChangesAsync();

                                expando.Msg = "ok";
                                expando.AddedItem = ItemName;
                                expando.ItemId = lst.Id;
                            }
                            else
                            {
                                expando.Msg = "Lead status already exist";
                                //return Json("exist", JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion
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
        public async Task<JsonResult> AddTicketDDLItem(string ItemName, string DropDownType)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            dynamic expando = new ExpandoObject();


            if (Session["CompanyID"] != null)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(ItemName))
                    {
                        expando.Msg = "empty";
                    }
                    else
                    {
                        #region ErrorType add
                        if (DropDownType == "ErrorType")
                        {
                            var checkExist = await db.crm_errortype.Where(em => em.ErrorName.ToLower() == ItemName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            if (checkExist == null)
                            {
                                crm_errortype errType = new crm_errortype();
                                errType.ErrorName = ItemName;
                                errType.BranchID = BranchID;
                                errType.CompanyID = CompanyID;
                                errType.IsActive = true;
                                errType.IsDeleted = false;
                                errType.CreatedOn = Constant.GetBharatTime();
                                errType.CreatedBy = Convert.ToInt32(Session["UID"]);
                                db.crm_errortype.Add(errType);
                                await db.SaveChangesAsync();

                                expando.Msg = "ok";
                                expando.AddedItem = ItemName;
                                expando.ItemId = errType.ErrrorID;
                                //return Json(expando, JsonRequestBehavior.AllowGet);                         
                            }
                            else
                            {
                                expando.Msg = "Error type name already exist";
                                //return Json(expando, JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion

                        #region product type add
                        else if (DropDownType == "ProductType")
                        {
                            var checkExist = await db.crm_producttypetbl.Where(em => em.ProductTypeName.ToLower() == ItemName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            if (checkExist == null)
                            {
                                crm_producttypetbl pdt = new crm_producttypetbl();
                                pdt.ProductTypeName = ItemName;
                                pdt.BranchID = BranchID;
                                pdt.CompanyID = CompanyID;
                                pdt.Status = true;
                                db.crm_producttypetbl.Add(pdt);
                                await db.SaveChangesAsync();

                                expando.Msg = "ok";
                                expando.AddedItem = ItemName;
                                expando.ItemId = pdt.Id;
                                //return Json(expando, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {

                                expando.Msg = "Product type already exist";
                                //return Json(expando, JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion

                        #region UrgencyType add
                        else if (DropDownType == "UrgencyType")
                        {
                            var checkExist = await db.crm_urgency.Where(em => em.urgencyName.ToLower() == ItemName.ToLower() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                            if (checkExist == null)
                            {
                                crm_urgency urg = new crm_urgency();
                                urg.urgencyName = ItemName;
                                urg.BranchID = BranchID;
                                urg.CompanyID = CompanyID;
                                urg.IsActive = true;
                                urg.IsDeleted = false;
                                urg.CreatedOn = Constant.GetBharatTime();
                                urg.CreatedBy = Convert.ToInt32(Session["UID"]);
                                db.crm_urgency.Add(urg);
                                await db.SaveChangesAsync();

                                expando.Msg = "ok";
                                expando.AddedItem = ItemName;
                                expando.ItemId = urg.urgencyID;
                            }
                            else
                            {
                                expando.Msg = "Urgency type already exist";
                                //return Json("exist", JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion
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



        public ActionResult test(string mob)
        {
            if (mob.Length < 9)
            {
                string mob1 = mob;
                mob = "0" + mob1;
            }
            //int CompanyId = 296;
            //string query = @"select count(*) as rowCount from Customers where CompanyId = " + CompanyId + "";

            //int rowCount = db.Database.SqlQuery<int>(query).FirstOrDefault();
            //string cid = "CUSCRM-" + (rowCount + 1);

            //string datePatt = @"M/d/yyyy hh:mm:ss tt";
            ////Get local time Now
            //DateTime now = DateTime.Now.ToLocalTime(); 
            //// To UTC time
            //DateTime utcNow = now.ToUniversalTime();
            //// Go back to local time which will be the same as 'now'
            //DateTime localNow = new DateTime();

            //using (WebResponse response = WebRequest.Create("http://www.microsoft.com").GetResponse())
            //   localNow =DateTime.ParseExact(response.Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);

            //using (var response = WebRequest.Create("http://www.google.com").GetResponse())
            //    //string todaysDates =  response.Headers["date"];
            //    localNow = DateTime.ParseExact(response.Headers["date"],
            //        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
            //        CultureInfo.InvariantCulture.DateTimeFormat,
            //        DateTimeStyles.AssumeUniversal);

            //string nowDate= DisplayNow("Server: ..........", now, datePatt);
            ////string utcNowDate = DisplayNow("UTCNow: .............", utcNow, datePatt);
            //string localDate = DisplayNow("Local: .............", localNow, datePatt);

            //string allDates = string.Format("{0}\n {1}", nowDate,  localDate);
            return Content(mob);
        }

        public static string DisplayNow(string title, DateTime inputDt, string datePatt)
        {
            // Display the date time in the given format
            string dtString = inputDt.ToString(datePatt);
            var dt = string.Format("{0} {1}, Kind = {2}", title, dtString, inputDt.Kind);
            return dt;
        }

        public JsonResult DeleteGoogtransCookie()
        {
            //delete google translate cookie
            HttpCookie gCookie = new HttpCookie("googtrans");
            gCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(gCookie);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SingleLeadReminderStop(int id)
        {
            var loggedUserId = Session["UID"] != null ? Convert.ToInt32(Session["UID"]) : 0;

            try
            {
                if (loggedUserId > 0)
                {
                    //db.Database.ExecuteSqlCommand("update crm_createleadstbl set IsLeadReminder=0 where Id=" + id);
                    if (!db.crm_leadreminderbyuser.Any(a => a.LeadId == id && a.UserId == loggedUserId))
                    {
                        var ld = new crm_leadreminderbyuser
                        {
                            UserId = loggedUserId,
                            LeadId = id,
                            IsLeadReminder = false
                        };
                        db.crm_leadreminderbyuser.Add(ld);
                        db.SaveChanges();
                    }
                }

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Json("err", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MultipleLeadReminderStop(string LeadIdArray)
        {
            var loggedUserId = Session["UID"] != null ? Convert.ToInt32(Session["UID"]) : 0;

            try
            {
                if (loggedUserId > 0 && !string.IsNullOrEmpty(LeadIdArray))
                {
                    var deSerializeIds = new JavaScriptSerializer().Deserialize<string[]>(LeadIdArray);//desrialize  ids
                    //db.Database.ExecuteSqlCommand("update crm_createleadstbl set IsLeadReminder=0 where Id=" + id);
                    for (int i = 0; i < deSerializeIds.Count(); i++)
                    {
                        int id = Convert.ToInt32(deSerializeIds[i]);
                        if (!db.crm_leadreminderbyuser.Any(a => a.LeadId == id && a.UserId == loggedUserId))
                        {
                            var ld = new crm_leadreminderbyuser
                            {
                                UserId = loggedUserId,
                                LeadId = id,
                                IsLeadReminder = false
                            };
                            db.crm_leadreminderbyuser.Add(ld);
                            db.SaveChanges();
                        }
                    }

                }

                //string result = string.Join(",", deSerializeIds);
                //db.Database.ExecuteSqlCommand("update crm_createleadstbl set IsLeadReminder=0 where Id in ("+ result + ")");

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                return Json("err", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTimeZones(string countryCode)
        {
            string qury = string.Format("select zone_name, standard_zone_name as StandardTZName from time_zone where country_code='{0}'", countryCode);
            var tzData = db.Database.SqlQuery<TZName>(qury).ToList();
            return Json(tzData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ForgotPassword(string UserEmail)
        {
            string msg = "not send";
            //var body = new StringBuilder();
            //body.AppendFormat("Dear, <br /> Arun Kumar");
            //body.AppendFormat("Your Ticket No:1120030,<br /> ");
            //body.AppendFormat("Issue has been updated description:Test mail is sending or not<br /> ");
            //body.AppendLine("To more details click <a href=\"https://www.smartcapita.com/view_tickets\">here</a>");
            //var SendNow = EmailUtility.SendTicketEmailToCustomer("arunkumar@nicoleinfosoft.com", "Ticket Update", body.ToString(),296,173);

            try
            {
                if (!string.IsNullOrEmpty(UserEmail))
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(UserEmail);
                    if (match.Success)
                    {
                        var GetUserData = db.crm_usertbl.Where(x => x.Email == UserEmail && x.Status == true).FirstOrDefault();
                        if (GetUserData != null)
                        {
                            string UserPassword = string.Empty;
                            if (!string.IsNullOrEmpty(GetUserData.KeyVersion))
                            {
                                #region password decryption
                                byte[] iv1;
                                byte[] key = EncriptAES.getdcriptkey(out iv1);
                                string decryptPwd = EncriptAES.DecryptString(GetUserData.Password, key, iv1);
                                #endregion
                                UserPassword = decryptPwd;//show decrypted password

                            }
                            else
                            {
                                UserPassword = GetUserData.Password;
                            }
                            string personName = string.Format("{0} {1}", GetUserData.Fname, GetUserData.Lname);

                            string htmlBody = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/MailTemplate/forgot_password.html"));

                            htmlBody = htmlBody.Replace("##PersonName##", personName);
                            htmlBody = htmlBody.Replace("##UserId##", GetUserData.UserName);
                            htmlBody = htmlBody.Replace("##Password##", UserPassword);

                            bool SendNow = EmailUtility.SendTicketEmailToCustomer(UserEmail, "Forgot Password", htmlBody, Convert.ToInt32(GetUserData.CompanyID), Convert.ToInt32(GetUserData.BranchID));
                            if (SendNow)
                            {
                                msg = "ok";
                            }
                            else
                            {
                                msg = "Something went wrong! Please try after some time";
                            }

                            return Json(msg, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            msg = "No record found";
                            return Json(msg, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        msg = "Invalid email address";
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    msg = "Please enter email address";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                msg = "Something went wrong";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ArchiveLeadById(long LeadIdArchive)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string msg = string.Empty;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var checkExist = await db.crm_leadstatus_tbl.Where(em => em.LeadStatusName.ToLower() == "archive" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                    if (checkExist == null)
                    {
                        crm_leadstatus_tbl lst = new crm_leadstatus_tbl();
                        lst.LeadStatusName = "Archive";
                        lst.BranchID = BranchID;
                        lst.CompanyID = CompanyID;
                        lst.Status = true;
                        lst.created_at = Constant.GetBharatTime();
                        db.crm_leadstatus_tbl.Add(lst);
                        await db.SaveChangesAsync();

                        var leadDeatil = await db.crm_createleadstbl.Where(a => a.Id == LeadIdArchive).FirstOrDefaultAsync();
                        if (leadDeatil != null)
                        {
                            leadDeatil.LeadStatusID = lst.Id;
                            leadDeatil.LeadStatus = "Archive";
                            await db.SaveChangesAsync();
                            msg = "ok";
                        }
                        else
                        {
                            msg = "error";
                        }
                        trans.Commit();//save all transaction 
                    }
                    else
                    {
                        var leadDeatil = await db.crm_createleadstbl.Where(a => a.Id == LeadIdArchive).FirstOrDefaultAsync();
                        if (leadDeatil != null)
                        {
                            leadDeatil.LeadStatusID = checkExist.Id;
                            leadDeatil.LeadStatus = "Archive";
                            await db.SaveChangesAsync();
                            msg = "ok";
                        }
                        else
                        {
                            msg = "error";
                        }
                        trans.Commit();//save all transaction 
                    }


                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    trans.Rollback();//rollback all transaction if erros has been occurred
                    msg = "error";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}
