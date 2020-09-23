using System.Web.Mvc;

namespace PatientManagement.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default_router",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", Controller = "Dashboard", id = UrlParameter.Optional },
                new[] { "PatientManagement.Areas.Admin.Controllers" }
            );
        }
    }
}