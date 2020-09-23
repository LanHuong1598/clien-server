using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class AttachmentController : BaseController
    {
        // GET: Admin/Attachment
        private const string KeyElement = "Tệp đính kèm";

        // GET: Admin/Event
        public ActionResult Index(Guid detailRecordId)
        {
            ViewBag.Feature = "Danh sách";
            ViewBag.Element = KeyElement;
            ViewBag.DetailRecordId = detailRecordId;

            ViewBag.BaseURL = "#";

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var listData = workScope.AttachmentAssigns
                    .Include(x => x.Attachment).Where(x => x.DetailRecordId == detailRecordId).ToList();
                return View(listData);
            }
        }

        [HttpPost]
        public JsonResult GetJson(Guid? id)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var attachment = workScope.Attachments.FirstOrDefault(x => x.Id == id);

                return attachment == default ?
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
                            attachment.Id,
                            attachment.Name,
                            attachment.Url,
                            attachment.Type
                        }
                    });
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult CreateOrEdit(Attachment input, AttachmentAssign assign, bool isEdit)
        {
            try
            {
                using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
                {
                    if (isEdit)
                    {
                        var elm = workScope.Attachments.Get(input.Id);

                        if (elm != null) //update
                        {
                            input.CreatedBy = elm.CreatedBy;
                            input.CreatedDate = elm.CreatedDate;
                            input.ModifiedDate = DateTime.Now;
                            input.ModifiedBy = GetCurrentUser().FullName;
                            elm = input;
                            workScope.Attachments.Put(elm, elm.Id);
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
                        input.CreatedBy = GetCurrentUser().FullName;
                        input.CreatedDate = DateTime.Now;
                        input.ModifiedDate = DateTime.Now;
                        input.ModifiedBy = GetCurrentUser().FullName;

                        workScope.Attachments.Add(input);
                        workScope.Complete();

                        workScope.AttachmentAssigns.Add(new AttachmentAssign
                        {
                            Id = Guid.NewGuid(),
                            DetailRecordId = assign.DetailRecordId,
                            AttachmentId = input.Id
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
                    var elm = workScope.Attachments.Get(id);
                    if (elm != null)
                    {
                        workScope.Attachments.Remove(elm);
                        //del
                        var assigns = workScope.AttachmentAssigns.Query(x => x.AttachmentId == elm.Id);
                        foreach (var assign in assigns)
                        {
                            workScope.AttachmentAssigns.Remove(assign);
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