using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Traders.Mailer;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_AddTicketTeamRemarkController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Add team remark based on ticktid and CompanyID,BranchID
        /// <summary>
        /// Add Description based on ticktid and CompanyID,BranchID
        /// Post:api/CRM_AddTicketTeamRemark/PostJson
        /// </summary>
        /// <param name="TicketID"></param>
        /// <param name="UID"></param>
        /// <param name="Token"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddRemark([FromBody]JToken postData, HttpRequestMessage request) 
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

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



                    if (value.TicketID == null || value.TicketID == 0)
                    {
                        ErrorMessage = "Please enter ticketid";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (string.IsNullOrEmpty(value.Description))
                    {
                        ErrorMessage = "Please enter remark";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    DateTime localTime = Constant.GetimeForApi(companyID);
                    //var CurrentDate = localTime.ToString("dd/MM/yyyy");
                    var CLM = db.crm_tickets.Where(em => em.TicketID == value.TicketID && em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefault();
                    if (CLM != null)
                    {
                    string AssignedUserEmail = string.Empty;
                    if (CLM.AssignedTo > 0)
                    {
                        var userData = db.crm_usertbl.Where(a => a.Id == CLM.AssignedTo).FirstOrDefault();
                        if(userData!=null && !string.IsNullOrEmpty(userData.Email))
                        {
                            AssignedUserEmail = userData.Email;
                        }
                    }
                    crm_ticketremarkforteam tr = new crm_ticketremarkforteam();
                    tr.TicketId = value.TicketID ?? 0;
                    tr.Message = value.Description;
                    tr.CreatedOn = localTime.Date;
                    tr.CreatedBy = Convert.ToInt32(value.UID);
                    tr.BranchId = branchID;
                    tr.CompanyID = companyID;
                    //tr.AttachmentFile = FileFullName;
                    tr.StatusName = value.TicketStatusName;
                    db.crm_ticketremarkforteam.Add(tr);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        SuccessMessage = string.Format("Remark added succesfully.");

                        if (!string.IsNullOrEmpty(AssignedUserEmail))
                        {
                            var body = new StringBuilder();

                            body.AppendFormat("Ticket No:, {0}<br />", CLM.TicketNo);
                            body.AppendLine(@", issue has been updated, below is the description.<br />");
                            body.AppendLine(value.Description);
                            var SendNow = EmailUtility.SendTicketEmailToCustomer(AssignedUserEmail, "Ticket Update", body.ToString(),companyID,branchID);
                        }

                    }

                }//ticket record null if end
                    else
                    {
                        ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters");
                    }

                }
                catch (Exception ex)
                {                  
                    ExceptionLogging.SendExcepToDB(ex);
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters");
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
