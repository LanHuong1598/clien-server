using System;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Core.Entity;
using BELibrary.Core.Utils;
using BELibrary.DbContext;
using BELibrary.Entity;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class PatientController : BaseController
    {
        // GET: Admin/Patient
        private readonly string KeyElement = "Bệnh nhân";

        public ActionResult Index()
        {
            ViewBag.Feature = "Bảng điều khiển";
            ViewBag.Element = KeyElement;
            ViewBag.BaseURL = "/Admin/Patient";
            return View();
        }

        public ActionResult All(string patientCode, string indentificationCardId, string fullName)
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            ViewBag.BaseURL = "/Admin/Patient";

            if (patientCode == "")
            {
                patientCode = null;
            }
            if (indentificationCardId == "")
            {
                indentificationCardId = null;
            }
            if (fullName == "")
            {
                fullName = null;
            }

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var listData = workScope.Patients.Query(x => x.Status).OrderByDescending(x => x.JoinDate).ToList();

                //var data = listData.Where(x => x.JoinDate > new DateTime(2020, 5, 1) && x.JoinDate < new DateTime(2020, 6, 1));
                //var r = new Random();
                //foreach (var p in data)
                //{
                //    int rInt = r.Next(-10, 0);
                //    if (rInt % 2 != 0)
                //        p.JoinDate = DateTime.Now.AddMonths(rInt);
                //}
                //workScope.Complete();

                var q = from mt in listData
                        where (!string.IsNullOrEmpty(patientCode) && mt.PatientCode.ToLower().Contains(patientCode.ToLower()))
                              || (!string.IsNullOrEmpty(indentificationCardId) && mt.IndentificationCardId.ToLower().Contains(indentificationCardId.ToLower()))
                              || (!string.IsNullOrEmpty(fullName) && mt.FullName.ToLower().Contains(fullName.ToLower()))
                        select mt;
                if (patientCode == null && indentificationCardId == null && fullName == null)
                {
                    return View(listData);
                }
                return View(q.OrderByDescending(x => x.JoinDate).ToList());
            }
        }

        public ActionResult Create()
        {
            ViewBag.Feature = "Thêm mới";
            ViewBag.Element = KeyElement;

            if (Request.Url != null)
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));

            string code;
            // Create patient code
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var patient = workScope.Patients.GetAll().OrderByDescending(x => x.JoinDate).FirstOrDefault();
                if (patient != null)
                {
                    var codeSplit = patient.PatientCode.Split('-');
                    code = Common.Prefix + (int.Parse(codeSplit[1]) + 1);
                }
                else
                {
                    code = Common.Prefix + "1";
                }
            }

            ViewBag.Code = code;
            ViewBag.isEdit = false;
            return View();
        }

        public ActionResult Update(Guid id)
        {
            ViewBag.isEdit = true;
            ViewBag.Feature = "Cập nhật";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
            {
                ViewBag.BaseURL = string.Join("", Request.Url.Segments.Take(Request.Url.Segments.Length - 1));
            }

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var patient = workScope.Patients
                    .FirstOrDefault(x => x.Id == id);

                if (patient != null)
                {
                    return View("Create", patient);
                }
                else
                {
                    return RedirectToAction("Create", "Patient");
                }
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Patient input, bool isEdit)
        {
            try
            {
                if (isEdit) //update
                {
                    using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                    {
                        var elm = workScope.Patients.Get(input.Id);
                        if (elm != null) //update
                        {
                            input.Status = true;
                            input.PatientCode = elm.PatientCode;
 			    input.RecordId = elm.RecordId;
                            elm = input;

                            workScope.Patients.Put(elm, elm.Id);
                            workScope.Complete();
                            return Json(new
                            {
                                status = true,
                                mess = "Cập nhập thành công ",
                                data = new
                                {
                                    input.Id
                                }
                            });
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
                        string code;

                        var patient = workScope.Patients.GetAll().OrderByDescending(x => x.JoinDate).FirstOrDefault();
                        if (patient != null)
                        {
                            var codeSplit = patient.PatientCode.Split('-');
                            code = Common.Prefix + (int.Parse(codeSplit[1]) + 1);
                        }
                        else
                        {
                            code = Common.Prefix + "1";
                        }

                        input.Id = Guid.NewGuid();
                        input.PatientCode = code;
                        input.JoinDate = DateTime.Now;
                        input.Status = true;

                        workScope.Patients.Add(input);

                        workScope.Complete();
                    }

                    return Json(new
                    {
                        status = true,
                        mess = "Thêm thành công " + KeyElement,
                        data = new
                        {
                            input.Id
                        }
                    });
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
        public JsonResult Del(Guid id)
        {
            try
            {
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    var elm = workScope.Patients.Get(id);
                    if (elm != null)
                    {
                        //del Patient
                        elm.Status = false;

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