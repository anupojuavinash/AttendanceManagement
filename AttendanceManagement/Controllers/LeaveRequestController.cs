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
    public class LeaveRequestController : Controller
    {
        private AMSEntities db = new AMSEntities();

        //
        // GET: /LeaveRequest/

        public ActionResult Index(int id = 0)
        {
            //var leaverequests = db.LeaveRequests.Where(l => l.Employee_Id == id);
            //var leaverequests = db.LeaveRequests.Include(l => l.Employee);
            //return View(leaverequests.ToList());
            id = Int32.Parse(Session["LogedUserId"].ToString());
            var leaveRequests = db.LeaveRequests.Include(c => c.Employee).Where(c => c.Employee.ManagerId == id);
            Session["leaveRequests"] = leaveRequests;
            return View(leaveRequests.ToList());
        }
        public ActionResult ViewStatus(int id =0)
        {
            var leaverequests = db.LeaveRequests.Where(l => l.Employee_Id == id);
            //var leaverequests = db.LeaveRequests.Include(l => l.Employee);
            return View(leaverequests.ToList());
        }

        //
        // GET: /LeaveRequest/Details/5
        [HttpPost, ActionName("Update")]
        public ActionResult Update(FormCollection collection)
        {
            int id = Int32.Parse(Session["LogedUserId"].ToString());
            var leaveRequests = db.LeaveRequests.Include(c => c.Employee).Where(c => c.Employee.ManagerId == id);


            foreach (LeaveRequest leaveRequest in leaveRequests)
            {
                string status = (string)collection.Get("status" + leaveRequest.Id);
                Console.WriteLine("Status" + status);
                System.Diagnostics.Debug.WriteLine("Status :" + status);
                if (status != null)
                {
                    if (status.Equals("No"))
                    {
                        leaveRequest.Status = "Disapproved";
                    }
                    else if (status.Equals("Yes"))
                    {
                        leaveRequest.Status = "Approved";
                        
                        for (DateTime d = leaveRequest.FromDate; d <= leaveRequest.ToDate; d=d.AddDays(1))
                        {
                            Attendance att = new Attendance();
                            att.EmployeeId = leaveRequest.Employee_Id;
                            if(leaveRequest.TypeOfLeave.Contains("Sick")){
                                Leave leave = db.Leaves.Where(l => l.EmployeeId == att.EmployeeId).FirstOrDefault();
                                if (leave.SickLeaves > 0)
                                {
                                    att.Status = "SickLeave";
                                }
                                leave.SickLeaves--;
                            }
                            else if(leaveRequest.TypeOfLeave.Contains("casual")){
                                Leave leave = db.Leaves.Where(l => l.EmployeeId == att.EmployeeId).FirstOrDefault();
                                if (leave.CasualLeaves > 0)
                                {
                                    att.Status = "CasualLeave";
                                }
                                leave.CasualLeaves--;
                            }
                            else if (leaveRequest.TypeOfLeave.Contains("Optional"))
                            {
                                Leave leave = db.Leaves.Where(l => l.EmployeeId == att.EmployeeId).FirstOrDefault();
                                if (leave.CasualLeaves > 0)
                                {
                                    att.Status = "OptionalLeave";
                                }
                                leave.OptionalHolidays--;
                            }
                            else
                            {
                                att.Status = "Absent";
                            }
                            att.Date = d;
                            db.Attendances.AddObject(att);
                        }

                    }
                }

                //AMSEntities newCtx = new AMSEntities(); 
                //newCtx.LeaveRequests.Attach(leaveRequest);
                //newCtx.ObjectStateManager.ChangeObjectState(leaveRequest, EntityState.Modified);

            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id = 0)
        {
            var leaverequests = db.LeaveRequests.Where(l => l.Employee_Id == id);
            if (leaverequests == null)
            {
                return HttpNotFound();
            }
            return View(leaverequests.ToList());
        }

        //
        // GET: /LeaveRequest/Create

        public ActionResult Create()
        {
            ViewBag.Employee_Id = new SelectList(db.Employees, "EmployeeId", "Name");
            return View();
        }

        //
        // POST: /LeaveRequest/Create

        [HttpPost]
        public ActionResult Create(LeaveRequest leaverequest)
        {
            if (ModelState.IsValid)
            {
                db.LeaveRequests.AddObject(leaverequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Employee_Id = new SelectList(db.Employees, "EmployeeId", "Name", leaverequest.Employee_Id);
            return View(leaverequest);
        }

        //
        // GET: /LeaveRequest/Edit/5

        public ActionResult Edit(int id = 0)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Single(l => l.Id == id);
            if (leaverequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employee_Id = new SelectList(db.Employees, "EmployeeId", "Name", leaverequest.Employee_Id);
            return View(leaverequest);
        }

        //
        // POST: /LeaveRequest/Edit/5

        [HttpPost]
        public ActionResult Edit(LeaveRequest leaverequest)
        {
            if (ModelState.IsValid)
            {
                db.LeaveRequests.Attach(leaverequest);
                db.ObjectStateManager.ChangeObjectState(leaverequest, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employee_Id = new SelectList(db.Employees, "EmployeeId", "Name", leaverequest.Employee_Id);
            return View(leaverequest);
        }

        //
        // GET: /LeaveRequest/Delete/5

        public ActionResult Delete(int id = 0)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Single(l => l.Id == id);
            if (leaverequest == null)
            {
                return HttpNotFound();
            }
            return View(leaverequest);
        }

        //
        // POST: /LeaveRequest/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveRequest leaverequest = db.LeaveRequests.Single(l => l.Id == id);
            db.LeaveRequests.DeleteObject(leaverequest);
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