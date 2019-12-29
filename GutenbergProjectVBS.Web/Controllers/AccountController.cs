using GutenbergProjectVBS.Web.Filters;
using GutenbergProjectVBS.Web.Models;
using GutenbergProjectVBS.Web.Models.Context;
using GutenbergProjectVBS.Web.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GutenbergProjectVBS.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [AuthFilterUserNull]
        public ActionResult Profile()
        {
            ProfileViewModel user = new ProfileViewModel();

            using (VBSContext db = new VBSContext())
            {
                User getUser = db.Users.AsQueryable().Where(x => x.Id == CurrentSession.User.Id).FirstOrDefault();
                user.Name = getUser.Name;
                user.Surname = getUser.Surname;
                user.Email = getUser.Email;
            }

            return View(user);
        }

        [AuthFilterUserNull]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (VBSContext db = new VBSContext())
                {
                    try
                    {                        
                        User user = db.Users.AsQueryable().FirstOrDefault(c => c.Email == model.Email);
                        user.Name = model.Name;
                        user.Surname = model.Surname;
                        user.UpdatedAt = DateTime.Now;
                        db.SaveChanges();
                        TempData.Add("UserProfileSuccess", "User profile was update successfully.");
                    }
                    catch (Exception ex)
                    {
                        TempData.Add("UserProfileFail", "User profile was not update successfully.");                        
                    }
                    
                    return RedirectToAction("Profile", "Account");
                }
            }
            return View();
        }

        // GET: Login
        [AuthFilterUserNotNull]
        public ActionResult Login()
        {
            return View();
        }

        [AuthFilterUserNotNull]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                using(VBSContext db = new VBSContext())
                {
                    var password = GVBSHelpers.GVBSHelperClass.CreateMD5Password(model.Password);
                    User result = db.Users.AsQueryable().FirstOrDefault(u => u.Email == model.Email && u.Password.Equals(password) && u.IsAdmin == false);
                    if(result == null)
                    {
                        model.Email = null;
                        model.Password = null;
                        ViewBag.UserNotFound = "Email Address and/or Password is incorrect.";
                        return View(model);
                    }
                    CurrentSession.Set<User>("userLogin", result);

                    string redirectToBack = Session["RedirectToBack"] == null ? "" : Session["RedirectToBack"].ToString();
                    if (Session["RedirectToBack"] != null)
                    {
                        Session.Remove("RedirectToBack");
                        return Redirect(redirectToBack);
                    } else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        // GET: Register
        [AuthFilterUserNotNull]
        public ActionResult Register()
        {
            return View();
        }

        [AuthFilterUserNotNull]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {                
                using (VBSContext db = new VBSContext())
                {
                    User control = db.Users.AsQueryable().FirstOrDefault(c => c.Email == model.Email);
                    // If user is not exists then save the user
                    if(control == null)
                    {
                        var password = GVBSHelpers.GVBSHelperClass.CreateMD5Password(model.Password);
                        User user = new User()
                        {
                            Name = model.Name,
                            Surname = model.Surname,
                            Email = model.Email,
                            Password = password,
                            IsAdmin = false,
                            CreatedAt = DateTime.Now
                        };
                        db.Users.Add(user);
                        db.SaveChanges();
                        TempData["UserCreateSuccess"] = string.Format("Dear {0}, welcome to visual bookshelf.", model.Name + " " + model.Surname);

                        string redirectToBack = Session["RedirectToBack"] == null ? "" : Session["RedirectToBack"].ToString();
                        if (Session["RedirectToBack"] != null)
                        {
                            Session.Remove("RedirectToBack");
                            return Redirect(redirectToBack);
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    } else
                    {
                        ViewBag.UserExists = string.Format("User ({0}) is already exist in system.", model.Email);
                        model.Name = null;
                        model.Surname = null;
                        model.Email = null;
                        model.Password = null;
                        model.PasswordConfirm = null;
                        return View(model);
                    }                    
                }
            }

            return View();
        }

        [AuthFilterUserNotNull]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AuthFilterUserNotNull]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (VBSContext db = new VBSContext())
                {
                    var user = db.Users.AsQueryable().FirstOrDefault(u => u.Email == model.Email && u.IsAdmin == false);
                    if(user == null)
                    {
                        ViewBag.UserNotFound = "User didn't find in the system.";
                    }
                    else
                    {
                        string resetCode = Guid.NewGuid().ToString();
                        GVBSHelpers.GVBSHelperClass.SendVerificationLinkEmail(model.Email, resetCode, "ResetPassword");
                        user.ResetPasswordCode = resetCode;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        ViewBag.UserSendEmailSuccess = string.Format("Verification email was send your {0} email address.", model.Email);
                    }
                    return View(model);
                }
            }
            return View();
        }

        [AuthFilterUserNotNull]
        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (VBSContext db = new VBSContext())
            {
                var user = db.Users.AsQueryable().FirstOrDefault(a => a.ResetPasswordCode == id);
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                } else
                {
                    return HttpNotFound();
                }
            }            
        }

        [AuthFilterUserNotNull]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (VBSContext db = new VBSContext())
                {
                    var user = db.Users.AsQueryable().FirstOrDefault(a => a.ResetPasswordCode == model.ResetCode);
                    if (user != null)
                    {
                        user.Password = GVBSHelpers.GVBSHelperClass.CreateMD5Password(model.NewPassword);
                        user.ResetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        ViewBag.SuccessMessage = "New password updated successfully.";
                    }
                }
            } else
            {
                ViewBag.ErrorMessage = "Something invalid.";
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            CurrentSession.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}