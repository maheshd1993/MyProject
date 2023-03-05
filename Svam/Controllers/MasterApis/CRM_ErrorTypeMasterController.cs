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
    public class CRM_ErrorTypeMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_ErrorTypeMaster/GetErrorTypeList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetErrorTypeList(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;


            var errTypes = new List<ErrorTypeApiModel>();
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

                var data = db.crm_errortype.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToList();
                if (data != null && data.Count > 0)
                {
                    errTypes = (from d in data
                                select new ErrorTypeApiModel
                                {
                                    Id = d.ErrrorID,
                                    ErrorName = d.ErrorName,
                                    Status = (bool)d.IsActive,
                                    BranchID = d.BranchID,
                                    CompanyID = d.CompanyID
                                }
                        ).ToList();
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
                return Request.CreateResponse(HttpStatusCode.OK, errTypes);
            }
        }


        //api/CRM_ErrorTypeMaster/AddUpdateErrorType
        [HttpPost]
        public HttpResponseMessage AddUpdateErrorType([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                ErrorTypeApiModel CIM = JsonConvert.DeserializeObject<ErrorTypeApiModel>(postData.ToString());

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

                if (string.IsNullOrEmpty(CIM.ErrorName))
                {
                    ErrorMessage = "Please enter error type name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                if (CIM.Id > 0)
                {
                    var GetData = db.crm_errortype.Where(em => em.ErrrorID == CIM.Id && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        GetData.ErrorName = CIM.ErrorName;
                        GetData.ModifiedOn = Constant.GetimeForApi(companyID);
                        GetData.ModifiedBy = Convert.ToInt32(CIM.UID);
                        db.SaveChanges();
                        SuccessMessage = "Error type updated successfully";
                    }
                    else
                    {
                        ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    }
                }
                else
                {
                    var checkExist = db.crm_errortype.Where(em => em.ErrorName.ToLower() == CIM.ErrorName.ToLower() && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_errortype lst = new crm_errortype();
                        lst.ErrorName = CIM.ErrorName;
                        lst.BranchID = branchID;
                        lst.CompanyID = companyID;
                        lst.IsActive = true;
                        lst.IsDeleted = false;
                        lst.CreatedOn = Constant.GetimeForApi(companyID);
                        lst.CreatedBy = Convert.ToInt32(CIM.UID);
                        db.crm_errortype.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            SuccessMessage = "Error type added successfully";
                        }
                    }
                    else
                    {
                        ErrorMessage = "This error type is already available";
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

        //api/CRM_ErrorTypeMaster/GetErrorTypeById?errorId=14
        [HttpGet]
        public HttpResponseMessage GetErrorTypeById(int errorId)
        {
            string ErrorMessage = string.Empty;


            var ETM = new ErrorTypeApiModel();
            try
            {
                var data = db.crm_errortype.Where(em => em.ErrrorID == errorId).FirstOrDefault();
                if (data != null)
                {
                    ETM.Id = data.ErrrorID;
                    ETM.ErrorName = data.ErrorName;
                    ETM.Status = Convert.ToBoolean(data.IsActive);
                    ETM.CompanyID = data.CompanyID;
                    ETM.BranchID = data.BranchID;
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
                return Request.CreateResponse(HttpStatusCode.OK, ETM);
            }
        }

        //api/CRM_ErrorTypeMaster/ChangeErrorTypeStatusById?errorId=14&CompanyID=307&BranchID=184&Status=true&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage ChangeErrorTypeStatusById(int errorId, string CompanyID, string BranchID, bool Status, string Token)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);

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
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                var data = db.crm_errortype.Where(em => em.ErrrorID == errorId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = Status == true ? true : false;
                    db.SaveChanges();

                    SuccessMessage = "Status changed successfully";
                }
                else
                {
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
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
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }
    }
}
