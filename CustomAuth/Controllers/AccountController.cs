using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CustomAuth.Data;

namespace CustomAuth.Controllers
{
    public class AccountController : Controller
    {
        private UserDb userDb = new UserDb(
            ConfigurationManager.ConnectionStrings["CustomAuthConnection"].ConnectionString);

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            
            var user = userDb.GetUser(username);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Username not found!";
                return View();
            }
            var salt = user.Salt;
            var enteredPassword = PasswordHasher.HashPassword(password, salt);
            if (user.Password == enteredPassword)
            {
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Secret");
            }

            ViewBag.ErrorMessage = "Invalid password!";
            return View();

        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User user)
        {
            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(user.Password, salt);
            user.Password = hashedPassword;
            user.Salt = salt;
            userDb.AddUser(user);
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
