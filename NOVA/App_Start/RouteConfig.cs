using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NOVA
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Arsiv",
                url: "Finans/DetayliRiskLimiti/{CariKod}",
                defaults: new { controller = "Finans", action = "DetayliRiskLimiti", CariKod = "null" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SecondPage",
                url: "{ controller}/{ action}/{ id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "Hata",
             url: "hata/{kod}",
             defaults: new { controller = "Error", action = "Page404", kod = UrlParameter.Optional });
        }
    }
}
