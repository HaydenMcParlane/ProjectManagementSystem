using CommerceIdeaBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CommerceIdeaBank.Controllers.Admin;
using CommerceIdeaBank.Controllers.Ambassador;
using CommerceIdeaBank.Controllers.Contributor;
using System.Windows.Forms;

namespace CommerceIdeaBank.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(CommerceIdeaBank.Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.Username, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    
                    //Retrieve relevant user object *** 
                    using (var context = new MainDBEntities())
                    {
                        User relevant_user = context.Users.Find(user.Username);

                        //Check to determine permissions of authenticated users
                        if (relevant_user.UserRole == 1 /* Contributor */)
                        {
                            return RedirectToAction("Index", "ContributorHome");
                        }
                        else if (relevant_user.UserRole == 2 /* Mentor */)
                        {
                            return RedirectToAction("Index", "AmbassadorHome");
                        }
                        else if (relevant_user.UserRole == 3 /* Ambassador */)
                        {
                            return RedirectToAction("Index", "AmbassadorHome");
                        }
                        else if (relevant_user.UserRole == 4 /* Admin */)
                        {
                            return RedirectToAction("Index", "AdminHome");
                        }
                        else
                        {
                            //Output message box ***
                            MessageBox.Show("Oh no! Something seems to have gone wrong while logging " +
                                "you in! We apologize about any inconvenience. Please try logging in " +
                                "again and notify us if the issue persists!");

                            //Route person to the default home page
                            return RedirectToAction("Index", "Home");
                        }
                    }                                        
                }
            }
            else
            {
                ModelState.AddModelError("", "Login Data is Incorrect.");
            }

            return View(user);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(CommerceIdeaBank.Models.User user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MainDBEntities())
                {
                    var crypto = new SimpleCrypto.PBKDF2();
                    var encrpPass = crypto.Compute(user.Password);
                    var sysUser = db.Users.Create();

                    sysUser.Username = user.Username;
                    sysUser.Email = user.Email;
                    sysUser.Password = encrpPass;
                    sysUser.PasswordSalt = crypto.Salt;
                    sysUser.UserRole = 1;
                    sysUser.IsActive = true;

                    db.Users.Add(sysUser);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(user);
        }
        
        [ActionName("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            HttpContext.Session.Clear();           

            return RedirectToAction("Index", "Home");
        }

        private bool IsValid(string username, string password)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            bool isValid = false;

            using (var db = new MainDBEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.Username == username);

                if (user != null)
                {
                    if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    {
                        HttpContext.Session["currentUser"] = user.Username;
                        HttpContext.Session["userEmail"] = user.Email;
                        HttpContext.Session["userRole"] = user.UserRole;
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

    }
}
