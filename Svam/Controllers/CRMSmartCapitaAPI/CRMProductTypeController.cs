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
    public class CRMProductTypeController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of Product Type
        /// <summary>
        /// Get List of Product Type
        /// GET api/CRMProductType/?CompanyID=123&BranchID=123
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
                string name= GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                var getProductType = db.crm_producttypetbl.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.Status==true).ToList();
                if (getProductType != null)
                {
                    List<ProductTypeModel> PTMList = new List<ProductTypeModel>();
                    PTMList.Add(new ProductTypeModel { Id=0,ProductTypeName=String.Format("Select {0}", name) });
                    foreach (var item in getProductType)
                    {
                        ProductTypeModel PTM = new ProductTypeModel();
                        PTM.Id = item.Id;
                        PTM.ProductTypeName = item.ProductTypeName;
                        PTMList.Add(PTM);
                    }
                    CLM.producttypetblList = PTMList;
                }
                else
                {
                    message = string.Format("** Product type list is blank  **");
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
                return Request.CreateResponse(HttpStatusCode.OK, CLM.producttypetblList);
            }
        }
        #endregion
    }
}
