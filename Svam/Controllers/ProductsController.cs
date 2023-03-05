using Svam._Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
namespace Svam.Controllers
{
    public class ProductsController : System.Web.Http.ApiController
    {
        apiclasses apiclass = new apiclasses();
        [HttpPost]
        public HttpResponseMessage getallproducts(int companyid, int branchid)
        {
            var users = apiclass.getallproductsrecord(companyid, branchid);
            return Request.CreateResponse(HttpStatusCode.OK, users, new JsonMediaTypeFormatter(), new MediaTypeHeaderValue("application/json"));
        }

    }
}
