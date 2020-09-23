using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Core.Utils;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class RecordController : BaseController
    {
        private readonly string KeyElement = "Bệnh án";

        // GET: Admin/Record
        public ActionResult Index(Guid patientId)
        {
            ViewBag.Feature = "Thêm mới";
            ViewBag.Element = KeyElement;
            if (Request.Url != null)
                ViewBag.BaseURL = Request.Url;
            try
            {
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    var patient = workScope.Patients.FirstOrDefault(x => x.Id == patientId);
                    if (patient != null)
                    {
                        var record = workScope.Records.FirstOrDefault(x => x.Id == patient.RecordId);
                        if (record == null)
                        {
                            record = new Record
                            {
                                Id = Guid.NewGuid(),
                                CreatedBy = GetCurrentUser().FullName,
                                ModifiedBy = GetCurrentUser().FullName,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                Note = "",
                                Result = ""
                            };
                            patient.RecordId = record.Id;
                            workScope.Records.Add(record);
                            workScope.Complete();
                        }
                        ViewBag.Record = record;
                        ViewBag.Patient = patient;

                        var doctors = workScope.Doctors.GetAll().ToList();
                        ViewBag.Doctors = new SelectList(doctors, "Id", "Name");

                        var faculties = workScope.Faculties.GetAll().ToList();
                        ViewBag.Faculties = new SelectList(faculties, "Id", "Name");

                        var lstStatus = StatusRecord.GetDic();
                        ViewBag.ListStatus = new SelectList(lstStatus, "Value", "Text");

                        var detailRecord = workScope.DetailRecords.Query(x => x.RecordId == record.Id).OrderByDescending(x => x.Process).ToList();
                        return View(detailRecord);
                    }
                    else
                    {
                        return RedirectToAction("Create", "Patient");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid id)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var detailRecord = workScope.DetailRecords.FirstOrDefault(x => x.Id == id);

                return detailRecord == null ?
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
                            detailRecord.Id,
                            detailRecord.DiseaseName,
                            detailRecord.FacultyId,
                            detailRecord.DoctorId,
                            detailRecord.Note,
                            detailRecord.Status,
                            detailRecord.Result,
                            detailRecord.Process
                        }
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(DetailRecord input, Guid detailDoctorId, bool isEdit)
        {
            try
            {
                if (isEdit) //update
                {
                    using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                    {
                        var elm = workScope.DetailRecords.Get(input.Id);

                        if (elm != null) //update
                        {
                            //Che bien du lieu

                            elm = input;
                            elm.DoctorId = detailDoctorId;

                            workScope.DetailRecords.Put(elm, elm.Id);
                            workScope.Complete();

                            //attachments handle

                            return Json(new
                            {
                                status = true,
                                mess = "Cập nhập thành công ",
                                data = new
                                {
                                    detailRecordId = input.Id
                                }
                            });
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
                        //Che bien du lieu
                        input.Id = Guid.NewGuid();
                        input.DoctorId = detailDoctorId;

                        workScope.DetailRecords.Add(input);
                        workScope.Complete();
                    }
                    return Json(new
                    {
                        status = true,
                        mess = "Thêm thành công " + KeyElement,
                        data = new
                        {
                            detailRecordId = input.Id
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

        [HttpPost, ValidateInput(false)]
        public JsonResult UpdateRecord(Record input)
        {
            try
            {
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    var elm = workScope.Records.Get(input.Id);

                    if (elm != null) //update
                    {
                        //Che bien du lieu
                        input.CreatedBy = elm.CreatedBy;
                        input.CreatedDate = elm.CreatedDate;
                        input.ModifiedBy = GetCurrentUser().FullName;
                        input.ModifiedDate = DateTime.Now;

                        elm = input;

                        workScope.Records.Put(elm, elm.Id);
                        workScope.Complete();

                        return Json(new { status = true, mess = "Cập nhập thành công " });
                    }
                    else
                    {
                        return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                    }
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
                    var elm = workScope.Medicines.Get(id);
                    if (elm != null)
                    {
                        //del
                        workScope.Medicines.Remove(elm);
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