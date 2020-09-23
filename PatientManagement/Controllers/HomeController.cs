using AutoMapper;
using BELibrary.Core.Entity;
using BELibrary.DbContext;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PatientManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var test = dbContext.News.FirstOrDefault();
            //var testview = Mapper.Map<NewsViewModel>(test);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page. ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page. ";

            return View();
        }

        public ActionResult E404()
        {
            ViewBag.Message = "Your contact page. ";

            return View();
        }
    }
}