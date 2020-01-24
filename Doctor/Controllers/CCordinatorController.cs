using Doctor.Models;
using Doctor.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Doctor.Controllers
{
    public class CCordinatorController : Controller
    {
        // DbContex
        private DataContex _contex;

        public CCordinatorController()
        {
            _contex = new DataContex();
        }

        protected override void Dispose(bool dispossing)
        {
            _contex.Dispose();
        }

        // GET: CCordinator
        public ActionResult Index()
        {
            return View(this.CurrentUser());
        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(
            [Bind(Exclude = "IsEmailVerified,ActivationCode")]
            CCordinator cCordinator)
        {
            ModelState["Id"].Errors.Clear();
            string Message = null;
            bool Status = false;
            if (!ModelState.IsValid)
            {
                Message = "Invalid Request";
                return View();
            }
            else
            {
                if (cCordinator.Id == 0)
                {
                    if (IsEmailExist(cCordinator.CCordinatorEmail))
                    {
                        ModelState.AddModelError("EmailExist", "Email is already exist");
                        return View();
                    }

                    #region Generate Activation Code

                    cCordinator.ActivationCode = Guid.NewGuid();

                    #endregion

                    #region Password Hashing

                    cCordinator.CCordinatorPassword = Crypto.Hash(cCordinator.CCordinatorPassword);
                    cCordinator.CCordinatorConfirmPassword = Crypto.Hash(cCordinator.CCordinatorConfirmPassword);

                    #endregion

                    cCordinator.IsEmailVarified = false;

                    #region Save Data

                    _contex.CCordinators.Add(cCordinator);
                    _contex.SaveChanges();

                    #endregion

                    #region Send Email

                    var url = "/CCordinator/VarifyAccount/" + cCordinator.ActivationCode.ToString();
                    string link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
                    Email.SendVarificationEmail(cCordinator.CCordinatorEmail, link);
                    Message = "Registration successfully done, Check your Email";
                    Status = true;

                    #endregion
                }
            }

            ViewBag.Message = Message;
            ViewBag.Status = Status;

            return View();
        }

        [NonAction]
        public bool IsEmailExist(string Email)
        {
            var e = _contex.CCordinators.Where(a => a.CCordinatorEmail == Email).FirstOrDefault();
            return e != null;
        }

        [HttpGet]
        public ActionResult VarifyAccount(string id)
        {
            bool Status = false;
            _contex.Configuration.ValidateOnSaveEnabled = false;
            var cCordinator = _contex.CCordinators.Where(d => d.ActivationCode == new Guid(id)).FirstOrDefault();
            if (cCordinator != null && cCordinator.IsEmailVarified != true)
            {
                cCordinator.IsEmailVarified = true;

                _contex.SaveChanges();
                Status = true;
                ViewBag.Message = "Your account has been successfully activated!";
            }
            else if (cCordinator.IsEmailVarified == true)
            {
                ViewBag.Message = "This account already active";
            }
            else
            {
                ViewBag.Message = "Invalid Request";
            }

            ViewBag.Status = Status;
            return View();
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
            var cCordinator = _contex.CCordinators.Where(c => c.CCordinatorEmail == login.Email).FirstOrDefault();
            if (cCordinator != null)
            {
                if (string.Compare(Crypto.Hash(login.Password), cCordinator.CCordinatorPassword) == 0
                    && cCordinator.IsEmailVarified == true)
                {
                    Status = true;
                    Session["CCordinator"] = cCordinator.CCordinatorEmail;
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
                        return RedirectToAction("Index", "CCordinator");
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
            Session.Remove("CCordinator");
            return RedirectToAction("Index", "Home");
        }

        [NonAction]
        public CCordinator CurrentUser()
        {
            var id = User.Identity.Name;
            return this._contex.CCordinators.Single(a => a.CCordinatorEmail == id);
        }

        public ActionResult CCordinators()
        {
            var CCordinator = this._contex.CCordinators.ToList();

            var CView = new CCordinatorView
            {
                CCordinators = CCordinator
            };
            return PartialView(CView);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                int isExecuted = 0;

                CCordinator aCCordinator = this._contex.CCordinators.FirstOrDefault(dr => dr.Id == id);
                this._contex.CCordinators.Remove(aCCordinator);
                isExecuted = this._contex.SaveChanges();

                if (isExecuted > 0)
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("Index", "CCordinator");
            }
            catch
            {
                return RedirectToAction("Index", "CCordinator");
            }
        }

        [Authorize]
        public ActionResult Deshboard()
        {
            var user = this.CurrentUser();
            ViewBag.Appointment = this._contex.Appointments
                .Where(a => a.Status == false && a.CCEmail == user.CCordinatorEmail).Count();
            return this.PartialView();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Profile()
        {
            var email = User.Identity.Name;
            var cCordinator = this._contex.CCordinators.Where(c => c.CCordinatorEmail == email).FirstOrDefault();
            return this.PartialView("Profile", cCordinator);
        }

        [HttpPost]
        public ActionResult Update(CCordinator cCordinator)
        {
            var Cordinator = this._contex.CCordinators.Single(p => p.Id == cCordinator.Id);
            ModelState["CCordinatorPassword"].Errors.Clear();
            ModelState["CCordinatorConfirmPassword"].Errors.Clear();
            cCordinator.CCordinatorConfirmPassword = Cordinator.CCordinatorPassword;
            cCordinator.CCordinatorPassword = Cordinator.CCordinatorPassword;
            if (ModelState.IsValid)
            {
                if (cCordinator.CCordinatorImagefile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(cCordinator.CCordinatorImagefile.FileName);
                    string Extantion = Path.GetExtension(cCordinator.CCordinatorImagefile.FileName);
                    fileName = fileName + DateTime.Now.Year + Extantion;
                    cCordinator.CCordinatorImagePath = "/Image/ccordinators/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/ccordinators/"), fileName);
                    cCordinator.CCordinatorImagefile.SaveAs(fileName);
                }
                else
                {
                    cCordinator.CCordinatorImagePath = "/Image/user.jpg";
                }
                Cordinator.CCordinatorName = cCordinator.CCordinatorName;
                Cordinator.CCordinatorPassword = cCordinator.CCordinatorPassword;
                Cordinator.CCordinatorConfirmPassword = cCordinator.CCordinatorConfirmPassword;
                Cordinator.CCordinatorEmail = cCordinator.CCordinatorEmail;
                Cordinator.CCordinatorPhone = cCordinator.CCordinatorPhone;
                Cordinator.CCordinatorImagePath = cCordinator.CCordinatorImagePath;
                this._contex.SaveChanges();
            }
            return RedirectToAction("Index", "CCordinator");
        }

        public ActionResult Doctor()
        {
            return this.View();
        }

        public JsonResult GetAppointment()
        {
            var doctor = _contex.Doctorses.ToList();
            return Json(doctor, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Accept(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    int doc = Convert.ToInt32(id);
                    var doctor = this._contex.Doctorses.Single(d => d.Id == doc);
                    var appoint = new AppointmentView
                    {
                        Doctorses = doctor,
                        CCordinator = CurrentUser(),
                        Appointment = new Appointment()
                    };
                    return this.View(appoint);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
        }

        [HttpPost]
        public ActionResult Accept(Patient patient, int id, string ccEmail)
        {
            Appointment appointment = new Appointment();
            var aPatient = _contex.Patients.Where(p => p.PatientEmail == patient.PatientEmail).FirstOrDefault();
            if(aPatient != null)
            {
                appointment.CCEmail = ccEmail;
                appointment.PatientId = aPatient.Id;
                appointment.DoctorsId = id;
                appointment.Date = DateTime.Now;

                if (id != 0)
                {
                    this._contex.Appointments.Add(appointment);
                    this._contex.SaveChanges();
                    ViewBag.Status = true;
                    ViewBag.Message = "Appointment Completed";
                    return Redirect("~/ccordinator");
                }
                else
                {
                    ViewBag.Status = false;
                    ViewBag.Message = "Error !! try again";
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.Message = "This email is not registerd";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        // Appointment:View
        public ActionResult Appointment()
        {
            var email = this.CurrentUser().CCordinatorEmail;
            var appointments = this._contex.Appointments.OrderByDescending(p => p.Id).Where(c => c.CCEmail == email).Take(10)
                .ToList();

            return this.PartialView(appointments);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Apointment(int id)
        {
            var appointment = this._contex.Appointments.FirstOrDefault(a => a.Id == id);

            return this.View(appointment);
        }

        [HttpPost]
        public ActionResult Apointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                this._contex.Entry(appointment).State = EntityState.Modified;
                this._contex.SaveChanges();
                ViewBag.Status = true;
                ViewBag.Message = "Appointment Completed";
                return Redirect("~/ccordinator");
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.Message = "Error !! try again";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}
