using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;
using Svam.EF;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_TicketAssignToUserListController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
        //api/CRM_TicketAssignToUserList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage Get(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;
            List<Userddl> UserList = new List<Userddl>();

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

                 UserList.Add(new Userddl() { uid = 0, UserName = "Select Assign To" });

                #region Assign All Users list
                var userData = (from em in db.crm_usertbl
                                where em.BranchID == branchID && em.CompanyID == companyID
                                && em.Status == true && em.ProfileId != null
                                select new Userddl
                                {
                                    uid = em.Id,
                                    UserName = em.Fname + " " + em.Lname
                                }
                                    ).ToList();
                if (userData != null && userData.Count > 0)
                {
                    UserList.AddRange(userData.ToList());
                }

                #endregion

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");

            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, UserList);
            }
        }
    }
}
