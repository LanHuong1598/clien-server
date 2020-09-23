using System;
using System.Linq;
using System.Web.Mvc;
using BELibrary.Core.Entity;
using BELibrary.DbContext;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.Element = "Hệ thống";
            ViewBag.Feature = "Bảng điều khiển";
            if (Request.Url != null) ViewBag.BaseURL = Request.Url.LocalPath;

            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var documents = workScope.Attachments.GetAll().ToList();
                var patients = workScope.Patients.GetAll().ToList();
                var prescriptions = workScope.Prescriptions.GetAll().ToList();
                var items = workScope.Items.GetAll().ToList();

                //
                ViewBag.DocumentCount = documents.Count;
                ViewBag.PatientCount = patients.Count;
                ViewBag.PrescriptionCount = prescriptions.Count;
                ViewBag.ItemCount = items.Count;

                var now = DateTime.Now;
                //
                ViewBag.DocumentTodayCount = documents.Count(x => x.ModifiedDate.Day == now.Day && x.ModifiedDate.Month == now.Month && x.ModifiedDate.Year == now.Year);
                ViewBag.DocumentMonthCount = documents.Count(x => x.ModifiedDate.Month == now.Month && x.ModifiedDate.Year == now.Year);

                ViewBag.PatientTodayCount = patients.Count(x => x.JoinDate.Day == now.Day && x.JoinDate.Month == now.Month && x.JoinDate.Year == now.Year);
                ViewBag.PatientMonthCount = patients.Count(x => x.JoinDate.Month == now.Month && x.JoinDate.Year == now.Year);

                ViewBag.PrescriptionTodayCount = prescriptions.Count(x => x.ModifiedDate.Day == now.Day && x.ModifiedDate.Month == now.Month && x.ModifiedDate.Year == now.Year);
                ViewBag.PrescriptionMonthCount = documents.Count(x => x.ModifiedDate.Month == now.Month && x.ModifiedDate.Year == now.Year);

                ViewBag.ItemTodayCount = items.Count(x => x.ModifiedDate.Day == now.Day && x.ModifiedDate.Month == now.Month && x.ModifiedDate.Year == now.Year);
                ViewBag.ItemMonthCount = items.Count(x => x.ModifiedDate.Month == now.Month && x.ModifiedDate.Year == now.Year);

                // new patient

                var patientsNew = workScope.Patients.Query(x => x.Status).OrderByDescending(x => x.JoinDate).Take(6).ToList();
                ViewBag.PatientsNew = patientsNew;

                var categories = workScope.Categories.GetAll().ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
            }

            return View();
        }
    }
}