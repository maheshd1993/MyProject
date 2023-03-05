using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Text;
using System.Web.Http;
namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_AssignTicketsToUserController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
     
        //api/CRM_AssignTicketsToUser?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        
        [HttpPost]
        public HttpResponseMessage AssignTickets([FromBody]JToken postData, HttpRequestMessage request) 
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {
                HttpResponseMessage response = null;

                AssignTicketApiDTO value = JsonConvert.DeserializeObject<AssignTicketApiDTO>(postData.ToString());

                DataTable responseObj = new DataTable();
                string json = string.Empty;
                json = JsonConvert.SerializeObject(responseObj);
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                int branchID = value.BranchID;
                int companyID = value.CompanyID;

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



                if ( value.AssignToUserId == 0)
                {
                    ErrorMessage = "Please select assign to user";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                if (string.IsNullOrEmpty(value.AssignTicketIds))
                {
                    ErrorMessage = "Please select assign ticket";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                DateTime localTime = Constant.GetimeForApi(companyID);

                var SpliteAssignedLeads = value.AssignTicketIds.Split(',');
                var AssignedId = value.AssignToUserId;

               

                //var time = localTime.ToString("hh:mm:ss tt");
                //var Date = localTime.ToString("dd/MM/yyyy");
                int CountAssign = 0;
                foreach (var item in SpliteAssignedLeads)
                {
                    int ItemticketID = Convert.ToInt32(item);
                    var ticket = db.crm_tickets.Where(em => em.TicketID == ItemticketID && em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefault();
                    if (ticket != null)
                    {
                        ticket.AssignedTo = AssignedId;
                        ticket.BranchID = branchID;
                        ticket.CompanyId = companyID;
                        ticket.AssignedDate = localTime;
                        ticket.AssignedBy = value.UID;
                    }
                    db.SaveChanges();
                    CountAssign++;
                }

                if (CountAssign > 0)
                {
                    SuccessMessage = "Ticket assigned successfully";
                }
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
       
    }
}
