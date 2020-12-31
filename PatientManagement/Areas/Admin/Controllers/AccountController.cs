using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BELibrary.Core.Entity;
using BELibrary.Core.Utils;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;
using PatientManagement.Areas.Admin.Authorization;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Admin/Course
        private readonly string KeyElement = "Tài khoản";

        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var lstRole = RoleKey.GetDic();
                ViewBag.Roles = new SelectList(lstRole, "Value", "Text");

                List<Patient> patients = new List<Patient>();
                patients = new PatientManagementDbContext().Database.SqlQuery<Patient>("exec getPatient").ToList();


                ViewBag.Patients = new SelectList(patients, "Id", "FullName");

                List<BELibrary.Entity.Doctor> doctors = new List<BELibrary.Entity.Doctor>();
                doctors = new PatientManagementDbContext().Database.SqlQuery<BELibrary.Entity.Doctor>("exec getDoctor").ToList();

                ViewBag.Doctors = new SelectList(doctors, "Id", "Name");

                var listData = workScope.Accounts.GetAll().ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var account = workScope.Accounts.FirstOrDefault(x => x.Id == id);

                return account == default ?
                    Json(new
                    {
                        status = false,
                        mess = "Có lỗi xảy ra: "
                    }) :
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = new
                        {
                            account.Id,
                            account.FullName,
                            account.PatientId,
                            account.DoctorId,
                            account.Phone,
                            account.UserName,
                            account.Gender,
                            account.Role
                        }
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Account input, bool isEdit, string oldPassword, string rePassword)
        {
            try
            {
                if (isEdit) //update
                {
                    using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                    {
                        var elm = workScope.Accounts.Get(input.Id);

                        if (elm != null) //update
                        {
                            //xu ly password
                            if (!string.IsNullOrEmpty(input.Password) || oldPassword != "")
                            {
                                if (oldPassword == "" || input.Password == "" || rePassword == "")
                                {
                                    return Json(new { status = false, mess = "Không được để trống" });
                                }
                                if (!CookiesManage.Logined())
                                {
                                    return Json(new { status = false, mess = "Chưa đăng nhập" });
                                }
                                if (input.Password != rePassword)
                                {
                                    return Json(new { status = false, mess = "Mật khẩu không khớp" });
                                }

                                var passwordFactory = input.Password + VariableExtensions.KeyCrypto;
                                var passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);
                                input.Password = passwordCryptor;
                            }
                            else
                            {
                                input.Password = elm.Password;
                            }

                            input.UserName = elm.UserName;

                            if (input.Role == RoleKey.Admin)
                            {
                                input.PatientId = null;
                                input.DoctorId = null;
                            }
                            else if (input.Role == RoleKey.Doctor)
                            {
                                input.PatientId = null;
                            }
                            else if (input.Role == RoleKey.Patient)
                            {
                                input.DoctorId = null;
                            }
                            elm = input;

                            workScope.Accounts.Put(elm, elm.Id);
                            workScope.Complete();

                            if (input.UserName != GetCurrentUser().UserName)
                                return Json(new { status = true, mess = "Cập nhập thành công " });
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

                                var cookieClient = elm.UserName + "|" + host.ToLower() + "|" + elm.Id;
                                var decodeCookieClient = CryptorEngine.Encrypt(cookieClient, true);
                                var userCookie = new HttpCookie(CookiesKey.Client)
                                {
                                    Value = decodeCookieClient,
                                    Expires = DateTime.Now.AddDays(30)
                                };
                                HttpContext.Response.Cookies.Add(userCookie);
                            }
                            else
                            {
                                return Json(new { status = false, mess = "Lỗi" });
                            }
                            return Json(new { status = true, mess = "Cập nhập thành công " });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                }
                else //Thêm mới
                {
                    using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                    {
                        if (input.Password != rePassword)
                        {
                            return Json(new { status = false, mess = "Mật khẩu không khớp" });
                        }

                        var elm = workScope.Accounts.Query(x => x.UserName.ToLower() == input.UserName.ToLower()).Any();
                        if (elm)
                        {
                            return Json(new { status = false, mess = "Tên đăng nhập đã tồn tại" });
                        }

                        var passwordFactory = input.Password + VariableExtensions.KeyCrypto;
                        var passwordCrypto = CryptorEngine.Encrypt(passwordFactory, true);

                        input.Password = passwordCrypto;
                        input.Id = Guid.NewGuid();

                        if (input.Role == RoleKey.Admin)
                        {
                            input.PatientId = null;
                            input.DoctorId = null;
                        }
                        else if (input.Role == RoleKey.Doctor)
                        {
                            input.PatientId = null;
                        }
                        else if (input.Role == RoleKey.Patient)
                        {
                            input.DoctorId = null;
                        }
                        workScope.Accounts.Add(input);
                        workScope.Complete();
                    }
                    return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    mess = "Có lỗi xảy ra: " + ex.Message
                });
            }
        }

        [HttpPost]
        public JsonResult Del(int id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    var elm = workScope.Accounts.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.Accounts.Remove(elm);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Xóa thành công " + KeyElement });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                    }
                }
            }
            catch
            {
                return Json(new { status = false, mess = "Thất bại" });
            }
        }
    }
}