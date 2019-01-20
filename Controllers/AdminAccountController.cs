using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace Project.Controllers
{
    public class AdminAccountController : Controller
    {
        // GET: AdminAccount
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Authorise(Project.Models.AdminLogin admin)
        {
            using (ProjectDataBaseEntities db = new ProjectDataBaseEntities())
            {
               var userdetails = db.AdminLogins.Where(x => x.UserName == admin.UserName && x.Password == admin.Password).FirstOrDefault();
                if(userdetails==null)
                {
                    admin.LoginErrorMessage = "Wrong User Name or Password ";
                    return View("AdminLogin", admin);
                }
                else
                {
                    Session["Id"] = userdetails.Id;
                    return RedirectToAction("AdminDashBoard", "AdminAccount");

                }
            }
                
        }
        public ActionResult AdminDashBoard()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("AdminLogin", "AdminAccount");
        }
        [HttpGet]
        public ActionResult Register(int id=0)
        {
            
            AdminLogin ad = new AdminLogin();
            return View(ad);
        }
        [HttpPost]
        public ActionResult Register(AdminLogin adm)
        {
            using (ProjectDataBaseEntities db = new ProjectDataBaseEntities())
            {
                if(db.AdminLogins.Any(x=>x.UserName==adm.UserName))
                {
                    ViewBag.DuplicateMessage = "User Name Already Exists";
                    return View("Register", adm);
                }
                db.AdminLogins.Add(adm);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful!";

                return View("Register",new AdminLogin());
        }
    }
}