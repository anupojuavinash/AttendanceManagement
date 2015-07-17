using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttendanceManagement.Models;

namespace AttendanceManagement.Controllers
{
    public class LeaveController : Controller
    {
        private AMSEntities db = new AMSEntities();

        //
        // GET: /Leave/

        public ActionResult Index()
        {
            var leaves = db.Leaves.Include("Employee");
            return View(leaves.ToList());
        }

        //
        // GET: /Leave/Details/5

        public ActionResult Details(int id = 0)
        {
            Leave leave = db.Leaves.Single(l => l.EmployeeId == id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        //
        // GET: /Leave/Create

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name");
            return View();
        }

        //
        // POST: /Leave/Create

        [HttpPost]
        public ActionResult Create(Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Leaves.AddObject(leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", leave.EmployeeId);
            return View(leave);
        }

        //
        // GET: /Leave/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Leave leave = db.Leaves.Single(l => l.EmployeeId == id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", leave.EmployeeId);
            return View(leave);
        }

        //
        // POST: /Leave/Edit/5

        [HttpPost]
        public ActionResult Edit(Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Leaves.Attach(leave);
                db.ObjectStateManager.ChangeObjectState(leave, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", leave.EmployeeId);
            return View(leave);
        }

        //
        // GET: /Leave/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Leave leave = db.Leaves.Single(l => l.EmployeeId == id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        //
        // POST: /Leave/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Leave leave = db.Leaves.Single(l => l.EmployeeId == id);
            db.Leaves.DeleteObject(leave);
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