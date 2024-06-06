using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vispl.Trainee.CricInfo.VO;
using Vispl.Trainee.CricInfo.BM;
using System.Web.Security;

namespace Vispl.Trainee.CricInfo.UI.Controllers
{
    public class UserAuthenticationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserRole, string UserName, string Password)
        {
            if (ModelState.IsValid)
            {
                UserVO user = new UserVO();
                UserValidationBL validatUser = new UserValidationBL();

                user.UserRole = UserRole;
                user.UserName = UserName;
                user.Password = Password;

                bool IsUserExist = validatUser.isUserPresent(user);
                if (IsUserExist)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("AdminDashboard", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("ekey", "Invalid Username or Password or UserType");
                    return View("Index");

                }
            }
            ModelState.AddModelError("e2key", "Invalid Username or Password or UserType");
            return View("Index");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("DashBoard", "Dashboard");
        }
    }
}