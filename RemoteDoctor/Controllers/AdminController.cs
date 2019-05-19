using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RemoteDoctor.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Register // by system admin
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // GET: Admin/Login // by admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // GET: Admin/ForgotPassword // by admin
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
    }
}