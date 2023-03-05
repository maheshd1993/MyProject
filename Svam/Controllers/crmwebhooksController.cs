using Facebook;
using Newtonsoft.Json;
using Svam.EF;
using Svam.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers
{
    public class crmwebhooksController : Controller
    {
        [HttpGet]
        public ActionResult Test()
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = "1142530202801810",
                client_secret = "2ea7084a3ef1aed2cd52cc1f6b4c818c",
                grant_type = "client_credentials"
            });
            fb.AccessToken = result.access_token;
            var fb1 = new FacebookClient
            {
                AccessToken = fb.AccessToken
            };
            //var your_user_id = "10219891940546354";
            //var user_access_Token = "EAAQPIAriFpIBAOy1qv0Em6O0kfZB7SDTFNcrvsSxUZCu1ZCkCl9c4CKdtpEZCUOaRWXxUhaSME5xYEAq84HEMTjNL1LKxZC0yXGkLE1EJYpZCC7VZC7vgo4P9KbHitFLD5C8kKfpm54mEMf3TXsHdpBjUth3O5GIgyfX4qXVpPKDq4IQZCKW0SyE";

            //var json = fb.Get("https://graph.facebook.com/" + your_user_id + "/accounts?access_token=" + user_access_Token + "");

            var PageID = "625832474544280";
            var jsonpageToken = fb.Get("https://graph.facebook.com/v5.0/" + PageID + "?fields=access_token&access_token=EAAQPIAriFpIBAEEeXX4EDZAhqZCqjwUxVZBv9oJN54K1dYvbYTk3fjwejCkY7V0ZCYYHlvGGjcSm7vyIP8qJ20wfPVaRpEXxCnh7CCM8C0iUcEwEctkpnAR94AwbTjbplOyQVwaZCy5cXFQGlToIpbLIOkfJRy3peLhdEzXlPhBCLgDqWBP5pCt9IuY5JuMzRUsuAXQhOZCEQ9PDuFLvkQ");
            if (jsonpageToken != null)
            {
                var getleadgen_FormLists = fb.Get("https://graph.facebook.com/v5.0/" + PageID + "/leadgen_forms");
                if (getleadgen_FormLists != null)
                {
                    //foreach (var item in getleadgen_FormLists)
                    //{ 
                    
                    //}
                }
            }

            //var user_accessToken = "EAAQPIAriFpIBAOy1qv0Em6O0kfZB7SDTFNcrvsSxUZCu1ZCkCl9c4CKdtpEZCUOaRWXxUhaSME5xYEAq84HEMTjNL1LKxZC0yXGkLE1EJYpZCC7VZC7vgo4P9KbHitFLD5C8kKfpm54mEMf3TXsHdpBjUth3O5GIgyfX4qXVpPKDq4IQZCKW0SyE";
            //var page_accesstoken = "EAAQPIAriFpIBAB90SDlbG53VqW0YL8VLKEmIU3OXACuNXTZBEZCKmhuHobeWDJHzBwg3roRvPZBURvn9lWRkkbWNt5zZCAtkV7hnDA2Oz7BSMXQP2v9QJaGZAUCDtZCybbBUsvnGpmg3yPLfvDH6xy3LU1pSx1rYHtKdKkPijqiGZAvQ3PRbHy5Brx2BMyGLkfbWsc6WcUUyiWkgJyYVBlx";

            //var json = fb.Post(page_id + "/fields?" + page_accesstoken + "");
            //var json = fb.Get(page_id + "/leadgen_forms&access_token=" + user_accessToken + "");
          
            //dynamic parameters = new ExpandoObject();
            //parameters.subscribed_fields = "leadgen";
            //parameters.accesstoken = page_accesstoken;
            //parameters.useraccessToken = user_accessToken;

            return View();
        }
    }
}
