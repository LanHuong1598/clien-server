using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PatientManagement.Areas.Admin.Authorization
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 1. Admin
        /// 2. Employee
        /// 3. Customer
        /// </summary>
        public int Role { set; get; }

        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    tblUser user = (tblUser)httpContext.Session["admin"];
        //    if (user != null)
        //    {
        //        if (this.Role == user.roleId || user.roleId == RoleKey.Admin)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var TempData = filterContext.Controller.TempData;
            TempData["Messages"] = "Bạn không có quyền truy cập mục này";
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "action", "E401" },
                    { "controller", "Error" },
                    { "Area", "" }
                });
        }
    }
}