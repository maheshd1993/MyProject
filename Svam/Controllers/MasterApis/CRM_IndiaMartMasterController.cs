using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Svam.Controllers.MasterApis
{
    public class CRM_IndiaMartMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_IndiaMartMaster/GetIndiaMartSetting?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetIndiaMartSetting(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;


            var CIM = new CRMIndiaMartModel();
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

                var getSetting = db.crm_indiamartsetting.Where(em => em.CompanyID == companyID && em.BranchID == branchID).FirstOrDefault();
                if (getSetting != null)
                {
                    CIM.IndiaMartID = getSetting.IndiaMartID;
                    CIM.MobileNumber = getSetting.MobileNumber;
                    CIM.IndiaMartCRMKey = getSetting.IndiaMartCRMKey;
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
                return Request.CreateResponse(HttpStatusCode.OK, CIM);
            }
        }


        //api/CRM_IndiaMartMaster/AddIndiaMartSetting
        [HttpPost]
        public HttpResponseMessage AddIndiaMartSetting([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                CRMIndiaMartModel CIM  = JsonConvert.DeserializeObject<CRMIndiaMartModel>(postData.ToString());
               
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

                if (string.IsNullOrEmpty(CIM.IndiaMartCRMKey))
                {
                    ErrorMessage = "Please enter indiamart crm key";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

               
               
                if (string.IsNullOrEmpty(CIM.MobileNumber))
                {
                    ErrorMessage = "Please enter register mobile no.";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                var match = Regex.IsMatch(CIM.MobileNumber, @"^[0-9]{10}$");//match mobile no is 10 digit or not
                if (!match)
                {
                    ErrorMessage = "Invalid mobile no.";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                if (CIM.IndiaMartID > 0)
                {
                    var updateSetting = db.crm_indiamartsetting.Where(em => em.IndiaMartID == CIM.IndiaMartID && em.CompanyID == companyID && em.BranchID == branchID).FirstOrDefault();
                    if (updateSetting != null)
                    {
                        updateSetting.MobileNumber = CIM.MobileNumber;
                        updateSetting.IndiaMartCRMKey = CIM.IndiaMartCRMKey;
                        //updateSetting.BranchID = BranchID;
                        //updateSetting.CompanyID = CompanyID;
                        db.SaveChanges();
                        SuccessMessage = "IndiaMart setting updated successfully";
                    }
                    else
                    {
                        ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    }
                }
                else
                {
                    var settingCheck = db.crm_indiamartsetting.Where(em => em.MobileNumber == CIM.MobileNumber && em.CompanyID == companyID && em.BranchID == branchID).FirstOrDefault();
                    if (settingCheck == null)
                    {
                        crm_indiamartsetting cims = new crm_indiamartsetting();
                        cims.MobileNumber = CIM.MobileNumber;
                        cims.IndiaMartCRMKey = CIM.IndiaMartCRMKey;
                        cims.BranchID = branchID;
                        cims.CompanyID = companyID;
                        db.crm_indiamartsetting.Add(cims);
                        if (db.SaveChanges() > 0)
                        {
                            SuccessMessage = "IndiaMart setting added successfully";
                        }
                    }
                    else
                    {
                        ErrorMessage = "Registered mobile number is already available.";
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
    }
}
