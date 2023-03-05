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
    public class CRM_InterviewStatusMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_InterviewStatusMaster/GetInterviewStatusList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetInterviewStatusList(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;


            var ISM = new List<InterviewStatusApiModel>();
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

                var data = db.crm_intervewstatus.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToList();
                if (data != null && data.Count > 0)
                {
                    ISM = (from d in data
                                select new InterviewStatusApiModel
                                {
                                    Id = d.ID,
                                    InterViewStatusName = d.InterViewStatus,
                                    Status = (bool)d.Status,
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
                return Request.CreateResponse(HttpStatusCode.OK, ISM);
            }
        }


        //api/CRM_InterviewStatusMaster/AddUpdateInterviewStatus
        [HttpPost]
        public HttpResponseMessage AddUpdateInterviewStatus([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                InterviewStatusApiModel CIM = JsonConvert.DeserializeObject<InterviewStatusApiModel>(postData.ToString());

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

                if (string.IsNullOrEmpty(CIM.InterViewStatusName))
                {
                    ErrorMessage = "Please enter interview status name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                if (CIM.Id > 0)
                {
                    var GetData = db.crm_intervewstatus.Where(em => em.ID == CIM.Id && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        GetData.InterViewStatus = CIM.InterViewStatusName;
                        
                        db.SaveChanges();
                        SuccessMessage = "Interview status updated successfully";
                    }
                    else
                    {
                        ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    }
                }
                else
                {
                    var checkExist = db.crm_intervewstatus.Where(em => em.InterViewStatus.ToLower() == CIM.InterViewStatusName.ToLower() && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_intervewstatus lst = new crm_intervewstatus();
                        lst.InterViewStatus = CIM.InterViewStatusName;
                        lst.BranchID = branchID;
                        lst.CompanyID = companyID;
                        lst.Status = true;
                        db.crm_intervewstatus.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            SuccessMessage = "Interview Status added successfully";
                        }
                    }
                    else
                    {
                        ErrorMessage = "This Interview status name is already available";
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

        //api/CRM_InterviewStatusMaster/GetInterviewStatusById?interviewStsId=906
        [HttpGet]
        public HttpResponseMessage GetInterviewStatusById(int interviewStsId)
        {
            string ErrorMessage = string.Empty;


            var ISM = new InterviewStatusApiModel();
            try
            {
                var data = db.crm_intervewstatus.Where(em => em.ID == interviewStsId).FirstOrDefault();
                if (data != null)
                {
                    ISM.Id = data.ID;
                    ISM.InterViewStatusName = data.InterViewStatus;
                    ISM.Status = Convert.ToBoolean(data.Status);
                    ISM.CompanyID = data.CompanyID;
                    ISM.BranchID = data.BranchID;
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
                return Request.CreateResponse(HttpStatusCode.OK, ISM);
            }
        }

        //api/CRM_InterviewStatusMaster/ChangeErrorTypeStatusById?interviewStsId=906&CompanyID=307&BranchID=184&Status=true&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage ChangeInterviewStatusById(int interviewStsId, string CompanyID, string BranchID, bool Status, string Token)
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

                var data = db.crm_intervewstatus.Where(em => em.ID == interviewStsId).FirstOrDefault();
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
