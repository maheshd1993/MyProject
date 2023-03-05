using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;
using Svam.EF;
using Svam.Models.ApiModel;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_ProdTypeLeadSourceVisibilityController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_ProdTypeLeadSourceVisibility?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage Get(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;
           
            var model = new List<ProdTpLeadSrcTermVM>();
            var GetData = new ProdTpLeadSrcTermVM();
            try
            {
                Int32 branchID = Convert.ToInt32(BranchID);
                Int32 companyID = Convert.ToInt32(CompanyID);

                //System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //     Token = headers.GetValues("Token").First();
                //}

                var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                //check dynamic fields data according to company and branch id
                var data = db.crm_viewleadsetting.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                //var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();

                
                if (data != null && (data.IsCustomerNameFilter || data.IsMobNoFilter || data.IsEmailFilter || data.IsDesigFilter || data.IsOrgNameFilter || data.IsSkypIdFilter))
                {
                    GetData.IsTerm = true;
                }
                else
                {
                    GetData.IsTerm = false;
                }
               
                GetData.IsProductTypeName = data != null && data.IsProductTypeFilter? true:false;

                GetData.IsLeadSourceName = data != null && data.IsLeadSourceFilter ? true : false;

                model.Add(GetData);

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");

            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
        }
    }
}
