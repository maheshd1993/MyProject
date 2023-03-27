using AutoMapper;
using Svam.EF;
using Svam.Models;
using Svam.Repository;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Traders.Mailer;
using Traders.Models;

namespace Svam.Controllers
{
    [NoCache]
    public class TicketsController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        CommonRepository cr = new CommonRepository();

        [HttpGet]
        public ActionResult CreateTicket(Int32? id)
        {
            CRMTicketModel CTM = new CRMTicketModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            int LoggedUID = Convert.ToInt32(Session["UID"]);

            CTM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            CTM.propVal = cr.GetCreateTicketSetting();
            var rights = cr.GetUserRights();
            try
            {
                TempData["TicketID"] = id;
                CTM.CreatedDate = Constant.GetBharatTime().ToString(CTM.DateFormat);
                #region Error Type/Product Type/Urgency/Status
                string ptQury = @"select Id as ProductTypeID, ProductTypeName from crm_producttypetbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and Status=1";
                var getProductType = db.Database.SqlQuery<CRMTicketModel>(ptQury).OrderBy(a => a.ProductTypeName).ToList();
                if (getProductType.Count > 0)
                {
                    CTM.ProductTypeList = getProductType;
                }

                string urgQry = @"select UrgencyID, UrgencyName from crm_urgency Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and IsActive=1";
                var getUrgency = db.Database.SqlQuery<CRMTicketModel>(urgQry).OrderBy(a => a.UrgencyName).ToList();
                if (getUrgency.Count > 0)
                {
                    CTM.UrgencyList = getUrgency;
                }

                string errQry = @"select ErrrorID as ErrorTypeID,ErrorName as ErrorTypeName  from crm_errortype Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and IsActive=1";
                var getError = db.Database.SqlQuery<CRMTicketModel>(errQry).OrderBy(a => a.ErrorTypeName).ToList();

                if (getError.Count > 0)
                {
                    CTM.ErrorTypeList = getError;
                }

                string Ticketdropdown1Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown1Text'";
                var Ticketdropdown1 = db.Database.SqlQuery<Ticketdropdownmodel1>(Ticketdropdown1Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                if (Ticketdropdown1.Count > 0)
                {
                    CTM.Ticketdropdownlist1 = Ticketdropdown1;
                }

                string Ticketdropdown2Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown2Text'";
                var Ticketdropdown2 = db.Database.SqlQuery<Ticketdropdownmodel2>(Ticketdropdown2Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                if (Ticketdropdown2.Count > 0)
                {
                    CTM.Ticketdropdownlist2 = Ticketdropdown2;
                }

                string Ticketdropdown3Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown3Text'";
                var Ticketdropdown3 = db.Database.SqlQuery<Ticketdropdownmodel3>(Ticketdropdown3Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                if (Ticketdropdown3.Count > 0)
                {
                    CTM.Ticketdropdownlist3 = Ticketdropdown3;
                }

                string Ticketdropdown4Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown4Text'";
                var Ticketdropdown4 = db.Database.SqlQuery<Ticketdropdownmodel4>(Ticketdropdown4Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                if (Ticketdropdown4.Count > 0)
                {
                    CTM.Ticketdropdownlist4 = Ticketdropdown4;
                }

                string Ticketdropdown5Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown5Text'";
                var Ticketdropdown5 = db.Database.SqlQuery<Ticketdropdownmodel5>(Ticketdropdown5Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                if (Ticketdropdown5.Count > 0)
                {
                    CTM.Ticketdropdownlist5 = Ticketdropdown5;
                }
                List<CRMTicketModel> TicketStatusList = new List<CRMTicketModel>();
                TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Open", TicketStatusID = 1 });
                TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Closed", TicketStatusID = 2 });
                TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Solved", TicketStatusID = 3 });
                TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Pending", TicketStatusID = 4 });
                TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Cancelled", TicketStatusID = 5 });
                CTM.TicketStatusList = TicketStatusList;



                string Customername = @"select Id,Customer as CustomerName from crm_createleadstbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and AssignTo='" + LoggedUID + "'";
                var getcustomer = db.Database.SqlQuery<CRMTicketModel>(Customername).OrderBy(a => a.leadCustomerName).ToList();

                if (getcustomer.Count > 0)
                {
                    CTM.CustomerList = getcustomer;
                }
                else
                {
                    string Customername1 = @"select Id,Customer as CustomerName from crm_createleadstbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "'";
                    var getcustomer1 = db.Database.SqlQuery<CRMTicketModel>(Customername1).OrderBy(a => a.leadCustomerName).ToList();
                    if (getcustomer1.Count > 0)
                    {
                        CTM.CustomerList = getcustomer1;
                    }

                }

                //DataTable dtCustomerList = DataAccessLayer.GetDataTable("call CRM_TicketCustomer(" + CompanyID + "," + BranchID + ",0)");
                //if (dtCustomerList.Rows.Count > 0)
                //{
                //    List<CRMTicketModel> CustomerList = new List<CRMTicketModel>();
                //    for (int i = 0; i < dtCustomerList.Rows.Count; i++)
                //    {
                //        CRMTicketModel customer = new CRMTicketModel();
                //        customer.CustomerID = Convert.ToInt32(dtCustomerList.Rows[i]["CustoemrID"]);
                //        customer.CustomerName = Convert.ToString(dtCustomerList.Rows[i]["CustomerName"]);
                //        CustomerList.Add(customer);
                //    }
                //    CTM.CustomerList = CustomerList;
                //}
                #endregion


                CTM.IsProdTypeAdd = rights == null ? false : rights.IsProductTypes;//get rigts for add product if not in list
                CTM.IsErrorTypeAdd = rights == null ? false : rights.IsErrorTypes;//get rigts for add error type if not in list
                CTM.IsUrgencyTypeAdd = rights == null ? false : rights.IsUrgencyTypes;//get rigts for add urgency type if not in list
                
                if (id > 0 && id != null)
                {
                    var ticketdetail = db.crm_tickets.Where(em => em.CompanyId == CompanyID && em.BranchID == BranchID && em.TicketID == id).FirstOrDefault();
                    if (ticketdetail != null)
                    {
                        CTM.TicketID = ticketdetail.TicketID;
                        if (ticketdetail.CustomerID != null)
                        {
                            CTM.CustomerID = ticketdetail.CustomerID;
                            CTM.ExistingNew = "Existing";
                            CTM.CustomerName = ticketdetail.Name;
                            CTM.CustomerNM = ticketdetail.Name;
                            CTM.leadCustomerName = ticketdetail.Name;
                            CTM.EmailID = ticketdetail.EmailID;
                        }
                        else
                        {
                            CTM.ExistingNew = "New";
                            CTM.NewCustomerName = ticketdetail.Name;
                            CTM.CustomerNM = ticketdetail.Name;
                            CTM.leadCustomerName = ticketdetail.Name;
                        }
                        CTM.AssignedTo = ticketdetail.AssignedTo;
                        CTM.TicketNo = ticketdetail.TicketNo;
                        CTM.TicketSubject = ticketdetail.subject;
                        CTM.PhoneNumber = ticketdetail.PhoneNumber;
                        CTM.EmailID = ticketdetail.EmailID;
                        CTM.ProductTypeID = ticketdetail.ProductTypeID;
                        CTM.UrgencyID = ticketdetail.UrgencyID;
                        CTM.ErrorTypeID = ticketdetail.ErrorTypeID;
                        CTM.TicketStatusID = ticketdetail.StatusID;
                        CTM.ExtraCol1 = ticketdetail.ExtraCol1;
                        CTM.ExtraCol2 = ticketdetail.ExtraCol2;
                        CTM.ExtraCol3 = ticketdetail.ExtraCol3;
                        CTM.ExtraCol4 = ticketdetail.ExtraCol4;
                        CTM.ExtraCol5 = ticketdetail.ExtraCol5;
                        CTM.ExtraCol6 = ticketdetail.ExtraCol6;
                        CTM.ExtraCol7 = ticketdetail.ExtraCol7;
                        CTM.ExtraCol8 = ticketdetail.ExtraCol8;
                        if (ticketdetail.ExtraCol9 != null)
                        {
                            CTM.ExtraCol9 = String.Format("{0:" + CTM.DateFormat + "}", ticketdetail.ExtraCol9);
                        }
                        if (ticketdetail.ExtraCol10 != null)
                        {
                            CTM.ExtraCol10 = String.Format("{0:" + CTM.DateFormat + "}", ticketdetail.ExtraCol10);
                        }
                        CTM.ExtraCol11 = ticketdetail.ExtraCol11;
                        CTM.ExtraCol12 = ticketdetail.ExtraCol12;
                        CTM.T_DropdownitemId1 = ticketdetail.Extracoldropdown1;
                        CTM.T_DropdownitemId2 = ticketdetail.Extracoldropdown2;
                        CTM.T_DropdownitemId3 = ticketdetail.Extracoldropdown3;
                        CTM.T_DropdownitemId4 = ticketdetail.Extracoldropdown4;
                        CTM.T_DropdownitemId5 = ticketdetail.Extracoldropdown5;
                        //CTM.T_DropdownitemId1 = ticketdetail.Extracoldropdown1;
                        //CTM.T_DropdownitemId2 = ticketdetail.Extracoldropdown2;
                        //CTM.T_DropdownitemId3 = ticketdetail.Extracoldropdown3;
                        //CTM.T_DropdownitemId4 = ticketdetail.Extracoldropdown4;
                        //CTM.T_DropdownitemId5 = ticketdetail.Extracoldropdown5;
                        //if(!string.IsNullOrEmpty(ticketdetail.ImageCol1))
                        //{
                        //    string file= System.Web.HttpContext.Current.Server.MapPath("~/TicketAttachment/" + ticketdetail.ImageCol1);
                        //    var fi  = new FileInfo(file);
                        //    CTM.ImageCol1Ext = fi.Extension;
                        //    CTM.ImageCol1 = ticketdetail.ImageCol1;

                        //}
                        //if (!string.IsNullOrEmpty(ticketdetail.ImageCol2))
                        //{
                        //    string file = System.Web.HttpContext.Current.Server.MapPath("~/TicketAttachment/" + ticketdetail.ImageCol2);
                        //    var fi = new FileInfo(file);
                        //    CTM.ImageCol2Ext = fi.Extension;
                        //    CTM.ImageCol2 = ticketdetail.ImageCol2;

                        //}
                        //if (!string.IsNullOrEmpty(ticketdetail.ImageCol3))
                        //{
                        //    string file = System.Web.HttpContext.Current.Server.MapPath("~/TicketAttachment/" + ticketdetail.ImageCol3);
                        //    var fi = new FileInfo(file);
                        //    CTM.ImageCol3Ext = fi.Extension;
                        //    CTM.ImageCol3 = ticketdetail.ImageCol3;

                        //}
                        //if (!string.IsNullOrEmpty(ticketdetail.ImageCol4))
                        //{
                        //    string file = System.Web.HttpContext.Current.Server.MapPath("~/TicketAttachment/" + ticketdetail.ImageCol4);
                        //    var fi = new FileInfo(file);
                        //    CTM.ImageCol4Ext = fi.Extension;
                        //    CTM.ImageCol4 = ticketdetail.ImageCol4;

                        //}
                        if (CTM.TicketStatusID == 1)
                        {
                            CTM.TicketStatusName = "Open";
                        }
                        else if (CTM.TicketStatusID == 2)
                        {
                            CTM.TicketStatusName = "Closed";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                TempData["alert"] = "** Sorry there is some technical problem. please try again.";
            }
            return View(CTM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTicket(CRMTicketModel CTM, int? id, HttpPostedFileBase file)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    CTM.DateFormat = Constant.DateFormat();//get date format by company id
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

                    CTM.propVal = cr.GetCreateTicketSetting();
                    TempData["TicketID"] = id;
                    #region Error Type/Product Type/Urgency
                    string ptQury = @"select Id as ProductTypeID, ProductTypeName from crm_producttypetbl Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and Status=1";
                    var getProductType = db.Database.SqlQuery<CRMTicketModel>(ptQury).OrderBy(a => a.ProductTypeName).ToList();
                    if (getProductType.Count > 0)
                    {
                        CTM.ProductTypeList = getProductType;
                    }

                    string urgQry = @"select UrgencyID, UrgencyName from crm_urgency Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and IsActive=1";
                    var getUrgency = db.Database.SqlQuery<CRMTicketModel>(urgQry).OrderBy(a => a.UrgencyName).ToList();
                    if (getUrgency.Count > 0)
                    {
                        CTM.UrgencyList = getUrgency;
                    }

                    string errQry = @"select ErrrorID as ErrorTypeID,ErrorName as ErrorTypeName  from crm_errortype Where BranchID = '" + BranchID + "' and CompanyID = '" + CompanyID + "' and IsActive=1";
                    var getError = db.Database.SqlQuery<CRMTicketModel>(errQry).OrderBy(a => a.ErrorTypeName).ToList();

                    if (getError.Count > 0)
                    {
                        CTM.ErrorTypeList = getError;
                    }
                    string Ticketdropdown1Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown1Text'";
                    var Ticketdropdown1 = db.Database.SqlQuery<Ticketdropdownmodel1>(Ticketdropdown1Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                    if (Ticketdropdown1.Count > 0)
                    {
                        CTM.Ticketdropdownlist1 = Ticketdropdown1;
                    }

                    string Ticketdropdown2Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown2Text'";
                    var Ticketdropdown2 = db.Database.SqlQuery<Ticketdropdownmodel2>(Ticketdropdown2Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                    if (Ticketdropdown2.Count > 0)
                    {
                        CTM.Ticketdropdownlist2 = Ticketdropdown2;
                    }

                    string Ticketdropdown3Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown3Text'";
                    var Ticketdropdown3 = db.Database.SqlQuery<Ticketdropdownmodel3>(Ticketdropdown3Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                    if (Ticketdropdown3.Count > 0)
                    {
                        CTM.Ticketdropdownlist3 = Ticketdropdown3;
                    }

                    string Ticketdropdown4Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown4Text'";
                    var Ticketdropdown4 = db.Database.SqlQuery<Ticketdropdownmodel4>(Ticketdropdown4Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                    if (Ticketdropdown4.Count > 0)
                    {
                        CTM.Ticketdropdownlist4 = Ticketdropdown4;
                    }

                    string Ticketdropdown5Qry = @"select T_DropdownitemId as T_DropdownitemId,T_DropDownItemName as T_DropDownItemName  from crm_Ticketdropdownlist_tbl Where T_BranchID = '" + BranchID + "' and T_CompanyID = '" + CompanyID + "' and Status=0 and T_DropDownfield='ExtraColdropdown5Text'";
                    var Ticketdropdown5 = db.Database.SqlQuery<Ticketdropdownmodel5>(Ticketdropdown5Qry).OrderBy(a => a.T_DropDownItemName).ToList();

                    if (Ticketdropdown5.Count > 0)
                    {
                        CTM.Ticketdropdownlist5 = Ticketdropdown5;
                    }
                    List<CRMTicketModel> TicketStatusList = new List<CRMTicketModel>();
                    TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Open", TicketStatusID = 1 });
                    TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Closed", TicketStatusID = 2 });
                    CTM.TicketStatusList = TicketStatusList;


                    DataTable dtCustomerList = DataAccessLayer.GetDataTable("call CRM_TicketCustomer(" + CompanyID + "," + BranchID + ",0)");
                    if (dtCustomerList.Rows.Count > 0)
                    {
                        List<CRMTicketModel> CustomerList = new List<CRMTicketModel>();
                        for (int i = 0; i < dtCustomerList.Rows.Count; i++)
                        {
                            CRMTicketModel customer = new CRMTicketModel();
                            customer.CustomerID = Convert.ToInt32(dtCustomerList.Rows[i]["CustoemrID"]);
                            customer.CustomerName = Convert.ToString(dtCustomerList.Rows[i]["CustomerName"]);
                            CustomerList.Add(customer);
                        }
                        CTM.CustomerList = CustomerList;
                    }
                    #endregion

                    if (!ModelState.IsValid)
                    {
                        return View(CTM);
                    }
                    DateTime localTime = Constant.GetBharatTime();


                    if (CTM.TicketID > 0)
                    {
                        var CheckTicket = db.crm_tickets.Where(em => em.TicketID == id && em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                        if (CheckTicket != null)
                        {
                            var CheckMapTicket = db.crm_tickestmap.Where(em => em.TicketId == id && em.CreatedOn.Value.Year == localTime.Year && em.CreatedOn.Value.Month == localTime.Month && em.CreatedOn.Value.Day == localTime.Day
                            && em.BranchId == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                            if (CheckMapTicket != null || !string.IsNullOrEmpty(CTM.TicketDescription))
                            {
                                CheckTicket.EmailID = CTM.EmailID;
                                CheckTicket.PhoneNumber = CTM.PhoneNumber;
                                CheckTicket.StatusID = CTM.TicketStatusID;
                                CheckTicket.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                CheckTicket.ModifiedOn = localTime;
                                CheckTicket.CompanyId = CompanyID;
                                CheckTicket.BranchID = BranchID;

                                CheckTicket.ExtraCol1 = CTM.ExtraCol1;
                                CheckTicket.ExtraCol2 = CTM.ExtraCol2;
                                CheckTicket.ExtraCol3 = CTM.ExtraCol3;
                                CheckTicket.ExtraCol4 = CTM.ExtraCol4;
                                CheckTicket.ExtraCol5 = CTM.ExtraCol5;
                                CheckTicket.ExtraCol6 = CTM.ExtraCol6;
                                CheckTicket.ExtraCol7 = CTM.ExtraCol7;
                                CheckTicket.ExtraCol8 = CTM.ExtraCol8;
                                CheckTicket.Sparepartstatus = CTM.Sparepartstatus;
                                if (!string.IsNullOrEmpty(CTM.ExtraCol9))
                                {
                                    CheckTicket.ExtraCol9 = DateTime.ParseExact(CTM.ExtraCol9, CTM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", CTM.ExtraCol9)); 
                                }
                                if (!string.IsNullOrEmpty(CTM.ExtraCol10))
                                {
                                    CheckTicket.ExtraCol10 = DateTime.ParseExact(CTM.ExtraCol10, CTM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", CTM.ExtraCol10));
                                }
                                CheckTicket.ExtraCol11 = CTM.ExtraCol11;
                                CheckTicket.ExtraCol12 = CTM.ExtraCol12;
                                CheckTicket.Extracoldropdown1 = CTM.T_DropdownitemId1;
                                CheckTicket.Extracoldropdown2 = CTM.T_DropdownitemId2;
                                CheckTicket.Extracoldropdown3 = CTM.T_DropdownitemId3;
                                CheckTicket.Extracoldropdown4 = CTM.T_DropdownitemId4;
                                CheckTicket.Extracoldropdown5 = CTM.T_DropdownitemId5;
                                if (!string.IsNullOrEmpty(CTM.ProductTypeName))
                                {
                                    crm_producttypetbl ptn = new crm_producttypetbl();
                                    ptn.ProductTypeName = CTM.ProductTypeName;
                                    ptn.BranchID = BranchID;
                                    ptn.CompanyID = CompanyID;
                                    ptn.Status = true;
                                    db.crm_producttypetbl.Add(ptn);
                                    db.SaveChanges();

                                    CheckTicket.ProductTypeID = ptn.Id;

                                }
                                else
                                {
                                    CheckTicket.ProductTypeID = CTM.ProductTypeID;
                                }

                                if (!string.IsNullOrEmpty(CTM.ErrorTypeName))
                                {
                                    crm_errortype etn = new crm_errortype();
                                    etn.ErrorName = CTM.ErrorTypeName;
                                    etn.BranchID = BranchID;
                                    etn.CompanyID = CompanyID;
                                    etn.IsActive = true;
                                    etn.IsDeleted = false;
                                    etn.CreatedOn = localTime;
                                    etn.CreatedBy = Convert.ToInt32(Session["UID"]);
                                    db.crm_errortype.Add(etn);
                                    db.SaveChanges();

                                    CheckTicket.ErrorTypeID = etn.ErrrorID;
                                }
                                else
                                {
                                    CheckTicket.ErrorTypeID = CTM.ErrorTypeID;
                                }

                                if (!string.IsNullOrEmpty(CTM.UrgencyName))
                                {
                                    crm_urgency un = new crm_urgency();
                                    un.urgencyName = CTM.UrgencyName;
                                    un.BranchID = BranchID;
                                    un.CompanyID = CompanyID;
                                    un.IsActive = true;
                                    un.IsDeleted = false;
                                    un.CreatedOn = localTime;
                                    un.CreatedBy = Convert.ToInt32(Session["UID"]);
                                    db.crm_urgency.Add(un);
                                    db.SaveChanges();

                                    CheckTicket.UrgencyID = un.urgencyID;
                                }
                                else
                                {
                                    CheckTicket.UrgencyID = CTM.UrgencyID;
                                }

                                #region Add-Ticket-Description
                                string FileName = string.Empty;
                                string FileFullName = string.Empty;
                                if (file != null)
                                {
                                    var customerName = "";
                                    if (CTM.CustomerID > 0)
                                    {
                                        customerName = CTM.CustomerName;
                                    }
                                    else
                                    {
                                        customerName = CTM.NewCustomerName;
                                    }
                                    if (file.ContentLength > 0)
                                    {
                                        var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                                        var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                                        if (supportedTypes.Contains(fileExt))
                                        {
                                            string extension = Path.GetExtension(file.FileName);
                                            FileName = "Ticket-" + Convert.ToString(Session["UserName"]).Trim() + "-" + customerName.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                            FileFullName = FileName + extension;
                                            string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                                            file.SaveAs(_path);
                                        }
                                        else
                                        {
                                            TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                            //return Redirect("/Tickets/CreateTicket");
                                            return View(CTM);
                                        }
                                    }
                                }

                                var tid = CTM.TicketID;
                                if (!string.IsNullOrEmpty(CTM.TicketDescription))
                                {
                                    crm_tickestmap LD = new crm_tickestmap();
                                    LD.TicketId = tid;
                                    LD.Message = CTM.TicketDescription;
                                    LD.CreatedOn = localTime.Date;
                                    LD.CreatedBy = Convert.ToInt32(Session["UID"]);
                                    LD.BranchId = Convert.ToInt32(Session["BranchID"]);
                                    LD.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                                    LD.AttachmentFile = FileFullName;
                                    LD.StatusName = CTM.TicketStatusID == 1 ? "Open" : "Closed";
                                    db.crm_tickestmap.Add(LD);
                                    db.SaveChanges();
                                }

                                #endregion

                                #region team remark
                                if (!string.IsNullOrEmpty(CTM.TeamRemark))
                                {
                                    crm_ticketremarkforteam tr = new crm_ticketremarkforteam();
                                    tr.TicketId = tid ?? 0;
                                    tr.Message = CTM.TeamRemark;
                                    tr.CreatedOn = localTime.Date;
                                    tr.CreatedBy = Convert.ToInt32(Session["UID"]);
                                    tr.BranchId = Convert.ToInt32(Session["BranchID"]);
                                    tr.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                                    tr.AttachmentFile = FileFullName;
                                    tr.StatusName = CTM.TicketStatusID == 1 ? "Open" : "Closed";
                                    db.crm_ticketremarkforteam.Add(tr);
                                    db.SaveChanges();
                                }

                                #endregion

                                #region save images/file
                                //if (CTM.ImageCol1File != null && CTM.ImageCol1File.ContentLength > 0)
                                //{

                                //    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                                //    var fileExt = System.IO.Path.GetExtension(CTM.ImageCol1File.FileName).Substring(1);
                                //    if (supportedTypes.Contains(fileExt))
                                //    {
                                //        string extension = Path.GetExtension(CTM.ImageCol1File.FileName);
                                //        string FileName = "TKTIMG1-" + CTM.PhoneNumber.Trim() + "-" + CTM.CustomerName.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                //        string FileFullName = FileName + extension;
                                //        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                                //        CTM.ImageCol1File.SaveAs(_path);
                                //        CheckTicket.ImageCol1 = FileFullName;
                                //    }
                                //    else
                                //    {
                                //        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                //        //return Redirect("/Tickets/CreateTicket");
                                //        return View(CTM);
                                //    }

                                //}

                                //if (CTM.ImageCol2File != null && CTM.ImageCol2File.ContentLength > 0)
                                //{

                                //    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                                //    var fileExt = System.IO.Path.GetExtension(CTM.ImageCol2File.FileName).Substring(1);
                                //    if (supportedTypes.Contains(fileExt))
                                //    {
                                //        string extension = Path.GetExtension(CTM.ImageCol2File.FileName);
                                //        string FileName = "TKTIMG2-" + CTM.PhoneNumber.Trim() + "-" + CTM.CustomerName.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                //        string FileFullName = FileName + extension;
                                //        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                                //        CTM.ImageCol2File.SaveAs(_path);
                                //        CheckTicket.ImageCol2 = FileFullName;
                                //    }
                                //    else
                                //    {
                                //        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                //        //return Redirect("/Tickets/CreateTicket");
                                //        return View(CTM);
                                //    }

                                //}

                                //if (CTM.ImageCol3File != null && CTM.ImageCol3File.ContentLength > 0)
                                //{

                                //    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                                //    var fileExt = System.IO.Path.GetExtension(CTM.ImageCol3File.FileName).Substring(1);
                                //    if (supportedTypes.Contains(fileExt))
                                //    {
                                //        string extension = Path.GetExtension(CTM.ImageCol3File.FileName);
                                //        string FileName = "TKTIMG3-" + CTM.PhoneNumber.Trim() + "-" + CTM.CustomerName.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                //        string FileFullName = FileName + extension;
                                //        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                                //        CTM.ImageCol3File.SaveAs(_path);
                                //        CheckTicket.ImageCol3 = FileFullName;
                                //    }
                                //    else
                                //    {
                                //        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                //        //return Redirect("/Tickets/CreateTicket");
                                //        return View(CTM);
                                //    }

                                //}

                                //if (CTM.ImageCol4File != null && CTM.ImageCol4File.ContentLength > 0)
                                //{

                                //    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                                //    var fileExt = System.IO.Path.GetExtension(CTM.ImageCol4File.FileName).Substring(1);
                                //    if (supportedTypes.Contains(fileExt))
                                //    {
                                //        string extension = Path.GetExtension(CTM.ImageCol4File.FileName);
                                //        string FileName = "TKTIMG3-" + CTM.PhoneNumber.Trim() + "-" + CTM.CustomerName.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                //        string FileFullName = FileName + extension;
                                //        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                                //        CTM.ImageCol4File.SaveAs(_path);
                                //        CheckTicket.ImageCol4 = FileFullName;
                                //    }
                                //    else
                                //    {
                                //        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                //        //return Redirect("/Tickets/CreateTicket");
                                //        return View(CTM);
                                //    }

                                //}
                                #endregion

                                db.SaveChanges();
                                trans.Commit(); //after data saved then commit transaction
                                TempData["success"] = "Ticket updated successfully";
                            }
                            else
                            {
                                TempData["alert"] = "Please add a description before Update";
                                return View(CTM);
                                //return Redirect("/Tickets/CreateTicket/" + id);
                            }
                        }
                    }
                    else
                    {
                        crm_tickets cts = new crm_tickets();
                        if (Convert.ToInt32(Session["CompanyID"]) == 2644)
                        {
                            cts.TicketNo = GetTicketNo();
                        }
                        else
                        {
                            cts.TicketNo = GenerateNumber();
                        }
                        if (CTM.CustomerID > 0 || CTM.leadCustomerName!=null)
                        {

                            cts.CustomerID = CTM.CustomerList.Where(m=>m.CustomerName.Equals(CTM.leadCustomerName)).Select(c=>c.CustomerID).FirstOrDefault();
                            cts.Name = CTM.CustomerList.Where(m => m.CustomerName.Equals(CTM.leadCustomerName)).Select(c => c.CustomerName).FirstOrDefault();
                        }
                        else
                        {
                            cts.Name = CTM.NewCustomerName;
                        }
                        cts.EmailID = CTM.EmailID;
                        cts.subject = string.IsNullOrEmpty(CTM.TicketSubject) ? "New Ticket" : CTM.TicketSubject;
                        cts.PhoneNumber = CTM.PhoneNumber;
                        cts.StatusID = CTM.TicketStatusID;
                        cts.CreatedBy = Convert.ToInt32(Session["UID"]);
                        cts.CreatedOn = localTime;
                        cts.CompanyId = CompanyID;
                        cts.BranchID = BranchID;
                        cts.ExtraCol1 = CTM.ExtraCol1;
                        cts.ExtraCol2 = CTM.ExtraCol2;
                        cts.ExtraCol3 = CTM.ExtraCol3;
                        cts.ExtraCol4 = CTM.ExtraCol4;
                        cts.ExtraCol5 = CTM.ExtraCol5;
                        cts.ExtraCol6 = CTM.ExtraCol6;
                        cts.ExtraCol7 = CTM.ExtraCol7;
                        cts.ExtraCol8 = CTM.ExtraCol8;
                        if (!string.IsNullOrEmpty(CTM.ExtraCol9))
                        {
                            cts.ExtraCol9 = DateTime.ParseExact(CTM.ExtraCol9, CTM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", CTM.ExtraCol9)); 
                        }
                        if (!string.IsNullOrEmpty(CTM.ExtraCol10))
                        {
                            cts.ExtraCol10 = DateTime.ParseExact(CTM.ExtraCol10, CTM.DateFormat, CultureInfo.InvariantCulture);//Convert.ToDateTime(String.Format("{0:yyyy-MM-dd}", CTM.ExtraCol10));
                        }
                        cts.ExtraCol11 = CTM.ExtraCol11;
                        cts.ExtraCol12 = CTM.ExtraCol12;
                        cts.Extracoldropdown1 = CTM.T_DropdownitemId1;
                        cts.Extracoldropdown2 = CTM.T_DropdownitemId2;
                        cts.Extracoldropdown3 = CTM.T_DropdownitemId3;
                        cts.Extracoldropdown4 = CTM.T_DropdownitemId4;
                        cts.Extracoldropdown5 = CTM.T_DropdownitemId5;
                        if (!string.IsNullOrEmpty(CTM.ProductTypeName))
                        {
                            crm_producttypetbl ptn = new crm_producttypetbl();
                            ptn.ProductTypeName = CTM.ProductTypeName;
                            ptn.BranchID = BranchID;
                            ptn.CompanyID = CompanyID;
                            ptn.Status = true;
                            db.crm_producttypetbl.Add(ptn);
                            db.SaveChanges();

                            cts.ProductTypeID = ptn.Id;

                        }
                        else
                        {
                            cts.ProductTypeID = CTM.ProductTypeID;
                        }

                        if (!string.IsNullOrEmpty(CTM.ErrorTypeName))
                        {
                            crm_errortype etn = new crm_errortype();
                            etn.ErrorName = CTM.ErrorTypeName;
                            etn.BranchID = BranchID;
                            etn.CompanyID = CompanyID;
                            etn.IsActive = true;
                            etn.IsDeleted = false;
                            etn.CreatedOn = localTime;
                            etn.CreatedBy = Convert.ToInt32(Session["UID"]);
                            db.crm_errortype.Add(etn);
                            db.SaveChanges();

                            cts.ErrorTypeID = etn.ErrrorID;
                        }
                        else
                        {
                            cts.ErrorTypeID = CTM.ErrorTypeID;
                        }

                        if (!string.IsNullOrEmpty(CTM.UrgencyName))
                        {
                            crm_urgency un = new crm_urgency();
                            un.urgencyName = CTM.UrgencyName;
                            un.BranchID = BranchID;
                            un.CompanyID = CompanyID;
                            un.IsActive = true;
                            un.IsDeleted = false;
                            un.CreatedOn = localTime;
                            un.CreatedBy = Convert.ToInt32(Session["UID"]);
                            db.crm_urgency.Add(un);
                            db.SaveChanges();

                            cts.UrgencyID = un.urgencyID;
                        }
                        else
                        {
                            cts.UrgencyID = CTM.UrgencyID;
                        }

                        #region save images/file
                        //if (CTM.ImageCol1File != null && CTM.ImageCol1File.ContentLength > 0)
                        //{

                        //        var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                        //        var fileExt = System.IO.Path.GetExtension(CTM.ImageCol1File.FileName).Substring(1);
                        //        if (supportedTypes.Contains(fileExt))
                        //        {
                        //            string extension = Path.GetExtension(CTM.ImageCol1File.FileName);
                        //           string FileName = "TKTIMG1-" + cts.PhoneNumber.Trim() + "-" + cts.Name.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                        //           string FileFullName = FileName + extension;
                        //            string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                        //        CTM.ImageCol1File.SaveAs(_path);
                        //        cts.ImageCol1 = FileFullName;
                        //        }
                        //        else
                        //        {
                        //            TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                        //            //return Redirect("/Tickets/CreateTicket");
                        //            return View(CTM);
                        //        }

                        //}

                        //if (CTM.ImageCol2File != null && CTM.ImageCol2File.ContentLength > 0)
                        //{

                        //    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                        //    var fileExt = System.IO.Path.GetExtension(CTM.ImageCol2File.FileName).Substring(1);
                        //    if (supportedTypes.Contains(fileExt))
                        //    {
                        //        string extension = Path.GetExtension(CTM.ImageCol2File.FileName);
                        //        string FileName = "TKTIMG2-" + cts.PhoneNumber.Trim() + "-" + cts.Name.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                        //        string FileFullName = FileName + extension;
                        //        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                        //        CTM.ImageCol2File.SaveAs(_path);
                        //        cts.ImageCol2 = FileFullName;
                        //    }
                        //    else
                        //    {
                        //        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                        //        //return Redirect("/Tickets/CreateTicket");
                        //        return View(CTM);
                        //    }

                        //}

                        //if (CTM.ImageCol3File != null && CTM.ImageCol3File.ContentLength > 0)
                        //{

                        //    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                        //    var fileExt = System.IO.Path.GetExtension(CTM.ImageCol3File.FileName).Substring(1);
                        //    if (supportedTypes.Contains(fileExt))
                        //    {
                        //        string extension = Path.GetExtension(CTM.ImageCol3File.FileName);
                        //        string FileName = "TKTIMG3-" + cts.PhoneNumber.Trim() + "-" + cts.Name.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                        //        string FileFullName = FileName + extension;
                        //        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                        //        CTM.ImageCol3File.SaveAs(_path);
                        //        cts.ImageCol3 = FileFullName;
                        //    }
                        //    else
                        //    {
                        //        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                        //        //return Redirect("/Tickets/CreateTicket");
                        //        return View(CTM);
                        //    }

                        //}

                        //if (CTM.ImageCol4File != null && CTM.ImageCol4File.ContentLength > 0)
                        //{

                        //    var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                        //    var fileExt = System.IO.Path.GetExtension(CTM.ImageCol4File.FileName).Substring(1);
                        //    if (supportedTypes.Contains(fileExt))
                        //    {
                        //        string extension = Path.GetExtension(CTM.ImageCol4File.FileName);
                        //        string FileName = "TKTIMG3-" + cts.PhoneNumber.Trim() + "-" + cts.Name.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                        //        string FileFullName = FileName + extension;
                        //        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                        //        CTM.ImageCol4File.SaveAs(_path);
                        //        cts.ImageCol4 = FileFullName;
                        //    }
                        //    else
                        //    {
                        //        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                        //        //return Redirect("/Tickets/CreateTicket");
                        //        return View(CTM);
                        //    }

                        //}
                        #endregion
                        db.crm_tickets.Add(cts);
                        if (db.SaveChanges() > 0)
                        {
                            #region Add-Ticket-Description
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
                                        FileName = "Ticket-" + Convert.ToString(Session["UserName"]).Trim() + "-" + cts.TicketNo.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                        FileFullName = FileName + extension;
                                        string _path = Server.MapPath("~/TicketAttachment/" + FileFullName);
                                        file.SaveAs(_path);
                                    }
                                    else
                                    {
                                        TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                                        //return Redirect("/Tickets/CreateTicket");
                                        return View(CTM);
                                    }
                                }
                            }

                            var tid = cts.TicketID;
                            crm_tickestmap LD = new crm_tickestmap();
                            LD.TicketId = tid;
                            LD.Message = CTM.TicketDescription;
                            LD.CreatedOn = localTime.Date;
                            LD.CreatedBy = Convert.ToInt32(Session["UID"]);
                            LD.BranchId = Convert.ToInt32(Session["BranchID"]);
                            LD.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                            LD.AttachmentFile = FileFullName;
                            LD.StatusName = CTM.TicketStatusID == 1 ? "Open" : "Closed";
                            db.crm_tickestmap.Add(LD);
                            db.SaveChanges();
                            #endregion

                            #region team remark
                            if (!string.IsNullOrEmpty(CTM.TeamRemark))
                            {
                                crm_ticketremarkforteam tr = new crm_ticketremarkforteam();
                                tr.TicketId = tid;
                                tr.Message = CTM.TeamRemark;
                                tr.CreatedOn = localTime.Date;
                                tr.CreatedBy = Convert.ToInt32(Session["UID"]);
                                tr.BranchId = Convert.ToInt32(Session["BranchID"]);
                                tr.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                                tr.AttachmentFile = FileFullName;
                                tr.StatusName = CTM.TicketStatusID == 1 ? "Open" : "Closed";
                                db.crm_ticketremarkforteam.Add(tr);
                                db.SaveChanges();
                            }

                            #endregion
                            trans.Commit(); //after data saved then commit transaction
                            TempData["success"] = "Ticket created successfully";

                        }
                    }
                    return Redirect("/Tickets/ViewTicket");
                }
                catch (Exception ex)
                {
                    trans.Rollback(); //rollback all data if exeption occurred
                    ExceptionLogging.SendExcepToDB(ex);
                    TempData["alert"] = "Something went wrong,please try again";
                    return View(CTM);
                    //return Redirect("/Tickets/CreateTicket");
                    //throw ex;
                }
            }

        }

        

        [HttpGet]
        public ActionResult ViewTicket(int? page, String ErrorTypeName, String UrgencyName, String SearchUserID, String SearchFromDate, String SearchToDate, string SearchTerm, string DateType, int? TicketStatusID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);


            if (ErrorTypeName != null && ErrorTypeName != string.Empty)
            {
                Session["ErrorTypeName"] = ErrorTypeName;
            }
            else
            {
                Session["ErrorTypeName"] = string.Empty;
            }

            if (UrgencyName != null && UrgencyName != string.Empty)
            {
                Session["UrgencyName"] = ErrorTypeName;
            }
            else
            {
                Session["UrgencyName"] = string.Empty;
            }

            Int32? UID = 0;
            //if (!String.IsNullOrWhiteSpace(SearchUserID))
            //{
            //    UID = Convert.ToInt32(SearchUserID);
            //    Session["SearchUserID"] = SearchUserID;
            //}            
            //else
            //{
            //    Session["SearchUserID"] = string.Empty;
            //}

            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {

                if (!String.IsNullOrWhiteSpace(SearchUserID))
                {
                    UID = Convert.ToInt32(SearchUserID);
                    Session["SearchUserID"] = SearchUserID;
                }
            }
            else
            {
                UID = Convert.ToInt32(Session["UID"]);
                Session["SearchUserID"] = string.Empty;
            }

            CRMTicketModel CTM = new CRMTicketModel();
            CTM.columnVal = cr.GetViewTicketSetting();
            CTM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid

            #region Default Date show of one month
            var dd = Constant.GetBharatTime();
            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
            #endregion
            if (!string.IsNullOrEmpty(SearchFromDate) && !string.IsNullOrEmpty(SearchToDate))
            {

                if (CTM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    Session["SearchFromDate"] = SearchFromDate;
                    Session["SearchToDate"] = SearchToDate;

                    var fmDate = DateTime.ParseExact(SearchFromDate, CTM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(SearchFromDate);
                    var tDate = DateTime.ParseExact(SearchToDate, CTM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(SearchToDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                }
                else
                {
                    Session["SearchFromDate"] = MStartDate;
                    Session["SearchToDate"] = MEndDate;

                    MStartDate = SearchFromDate;
                    MEndDate = SearchToDate;
                }
            }
            else
            {
                //Session["SearchFromDate"] = MStartDate;
                //Session["SearchToDate"] = MEndDate;
                //MStartDate = SearchFromDate;
                //MEndDate = SearchToDate;

                if (CTM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    Session["SearchFromDate"] = monthStartDate.ToString(CTM.DateFormat);
                    Session["SearchToDate"] = MonthendDate.ToString(CTM.DateFormat);
                }
                else
                {
                    Session["SearchFromDate"] = MStartDate;
                    Session["SearchToDate"] = MEndDate;
                }
            }



            var urgencyList = db.crm_urgency.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.IsDeleted == false && em.IsActive == true).OrderBy(a => a.urgencyName).ToList();
            if (urgencyList != null && urgencyList.Count > 0)
            {
                List<CRMTicketModel> oUrgencyList = new List<CRMTicketModel>();
                foreach (var item in urgencyList)
                {
                    CRMTicketModel oUrgency = new CRMTicketModel();
                    oUrgency.UrgencyID = item.urgencyID;
                    oUrgency.UrgencyName = item.urgencyName;
                    oUrgencyList.Add(oUrgency);
                }
                CTM.UrgencyList = oUrgencyList;
            }
            else
            {
                CTM.UrgencyList = new List<CRMTicketModel>();
            }

            var errorList = db.crm_errortype.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.IsDeleted == false && em.IsActive == true).OrderBy(a => a.ErrorName).ToList();
            if (errorList.Count > 0)
            {
                List<CRMTicketModel> oErrorList = new List<CRMTicketModel>();
                foreach (var item in errorList)
                {
                    CRMTicketModel oError = new CRMTicketModel();
                    oError.ErrorTypeID = item.ErrrorID;
                    oError.ErrorTypeName = item.ErrorName;
                    oErrorList.Add(oError);
                }
                CTM.ErrorTypeList = oErrorList;
            }
            else
            {
                CTM.ErrorTypeList = new List<CRMTicketModel>(); ;
            }

            List<CRMTicketModel> TicketStatusList = new List<CRMTicketModel>();
            TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Open", TicketStatusID = 1 });
            TicketStatusList.Add(new CRMTicketModel { TicketStatusName = "Closed", TicketStatusID = 2 });
            CTM.TicketStatusList = TicketStatusList;

            CTM.TicketStatusID = TicketStatusID;
            CTM.DateType = DateType;

            Int32? UserID = Convert.ToInt32(Session["UID"]);

            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {
                #region Admin View All Users
                var userlist = (from asUser in db.crm_tickets
                                join user in db.crm_usertbl on asUser.AssignedTo equals user.Id
                                join user2 in db.crm_usertbl on asUser.CreatedBy equals user2.Id
                                where (asUser.CreatedBy != UserID || asUser.AssignedTo != null) &&
                                 asUser.BranchID == BranchID
                                && asUser.CompanyId == CompanyID
                                && user.Status == true && user2.Status == true
                                orderby user2.Fname
                                select new CRMTicketModel
                                {
                                    UserID = user.Id,
                                    UserName = user.Fname + " " + user.Lname + "(" + user.EmployeeCode + ")"
                                }
                               ).Distinct().ToList();

                //db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).OrderBy(em => em.Fname).ToList();
                if (userlist != null && userlist.Count > 0)
                {
                    //List<CRMTicketModel> ouserList = new List<CRMTicketModel>();
                    //foreach (var item in userlist)
                    //{
                    //    CRMTicketModel ouser = new CRMTicketModel();
                    //    ouser.UserID = item.Id;
                    //    ouser.UserName = item.Fname + " " + item.Lname;
                    //    ouserList.Add(ouser);
                    //}
                    CTM.UserList = userlist.OrderBy(a => a.UserName).ToList();
                }
                #endregion
            }
            else
            {
                #region Employee will view only mapped user
                var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetUserData != null && GetUserData.MappedUsers != null)
                {
                    List<CRMTicketModel> ouserList = new List<CRMTicketModel>();
                    string mapUser = GetUserData.MappedUsers;
                    CTM.MappedUser = mapUser;
                    var GetMapUser = GetUserData.MappedUsers.Split(',');
                    foreach (var item in GetMapUser)
                    {
                        var mapid = Convert.ToInt32(item);
                        var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetMapUserData != null)
                        {
                            CRMTicketModel ouser = new CRMTicketModel();
                            ouser.UserID = mapid;
                            ouser.UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname;
                            ouserList.Add(ouser);
                        }
                    }
                    CTM.UserList = ouserList.OrderBy(a => a.UserName).ToList();
                }
                #endregion
            }

            //DataTable GetAllLeadCount = DataAccessLayer.GetDataTable("call CRM_TotalTickets(" + CompanyID + "," + BranchID + "," + UID + ",'" + MStartDate + "','" + MEndDate + "')");
            //if (GetAllLeadCount.Rows.Count > 0)
            //{
            //    CTM.TotalTicket = Convert.ToInt32(GetAllLeadCount.Rows[0]["TotalTicket"]);
            //}
            //else
            //{
            //    CTM.TotalTicket = 0;
            //}
            //if (UID == 2879)
            //{
            //    if (UID != 0)
            //    {
            //        var AssignList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id==UID).OrderBy(em => em.Fname).FirstOrDefault();
            //        if (AssignList != null)
            //        {
            //            CRMTicketModel CRM = new CRMTicketModel();

            //            var GetMapUser = AssignList.MappedUsers.Split(',');
            //            foreach (var item in GetMapUser)
            //            {
            //                int id = Convert.ToInt32(item);
            //                var AssignList1 = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id== id).OrderBy(em => em.Fname).ToList();
            //                if (AssignList1 != null)
            //                {
            //                    List<CRMTicketModel> assignToList = new List<CRMTicketModel>();
            //                    foreach (var item1 in AssignList1)
            //                    {                                    
            //                        CRM.UserID = item1.Id;
            //                        CRM.UserName = item1.Fname + ' ' + item1.Lname + '(' + item1.EmployeeCode + ')';
            //                        assignToList.Add(CRM);

            //                    }
            //                    CTM.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
            //                }

            //            }

            //        }
            //    }
            //}
            //else
            //{
            var AssignList1 = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Id == UID).OrderBy(em => em.Fname).FirstOrDefault();

            if (AssignList1 != null)
            {
                if (AssignList1.MappedUsers != null)
                {
                    List<CRMTicketModel> assignToList = new List<CRMTicketModel>();
                    var GetMapUser = AssignList1.MappedUsers.Split(',');
                    foreach (var item in GetMapUser)
                    {
                        var mapid = Convert.ToInt32(item);
                        var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetMapUserData != null)
                        {
                            int roleid = Convert.ToInt32(GetMapUserData.ProfileId);
                            var roledata = db.crm_roleassigntbl.Where(em => em.Id == roleid && em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true).FirstOrDefault();
                            if (roledata.ViewTicket == true)
                            {
                                CRMTicketModel ouser = new CRMTicketModel();
                                ouser.UserID = mapid;
                                ouser.UserName = GetMapUserData.Fname + ' ' + GetMapUserData.Lname + '(' + GetMapUserData.EmployeeCode + ')';
                                assignToList.Add(ouser);
                            }
                        }
                    }
                    CTM.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
                }
                //else
                //{
                //    var AssignList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
                //    if (AssignList != null)
                //    {
                //        List<CRMTicketModel> assignToList = new List<CRMTicketModel>();
                //        foreach (var item in AssignList)
                //        {
                //            CRMTicketModel CRM = new CRMTicketModel();
                //            CRM.UserID = item.Id;
                //            CRM.UserName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                //            assignToList.Add(CRM);
                //        }
                //        CTM.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
                //    }
                //}

                //var GetMapUser = AssignList1.MappedUsers.Split(',');
                //foreach (var item in GetMapUser)
                //{
                //    int id = Convert.ToInt32(item);
                //    string assignquery = @"select ur.Id as UserID,  CONCAT(ur.FName ,' ', '(' , ur.EmployeeCode,')') AS UserName  
                //            from crm_usertbl ur
                //            join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                //            Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewTicket = 1 and ur.Id='" + id + "'";
                //    var data = db.Database.SqlQuery<CRMTICKETASSIGNlist>(assignquery).ToList();

                //    if (data.Count > 0)
                //    {
                //        List<CRMTICKETASSIGNlist> assignlist = new List<CRMTICKETASSIGNlist>();
                //        foreach (var item1 in data)
                //        {
                //            CRMTICKETASSIGNlist CRM = new CRMTICKETASSIGNlist();
                //            CRM.UserID = item1.UserID;
                //            CRM.UserName = item1.UserName;
                //            assignlist.Add(CRM);
                //        }
                //        CTM.selectAssignUserList = assignlist.ToList();
                //        //CTM.AssignUserList = assignlist.ToList();
                //    }                       

                //}
            }
            else
            {

                var AssignList = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true && em.ProfileId != null).OrderBy(em => em.Fname).ToList();
                if (AssignList != null)
                {
                    //string assignquery = @"select *
                    //            from crm_usertbl ur
                    //            left join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                    //            Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and ur.ProfileId IS NOT null";
                    //var data = db.Database.SqlQuery<CreatRoleModel>(assignquery).ToList();
                   
                    List<CRMTicketModel> assignToList = new List<CRMTicketModel>();
                    foreach (var item in AssignList)
                    {
                        Int32 ProfileIds = Convert.ToInt32(item.ProfileId);
                        var GetPermission = db.crm_roleassigntbl.Where(em => em.Id == ProfileIds && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        CreatRoleModel CreateroleModels = Mapper.Map<CreatRoleModel>(GetPermission);
                        ///var isTicket = data.Where(m => m.Id == item.Id && (m.IsTicketForm == true || m.IsTicketsView == true)).Any();
                        if (CreateroleModels.CreateTicket==true || CreateroleModels.ViewTicket == true)
                        {
                            CRMTicketModel CRM = new CRMTicketModel();
                            CRM.UserID = item.Id;
                            CRM.UserName = item.Fname + ' ' + item.Lname + '(' + item.EmployeeCode + ')';
                            assignToList.Add(CRM);
                        }
                     
                    }

                    CTM.AssignUserList = assignToList.Where(em => em.UserID != UserID).ToList();
                }
            }

            List<CRMTicketModel> TicketList = new List<CRMTicketModel>();
            DataTable dtTicket = new DataTable();
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                TempData["SearchTerm"] = SearchTerm;
                dtTicket = DataAccessLayer.GetDataTable("call CRM_GetTicketListBySearchText(" + CompanyID + "," + BranchID + ",'" + SearchTerm + "')");

            }
            else if ((string.IsNullOrEmpty(DateType) || DateType == "cDate") && string.IsNullOrEmpty(SearchTerm))
            {
                dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketList(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
            }
            else if ((string.IsNullOrEmpty(DateType) || DateType == "assigndate") && string.IsNullOrEmpty(SearchTerm))
            {
                dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketListByAssignedDate(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
            }
            else if (DateType == "mDate" && string.IsNullOrEmpty(SearchTerm))
            {
                dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketListByModifiedDate(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
            }
            //else
            //{
            //    dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketList(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
            //}

            if (dtTicket.Rows.Count > 0)
            {
                for (int i = 0; i < dtTicket.Rows.Count; i++)
                {
                    CRMTicketModel oticket = new CRMTicketModel();
                    oticket.TicketID = Convert.ToInt32(dtTicket.Rows[i]["TicketID"]);
                    oticket.TicketNo = Convert.ToString(dtTicket.Rows[i]["TicketNo"]);
                    oticket.CustomerID = dtTicket.Rows[i]["CustomerID"] == DBNull.Value ? Convert.ToInt32(0) : Convert.ToInt32(dtTicket.Rows[i]["CustomerID"]);
                    oticket.CustomerName = dtTicket.Rows[i]["CustomerName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CustomerName"]);
                    oticket.EmailID = Convert.ToString(dtTicket.Rows[i]["EmailID"]);
                    oticket.PhoneNumber = Convert.ToString(dtTicket.Rows[i]["PhoneNumber"]);
                    oticket.ErrorTypeID = dtTicket.Rows[i]["ErrrorID"] == DBNull.Value ? 0 : Convert.ToInt32(dtTicket.Rows[i]["ErrrorID"]);
                    oticket.ErrorTypeName = Convert.ToString(dtTicket.Rows[i]["ErrorName"]);
                    oticket.UrgencyID = dtTicket.Rows[i]["urgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dtTicket.Rows[i]["urgencyID"]);
                    oticket.UrgencyName = Convert.ToString(dtTicket.Rows[i]["urgencyName"]);
                    oticket.TicketStatusID = dtTicket.Rows[i]["StatusId"] == DBNull.Value ? 0 : Convert.ToInt32(dtTicket.Rows[i]["StatusId"]);
                    oticket.ProductTypeID = dtTicket.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dtTicket.Rows[i]["ProductTypeID"]);
                    oticket.ProductTypeName = Convert.ToString(dtTicket.Rows[i]["ProductTypeName"]);
                    oticket.TicketSubject = Convert.ToString(dtTicket.Rows[i]["subject"]);
                    oticket.TicketStatusName = dtTicket.Rows[i]["StatusId"] == DBNull.Value ? string.Empty : Convert.ToInt32(dtTicket.Rows[i]["StatusId"]) == 1 ? "Open" : "Closed";
                    oticket.CreatedBy = dtTicket.Rows[i]["CreatedByName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CreatedByName"]);
                    oticket.AssignedBy = dtTicket.Rows[i]["AssignedBy"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dtTicket.Rows[i]["AssignedBy"]);
                    oticket.AssignedByName = dtTicket.Rows[i]["AssignedByName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["AssignedByName"]);
                    oticket.AssignedTo = dtTicket.Rows[i]["AssignedTo"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dtTicket.Rows[i]["AssignedTo"]);
                    oticket.AssignedToName = dtTicket.Rows[i]["AssignedToName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["AssignedToName"]);
                    oticket.CreatedDate = dtTicket.Rows[i]["CreatedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CreatedDate"]);
                    oticket.ModifiedDate = dtTicket.Rows[i]["ModifiedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["ModifiedDate"]);
                    oticket.AssignDate = dtTicket.Rows[i]["AssignedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["AssignedDate"]);
                    oticket.Sparepartstatus= Convert.ToString(dtTicket.Rows[i]["Sparepartstatus"]);
                    oticket.ExtraCol1 = Convert.ToString(dtTicket.Rows[i]["ExtraCol1"]);
                    oticket.ExtraCol2 = Convert.ToString(dtTicket.Rows[i]["ExtraCol2"]);
                    oticket.ExtraCol3 = Convert.ToString(dtTicket.Rows[i]["ExtraCol3"]);
                    oticket.ExtraCol4 = Convert.ToString(dtTicket.Rows[i]["ExtraCol4"]);
                    oticket.ExtraCol5 = Convert.ToString(dtTicket.Rows[i]["ExtraCol5"]);
                    oticket.ExtraCol6 = Convert.ToString(dtTicket.Rows[i]["ExtraCol6"]);
                    oticket.ExtraCol7 = Convert.ToDecimal(dtTicket.Rows[i]["ExtraCol7"]);
                    oticket.ExtraCol8 = Convert.ToDecimal(dtTicket.Rows[i]["ExtraCol8"]);
                    oticket.ExtraCol9 = dtTicket.Rows[i]["ExtraCol9"] == DBNull.Value ? string.Empty : string.Format("{0:" + CTM.DateFormat + "}", Convert.ToDateTime(dtTicket.Rows[i]["ExtraCol9"]));
                    oticket.ExtraCol10 = dtTicket.Rows[i]["ExtraCol10"] == DBNull.Value ? string.Empty : string.Format("{0:" + CTM.DateFormat + "}", Convert.ToDateTime(dtTicket.Rows[i]["ExtraCol10"]));
                    oticket.ExtraCol11 = Convert.ToInt32(dtTicket.Rows[i]["ExtraCol11"]);
                    oticket.ExtraCol12 = Convert.ToInt32(dtTicket.Rows[i]["ExtraCol12"]);
                    //oticket.ImageCol1 = Convert.ToString(dtTicket.Rows[i]["ImageCol1"]);
                    //oticket.ImageCol2 = Convert.ToString(dtTicket.Rows[i]["ImageCol2"]);
                    //oticket.ImageCol3 = Convert.ToString(dtTicket.Rows[i]["ImageCol3"]);
                    //oticket.ImageCol4 = Convert.ToString(dtTicket.Rows[i]["ImageCol4"]);
                    TicketList.Add(oticket);
                }
            }
            CTM.CRMTicketModelList = TicketList;

            if (!String.IsNullOrWhiteSpace(UrgencyName))
            {
                CTM.CRMTicketModelList = CTM.CRMTicketModelList.Where(em => em.UrgencyName == UrgencyName).ToList();
            }
            if (!String.IsNullOrWhiteSpace(ErrorTypeName))
            {
                CTM.CRMTicketModelList = CTM.CRMTicketModelList.Where(em => em.ErrorTypeName == ErrorTypeName).ToList();
            }
            if (TicketStatusID != null)
            {
                CTM.CRMTicketModelList = CTM.CRMTicketModelList.Where(em => em.TicketStatusID == TicketStatusID).ToList();
            }

            CTM.TotalTicket = CTM.CRMTicketModelList.Count;

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

            TotalProducts = CTM.CRMTicketModelList.Count();
            pages = (TotalProducts / pageSize);
            var Product = CTM.CRMTicketModelList.Skip(pageSkip).Take(pageSize).ToList();
            CTM.CRMTicketModelList = Product;
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
            return View(CTM);
        }

        [HttpPost]
        public ActionResult AssignTicketToUser(CRMTicketModel LMM, int? UserAssignTo)
        {
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    #region calculate the Assigned Leads
                    string Assignedlist = string.Empty;
                    if (Request.Form["MapperTickets"] != null)
                    {
                        Assignedlist = Request.Form["MapperTickets"].ToString();
                    }
                    #endregion
                    Int32 CountAssign = 0;
                    LMM.AssignedTo = Convert.ToInt32(UserAssignTo);
                    if (Assignedlist != string.Empty && LMM.AssignedTo != null)
                    {
                        var SpliteAssignedLeads = Assignedlist.Split(',');
                        var AssignedId = Convert.ToInt32(LMM.AssignedTo);

                        DateTime utcTime = DateTime.UtcNow;
                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);

                        var time = localTime.ToString("hh:mm:ss tt");
                        var Date = localTime.ToString("dd/MM/yyyy");

                        foreach (var item in SpliteAssignedLeads)
                        {
                            Int32 ItemticketID = Convert.ToInt32(item);
                            var ticket = db.crm_tickets.Where(em => em.TicketID == ItemticketID && em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                            if (ticket != null)
                            {
                                ticket.AssignedTo = AssignedId;
                                ticket.BranchID = BranchID;
                                ticket.CompanyId = CompanyID;
                                ticket.AssignedDate = localTime;
                                ticket.AssignedBy = Convert.ToInt32(Session["UID"]);
                            }
                            db.SaveChanges();
                            CountAssign++;
                        }

                        if (CountAssign > 0)
                        {
                            TempData["success"] = "Ticket assigned successfully";
                        }
                    }
                    else
                    {
                        TempData["alert"] = "Please select the tickets and User to Assign";
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
                TempData["alert"] = ex.Message.ToString();
            }
            return Redirect("/Tickets/ViewTicket/?page=1&ErrorTypeName=" + Session["ErrorTypeName"] + "&UrgencyName=" + Session["UrgencyName"] + "&SearchUserID=" + Session["SearchUserID"] + "&SearchFromDate=" + Session["SearchFromDate"] + "&SearchToDate=" + Session["SearchToDate"] + "");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDescriptionTicket()
        {
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
                    Int64? TicketID = Convert.ToInt32(Request.Form[0]);
                    String Description = Convert.ToString(Request.Form[1]).TrimEnd();
                    String TicketStatusName = Convert.ToString(Request.Form[2]).TrimEnd();
                    string EmailId = Convert.ToString(Request.Form[3]).TrimEnd();
                    string TicketNo = Convert.ToString(Request.Form[4]).TrimEnd();
                    string CustomerName = Convert.ToString(Request.Form[5]).TrimEnd();

                    string FileName = string.Empty;
                    string FileFullName = string.Empty;
                    //get Customer name CLM.Customer
                    var CLM = db.crm_tickets.Where(em => em.TicketID == TicketID && em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                    if (Postfile != null)
                    {
                        int fileSize = Postfile.ContentLength;
                        if (fileSize > 0)
                        {
                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                            var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
                            if (supportedTypes.Contains(fileExt))
                            {

                                string extension = Path.GetExtension(Postfile.FileName);
                                FileName = "Ticket-" + Convert.ToString(Session["UserName"]) + "-" + CLM.Name + "-" + Constant.GetBharatTime().ToString("ddMMyyyyhhmmss") + "";
                                FileFullName = FileName + extension;
                                string _path = Server.MapPath("~/TicketAttachment/" + FileName + extension);
                                Postfile.SaveAs(_path);
                            }
                            else
                            {
                                TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                            }
                        }
                    }


                    DateTime localTime = Constant.GetBharatTime();
                    var CurrentDate = localTime.ToString("dd/MM/yyyy");
                    if (!string.IsNullOrEmpty(Description))
                    {
                        crm_tickestmap LD = new crm_tickestmap();
                        LD.TicketId = TicketID;
                        LD.Message = Description;
                        LD.CreatedOn = localTime.Date;
                        LD.CreatedBy = Convert.ToInt32(Session["UID"]);
                        LD.BranchId = Convert.ToInt32(Session["BranchID"]);
                        LD.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                        LD.AttachmentFile = FileFullName;
                        LD.StatusName = TicketStatusName;
                        db.crm_tickestmap.Add(LD);

                        CLM.ModifiedBy = Convert.ToInt32(Session["UID"]);
                        CLM.ModifiedOn = Constant.GetBharatTime();
                        int i = db.SaveChanges();
                        if (!string.IsNullOrEmpty(EmailId))
                        {
                            if (i > 0)
                            {
                                var body = new StringBuilder();
                                body.AppendFormat("Dear {0}, <br />", CustomerName);
                                body.AppendFormat("Your Ticket No: {0},<br />", TicketNo);
                                body.AppendFormat("Issue has been updated description: {0}<br />", Description);
                                body.AppendLine("To more details click <a href=\"https://www.smartcapita.com/view_tickets\">here</a>");
                                var SendNow = EmailUtility.SendTicketEmailToCustomer(EmailId, "Ticket Update", body.ToString(), CompanyID, BranchID);
                            }
                        }

                        Successmsg = "Description add succesfully.";
                    }
                    else
                    {
                        Errormsg = "Please enter description";
                    }

                }
                else
                {
                    Errormsg = "Session has expire. Please login again";
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
        public ActionResult AddTicketTeamRemark()
        {
            string Successmsg = string.Empty;
            string Errormsg = string.Empty;
            try
            {
                if (Session["UID"] != null)
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                    HttpPostedFileBase Postfile = Request.Files.Count == 0 ? null : Request.Files[0];
                    Int64? TicketID = Convert.ToInt32(Request.Form[0]);
                    String Description = Convert.ToString(Request.Form[1]).TrimEnd();
                    String TicketStatusName = Convert.ToString(Request.Form[2]).TrimEnd();
                    string TicketNo = Convert.ToString(Request.Form[3]);
                    int AssignedTo = Convert.ToInt32(Request.Form[4]);

                    string AssignedUserEmail = "";
                    if (AssignedTo > 0)
                    {
                        var email = db.crm_usertbl.Where(a => a.Id == AssignedTo).FirstOrDefault().Email;
                        AssignedUserEmail = !string.IsNullOrEmpty(email) ? email : "";
                    }

                    string FileName = string.Empty;
                    string FileFullName = string.Empty;
                    //if (Postfile != null)
                    //{
                    //    int fileSize = Postfile.ContentLength;
                    //    if (fileSize > 0)
                    //    {
                    //        var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "gif", "bmp" };
                    //        var fileExt = System.IO.Path.GetExtension(Postfile.FileName).Substring(1);
                    //        if (supportedTypes.Contains(fileExt))
                    //        {
                    //            //get Customer name CLM.Customer
                    //            var CLM = db.crm_tickets.Where(em => em.TicketID == TicketID && em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                    //            string extension = Path.GetExtension(Postfile.FileName);
                    //            FileName = "Ticket-" + Convert.ToString(Session["UserName"]) + "-" + CLM.Name + "-" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "";
                    //            FileFullName = FileName + extension;
                    //            string _path = Server.MapPath("~/TicketAttachment/" + FileName + extension);
                    //            Postfile.SaveAs(_path);
                    //        }
                    //        else
                    //        {
                    //            TempData["alert"] = "File upload " + fileExt + " extension not valid. Please Try .txt, .doc, .docx, .pdf, .xls, .xlsx, .jpg, .jpeg, .png,.gif,.bmp only";
                    //        }
                    //    }
                    //}


                    DateTime localTime = Constant.GetBharatTime();
                    var CurrentDate = localTime.ToString("dd/MM/yyyy");

                    if (!string.IsNullOrEmpty(Description))
                    {
                        crm_ticketremarkforteam tr = new crm_ticketremarkforteam();
                        tr.TicketId = TicketID ?? 0;
                        tr.Message = Description;
                        tr.CreatedOn = localTime.Date;
                        tr.CreatedBy = Convert.ToInt32(Session["UID"]);
                        tr.BranchId = Convert.ToInt32(Session["BranchID"]);
                        tr.CompanyID = Convert.ToInt32(Session["CompanyID"]);
                        //tr.AttachmentFile = FileFullName;
                        tr.StatusName = TicketStatusName;
                        db.crm_ticketremarkforteam.Add(tr);
                        int i = db.SaveChanges();
                        if (!string.IsNullOrEmpty(AssignedUserEmail))
                        {
                            if (i > 0)
                            {
                                var body = new StringBuilder();

                                body.AppendFormat("Ticket No:, {0}<br />", TicketNo);
                                body.AppendLine(@", issue has been updated, below is the description.<br />");
                                body.AppendLine(Description);
                                var SendNow = EmailUtility.SendTicketEmailToCustomer(AssignedUserEmail, "Ticket Update", body.ToString(), CompanyID, BranchID);
                            }
                        }
                        Successmsg = "Remark added succesfully.";
                    }
                    else
                    {
                        Errormsg = "Please enter description";
                    }

                }
                else
                {
                    Errormsg = "Session has expire. Please login again";
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

        /// <summary>
        /// View Ticket List description
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public ActionResult ViewTicketDecsription(Int64 tid)
        {

            try
            {
                var DateFormat = Constant.DateFormat();//get date format by company id
                List<TicketMap> oTicketMap = new List<TicketMap>();
                DataTable Gettickectdetaillist = DataAccessLayer.GetDataTable("call CRM_TicketMap(" + tid + ")");
                if (Gettickectdetaillist.Rows.Count > 0)
                {
                    for (int i = 0; i < Gettickectdetaillist.Rows.Count; i++)
                    {
                        TicketMap tm = new TicketMap();
                        tm.Message = Convert.ToString(Gettickectdetaillist.Rows[i]["Message"]);
                        tm.AttachmentFile = Convert.ToString(Gettickectdetaillist.Rows[i]["AttachmentFile"]);
                        tm.UserName = Convert.ToString(Gettickectdetaillist.Rows[i]["UserName"]);
                        tm.CreatedOn = String.Format("{0:" + DateFormat + "}", Convert.ToDateTime(Gettickectdetaillist.Rows[i]["CreatedOn"]));
                        tm.StatusName = Convert.ToString(Gettickectdetaillist.Rows[i]["StatusName"]);
                        oTicketMap.Add(tm);
                    }
                }
                ViewBag.result = oTicketMap.ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialViewTicketDescription");
        }

        #region view Ticket team remark
        public ActionResult ViewTicketTeamRemark(Int64 tid)
        {
            try
            {
                List<TicketMap> oTicketMap = new List<TicketMap>();
                DataTable Gettickectdetaillist = DataAccessLayer.GetDataTable("call CRM_TicketTeamRemarks(" + tid + ")");
                if (Gettickectdetaillist.Rows.Count > 0)
                {
                    for (int i = 0; i < Gettickectdetaillist.Rows.Count; i++)
                    {
                        TicketMap tm = new TicketMap();
                        tm.Message = Convert.ToString(Gettickectdetaillist.Rows[i]["Message"]);
                        tm.AttachmentFile = Convert.ToString(Gettickectdetaillist.Rows[i]["AttachmentFile"]);
                        tm.UserName = Convert.ToString(Gettickectdetaillist.Rows[i]["UserName"]);
                        tm.CreatedOn = Convert.ToString(Gettickectdetaillist.Rows[i]["CreatedOn"]).Replace(" 00:00:00", "");
                        tm.StatusName = Convert.ToString(Gettickectdetaillist.Rows[i]["StatusName"]);
                        oTicketMap.Add(tm);
                    }
                }
                ViewBag.result = oTicketMap.ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_PartialViewTicketDescription");
        }
        #endregion

        public ActionResult ResetTicketFilter()
        {
            Session["ErrorTypeName"] = null;
            Session["UrgencyName"] = null;
            Session["SearchUserID"] = null;
            Session["SearchFromDate"] = null;
            Session["SearchToDate"] = null;
            return Redirect("/Tickets/ViewTicket/?page=1");
        }

        public string GenerateNumber()
        {
            string TicketGenerateNumber = string.Empty;
            var chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            var stringChars1 = new char[12];
            var random1 = new Random();

            for (int i = 0; i < stringChars1.Length; i++)
            {
                stringChars1[i] = chars1[random1.Next(chars1.Length)];
            }
            TicketGenerateNumber = new String(stringChars1);
            return TicketGenerateNumber.ToUpper();
        }
        public JsonResult ChangeStatus(int tid,string txt)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_tickets set Sparepartstatus='"+ txt + "' where TicketID=" + tid);
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
        [HttpPost]
        public ActionResult GetCustomerDetail(string id)
        {
            CRMTicketModel CTM = new CRMTicketModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            //DataTable dtCustomerList = DataAccessLayer.GetDataTable("call CRM_TicketCustomer(" + CompanyID + "," + BranchID + ",'" + id + "')");
            //if (dtCustomerList.Rows.Count > 0)
            //{
            //    CTM.PhoneNumber = Convert.ToString(dtCustomerList.Rows[0]["MobileNo"]);
            //    CTM.EmailID = Convert.ToString(dtCustomerList.Rows[0]["EmailId"]);
            //}
            var CLM = db.crm_createleadstbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.Customer == id).FirstOrDefault();
            if (CLM != null)
            {
                CTM.PhoneNumber = CLM.MobileNo;
                CTM.EmailID = CLM.EmailId;
            }
            return Json(CTM, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult checkroleassignsahil()
        {
            CRMTicketModel CTM = new CRMTicketModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            int userid = Convert.ToInt32(Session["UID"]);
            string profilename = Convert.ToString(Session["UserType"]);
            var CLM = db.crm_roleassigntbl.Where(em => em.CompanyID == CompanyID && em.BranchID == BranchID && em.ProfileName == profilename).FirstOrDefault();
            if (CLM != null)
            {
                CTM.CreateTicket = CLM.CreateTicket;
                CTM.ViewTicket = CLM.ViewTicket;
            }
            return Json(CTM, JsonRequestBehavior.AllowGet);
        }
        public FileResult TicketDownload(string PostFile)
        {
            var filepath = System.IO.Path.Combine(Server.MapPath("~/TicketAttachment/"), PostFile);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), PostFile);
        }

        public ActionResult TicketPopUp()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            CRMTicketModel CTM = new CRMTicketModel();

            CTM.DateFormat = Constant.DateFormat();//get date format by company id
            int? UID = Convert.ToString(Session["UserType"]) == "SuperAdmin" ? 0 : Convert.ToInt32(Session["UID"]);



            #region Default Date show of 15 days back to current
            var dd = Constant.GetBharatTime();//get india datetime
            DateTime monthStartDate = dd.AddDays(-15);
            //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            var MEndDate = dd.ToString("dd/MM/yyyy");


            if (CTM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
            {
                TempData["startDate"] = monthStartDate.ToString(CTM.DateFormat);
                TempData["endDate"] = dd.ToString(CTM.DateFormat);


                MStartDate = String.Format("{0:dd/MM/yyyy}", monthStartDate);//convert to dd/MM/yyyy format for stored procedure
                MEndDate = String.Format("{0:dd/MM/yyyy}", dd);//convert to dd/MM/yyyy format for stored procedure

            }
            else
            {
                TempData["startDate"] = MStartDate;
                TempData["endDate"] = MEndDate;
            }
            #endregion




            //Int32? UserID = Convert.ToInt32(Session["UID"]);


            List<CRMTicketModel> TicketList = new List<CRMTicketModel>();
            DataTable dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketList(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
            if (dtTicket.Rows.Count > 0)
            {
                for (int i = 0; i < dtTicket.Rows.Count; i++)
                {
                    CRMTicketModel oticket = new CRMTicketModel();
                    oticket.TicketID = Convert.ToInt32(dtTicket.Rows[i]["TicketID"]);
                    oticket.TicketNo = Convert.ToString(dtTicket.Rows[i]["TicketNo"]);
                    oticket.CustomerID = dtTicket.Rows[i]["CustomerID"] == DBNull.Value ? Convert.ToInt32(0) : Convert.ToInt32(dtTicket.Rows[i]["CustomerID"]);
                    oticket.CustomerName = dtTicket.Rows[i]["CustomerName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CustomerName"]);
                    oticket.EmailID = Convert.ToString(dtTicket.Rows[i]["EmailID"]);
                    oticket.PhoneNumber = Convert.ToString(dtTicket.Rows[i]["PhoneNumber"]);
                    //oticket.ErrorTypeID = Convert.ToInt32(dtTicket.Rows[i]["ErrrorID"]);
                    oticket.ErrorTypeName = Convert.ToString(dtTicket.Rows[i]["ErrorName"]);
                    //oticket.UrgencyID = Convert.ToInt32(dtTicket.Rows[i]["urgencyID"]);
                    oticket.UrgencyName = Convert.ToString(dtTicket.Rows[i]["urgencyName"]);
                    oticket.TicketStatusName = dtTicket.Rows[i]["StatusId"] == null ? string.Empty : Convert.ToInt32(dtTicket.Rows[i]["StatusId"]) == 1 ? "Open" : "Closed";
                    // oticket.CreatedBy = dtTicket.Rows[i]["CreatedByName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CreatedByName"]);
                    // oticket.AssignedBy = dtTicket.Rows[i]["AssignedBy"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dtTicket.Rows[i]["AssignedBy"]);
                    // oticket.AssignedByName = dtTicket.Rows[i]["AssignedByName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["AssignedByName"]);
                    // oticket.AssignedTo = dtTicket.Rows[i]["AssignedTo"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dtTicket.Rows[i]["AssignedTo"]);
                    // oticket.AssignedToName = dtTicket.Rows[i]["AssignedToName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["AssignedToName"]);
                    oticket.CreatedDate = dtTicket.Rows[i]["CreatedDate"] == DBNull.Value ? String.Empty : String.Format("{0:" + CTM.DateFormat + "}", Convert.ToDateTime(dtTicket.Rows[i]["CreatedDate"]));
                    // oticket.ModifiedDate = dtTicket.Rows[i]["ModifiedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["ModifiedDate"]);
                    TicketList.Add(oticket);
                }
            }

            if (TicketList.Count > 0)
            {
                CTM.CRMTicketModelList = TicketList.Where(a => a.TicketStatusName == "Open").OrderByDescending(x => x.CreatedDate).ThenByDescending(m => m.UrgencyName).ToList();

                return PartialView("_PartialViewTicketPopUp", CTM);
            }
            else
            {
                return Json("No record found", JsonRequestBehavior.AllowGet);
            }
        }


        public static string GetCurrentFinancialYear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (DateTime.Today.Month > 3)
            {
                FinYear = CurYear.Substring(2) + "-" + NexYear.Substring(2);
            }
            else
            {
                FinYear = PreYear.Substring(2) + "-" + CurYear.Substring(2);
            }
            return FinYear.Trim();
        }

        public string GetTicketNo()
        {
            var text = "";
            if (Session["UID"] != null)
            {           

                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                int? MaxId = 0;
                int ID = 0;
                text = GetCurrentFinancialYear();
                DataTable GetRecords = DataAccessLayer.GetDataTable("call P_GetTicketFiscalMaxCount('" + BranchID + "','" + CompanyID + "')");
                if (GetRecords.Rows.Count > 0)
                {
                    MaxId = Convert.ToInt32(GetRecords.Rows[0]["ticketNo"]);
                }
                else
                {
                    MaxId = 0;
                }
                
                if (MaxId != null)
                {
                    ID = Convert.ToInt32(MaxId) + 1;
                }
                else
                {
                    ID = 1;
                }
                if (ID < 10)
                {
                    text += "/0" + ID;
                }
                string month = String.Format("{0:MMM}", DateTime.Now);
                text = GetCurrentFinancialYear() + "/" + month +"/"+ ID;
            }
            return text;
        }
    }
}
