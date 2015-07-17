using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AttendanceManagement.Models;

namespace AttendanceManagement.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Employee u)
        {
            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                using (AMSEntities dc = new AMSEntities())
                {
                    var v = dc.Employees.Where(a => a.Username.Equals(u.Username) && a.Password.Equals(u.Password)).FirstOrDefault();
                    if (v != null)
                    {
                        Session["LogedUserId"] = v.EmployeeId;
                        Session["LogedUserType"] = v.Designation.ToString();
                        Session["LogedUsername"] = v.Username.ToString();
                        Session["LogedUserFullName"] = v.Name.ToString();
                        return RedirectToAction("AfterLogin");
                    }
                }
            }
            return View(u);
        }
        public ActionResult AfterLogin()
        {
            if (Session["LogedUsername"] != null)
            {
                return RedirectToAction("Index", "Attendance");
                
            }
            else
            {
                return View();
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
