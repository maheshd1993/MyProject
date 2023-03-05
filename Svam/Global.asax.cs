using Svam.AutoMapperConfig;
using Svam.Models;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Svam
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutomapperConfig.MapIt();
            RegisterCacheEntry();
        }
        
        private static string PageUrl = GetSiteUrl();
        private const string CacheItemKey = "CalculateAllTodayOrderList";
        private void RegisterCacheEntry()
        {
            
            // Prevent duplicate key addition
            if (null != HttpContext.Current.Cache[CacheItemKey]) return;
            HttpContext.Current.Cache.Add(CacheItemKey, "CATO", null, DateTime.MaxValue, TimeSpan.FromMinutes(10), CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }
        public void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            HitPage();
        }
        private void HitPage()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            //client.DownloadData(PageUrl);
            client.DownloadData(PageUrl + "TicketEscalationSchedular/RubJobSchduler");
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();//get current culture
            //newCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";//set date format to MM/dd/yyyy
            //newCulture.DateTimeFormat.DateSeparator = "/";//add seperate 
            //Thread.CurrentThread.CurrentCulture = newCulture;//assign new culture
            if (HttpContext.Current.Request.Url.ToString() == PageUrl)
            {
                // Add the item in cache and when succesful, do the work.
                RegisterCacheEntry();
            }
        }

        public static string GetSiteUrl()
        {
            string url = string.Empty;
            HttpRequest request = HttpContext.Current.Request;
            url = HttpContext.Current.Request.Url.AbsoluteUri + "";
            if (url == "http://127.0.0.1/")
            {
                //int Port = HttpContext.Current.Request.Url.Port;
                url = "http://localhost:4365/";
            }
            else if (url == "http://demo3.nicoleinfosoftdemo.com/")
            {
                url = "http://demo3.nicoleinfosoftdemo.com/";
            }
            else if (url == "http://crm.smartcapita.com/")
            {
                url = "http://crm.smartcapita.com/";
            }
            return url;
        }
    }
}