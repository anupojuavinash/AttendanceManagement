using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceManagement.Models;

namespace MvcApplication1.Controllers
{
    public class AttendanceController : Controller
    {
        private AMSEntities db = new AMSEntities();

        //
        // GET: /Attendance/

        public ActionResult Index()
        {
            int id = Int32.Parse(Session["LogedUserId"].ToString());
            var attendances = db.Attendances.Where(a => a.EmployeeId==id);
            return View(attendances.ToList());
        }

        //
        // GET: /Attendance/Details/5

        public ActionResult Details(int id = 0)
        {
            Attendance attendance = db.Attendances.Single(a => a.Id == id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        //
        // GET: /Attendance/Create

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name");
            return View();
        }

        //
        // POST: /Attendance/Create

        [HttpPost]
        public ActionResult Create(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Attendances.AddObject(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", attendance.EmployeeId);
            return View(attendance);
        }

        //
        // GET: /Attendance/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Attendance attendance = db.Attendances.Single(a => a.Id == id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", attendance.EmployeeId);
            return View(attendance);
        }

        //
        // POST: /Attendance/Edit/5

        [HttpPost]
        public ActionResult Edit(Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Attendances.Attach(attendance);
                db.ObjectStateManager.ChangeObjectState(attendance, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", attendance.EmployeeId);
            return View(attendance);
        }

        //
        // GET: /Attendance/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Attendance attendance = db.Attendances.Single(a => a.Id == id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        //
        // POST: /Attendance/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Attendance attendance = db.Attendances.Single(a => a.Id == id);
            db.Attendances.DeleteObject(attendance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}