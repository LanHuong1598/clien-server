using System.Web.Mvc;

namespace PatientManagement.Doctor.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Doctor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Doctor_default_router",
                "Doctor/{controller}/{action}/{id}",
                new { action = "Index", Controller = "Dashboard", id = UrlParameter.Optional },
                new[] { "PatientManagement.Areas.Doctor.Controllers" }
            );
        }
    }
}