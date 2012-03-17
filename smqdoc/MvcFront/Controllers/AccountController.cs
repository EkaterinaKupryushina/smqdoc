using System;
using System.Web.Mvc;
using System.Web.Security;
using MvcFront.Helpers;
using MvcFront.Models;
using MvcFront.Interfaces;

namespace MvcFront.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserAccountRepository _userRepository;

        public AccountController(IUserAccountRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.Login(model.UserName, model.Password);
                if (user != null)
                {
                    //var sessionData = new SmqUserSessionData {UserName = model.UserName,UserId = user.userid,
                    //    UserType = user.IsAdmin ? SmqUserProfileType.Systemadmin: SmqUserProfileType.User,CurrentProfileName = user.IsAdmin ? "Администратор" : "Пользователь"};
                    //SessionHelper.SetUserSessionData(Session,sessionData);

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Не верный логин/Пароль");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            SessionHelper.ClearUserSessionData(Session);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    var currentUser = _userRepository.GetByLogin(User.Identity.Name);
                    if (currentUser.Password == model.OldPassword && model.NewPassword == model.ConfirmPassword)
                    {
                        currentUser.Password = model.NewPassword;
                        _userRepository.Save(currentUser);
                        changePasswordSucceeded = true;
                    }
                    else
                    {
                        changePasswordSucceeded = false;
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                ModelState.AddModelError("", "Ошибка при вводе пароля");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}
