using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BELibrary.Core.Entity;
using BELibrary.Core.Utils;
using BELibrary.DbContext;

namespace PatientManagement.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var categories = workScope.Categories.GetAll().ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View();
            }
        }

        [HttpPost]
        public JsonResult GetRegByYear(int year)
        {
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var patients = workScope.Patients.GetAll();

                var date = new DateTime(year, 1, 1);
                var months = Enumerable.Range(0, 11)
                    .Select(x => new
                    {
                        month = date.AddMonths(x).Month,
                        year = date.AddMonths(x).Year
                    }).ToList();

                var dataPerYearAndMonth =
                    months.GroupJoin(patients,
                        m => new { m.month, m.year },
                        patient => new
                        {
                            month = patient.JoinDate.Month,
                            year = patient.JoinDate.Year
                        },
                        (p, g) => new
                        {
                            month = "Tháng " + p.month,
                            p.year,
                            count = g.Count()
                        });

                return
                    Json(new
                    {
                        status = true,
                        mess = "Thành công ",
                        data = dataPerYearAndMonth.ToList()
                    });
            }
        }

        [HttpPost]
        public JsonResult GetItemByCategory(Guid? categoryId)
        {
            if (!categoryId.HasValue)
            {
                return
                    Json(new
                    {
                        status = false,
                        mess = "Danh mục không tồn tại"
                    });
            }
            using (var workScope = new UnitOfWork(new PatientManagementDbContext()))
            {
                var amountItem = workScope.Items.Query(x => x.CategoryId == categoryId).Sum(item => item.Amount);

                var supplies = workScope.MedicalSupplies.Include(x => x.Item).Where(x => x.Item.CategoryId == categoryId).ToList();

                var hireCount = supplies.Where(x => x.Status == StatusMedical.Hired).Sum(x => x.Amount);
                var availabilityCount = supplies.Where(x => x.Status == StatusMedical.Availability).Sum(x => x.Amount);
                var expiredCount = supplies.Where(x => x.Status == StatusMedical.Expired).Sum(x => x.Amount);
                var unavailableCount = supplies.Where(x => x.Status == StatusMedical.Unavailable).Sum(x => x.Amount);
                var maintenanceCount = supplies.Where(x => x.Status == StatusMedical.Maintenance).Sum(x => x.Amount);

                var availabilityItem = amountItem - hireCount - expiredCount - unavailableCount - maintenanceCount;

                return
                    Json(new
                    {
                        status = true,
                        mess = "Thành công ",
                        data = new[]
                        {
                            new
                            {
                                label = "Đã sử dụng", value = hireCount
                            },
                            new
                            {
                                label = "Khả dụng", value = availabilityItem
                            },
                            new
                            {
                                label = "Không khả dụng", value = unavailableCount
                            },
                            new
                            {
                                label = "Bảo trì", value = maintenanceCount
                            },
                            new
                            {
                                label = "Hết Hạn", value = expiredCount
                            }
                        }
                    });
            }
        }
    }
}