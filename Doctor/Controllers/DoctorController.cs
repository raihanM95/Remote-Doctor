using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doctor.Models;
using Doctor.ViewModel;
using System.Net;
using Newtonsoft.Json;
using System.Web.Security;
using System.Data.Entity;

namespace Doctor.Controllers
{
    public class DoctorController : Controller
    {
        // DbContex
        private DataContex _contex;

        public DoctorController()
        {
            _contex = new DataContex();
        }

        protected override void Dispose(bool dispossing)
        {
            _contex.Dispose();
        }

        // GET: Doctor
        public ActionResult Index()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                return View(GetCurrentDoctor());
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        [HttpGet]
        public ActionResult New()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor == null)
            {
                ViewBag.Message = null;
                ViewBag.Status = false;

                var department = this._contex.Departments.ToList();
                var doctorview = new DoctorView { Doctors = new Doctors(), Departments = department, };
                return this.View(doctorview);
            }
            else
            {
                return RedirectToAction("Index", "Doctor");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(
            [Bind(Exclude = "IsEmailVerified,ActivationCode")]
            Doctors doctors)
        {
            bool Status = false;
            string Message = null;

            var ViewModel = new DoctorView { Doctors = new Doctors(), Departments = _contex.Departments.ToList(), };
            if (!ModelState.IsValid)
            {
                Message = "Invalid Request";
                return this.View("New", ViewModel);
            }
            else
            {
                if (doctors.Id == 0)
                {
                    if (this.IsEmailExist(doctors.DoctorEmail))
                    {
                        this.ModelState.AddModelError("EmailExist", "Email is already exist");
                        return this.View("New", ViewModel);
                    }

                    #region Generate Activation Code

                    doctors.ActivationCode = Guid.NewGuid();

                    #endregion

                    #region Password Hashing

                    doctors.DoctorPassword = Crypto.Hash(doctors.DoctorPassword);
                    doctors.DoctorConfirmPassword = Crypto.Hash(doctors.DoctorConfirmPassword);

                    #endregion

                    doctors.IsEmailVarified = false;

                    #region Image file upload

                    string fileName = Path.GetFileName(doctors.DoctorImagefile.FileName);
                    doctors.DoctorImagePath = "/Image/doctors/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/doctors/"), fileName);

                    #endregion

                    #region Save Data

                    doctors.DoctorImagefile.SaveAs(fileName);
                    this._contex.Doctorses.Add(doctors);
                    this._contex.SaveChanges();

                    #endregion

                    #region Send Email

                    var url = "/Doctor/VarifyAccount/" + doctors.ActivationCode.ToString();
                    var requestUrl = this.Request.Url;
                    if (requestUrl != null)
                    {
                        string link = requestUrl.AbsoluteUri.Replace(requestUrl.PathAndQuery, url);
                        Email.SendVarificationEmail(doctors.DoctorEmail, link);
                    }

                    Message = "Registration successfully done, Check your Email";
                    Status = true;

                    this.ViewBag.Message = Message;
                    this.ViewBag.Status = true;

                    #endregion
                }
                else
                {
                    var doctorInDb = this._contex.Doctorses.Single(d => d.Id == doctors.Id);
                    doctorInDb.DoctorName = doctors.DoctorName;
                    doctorInDb.DoctorBirthDate = doctors.DoctorBirthDate;
                    doctorInDb.DepartmentId = doctors.DepartmentId;
                    this._contex.SaveChanges();
                }

                return View("New", ViewModel);
            }

            this.ViewBag.Message = Message;
            this.ViewBag.Status = Status;

            return this.View();
        }

        [NonAction]
        public bool IsEmailExist(string Email)
        {
            var e = this._contex.Doctorses.FirstOrDefault(a => a.DoctorEmail == Email);
            return e != null;
        }

        public ActionResult VarifyAccount(string id)
        {
            bool Status = false;
            this._contex.Configuration.ValidateOnSaveEnabled = false;
            var patient = this._contex.Doctorses.Where(d => d.ActivationCode == new Guid(id)).FirstOrDefault();

            if (patient != null && patient.IsEmailVarified != true)
            {
                patient.IsEmailVarified = true;

                // doctor.ActivationCode = null;
                this._contex.SaveChanges();
                Status = true;
                this.ViewBag.Message = "Your account has been successfully activated!";
            }
            else if (patient != null && patient.IsEmailVarified == true)
            {
                this.ViewBag.Message = "This account already active";
            }
            else
            {
                this.ViewBag.Message = "Invalid Request";
            }

            this.ViewBag.Status = Status;
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Doctor");
            }
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            string Meggage = null;
            bool Status = false;

            var doc = this._contex.Doctorses.FirstOrDefault(d => d.DoctorEmail == login.Email);
            if (doc != null)
            {
                if (string.Compare(Crypto.Hash(login.Password), doc.DoctorPassword) == 0 
                    && doc.IsEmailVarified == true)
                {
                    Status = true;
                    int timeout = login.Remember ? 1440 : 720; // 1440 min = 1 day && 720 min= 12 hour
                    Session["Doctor"] = doc.DoctorEmail;
                    var cokie = new HttpCookie("Doctor", doc.DoctorEmail);
                    cokie.Expires = DateTime.Now.AddMinutes(timeout);
                    cokie.HttpOnly = true;
                    Response.Cookies.Add(cokie);

                    return RedirectToAction("Index", "Doctor");
                }
                else
                {
                    Meggage = "Account is not verified or Password incorrect!";
                }
            }
            else
            {
                Meggage = "This doctor doesn't exist!";
            }

            ViewBag.Message = Meggage;
            ViewBag.Status = Status;
            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            Response.Cookies["Doctor"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Login", "Doctor");
        }

        public ActionResult Doctors()
        {
            var doctors = this._contex.Doctorses.ToList();
            var departments = this._contex.Departments.ToList();
            
            var DrView = new DoctorView
            {
                Departments = departments,
                Doctorses = doctors
            };
            return PartialView(DrView);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                int isExecuted = 0;

                Doctors aDoctors = this._contex.Doctorses.FirstOrDefault(dr => dr.Id == id);
                this._contex.Doctorses.Remove(aDoctors);
                isExecuted = this._contex.SaveChanges();

                if (isExecuted > 0)
                {
                    ViewBag.AlertMsg = "Delete Successfully";
                }
                return RedirectToAction("Index", "Admin");
            }
            catch
            {
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var department = this._contex.Departments.ToList();
            Doctors aDoctors = this._contex.Doctorses.FirstOrDefault(dr => dr.Id == id);
            
            var DrView = new DoctorView
            {
                Departments = department,
                Doctors = aDoctors
            };
            return PartialView(DrView);
        }

        public JsonResult Edit(int? id)
        {
            Doctors aDoctors = this._contex.Doctorses.FirstOrDefault(dr => dr.Id == id);
            return Json(aDoctors, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Doctors doctors)
        {
            var department = this._contex.Departments.ToList();
            var DrView = new DoctorView
            {
                Departments = department
            };
            var Doctor = this._contex.Doctorses.Single(p => p.Id == doctors.Id);
            ModelState["DoctorPassword"].Errors.Clear();
            ModelState["DoctorConfirmPassword"].Errors.Clear();
            doctors.DoctorPassword = Doctor.DoctorPassword;
            doctors.DoctorConfirmPassword = Doctor.DoctorPassword;
            
            if (ModelState.IsValid)
            {
                this._contex.Entry(doctors).State = EntityState.Modified;
                this._contex.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return PartialView(DrView);
        }

        public ActionResult Appointment()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                var appointment = this._contex.Appointments.Include("Patient").OrderByDescending(a => a.Id)
                    .Where(a => a.DoctorsId == doctor.Id).Take(10);

                var appointmentView = new AppointmentView
                {
                    Appointments = appointment
                };
                return this.PartialView(appointmentView);
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        [HttpPost]
        public ActionResult Accept(Appointment appointment)
        {
            if (appointment.Id != 0)
            {
                try
                {
                    Appointment aAppointment = this._contex.Appointments.FirstOrDefault(a => a.Id == appointment.Id);
                    aAppointment.AcceptStatus = true;
                    aAppointment.AppointmentDate = appointment.AppointmentDate;
                    aAppointment.AppointmentTime = appointment.AppointmentTime;
                    this._contex.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception x)
                {
                    return Json(x);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult Refer(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                try
                {
                    int Id = Convert.ToInt32(id);
                    var appointment = this._contex.Appointments.Single(p => p.Id == Id);
                    var doctors = this._contex.Doctorses.ToList();

                    var referView = new ReferView
                    {
                        Doctors = doctors,
                        Appointment = appointment
                    };
                    return this.View(referView);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
            }
        }

        [HttpPost]
        public ActionResult Refer(int id, int dId)
        {
            if (id != 0)
            {
                try
                {
                    Appointment aAppointment = this._contex.Appointments.FirstOrDefault(a => a.Id == id);
                    aAppointment.DoctorsId = dId;
                    this._contex.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception x)
                {
                    return Json(x);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult Chat()
        {
            if (this.GetCurrentDoctor() != null)
            {
                return Redirect("~/Doctor/ChatBox");
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        public ActionResult Patient()
        {
            List<Patient> Pa = new List<Patient>();
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                var Patient = this._contex.Appointments.Include("Patient").Where(a => a.DoctorsId == doctor.Id).ToList();
                foreach (var p in Patient)
                {
                    Pa.Add(p.Patient);
                }

                var past = Pa.Distinct();
                return this.PartialView(past);
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        public JsonResult GetTreatmentHistory(int Id)
        {
            List<Medicine> Ma = new List<Medicine>();

            var Appointment = this._contex.Appointments.Where(p => p.PatientId == Id).ToList();
            foreach (var ap in Appointment)
            {
                var Medicine = this._contex.Medicines.Where(a => a.AppointmentId == ap.Id).ToList();
                foreach (var ma in Medicine)
                {
                    Ma.Add(ma);
                }
            }
            return Json(Ma, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Profile()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                return this.PartialView(doctor);
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        [HttpPost]
        public ActionResult Update(Doctors doctor)
        {
            var Doctor = this._contex.Doctorses.Single(p => p.Id == doctor.Id);
            ModelState["DoctorPassword"].Errors.Clear();
            ModelState["DoctorConfirmPassword"].Errors.Clear();
            ModelState["RegNo"].Errors.Clear();
            ModelState["DoctorBirthDate"].Errors.Clear();
            doctor.DoctorBirthDate = Doctor.DoctorBirthDate;
            doctor.RegNo = Doctor.RegNo;
            doctor.DoctorPassword = Doctor.DoctorPassword;
            doctor.DoctorConfirmPassword = Doctor.DoctorPassword;
            doctor.DepartmentId = Doctor.DepartmentId;
            if (ModelState.IsValid)
            {
                if (doctor.DoctorImagefile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(doctor.DoctorImagefile.FileName);
                    string Extantion = Path.GetExtension(doctor.DoctorImagefile.FileName);
                    fileName = fileName + DateTime.Now.Year + Extantion;
                    doctor.DoctorImagePath = "/Image/doctors/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/doctors/"), fileName);
                    doctor.DoctorImagefile.SaveAs(fileName);
                }
                else
                {
                    doctor.DoctorImagePath = Doctor.DoctorImagePath;
                }
                Doctor.DoctorName = doctor.DoctorName;
                Doctor.DoctorImagePath = doctor.DoctorImagePath;
                Doctor.DoctorBirthDate = doctor.DoctorBirthDate;
                Doctor.DoctorEmail = doctor.DoctorEmail;
                Doctor.DoctorDegree = doctor.DoctorDegree;
                Doctor.RegNo = doctor.RegNo;
                Doctor.DoctorDetails = doctor.DoctorDetails;
                Doctor.StartTime = doctor.StartTime;
                Doctor.EndTime = doctor.EndTime;
                Doctor.DoctorPassword = doctor.DoctorPassword;
                Doctor.DoctorConfirmPassword = doctor.DoctorConfirmPassword;
                Doctor.DepartmentId = doctor.DepartmentId;
                //this._contex.Entry(doctor).State = EntityState.Modified;
                this._contex.SaveChanges();
            }
            return RedirectToAction("Index", "Doctor");
        }

        public ActionResult Deshboard()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                ViewBag.Appointments = this._contex.Appointments
                    .Where(a => a.Status == false && a.DoctorsId == doctor.Id).Count();
                return this.PartialView();
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        public ActionResult History()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                var ap = this._contex.Appointments.Where(a => a.DoctorsId == doctor.Id && a.Status == false);
                return this.PartialView(ap);
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        public ActionResult Prescription(string id)
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    try
                    {
                        int appointId = Convert.ToInt32(id);
                        var appointment = this._contex.Appointments.Include("Patient").Include("Doctors")
                            .Single(a => a.Id == appointId);
                        ViewBag.Report = this._contex.Reports.Where(r => r.Id == appointment.PatientId).ToList();
                        var solution = new SloliutionView { Appointment = appointment, Prescription = new Prescription() };
                        return this.View(solution);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        private Doctors GetCurrentDoctor()
        {
            if (Request.Cookies.Get("Doctor") != null)
            {
                var dEmail = this.Request.Cookies["Doctor"].Value;
                return this._contex.Doctorses.Single(d => d.DoctorEmail == dEmail);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMedicine(Medicine medicine, string id)
        {
            if (id != null)
            {
                try
                {
                    int ID = Convert.ToInt32(id);
                    var appointment = this._contex.Appointments.Single(a => a.Id == ID);
                    medicine.AppointmentId = appointment.Id;
                    this._contex.Medicines.Add(medicine);
                    this._contex.SaveChanges();
                    return Json("Success!");
                }
                catch (Exception ex)
                {
                    return Json("Error try again!");
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTest(Report report, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int ID = Convert.ToInt32(id);
                var appointment = this._contex.Appointments.Single(a => a.Id == ID);
                report.AppointmentId = appointment.Id;

                if (ModelState.IsValid)
                {
                    try
                    {
                        this._contex.Reports.Add(report);
                        this._contex.SaveChanges();
                        return Json("Success!");
                    }
                    catch (Exception ex)
                    {
                        return Json("Input is invalide");
                    }
                }
                else
                {
                    return Json("Input is invalide");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPrescription(Prescription prescription, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int ID = Convert.ToInt32(id);
                var appointment = this._contex.Appointments.Single(a => a.Id == ID);
                prescription.AppointmentId = appointment.Id;

                if (ModelState.IsValid)
                {
                    try
                    {
                        this._contex.Prescriptions.Add(prescription);
                        this._contex.SaveChanges();
                        return Json("Success!");
                    }
                    catch (Exception e)
                    {
                        return Json("Error! try again");
                    }
                }
                else
                {
                    return Json("Input is invalide");
                }
            }
        }

        public ActionResult ChatBox()
        {
            var doctor = this.GetCurrentDoctor();
            if (doctor != null)
            {
                List<Patient> patient = new List<Patient>();
                var appointment = this._contex.Appointments.Include("Patient").Where(a => a.DoctorsId == doctor.Id);
                foreach (var ap in appointment)
                {
                    patient.Add(ap.Patient);
                }

                List<Patient> plist = patient.Distinct().ToList();
                var chatView = new ChatViewForDoctor
                {
                    Doctors = doctor,
                    Patient = plist,
                    Chat = new Chat()
                };
                return this.PartialView(chatView);
            }
            else
            {
                return RedirectToAction("Login", "Doctor");
            }
        }

        [HttpGet]
        public JsonResult GetMeeage(string id)
        {
            var patient = this._contex.Patients.Single(p => p.PatientEmail == id);
            var doctor = this.GetCurrentDoctor();
            var message = JsonConvert.SerializeObject(
                this._contex.Chats.OrderByDescending(c => c.Id).Where(
                    c => c.Sender == doctor.DoctorEmail && c.Reciver == patient.PatientEmail
                         || (c.Sender == patient.PatientEmail && c.Reciver == doctor.DoctorEmail)).ToList());
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SendMessage(string MessageText, string Reciver)
        {
            Chat chat = new Chat();
            var sender = this.GetCurrentDoctor();
            chat.MessageText = MessageText;
            chat.Sender = sender.DoctorEmail;
            chat.Reciver = Reciver;
            chat.time = DateTime.Now;

            try
            {
                this._contex.Chats.Add(chat);
                this._contex.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult Solution(string Id)
        {
            if (Id != null)
            {
                try
                {
                    int id = Convert.ToInt32(Id);
                    Appointment appointment = this._contex.Appointments.FirstOrDefault(a => a.Id == id);
                    appointment.Status = true;
                    this._contex.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception x)
                {
                    return Json(x);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}
