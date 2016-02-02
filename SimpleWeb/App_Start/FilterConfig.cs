using System.Web;
using System.Web.Mvc;

namespace Kakariki.Scrabble.SimpleWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
