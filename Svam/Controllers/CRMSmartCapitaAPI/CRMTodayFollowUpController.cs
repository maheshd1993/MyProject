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
    public class CRMTodayFollowUpController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        [HttpGet]
        public HttpResponseMessage Get(Int32 CompanyID, Int32 BranchID, string ProfileName, Int32? LoginID, string Token)
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
                if (ProfileName == "SuperAdmin")
                {
                    DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable("call CRM_TodayFollowupLeads(" + BranchID + "," + CompanyID + "," + 0 + ")");
                    if (TodayFollowupLeads.Rows.Count > 0)
                    {
                        List<APIViewLeadModel> vlmList = new List<APIViewLeadModel>();
                        for (int i = 0; i < TodayFollowupLeads.Rows.Count; i++)
                        {
                            APIViewLeadModel vlm = new APIViewLeadModel();
                            int spaceIndex = 0;
                            string afterSpaceValue = "";
                            //var FullName = Convert.ToString(TodayFollowupLeads.Rows[i]["Customer"]);
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
                            vlm.Id = Convert.ToInt32(TodayFollowupLeads.Rows[i]["Id"]);
                            vlm.LeadName = Convert.ToString(TodayFollowupLeads.Rows[i]["Customer"]);

                            spaceIndex = vlm.LeadName.IndexOf(' ');//get index of fist space
                            afterSpaceValue = spaceIndex > 0 && vlm.LeadName.Length > (spaceIndex + 1) ? vlm.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
                            vlm.ShortName = !string.IsNullOrEmpty(vlm.LeadName) ? vlm.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

                            vlm.Mob = Convert.ToString(TodayFollowupLeads.Rows[i]["MobileNo"]);
                            vlm.EMail = Convert.ToString(TodayFollowupLeads.Rows[i]["EmailId"]);
                            vlm.FollowupDate = Convert.ToString(TodayFollowupLeads.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.LeadStatus = Convert.ToString(TodayFollowupLeads.Rows[i]["LeadStatus"]);
                            vlm.Created_By = Convert.ToString(TodayFollowupLeads.Rows[i]["Fname"] + " " + TodayFollowupLeads.Rows[i]["Lname"]);
                            vlmList.Add(vlm);
                        }
                        viewleadsList = vlmList.Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();                        
                    }
                    else
                    {
                        Records = "** Today No followup Leads Assigned **";
                    }
                }
                else
                {
                    DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable("call CRM_TodayFollowupLeads(" + BranchID + "," + CompanyID + "," + LoginID + ")");
                    if (TodayFollowupLeads.Rows.Count > 0)
                    {
                        List<APIViewLeadModel> vlmList = new List<APIViewLeadModel>();
                        for (int i = 0; i < TodayFollowupLeads.Rows.Count; i++)
                        {
                            APIViewLeadModel vlm = new APIViewLeadModel();
                            int spaceIndex = 0;
                            string afterSpaceValue = "";
                            //var FullName = Convert.ToString(TodayFollowupLeads.Rows[i]["Customer"]);
                            //var splitted = FullName.Split(new[] { ' ' }, 2);
                            //if (splitted[0] != null)
                            //{
                            //    string First = splitted[0].Substring(0, 1);
                            //    vlm.ShortName = Convert.ToString(First).ToUpper();
                            //}
                            //if (splitted.Length == 2)
                            //{
                            //    string First = splitted[0].Substring(0, 1);
                            //    if (!string.IsNullOrWhiteSpace(splitted[1]))
                            //    {
                            //        string Last = splitted[1].Substring(0, 1);
                            //        vlm.ShortName = Convert.ToString(First + Last).ToUpper();
                            //    }
                            //    else 
                            //    {
                            //        vlm.ShortName = Convert.ToString(First).ToUpper();
                            //    }                               
                            //}
                            vlm.Id = Convert.ToInt32(TodayFollowupLeads.Rows[i]["Id"]);
                            vlm.LeadName = Convert.ToString(TodayFollowupLeads.Rows[i]["Customer"]);

                            spaceIndex = vlm.LeadName.IndexOf(' ');//get index of fist space
                            afterSpaceValue = spaceIndex > 0 && vlm.LeadName.Length > (spaceIndex + 1) ? vlm.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
                            vlm.ShortName = !string.IsNullOrEmpty(vlm.LeadName) ? vlm.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

                            vlm.Mob = Convert.ToString(TodayFollowupLeads.Rows[i]["MobileNo"]);
                            vlm.EMail = Convert.ToString(TodayFollowupLeads.Rows[i]["EmailId"]);
                            vlm.FollowupDate = Convert.ToString(TodayFollowupLeads.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                            vlm.LeadStatus = Convert.ToString(TodayFollowupLeads.Rows[i]["LeadStatus"]);
                            vlm.Created_By = Convert.ToString(TodayFollowupLeads.Rows[i]["Fname"] + " " + TodayFollowupLeads.Rows[i]["Lname"]);
                            vlm.LeadOwner = TodayFollowupLeads.Rows[i]["LeadOwner"] == DBNull.Value ? String.Empty : Convert.ToString(TodayFollowupLeads.Rows[i]["LeadOwner"]);
                            vlm.AssignTo = TodayFollowupLeads.Rows[i]["AssignTo"] == DBNull.Value ? 0 : Convert.ToInt32(TodayFollowupLeads.Rows[i]["AssignTo"]);
                            vlmList.Add(vlm);
                        }
                        viewleadsList = vlmList.Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Not Interested" && em.LeadStatus != "Delivered").ToList();

                        List<APIViewLeadModel> assignList = viewleadsList.Where(em => em.AssignTo == LoginID).ToList();
                        String LoginUserId = Convert.ToString(LoginID);
                        viewleadsList = viewleadsList.Where(em => em.LeadOwner == LoginUserId).ToList();
                        if (assignList.Count > 0) 
                        {
                            viewleadsList.AddRange(assignList);
                        }
                    }
                    else
                    {
                        Records = "** Today No followup Leads Assigned **";
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
                    return Request.CreateResponse(HttpStatusCode.OK, viewleadsList);
                }
            }
        }
    }
}
