using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HelperSite.Routing;

namespace titizOto
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("download/{*pathInfo}");
            routes.IgnoreRoute("Download/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("robots.txt");

            #region Image

            routes.MapRoute(
name: "DefaultImageResize",
url: "ImageShow/Resize/{path}/{photoName}/{width}/{height}",
defaults: new { controller = "ImageShow", Action = "Resize" },
namespaces: new[] { "titizOto.Controllers" }
);

            routes.MapRoute(
name: "DefaultImageShowGuidWithWaterMark",
url: "ImageShow/ShowGuid/{path}/{guid}/{width}/{height}",
defaults: new { controller = "ImageShow", Action = "ShowGuidWithoutWatermark" },
namespaces: new[] { "titizOto.Controllers" }
);

            routes.MapRoute(
name: "DefaultImage",
url: "ImageShow/{path}/{photoName}/{coordinate}",
defaults: new { controller = "ImageShow", action = "Crop" },
namespaces: new[] { "titizOto.Controllers" }
);

            routes.MapRoute(
name: "DefaultImageExact",
url: "ImageShow/{path}/{photoName}/{coordinate}/{width}/{height}",
defaults: new { controller = "ImageShow", action = "CropExact" },
namespaces: new[] { "titizOto.Controllers" }
);

            routes.MapRoute(
name: "DefaultImageExactImage",
url: "ImageShow/{path}/{photoName}/{coordinate}/{width}/{height}/1",
defaults: new { controller = "ImageShow", action = "CropExactExactImage" },
namespaces: new[] { "titizOto.Controllers" }
);




            #endregion
              
            routes.Add(new CustomRouting());

            #region DefaultPattern

            routes.MapRoute(
                name: "DefaultController",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "MainPage", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"^(tr|en)$" }, namespaces: new[] { "titizOto.Controllers" }
            );

            routes.MapRoute(
              name: "Default",
              url: "{lang}",
              defaults: new { controller = "MainPage", action = "Index", id = UrlParameter.Optional },
              constraints: new { lang = @"^(tr|en)$" }, namespaces: new[] { "titizOto.Controllers" }
          );


            routes.MapRoute(
                name: "DefaultMain",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MainPage", action = "Index", id = UrlParameter.Optional }
            );

            #endregion

        }
    }
}