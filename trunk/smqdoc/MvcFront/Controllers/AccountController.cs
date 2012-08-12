using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MvcFront.Entities;
using MvcFront.Enums;
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
        

        public ActionResult Index()
        {
            UserSessionData sessData = SessionHelper.GetUserSessionData(Session);
            if (sessData == null)
                return RedirectToAction("LogOn");

            return View();
        }

        /// <summary>
        /// GET: /Account/LogOn
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOn()
        {
            return View();
        }

        /// <summary>
        ///  POST: /Account/LogOn 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
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
                    
                        //Читаем код последнего профиля пользовтаеля
                        var userProfileType = UserProfileTypes.User;
                        var userProfileName = "Пользователь";
                        string userProfileGroupName = null;
                        var userProfileGroupId = 0;
                        int? groupId;
                        bool isManager;
                        SessionHelper.ParseUserProfileCode(user.LastAccessProfileCode,out groupId,out isManager);
                        // Администратор
                        if (groupId == null && isManager)
                        {
                            userProfileGroupId = 0;
                            userProfileType = UserProfileTypes.Systemadmin;
                            userProfileName = "Администратор";

                        }
                        //Пользователь
                        if (groupId == null && !isManager)
                        {
                            userProfileGroupId = 0;
                            userProfileType = UserProfileTypes.User;
                            userProfileName = "Пользователь";

                        }
                        //Менеджер группы 
                        if(groupId != null && isManager)
                        {
                            var group = user.ManagedGroups.FirstOrDefault(x => x.usergroupid == groupId);
                            if(group != null)
                            {
                                userProfileGroupName = group.GroupName;
                                userProfileGroupId = group.usergroupid;
                                userProfileType = UserProfileTypes.Groupmanager;
                                userProfileName = "Менеджер " + userProfileGroupName;
                            }
                        }
                        //Участник группы
                        if (groupId != null && !isManager)
                        {
                            var group = user.MemberGroups.FirstOrDefault(x => x.usergroupid == groupId);
                            if (group != null)
                            {
                                userProfileGroupName = group.GroupName;
                                userProfileGroupId = group.usergroupid;
                                userProfileType = UserProfileTypes.Groupuser;
                                userProfileName = "Участник " + userProfileGroupName;
                            }
                        }

                        //Сохраняем данные в сессию
                        var sessData = new UserSessionData {UserName = user.Login,UserId = user.userid,UserType = userProfileType,
                            CurrentProfileName = userProfileName,UserGroupName = userProfileGroupName,UserGroupId = userProfileGroupId};
                        SessionHelper.SetUserSessionData(Session,sessData);
                        
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Неверный логин/Пароль");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// GET: /Account/LogOff 
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            SessionHelper.ClearUserSessionData(Session);
            return RedirectToAction("LogOn", "Account");
        }

        /// <summary>
        /// GET: /Account/ChangePassword
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// POST: /Account/ChangePassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

       /// <summary>
        ///  GET: /Account/ChangePasswordSuccess (смена пароля прошла успешно)
       /// </summary>
       /// <returns></returns>
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        
        /// <summary>
        /// Смена текущего профиля пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeUserProfile(ChangeUserProfileModel model)
        {
            if(ModelState.IsValid)
            {
                UserSessionData sessData = SessionHelper.GetUserSessionData(Session);
                var user = _userRepository.GetById(sessData.UserId);
                
                int? groupId;
                bool isManager;
                SessionHelper.ParseUserProfileCode(model.UserProfileCode,out groupId,out isManager);
                if (groupId == null && isManager)
                {
                    sessData.UserGroupId = 0;
                    sessData.UserGroupName = null;
                    sessData.UserType = UserProfileTypes.Systemadmin;
                    sessData.CurrentProfileName = "Администратор";

                    SessionHelper.SetUserSessionData(Session, sessData);
                    user.LastAccessProfileCode = model.UserProfileCode;
                    _userRepository.Save(user);

                    return RedirectToAction("Index", "DocTemplateList");
                }
                if (groupId == null && !isManager)
                {
                    sessData.UserGroupId = 0;
                    sessData.UserGroupName = null;
                    sessData.UserType = UserProfileTypes.User;
                    sessData.CurrentProfileName = "Пользователь";

                    SessionHelper.SetUserSessionData(Session, sessData);
                    user.LastAccessProfileCode = model.UserProfileCode;
                    _userRepository.Save(user);

                    return RedirectToAction("Index", "Account");
                }
                if (groupId != null && isManager)
                {
                    sessData.UserGroupId = groupId.Value;
                    sessData.UserGroupName =
                    user.ManagedGroups.First(
                            x => x.usergroupid == groupId.Value).GroupName;
                    sessData.UserType = UserProfileTypes.Groupmanager;
                    sessData.CurrentProfileName = "Менеджер " + sessData.UserGroupName;

                    SessionHelper.SetUserSessionData(Session, sessData);
                    user.LastAccessProfileCode = model.UserProfileCode;
                    _userRepository.Save(user);

                    return RedirectToAction("Index", "ManagerDocument");
                }
                if (groupId != null && !isManager)
                {
                    sessData.UserGroupId = groupId.Value;
                    sessData.UserGroupName =
                    user.MemberGroups.First(
                            x => x.usergroupid == groupId.Value).GroupName;
                    sessData.UserType = UserProfileTypes.Groupuser;
                    sessData.CurrentProfileName = "Участник " + sessData.UserGroupName;

                    SessionHelper.SetUserSessionData(Session, sessData);
                    user.LastAccessProfileCode = model.UserProfileCode;
                    _userRepository.Save(user);

                    return RedirectToAction("UserDocAppointments", "UserDocument");
                }                
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Get: /Account/EditUserInfo
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditUserInfo()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);   
            return View(new EditUserAccountForUserModel(_userRepository.GetById(sessData.UserId)));
        }

        /// <summary>
        /// POST: /Account/EditUserInfo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult EditUserInfo(EditUserAccountForUserModel model)
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
