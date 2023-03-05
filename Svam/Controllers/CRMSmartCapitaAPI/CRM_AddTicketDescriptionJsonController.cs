using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class CRM_AddTicketDescriptionJsonController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Add Description based on ticktid and CompanyID,BranchID
        /// <summary>
        /// Add Description based on ticktid and CompanyID,BranchID
        /// Post:api/CRM_AddTicketDescriptionJson/PostJson
        /// </summary>
        /// <param name="TicketID"></param>
        /// <param name="UID"></param>
        /// <param name="Token"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> PostJson([FromBody]JToken postData, HttpRequestMessage request) 
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    HttpResponseMessage response = null;
                   
                    AddTicketDescriptionDTO value = JsonConvert.DeserializeObject<AddTicketDescriptionDTO>(postData.ToString());

                    DataTable responseObj = new DataTable();
                    string json = string.Empty;
                    json = JsonConvert.SerializeObject(responseObj);
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    int branchID = Convert.ToInt32(value.BranchID);
                    int companyID = Convert.ToInt32(value.CompanyID);

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


                    
                    if (value.TicketID == null || value.TicketID ==0)
                    {
                        ErrorMessage = "Please enter ticketid";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    
                    if (string.IsNullOrEmpty(value.Description))
                    {
                        ErrorMessage = "Please enter description";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    //var CurrentDate = localTime.ToString("dd/MM/yyyy");
                    var CLM = await db.crm_tickets.Where(em => em.TicketID == value.TicketID && em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefaultAsync();
                            if (CLM != null)
                            {
                        DateTime localTime = Constant.GetimeForApi(companyID);

                        crm_tickestmap LD = new crm_tickestmap();
                                LD.TicketId = value.TicketID;
                                LD.Message = value.Description;
                                LD.CreatedOn = localTime.Date;
                                LD.CreatedBy = Convert.ToInt32(value.UID);
                                LD.BranchId = branchID;
                                LD.CompanyID = companyID;
                                LD.AttachmentFile = null;
                                LD.StatusName = value.TicketStatusName;
                                db.crm_tickestmap.Add(LD);

                                CLM.ModifiedBy = Convert.ToInt32(value.UID);
                                CLM.ModifiedOn = localTime;
                               await db.SaveChangesAsync();

                                trans.Commit();//transaction commit
                                    SuccessMessage = string.Format("Description added succesfully.");

                                if (!string.IsNullOrEmpty(CLM.EmailID))
                                {
                                    var body = new StringBuilder();
                                    body.AppendFormat("Dear {0}, <br />", CLM.Name);
                                    body.AppendFormat("Your Ticket No: {0},<br />", CLM.TicketNo);
                                    body.AppendFormat("Issue has been updated description: {0}<br />", value.Description);
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
