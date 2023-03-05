using Svam.EF;
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
    public class CRM_ViewTicketTeamRemarkController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
        //api/CRM_ViewTicketTeamRemark?TicketId=383&CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        /// <summary>
        /// Get :view ticket status on companyID and BranchID and ticketid
        /// </summary>
        /// <param name="TicketId"></param>       
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(int TicketId, string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;
            List<TicketMap> oTicketMap = new List<TicketMap>();

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

                var DateFormat = Constant.DateFormatForApi(companyID);//get date format by company id
                DataTable Gettickectdetaillist = DataAccessLayer.GetDataTable("call CRM_TicketTeamRemarks(" + TicketId + ")");
                if (Gettickectdetaillist.Rows.Count > 0)
                {
                    for (int i = 0; i < Gettickectdetaillist.Rows.Count; i++)
                    {
                        TicketMap tm = new TicketMap();
                        tm.Message = Convert.ToString(Gettickectdetaillist.Rows[i]["Message"]);
                        tm.AttachmentFile = Convert.ToString(Gettickectdetaillist.Rows[i]["AttachmentFile"]);
                        tm.UserName = Convert.ToString(Gettickectdetaillist.Rows[i]["UserName"]);
                        tm.CreatedOn = String.Format("{0:" + DateFormat + "}", Convert.ToDateTime(Gettickectdetaillist.Rows[i]["CreatedOn"]));
                        tm.StatusName = Convert.ToString(Gettickectdetaillist.Rows[i]["StatusName"]);
                        oTicketMap.Add(tm);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No record found");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            return Request.CreateResponse(HttpStatusCode.OK, oTicketMap);

        }
    }
}
