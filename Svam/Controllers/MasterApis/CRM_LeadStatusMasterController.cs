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
    public class CRM_LeadStatusMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_LeadStatusMaster/GetLeadStatusList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetLeadStatusList(string CompanyID, string BranchID, string Token) 
        {
            string ErrorMessage = string.Empty;


            var LS = new List<LeadStatusApiModel>();
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

                var data = db.crm_leadstatus_tbl.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToList();
                if (data != null && data.Count > 0)
                {
                    LS = (from d in data
                          select new LeadStatusApiModel
                          {
                              Id = d.Id,
                              LeadStatusName=d.LeadStatusName,
                              Status = (bool)d.Status,
                              ColorHexValue=d.ColorHexValue
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
                return Request.CreateResponse(HttpStatusCode.OK, LS);
            }
        }


        //api/CRM_LeadStatusMaster/AddUpdateLeadStatus
        [HttpPost]
        public HttpResponseMessage AddUpdateLeadStatus([FromBody]JToken postData, HttpRequestMessage request)  
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                LeadStatusApiModel LS = JsonConvert.DeserializeObject<LeadStatusApiModel>(postData.ToString());

                int branchID = Convert.ToInt32(LS.BranchID);
                int companyID = Convert.ToInt32(LS.CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, LS.Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                #region check nullable validation

                if (string.IsNullOrEmpty(LS.LeadStatusName))
                {
                    ErrorMessage = "Please enter lead status name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                if (LS.Id > 0)
                {
                    var GetData = db.crm_leadstatus_tbl.Where(em => em.Id == LS.Id && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        GetData.LeadStatusName = LS.LeadStatusName;
                        GetData.ColorHexValue = LS.ColorHexValue;
                        //GetData.BranchID = branchID;
                        //GetData.CompanyID = companyID;
                        db.SaveChanges();
                        SuccessMessage = "Lead Status updated successfully";
                    }
                    else
                    {
                        ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    }
                }
                else
                {
                    var checkExist = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName.ToLower() == LS.LeadStatusName.ToLower() && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_leadstatus_tbl lst = new crm_leadstatus_tbl();
                        lst.LeadStatusName = LS.LeadStatusName;
                        lst.ColorHexValue = LS.ColorHexValue;
                        lst.BranchID = branchID;
                        lst.CompanyID = companyID;
                        lst.Status = true;
                        lst.created_at = Constant.GetimeForApi(companyID);
                        db.crm_leadstatus_tbl.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            SuccessMessage = "Lead Status added successfully";
                        }
                    }
                    else
                    {
                        ErrorMessage = "This Status name is already available";
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

        //api/CRM_LeadStatusMaster/GetLeadStatusById?leadStatusId=795
        [HttpGet]
        public HttpResponseMessage GetLeadStatusById(int leadStatusId)
        {
            string ErrorMessage = string.Empty;


            var LSM  = new LeadStatusApiModel();
            try
            {
                var data = db.crm_leadstatus_tbl.Where(em => em.Id == leadStatusId).FirstOrDefault();
                if (data != null)
                {
                    LSM.LeadStatusName = data.LeadStatusName;
                    LSM.ColorHexValue = data.ColorHexValue;
                    LSM.Status = Convert.ToBoolean(data.Status);
                    LSM.CompanyID = data.CompanyID;
                    LSM.BranchID = data.BranchID;
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
                return Request.CreateResponse(HttpStatusCode.OK, LSM);
            }
        }

        //api/CRM_LeadStatusMaster/LeadStatusChangeById?leadStatusId=795&CompanyID=307&BranchID=184&Status=true&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage LeadStatusChangeById(int leadStatusId, string CompanyID, string BranchID, bool Status, string Token)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;


            var JP = new JobProfileModel();
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

                var data = db.crm_leadstatus_tbl.Where(em => em.Id == leadStatusId).FirstOrDefault();
                if (data != null)
                {
                    data.Status = Status == true ? true : false;
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
