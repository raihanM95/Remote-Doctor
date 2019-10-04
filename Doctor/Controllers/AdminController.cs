using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doctor.Models;
using Doctor.ViewModel;
using System.Net;
using Newtonsoft.Json;
using System.Web.Security;

namespace Doctor.Controllers
{
    public class AdminController : Controller
    {
        // DbContex
        private DataContex _contex;

        public AdminController()
        {
            _contex = new DataContex();
        }

        protected override void Dispose(bool dispossing)
        {
            _contex.Dispose();
        }

        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            return View(this.GetCurrentUser());
        }
        
        [HttpGet]
        public ActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Login(Login login, string ReturnUrl)
        {
            bool Status = false;
            string Message = null;
            var admin = _contex.Admins.Where(d => d.Email == login.Email).FirstOrDefault();
            if (admin != null)
            {
                if (string.Compare(Crypto.Hash(login.Password), admin.Password) == 0
                    && admin.IsEmailVarified == true)
                {
                    Status = true;
                    Session["Admin"] = admin.Email;
                    int TimeOut = login.Remember ? 525600 : 20;
                    var tiket = new FormsAuthenticationTicket(login.Email, login.Remember, TimeOut);
                    string encrypts = FormsAuthentication.Encrypt(tiket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypts);
                    cookie.Expires = DateTime.Now.AddMinutes(TimeOut);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        Status = true;
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    Message = "Password is incorrect or Email is not Verified";
                }
            }
            else
            {
                Message = "This email is not registerd";
            }

            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("Admin");
            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        public Admin GetCurrentUser()
        {
            var id = User.Identity.Name;
            return this._contex.Admins.Single(a => a.Email == id);
        }

        [Authorize]
        public ActionResult Deshboard()
        {
            var CCordinators = this._contex.CCordinators.Count();
            var Doctors = this._contex.Doctorses.Count();
            var Patients = this._contex.Patients.Count();
            ViewBag.Users = CCordinators + Doctors + Patients;
            return this.PartialView();
        }
    }
}
