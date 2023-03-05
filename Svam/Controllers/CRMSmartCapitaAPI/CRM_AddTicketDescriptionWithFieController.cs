using Newtonsoft.Json;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Traders.Mailer;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_AddTicketDescriptionWithFieController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Add Description with file based on TicketId and CompanyID,BranchID
        /// <summary>

        /// Post:api/CRM_AddTicketDescriptionWithFie/AddWithFile
        /// </summary>
        /// <param name="LeadID"></param>
        /// <param name="UID"></param>
        /// <param name="UserName"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="txtDescription"></param>
        /// <param name="Postfile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> AddWithFile(HttpRequestMessage httpmessage)  
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            //var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            //var json = await filesReadToProvider.Contents[0].ReadAsStringAsync();
            //var fileBytes = await filesReadToProvider.Contents[0].ReadAsByteArrayAsync();

            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            //AddTicketDescriptionDTO value = JsonConvert.DeserializeObject<AddTicketDescriptionDTO>(json);

            #region this code for upload data in key value pair form-data with file
            NameValueCollection collection = HttpContext.Current.Request.Form;

            var items = collection.AllKeys.SelectMany(collection.GetValues, (k, v) => new { key = k, value = v });

            //We just collect your multiple form data key/value pair in this dictinary
            //The following code will be replaced by yours
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            var sb = new StringBuilder();
            sb.Append("{");//append open curlly braces in string builder for make json data
            //string removeLastcomma = string.Empty;
            foreach (var item in items)
            {
                keyValuePairs.Add(item.key, item.value);
                sb.AppendFormat("\"{0}\" : \"{1}\",", item.key, item.value);
            }
            sb.Append("}");//append close curlly braces in string builder for make json data
            //int index = sb.ToString().LastIndexOf(',');
            //if(index>0)
            //{
            //    removeLastcomma= sb.ToString().Remove(index, 1);
            //}
            var jsonData = sb.ToString();
            AddTicketDescriptionDTO value = JsonConvert.DeserializeObject<AddTicketDescriptionDTO>(jsonData);
            #endregion

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Int32 branchID = Convert.ToInt32(value.BranchID);
                    Int32 companyID = Convert.ToInt32(value.CompanyID);
                    int TicketId = Convert.ToInt32(value.TicketID);
                    string Description = value.Description;
                    int UID = Convert.ToInt32(value.UID);
                    string TicketStatusName = value.TicketStatusName;
                    string UserName = string.Empty;
                    //string Token = string.Empty;

                    //var re = Request;
                    //var headers = re.Headers;

                    //if (headers.Contains("Token"))
                    //{
                    //    Token = headers.GetValues("Token").First();
                    //}
                    var auth = Utility.TokenVerify(companyID, value.Token);//verify token for is authorized user

                    if (auth == false)
                    {
                        ErrorMessage = string.Format("** User authentication failed!");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    if (TicketId == 0)
                    {
                        ErrorMessage = "Please enter ticketid";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (string.IsNullOrEmpty(Description))
                    {
                        ErrorMessage = "Please enter description";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    DateTime localTime = Constant.GetimeForApi(companyID);
                    //var CurrentDate = localTime.ToString("dd/MM/yyyy");
                    var CLM = await db.crm_tickets.Where(em => em.TicketID == TicketId && em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefaultAsync();

                    if (CLM != null)
                    {
                        var userData = await db.crm_usertbl.Where(a => a.Id == UID).FirstOrDefaultAsync();

                        if(userData!=null)
                        {
                            UserName = string.Format("{0} {1}", userData.Fname, userData.Lname);
                        }
                       

                        string FileFullName = string.Empty;
                        string FileName = string.Empty;

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
                                        var message = string.Format("Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
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

                                        FileName = "Ticket-" + UserName + "-" + CLM.Name + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                        FileFullName = FileName + extension;

                                        var filePath = HttpContext.Current.Server.MapPath("~/TicketAttachment/" + FileFullName);
                                        postedFile.SaveAs(filePath);
                                    }
                                }
                            }
                        }

                        crm_tickestmap LD = new crm_tickestmap();
                        LD.TicketId = TicketId;
                        LD.Message = Description;
                        LD.CreatedOn = localTime.Date;
                        LD.CreatedBy = Convert.ToInt32(UID);
                        LD.BranchId = branchID;
                        LD.CompanyID = companyID;
                        LD.AttachmentFile = FileFullName;
                        LD.StatusName = TicketStatusName;
                        db.crm_tickestmap.Add(LD);

                        CLM.ModifiedBy = Convert.ToInt32(UID);
                        CLM.ModifiedOn = localTime;
                        await db.SaveChangesAsync();

                        trans.Commit();//transaction commit
                        SuccessMessage = string.Format("Description added succesfully.");


                        if (!string.IsNullOrEmpty(CLM.EmailID))
                        {
                            var body = new StringBuilder();
                            body.AppendFormat("Dear {0}, <br />", CLM.Name);
                            body.AppendFormat("Your Ticket No: {0},<br />", CLM.TicketNo);
                            body.AppendFormat("Issue has been updated description: {0}<br />", Description);
                            body.AppendLine("To more details click <a href=\"https://www.smartcapita.com/view_tickets\">here</a>");
                            EmailUtility.SendTicketEmailToCustomer(CLM.EmailID, "Ticket Update", body.ToString(),companyID,branchID);
                        }

                        

                    }//ticket record null if end
                    else
                    {
                        ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters");
                    }                                     
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters");
                }
            }


            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }

        #endregion
    }
}
