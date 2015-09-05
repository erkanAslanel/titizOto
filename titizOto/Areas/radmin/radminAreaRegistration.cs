using System.Web.Mvc;

namespace titizOto.Areas.radmin
{
    public class radminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "radmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("adminLogin", "radmin/", new { controller = "Login", action = "Index" });

            context.MapRoute("radmin_ProductCritear_filter", "radmin/CritearProduct/{action}/{filterId}/{id}", new { controller = "ProductCritear", action = "IndexWithFilter", id = UrlParameter.Optional }, new[] { "titizOto.Areas.radmin.Controllers" });

            context.MapRoute("radmin_Stock_filter", "radmin/Stock/{action}/{filterId}/{id}", new { controller = "Stock", action = "IndexWithFilter", id = UrlParameter.Optional }, new[] { "titizOto.Areas.radmin.Controllers" });

            context.MapRoute("radmin_PosOption_filter", "radmin/PosOption/{action}/{filterId}/{id}", new { controller = "PosOption", action = "IndexWithFilter", id = UrlParameter.Optional }, new[] { "titizOto.Areas.radmin.Controllers" });

            context.MapRoute("radmin_Gallery_filter", "radmin/Gallery/{action}/{filterId}/{id}", new { controller = "Gallery", action = "IndexWithFilter", id = UrlParameter.Optional }, new[] { "titizOto.Areas.radmin.Controllers" });

            context.MapRoute("radmin_Address_filter", "radmin/Address/{action}/{filterId}/{id}", new { controller = "Address", action = "IndexWithFilter", id = UrlParameter.Optional }, new[] { "titizOto.Areas.radmin.Controllers" });

            context.MapRoute(
                "radmin_default",
                "radmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, new[] { "titizOto.Areas.radmin.Controllers" }
            );
        }
    }
}
