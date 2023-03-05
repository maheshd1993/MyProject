using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers
{
    public class CRMPaymentController : Controller
    {
        public ActionResult Payment(string FromDate, String ToDate, Int32? CustomerID)
        {
            var dd = Constant.GetBharatTime();
            DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            CRMPaymentModel CPM = new CRMPaymentModel();

            CPM.DateFormat = Constant.DateFormat();//get date format by company id
            Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate))
            {
               
                if (CPM.DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                {
                    TempData["MStartDate"] = FromDate;
                    TempData["MEndDate"] = ToDate;

                    var fmDate = Convert.ToDateTime(FromDate);
                    var tDate = Convert.ToDateTime(ToDate);

                    MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                    MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                }
                else
                {
                    TempData["MStartDate"] = FromDate;
                    TempData["MEndDate"] = ToDate;

                    MStartDate = FromDate;
                    MEndDate = ToDate;
                }
            }

            if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            {
                DataTable GetpaymentList = DataAccessLayer.GetDataTable("call CRM_CustomerPayment(" + 0 + "," + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "')");
                if (GetpaymentList.Rows.Count > 0)
                {
                    List<CRMPaymentModel> CPMList = new List<CRMPaymentModel>();
                    for (int i = 0; i < GetpaymentList.Rows.Count; i++)
                    {
                        CRMPaymentModel cPM = new CRMPaymentModel();
                        cPM.BillNumber = Convert.ToString(GetpaymentList.Rows[i]["BillNumber"]);
                        cPM.CustomerName = Convert.ToString(GetpaymentList.Rows[i]["CustomerName"]);
                        cPM.BillDate = Convert.ToDateTime(GetpaymentList.Rows[i]["BillDate"]);
                        cPM.DueDate = Convert.ToDateTime(GetpaymentList.Rows[i]["DueDate"]);
                        cPM.BalanceAmount = Convert.ToDecimal(GetpaymentList.Rows[i]["BalanceAmt"]);
                        CPMList.Add(cPM);
                    }
                    CPM.CRMPaymentModelList = CPMList;
                }
            }
            else 
            {
                DataTable GetpaymentList = DataAccessLayer.GetDataTable("call CRM_CustomerPayment(" + Convert.ToInt32(Session["UID"]) + "," + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "')");
                if (GetpaymentList.Rows.Count > 0)
                {
                    List<CRMPaymentModel> CPMList = new List<CRMPaymentModel>();
                    for (int i = 0; i < GetpaymentList.Rows.Count; i++)
                    {
                        CRMPaymentModel cPM = new CRMPaymentModel();
                        cPM.BillNumber = Convert.ToString(GetpaymentList.Rows[i]["BillNumber"]);
                        cPM.CustomerName = Convert.ToString(GetpaymentList.Rows[i]["CustomerName"]);
                        cPM.BillDate = Convert.ToDateTime(GetpaymentList.Rows[i]["BillDate"]);
                        cPM.DueDate = Convert.ToDateTime(GetpaymentList.Rows[i]["DueDate"]);
                        cPM.BalanceAmount = Convert.ToDecimal(GetpaymentList.Rows[i]["BalanceAmt"]);
                        CPMList.Add(cPM);
                    }
                    CPM.CRMPaymentModelList = CPMList;
                }
            
            }
            return View(CPM);
        }
    }
}
