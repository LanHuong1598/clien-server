using BELibrary.Core.Entity;
using BELibrary.DbContext;
using System;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Entity;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class FacultyController : BaseController
    {
        private const string KeyElement = "Khoa";

        // GET: Admin/Event
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var listData = workScope.Faculties.GetAll().ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var faculty = workScope.Faculties.FirstOrDefault(x => x.Id == id);

                return faculty == default ?
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
                            faculty.Id,
                            faculty.Name
                        }
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Faculty input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                    {
                        var elm = workScope.Faculties.Get(input.Id);

                        if (elm != null) //update
                        {
                            elm = input;

                            workScope.Faculties.Put(elm, elm.Id);
                            workScope.Complete();

                            return Json(new { status = true, mess = "Cập nhập thành công " });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                }
                else
                {
                    using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                    {
                        input.Id = Guid.NewGuid();
                        workScope.Faculties.Add(input);
                        workScope.Complete();
                        return Json(new { status = true, mess = "Thêm thành công " + KeyElement });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, mess = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Del(Guid id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    var elm = workScope.Faculties.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.Faculties.Remove(elm);
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