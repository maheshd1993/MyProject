using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_Create_Ticket_JsonController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        public HttpResponseMessage Post([FromBody]JToken postData, HttpRequestMessage request)
        {

            HttpResponseMessage response = null;
            CreateTicketApiDTO CTM = JsonConvert.DeserializeObject<CreateTicketApiDTO>(postData.ToString());
            DataTable responseObj = new DataTable();
            string json = string.Empty;
            json = JsonConvert.SerializeObject(responseObj);
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {


                    Int32 branchID = Convert.ToInt32(CTM.BranchID);
                    Int32 companyID = Convert.ToInt32(CTM.CompanyID);

                    //string Token = string.Empty;

                    //var re = Request;
                    //var headers = re.Headers;

                    //if (headers.Contains("Token"))
                    //{
                    //    Token = headers.GetValues("Token").First();
                    //}
                    var auth = Utility.TokenVerify(companyID, CTM.Token);//verify token for is authorized user

                    if (auth == false)
                    {
                        ErrorMessage = string.Format("** User authentication failed!");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    string MobileNumber = CTM.PhoneNumber.Replace("+91", "");


                    long? TicketID = CTM.TicketID;
                    int? UID = CTM.UserID;

                    DateTime localTime = Constant.GetimeForApi(companyID);

                    if (CTM.TicketID > 0)
                    {
                        var CheckTicket = db.crm_tickets.Where(em => em.TicketID == CTM.TicketID && em.BranchID == branchID && em.CompanyId == branchID).FirstOrDefault();
                        if (CheckTicket != null)
                        {
                            var CheckMapTicket = db.crm_tickestmap.Where(em => em.TicketId == CTM.TicketID && em.CreatedOn.Value.Year == localTime.Year && em.CreatedOn.Value.Month == localTime.Month && em.CreatedOn.Value.Day == localTime.Day
                            && em.BranchId == branchID && em.CompanyID == companyID).FirstOrDefault();
                            if (CheckMapTicket != null || !string.IsNullOrEmpty(CTM.TicketDescription))
                            {
                                CheckTicket.EmailID = CTM.EmailID;
                                CheckTicket.PhoneNumber = MobileNumber;
                                CheckTicket.ProductTypeID = CTM.ProductTypeID;
                                CheckTicket.UrgencyID = CTM.UrgencyID;
                                CheckTicket.ErrorTypeID = CTM.ErrorTypeID;
                                CheckTicket.StatusID = CTM.StatusID;
                                CheckTicket.ModifiedBy = Convert.ToInt32(UID);
                                CheckTicket.ModifiedOn = localTime;
                                //CheckTicket.CompanyId = CompanyID;
                                //CheckTicket.BranchID = BranchID;

                                CheckTicket.ExtraCol1 = CTM.ExtraCol1;
                                CheckTicket.ExtraCol2 = CTM.ExtraCol2;
                                CheckTicket.ExtraCol3 = CTM.ExtraCol3;
                                CheckTicket.ExtraCol4 = CTM.ExtraCol4;
                                CheckTicket.ExtraCol5 = CTM.ExtraCol5;
                                CheckTicket.ExtraCol6 = CTM.ExtraCol6;
                                CheckTicket.ExtraCol7 = CTM.ExtraCol7??0;
                                CheckTicket.ExtraCol8 = CTM.ExtraCol8??0;
                                if (!string.IsNullOrEmpty(CTM.ExtraCol9))
                                {
                                    CheckTicket.ExtraCol9 = Convert.ToDateTime(CTM.ExtraCol9);
                                }
                                if (!string.IsNullOrEmpty(CTM.ExtraCol10))
                                {
                                    CheckTicket.ExtraCol10 = Convert.ToDateTime(CTM.ExtraCol10);
                                }
                                CheckTicket.ExtraCol11 = CTM.ExtraCol11??0;
                                CheckTicket.ExtraCol12 = CTM.ExtraCol12??0;

                                #region Add-Ticket-Description                               
                                var tid = CTM.TicketID;
                                if (!string.IsNullOrEmpty(CTM.TicketDescription))
                                {
                                    crm_tickestmap LD = new crm_tickestmap();
                                    LD.TicketId = tid;
                                    LD.Message = CTM.TicketDescription;
                                    LD.CreatedOn = localTime.Date;
                                    LD.CreatedBy = Convert.ToInt32(UID);
                                    LD.BranchId = branchID;
                                    LD.CompanyID = companyID;
                                    LD.AttachmentFile = null;
                                    LD.StatusName = CTM.StatusID == 1 ? "Open" : "Closed";
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
                                    tr.CreatedBy = Convert.ToInt32(UID);
                                    tr.BranchId = branchID;
                                    tr.CompanyID = companyID;
                                    tr.AttachmentFile = null;
                                    tr.StatusName = CTM.StatusID == 1 ? "Open" : "Closed";
                                    db.crm_ticketremarkforteam.Add(tr);
                                    db.SaveChanges();
                                }

                                #endregion
                                db.SaveChanges();
                                trans.Commit(); //after data saved then commit transaction
                                SuccessMessage = "Ticket updated successfully";
                            }
                            else
                            {
                                ErrorMessage = "Please add a description before Update";
                            }
                        }
                    }
                    else
                    {
                        crm_tickets cts = new crm_tickets();
                        cts.TicketNo = GenerateNumber();
                        if (CTM.CustomerID > 0)
                        {
                            cts.CustomerID = CTM.CustomerID;
                            cts.Name = CTM.CustomerName;
                        }
                        else
                        {
                            cts.Name = CTM.NewCustomerName;
                        }
                        cts.EmailID = CTM.EmailID;
                        cts.subject = string.IsNullOrEmpty(CTM.subject) ? "New Ticket" : CTM.subject;
                        cts.PhoneNumber = MobileNumber;
                        cts.ProductTypeID = CTM.ProductTypeID;
                        cts.UrgencyID = CTM.UrgencyID;
                        cts.ErrorTypeID = CTM.ErrorTypeID;
                        cts.StatusID = CTM.StatusID;
                        cts.CreatedBy = Convert.ToInt32(UID);
                        cts.CreatedOn = localTime;
                        cts.CompanyId = companyID;
                        cts.BranchID = branchID;
                        cts.ExtraCol1 = CTM.ExtraCol1;
                        cts.ExtraCol2 = CTM.ExtraCol2;
                        cts.ExtraCol3 = CTM.ExtraCol3;
                        cts.ExtraCol4 = CTM.ExtraCol4;
                        cts.ExtraCol5 = CTM.ExtraCol5;
                        cts.ExtraCol6 = CTM.ExtraCol6;
                        cts.ExtraCol7 = CTM.ExtraCol7??0;
                        cts.ExtraCol8 = CTM.ExtraCol8??0;
                        if (!string.IsNullOrEmpty(CTM.ExtraCol9))
                        {
                            cts.ExtraCol9 = Convert.ToDateTime(CTM.ExtraCol9);
                        }
                        if (!string.IsNullOrEmpty(CTM.ExtraCol10))
                        {
                            cts.ExtraCol10 = Convert.ToDateTime(CTM.ExtraCol10);
                        }
                        cts.ExtraCol11 = CTM.ExtraCol11??0;
                        cts.ExtraCol12 = CTM.ExtraCol12??0;


                        db.crm_tickets.Add(cts);
                        if (db.SaveChanges() > 0)
                        {
                            #region Add-Ticket-Description

                            var tid = cts.TicketID;
                            crm_tickestmap LD = new crm_tickestmap();
                            LD.TicketId = tid;
                            LD.Message = CTM.TicketDescription;
                            LD.CreatedOn = localTime.Date;
                            LD.CreatedBy = Convert.ToInt32(UID);
                            LD.BranchId = Convert.ToInt32(branchID);
                            LD.CompanyID = Convert.ToInt32(companyID);
                            LD.AttachmentFile = null;
                            LD.StatusName = CTM.StatusID == 1 ? "Open" : "Closed";
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
                                tr.CreatedBy = Convert.ToInt32(UID);
                                tr.BranchId = branchID;
                                tr.CompanyID = companyID;
                                tr.AttachmentFile = null;
                                tr.StatusName = CTM.StatusID == 1 ? "Open" : "Closed";
                                db.crm_ticketremarkforteam.Add(tr);
                                db.SaveChanges();
                            }

                            #endregion
                            trans.Commit(); //after data saved then commit transaction
                            SuccessMessage = "Ticket created successfully";

                        }
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the Post Data(Format) Parameters **");
                    ExceptionLogging.SendExcepToDB(ex);
                }
            }


            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }


        }

        #region generate ticket no auto
        private string GenerateNumber()
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
        #endregion
    }
}
