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
    public class CRM_LeadSourceMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_LeadSourceMaster/GetLeadSourceList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetLeadSourceList(string CompanyID, string BranchID, string Token) 
        {
            string ErrorMessage = string.Empty;


            var leadSources = new List<LeadSourceApiModel>();
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

                var data = db.crm_leadsource_tbl.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToList();
                if (data != null && data.Count > 0)
                {
                    leadSources = (from d in data
                          select new LeadSourceApiModel
                          {
                              Id = d.Id,
                              LeadsourceName = d.LeadsourceName,
                              Status = (bool)d.Status,
                              BranchID=d.BranchID,
                              CompanyID=d.CompanyID
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
                return Request.CreateResponse(HttpStatusCode.OK, leadSources);
            }
        }


        //api/CRM_LeadSourceMaster/AddUpdateLeadSource
        [HttpPost]
        public HttpResponseMessage AddUpdateLeadSource([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                LeadSourceApiModel CIM = JsonConvert.DeserializeObject<LeadSourceApiModel>(postData.ToString());

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

                if (string.IsNullOrEmpty(CIM.LeadsourceName))
                {
                    ErrorMessage = "Please enter lead source name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                if (CIM.Id > 0)
                {
                    var GetData = db.crm_leadsource_tbl.Where(em => em.Id == CIM.Id && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        GetData.LeadsourceName = CIM.LeadsourceName;                        
                        db.SaveChanges();
                        SuccessMessage = "Lead source updated successfully";
                    }
                    else
                    {
                        ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    }
                }
                else
                {
                    var checkExist = db.crm_leadsource_tbl.Where(em => em.LeadsourceName.ToLower() == CIM.LeadsourceName.ToLower() && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_leadsource_tbl lst = new crm_leadsource_tbl();
                        lst.LeadsourceName = CIM.LeadsourceName;
                        lst.BranchID = branchID;
                        lst.CompanyID = companyID;
                        lst.Status = true;
                        lst.Created_at = Constant.GetimeForApi(companyID);
                        db.crm_leadsource_tbl.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            SuccessMessage = "Lead source added successfully";
                        }
                    }
                    else
                    {
                        ErrorMessage = "This Lead source name is already available.";
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

        //api/CRM_LeadSourceMaster/GetLeadSourceById?leadSourceId=852
        [HttpGet]
        public HttpResponseMessage GetLeadSourceById(int leadSourceId)
        {
            string ErrorMessage = string.Empty;


            var LS = new LeadSourceApiModel();
            try
            {
                var data = db.crm_leadsource_tbl.Where(em => em.Id == leadSourceId).FirstOrDefault();
                if (data != null)
                {
                    LS.Id = data.Id;
                    LS.LeadsourceName = data.LeadsourceName;
                    LS.Status = Convert.ToBoolean(data.Status);
                    LS.CompanyID = data.CompanyID;
                    LS.BranchID = data.BranchID;
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

        //api/CRM_LeadSourceMaster/ChangeLeadSourceStatusById?leadSourceId=852&CompanyID=307&BranchID=184&Status=true&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage ChangeLeadSourceStatusById(int leadSourceId, string CompanyID, string BranchID, bool Status, string Token)
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

                var data = db.crm_leadsource_tbl.Where(em => em.Id == leadSourceId).FirstOrDefault();
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
