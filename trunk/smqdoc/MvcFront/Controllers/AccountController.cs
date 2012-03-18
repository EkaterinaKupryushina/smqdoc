using System;
using System.Linq;
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

        public ActionResult Index()
        {
            return View();
        }

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
            return RedirectToAction("LogOn", "Account");
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
        
        [HttpPost]
        public ActionResult ChangeUserProfile(ChangeUserProfileModel model)
        {
            if(ModelState.IsValid)
            {
                SmqUserSessionData sessData = SessionHelper.GetUserSessionData(Session);
                var user = _userRepository.GetById(sessData.UserId);
                
                int? groupId;
                bool isManager;
                SessionHelper.ParseUserProfileCode(model.UserProfileCode,out groupId,out isManager);
                if (groupId == null && isManager)
                {
                    sessData.UserGroupId = 0;
                    sessData.UserGroupName = null;
                    sessData.UserType = SmqUserProfileType.Systemadmin;
                    sessData.CurrentProfileName = "Администратор";

                }
                if (groupId == null && !isManager)
                {
                    sessData.UserGroupId = 0;
                    sessData.UserGroupName = null;
                    sessData.UserType = SmqUserProfileType.User;
                    sessData.CurrentProfileName = "Пользователь";

                }
                if (groupId != null && isManager)
                {
                    sessData.UserGroupId = groupId.Value;
                    sessData.UserGroupName =
                    user.ManagedGroups.First(
                            x => x.usergroupid == groupId.Value).GroupName;
                    sessData.UserType = SmqUserProfileType.Groupmanager;
                    sessData.CurrentProfileName = "Менеджер " + sessData.UserGroupName;

                }
                if (groupId != null && !isManager)
                {
                    sessData.UserGroupId = groupId.Value;
                    sessData.UserGroupName =
                    user.MemberGroups.First(
                            x => x.usergroupid == groupId.Value).GroupName;
                    sessData.UserType = SmqUserProfileType.Groupuser;
                    sessData.CurrentProfileName = "Участник " + sessData.UserGroupName;

                }
                SessionHelper.SetUserSessionData(Session,sessData);
                user.LastAccessProfileCode = model.UserProfileCode;
                _userRepository.Save(user);
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        public ActionResult EditUserInfo()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            //return View(new EditUserInfoModel(_userRepository.GetAll().Where(x => x.userid == sessData.UserId).SingleOrDefault()));             
            return View(new EditUserInfoModel(_userRepository.GetById(sessData.UserId)));
        }

        //
        // POST: /Account/EditUserInfo

        [Authorize]
        [HttpPost]
        public ActionResult EditUserInfo(EditUserInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userRepository.GetByLogin(User.Identity.Name);

                currentUser.FirstName = model.FirstName;
                currentUser.SecondName = model.SecondName;
                currentUser.LastName = model.LastName;
                currentUser.Email = model.Email;

                try
                {
                    _userRepository.Save(currentUser);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", String.Format("Ошибка сохранения изменений пользователя: {0}", ex.Message));
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
