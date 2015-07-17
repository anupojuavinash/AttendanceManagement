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
    public class EmployeeController : Controller
    {
        private AMSEntities db = new AMSEntities();

        //
        // GET: /Employee/

        public ActionResult Index()
        {
            var employees = db.Employees.Include("Leave").Include("Employee2");
            return View(employees.ToList());
        }

        //
        // GET: /Employee/Details/5

        public ActionResult Details(int id = 0)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeId == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // GET: /Employee/Create

        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Leaves, "EmployeeId", "EmployeeId");
            ViewBag.ManagerId = new SelectList(db.Employees, "EmployeeId", "Name");
            return View();
        }

        //
        // POST: /Employee/Create

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.AddObject(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Leaves, "EmployeeId", "EmployeeId", employee.EmployeeId);
            ViewBag.ManagerId = new SelectList(db.Employees, "EmployeeId", "Name", employee.ManagerId);
            return View(employee);
        }

        //
        // GET: /Employee/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeId == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Leaves, "EmployeeId", "EmployeeId", employee.EmployeeId);
            ViewBag.ManagerId = new SelectList(db.Employees, "EmployeeId", "Name", employee.ManagerId);
            return View(employee);
        }

        //
        // POST: /Employee/Edit/5

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Attach(employee);
                db.ObjectStateManager.ChangeObjectState(employee, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Leaves, "EmployeeId", "EmployeeId", employee.EmployeeId);
            ViewBag.ManagerId = new SelectList(db.Employees, "EmployeeId", "Name", employee.ManagerId);
            return View(employee);
        }

        //
        // GET: /Employee/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeId == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //
        // POST: /Employee/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Single(e => e.EmployeeId == id);
            db.Employees.DeleteObject(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult ApplyLeave()
        {
            
            return View();
        }
        public ActionResult Leaves()
        {
            int empId = Int32.Parse(Session["LogedUserId"].ToString());

            return RedirectToAction("Details", "Leave", new { id = empId });
        }
        public ActionResult ViewReportees()
        {
            int id = Int32.Parse(Session["LogedUserId"].ToString());
            var employees = db.Employees.Where(e => e.ManagerId == id);
            
            if (employees == null)
            {
                return HttpNotFound();
            }
            return View(employees.ToList());
        }
        public ActionResult AddRequest(LeaveRequest leaveRequest)
        {
            int id = Int32.Parse(Session["LogedUserId"].ToString());
            leaveRequest.Employee_Id = id;
            leaveRequest.DateOfApplication = DateTime.Today;
            leaveRequest.Status = "Requested";
            /*string startingDate = collection.Get("sDate").ToString();
            string endingDate = collection.Get("eDate").ToString();
            string leaveType = collection.Get("leaveType").ToString();
            System.Diagnostics.Debug.Write("Leave type : " + leaveType);
            System.Diagnostics.Debug.Write("EDate : " + endingDate);
            string reason = collection.Get("reason").ToString();
            LeaveRequest leaveRequest = new LeaveRequest();
            leaveRequest.FromDate = Convert.ToDateTime(startingDate);

            leaveRequest.ToDate = Convert.ToDateTime(startingDate);
            leaveRequest.DateOfApplication = DateTime.Today;
            leaveRequest.TypeOfLeave = leaveType;
            leaveRequest.Reason = reason;
            leaveRequest.Employee_Id = id;*/


            db.LeaveRequests.AddObject(leaveRequest);
            db.SaveChanges();
            return RedirectToAction("Index", "Attendance");


        }
        
        

    }
}