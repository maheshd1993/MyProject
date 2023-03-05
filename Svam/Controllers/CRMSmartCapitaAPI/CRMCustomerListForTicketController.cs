using Svam.EF;
using Svam.Models;
using Svam.Models.ViewModel;
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
    public class CRMCustomerListForTicketController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
        // GET api/CRMCustomerListForTicket?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        public HttpResponseMessage Get(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;
            List<TicketCustomerVM> CustomerList = new List<TicketCustomerVM>();

            try
            {
                Int32 branchID = Convert.ToInt32(BranchID);
                Int32 companyID = Convert.ToInt32(CompanyID);

                //System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //     Token = headers.GetValues("Token").First();
                //}

                var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
               
                    CustomerList.Add(new TicketCustomerVM() { CustomerID = 0, CustomerName = "Select Customer" });
                DataTable dtCustomerList = DataAccessLayer.GetDataTable("call CRM_TicketCustomer(" + CompanyID + "," + BranchID + ",0)");
                if (dtCustomerList.Rows.Count > 0)
                {
                    
                    for (int i = 0; i < dtCustomerList.Rows.Count; i++)
                    {
                        TicketCustomerVM customer = new TicketCustomerVM();
                        customer.CustomerID = Convert.ToInt32(dtCustomerList.Rows[i]["CustoemrID"]);
                        customer.CustomerName = Convert.ToString(dtCustomerList.Rows[i]["CustomerName"]);
                        CustomerList.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            return Request.CreateResponse(HttpStatusCode.OK, CustomerList);

            //if (ErrorMessage != string.Empty)
            //{
            //    HttpError err = new HttpError(ErrorMessage);
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, CustomerList);
            //}
        }
    }
}
