using Svam._Class;
using Svam.EF;
using Svam.UtilityManager;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Svam.Controllers.Services
{
    public class CRMSmartCapitaLoginController : ApiController
    {
        niscrmEntities db = new niscrmEntities();

        public HttpResponseMessage Post(CRMUserModel CRM)
        {
            string msg = String.Empty;
            crm_usertbl utbl = new crm_usertbl();
            utbl.ByUID = CRM.ByUID;
            utbl.UserName = CRM.UserName.Trim();
            utbl.Fname = CRM.Fname.Trim();
            utbl.Lname = CRM.Lname.Trim();
            utbl.Email = CRM.Email.Trim();
            utbl.TimeZone = CRM.TimeZone.Trim();
            utbl.Password = CRM.Password.Trim();
            utbl.ProfileId = null;
            utbl.ProfileName = null;
            utbl.Status = true;
            utbl.MappedUsers = null;
            utbl.Created_at = Constant.GetBharatTime();
            utbl.CasualLeave = null;
            utbl.MedicalLeave = null;
            utbl.Year = null;
            utbl.BranchID = CRM.BranchID;
            utbl.CompanyID = CRM.CompanyID;
            db.crm_usertbl.Add(utbl);
            if (db.SaveChanges() > 0)
            {
                msg = "User created successful";
            }          
          
            if (!String.IsNullOrEmpty(msg))
            {
                return Request.CreateResponse(HttpStatusCode.OK, msg);
            }
            else
            {
                var message = string.Format("");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
        }
    }
}
