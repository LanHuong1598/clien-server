using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BELibrary.Core.Entity;
using BELibrary.DbContext;

namespace PatientManagement.Controllers
{
    public class RecordController : BaseController
    {
        // GET: Record
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Account");
        }

        public ActionResult Attachment(Guid detailRecordId)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                //Check isRecord of current user
                var listData = workScope.AttachmentAssigns
                    .Include(x => x.Attachment).Where(x => x.DetailRecordId == detailRecordId).ToList();
                return View(listData);
            }
        }

        public ActionResult Prescription(Guid detailRecordId)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                //Check isRecord of current user
                var listData = workScope.Prescriptions.Include(x => x.DetailPrescription.Medicine)
                    .Where(x => x.DetailRecordId == detailRecordId).ToList();
                return View(listData);
            }
        }
    }
}