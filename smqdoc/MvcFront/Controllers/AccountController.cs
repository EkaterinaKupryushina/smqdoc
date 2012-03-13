using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
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
                

                if (_userRepository.Login(model.UserName,model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Не верный логин/Пароль");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

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
                else
                {
                    ModelState.AddModelError("", "Ошибка при вводе пароля");
                }
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

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Пользователь с заданным именем уже существует.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Пользователь с заданным e-mail уже существует.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Не верный пароль.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Не вырный e-mail.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Не верное имя пользователя";

                case MembershipCreateStatus.ProviderError:
                    return "Ошибка провадера авторизации.";

                case MembershipCreateStatus.UserRejected:
                    return "Обратитесь к разработчику.";

                default:
                    return "Неизвестаня ошибка, обратитесь к администратору.";
            }
        }
        #endregion
    }
}
