using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Traders.Mailer;

namespace Svam.Controllers.MasterApis 
{
    public class CRM_AddEmailSetupController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Add email setting
             
        // Post:api/CRM_AddEmailSetup/AddEmailSetting
        
        [HttpPost]
        public HttpResponseMessage  AddEmailSetting([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

          try
                {

                    //HttpResponseMessage response = null;

                    EmailSetupDTO oCRMEmailSettingModel = JsonConvert.DeserializeObject<EmailSetupDTO>(postData.ToString());

                    //DataTable responseObj = new DataTable();
                    //string json = string.Empty;
                    //json = JsonConvert.SerializeObject(responseObj);
                    //response = Request.CreateResponse(HttpStatusCode.OK);
                    //response.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    int branchID = Convert.ToInt32(oCRMEmailSettingModel.BranchID);
                    int companyID = Convert.ToInt32(oCRMEmailSettingModel.CompanyID);

                    //string Token = string.Empty;

                    //var re = Request;
                    //var headers = re.Headers;

                    //if (headers.Contains("Token"))
                    //{
                    //    Token = headers.GetValues("Token").First();
                    //}
                    var auth = Utility.TokenVerify(companyID, oCRMEmailSettingModel.Token);//verify token for is authorized user

                    if (auth == false)
                    {
                        ErrorMessage = string.Format("** User authentication failed!");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    #region check nullable validation

                    if (string.IsNullOrEmpty(oCRMEmailSettingModel.EmailAddress))
                    {
                        ErrorMessage = "Please enter email address";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (string.IsNullOrEmpty(oCRMEmailSettingModel.SMTPHost))
                    {
                        ErrorMessage = "Please enter smtp host";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (oCRMEmailSettingModel.Port==null)
                    {
                        ErrorMessage = "Please enter port no.";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (string.IsNullOrEmpty(oCRMEmailSettingModel.UserName))
                    {
                        ErrorMessage = "Please enter username";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (string.IsNullOrEmpty(oCRMEmailSettingModel.Password))
                    {
                        ErrorMessage = "Please enter password";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    if (string.IsNullOrEmpty(oCRMEmailSettingModel.DisplayName))
                    {
                        ErrorMessage = "Please enter display name";
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    #endregion
                    
                    DateTime localTime = Constant.GetimeForApi(companyID);


                    if (oCRMEmailSettingModel.EmailSettingID > 0)
                    {
                        var updateEmailSetting = db.crm_emailsetting.Where(em => em.EmailSettingID == oCRMEmailSettingModel.EmailSettingID && em.CompanyID == companyID && em.BranchCode == branchID).FirstOrDefault();
                        if (updateEmailSetting != null)
                        {
                            updateEmailSetting.EmailAddress = oCRMEmailSettingModel.EmailAddress;
                            updateEmailSetting.SMTPHost = oCRMEmailSettingModel.SMTPHost;
                            updateEmailSetting.Port = oCRMEmailSettingModel.Port;
                            updateEmailSetting.UserName = oCRMEmailSettingModel.UserName;
                            updateEmailSetting.Password = oCRMEmailSettingModel.Password;
                            updateEmailSetting.SSL = oCRMEmailSettingModel.SSL;
                            updateEmailSetting.CCEmail = oCRMEmailSettingModel.CCEmail;
                            updateEmailSetting.DisplayName = oCRMEmailSettingModel.DisplayName;
                            //updateEmailSetting.CompanyID = companyID;
                            //updateEmailSetting.BranchCode = branchID;
                            updateEmailSetting.ModifiedBy = oCRMEmailSettingModel.UID;
                            updateEmailSetting.ModifiedDate = localTime;
                            db.SaveChanges();
                            SuccessMessage = "Email setting updated successfully";
                        }
                        else
                        {
                            ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                        }
                    }
                    else
                    {
                        var emailCheck = db.crm_emailsetting.Where(em => em.EmailAddress == oCRMEmailSettingModel.EmailAddress && em.CompanyID == companyID && em.BranchCode == branchID).FirstOrDefault();
                        if (emailCheck == null)
                        {
                            crm_emailsetting emsetting = new crm_emailsetting();
                            emsetting.EmailAddress = oCRMEmailSettingModel.EmailAddress;
                            emsetting.SMTPHost = oCRMEmailSettingModel.SMTPHost;
                            emsetting.Port = oCRMEmailSettingModel.Port;
                            emsetting.UserName = oCRMEmailSettingModel.UserName;
                            emsetting.Password = oCRMEmailSettingModel.Password;
                            emsetting.SSL = oCRMEmailSettingModel.SSL;
                            emsetting.CCEmail = oCRMEmailSettingModel.CCEmail;
                            emsetting.DisplayName = oCRMEmailSettingModel.DisplayName;
                            emsetting.CompanyID = companyID;
                            emsetting.BranchCode = branchID;
                            emsetting.CreatedBy = oCRMEmailSettingModel.UID;
                            emsetting.CreatedDate = localTime;

                            db.crm_emailsetting.Add(emsetting);
                            if (db.SaveChanges() > 0)
                            {
                                SuccessMessage = "Email setting added successfully";
                            }
                        }
                        else
                        {
                            ErrorMessage = "This email address is already available.";
                        }
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
        #endregion
    }
}
