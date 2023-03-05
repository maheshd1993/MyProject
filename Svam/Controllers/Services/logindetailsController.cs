using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Svam.EF;
using Svam._Class;

namespace Svam.Controllers.Services
{
    public class logindetailsController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        // GET api/logindetails/2
        public HttpResponseMessage Get(string id)
        {
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
                ug.id = u.Id.ToString();
                ug.userName = u.UserName.ToString();
                ug.EmailID = u.Email;
                ug.FirstName = u.Fname;
                ug.TimeZone = u.TimeZone;
                ug.MappedUsers = u.MappedUsers;
                ug.ProfileName = u.ProfileName;
                ug.ProfileId = u.ProfileId;
                ug.UserId = u.Id.ToString();
                ug.Status = u.Status.ToString();
                return Request.CreateResponse(HttpStatusCode.OK, ug);
            }
        }
    }
}