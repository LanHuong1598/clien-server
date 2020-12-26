using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;
using PatientManagement.Handler;
using PatientManagement.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BELibrary.Core.Utils;

namespace PatientManagement.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (!CookiesManage.Logined())
            {
                return RedirectToAction("Login", "Account");
            }

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var user = CookiesManage.GetUser();
                var patient = workScope.Patients.FirstOrDefault(x => x.Id == user.PatientId);

                if (patient != null)
                {
                    var record = workScope.Records.FirstOrDefault(x => x.Id == patient.RecordId);
                    record.Doctor = workScope.Doctors.FirstOrDefault(x => x.Id == record.DoctorId);
                    ViewBag.Record = record;
                    ViewBag.Patient = patient;

                    var detailRecord = workScope.DetailRecords
                        .Query(x => x.RecordId == record.Id)
                        .OrderByDescending(x => x.Process).ToList();
                    return View(detailRecord);
                }
                else
                {
                    return RedirectToAction("E404", "Home");
                }
            }
        }

        public ActionResult Edit()
        {
            if (!CookiesManage.Logined())
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var user = CookiesManage.GetUser();

                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    var account = workScope.Accounts.GetAll().Where(x => x.UserName.Trim().ToLower() == user.UserName.Trim().ToLower());
                    return View(account);
                }
            }
        }

        public ActionResult Login(string returnUrl = "")
        {
            if (CookiesManage.Logined())
            {
                return RedirectToAction("Index", "Account");
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
                var account = workScope.Accounts.ValidFeAccount(model.Username, model.Password);

                if (HttpContext.Request.Url != null)
                {
                    var host = HttpContext.Request.Url.Authority;
                    if (account != null)
                    {
                        //đăng nhập thành công
                        var cookieClient = account.UserName + "|" + host.ToLower() + "|" + account.Id;
                        var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                        var userCookie = new HttpCookie(CookiesKey.Client)
                        {
                            Value = decodeCookieClient,
                            Expires = DateTime.Now.AddDays(30)
                        };
                        HttpContext.Response.Cookies.Add(userCookie);
                        return Json(new { status = true, mess = "Đăng nhập thành công" });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Tên và mật khẩu không chính xác" });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Tên và mật khẩu không chính xác" });
                }
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Register(Account us, string rePassword)
        {
            if (us.Password != rePassword)
            {
                return Json(new { status = false, mess = "Mật khẩu không khớp" });
            }
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var account = workScope.Accounts.FirstOrDefault(x => x.UserName.ToLower() == us.UserName.ToLower());
                if (account == null)
                {
                    try
                    {
                        var passwordFactory = us.Password + VariableExtensions.KeyCrypto;
                        var passwordCrypto = CryptorEngine.Encrypt(passwordFactory, true);

                        us.Password = passwordCrypto;
                        us.Role = RoleKey.Patient;
                        us.LinkAvatar = us.Gender ? "/Content/images/team/2.png" : "/Content/images/team/3.png";
                        us.Id = Guid.NewGuid();
                        workScope.Accounts.Add(us);
                        workScope.Complete();

                        //Login luon
                        if (HttpContext.Request.Url != null)
                        {
                            var host = HttpContext.Request.Url.Authority;

                            var cookieClient = us.UserName + "|" + host.ToLower() + "|" + us.Id;
                            var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                            var userCookie = new HttpCookie(CookiesKey.Client)
                            {
                                Value = decodeCookieClient,
                                Expires = DateTime.Now.AddDays(30)
                            };
                            HttpContext.Response.Cookies.Add(userCookie);
                            return Json(new { status = true, mess = "Đăng ký thành công" });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Thêm không thành công" });
                        }
                    }
                    catch (Exception)
                    {
                        return Json(new { status = false, mess = "Thêm không thành công" });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Username không khả dụng" });
                }
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult Update(Account us, HttpPostedFileBase avataUpload)
        {
            if (!CookiesManage.Logined())
            {
                return Json(new { status = false, mess = "Chưa đăng nhập" });
            }
            var user = CookiesManage.GetUser();
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var account = workScope.Accounts.FirstOrDefault(x => x.UserName.ToLower() == user.UserName.ToLower());
                if (account != null)
                {
                    try
                    {
                        if (avataUpload?.FileName != null)
                        {
                            if (avataUpload.ContentLength >= FileKey.MaxLength)
                            {
                                return Json(new { status = false, mess = L.T("FileMaxLength") });
                            }
                            var splitFilename = avataUpload.FileName.Split('.');
                            if (splitFilename.Length > 1)
                            {
                                var fileExt = splitFilename[splitFilename.Length - 1];

                                //Check ext

                                if (FileKey.FileExtensionApprove().Any(x => x == fileExt))
                                {
                                    var slugName = StringHelper.ConvertToAlias(account.FullName);
                                    var fileName = slugName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + fileExt;
                                    var path = Path.Combine(Server.MapPath("~/FileUploads/images/avatas/"), fileName);
                                    avataUpload.SaveAs(path);
                                    us.LinkAvatar = "/FileUploads/images/avatas/" + fileName;
                                }
                                else
                                {
                                    return Json(new { status = false, mess = L.T("FileExtensionReject") });
                                }
                            }
                            else
                            {
                                return Json(new { status = false, mess = L.T("FileExtensionReject") });
                            }
                        }

                        us.Password = account.Password;
                        us.UserName = account.UserName;
                        us.Role = RoleKey.Patient;
                        us.Id = account.Id;

                        if (string.IsNullOrEmpty(us.LinkAvatar))
                        {
                            us.LinkAvatar = us.Gender ? "/Content/images/team/2.png" : "/Content/images/team/3.png";
                        }
                        account = us;
                        workScope.Accounts.Put(account, account.Id);
                        workScope.Complete();

                        //Đăng xuất
                        var nameCookie = Request.Cookies[CookiesKey.Client];
                        if (nameCookie != null)
                        {
                            var myCookie = new HttpCookie(CookiesKey.Client)
                            {
                                Expires = DateTime.Now.AddDays(-1d)
                            };
                            Response.Cookies.Add(myCookie);
                        }

                        //Login luon
                        if (HttpContext.Request.Url != null)
                        {
                            var host = HttpContext.Request.Url.Authority;

                            var cookieClient = account.UserName + "|" + host.ToLower() + "|" + account.Id;
                            var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                            var userCookie = new HttpCookie(CookiesKey.Client)
                            {
                                Value = decodeCookieClient,
                                Expires = DateTime.Now.AddDays(30)
                            };
                            HttpContext.Response.Cookies.Add(userCookie);
                            //RedirectToAction("Account", "Edit");
                            return Json(new { status = true, mess = "Cập nhật thành công" });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Cập nhật K thành công" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = false, mess = "Cập nhật không thành công", ex });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Tài khoản không khả dụng" });
                }
            }
        }

        [HttpPost]
        [ValidateInput(true)]
        public JsonResult UpdatePass(string oldPassword, string newPassword, string rePassword)
        {
            if (oldPassword == "" || newPassword == "" || rePassword == "")
            {
                return Json(new { status = false, mess = "Không được để trống" });
            }
            if (!CookiesManage.Logined())
            {
                return Json(new { status = false, mess = "Chưa đăng nhập" });
            }
            if (newPassword != rePassword)
            {
                return Json(new { status = false, mess = "Mật khẩu không khớp" });
            }
            var user = CookiesManage.GetUser();
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var account = workScope.Accounts.FirstOrDefault(x => x.UserName.ToLower() == user.UserName.ToLower());
                if (account != null)
                {
                    try
                    {
                        var passwordFactory = oldPassword + VariableExtensions.KeyCryptorClient;
                        var passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);

                        if (passwordCryptor == account.Password)
                        {
                            passwordFactory = newPassword + VariableExtensions.KeyCryptorClient;
                            passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);

                            account.Password = passwordCryptor;
                            workScope.Accounts.Put(account, account.Id);
                            workScope.Complete();

                            //Đăng xuất
                            var nameCookie = Request.Cookies[CookiesKey.Client];
                            if (nameCookie != null)
                            {
                                var myCookie = new HttpCookie(CookiesKey.Client)
                                {
                                    Expires = DateTime.Now.AddDays(-1d)
                                };
                                Response.Cookies.Add(myCookie);
                            }

                            //Login luon
                            if (HttpContext.Request.Url != null)
                            {
                                var host = HttpContext.Request.Url.Authority;

                                var cookieClient = account.UserName + "|" + host.ToLower() + "|" + account.Id;
                                var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                                var userCookie = new HttpCookie(CookiesKey.Client)
                                {
                                    Value = decodeCookieClient,
                                    Expires = DateTime.Now.AddDays(30)
                                };
                                HttpContext.Response.Cookies.Add(userCookie);
                                return Json(new { status = true, mess = "Cập nhật thành công" });
                            }
                            else
                            {
                                return Json(new { status = false, mess = "Cập nhật K thành công" });
                            }
                        }
                        else
                        {
                            return Json(new { status = false, mess = "mật khẩu cũ không đúng" });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { status = false, mess = "Cập nhật không thành công", ex });
                    }
                }
                else
                {
                    return Json(new { status = false, mess = "Tài khoản không khả dụng" });
                }
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var nameCookie = Request.Cookies[CookiesKey.Client];
            if (nameCookie == null) return RedirectToAction("Index", "Account");
            var myCookie = new HttpCookie(CookiesKey.Client)
            {
                Expires = DateTime.Now.AddDays(-1d)
            };
            Response.Cookies.Add(myCookie);
            return RedirectToAction("Index", "Account");
        }
    }
}