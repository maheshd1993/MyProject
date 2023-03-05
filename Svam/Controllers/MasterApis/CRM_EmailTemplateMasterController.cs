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
using System.Text.RegularExpressions;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.MasterApis
{
    public class CRM_EmailTemplateMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_EmailTemplateMaster/GetEmailTemplateList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetEmailTemplateList(string CompanyID, string BranchID, string Token) 
        {
            string ErrorMessage = string.Empty;


            List<EmailTemplateApiModel> etmList = new List<EmailTemplateApiModel>();

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
                DataTable dtETM = DataAccessLayer.GetDataTable("Call CRM_GetEmailTemplate(" + CompanyID + "," + BranchID + ")");
                if (dtETM.Rows.Count > 0)
                {                   
                    for (int i = 0; i < dtETM.Rows.Count; i++)
                    {
                        EmailTemplateApiModel oETMModel = new EmailTemplateApiModel();
                        oETMModel.EmailTemplateID = Convert.ToInt32(dtETM.Rows[i]["EmailTemplateID"]);
                        oETMModel.EmailTemplateName = Convert.ToString(dtETM.Rows[i]["EmailTemplateName"]);
                        oETMModel.CreatedOn = Convert.ToString(dtETM.Rows[i]["CreatedOn"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        etmList.Add(oETMModel);
                    }
                    
                }
                else
                {
                    ErrorMessage = string.Format("No record found");
                }

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
                return Request.CreateResponse(HttpStatusCode.OK, etmList);
            }
        }


        //api/CRM_EmailTemplateMaster/AddUpdateEmailTemplate
        [HttpPost]
        public HttpResponseMessage AddUpdateEmailTemplate([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                EmailTemplateApiModel CIM = JsonConvert.DeserializeObject<EmailTemplateApiModel>(postData.ToString());

                int branchID = Convert.ToInt32(CIM.BranchID);
                int companyID = Convert.ToInt32(CIM.CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, CIM.Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                #region check nullable validation

                if (string.IsNullOrEmpty(CIM.EmailTemplateName))
                {
                    ErrorMessage = "Please enter email template name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                if (CIM.EmailTemplateID > 0)
                {
                    //var getTemplete = db.crm_emailtemplate.Where(em => em.EmailTemplateID == CIM.EmailTemplateID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    //if (getTemplete != null)
                    //{
                    //    getTemplete.EmailTemplateName = CIM.EmailTemplateName;
                    //    getTemplete.EmailTemplateContent = CIM.EmailTempleteBody;
                    //    getTemplete.ModifiedBy = Convert.ToInt32(CIM.UID);
                    //    getTemplete.ModifiedOn = Constant.GetimeForApi(companyID);
                        
                    //    db.SaveChanges();
                    //    SuccessMessage = "Email template updated successfully";
                    //}
                    //else
                    //{
                    //    ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    //}
                }
                else
                {
                    //crm_emailtemplate etemplete = new crm_emailtemplate();
                    //etemplete.EmailTemplateName = CIM.EmailTemplateName;
                    //etemplete.EmailTemplateContent = CIM.EmailTempleteBody;
                    //etemplete.CreatedBy = Convert.ToInt32(CIM.UID);
                    //etemplete.CreatedOn = Constant.GetimeForApi(companyID);
                    //etemplete.BranchID = branchID;
                    //etemplete.CompanyID = companyID;
                    //db.crm_emailtemplate.Add(etemplete);                    
                    //if (db.SaveChanges() > 0)
                    //{
                    //    SuccessMessage = "Email template added successfully";
                    //}
                    //else
                    //{
                    //    ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    //}
                }

            }
            catch (Exception ex)
            {

                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
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

        //api/CRM_EmailTemplateMaster/GetEmailTemplateById?EmailTemplateID=984
        [HttpGet]
        public HttpResponseMessage GetEmailTemplateById(int EmailTemplateID)
        {
            string ErrorMessage = string.Empty;


            var ETM = new EmailTemplateApiModel();
            try
            {
                //var getTemplatedetail = db.crm_emailtemplate.Where(em => em.EmailTemplateID == EmailTemplateID).FirstOrDefault(); 
                //if (getTemplatedetail != null)
                //{
                //    ETM.EmailTemplateID = getTemplatedetail.EmailTemplateID;
                //    ETM.EmailTemplateName = getTemplatedetail.EmailTemplateName;
                //    ETM.EmailTempleteBody = getTemplatedetail.EmailTemplateContent;
                //}
                //else
                //{
                //    ErrorMessage = string.Format("No record found");
                //}

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
                return Request.CreateResponse(HttpStatusCode.OK, ETM);
            }
        }
    }
}
