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
    public class CRMPaymentController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(Int32 CompanyID, Int32 BranchID, string ProfileName, Int32? LoginID, string Token)
        {
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

            List<CRMPaymentModel> cRMPaymentModelList = new List<CRMPaymentModel>();
            String Records = String.Empty;
           
            try
            {
                var dd = Constant.GetBharatTime();
                DateTime MonthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = MonthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthEndDate.ToString("dd/MM/yyyy");

                if (!string.IsNullOrEmpty(ProfileName) && ProfileName.Trim('"') == "SuperAdmin")
                {
                    CRMPaymentModel CPM = new CRMPaymentModel();
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
                        cRMPaymentModelList = CPMList;
                    }
                    else
                    {
                        Records = "** No Record Found **";
                    }
                }
                else
                {
                    CRMPaymentModel CPM = new CRMPaymentModel();
                    DataTable GetpaymentList = DataAccessLayer.GetDataTable("call CRM_CustomerPayment(" + LoginID + "," + BranchID + "," + CompanyID + ",'" + MStartDate + "','" + MEndDate + "')");
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
                        cRMPaymentModelList = CPMList;
                    }
                    else
                    {
                        Records = "** No Record Found **";
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
                    return Request.CreateResponse(HttpStatusCode.OK, cRMPaymentModelList);
                }
            }
        }
    }
}
