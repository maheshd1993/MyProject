using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Svam.EF;
using Svam._Class;
using System.Data;
using Traders.Models;

namespace Svam.Controllers.Services
{
    public class getLeadsController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        // GET api/getLeads/?id=17&status=Open&fromDate=09/01/2017&toDate=09/21/2017&filterByType=ModifiedDate&byUser=50&Page=1
        public HttpResponseMessage Get(string id, string status, string fromDate, string toDate, string filterByType, string byUser,int? page)
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            crm_usertbl u = new crm_usertbl();
            UserDetailModel ug = new UserDetailModel();
            int UID = Convert.ToInt32(id);
            u = db.crm_usertbl.Where(em => em.Id == UID).SingleOrDefault();
            if (u == null)
            {
                var message = string.Format("Empty User-ID");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                #region Cal- DateTime.......
                var dd = System.DateTime.Now;
                DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = monthStartDate.ToString("MM/dd/yyyy");
                var MEndDate = MonthendDate.ToString("MM/dd/yyyy");
                var FilterType = Convert.ToString(filterByType);
                #region To-CheckFilter-Date
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    MStartDate = fromDate;
                    MEndDate = toDate;
                }
                #endregion
                #endregion
                DashBoardLeadsModel DBLM = new DashBoardLeadsModel();
                DashBoardData obj = new DashBoardData();
                if (u.ProfileName == "SuperAdmin")
                {
                    #region Super-Admin                   
                    UID = 0;                    
                    #endregion
                }
                else
                {
                    #region Get-Today-FollowUp-Leads By Other User
                    //DataTable GetRecords = DataAccessLayer.GetDataTable(" call T_GetViewLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "')");                  

                    #endregion
                }
                if (FilterType == "ModifiedDate")
                {
                    #region Modified-Date
                    //var GetRecords = db.P_ViewModifiedLeadbyDate(UID, MStartDate, MEndDate).ToList();    //New
                    DataTable P_ViewModifiedLeadbyDate = DataAccessLayer.GetDataTable(" call CRM_ViewModifiedLeadbyDate('" + UID + "','" + MStartDate + "','" + MEndDate + "')");
                    if (P_ViewModifiedLeadbyDate.Rows.Count > 0)
                    {
                        VLM.viewleadsList = (from dr in P_ViewModifiedLeadbyDate.AsEnumerable()
                                             select new ViewLeadsModel()
                                             {
                                                 Id = Convert.ToInt32(dr["ID"]),
                                                 LeadName = Convert.ToString(dr["Customer"]),
                                                 Mob = Convert.ToString(dr["MobileNo"]),
                                                 EMail = Convert.ToString(dr["EmailId"]),
                                                 Country = Convert.ToString(dr["Country"]),
                                                 State = Convert.ToString(dr["State"]),
                                                 City = Convert.ToString(dr["City"]),
                                                 FollowupDate = Convert.ToString(dr["FollowDate"]),
                                                 Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                                 LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                 AssinedTo = Convert.ToString(dr["AssinedTo"]),
                                                 AssignBy = Convert.ToString(dr["AssinedBy"]),
                                                 Address = Convert.ToString(dr["Address"]),
                                                 LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                 AssignTo = Convert.ToString(dr["AssignTo"]),
                                                 AssignedBy = Convert.ToString(dr["AssignedBy"]),
                                                 AssignDate = Convert.ToString(dr["AssignedDate"]),
                                             }).ToList();

                    }
                    #endregion
                }
                else if (FilterType == "FollowupDate")
                {
                    #region Followup-Date
                    DataTable P_ViewFollowUpLeadbyDate = DataAccessLayer.GetDataTable(" call CRM_ViewFollowUpLeadbyDate('" + UID + "','" + MStartDate + "','" + MEndDate + "')");
                    //var GetRecords = db.P_ViewFollowUpLeadbyDate(UID, MStartDate, MEndDate).ToList();    //New
                    if (P_ViewFollowUpLeadbyDate.Rows.Count > 0)
                    {
                        VLM.viewleadsList = (from dr in P_ViewFollowUpLeadbyDate.AsEnumerable()
                                             select new ViewLeadsModel()
                                             {
                                                 Id = Convert.ToInt32(dr["ID"]),
                                                 LeadName = Convert.ToString(dr["Customer"]),
                                                 Mob = Convert.ToString(dr["MobileNo"]),
                                                 EMail = Convert.ToString(dr["EmailId"]),
                                                 Country = Convert.ToString(dr["Country"]),
                                                 State = Convert.ToString(dr["State"]),
                                                 City = Convert.ToString(dr["City"]),
                                                 FollowupDate = Convert.ToString(dr["FollowDate"]),
                                                 Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                                 LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                 AssinedTo = Convert.ToString(dr["AssinedTo"]),
                                                 AssignBy = Convert.ToString(dr["AssinedBy"]),
                                                 Address = Convert.ToString(dr["Address"]),
                                                 LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                 AssignTo = Convert.ToString(dr["AssignTo"]),
                                                 AssignedBy = Convert.ToString(dr["AssignedBy"]),
                                                 AssignDate = Convert.ToString(dr["AssignedDate"]),
                                             }).ToList();

                    }
                    #endregion
                }
                else
                {
                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "')");
                    if (GetRecords.Rows.Count > 0)
                    {
                        VLM.viewleadsList = (from dr in GetRecords.AsEnumerable()
                                             select new ViewLeadsModel()
                                             {
                                                 Id = Convert.ToInt32(dr["ID"]),
                                                 LeadName = Convert.ToString(dr["Customer"]),
                                                 Mob = Convert.ToString(dr["MobileNo"]),
                                                 EMail = Convert.ToString(dr["EmailId"]),
                                                 Country = Convert.ToString(dr["Country"]),
                                                 State = Convert.ToString(dr["State"]),
                                                 City = Convert.ToString(dr["City"]),
                                                 FollowupDate = Convert.ToString(dr["FollowDate"]),
                                                 Created_By = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"]),
                                                 LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                 AssinedTo = Convert.ToString(dr["AssinedTo"]),
                                                 AssignBy = Convert.ToString(dr["AssinedBy"]),
                                                 Address = Convert.ToString(dr["Address"]),
                                                 LeadOwner = Convert.ToString(dr["LeadOwner"]),
                                                 AssignDate = Convert.ToString(dr["AssignedDate"]),
                                             }).ToList();

                    }
                }
                if (!string.IsNullOrEmpty(status))
                {
                    VLM.viewleadsList = VLM.viewleadsList.Where(em => em.LeadStatus.ToLower() == status.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(byUser) && byUser != "0")
                {
                    var FilterData = VLM.viewleadsList.Where(em => (em.LeadOwner == byUser && (em.AssignTo == null || em.AssignTo != byUser)) || em.AssignTo == byUser).ToList();
                    VLM.viewleadsList = FilterData;
                }
                #region Pagging-Start
                int pageNumber = 1;
                int pageSize = 200;
                int pages = 0;
                if (page == null)
                {
                    pageNumber = 1;
                }
                else
                {
                    pageNumber = Convert.ToInt32(page);
                }
                int TotalProducts = 0;
                int Rem = 0;
                int pageSkip = (pageNumber - 1) * pageSize;

                TotalProducts = VLM.viewleadsList.Count();
                pages = (TotalProducts / pageSize);
                var Product = VLM.viewleadsList.Skip(pageSkip).Take(pageSize).ToList();
                VLM.viewleadsList = Product;
                pages = (TotalProducts / pageSize);
                Rem = (TotalProducts % pageSize);
                if (Rem < pageSize && Rem != 0)
                {
                    pages = (pages + 1);
                }
               // ViewData["NoOfPages"] = pages;

                //For Page Index Count.......
                if (page > 1)
                {
                    var DeclareIndex = (pageSize * (page - 1)) + 1;
                    //ViewData["DeclareIndex"] = DeclareIndex;
                }
                else
                {
                   // ViewData["DeclareIndex"] = 1;
                }
                #endregion
                if (VLM.viewleadsList.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, VLM.viewleadsList);
                }
                else
                {
                    var message = string.Format("No Record Found!");
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }                
            }
        }
    }
}