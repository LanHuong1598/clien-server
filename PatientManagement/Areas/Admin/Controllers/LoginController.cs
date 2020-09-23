using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Utils;
using System;
using System.Web;
using System.Web.Mvc;
using BELibrary.Core.Utils;
using PatientManagement.Areas.Admin.Authorization;
using PatientManagement.Areas.Admin.Models;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Administrator/Login/

        [HttpGet]
        public ActionResult Index(string returnUrl = "")
        {
            if (CookiesManage.Logined())
            {
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult CheckLogin(LoginModel model)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var account = workScope.Accounts.ValidBEAccount(model.Username, model.Password);
                if (account != null)
                {
                    if (HttpContext.Request.Url != null)
                    {
                        var host = HttpContext.Request.Url.Authority;

                        //đăng nhập thành công
                        var cookieClient = account.UserName + "|" + host.ToLower() + "|" + account.Id;
                        var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                        var userCookie = new HttpCookie(CookiesKey.Admin)
                        {
                            Value = decodeCookieClient,
                            Expires = DateTime.Now.AddDays(30)
                        };
                        HttpContext.Response.Cookies.Add(userCookie);
                        return Json(new { status = true, mess = "Đăng nhập thành công" });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Đăng nhập Không thành công" });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Tên và mật khẩu không chính xác" });
                }
            }
        }

        //[HttpGet]
        //[ValidateInput(true)]
        //public ActionResult GetPassWord()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateInput(true)]
        //public ActionResult GetPassWord(GetPassword model)
        //{
        //    using (var db = new MrLongToeicEntities())
        //    {
        //        User detailUser =
        //            db.Users.FirstOrDefault(a => a.Email == model.Email && a.UserName == model.UserName);
        //        if (Convert.ToDateTime(CurrentSession.LockUser) > DateTime.Now)
        //        {
        //            DateTime dateBlog = Convert.ToDateTime(CurrentSession.LockUser);
        //            int minuteLock = dateBlog.Minute + (dateBlog.Hour*60) - DateTime.Now.Minute - (DateTime.Now.Hour*60);
        //            ModelState.AddModelError("Email",
        //                "Bạn đã nhập quá nhiều lần quy định, xin vui lòng quay lại sau " + minuteLock + " Phút.");
        //            return View();
        //        }
        //        if (detailUser == null)
        //        {
        //            if (TempData["Count"] == null)
        //            {
        //                TempData["Count"] = 1;
        //                TempData.Keep("Count");
        //            }
        //            else
        //            {
        //                TempData["Count"] = int.Parse(TempData["Count"].ToString()) + 1;
        //                TempData.Keep("Count");
        //            }
        //            if (int.Parse(TempData["Count"].ToString()) == 5)
        //            {
        //                DateTime dateBlog = DateTime.Now.AddMinutes(1);
        //                CurrentSession.LockUser = dateBlog;
        //                int minuteLock = dateBlog.Minute + (dateBlog.Hour*60) - DateTime.Now.Minute -
        //                                 (DateTime.Now.Hour*60);
        //                TempData.Remove("Count");
        //                ModelState.AddModelError("Email",
        //                    "Bạn đã nhập quá nhiều lần quy định, xin vui lòng quay lại sau " + minuteLock + " phút.");
        //                return View();
        //            }
        //            ModelState.AddModelError("Email",
        //                "Email hoặc tên người dùng là không chính xác, bạn còn " +
        //                (5 - int.Parse(TempData["Count"].ToString())) + " lần nhập");

        //            return View();
        //        }
        //        string content =
        //            System.IO.File.ReadAllText(Server.MapPath("/Areas/Admin/lib/Forgot_Password.html"));

        //        content = content.Replace("{{Password}}", CryptorEngine.Decrypt(detailUser.PasswordOld, true));

        //        MailHelper.SendMail(detailUser.Email, "Lấy lại mật khẩu", content);

        //        ViewBag.Messeages = "Vui lòng đăng nhập vào địa chỉ email: " + model.Email + " để lấy lại mật khẩu.";
        //        return View();
        //    }
        //}

        [HttpGet]
        public ActionResult Logout()
        {
            var nameCookie = Request.Cookies[CookiesKey.Admin];
            if (nameCookie == null) return RedirectToAction("Index");
            var newCookie = new HttpCookie(CookiesKey.Admin)
            {
                Expires = DateTime.Now.AddDays(-1d)
            };
            Response.Cookies.Add(newCookie);
            return RedirectToAction("Index");
        }
    }
}