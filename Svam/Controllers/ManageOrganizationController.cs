using Svam.EF;
using Svam.Models;
using Svam.Models.ViewModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers
{
    public class ManageOrganizationController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        public ActionResult UserListByBranch( string FromDate, string ToDate, int sBranchId = 0, string Term = "") 
        {
            if (Session["UID"] != null && Convert.ToString(Session["UserType"]) == "SuperAdmin" && Convert.ToInt32(Session["BranchID"]) == 173 && Convert.ToInt32(Session["CompanyID"]) == 296)
            {
                //Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                //Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                //var UserProfileName = Convert.ToString(Session["UserType"]);
                //if (string.IsNullOrEmpty(Term) && TempData["FltrTerm"] != null)
                //{
                //    Term = Convert.ToString(TempData["FltrTerm"]);
                //}
                //if (string.IsNullOrEmpty(FromDate) && TempData["FltrFrmDt"] != null)
                //{
                //    FromDate = Convert.ToString(TempData["FltrFrmDt"]);
                //}
                //if (string.IsNullOrEmpty(ToDate) && TempData["FltrToDt"] != null)
                //{
                //    ToDate = Convert.ToString(TempData["FltrToDt"]);
                //}

                var model = new ManageOrganizationVM();

                #region Data-time-Formate

                //DateTime bharatTime = Constant.GetBharatTime();//get india datetime
                //var dd = bharatTime;                
                //DateTime monthStartDate = dd.AddDays(-15);
                //DateTime MonthendDate = new DateTime(dd.Year, dd.Month, dd.Day);

                //var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                //var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
                #region To-CheckFilter-Date
                //if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                //{
                //    MStartDate = FromDate;
                //    MEndDate = ToDate;
                //    model.FromDate = FromDate;
                //    model.ToDate = ToDate;
                //}
                //else
                //{
                //    model.FromDate = MStartDate;
                //    model.ToDate = MEndDate;
                //}
                #endregion

                #endregion

                #region Organization List between dates
                //DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetCompanyBranchList('" + Term + "','" + MStartDate + "','" + MEndDate + "')");
                //var GetOrganization = (from dr in GetRecords.AsEnumerable()
                //                       select new ManageOrganizationVM()
                //                       {
                //                           Id = Convert.ToInt32(dr["Id"]),
                //                           CompanyId = Convert.ToInt32(dr["CompanyId"]),
                //                           BranchName = Convert.ToString(dr["BranchName"]),
                //                           Country = Convert.ToString(dr["Country"]),
                //                           State = Convert.ToString(dr["State"]),
                //                           City = Convert.ToString(dr["City"]),
                //                           EmailID = Convert.ToString(dr["EmailID"]),
                //                           MobileNo = Convert.ToString(dr["MobileNo"]),
                //                           CreateDate = Convert.ToString(dr["CreateDate"])
                //                       }).ToList();

                //if (GetOrganization.Count > 0)
                //{
                //    model.TotalRecord = GetOrganization.Count;
                //    model.OrganizationList = GetOrganization;

                //}

                #endregion

                if(sBranchId>0)
                {
                    DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetSalesUsersByBranchId("+sBranchId+")");
                    var GetOrganization = (from dr in GetRecords.AsEnumerable()
                                           select new ManageOrganizationVM()
                                           {
                                               Id = Convert.ToInt32(dr["Id"]),
                                               UserName = Convert.ToString(dr["UserName"]),
                                               BranchName = Convert.ToString(dr["BranchName"]),
                                               EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                               ProfileName = Convert.ToString(dr["ProfileName"]),                                              
                                               EmailID = Convert.ToString(dr["EmailID"]),                                              
                                           }).ToList();

                    if (GetOrganization.Count > 0)
                    {
                        model.TotalRecord = GetOrganization.Count;
                        model.UserList = GetOrganization;

                    }
                }

                #region AssignedToList
                DataTable Getasrecord = DataAccessLayer.GetDataTable("call CRM_GetAssignTOBranchList()");
                var GetBranchList = (from dr in Getasrecord.AsEnumerable()
                                     select new AssignToBranchModel()
                                     {
                                         Id = Convert.ToInt32(dr["Id"]),
                                         BranchName = Convert.ToString(dr["BranchName"]),
                                     }).ToList();

                if (GetBranchList.Count > 0)
                {
                    model.AssigntoList = GetBranchList;
                }
                #endregion

                //model.Term = Term;
                //TempData["sDate"] = FromDate;
                //TempData["eDate"] = ToDate;
                //TempData["TxtTerm"] = Term;
                return View(model);
            }
            else
            {
                return Redirect("/home/login");
            }

        }

        [HttpPost]
        public async Task<ActionResult> AssignedOrganization(int[] SelectedBranch, int AssignedId)
        {
            if (Session["UID"] != null && Convert.ToString(Session["UserType"]) == "SuperAdmin" && Convert.ToInt32(Session["BranchID"]) == 173 && Convert.ToInt32(Session["CompanyID"]) == 296)
            {
                //Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                //Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                //var UserProfileName = Convert.ToString(Session["UserType"]);
                var uid = Convert.ToInt32(Session["UID"]);
                try
                {
                    if (SelectedBranch.Count() > 0)
                    {
                        var ato = new crm_assignedtootherorganization();
                        foreach (var item in SelectedBranch)
                        {
                            if (db.crm_assignedtootherorganization.Any(a => a.AssignToCompanyID == AssignedId))
                            {
                                if(!db.crm_assignedtootherorganization.Any(a => a.AssignedUserID == item))
                                {
                                    ato.AssignToCompanyID = AssignedId;
                                    ato.AssignedUserID = item;
                                    ato.AssignedBy = uid;
                                    ato.AssignedDate = Constant.GetBharatTime();
                                    ato.IsActive = true;
                                    db.crm_assignedtootherorganization.Add(ato);
                                    await db.SaveChangesAsync();

                                }
                            }
                            else
                            {
                                ato.AssignToCompanyID = AssignedId;
                                ato.AssignedUserID = item;
                                ato.AssignedBy = uid;
                                ato.AssignedDate = Constant.GetBharatTime();
                                ato.IsActive = true;
                                db.crm_assignedtootherorganization.Add(ato);
                                await db.SaveChangesAsync();

                            }
                        }
                        
                        TempData["success"] = "User assigned successfully ";
                        //if (SelectedBranch.Any(a => a.Equals(AssignedId)))
                        //{
                        //    TempData["alert"] = "Selected branch and assigned to branch are not assigned to same branch";
                        //}
                        //else
                        //{
                        //    var ato = new crm_assignedtootherorganization();
                        //    foreach (var item in SelectedBranch)
                        //    {
                        //        if (db.crm_assignedtootherorganization.Any(a => a.AssignToCompanyID == AssignedId && a.CompanyBranchID!=item))
                        //        {
                        //            ato.AssignToCompanyID = AssignedId;
                        //            ato.CompanyBranchID = item;
                        //            ato.AssignedBy = uid;
                        //            ato.AssignedDate = Constant.GetBharatTime();
                        //            ato.IsActive = true;
                        //        }
                        //    db.crm_assignedtootherorganization.Add(ato);
                        //    }
                        //    await db.SaveChangesAsync();
                        //    TempData["success"] = "User assigned successfully ";
                        //}
                    }
                    else
                    {
                        TempData["alert"] = "Please select user to assign";

                    }
                }
                catch(Exception ex)
                {
                    ExceptionLogging.SendExcepToDB(ex);
                    ex.Message.ToString();
                    TempData["alert"] = "Some technical error occured, please try again";

                }
               
                TempData["FltrFrmDt"]= TempData["sDate"];
                TempData["FltrToDt"] = TempData["eDate"];
                TempData["FltrTerm"] = TempData["TxtTerm"];

                return RedirectToAction("UserListByBranch");
            }
            else
            {
                return Redirect("/home/login");
            }

        }

        public ActionResult ViewAssinedBranch(int AssignedId=0,string FromDate="", string ToDate="") 
        {
            if (Session["UID"] != null && Convert.ToString(Session["UserType"]) == "SuperAdmin" && Convert.ToInt32(Session["BranchID"]) == 173 && Convert.ToInt32(Session["CompanyID"]) == 296)
            {
                //Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                //Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                //var UserProfileName = Convert.ToString(Session["UserType"]);
                

                var model = new ManageOrganizationVM();


                model.FromDate = FromDate;
                model.ToDate = ToDate;
                model.AssignedId = AssignedId;



                #region Organization List between dates
                DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewAssignedOtherBranchUsers('" + AssignedId + "','" + FromDate + "','" + ToDate + "')");
                var GetOrganization = (from dr in GetRecords.AsEnumerable()
                                       select new ManageOrganizationVM()
                                       {
                                           Id = Convert.ToInt32(dr["Id"]),
                                           AssignToCompanyID= Convert.ToInt32(dr["AssignToCompanyID"]),
                                           AssignedUserID = Convert.ToInt32(dr["AssignedUserID"]),
                                           BranchName = Convert.ToString(dr["BranchName"]),
                                           AssignedBranchName= Convert.ToString(dr["AssignedBranchName"]),
                                           //Country = Convert.ToString(dr["Country"]),
                                           //State = Convert.ToString(dr["State"]),
                                           //City = Convert.ToString(dr["City"]),                                          
                                           //MobileNo = Convert.ToString(dr["MobileNo"]),
                                           AssignedDate = Convert.ToString(dr["AssignedDate"]),
                                           UserName = Convert.ToString(dr["UserName"]),                                           
                                           EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                           ProfileName = Convert.ToString(dr["ProfileName"]),
                                           EmailID = Convert.ToString(dr["EmailID"]),
                                           IsActive=Convert.ToBoolean(dr["IsActive"])
                                       }).ToList();

                if (GetOrganization.Count > 0)
                {
                    model.TotalRecord = GetOrganization.Count;
                    model.UserList = GetOrganization;

                }

                #endregion

                #region AssignedToList
                DataTable Getasrecord = DataAccessLayer.GetDataTable("call CRM_GetAssignTOBranchList()");
                var GetBranchList = (from dr in Getasrecord.AsEnumerable()
                                     select new AssignToBranchModel()
                                     {
                                         Id = Convert.ToInt32(dr["Id"]),
                                         BranchName = Convert.ToString(dr["BranchName"]),
                                     }).ToList();

                if (GetBranchList.Count > 0)
                {
                    model.AssigntoList = GetBranchList;
                }
                #endregion

                
                return View(model);
            }
            else
            {
                return Redirect("/home/login");
            }
        }

        public JsonResult ChangeStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_assignedtootherorganization set IsActive=case when IsActive=1 then 0 else 1 end where id="+id);
                msg = "ok";
            }
            catch(Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg,JsonRequestBehavior.AllowGet);
        }
    }
}
