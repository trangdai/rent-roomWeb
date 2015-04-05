using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using System.Threading;


namespace NhaTroDBFirstVer1
{
    public class MyRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var culture = requestContext.RouteData.Values["culture"].ToString();
            var ci = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            return base.GetHttpHandler(requestContext);
        }
    }
}
//namespace NhaTroBasic
//{

//    public class BasePage : System.Web.UI.Page
//    {
//        protected override void InitializeCulture()
//        {
//            if (!string.IsNullOrEmpty(Request["lang"]))
//            {
//                Session["lang"] = Request["lang"];
//            }
//            string lang = Convert.ToString(Session["lang"]);
//            string culture = string.Empty;

//            if (lang.ToLower().CompareTo("en") == 0 || string.IsNullOrEmpty(culture))
//            {
//                culture = "en-US";
//            }
//            if (lang.ToLower().CompareTo("vi") == 0)
//            {
//                culture = "vi-VN";
//            }
//            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
//            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

//            base.InitializeCulture();
//        }
//    }
//}