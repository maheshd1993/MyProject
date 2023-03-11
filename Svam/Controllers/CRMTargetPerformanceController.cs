using Newtonsoft.Json;
using Svam.EF;
using Svam.Models;
using Svam.Repository;
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
    public class CRMTargetPerformanceController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        CommonRepository cr = new CommonRepository();
        public ActionResult CRMTargetPerformanceList(string SearchSalePersonName, string FromDate, string ToDate)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            CRMTargetSaleModel VLM = new CRMTargetSaleModel();
            VLM.Language = cr.GetCompanyLanguage(0);
            VLM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            //var dd = Constant.GetBharatTime();
            //DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            //var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            //var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

            var MStartDate = string.Empty;
            var MEndDate = string.Empty;
            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
            {
                //MStartDate = FromDate;
                //MEndDate = ToDate;

                if (VLM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    ViewBag.FromDate = FromDate;
                    ViewBag.ToDate = ToDate;

                    var fmDate = DateTime.ParseExact(FromDate, VLM.DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(FromDate);
                    var tDate = DateTime.ParseExact(ToDate, VLM.DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(ToDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                }
                else
                {
                    ViewBag.FromDate = FromDate;
                    ViewBag.ToDate = ToDate;

                    MStartDate = FromDate;
                    MEndDate = ToDate;
                }
            }
            //else 
            //{
            //    FromDate = MStartDate;
            //    ToDate = MEndDate;
            //    ViewBag.FromDate = MStartDate;
            //    ViewBag.ToDate = MEndDate;
            //}           

            var UID = 0;

            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {
                if (!string.IsNullOrEmpty(SearchSalePersonName))
                {
                    UID = Convert.ToInt32(SearchSalePersonName);
                }
                //var q = db.crm_usertbl.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.Status == true && em.ProfileName.Contains("Sales")).OrderBy(em => em.Fname).ToList();
                string userquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName,ur.EmployeeCode
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewLeads = 1";
                var q = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                VLM.oSalePersonList = new List<CRMTargetSaleModel>();

                if (q != null && q.Count > 0)//add all option if user list greater 0
                {
                    VLM.oSalePersonList.Add(new CRMTargetSaleModel
                    {
                        SalePersonID = 0,
                        SalePersonName = "All"
                    });

                    var user = (from item in q
                                select new CRMTargetSaleModel
                                {
                                    SalePersonID = Convert.ToInt32(item.UserID),
                                    SalePersonName = item.UserName + "    (" + item.EmployeeCode + ")",
                                }).ToList();
                    VLM.oSalePersonList.AddRange(user);//added users list
                    VLM.oSalePersonList = VLM.oSalePersonList.OrderBy(a => a.SalePersonName).ToList();//set users list alphabetical order
                }


                List<CRMTargetSaleModel> TSList = new List<CRMTargetSaleModel>();
                //DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_TargetSaleList(" + BranchID + "," + CompanyID + ",'" + SearchSalePersonName + "','" + MStartDate + "','" + MEndDate + "'," + UID + ")");

                DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_TargetSaleList(" + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
                if (GetRecords.Rows.Count > 0)
                {
                    for (int i = 0; i < GetRecords.Rows.Count; i++)
                    {
                        CRMTargetSaleModel TS = new CRMTargetSaleModel();
                        TS.SalePersonID = Convert.ToInt32(GetRecords.Rows[i]["SalePersonID"]);
                        TS.SalePersonName = GetRecords.Rows[i]["SalePersonName"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["SalePersonName"]);
                        TS.TargetID = GetRecords.Rows[i]["TargetID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["TargetID"]);
                        TS.sFromDate = GetRecords.Rows[i]["FromDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["FromDate"]);
                        TS.sToDate = GetRecords.Rows[i]["ToDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ToDate"]);
                        TS.TargetAchieveAmount = GetRecords.Rows[i]["TargetAchieveAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["TargetAchieveAmount"]);
                        TS.CurentTargetAchieveAmount = GetRecords.Rows[i]["CurentTargetAchieveAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["CurentTargetAchieveAmount"]);
                        TS.CurentTargetAdvanceAmount = GetRecords.Rows[i]["CurentTargetAdvanceAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["CurentTargetAdvanceAmount"]);

                        TSList.Add(TS);
                    }
                    VLM.oSaleTargetModelList = TSList;
                }
            }
            else
            {

                int loggedUID = Convert.ToInt32(Session["UID"]);
                if (!string.IsNullOrEmpty(SearchSalePersonName))
                {
                    UID = Convert.ToInt32(SearchSalePersonName);
                }
                else
                {
                    UID = Convert.ToInt32(Session["UID"]);
                }
                var GetUserData = db.crm_usertbl.Where(em => em.Id == loggedUID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                if (GetUserData != null && !string.IsNullOrEmpty(GetUserData.MappedUsers))
                {
                    VLM.MappedUser = GetUserData.MappedUsers.ToString();
                    //var GetMapUser = GetUserData.MappedUsers.Split(',');

                    var SpliteUserIds = GetUserData.MappedUsers.Split(',');
                    int[] UserIds = SpliteUserIds.Select(int.Parse).ToArray();


                    VLM.oSalePersonList = new List<CRMTargetSaleModel>();
                    //CRMTargetSaleModel u1 = new CRMTargetSaleModel();

                    VLM.oSalePersonList.Add(new CRMTargetSaleModel
                    {
                        SalePersonID = 0,
                        SalePersonName = "All"
                    });

                    VLM.oSalePersonList.Add(new CRMTargetSaleModel
                    {
                        SalePersonID = loggedUID,
                        SalePersonName = Session["UserName"].ToString()
                    });


                    var mappedUser = (from uIds in UserIds
                                      join item in db.crm_usertbl on uIds equals item.Id
                                      where item.Status == true && item.BranchID == BranchID && item.CompanyID == CompanyID
                                      select new CRMTargetSaleModel
                                      {
                                          SalePersonID = item.Id,
                                          SalePersonName = item.Fname + " " + item.Lname
                                      }).ToList();

                    VLM.oSalePersonList.AddRange(mappedUser);
                    VLM.oSalePersonList = VLM.oSalePersonList.OrderBy(a => a.SalePersonName).ToList();
                }

                List<CRMTargetSaleModel> TSList = new List<CRMTargetSaleModel>();
                //DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_TargetSaleList(" + BranchID + "," + CompanyID + ",'" + SearchSalePersonName + "','" + MStartDate + "','" + MEndDate + "'," + UID + ")");

                DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_TargetSaleList(" + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
                if (GetRecords.Rows.Count > 0)
                {
                    for (int i = 0; i < GetRecords.Rows.Count; i++)
                    {
                        CRMTargetSaleModel TS = new CRMTargetSaleModel();
                        TS.SalePersonID = Convert.ToInt32(GetRecords.Rows[i]["SalePersonID"]);
                        TS.SalePersonName = GetRecords.Rows[i]["SalePersonName"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["SalePersonName"]);
                        TS.TargetID = GetRecords.Rows[i]["TargetID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["TargetID"]);
                        TS.sFromDate = GetRecords.Rows[i]["FromDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["FromDate"]);
                        TS.sToDate = GetRecords.Rows[i]["ToDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ToDate"]);
                        TS.TargetAchieveAmount = GetRecords.Rows[i]["TargetAchieveAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["TargetAchieveAmount"]);
                        TS.CurentTargetAchieveAmount = GetRecords.Rows[i]["CurentTargetAchieveAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["CurentTargetAchieveAmount"]);
                        TSList.Add(TS);
                    }

                    VLM.oSaleTargetModelList = TSList;

                    if (VLM.oSalePersonList != null && VLM.oSalePersonList.Count > 0 && !string.IsNullOrEmpty(SearchSalePersonName) && SearchSalePersonName == "0")
                    {
                        VLM.oSaleTargetModelList = null;
                        VLM.oSaleTargetModelList = new List<CRMTargetSaleModel>();
                        foreach (var item in VLM.oSalePersonList)
                        {
                            int leadOwner = Convert.ToInt32(item.SalePersonID);
                            if (leadOwner != 0)
                            {
                                List<CRMTargetSaleModel> VlieadList = TSList.Where(em => em.SalePersonID == leadOwner).ToList();
                                VLM.oSaleTargetModelList.AddRange(VlieadList);
                            }
                        }
                    }
                }
            }
            return View(VLM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddModifiyTargetSale(CRMTargetSaleModel CTSM)
        {
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                CTSM.DateFormat = Constant.DateFormat();//get date format by company id
                Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                if (ModelState.IsValid)
                {
                    if (CTSM.TargetID > 0)
                    {
                        var GetTarget = db.crm_salestarget.Where(em => em.TargetID == CTSM.TargetID && em.SalePersonID == CTSM.SalePersonID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetTarget != null)
                        {
                            GetTarget.TargetID = Convert.ToInt32(CTSM.TargetID);
                            GetTarget.SalePersonID = CTSM.SalePersonID;
                            GetTarget.FromDate = DateTime.ParseExact(CTSM.FromDate, CTSM.DateFormat, CultureInfo.InvariantCulture);
                            GetTarget.ToDate = DateTime.ParseExact(CTSM.ToDate, CTSM.DateFormat, CultureInfo.InvariantCulture);
                            GetTarget.TargetAchieveAmount = CTSM.TotalTargetAmount;
                            db.SaveChanges();
                            TempData["success"] = "Sales target modified succesfully.";
                        }
                        else
                        {
                            TempData["alert"] = "Something went wrong.Please try again.";
                        }
                    }
                    else
                    {
                        DateTime sdate = DateTime.ParseExact(CTSM.FromDate, CTSM.DateFormat, CultureInfo.InvariantCulture);
                        DateTime eDate = DateTime.ParseExact(CTSM.ToDate, CTSM.DateFormat, CultureInfo.InvariantCulture);

                        var GetTargetExists = db.crm_salestarget.Where(em => em.SalePersonID == CTSM.SalePersonID &&
                        (em.FromDate.Value.Day == sdate.Day && em.FromDate.Value.Month == sdate.Month && em.FromDate.Value.Year == sdate.Year)
                        && (em.ToDate.Value.Day == eDate.Day && em.ToDate.Value.Month == eDate.Month && em.ToDate.Value.Year == eDate.Year)
                        && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                        if (GetTargetExists == null)
                        {
                            crm_salestarget CT = new crm_salestarget();
                            CT.SalePersonID = CTSM.SalePersonID;
                            CT.FromDate = DateTime.ParseExact(CTSM.FromDate, CTSM.DateFormat, CultureInfo.InvariantCulture);
                            CT.ToDate = DateTime.ParseExact(CTSM.ToDate, CTSM.DateFormat, CultureInfo.InvariantCulture);
                            CT.TargetAchieveAmount = CTSM.TotalTargetAmount;
                            CT.BranchID = BranchID;
                            CT.CompanyID = CompanyID;
                            db.crm_salestarget.Add(CT);
                            db.SaveChanges();
                            TempData["success"] = "Sales target save succesfully.";
                        }
                        else
                        {
                            TempData["alert"] = "Sales target already exists for this month.";
                        }
                    }
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(a => a.Errors).Select(a => a.ErrorMessage));

                    TempData["alert"] = message;
                }
                return Redirect("/CRMTargetPerformance/CRMTargetPerformanceList");
            }
            catch (Exception ex)
            {
                TempData["alert"] = "Something went wrong.Please try again.";
                ExceptionLogging.SendExcepToDB(ex);
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult ViewTargerAchived(Int32? SalePersonID, String FromDate, String ToDate)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            var dd = Constant.GetBharatTime();
            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            var MStartDate = monthStartDate.ToString("yyyy/MM/dd");
            var MEndDate = MonthendDate.ToString("yyyy/MM/dd");

            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
            {
                MStartDate = FromDate;
                MEndDate = ToDate;
                ViewBag.FromDate = MStartDate;
                ViewBag.ToDate = MEndDate;
            }
            else
            {
                FromDate = MStartDate;
                ToDate = MEndDate;
                ViewBag.FromDate = MStartDate;
                ViewBag.ToDate = MEndDate;
            }
            CRMTargetSaleModel VLM = new CRMTargetSaleModel();
            //JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };


            var sDate = new DateTime();
            var eDate = new DateTime();
            //decimal? SalePercentage = 0;

            List<DateTime> list = new List<DateTime>();


            //List<string> list2 = new List<string>();

            List<CRMTargetSaleModel> TSList = new List<CRMTargetSaleModel>();
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_TargetSalePersonByID(" + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "'," + SalePersonID + ")");
            if (GetRecords.Rows.Count > 0)
            {
                for (int i = 0; i < GetRecords.Rows.Count; i++)
                {
                    CRMTargetSaleModel TS = new CRMTargetSaleModel();
                    TS.sFromDate = GetRecords.Rows[i]["FromDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["FromDate"]);
                    TS.sToDate = GetRecords.Rows[i]["ToDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ToDate"]);
                    TS.CREATEDDATE = GetRecords.Rows[i]["CREATEDDATE"] == DBNull.Value ? new DateTime() : Convert.ToDateTime(GetRecords.Rows[i]["CREATEDDATE"]);
                    TS.TargetAchieveAmount = GetRecords.Rows[i]["TargetAchieveAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["TargetAchieveAmount"]);
                    TS.CurentTargetAchieveAmount = GetRecords.Rows[i]["CurentTargetAchieveAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["CurentTargetAchieveAmount"]);
                    TS.ProjectValueDateGroupBy = GetRecords.Rows[i]["ProjectValueDateGroupBy"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ProjectValueDateGroupBy"]);


                    TSList.Add(TS);
                }
                sDate = Convert.ToDateTime(MStartDate);
                eDate = Convert.ToDateTime(MEndDate);

                //sDate = Convert.ToDateTime(TSList[0].sFromDate);
                //eDate = Convert.ToDateTime(TSList[0].sToDate);

                //if (sDate != null && eDate != null)
                //{
                //    for (var day = sDate.Date; day.Date <= eDate.Date; day = day.AddDays(1))
                //    {
                //        list.Add(day);                        
                //    }
                //}

                //var record  = (from dlist in list 
                //           join dummy in TSList on dlist.Date equals dummy.CREATEDDATE.Date  into ddd
                //           from dumlst in ddd.DefaultIfEmpty()
                //           select new ChartModel
                //           {
                //               x = new DateTime(dlist.Year, dlist.Month, dlist.Day),
                //               y = dumlst==null ? 0: dumlst.ProjectValueDateGroupBy??0
                //           }).ToList();

                //VLM.oSaleTargetModelList = TSList;
                VLM.TargetAchieveAmount = TSList != null && TSList.Count > 0 ? TSList[0].TargetAchieveAmount : 0;
                VLM.CurentTargetAchieveAmount = TSList != null && TSList.Count > 0 ? TSList[0].CurentTargetAchieveAmount : 0;
                //ViewBag.AxisYAmount  = TSList[0].CurentTargetAchieveAmount > TSList[0].TargetAchieveAmount? TSList[0].CurentTargetAchieveAmount: TSList[0].TargetAchieveAmount;
                //ViewBag.DataList = "" + TSList[0].TargetAchieveAmount + "," + TSList[0].CurentTargetAchieveAmount + ",";               
                //SalePercentage = (TSList[0].CurentTargetAchieveAmount / TSList[0].TargetAchieveAmount) * 100;
                //VLM.SalePercentage = SalePercentage;

                VLM.FromToDate = sDate.ToString("dd-MM-yyyy") + " - " + eDate.ToString("dd-MM-yyyy");

                //var record1  = new List<ChartModel>();
                //var record2 = new List<ChartModel>();

                //record1.Add(new ChartModel("Assigned Target", Convert.ToDouble(TSList[0].TargetAchieveAmount)));
                //record1.Add(new ChartModel("Achieved Target", null));
                //record2.Add(new ChartModel("Assigned Target", null));
                //record2.Add(new ChartModel("Achieved Target", Convert.ToDouble(TSList[0].CurentTargetAchieveAmount)));

                ////serialize data to json format
                //ViewBag.DataPoints1 = JsonConvert.SerializeObject(record1);
                //ViewBag.DataPoints2 = JsonConvert.SerializeObject(record2);
            }

            #region dummy data


            //var dummyData = new List<CRMTargetSaleModel> {
            //    new CRMTargetSaleModel
            //    {

            //        CREATEDDATE="2020-05-01",
            //        TargetAchieveAmount=50000,
            //        ProjectValueDateGroupBy=10000,
            //        CurentTargetAchieveAmount=33700
            //    },
            //    new CRMTargetSaleModel
            //    {

            //        CREATEDDATE="2020-05-05",
            //        TargetAchieveAmount=50000,
            //        ProjectValueDateGroupBy=30000,
            //        CurentTargetAchieveAmount=33700
            //    },
            //    new CRMTargetSaleModel
            //    {
            //        CREATEDDATE="2020-05-10",
            //        TargetAchieveAmount=50000,
            //        ProjectValueDateGroupBy=40000
            //    },
            //    new CRMTargetSaleModel
            //    {

            //        CREATEDDATE="2020-05-15",
            //        TargetAchieveAmount=50000,
            //        ProjectValueDateGroupBy=60000,
            //        CurentTargetAchieveAmount=33700
            //    },
            //    new CRMTargetSaleModel
            //    {

            //        CREATEDDATE="2020-05-20",
            //        TargetAchieveAmount=50000,
            //        ProjectValueDateGroupBy=50000,
            //        CurentTargetAchieveAmount=33700
            //    }
            //};
            ////VLM.GetChartData = dummyData;
            //sDate = Convert.ToDateTime("2020-05-01");
            //eDate = Convert.ToDateTime("2020-05-31");

            //if (sDate != null && eDate != null)
            //{
            //    for (var day = sDate.Date; day.Date <= eDate.Date; day = day.AddDays(1))
            //    {
            //        list.Add(day);
            //        list2.Add(String.Format("{0:dd-MM-yyyy}",day));
            //    }
            //}

            //var rec = (from dummy in dummyData
            //           join dlist in list on dummy.CREATEDDATE equals dlist.Date.ToString("yyyy-MM-dd") into ddd
            //           from ddlst in ddd.DefaultIfEmpty()
            //           select new ChartModel
            //           {
            //              x=ddlst.Date,
            //               y=dummy.ProjectValueDateGroupBy??0
            //           }).ToList();




            //VLM.TargetAchieveAmount = dummyData[0].TargetAchieveAmount;
            //VLM.FromToDate = sDate.ToString("dd-MM-yyyy") + " - " + eDate.ToString("dd-MM-yyyy");

            //ViewBag.DataPoints = JsonConvert.SerializeObject(rec);

            // SalePercentage = (dummyData[0].CurentTargetAchieveAmount / dummyData[0].TargetAchieveAmount) * 100;
            //VLM.SalePercentage = SalePercentage;
            #endregion

            List<CRMTargetSaleModel> TSClosedList = new List<CRMTargetSaleModel>();
            DataTable GetRecordClosed = DataAccessLayer.GetDataTable("call CRM_TargetSalePersonClosedList(" + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "'," + SalePersonID + ")");
            if (GetRecordClosed.Rows.Count > 0)
            {
                for (int i = 0; i < GetRecordClosed.Rows.Count; i++)
                {
                    CRMTargetSaleModel TS = new CRMTargetSaleModel();
                    TS.CustomerName = GetRecordClosed.Rows[i]["Customer"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecordClosed.Rows[i]["Customer"]);
                    TS.MobileNumber = GetRecordClosed.Rows[i]["MobileNo"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecordClosed.Rows[i]["MobileNo"]);
                    TSClosedList.Add(TS);
                }
                VLM.oSaleTargetClosedModelList = TSClosedList;
            }
            return View(VLM);
        }
    }
}
