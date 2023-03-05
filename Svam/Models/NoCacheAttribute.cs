using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            var cache = GetCache(filterContext);

            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            cache.SetValidUntilExpires(false);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }

        protected virtual HttpCachePolicyBase GetCache(ResultExecutingContext filterContext)
        {
            return filterContext.HttpContext.Response.Cache;
        }
    }
}