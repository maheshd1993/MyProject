using Svam.EF;
using Svam.Models;
using Svam.Models.DTO;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Controllers
{
    public class TestController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        public ActionResult Stacked100BarChart()
        {
            return View(/*StackedChartData.GetData()*/);
        }

        public ActionResult TZones() 
        {


            //var list = new List<dynamic>();
            var tzList = new List<TimeZoneDTO>();
            //TimeZoneInfo infos = System.TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            //ReadOnlyCollection<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();

            //foreach (TimeZoneInfo info in zones)
            //{
            //    //var= "time zone id : " + info.Id + " display name : " + info.DisplayName;
            //    list.Add(new{ZoneId= info.Id, ZoneName  = info.DisplayName });              
            //}
            
            //var tzData = db.time_zone.ToList();
            //foreach (var item in tzData)
            //{
            //    var time1 = Constant.AddStdZoneName(item.zone_name);
            //    if (!string.IsNullOrEmpty(time1))
            //    {
            //        //tzList.Add(new TimeZoneDTO { Id = item.id, ZoneName = item.zone_name, Time = "" });

            //        item.standard_zone_name = time1;
            //        db.SaveChanges();
            //    }
            //}
            //var time = Constant.GetTimeZoneInfoForTzdbId("Asia/Bangkok");

            //DateTime dt;
            //TimeZoneInfo tzf;
            //tzf = TimeZoneInfo.FindSystemTimeZoneById(time);
            //dt = TimeZoneInfo.ConvertTime(DateTime.Now, tzf);


            //TempData["SLTime"] = time;
            return View(tzList);

            
        }

        public JsonResult ListOfDates() 
        {
            var dd = System.DateTime.Now;
            DateTime sDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime eDate  = sDate.AddMonths(1).AddDays(-1);

            List<DateTime> list = new List<DateTime> ();

            for (var day = sDate.Date; day.Date <= eDate.Date; day = day.AddDays(1))
            {
                list.Add(day);
            }

                return Json(list,JsonRequestBehavior.AllowGet);
        }

        public ActionResult TC() 
        {
            return View();
        }

        //public ActionResult Lnm()
        //{
        //    string msg = string.Empty;
        //    string query = @"select distinct ur.Id, lrt.BranchID, lrt.CompanyID 
        //                     from crm_leaverequest_tbl lrt
        //                     join crm_usertbl ur on lrt.BranchID=ur.BranchID and lrt.CompanyID=ur.CompanyID
        //                     where ur.ProfileName='SuperAdmin'";

        //    var data = db.Database.SqlQuery<CompanyIdBranchIdDTO>(query).ToList();
        //    data = data.Where(a => a.BranchID != 184 && a.CompanyID != 307).ToList();
        //    var dt = Constant.GetBharatTime();
        //    string[] leaveNames = { "Casual Leave", "Medical Leave" };
        //    foreach (var item in data)
        //    {
        //        for (int i = 0; i < leaveNames.Count(); i++)
        //        {
        //            string LeaveName = leaveNames[i];
        //            crm_leavetypename lst = new crm_leavetypename();
        //            lst.LeaveName = LeaveName;
        //            lst.BranchID = item.BranchID;
        //            lst.CompanyID = item.CompanyID;
        //            lst.IsActive = true;
        //            lst.IsDeleted = false;
        //            lst.CreatedOn = dt;
        //            lst.CreatedBy = item.Id;
        //            db.crm_leavetypename.Add(lst);
        //            db.SaveChanges();
        //        }
        //    }
        //    return Content(msg);
        //}

        //public ActionResult Lnmu()
        //{
        //    string msg = string.Empty;
        //    var data = db.crm_leavetypename.ToList();
        //    foreach (var item in data)
        //    {
        //        var ltId = item.ID;
        //        var fakeId = item.LeaveName == "Casual Leave" ? 1 : 2;

        //        var GetEmployeeLeave = db.crm_leaverequest_tbl.Where(em => em.CompanyID == item.CompanyID && em.BranchID == item.BranchID && em.LeaveTypeID == fakeId).FirstOrDefault();
        //        if (GetEmployeeLeave != null)
        //        {

        //            GetEmployeeLeave.LeaveTypeID = ltId;
        //            db.SaveChanges();
        //        }
        //    }
        //    return Content(msg);
        //}
    }
}
