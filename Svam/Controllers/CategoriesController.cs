using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Svam._Classes;

namespace Svam.Controllers
{
    public class CategoriesController : System.Web.Http.ApiController
    {
       apiclasses apiclass = new apiclasses();     
       [HttpPost]
       public HttpResponseMessage getcategories(int companyid, int branchid)
       {
           var users = apiclass.getcategoriesrecord(companyid, branchid);
           return Request.CreateResponse(HttpStatusCode.OK, users, new JsonMediaTypeFormatter(), new MediaTypeHeaderValue("application/json"));
       }      
    }
}
