using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NhaTroDBFirstVer1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

             //multi language
            foreach (Route r in routes)
            {

                r.RouteHandler = new MyRouteHandler();
                r.Url = "{culture}/" + r.Url;
                //thêm default culture
                if (r.Defaults == null)
                {
                    r.Defaults = new RouteValueDictionary();
                }
                r.Defaults.Add("culture", Culture.vi.ToString());

                //thêm ràng buộc cho culture parameter
                if (r.Constraints == null)
                {
                    r.Constraints = new RouteValueDictionary();
                }
                r.Constraints.Add("culture", new CultureConstraint(Culture.en.ToString(),
                    Culture.vi.ToString()));
            }

        }


    }
}