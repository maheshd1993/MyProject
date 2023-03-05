using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_GetTicketByTicketIdController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        [HttpGet]
        public HttpResponseMessage Get(int CompanyID, int BranchID, int TicketId , string Token)
        {
            String ErrorMessage = String.Empty;

            //string Token = string.Empty;
            //var re = Request;
            //var headers = re.Headers;

            //if (headers.Contains("Token"))
            //{
            //    Token = headers.GetValues("Token").First();
            //}
            var auth = Utility.TokenVerify(CompanyID, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            var CTM = new CreateTicketApiDTO();
            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);

               

                if (TicketId > 0)
                {
                    var DateFormat = Constant.DateFormatForApi(companyID);//get date format by company id

                    //var ticketData = (from ticketdetail in db.crm_tickets
                    //                  join pt in db.crm_producttypetbl on ticketdetail.ProductTypeID equals pt.Id into pts
                    //                  from ptbl in pts.DefaultIfEmpty()//product type table left join
                    //                  join et  in db.crm_errortype on ticketdetail.ErrorTypeID equals et.ErrrorID into ets
                    //                  from etbl in ets.DefaultIfEmpty()//error type table left join
                    //                  join urgt in db.crm_urgency on ticketdetail.UrgencyID equals urgt.urgencyID into uts
                    //                  from utbl in uts.DefaultIfEmpty()//urgency type table left join
                    //                  where ticketdetail.TicketID == TicketId
                    //                  select new CreateTicketApiDTO
                    //                  {
                    //                      TicketID = ticketdetail.TicketID,
                    //                      CustomerID = ticketdetail.CustomerID==null ? 0: ticketdetail.CustomerID,
                    //                      ExistingNew = ticketdetail.CustomerID == null ? "New" : "Existing",
                    //                      CustomerName = ticketdetail.CustomerID == null ? "" : ticketdetail.Name,
                    //                      NewCustomerName = ticketdetail.Name,
                    //                      //CustomerNM = ticketdetail.Name,
                    //                      TicketNo = ticketdetail.TicketNo,
                    //                      subject = ticketdetail.subject,
                    //                      PhoneNumber = ticketdetail.PhoneNumber,
                    //                      EmailID = ticketdetail.EmailID,
                    //                      ProductTypeID = ticketdetail.ProductTypeID == null ? 0 : ticketdetail.ProductTypeID,
                    //                      ProductTypeName= ticketdetail.ProductTypeID == null ? "" :ptbl.ProductTypeName,
                    //                      UrgencyID = ticketdetail.UrgencyID == null ? 0 : ticketdetail.UrgencyID,
                    //                      UrgencyName= ticketdetail.UrgencyID == null ? "" :utbl.urgencyName,
                    //                      ErrorTypeID = ticketdetail.ErrorTypeID == null ? 0 : ticketdetail.ErrorTypeID,
                    //                      ErrorTypeName= ticketdetail.ErrorTypeID == null ? "" : etbl.ErrorName,
                    //                      StatusID = ticketdetail.StatusID == null ? 0 : ticketdetail.StatusID,
                    //                      TicketStatusName = ticketdetail.StatusID == null ? "": ticketdetail.StatusID == 1 ? "Open" : "Closed",
                    //                      ExtraCol1 = ticketdetail.ExtraCol1,
                    //                      ExtraCol2 = ticketdetail.ExtraCol2,
                    //                      ExtraCol3 = ticketdetail.ExtraCol3,
                    //                      ExtraCol4 = ticketdetail.ExtraCol4,
                    //                      ExtraCol5 = ticketdetail.ExtraCol5,
                    //                      ExtraCol6 = ticketdetail.ExtraCol6,
                    //                      ExtraCol7 = ticketdetail.ExtraCol7,
                    //                      ExtraCol8 = ticketdetail.ExtraCol8,
                    //                      ExtraCol9 = ticketdetail.ExtraCol9 != null ? String.Format("{0:" + DateFormat + "}", ticketdetail.ExtraCol9) : "",
                    //                      ExtraCol10 = ticketdetail.ExtraCol10 != null ? String.Format("{0:" + DateFormat + "}", ticketdetail.ExtraCol10) : "",                   
                    //                      ExtraCol11 = ticketdetail.ExtraCol11,
                    //                      ExtraCol12 = ticketdetail.ExtraCol12
                    //                  }
                    //              ).FirstOrDefault();

                    var ticketdetail = db.crm_tickets.Where(em => em.CompanyId == CompanyID && em.BranchID == BranchID && em.TicketID == TicketId).FirstOrDefault();
                    if (ticketdetail != null)
                    {
                        CTM.TicketID = ticketdetail.TicketID;
                        if (ticketdetail.CustomerID != null)
                        {
                            CTM.CustomerID = ticketdetail.CustomerID;
                            CTM.ExistingNew = "Existing";
                            CTM.CustomerName = ticketdetail.Name;
                            //CTM.CustomerNM = ticketdetail.Name;
                        }
                        else
                        {
                            CTM.ExistingNew = "New";
                            CTM.NewCustomerName = ticketdetail.Name;
                            //CTM.CustomerNM = ticketdetail.Name;
                        }
                        //CTM.AssignedTo = ticketdetail.AssignedTo;
                        CTM.TicketNo = ticketdetail.TicketNo;
                        CTM.subject = ticketdetail.subject;
                        CTM.PhoneNumber = ticketdetail.PhoneNumber;
                        CTM.EmailID = ticketdetail.EmailID;
                        CTM.ProductTypeID = ticketdetail.ProductTypeID == null ? 0 : ticketdetail.ProductTypeID;
                        var prodType = db.crm_producttypetbl.Where(a => a.Id == ticketdetail.ProductTypeID).FirstOrDefault();

                        CTM.ProductTypeName = ticketdetail.ProductTypeID == null ? "" : prodType==null?"": prodType.ProductTypeName;
                        CTM.UrgencyID = ticketdetail.UrgencyID == null ? 0 : ticketdetail.UrgencyID;

                        var urgency = db.crm_urgency.Where(a => a.urgencyID == ticketdetail.UrgencyID).FirstOrDefault();
                        CTM.UrgencyName = ticketdetail.UrgencyID == null ? "" :urgency==null?"":urgency.urgencyName;
                        CTM.ErrorTypeID = ticketdetail.ErrorTypeID == null ? 0 : ticketdetail.ErrorTypeID;

                        var errorT = db.crm_errortype.Where(a => a.ErrrorID == ticketdetail.ErrorTypeID).FirstOrDefault();
                        CTM.ErrorTypeName = ticketdetail.ErrorTypeID == null ? "" :errorT==null?"":errorT.ErrorName;
                        CTM.StatusID = ticketdetail.StatusID == null ? 0 : ticketdetail.StatusID;
                        CTM.TicketStatusName = CTM.StatusID == 0 ?"": CTM.StatusID == 1? "Open" : "Closed";
                        CTM.ExtraCol1 = ticketdetail.ExtraCol1;
                        CTM.ExtraCol2 = ticketdetail.ExtraCol2;
                        CTM.ExtraCol3 = ticketdetail.ExtraCol3;
                        CTM.ExtraCol4 = ticketdetail.ExtraCol4;
                        CTM.ExtraCol5 = ticketdetail.ExtraCol5;
                        CTM.ExtraCol6 = ticketdetail.ExtraCol6;
                        CTM.ExtraCol7 = ticketdetail.ExtraCol7;
                        CTM.ExtraCol8 = ticketdetail.ExtraCol8;
                        if (CTM.ExtraCol9 != null)
                        {
                            CTM.ExtraCol9 = String.Format("{0:" + DateFormat + "}", ticketdetail.ExtraCol9);
                        }
                        if (CTM.ExtraCol10 != null)
                        {
                            CTM.ExtraCol10 = String.Format("{0:" + DateFormat + "}", ticketdetail.ExtraCol10);
                        }
                        CTM.ExtraCol11 = ticketdetail.ExtraCol11;
                        CTM.ExtraCol12 = ticketdetail.ExtraCol12;

                    }


                }
                else
                {
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            if(CTM!=null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, CTM);
            }
            else
            {
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    }
}
