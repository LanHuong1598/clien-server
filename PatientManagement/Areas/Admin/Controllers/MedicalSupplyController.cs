using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Core.Utils;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class MedicalSupplyController : BaseController
    {
        // GET: Admin/Attachment
        private const string KeyElement = "Cung cấp vật tư";

        // GET: Admin/Event
        public ActionResult Index(Guid patientId)
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            ViewBag.PatientId = patientId;

            ViewBag.BaseURL = "#";

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var listData = workScope.MedicalSupplies
                    .Include(x => x.Item).Where(x => x.PatientId == patientId).ToList();

                var items = workScope.Items.GetAll().ToList();
                ViewBag.Items = new SelectList(items, "Id", "Name");

                var lstStatus = StatusMedical.GetDic();
                ViewBag.ListStatus = new SelectList(lstStatus, "Value", "Text");
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var medicalSupply = workScope.MedicalSupplies.FirstOrDefault(x => x.Id == id);

                return medicalSupply == default ?
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
                            medicalSupply.Id,
                            medicalSupply.Amount,
                            medicalSupply.ItemId,
                            DateHide = medicalSupply.DateOfHire.ToString("g"),
                            medicalSupply.Status
                        }
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(MedicalSupply input, AttachmentAssign assign, bool isEdit)
        {
            try
            {
                if (input.Amount <= 0)
                {
                    return Json(new { status = false, mess = "Lỗi số lượng" });
                }
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    var item = workScope.Items.FirstOrDefault(x => x.Id == input.ItemId);
                    var medicalSupplies = workScope.MedicalSupplies.Query(x => x.ItemId == input.ItemId).ToList();

                    var hireCount = medicalSupplies.Where(x => x.Status == StatusMedical.Hired).Sum(x => x.Amount);
                    var availabilityCount = medicalSupplies.Where(x => x.Status == StatusMedical.Availability).Sum(x => x.Amount);
                    var expiredCount = medicalSupplies.Where(x => x.Status == StatusMedical.Expired).Sum(x => x.Amount);
                    var unavailableCount = medicalSupplies.Where(x => x.Status == StatusMedical.Unavailable).Sum(x => x.Amount);
                    var maintenanceCount = medicalSupplies.Where(x => x.Status == StatusMedical.Maintenance).Sum(x => x.Amount);

                    var availabilityItem = item.Amount - hireCount - expiredCount - unavailableCount - maintenanceCount;

                    if (availabilityItem < 0)
                    {
                        return Json(new { status = false, mess = "Lỗi, dữ liệu âm" + KeyElement });
                    }

                    if (input.Amount > availabilityItem)
                    {
                        return Json(new { status = false, mess = $"Lỗi, kho đã hết" + KeyElement });
                    }
                    if (isEdit)
                    {
                        var elm = workScope.MedicalSupplies.Get(input.Id);

                        if (elm != null) //update
                        {
                            elm = input;
                            workScope.MedicalSupplies.Put(elm, elm.Id);
                            workScope.Complete();

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

                        workScope.MedicalSupplies.Add(input);
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
                    var elm = workScope.MedicalSupplies.Get(id);
                    if (elm != null)
                    {
                        workScope.MedicalSupplies.Remove(elm);
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