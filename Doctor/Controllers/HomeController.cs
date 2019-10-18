using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doctor.Controllers
{
    public class HomeController : Controller
    {
        private DataContex _contex;

        public HomeController()
        {
            _contex = new DataContex();
        }

        protected override void Dispose(bool dispossing)
        {
            _contex.Dispose();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Doctor()
        {
            return this.View();
        }

        public JsonResult GetData()
        {
            var doctor = _contex.Doctorses.ToList();
            return Json(doctor,JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Contact()
        {
            return this.View();
        }

        public ActionResult FAQ()
        {
            return this.View();
        }
    }
}
