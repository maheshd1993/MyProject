using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMLeadStatusController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of Lead Status
        /// <summary>
        /// Get List of Lead Status
        /// GET api/CRMLeadStatus/?CompanyID=123&BranchID=123
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string CompanyID, string BranchID, string Token)
        {
            CreateLeadsModel CLM = new CreateLeadsModel();
            string message = string.Empty;
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
                var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                string name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";

                var getleadStatus = db.crm_leadstatus_tbl.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.Status==true).ToList();
                if (getleadStatus != null)
                {
                    List<LeadStatusModel> LSMList = new List<LeadStatusModel>();
                    LSMList.Add(new LeadStatusModel { Id=0,LeadStatusname= String.Format("Select {0}", name) });
                    foreach (var item in getleadStatus)
                    {
                        LeadStatusModel LSM = new LeadStatusModel();
                        LSM.Id = item.Id;
                        LSM.LeadStatusname = item.LeadStatusName;
                        LSMList.Add(LSM);
                    }
                    CLM.leadstatusList = LSMList;
                }
                else
                {
                    message = string.Format("** Lead Status is Blank  **");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (message != string.Empty)
            {
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, CLM.leadstatusList);
            }
        }
        #endregion
    }
}
