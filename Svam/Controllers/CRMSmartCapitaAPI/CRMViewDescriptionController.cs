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
    public class CRMViewDescriptionController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        public HttpResponseMessage Get(Int32? LeadID, Int32? BranchID, Int32? CompanyID, string Token)
        {
            List<APLViewDescription> aPLViewDescriptionList = new List<APLViewDescription>();
            string ErrorMessage = string.Empty;
            //string Token = string.Empty;

            //var re = Request;
            //var headers = re.Headers;

            //if (headers.Contains("Token"))
            //{
            //    Token = headers.GetValues("Token").First();
            //}
            var auth = Utility.TokenVerify(Convert.ToInt32(CompanyID), Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            try
            {
                var GetDescriptionList = db.crm_leaddescriptiontbl.Where(em => em.LeadId == LeadID && em.CompanyID == CompanyID && em.BranchID == BranchID).OrderByDescending(em => em.CreatedDateTime).ToList();
                if (GetDescriptionList.Count > 0) 
                {
                    foreach (var item in GetDescriptionList)
                    {
                        APLViewDescription APLDesc = new APLViewDescription();
                        APLDesc.UserName = item.ByUserName;
                        if (!string.IsNullOrWhiteSpace(item.Description))
                        {
                            APLDesc.Description = item.Description.Replace("&nbsp;", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\r\n", string.Empty).Replace("\t", string.Empty).Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                        }
                        else 
                        {
                            APLDesc.Description = string.Empty;
                        }
                        APLDesc.CreatedDateTime = item.CreatedDateTime;
                        if (!string.IsNullOrWhiteSpace(item.LeadAttachment))
                        {
                            APLDesc.LeadAttachment = "~/LeadAttachment/" + item.LeadAttachment;
                        }
                        else
                        {
                            APLDesc.LeadAttachment = String.Empty;
                        }
                        aPLViewDescriptionList.Add(APLDesc);
                       
                    }
                }
                else
                {
                    ErrorMessage = string.Format("** No Record Found **");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Format **");
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, aPLViewDescriptionList);
            }
        }

    }
}
