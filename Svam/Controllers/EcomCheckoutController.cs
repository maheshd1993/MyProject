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
    public class EcomCheckoutController : System.Web.Http.ApiController
    {
        apiclasses apiclass = new apiclasses();
        [HttpPost]
        public HttpResponseMessage checkoutrecords(ecomclasses.Cartproducts cartproducts)
        {
            var users = apiclass.Checkoutrecords(cartproducts);
            return Request.CreateResponse(HttpStatusCode.OK, users.checkoutdetails, new JsonMediaTypeFormatter(), new MediaTypeHeaderValue("application/json"));
        }

    }
}
