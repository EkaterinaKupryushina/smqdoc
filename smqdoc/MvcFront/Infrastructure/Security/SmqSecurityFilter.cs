using System;
using System.Web.Mvc;
using MvcFront.Controllers;
using System.Linq;
using MvcFront.Entities;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using NLog;

namespace MvcFront.Infrastructure.Security
{
    public class SmqSecurityFilter : FilterAttribute, IAuthorizationFilter
    {
        private readonly IUserAccountRepository _userAccountRepository;

        public SmqSecurityFilter()
        {
            _userAccountRepository = DependencyResolver.Current.GetService<IUserAccountRepository>();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                //UserAccount user = null;
                //var action = (string)filterContext.RouteData.Values["action"];
                var controllerType = filterContext.Controller.GetType();
                //var routeData = filterContext.RouteData.Values;
                //var httpContext = filterContext.RequestContext.HttpContext;
                //Старницы авторизации

                if (controllerType == typeof(AccountController))
                    return;


                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                        SessionHelper.ClearUserSessionData(filterContext.HttpContext.Session);
                        filterContext.Result = new HttpUnauthorizedResult();
                }
                //Поулчаем данные из сессии
                var sessData = SessionHelper.GetUserSessionData(filterContext.HttpContext.Session);
                if(sessData == null)
                {
                    //Если в сессии пусто получаем пользователя
                    var user = _userAccountRepository.GetByLogin(filterContext.HttpContext.User.Identity.Name,true);
                    if(user!= null)
                    {
                        //Читаем код последнего профиля пользовтаеля
                        var userProfileType = UserProfileTypes.User;
                        var userProfileName = "Пользователь";
                        string userProfileGroupName = null;
                        var userProfileGroupId = 0;
                        int? groupId;
                        bool isManager;
                        SessionHelper.ParseUserProfileCode(user.LastAccessProfileCode,out groupId,out isManager);
                        if (groupId == null && isManager)
                        {
                            userProfileGroupId = 0;
                            userProfileType = UserProfileTypes.Systemadmin;
                            userProfileName = "Администратор";

                        }
                        if (groupId == null && !isManager)
                        {
                            userProfileGroupId = 0;
                            userProfileType = UserProfileTypes.User;
                            userProfileName = "Пользователь";

                        }
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
                        sessData = new UserSessionData {UserName = user.Login,UserId = user.userid,UserType = userProfileType,
                            CurrentProfileName = userProfileName,UserGroupName = userProfileGroupName,UserGroupId = userProfileGroupId};
                        SessionHelper.SetUserSessionData(filterContext.HttpContext.Session,sessData);
                        return;
                    }
                    SessionHelper.ClearUserSessionData(filterContext.HttpContext.Session);
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    if (sessData.UserName != filterContext.HttpContext.User.Identity.Name)
                    {
                        SessionHelper.ClearUserSessionData(filterContext.HttpContext.Session);
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }
            catch (Exception ex)
            {
                SessionHelper.ClearUserSessionData(filterContext.HttpContext.Session);
                filterContext.Result = new HttpUnauthorizedResult();
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "SmqSecurityFilter.OnAuthorization()", ex);
            }
        }
    }
}