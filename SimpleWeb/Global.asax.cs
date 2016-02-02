using Kakariki.Scrabble.SimpleWeb.Binder;
using Kakariki.Scrabble.SimpleWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Kakariki.Scrabble.SimpleWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WordListConfig.RegisterWordLists(Server);

            ModelBinders.Binders.Add(typeof(BoardModel), new BoardModelBinder());
        }
    }
}
