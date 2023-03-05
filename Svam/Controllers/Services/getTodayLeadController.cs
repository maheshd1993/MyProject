﻿using System;
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
    public class getTodayLeadController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        // GET api/getTodayLead/17
        public HttpResponseMessage Get(string id)
        {
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
                var CurrentDate = System.DateTime.Now;
                DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                var TodayDate = CurrentDate.ToString("MM/dd/yyyy");
                var MStartDate = monthStartDate.ToString("MM/dd/yyyy");
                var MEndDate = MonthendDate.ToString("MM/dd/yyyy");
                #endregion
                DashBoardLeadsModel DBLM = new DashBoardLeadsModel();
                DashBoardData obj = new DashBoardData();
                if (u.ProfileName == "SuperAdmin")
                {
                    #region Get-Today-FollowUp-Leads By Admin
                    DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable(" call CRM_TodayLeads()");
                    if (TodayFollowupLeads.Rows.Count > 0)
                    {
                        obj.dashboardDataList = (from dr in TodayFollowupLeads.AsEnumerable()
                                                 select new DashBoardLeadsApiModel()
                                                                {

                                                                    Id = Convert.ToInt32(dr["Id"]),
                                                                    LeadName = Convert.ToString(dr["Customer"]),
                                                                    Phone = Convert.ToString(dr["MobileNo"]),
                                                                    Email = Convert.ToString(dr["EmailId"]),
                                                                    Country = Convert.ToString(dr["Country"]),
                                                                    CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                    FollowUpDate = Convert.ToString(dr["FollowDate"]),
                                                                    LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                                    LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
                                                                    AssignTo = Convert.ToInt32((Convert.ToString(dr["AssignTo"]) == null || Convert.ToString(dr["AssignTo"]) == "") ? 0 : dr["AssignTo"]),
                                                                    AssignedBy = Convert.ToString(dr["AssinedBy"])
                                                                }).Where(em => em.LeadStatus != "Closed" && em.LeadStatus != "Delivered").ToList();
                    }

                    #endregion
                }
                else
                {
                    #region Get-Today-FollowUp-Leads By Other User
                    DataTable TodayFollowupLeads = DataAccessLayer.GetDataTable(" call CRM_TodayLeads()");
                    if (TodayFollowupLeads.Rows.Count > 0)
                    {
                        obj.dashboardDataList = (from dr in TodayFollowupLeads.AsEnumerable()
                                                 select new DashBoardLeadsApiModel()
                                                                {

                                                                    Id = Convert.ToInt32(dr["Id"]),
                                                                    LeadName = Convert.ToString(dr["Customer"]),
                                                                    Phone = Convert.ToString(dr["MobileNo"]),
                                                                    Email = Convert.ToString(dr["EmailId"]),
                                                                    Country = Convert.ToString(dr["Country"]),
                                                                    CreatedBy = Convert.ToString(dr["Fname"] + " " + dr["Lname"]),
                                                                    FollowUpDate = Convert.ToString(dr["FollowDate"]),
                                                                    LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                                    LeadOwner = Convert.ToInt32(dr["LeadOwner"]),
                                                                    AssignTo = Convert.ToInt32((Convert.ToString(dr["AssignTo"]) == null || Convert.ToString(dr["AssignTo"]) == "") ? 0 : dr["AssignTo"]),
                                                                    AssignedBy = Convert.ToString(dr["AssinedBy"])
                                                                }).Where(em => (em.LeadOwner == UID && em.AssignTo == 0) || em.AssignTo == UID && em.LeadStatus != "Closed" && em.LeadStatus != "Delivered").ToList();
                    }

                    #endregion
                }
                if (obj.dashboardDataList.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, obj.dashboardDataList);
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