using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Svam.EF;

namespace Svam.Controllers.Services
{
    public class getLeadDescriptionController : ApiController
    {
        niscrmEntities db = new niscrmEntities();
        // GET api/getLeadDescription/17
        public HttpResponseMessage Get(string id)
        {
            Int64 Lid = Convert.ToInt64(id);
            var getData = db.crm_leaddescriptiontbl.Where(em => em.LeadId == Lid).OrderByDescending(em => em.Date).ToList();
            if (getData.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, getData);
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