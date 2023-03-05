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
    public class CRMLeadSourceController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of Lead Status
        /// <summary>
        /// Get List of Lead Source
        /// GET api/CRMLeadSource/?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
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
                string name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";


                var getleadSource = db.crm_leadsource_tbl.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.Status==true).ToList();
                if (getleadSource != null)
                {
                    List<LeadSourceModel> LMList = new List<LeadSourceModel>();
                    LMList.Add(new LeadSourceModel { Id=0,LeadsourceName= String.Format("Select {0}", name) });
                    foreach (var item in getleadSource)
                    {
                        LeadSourceModel LSM = new LeadSourceModel();
                        LSM.Id = item.Id;
                        LSM.LeadsourceName = item.LeadsourceName;
                        LMList.Add(LSM);
                    }
                    CLM.leadsourceList = LMList;
                }
                else 
                {
                    message = string.Format("** Lead Source is Blank **");
                }
            }
            catch (Exception ex)
            {
                message = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (message != string.Empty)
            {
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, CLM.leadsourceList);
            }
        }
        #endregion
    }
}
