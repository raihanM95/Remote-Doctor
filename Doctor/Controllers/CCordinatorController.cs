using Doctor.Models;
using Doctor.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            return PartialView();
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
                    cCordinator.CCordinatorImagePath = "/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
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
    }
}