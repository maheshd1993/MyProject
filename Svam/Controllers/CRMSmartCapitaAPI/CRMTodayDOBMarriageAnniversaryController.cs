using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMTodayDOBMarriageAnniversaryController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(Int32 CompanyID, Int32 BranchID, string ProfileName, Int32? LoginID, string Token)
        {
            string ErrorMessage = string.Empty;
            //string Token = string.Empty;

            //var re = Request;
            //var headers = re.Headers;

            //if (headers.Contains("Token"))
            //{
            //    Token = headers.GetValues("Token").First();
            //}
            var auth = Utility.TokenVerify(CompanyID, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            List<DOBModel> DashBoardLeadsModelList = new List<DOBModel>();
            String Records = String.Empty;
            
            try
            {
                
                var dd = Constant.GetimeForApi(CompanyID);
                DateTime MonthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = MonthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthEndDate.ToString("dd/MM/yyyy");

                if (!string.IsNullOrEmpty(ProfileName) && ProfileName.Trim('"') == "SuperAdmin")
                {
                    DOBModel CPM = new DOBModel();
                    DataTable GetDOBAMList = DataAccessLayer.GetDataTable("call CRM_TodayDOBMarriageAnniversary(" + BranchID + "," + CompanyID + "," + 0 + ")");
                    if (GetDOBAMList.Rows.Count > 0)
                    {
                        List<DOBModel> BLMList = new List<DOBModel>();
                        for (int i = 0; i < GetDOBAMList.Rows.Count; i++)
                        {
                            DOBModel cPM = new DOBModel();
                            int spaceIndex = 0;
                            string afterSpaceValue = "";
                            cPM.Id = Convert.ToInt32(GetDOBAMList.Rows[i]["Id"]);
                            cPM.LeadName = Convert.ToString(GetDOBAMList.Rows[i]["Customer"]);

                            spaceIndex = cPM.LeadName.IndexOf(' ');//get index of fist space
                            afterSpaceValue = spaceIndex > 0 && cPM.LeadName.Length > (spaceIndex + 1) ? cPM.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
                            cPM.NickName = !string.IsNullOrEmpty(cPM.LeadName) ? cPM.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

                            cPM.Email = Convert.ToString(GetDOBAMList.Rows[i]["EmailId"]);
                            cPM.Phone = Convert.ToString(GetDOBAMList.Rows[i]["MobileNo"]);
                            cPM.DateofBirth = Convert.ToString(GetDOBAMList.Rows[i]["DateofBirth"]);
                            cPM.MarriageAnniversary = Convert.ToString(GetDOBAMList.Rows[i]["MarriageAnniversary"]);
                            cPM.CreatedBy = Convert.ToString(GetDOBAMList.Rows[i]["Fname"] + " " + GetDOBAMList.Rows[i]["Lname"]);
                            cPM.FollowUpDate = Convert.ToString(GetDOBAMList.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            cPM.LeadStatus = Convert.ToString(GetDOBAMList.Rows[i]["LeadStatus"]);
                            cPM.AssignedBy = Convert.ToString(GetDOBAMList.Rows[i]["AssinedBy"]);
                            BLMList.Add(cPM);
                        }
                        DashBoardLeadsModelList = BLMList;
                    }
                    else
                    {
                        Records = "** No Record Found **";
                    }
                }
                else 
                {
                    DOBModel CPM = new DOBModel();
                    DataTable GetDOBAMList = DataAccessLayer.GetDataTable("call CRM_TodayDOBMarriageAnniversary(" + BranchID + "," + CompanyID + "," + LoginID  + ")");
                    if (GetDOBAMList.Rows.Count > 0)
                    {
                        List<DOBModel> BLMList = new List<DOBModel>();
                        for (int i = 0; i < GetDOBAMList.Rows.Count; i++)
                        {
                            DOBModel cPM = new DOBModel();
                            int spaceIndex = 0;
                            string afterSpaceValue = "";
                            cPM.Id = Convert.ToInt32(GetDOBAMList.Rows[i]["Id"]);
                            cPM.LeadName = Convert.ToString(GetDOBAMList.Rows[i]["Customer"]);

                            spaceIndex = cPM.LeadName.IndexOf(' ');//get index of fist space
                            afterSpaceValue = spaceIndex > 0 && cPM.LeadName.Length > (spaceIndex + 1) ? cPM.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
                            cPM.NickName = !string.IsNullOrEmpty(cPM.LeadName) ? cPM.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

                            cPM.Email = Convert.ToString(GetDOBAMList.Rows[i]["EmailId"]);
                            cPM.Phone = Convert.ToString(GetDOBAMList.Rows[i]["MobileNo"]);
                            cPM.DateofBirth = Convert.ToString(GetDOBAMList.Rows[i]["DateofBirth"]);
                            cPM.MarriageAnniversary = Convert.ToString(GetDOBAMList.Rows[i]["MarriageAnniversary"]);
                            cPM.CreatedBy = Convert.ToString(GetDOBAMList.Rows[i]["Fname"] + " " + GetDOBAMList.Rows[i]["Lname"]);
                            cPM.FollowUpDate = Convert.ToString(GetDOBAMList.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            cPM.LeadStatus = Convert.ToString(GetDOBAMList.Rows[i]["LeadStatus"]);
                            cPM.AssignedBy = Convert.ToString(GetDOBAMList.Rows[i]["AssinedBy"]);
                            BLMList.Add(cPM);
                        }
                        DashBoardLeadsModelList = BLMList;
                    }
                    else
                    {
                        Records = "** No Record Found **";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                ExceptionLogging.SendExcepToDB(ex);
            }

             if (ErrorMessage != string.Empty)
             {
                 HttpError err = new HttpError(ErrorMessage);
                 return Request.CreateResponse(HttpStatusCode.BadRequest, err);
             }
             else
             {
                 if (Records != string.Empty)
                 {
                     return Request.CreateResponse(HttpStatusCode.OK, Records);
                 }
                 else
                 {
                     return Request.CreateResponse(HttpStatusCode.OK, DashBoardLeadsModelList);
                 }
             }
        }
    }
}
