using Svam.EF;
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
    public class CRMViewAssignedLeadFilterController : ApiController
    { 
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //Int32? AssignedUserID,
        [HttpGet]
        public HttpResponseMessage Get(Int32 CompanyID, Int32 BranchID, string ProfileName,Int32? LeadOwnerID,  string UserID, String FromDate, String ToDate, String SearchStatusName, string Token)
        {
            List<APIViewLeadModel> viewleadsList = new List<APIViewLeadModel>();
            String Records = String.Empty;
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
            try
            {
               
                var dd = Constant.GetimeForApi(CompanyID);
                DateTime MonthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = MonthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthEndDate.ToString("dd/MM/yyyy");

                //if (Convert.ToString(ProfileName) == "SuperAdmin")
                //{
                //    if (LeadOwnerID == 0)
                //    {
                //        LeadOwnerID = 0;
                //    }
                //}
                //else 
                //{
                //    if (LeadOwnerID == 0)
                //    {
                //        LeadOwnerID = 0;
                //    }
                //}

                var UID = 0;
                if (Convert.ToString(ProfileName) == "SuperAdmin")
                {
                    UID = 0;
                    if (LeadOwnerID != null && LeadOwnerID != 0)
                    {                     
                        UID = Convert.ToInt32(LeadOwnerID);                        
                    }
                }
                else
                {
                    if (LeadOwnerID != null && LeadOwnerID != 0)
                    {
                        UID = Convert.ToInt32(LeadOwnerID);
                    }
                    else
                    {
                        UID = Convert.ToInt32(UserID);
                    }
                }


                if (!String.IsNullOrWhiteSpace(FromDate))
                {
                    MStartDate = FromDate;
                }
                if (!String.IsNullOrWhiteSpace(ToDate))
                {
                    MEndDate = ToDate;
                }

                DataTable GetLeads = DataAccessLayer.GetDataTable("call CRM_GetAssignedLeadList(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetLeads.Rows.Count > 0)
                {
                    List<APIViewLeadModel> vlmList = new List<APIViewLeadModel>();
                    for (int i = 0; i < GetLeads.Rows.Count; i++)
                    {
                        APIViewLeadModel vlm = new APIViewLeadModel();
                        int spaceIndex = 0;
                        string afterSpaceValue = "";
                        //var FullName = Convert.ToString(GetLeads.Rows[i]["Customer"]);
                        //var splitted = FullName.Split(new[] { ' ' }, 2);
                        //if (splitted[0] != null)
                        //{
                        //    string First = splitted[0].Substring(0, 1);
                        //    vlm.ShortName = Convert.ToString(First).ToUpper();
                        //}
                        //if (splitted.Length == 2)
                        //{
                        //    string First = splitted[0].Substring(0, 1);
                        //    string Last = splitted[1].Substring(0, 1);
                        //    vlm.ShortName = Convert.ToString(First + Last).ToUpper();
                        //}
                        vlm.Id = Convert.ToInt32(GetLeads.Rows[i]["Id"]);
                        vlm.LeadOwner = Convert.ToString(GetLeads.Rows[i]["LeadOwnerName"]);
                        vlm.LeadName = Convert.ToString(GetLeads.Rows[i]["Customer"]);

                        spaceIndex = vlm.LeadName.IndexOf(' ');//get index of fist space
                        afterSpaceValue = spaceIndex > 0 && vlm.LeadName.Length > (spaceIndex + 1) ? vlm.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
                        vlm.ShortName = !string.IsNullOrEmpty(vlm.LeadName) ? vlm.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

                        vlm.Mob = Convert.ToString(GetLeads.Rows[i]["MobileNo"]);
                        vlm.EMail = Convert.ToString(GetLeads.Rows[i]["EmailId"]);
                        vlm.FollowupDate = Convert.ToString(GetLeads.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        vlm.LeadStatus = Convert.ToString(GetLeads.Rows[i]["LeadStatus"]);                       
                        vlm.AssignDate = Convert.ToString(GetLeads.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        vlm.AssignTo = Convert.ToInt32(GetLeads.Rows[i]["AssignTo"]);
                        vlm.AssignToUserName = Convert.ToString(GetLeads.Rows[i]["AssignToUserName"]);
                        vlm.AssignByUserName = Convert.ToString(GetLeads.Rows[i]["AssignByUserName"]);
                        vlm.Country = Convert.ToString(GetLeads.Rows[i]["CountryName"]);
                        vlm.State = Convert.ToString(GetLeads.Rows[i]["StateName"]);
                        vlm.City = Convert.ToString(GetLeads.Rows[i]["CityName"]);
                        vlmList.Add(vlm);
                    }
                    viewleadsList = vlmList;
                }
                
                if (SearchStatusName != null && SearchStatusName != "" && SearchStatusName != "Select Lead Status")
                {
                    var FilterData = viewleadsList.Where(em => em.LeadStatus.Contains(SearchStatusName)).ToList();
                    viewleadsList = FilterData;
                }
                if (SearchStatusName != "Not Interested")
                {
                    var FilterData = viewleadsList.Where(em => em.LeadStatus != "Not Interested").ToList();
                    viewleadsList = FilterData;
                }

                //if (AssignedUserID>0)
                //{
                    //viewleadsList = viewleadsList.Where(em => em.AssignTo == AssignedUserID).ToList();
                    if (viewleadsList.Count == 0)
                    {
                        Records = "** No Record Found **";
                    }
                //}

                if (viewleadsList.Count == 0)
                {
                    Records = "** No Record Found **";
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
                    return Request.CreateResponse(HttpStatusCode.OK, viewleadsList);
                }
            }
        }
    }
}
