using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers
{
    public class TrackSalePersonController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        public ActionResult TrackSales(int? Page, int? SearchUserID, string FromDate, string ToDate)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            TrackSalePersonModel TModel = new TrackSalePersonModel();
            TModel.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            //var dd = Constant.GetBharatTime().AddDays(-2);
            var dd = Constant.GetBharatTime();
            //var dd1 = Constant.GetBharatTime();
            //var MStartDate = dd.ToString("dd/MM/yyyy");
            //var MEndDate = dd1.ToString("dd/MM/yyyy");

            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = new DateTime(monthStartDate.Year, monthStartDate.Month, dd.Day);

            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

            TModel.FromDate = MStartDate;
            TModel.ToDate = MEndDate;
            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
            {
                //   MStartDate = FromDate;
                //   MEndDate = ToDate;
                //if (TModel.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                //{                  
                //    var fmDate = DateTime.ParseExact(FromDate, TModel.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                //    var tDate = DateTime.ParseExact(ToDate, TModel.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                //    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                //    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure
                //    TModel.FromDate = MStartDate;
                //    TModel.ToDate = MEndDate;

                //}
                //else
                //{   
                TModel.DateFormat = "dd/MM/yyyy";
                MStartDate = FromDate;
                MEndDate = ToDate;

                TModel.FromDate = MStartDate;
                TModel.ToDate = MEndDate;
                //}
            }



            var UID = 0;
            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {
                UID = 0;//&& em.ProfileName == "Sales"
                if (SearchUserID != null)
                {
                    UID = Convert.ToInt32(SearchUserID);
                }
                //var q = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status==true && em.ProfileName.Contains("Sales")).OrderBy(em => em.Fname).ToList();
                string userquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName ,ur.EmployeeCode
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewLeads = 1";
                var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                //List<TrackSalePersonModel> uList = new List<TrackSalePersonModel>();
                TModel.SaleUserList = new List<TrackSalePersonModel>();
                //foreach (var item in q)
                //{
                //    TrackSalePersonModel u = new TrackSalePersonModel();
                //    u.UserID = item.UserID;
                //    u.UserName = item.UserName;
                //    uList.Add(u);
                //}
                //TModel.SaleUserList = uList;

                if (q != null && q.Count > 0)//add all option if user list greater 0
                {
                    TModel.SaleUserList.Add(new TrackSalePersonModel
                    {
                        UserID = 0,
                        UserName = "All"
                    });

                    var user = (from item in q
                                select new TrackSalePersonModel
                                {
                                    UserID = Convert.ToInt32(item.UserID),
                                    UserName = item.UserName + "    (" + item.EmployeeCode + ")",
                                }).ToList();
                    TModel.SaleUserList.AddRange(user);//added users list
                    TModel.SaleUserList = TModel.SaleUserList.OrderBy(a => a.UserName).ToList();//set users list alphabetical order
                }
            }
            else
            {
                int loggedUID = Convert.ToInt32(Session["UID"]);
                if (SearchUserID != null)
                {
                    UID = Convert.ToInt32(SearchUserID);
                }
                else
                {
                    UID = Convert.ToInt32(Session["UID"]);
                }
                var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetUserData != null && !string.IsNullOrEmpty(GetUserData.MappedUsers))
                {
                    int Count = 0;
                    //var GetMapUser = GetUserData.MappedUsers.Split(',');
                    //List<TrackSalePersonModel> uList = new List<TrackSalePersonModel>();
                    //foreach (var item in GetMapUser)
                    //{
                    //    var mapid = Convert.ToInt32(item);
                    //    var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID && em.ProfileName == "Sales" && em.Status == true).FirstOrDefault();
                    //    if (GetMapUserData != null)
                    //    {
                    //        TrackSalePersonModel u = new TrackSalePersonModel();
                    //        u.UserID = mapid;
                    //        u.UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname;
                    //        uList.Add(u);
                    //        Count++;
                    //    }                       
                    //}
                    //TModel.SaleUserList = uList;
                    //TModel.CountMapUser = Count;

                    TModel.SaleUserList = new List<TrackSalePersonModel>();
                    var SpliteUserIds = GetUserData.MappedUsers.Split(',');
                    int[] UserIds = SpliteUserIds.Select(int.Parse).ToArray();
                    TModel.CountMapUser = UserIds.Count();
                    TModel.SaleUserList.Add(new TrackSalePersonModel
                    {
                        UserID = 0,
                        UserName = "All"
                    });

                    TModel.SaleUserList.Add(new TrackSalePersonModel
                    {
                        UserID = loggedUID,
                        UserName = Session["UserName"].ToString()
                    });


                    var mappedUser = (from uIds in UserIds
                                      join item in db.crm_usertbl on uIds equals item.Id
                                      where item.Status == true && item.BranchID == BranchID && item.CompanyID == CompanyID
                                      select new TrackSalePersonModel
                                      {
                                          UserID = item.Id,
                                          UserName = item.Fname + " " + item.Lname
                                      }).ToList();

                    TModel.SaleUserList.AddRange(mappedUser);
                    TModel.SaleUserList = TModel.SaleUserList.OrderBy(a => a.UserName).ToList();
                }
            }
            //if (SearchUserID != null)
            //{
            //    UID = Convert.ToInt32(SearchUserID);
            //}
            //else
            //{
            //    if(Convert.ToString(Session["UserType"]) == "SuperAdmin")
            //    {
            //        UID = 0;
            //    }
            //    else
            //    {
            //        UID = Convert.ToInt32(Session["UID"]);
            //    }
            //}
            DataTable GetTrackSalePersons = DataAccessLayer.GetDataTable("call CRM_GetSalePersonTrack(" + BranchID + "," + CompanyID + "," + UID + ",'" + MStartDate + "','" + MEndDate + "')");
            if (GetTrackSalePersons.Rows.Count > 0)
            {
                List<TrackSalePersonModel> TSMList = new List<TrackSalePersonModel>();
                for (int i = 0; i < GetTrackSalePersons.Rows.Count; i++)
                {
                    TrackSalePersonModel TSM = new TrackSalePersonModel();
                    TSM.TrackID = Convert.ToInt64(GetTrackSalePersons.Rows[i]["TrackID"]);
                    TSM.UserID = Convert.ToInt32(GetTrackSalePersons.Rows[i]["UserID"]);
                    TSM.UserName = Convert.ToString(GetTrackSalePersons.Rows[i]["UserName"]);
                    TSM.Address = Convert.ToString(GetTrackSalePersons.Rows[i]["Address"]);
                    TSM.Country = Convert.ToString(GetTrackSalePersons.Rows[i]["Country"]);
                    TSM.StateName = Convert.ToString(GetTrackSalePersons.Rows[i]["StateName"]);
                    TSM.CityName = Convert.ToString(GetTrackSalePersons.Rows[i]["CityName"]);
                    TSM.Latitude = Convert.ToString(GetTrackSalePersons.Rows[i]["Latitude"]);
                    TSM.Longitude = Convert.ToString(GetTrackSalePersons.Rows[i]["Longitude"]);
                    TSM.TrackDatetime = Convert.ToDateTime(GetTrackSalePersons.Rows[i]["TrackDatetime"]);
                    TSMList.Add(TSM);
                }

                TModel.TrackSalePersonList = TSMList;
                if (Convert.ToString(Session["UserType"]) == "SuperAdmin" && SearchUserID == null)
                {
                    TModel.TrackSalePersonList = null;
                    TModel.TrackSalePersonList = new List<TrackSalePersonModel>();
                    if (TModel.SaleUserList != null && TModel.SaleUserList.Count > 0)
                    {
                        for (int i1 = 0; i1 < TModel.SaleUserList.Count; i1++)
                        {
                            int useId = Convert.ToInt32(TModel.SaleUserList[i1].UserID);
                            if (useId != 0)
                            {
                                List<TrackSalePersonModel> VlieadList = TSMList.Where(em => em.UserID == useId).ToList();
                                TModel.TrackSalePersonList.AddRange(VlieadList);
                            }
                        }
                    }
                }
                else if (TModel.SaleUserList != null && TModel.SaleUserList.Count > 0 && SearchUserID != null && SearchUserID != Convert.ToInt32(Session["UID"]))
                {
                    TModel.TrackSalePersonList = null;
                    TModel.TrackSalePersonList = new List<TrackSalePersonModel>();
                    if (TModel.SaleUserList != null && TModel.SaleUserList.Count > 0)
                    {
                        for (int i1 = 0; i1 < TModel.SaleUserList.Count; i1++)
                        {
                            int useId = Convert.ToInt32(TModel.SaleUserList[i1].UserID);
                            if (useId != 0)
                            {
                                List<TrackSalePersonModel> VlieadList = TSMList.Where(em => em.UserID == useId).ToList();
                                TModel.TrackSalePersonList.AddRange(VlieadList);
                            }
                        }
                    }
                }
            }
            return View(TModel);
        }

        public ActionResult ViewTrackMap(Int32? SearchUserID)
        {

            TrackSalePersonModel TModel = new TrackSalePersonModel();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            TModel.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            var UID = 0;
            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {
                UID = 0;//&& em.ProfileName == "Sales"
                //var q = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status==true && em.ProfileName.Contains("Sales")).OrderBy(em => em.Fname).ToList();
                string userquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName,ur.EmployeeCode 
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewLeads = 1";
                var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                List<TrackSalePersonModel> uList = new List<TrackSalePersonModel>();
                foreach (var item in q)
                {
                    TrackSalePersonModel u = new TrackSalePersonModel();
                    u.UserID = item.UserID;
                    u.UserName = item.UserName + "    (" + item.EmployeeCode + ")";
                    uList.Add(u);
                }
                TModel.SaleUserList = uList;
            }
            else
            {
                var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetUserData != null && GetUserData.MappedUsers != null)
                {
                    int Count = 0;
                    var GetMapUser = GetUserData.MappedUsers.Split(',');
                    List<TrackSalePersonModel> uList = new List<TrackSalePersonModel>();
                    foreach (var item in GetMapUser)
                    {
                        var mapid = Convert.ToInt32(item);
                        var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == BranchID && em.CompanyID == CompanyID && em.ProfileName == "Sales" && em.Status == true).FirstOrDefault();
                        if (GetMapUserData != null)
                        {
                            TrackSalePersonModel u = new TrackSalePersonModel();
                            u.UserID = mapid;
                            u.UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname;
                            uList.Add(u);
                            Count++;
                        }
                    }
                    TModel.SaleUserList = uList;
                    TModel.CountMapUser = Count;
                }
            }
            return View(TModel);
        }

        public ActionResult ShowMap(Int32? SearchUserID, string SelectDate)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            var UID = Convert.ToInt32(SearchUserID);
            var dd = Constant.GetBharatTime();
            var MStartDate = dd.ToString("dd/MM/yyyy");
            var MEndDate = dd.ToString("dd/MM/yyyy");
            if (!string.IsNullOrWhiteSpace(SelectDate))
            {
                //MStartDate = SelectDate;
                //MEndDate = SelectDate;

                if (DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    var fmDate = DateTime.ParseExact(SelectDate, DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(SelectDate);
                    var tDate = DateTime.ParseExact(SelectDate, DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(SelectDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                }
                else
                {
                    MStartDate = SelectDate;
                    MEndDate = SelectDate;
                }
            }

            List<TrackSalePersonModel> TSMList = new List<TrackSalePersonModel>();
            DataTable GetTrackSalePersons = DataAccessLayer.GetDataTable("call CRM_GetSalePersonTrack(" + BranchID + "," + CompanyID + "," + UID + ",'" + MStartDate + "','" + MEndDate + "')");
            if (GetTrackSalePersons.Rows.Count > 0)
            {

                for (int i = 0; i < GetTrackSalePersons.Rows.Count; i++)
                {
                    TrackSalePersonModel TSM = new TrackSalePersonModel();
                    TSM.lat = Convert.ToString(GetTrackSalePersons.Rows[i]["Latitude"]);
                    TSM.lng = Convert.ToString(GetTrackSalePersons.Rows[i]["Longitude"]);
                    TSM.description = Convert.ToString(GetTrackSalePersons.Rows[i]["Address"]);
                    TSMList.Add(TSM);
                }
            }
            return Json(TSMList, JsonRequestBehavior.AllowGet);
        }
    }
}
