using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.GeneralModuleApis
{
    public class CRM_GeneralController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
        //api/CRM_General/CRM_ViewGeneralRemark?UID=61&ProfileName=SuperAdmin&CompanyID=307&BranchID=184&FromDate=&ToDate=&Token=VwFdB3OPEwOoHnr15a5qgg==
        /// <param name="UID"></param>  
        /// <param name="ProfileName"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage CRM_ViewGeneralRemark(int UID, string ProfileName, int CompanyID, int BranchID, string FromDate, string ToDate, string Token) 
        {
            string ErrorMessage = string.Empty;
            List<ViewGeneralRemarkApiModel> remarks = new List<ViewGeneralRemarkApiModel>();

            try
            {
                //int branchID = Convert.ToInt32(BranchID);
                //int companyID = Convert.ToInt32(CompanyID);

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

                var auth = Utility.TokenVerify(CompanyID, Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                var DateFormat = Constant.DateFormatForApi(CompanyID);//get date format by company id
                int uid = UID;
                if (ProfileName == "SuperAdmin")
                {
                    uid = 0;
                    //if (SearchUserId != null)
                    //{
                    //    uid = Convert.ToInt32(SearchUserId);
                    //}
                    //else
                    //{
                    //    uid = 0;
                    //}
                }

                #region Data-time-Formate
                var dd = Constant.GetimeForApi(CompanyID);
                DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

                if (!string.IsNullOrEmpty(FromDate) && FromDate!="null" && !string.IsNullOrEmpty(ToDate) && ToDate!="null")
                {

                    if (DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {
                        var fmDate = DateTime.ParseExact(FromDate, DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(fromdate);
                        var tDate = DateTime.ParseExact(ToDate, DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(toDate);

                        MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                        MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                    }
                    else
                    {
                        MStartDate = FromDate;
                        MEndDate = ToDate;
                    }
                }
                #endregion


                DataTable getRemark = DataAccessLayer.GetDataTable("call CRM_GetCommonActivityReport('" + uid + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (getRemark.Rows.Count > 0)
                {
                    remarks = (from dr in getRemark.AsEnumerable()
                                                     select new ViewGeneralRemarkApiModel()
                                                     {
                                                         Id = Convert.ToInt32(dr["Id"]),
                                                         UID = Convert.ToInt32(dr["UID"]),
                                                         UserName = Convert.ToString(dr["UserName"]),
                                                         Profile = Convert.ToString(dr["ProfileName"]),
                                                         Email = Convert.ToString(dr["Email"]),
                                                         Remarks = Convert.ToString(dr["Remark"]),
                                                         Date = String.Format("{0:" + DateFormat + "}", Convert.ToDateTime(dr["Created_at"]))
                                                     }).ToList();
                }
                else
                {
                    ErrorMessage = "No record found";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            return Request.CreateResponse(HttpStatusCode.OK, remarks);

        }


        [HttpPost]
        public async Task<HttpResponseMessage> CreateRemark([FromBody]JToken postData)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            try
            {
                CreateGeneralRemarkModel value = JsonConvert.DeserializeObject<CreateGeneralRemarkModel>(postData.ToString());

                
                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(value.CompanyId, value.Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }



                if (value.UID == 0)
                {
                    ErrorMessage = "Please enter user id";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                if (string.IsNullOrEmpty(value.Remark))
                {
                    ErrorMessage = "Please enter remark";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                crm_commonactivityremarktbl CAR = new crm_commonactivityremarktbl();

                    CAR.Remark = value.Remark;
                    CAR.UID =value.UID;
                    CAR.Created_at = Constant.GetimeForApi(value.CompanyId).Date.ToString("dd/MM/yyyy");
                    CAR.Status = true;
                    CAR.BranchID = value.BranchId;
                    CAR.CompanyID = value.CompanyId;
                    db.crm_commonactivityremarktbl.Add(CAR);
                    await db.SaveChangesAsync();  
                
                    SuccessMessage = "Remark added successfully";
                     
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the Post Data Parameters");
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
