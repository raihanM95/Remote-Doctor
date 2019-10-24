using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doctor;
using Doctor.Models;
using Doctor.ViewModel;

namespace Doctor.Controllers
{
    public class DepartmentController : Controller
    {
        // DbContex
        private DataContex _contex;

        public DepartmentController()
        {
            _contex = new DataContex();
        }

        protected override void Dispose(bool dispossing)
        {
            _contex.Dispose();
        }

        // GET: Department
        public ActionResult Index()
        {
            var departments = this._contex.Departments.ToList();
            
            var DeptView = new DepartmentView
            {
                Departments = departments,
            };
            return PartialView(DeptView);
        }

        //// GET: Department/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DeptName,DeptDetails")] Department department)
        {
            if (ModelState.IsValid)
            {
                this._contex.Departments.Add(department);
                this._contex.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(department);
        }

        //// GET: Department/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Department department = db.Departments.Find(id);
        //    if (department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(department);
        //}

        //// POST: Department/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,DeptName,DeptDetails")] Department department)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(department).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(department);
        //}

        //// GET: Department/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Department department = db.Departments.Find(id);
        //    if (department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(department);
        //}

        //// POST: Department/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Department department = db.Departments.Find(id);
        //    db.Departments.Remove(department);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Delete(int id)
        {
            try
            {
                int isExecuted = 0;

                Department aDepartment = this._contex.Departments.FirstOrDefault(dr => dr.Id == id);
                this._contex.Departments.Remove(aDepartment);
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
    }
}
