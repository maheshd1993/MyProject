
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
    public class CRMUserListController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        /// <summary>
        /// Get : List of Users based on companyID and BranchID
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="ProfileName"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
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
                    UserList.Add(new Userddl() { uid = 0, UserName = "Select User" });
                    //var userList = db.crm_usertbl.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.Status == true && em.ProfileName.ToLower().Contains("sales") && em.Id != 1).OrderBy(em => em.Fname).ToList();
                    string userquery = @"select ur.Id as UserID, CONCAT(ur.Fname, ' ', ur.Lname) AS UserName 
                                from crm_usertbl ur
                                join crm_roleassigntbl rl on ur.ProfileId = rl.Id 
                                Where ur.BranchID = " + BranchID + " and ur.CompanyID = " + CompanyID + " and ur.Status = 1 and rl.ViewLeads = 1";
                    var userList  = db.Database.SqlQuery<CreateUserModel>(userquery).OrderBy(em => em.UserName).ToList();
                    if (userList.Count > 0)
                    {
                        var userData = (from usr in userList
                                        select new Userddl
                                        {
                                            uid = usr.UserID??0,
                                            UserName = usr.UserName
                                        }).ToList();
                        UserList.AddRange(userData.ToList());
                        //foreach (var item in userList)
                        //{
                        //    Userddl u = new Userddl();
                        //    u.uid = item.Id;
                        //    u.UserName = item.Fname + " " + item.Lname;
                        //    UserList.Add(u);
                        //}                        
                    }
                }
                else
                {
                    var GetUserData = db.crm_usertbl.Where(em => em.Id == UID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (GetUserData != null && GetUserData.MappedUsers != null)
                    {
                        UserList.Add(new Userddl() { uid = 0, UserName = "Select User" });

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
                        var uniq = userData.Where(x => !UserList.Any(y => y.uid == x.uid)).ToList();
                        UserList.AddRange(uniq);

                        //foreach (var item in GetMapUser)
                        //{
                        //    var mapid = Convert.ToInt32(item);
                        //    var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                        //    if (GetMapUserData != null)
                        //    {
                        //        Userddl u = new Userddl();
                        //        u.uid = mapid;
                        //        u.UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname;
                        //        UserList.Add(u);
                        //    }
                        //}
                    }
                    else
                    {
                        //UserList.Add(new Userddl() { uid = 0, UserName = "Select User" });
                        var userList = db.crm_usertbl.Where(em => em.Id == UID && em.Status == true && em.BranchID == branchID && em.CompanyID == companyID && em.ProfileId!=null).FirstOrDefault();
                        Userddl u = new Userddl();
                        u.uid = userList.Id;
                        u.UserName = userList.Fname + " " + userList.Lname;
                        UserList.Add(u);
                    }
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
