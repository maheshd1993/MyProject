using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;
namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_TicketUserListController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
        //api/CRM_TicketUserList?UID=61&ProfileName=SuperAdmin&CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        /// <summary>
        /// Get : List of Users based on companyID and BranchID
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="ProfileName"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(Int32 UID, string ProfileName, string CompanyID, string BranchID, string Token)
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
                if (ProfileName == "SuperAdmin")
                {
                        UserList.Add(new Userddl() { uid = 0, UserName = "ALL" });

                    #region Admin View All Users
                    var userData = (from asUser in db.crm_tickets
                                        join user in db.crm_usertbl on asUser.AssignedTo equals user.Id
                                        join user2 in db.crm_usertbl on asUser.CreatedBy equals user2.Id
                                        where (asUser.CreatedBy != UID || asUser.AssignedTo != null) &&
                                         asUser.BranchID == branchID
                                        && asUser.CompanyId == companyID
                                        && user.Status == true && user2.Status == true
                                        orderby user2.Fname
                                        select new Userddl
                                        {
                                            uid = user.Id,
                                            UserName = user.Fname + " " + user.Lname
                                        }
                                       ).Distinct().ToList();
                    if(userData!=null && userData.Count>0)
                    {
                        UserList.AddRange(userData.ToList());
                    }
                    #endregion
                }
                else
                {
                    #region Employee will view only mapped user
                    var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();

                    if (GetUserData != null && GetUserData.MappedUsers != null)
                    {
                        UserList.Add(new Userddl() { uid = 0, UserName = "ALL" });

                        Userddl u1 = new Userddl();
                        u1.uid = UID;
                        u1.UserName = GetUserData.Fname + " " + GetUserData.Lname; ;
                        UserList.Add(u1);

                        var GetMapUser = GetUserData.MappedUsers.Split(',');
                        int[] UserIds = GetMapUser.Select(int.Parse).ToArray();
                        var userData = (from usr in db.crm_usertbl
                                        join mu in UserIds on usr.Id equals mu
                                        where usr.Status == true && usr.BranchID == branchID && usr.CompanyID == companyID
                                        select new Userddl
                                        {
                                            uid = usr.Id,
                                            UserName = usr.Fname + " " + usr.Lname
                                        }).ToList();
                        if(userData!=null && userData.Count>0)
                        {
                            var uniq = userData.Where(x => !UserList.Any(y => y.uid == x.uid)).ToList();
                            UserList.AddRange(uniq);
                        }                      
                    }
                    #endregion

                }
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
