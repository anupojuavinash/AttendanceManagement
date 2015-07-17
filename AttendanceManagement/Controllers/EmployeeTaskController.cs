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
    public class EmployeeTaskController : Controller
    {
        private AMSEntities db = new AMSEntities();

        //
        // GET: /EmployeeTask/

        public ActionResult Index()
        {
            var employeetasks = db.EmployeeTasks.Include("Employee");
            return View(employeetasks.ToList());
        }

        //
        // GET: /EmployeeTask/Details/5

        public ActionResult Details(int id = 0)
        {
            EmployeeTask employeetask = db.EmployeeTasks.Single(e => e.Id == id);
            if (employeetask == null)
            {
                return HttpNotFound();
            }
            return View(employeetask);
        }

        //
        // GET: /EmployeeTask/Create

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name");
            return View();
        }

        //
        // POST: /EmployeeTask/Create

        [HttpPost]
        public ActionResult Create(EmployeeTask employeetask)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeTasks.AddObject(employeetask);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", employeetask.EmployeeId);
            return View(employeetask);
        }

        //
        // GET: /EmployeeTask/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EmployeeTask employeetask = db.EmployeeTasks.Single(e => e.Id == id);
            if (employeetask == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", employeetask.EmployeeId);
            return View(employeetask);
        }

        //
        // POST: /EmployeeTask/Edit/5

        [HttpPost]
        public ActionResult Edit(EmployeeTask employeetask)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeTasks.Attach(employeetask);
                db.ObjectStateManager.ChangeObjectState(employeetask, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "EmployeeId", "Name", employeetask.EmployeeId);
            return View(employeetask);
        }

        //
        // GET: /EmployeeTask/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EmployeeTask employeetask = db.EmployeeTasks.Single(e => e.Id == id);
            if (employeetask == null)
            {
                return HttpNotFound();
            }
            return View(employeetask);
        }

        //
        // POST: /EmployeeTask/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeTask employeetask = db.EmployeeTasks.Single(e => e.Id == id);
            db.EmployeeTasks.DeleteObject(employeetask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult ViewDetails(int id = 0)
        {

            var employeetask = db.EmployeeTasks.Include(e => e.Employee).Where(e => e.EmployeeId == id);
            Session["EmpId"] = id;
            if (employeetask == null)
            {
                return HttpNotFound();
            }
            return View(employeetask.ToList());
        }
        [HttpPost, ActionName("Update")]
        public ActionResult Update(FormCollection collection)
        {
            int id = Int32.Parse(Session["LogedUserId"].ToString());
            var tasks = db.EmployeeTasks.Include(c => c.Employee).Where(c => c.Employee.ManagerId == id);

            int count = 0;
            int approved = 0;
            int disapproved = 0;
            DateTime date=DateTime.Today;
            foreach (EmployeeTask task in tasks)
            {
                string status = (string)collection.Get("status" + task.Id);
                Console.WriteLine("Status" + status);
                System.Diagnostics.Debug.WriteLine("Status :" + status);
                date = task.Date;
                if (status != null)
                {
                    count++;
                    if (status.Equals("No"))
                    {
                        disapproved++;
                        task.Status = "Disapproved";
                    }
                    else if (status.Equals("Yes"))
                    {
                        approved++;
                        task.Status = "Approved";
                    }
                }
            }
            string attendance="";
            if (approved == count)
            {
                attendance = "Present";
            }
            else if (disapproved == count)
            {
                attendance = "Absent";
            }
            else
            {
                attendance = "HalfDay";
            }
            Attendance att = new Attendance();
            att.Date = date;
            att.EmployeeId = Int32.Parse(Session["EmpId"].ToString());
            att.Status = attendance;
            db.Attendances.AddObject(att);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}