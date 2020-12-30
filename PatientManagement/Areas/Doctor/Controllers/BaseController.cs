using PatientManagement.Areas.Doctor.Authorization;
using System.Web.Mvc;

namespace PatientManagement.Areas.Doctor.Controllers
{
    public class BaseController : Controller
    {
        // GET: /Administrator/Base/
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CookiesManage.Logined())
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result =
                new RedirectResult(string.Concat("~/Doctor/Login/Index", "?ReturnUrl=", returnUrl));
            }
            base.OnActionExecuting(filterContext);
        }

        public static BELibrary.Entity.Account GetCurrentUser()
        {
            return CookiesManage.GetUser();
        }
    }
}