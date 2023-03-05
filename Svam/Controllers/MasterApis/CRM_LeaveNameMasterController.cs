using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.MasterApis
{
    public class CRM_LeaveNameMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_LeaveNameMaster/GetLeaveNameList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public async Task<HttpResponseMessage>  GetLeaveNameList(string CompanyID, string BranchID, string Token) 
        {
            string ErrorMessage = string.Empty;


            var LS = new List<LeaveTypeApiVM>();
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

                var data =await db.crm_leavetypename.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToListAsync();
                if (data != null && data.Count > 0)
                {
                    LS = (from d in data
                          select new LeaveTypeApiVM
                          {
                              ID = d.ID,
                              LeaveName = d.LeaveName,
                              IsActive = d.IsActive                              
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


        //api/CRM_LeaveNameMaster/AddUpdateLeaveName
        [HttpPost]
        public async Task<HttpResponseMessage> AddUpdateLeaveName([FromBody]JToken postData, HttpRequestMessage request) 
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                LeaveTypeApiDTO LS = JsonConvert.DeserializeObject<LeaveTypeApiDTO>(postData.ToString());

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

                if (string.IsNullOrEmpty(LS.LeaveName))
                {
                    ErrorMessage = "Please enter leave name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                var dt = Constant.GetimeForApi(companyID);
                #endregion
                if (LS.ID > 0)
                {
                    var GetData = await db.crm_leavetypename.Where(em => em.ID == LS.ID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                    if (GetData != null)
                    {
                        GetData.LeaveName = LS.LeaveName;
                        GetData.IsDeleted = false;
                        GetData.ModifiedOn = dt;
                        GetData.ModifiedBy = Convert.ToInt32(LS.UID);
                        await db.SaveChangesAsync();
                        SuccessMessage = "Leave name updated successfully";
                    }
                    else
                    {
                        ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    }
                }
                else
                {
                    var checkExist =await db.crm_leavetypename.Where(em => em.LeaveName.ToLower() == LS.LeaveName.ToLower() && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                    if (checkExist == null)
                    {
                        crm_leavetypename lst = new crm_leavetypename();
                        lst.LeaveName = LS.LeaveName;
                        lst.BranchID = branchID;
                        lst.CompanyID = companyID;
                        lst.IsActive = true;
                        lst.IsDeleted = false;
                        lst.CreatedOn = dt;
                        lst.CreatedBy = Convert.ToInt32(LS.UID);
                        db.crm_leavetypename.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            SuccessMessage = "Leave name added successfully";
                        }
                    }
                    else
                    {
                        ErrorMessage = "This leave name is already exists in our record";
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

        //api/CRM_LeaveNameMaster/GetLeaveNameById?leaveNameId=10
        [HttpGet]
        public async Task<HttpResponseMessage> GetLeaveNameById(int leaveNameId)
        {
            string ErrorMessage = string.Empty;


            var LSM = new LeaveTypeApiDTO();
            try
            {
                var data =await db.crm_leavetypename.Where(em => em.ID == leaveNameId).FirstOrDefaultAsync();
                if (data != null)
                {
                    LSM.ID = data.ID;
                    LSM.LeaveName = data.LeaveName;               
                    LSM.IsActive =data.IsActive;
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

        //api/CRM_LeaveNameMaster/LeaveNameIsActiveChangeById?leaveNameId=10&CompanyID=307&BranchID=184&IsActive=true&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public async Task<HttpResponseMessage> LeaveNameIsActiveChangeById(int leaveNameId , string CompanyID, string BranchID, bool IsActive , string Token) 
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

                var data = await db.crm_leavetypename.Where(em => em.ID == leaveNameId).FirstOrDefaultAsync();
                if (data != null)
                {
                    data.IsActive = IsActive == true ? true : false;
                   await db.SaveChangesAsync();

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
