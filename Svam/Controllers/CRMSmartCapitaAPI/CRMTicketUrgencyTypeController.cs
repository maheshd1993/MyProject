using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMTicketUrgencyTypeController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of Urgency Type
        /// <summary>
        /// Get List of Product Type
        /// GET api/CRMTicketUrgencyType?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string CompanyID, string BranchID, string Token)
        {
            //CreateLeadsModel CLM = new CreateLeadsModel();
            string message = string.Empty;
            List<UrgencyTypeModel> oUrgencyList = new List<UrgencyTypeModel>();

            try
            {
                Int32 branchID = Convert.ToInt32(BranchID);
                Int32 companyID = Convert.ToInt32(CompanyID);
                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                if (auth == false)
                {
                    message = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefault();
                string name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";
                var urgencyList = db.crm_urgency.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.IsActive == true).ToList();
                if (urgencyList != null && urgencyList.Count > 0)
                {

                    oUrgencyList.Add(new UrgencyTypeModel { Id = 0, UrgencyName = String.Format("Select {0}", name) });
                    foreach (var item in urgencyList)
                    {
                        UrgencyTypeModel oError = new UrgencyTypeModel();
                        oError.Id = item.urgencyID;
                        oError.UrgencyName = item.urgencyName;
                        oUrgencyList.Add(oError);
                    }

                }
                else
                {
                    message = string.Format("** Urgency type list is blank  **");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                message = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
            }

            if (message != string.Empty)
            {
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, oUrgencyList);
            }
        }
        #endregion
    }
}
