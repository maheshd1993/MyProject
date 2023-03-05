using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Svam.EF;
using Svam._Class;
using Traders.Models;

namespace Svam.Controllers.Services
{
    public class getMappedUsersController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        // GET api/getMappedUsers/17
        public HttpResponseMessage Get(string id)
        {
            ViewLeadsModel VLM = new ViewLeadsModel();
            crm_usertbl u = new crm_usertbl();
            UserDetailModel ug = new UserDetailModel();
            int usetID = Convert.ToInt32(id);
            u = db.crm_usertbl.Where(em => em.Id == usetID).SingleOrDefault();
            if (u == null)
            {
                var message = string.Format("Empty User-ID");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                if (!string.IsNullOrEmpty(u.MappedUsers))
                {
                    #region Get-MappedUser-Parents
                    //VLM.MappedUser = u.MappedUsers.ToString();
                    var GetMapUser = u.MappedUsers.Split(',');
                    VLM.Userddllist = new List<Userddl>();
                    Userddl us = new Userddl();
                    us.UserName = "All";
                    Userddl u1 = new Userddl();
                    VLM.Userddllist.Add(us);
                    u1.uid = usetID;
                    u1.UserName = u.Fname + " " + u.Lname;
                    VLM.Userddllist.Add(u1);

                    foreach (var item in GetMapUser)
                    {
                        var mapid = Convert.ToInt32(item);
                        var GetMapUserData = db.crm_usertbl.Where(em => em.Id == mapid && em.Status == true).SingleOrDefault();
                        if (GetMapUserData != null)
                        {
                            var user = new Userddl
                            {
                                uid = mapid,
                                UserName = GetMapUserData.Fname + " " + GetMapUserData.Lname
                            };
                            VLM.Userddllist.Add(user);
                        }
                        //ViewBag.AssignTo = new SelectList(LMM.mapUserList, "Id", "UserName");
                    }
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, VLM.Userddllist.OrderBy(em => em.UserName));
                }
                else
                {
                    var message = string.Format("No Record Found!");
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
        }
    }
}