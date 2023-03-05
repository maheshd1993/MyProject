using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Svam.EF;
using Traders.Models;
using Svam.Models;
using Svam.UtilityManager;

namespace Traders.Controllers
{
    public class LeadManagementController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        public ActionResult Index(int? id)
        {
            LeadManagementModel LMM = new LeadManagementModel();
            try
            {
                if (Session["UID"] != null)
                {
                    var uid = Convert.ToString(Session["UID"]);
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    #region Get-MappedUser-Parents
                    DataTable GetData = DataAccessLayer.GetDataTable(" call CRM_GetMapuserParents('" + uid + "'," + BranchID + "," + CompanyID + ")");
                    if (GetData.Rows.Count > 0)
                    {
                        LMM.mapuserParentsModel = (from dr in GetData.AsEnumerable()
                                                   select new MappedUserParentsModel()
                                                       {
                                                           Id = Convert.ToInt32(dr["Id"]),
                                                           ParentsName = Convert.ToString(dr["Fname"]) + " " + Convert.ToString(dr["Lname"])
                                                       }).ToList();
                    }


                    ViewBag.AssignTo = new SelectList(LMM.mapuserParentsModel, "Id", "ParentsName");
                    #endregion

                    #region Select-TimeZone
                    var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList().Select(em => new crm_zonetbl
                    {
                        ZoneName = em.ZoneName
                    }).AsQueryable();
                    ViewBag.TimeZoneName = new SelectList(GetZoneName, "ZoneName", "ZoneName");
                    #endregion

                    var GetLeadsData = db.crm_createleadstbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).SingleOrDefault();
                    if (GetLeadsData != null)
                    {
                        LMM.PrimaryPhNo = GetLeadsData.MobileNo;
                        LMM.SecondaryPhNo = GetLeadsData.OtherNo;
                        LMM.Email = GetLeadsData.EmailId;
                        LMM.LeadSource = GetLeadsData.LeadResource;
                        LMM.CustomerName = GetLeadsData.Customer;
                        LMM.CompanyName = GetLeadsData.OrganizationName;
                        LMM.Website = GetLeadsData.Url;
                        LMM.SkypeId = GetLeadsData.SkypeId;
                        LMM.Country = GetLeadsData.Country;
                        LMM.Address = GetLeadsData.Address;
                        LMM.FollowUpDate = GetLeadsData.FollowDate.Value.ToShortDateString();
                        LMM.OtherRemark = GetLeadsData.OtherRemarks;
                        LMM.Description = GetLeadsData.Description;
                        ViewBag.AssignTo = new SelectList(LMM.mapuserParentsModel, "Id", "ParentsName", GetLeadsData.AssignTo);
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/LeadManagement";
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(LMM);
        }

        [HttpPost]
        public ActionResult Index(LeadManagementModel LMM,int? id)
        {
            try
            {
                if (Session["UID"] != null)
                { 
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    if (id != null)
                    {
                        var GetLeadData = db.crm_createleadstbl.Where(em => em.Id == id && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetLeadData != null)
                        {
                            GetLeadData.MobileNo = LMM.PrimaryPhNo;
                            GetLeadData.OtherNo = LMM.SecondaryPhNo;
                            GetLeadData.EmailId = LMM.Email;
                            GetLeadData.LeadResource = LMM.LeadSource;
                            GetLeadData.Customer = LMM.CustomerName;
                            GetLeadData.OrganizationName = LMM.CompanyName;
                            GetLeadData.Url = LMM.Website;
                            GetLeadData.SkypeId = LMM.SkypeId;
                            GetLeadData.Country = LMM.Country;
                            GetLeadData.Address = LMM.Address;
                            GetLeadData.FollowDate = Convert.ToDateTime(LMM.FollowUpDate);
                            GetLeadData.OtherRemarks = LMM.OtherRemark;
                            GetLeadData.Description = LMM.Description;
                            GetLeadData.ZoneName = LMM.TimeZoneName;
                            GetLeadData.AssignTo = LMM.AssignTo;
                            GetLeadData.BranchID = BranchID;
                            GetLeadData.CompanyID = CompanyID;
                            int i= db.SaveChanges();

                            //Add new LeadDescription
                            crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                            LD.LeadId = id;
                            LD.Date = System.DateTime.Now.ToString("dd/MM/yyyy");
                            LD.Description = LMM.Description;
                            LD.BranchID = BranchID;
                            LD.CompanyID = CompanyID;
                            db.crm_leaddescriptiontbl.Add(LD);
                            db.SaveChanges();
                            TempData["success"] = "Leads updated successfully";
                        }
                    }
                    else
                    {
                        crm_createleadstbl CLt = new crm_createleadstbl();
                        CLt.LeadOwner = Convert.ToInt32(Session["UID"]);
                        CLt.MobileNo = LMM.PrimaryPhNo;
                        CLt.OtherNo = LMM.SecondaryPhNo;
                        CLt.EmailId = LMM.Email;
                        CLt.LeadResource = LMM.LeadSource;
                        CLt.LeadStatus = "Open";
                        CLt.Customer = LMM.CustomerName;
                        CLt.OrganizationName = LMM.CompanyName;
                        CLt.Url = LMM.Website;
                        CLt.SkypeId = LMM.SkypeId;
                        CLt.Country = LMM.Country;
                        CLt.Address = LMM.Address;
                        CLt.FollowDate = Convert.ToDateTime(LMM.FollowUpDate);
                        CLt.OtherRemarks = LMM.OtherRemark;
                        CLt.Description = LMM.Description;
                        CLt.ZoneName = LMM.TimeZoneName;
                        CLt.AssignTo = LMM.AssignTo;
                        CLt.Status = true;
                        CLt.date = System.DateTime.Now.ToString("dd/MM/yyyy");
                        CLt.Createddate = System.DateTime.Now;
                        CLt.BranchID = BranchID;
                        CLt.CompanyID = CompanyID;
                        db.crm_createleadstbl.Add(CLt);
                        if (db.SaveChanges() > 0)
                        {
                            var lid = CLt.Id;
                            //Add Description To LeadDescription Table....
                            crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                            LD.LeadId = lid;
                            LD.Date = System.DateTime.Now.ToString("dd/MM/yyyy");
                            LD.Description = LMM.Description;
                            LD.BranchID = BranchID;
                            LD.CompanyID = CompanyID;
                            db.crm_leaddescriptiontbl.Add(LD);
                            db.SaveChanges();

                            crm_leadassignhistorytbl LAS = new crm_leadassignhistorytbl();
                            LAS.LeadId = lid;
                            LAS.LeadAssignBy = Convert.ToInt32(Session["UID"]);
                            LAS.LeadAssignTo = Convert.ToInt32(LMM.AssignTo);
                            LAS.LeadAssignDate = System.DateTime.Now.ToString("dd/MM/yyyy");
                            LAS.LeadStatus = "Open";
                            LAS.CreatedDate = System.DateTime.Now;
                            LAS.BranchID = BranchID;
                            LAS.CompanyID = CompanyID;
                            db.crm_leadassignhistorytbl.Add(LAS);
                            db.SaveChanges();
                            TempData["success"] = "Leads created successfully";
                        }
                    }
                }
                else
                {
                    Session["ReturnUrl"] = "/LeadManagement";
                    return Redirect("/LeadManagement");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);  
            }
            return Redirect("/LeadManagement");
        }

        public ActionResult show(string filterText, int? page)
        {
            LeadManagementModel LMM = new LeadManagementModel();
            try
            {
                #region Data-time-Formate
                var dd = Constant.GetBharatTime();
                DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = monthStartDate.ToString("MM/dd/yyyy");
                var MEndDate = MonthendDate.ToString("MM/dd/yyyy");
                #region To-CheckFilter-Date
                if (Session["VLFltrFrmDt"] != null && Session["VLFltrToDt"] != null)
                {
                    MStartDate = Convert.ToString(Session["VLFltrFrmDt"]);
                    MEndDate = Convert.ToString(Session["VLFltrToDt"]);
                }
                #endregion

                #endregion

                int UID = Convert.ToInt32(Session["UID"]);
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate('" + UID + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ")");
                if (GetRecords.Rows.Count > 0)
                {
                    LMM.leadManagementmodelList = (from dr in GetRecords.AsEnumerable()
                                                   select new LeadManagementModel()
                                                    {
                                                        Id = Convert.ToInt64(dr["Id"]),
                                                        CustomerName = Convert.ToString(dr["Customer"]),
                                                        PrimaryPhNo = Convert.ToString(dr["MobileNo"]),
                                                        Country = Convert.ToString(dr["Country"]),
                                                        FollowUpDate = Convert.ToString(dr["FollowDate"]),
                                                        LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                        Created_at = Convert.ToString(dr["date"])
                                                    }).ToList();
                }
                
                if (filterText != null)
                {
                    var FilterData = LMM.leadManagementmodelList.Where(em => em.LeadStatus.Contains(filterText)).ToList();
                    LMM.leadManagementmodelList = FilterData;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(LMM);
        }

        public ActionResult FIlterLeadsByDate(string fromDate,string EndDate)
        {
            LeadManagementModel LMM = new LeadManagementModel();
            try
            {
                if (Session["UID"] != null)
                {
                    int UID = Convert.ToInt32(Session["UID"]);
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadsByDate('" + UID + "','" + fromDate + "','" + EndDate + "'," + BranchID + "," + CompanyID + ")");
                    if (GetRecords.Rows.Count > 0)
                    {
                        LMM.leadManagementmodelList = (from dr in GetRecords.AsEnumerable()
                                                       select new LeadManagementModel()
                                                       {
                                                           Id = Convert.ToInt64(dr["Id"]),
                                                           CustomerName = Convert.ToString(dr["Customer"]),
                                                           PrimaryPhNo = Convert.ToString(dr["MobileNo"]),
                                                           Country = Convert.ToString(dr["Country"]),
                                                           FollowUpDate = Convert.ToString(dr["FollowDate"]),
                                                           LeadStatus = Convert.ToString(dr["LeadStatus"]),
                                                           Created_at = Convert.ToString(dr["date"])
                                                       }).ToList();
                    }
                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_FilterManagedLeadsByDate",LMM);
        }
    }
}
