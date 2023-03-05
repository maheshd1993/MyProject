using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class TestApiController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        public async Task<HttpResponseMessage> PostTicketData()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            var json = await filesReadToProvider.Contents[0].ReadAsStringAsync();
            var fileBytes = await filesReadToProvider.Contents[0].ReadAsByteArrayAsync();

            CreateTicketApiDTO CTM = JsonConvert.DeserializeObject<CreateTicketApiDTO>(json);
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
                                string FileName = string.Empty;
                                string FileFullName = string.Empty;

                                Dictionary<string, object> dict = new Dictionary<string, object>();
                                var httpRequest = HttpContext.Current.Request;
                                if (httpRequest.Files.Count > 0)
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
                                    foreach (string file in httpRequest.Files)
                                    {
                                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                                        var postedFile = httpRequest.Files[file];
                                        if (postedFile != null && postedFile.ContentLength > 0)
                                        {
                                            int MaxContentLength = 1024 * 1024 * 5; //Size = 1 MB  

                                            IList<string> AllowedFileExtensions = new List<string> { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                                            //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                            var ext = Path.GetExtension(postedFile.FileName);
                                            var extension = ext.ToLower();
                                            if (!AllowedFileExtensions.Contains(extension))
                                            {
                                                var message = string.Format("** Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
                                                dict.Add("error", message);
                                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                            }
                                            else if (postedFile.ContentLength > MaxContentLength)
                                            {
                                                var message = string.Format("Please Upload a file upto 5 mb.");
                                                dict.Add("error", message);
                                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                            }
                                            else
                                            {
                                                //var CLM = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                                                FileName = "Ticket-" + CTM.UserName + "-" + customerName.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                                FileFullName = FileName + extension;
                                                var filePath = HttpContext.Current.Server.MapPath("~/TicketAttachment/" + FileName + extension);
                                                postedFile.SaveAs(filePath);
                                            }
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
                                    LD.CreatedBy = Convert.ToInt32(UID);
                                    LD.BranchId = branchID;
                                    LD.CompanyID = companyID;
                                    LD.AttachmentFile = FileFullName;
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
                                    tr.AttachmentFile = FileFullName;
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
                            string FileName = string.Empty;
                            string FileFullName = string.Empty;

                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            var httpRequest = HttpContext.Current.Request;
                            if (httpRequest.Files.Count > 0)
                            {

                                foreach (string file in httpRequest.Files)
                                {
                                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                                    var postedFile = httpRequest.Files[file];
                                    if (postedFile != null && postedFile.ContentLength > 0)
                                    {
                                        int MaxContentLength = 1024 * 1024 * 5; //Size = 1 MB  

                                        IList<string> AllowedFileExtensions = new List<string> { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                                        //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                        var ext = Path.GetExtension(postedFile.FileName);
                                        var extension = ext.ToLower();
                                        if (!AllowedFileExtensions.Contains(extension))
                                        {
                                            var message = string.Format("** Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
                                            dict.Add("error", message);
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                        }
                                        else if (postedFile.ContentLength > MaxContentLength)
                                        {
                                            var message = string.Format("Please Upload a file upto 5 mb.");
                                            dict.Add("error", message);
                                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                        }
                                        else
                                        {
                                            FileName = "Ticket-" + CTM.UserName + "-" + cts.Name.Trim() + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                            FileFullName = FileName + extension;
                                            var filePath = HttpContext.Current.Server.MapPath("~/TicketAttachment/" + FileName + extension);
                                            postedFile.SaveAs(filePath);
                                        }
                                    }
                                }
                            }
                            var tid = cts.TicketID;
                            crm_tickestmap LD = new crm_tickestmap();
                            LD.TicketId = tid;
                            LD.Message = CTM.TicketDescription;
                            LD.CreatedOn = localTime.Date;
                            LD.CreatedBy = Convert.ToInt32(UID);
                            LD.BranchId = Convert.ToInt32(branchID);
                            LD.CompanyID = Convert.ToInt32(companyID);
                            LD.AttachmentFile = FileFullName;
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
                                tr.AttachmentFile = FileFullName;
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
        #endregion
    }
}
