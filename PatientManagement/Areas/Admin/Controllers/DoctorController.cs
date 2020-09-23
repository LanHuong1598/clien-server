using BELibrary.Core.Entity;
using BELibrary.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Entity;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class DoctorController : BaseController
    {
        private const string KeyElement = "Bác sĩ";

        // GET: Admin/Event
        public ActionResult Index()
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;

            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var listData = workScope.Doctors.Include(x => x.Faculty).ToList();

                var faculties = workScope.Faculties.GetAll().ToList();
                ViewBag.Faculties = new SelectList(faculties, "Id", "Name");

                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var doctor = workScope.Doctors.FirstOrDefault(x => x.Id == id);

                return doctor == default ?
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
                            doctor.Id,
                            doctor.Name,
                            doctor.Address,
                            doctor.Email,
                            doctor.Phone,
                            doctor.FacultyId,
                            doctor.Gender
                        }
                    });
            }
        }

        [HttpPost]
        public JsonResult GetDoctors(Guid? facultyId)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var lst = facultyId.HasValue
                    ? workScope.Doctors.Query(x => x.FacultyId == facultyId).ToList()
                    : workScope.Doctors.GetAll().ToList();

                return
                    Json(new
                    {
                        status = true,
                        mess = "Lấy thành công " + KeyElement,
                        data = lst.Select(x => new
                        {
                            x.Id,
                            x.Name
                        })
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Doctor input, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                    {
                        var elm = workScope.Doctors.Get(input.Id);

                        if (elm != null) //update
                        {
                            elm = input;

                            workScope.Doctors.Put(elm, elm.Id);
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
                        workScope.Doctors.Add(input);
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
                    var elm = workScope.Doctors.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.Doctors.Remove(elm);
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