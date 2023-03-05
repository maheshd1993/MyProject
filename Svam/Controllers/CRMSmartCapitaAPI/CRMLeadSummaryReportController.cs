using Svam._Class;
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
    public class CRMLeadSummaryReportController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get Lead Summary Report Count
        /// <summary>
        /// GET:api/CRMLeadSummaryReport
        /// </summary>
        /// <param name="LoginID"></param>
        /// <param name="ProfileName"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SearchUserID"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(Int32? LeadOwnerID, int? AssignedUserID, string ProfileName, string CompanyID, string BranchID, string Token)
        {
            APILeadSummaryModel APILeadCount = new APILeadSummaryModel();
            string ErrorMessage = string.Empty;
            try
            {
                Int32  companyID = Convert.ToInt32(CompanyID);
                Int32 branchID = Convert.ToInt32(BranchID);
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
               
                var dd = Constant.GetimeForApi(companyID);
                //DateTime MonthStartDate = new DateTime(dd.Year, dd.Month, 1);
                //DateTime MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);

                DateTime MonthStartDate = dd.AddDays(-15);//get previous 15 days
                DateTime MonthEndDate = new DateTime(dd.Year, dd.Month, dd.Day);//get current date

                var MStartDate = MonthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthEndDate.ToString("dd/MM/yyyy");

                var UID = 0;
                if (!string.IsNullOrEmpty(ProfileName) && ProfileName.Trim('"') == "SuperAdmin")
                {
                    UID = 0;
                    if (AssignedUserID != null && AssignedUserID != 0)
                    {
                        UID = Convert.ToInt32(AssignedUserID);
                    }
                }
                else
                {
                    if (AssignedUserID != null && AssignedUserID != 0)
                    {
                        UID = Convert.ToInt32(AssignedUserID);
                    }
                    else
                    {
                        UID = Convert.ToInt32(LeadOwnerID);
                    }
                }


                #region arun Code
                var LeadRecords = new List<ViewLeadsModel>();

                #region New-Lead
                DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetNewLeadSummaryReport('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                var GetLeadsRecords = (from dr in GetRecords.AsEnumerable()
                                       select new ViewLeadsModel()
                                       {
                                           Id = Convert.ToInt32(dr["ID"]),
                                           LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                           AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                           AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                           AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                           LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                           AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                           ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                           LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                           LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                       }).ToList();

                if (GetLeadsRecords.Count > 0)
                {
                    LeadRecords.AddRange(GetLeadsRecords);
                }
                #endregion

                #region Followup-Missed-Delayed lead

                DataTable GetFMDRecords = DataAccessLayer.GetDataTable("call CRM_GetFollowupMissedDelayedLead('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                var GetFMDLeadsRecords = (from dr in GetFMDRecords.AsEnumerable()
                                          select new ViewLeadsModel()
                                          {
                                              Id = Convert.ToInt32(dr["ID"]),
                                              LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                              AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                              AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                              AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                              LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                              AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                              ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                              LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                              LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                          }).ToList();


                if (GetFMDLeadsRecords.Count > 0)
                {
                    LeadRecords.AddRange(GetFMDLeadsRecords);
                }

                DataTable GetFUpRecords = DataAccessLayer.GetDataTable("call CRM_GetLSReportByFollowupDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetFUpRecords.Rows.Count > 0)
                {
                    var GetFUPLeadsRecords = (from dr in GetFUpRecords.AsEnumerable()
                                              select new ViewLeadsModel()
                                              {
                                                  Id = Convert.ToInt32(dr["ID"]),
                                                  LeadStatus = dr["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatus"]),
                                                  AssignDate = dr["AssignedDate"] == DBNull.Value ? "" : Convert.ToString(dr["AssignedDate"]),
                                                  AssinedTo = dr["AssignedTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedTo"]),
                                                  AssignBy = dr["AssignedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedBy"]),
                                                  LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                  AssignTo = dr["AssignTo"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignTo"]),
                                                  ModifiedDate = dr["ModifiedDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifiedDate"]),
                                                  LeadDate = Convert.ToDateTime(dr["LeadDate"]),
                                                  LeadStatusName = dr["LeadStatusName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LeadStatusName"])
                                              }).ToList();



                    var FUPLead = GetFUPLeadsRecords.Where(x => !LeadRecords.Any(y => y.Id == x.Id));
                    LeadRecords.AddRange(FUPLead);

                }
                #endregion

                int followup = LeadRecords.Where(a => a.LeadStatusName.Equals("Follow")).ToList().Count();
                APILeadCount.FollowUpCount = followup;

                int Missedfollowup = LeadRecords.Where(a => a.LeadStatusName.Equals("Missed")).ToList().Count();
                APILeadCount.MissedFollowUpCount = Missedfollowup;

                int Delayedfollowup = LeadRecords.Where(a => a.LeadStatusName.Equals("Delayed")).ToList().Count();
                APILeadCount.DelayedFollowUpCount = Delayedfollowup;

                #region New-Leads                    
                int NewLeadsRecords = LeadRecords.Where(a => a.LeadStatusName.Equals("NewLead")).ToList().Count();
                APILeadCount.NewLeadCount = NewLeadsRecords;
                #endregion

                #region Not-Interested
                int notinterestCount = LeadRecords.Where(a => a.LeadStatus.Equals("Not Interested")).GroupBy(a => a.Id).ToList().Count();
                APILeadCount.NotInterestedCount = notinterestCount;
                #endregion

                #region Closed
                int ClosedCount = LeadRecords.Where(a => a.LeadStatus.Equals("Closed")).GroupBy(a => a.Id).ToList().Count();
                APILeadCount.ClosedLeadsUpCount = ClosedCount;
                #endregion

                #region Suspect-Leads
                int SuspectCount = LeadRecords.Where(a => a.LeadStatus.Equals("Suspect")).GroupBy(a => a.Id).ToList().Count();
                APILeadCount.SuspectLeadsCount = SuspectCount;
                #endregion

                #region Prospect-Leads                   
                int prospectCount = LeadRecords.Where(a => a.LeadStatus.Equals("Prospect")).GroupBy(a => a.Id).ToList().Count();
                APILeadCount.ProspectLeadsCount = prospectCount;
                #endregion

                #endregion


            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameter **s");
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, APILeadCount);
            }
        }
        #endregion
    }
}