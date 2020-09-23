using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class PrescriptionController : BaseController
    {
        // GET: Admin/Attachment
        private const string KeyElement = "Đơn thuốc";

        // GET: Admin/Event
        public ActionResult Index(Guid detailRecordId)
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            ViewBag.DetailRecordId = detailRecordId;

            ViewBag.BaseURL = "#";

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var medicines = workScope.Medicines.GetAll().ToList();
                ViewBag.Medicines = new SelectList(medicines, "Id", "Name");

                var listData = workScope.Prescriptions
                    .Include(x => x.DetailPrescription)
                    .Where(x => x.DetailRecordId == detailRecordId)
                    .ToList();

                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var detail = workScope.DetailPrescriptions.FirstOrDefault(x => x.Id == id);

                return detail == default ?
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
                            detail.Id,
                            detail.MedicineId,
                            detail.Amount,
                            detail.Unit,
                            detail.Note
                        }
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(DetailPrescription input, Prescription prescription, bool isEdit)
        {
            try
            {
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    if (isEdit)
                    {
                        var elm = workScope.DetailPrescriptions.Get(input.Id);

                        if (elm != null) //update
                        {
                            elm = input;
                            workScope.DetailPrescriptions.Put(elm, elm.Id);
                            workScope.Complete();

                            //var attachmentAssign = workScope.AttachmentAssigns
                            //    .FirstOrDefault(x =>
                            //        x.DetailRecordId == assign.DetailRecordId && x.AttachmentId == elm.Id);

                            //if (attachmentAssign == null)
                            //{
                            //    workScope.AttachmentAssigns.Add(new AttachmentAssign
                            //    {
                            //        Id = Guid.NewGuid(),
                            //        DetailRecordId = assign.DetailRecordId,
                            //        AttachmentId = elm.Id
                            //    });

                            //    workScope.Complete();
                            //}
                            return Json(new { status = true, mess = "Cập nhập thành công " });
                        }
                        else
                        {
                            return Json(new { status = false, mess = "Không tồn tại " + KeyElement });
                        }
                    }
                    else
                    {
                        input.Id = Guid.NewGuid();

                        workScope.DetailPrescriptions.Add(input);
                        workScope.Complete();

                        workScope.Prescriptions.Add(new Prescription
                        {
                            Id = Guid.NewGuid(),
                            DetailRecordId = prescription.DetailRecordId,
                            DetailPrescriptionId = input.Id,
                            CreatedBy = GetCurrentUser().FullName,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = GetCurrentUser().FullName
                        });

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
                    var elm = workScope.DetailPrescriptions.Get(id);
                    if (elm != null)
                    {
                        workScope.DetailPrescriptions.Remove(elm);
                        //del
                        var prescriptions = workScope.Prescriptions.Query(x => x.DetailPrescriptionId == elm.Id);
                        foreach (var prescription in prescriptions)
                        {
                            workScope.Prescriptions.Remove(prescription);
                        }
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