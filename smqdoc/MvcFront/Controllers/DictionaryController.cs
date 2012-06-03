using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Interfaces;
using MvcFront.Helpers;
using MvcFront.Models;

namespace MvcFront.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IUserAccountRepository _userRepository;

        public DictionaryController(IUserAccountRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #region Пользователи

        /// <summary>
        ///   Загрузка списка пользователей по заданной маске.
        /// </summary>
        /// <param name="text"> Маска для поиска по названию. </param>
        [HttpPost]
        public ActionResult AjaxUserAccountList(string text)
        {
            var data = _userRepository.GetAll();
            if (!String.IsNullOrEmpty(text))
            {
                data = data.Where(p => p.Login != null && p.Login.ToLower().Contains(text.ToLower()) 
                    || p.FirstName.Contains(text.ToLower()) || p.LastName.Contains(text.ToLower()) || p.SecondName.Contains(text.ToLower())).Take(20);
            }
            return new JsonResult { Data = new SelectList(data.ToList().Select(x => new { Id = x.userid,Name = x.FullName + " ("+x.Login+")"}), "Id", "Name") };
        }
        [HttpPost]
        public ActionResult AjaxUserAccountProfiles()
        {
            var sessData = SessionHelper.GetUserSessionData(Session);
            if (sessData == null)
                return new HttpUnauthorizedResult();
            var user = _userRepository.GetById(sessData.UserId);

            var profDicts = new Dictionary<string, string>();
            if(user.IsAdmin && sessData.UserType!= SmqUserProfileType.Systemadmin) profDicts.Add(SessionHelper.GenerateUserProfileCode(null,true),"Администратор");
            foreach (var mgroup in user.ManagedGroups.Where(x=>x.Status == (int)UserGroupStatus.Active))
            {
                if(!(mgroup.usergroupid == sessData.UserGroupId && sessData.UserType == SmqUserProfileType.Groupmanager))
                    profDicts.Add(SessionHelper.GenerateUserProfileCode(mgroup.usergroupid, true), "Менеджер " + mgroup.GroupName);
            }
            foreach (var mgroup in user.MemberGroups.Where(x => x.Status == (int)UserGroupStatus.Active))
            {
                if (!(mgroup.usergroupid == sessData.UserGroupId && sessData.UserType == SmqUserProfileType.Groupuser))
                 profDicts.Add(SessionHelper.GenerateUserProfileCode(mgroup.usergroupid, false), "Участник " + mgroup.GroupName);
            }

            if (profDicts.Count == 0 ) profDicts.Add(SessionHelper.GenerateUserProfileCode(null, false), "Пользователь");
            return new JsonResult { Data = new SelectList(profDicts.Select(x => new {Id = x.Key, Name = x.Value}).ToList(),"Id","Name")};

        }
        #endregion


        [HttpPost]
        public ActionResult AjaxUserGroupList()
        {
            var repGroup = DependencyResolver.Current.GetService<IUserGroupRepository>();
            var model = repGroup.GetAll().Where(x => x.Status != (int)UserGroupStatus.Deleted)
                        .Select(x => new UserGroupListViewModel { GroupId = x.usergroupid, Manager = x.Manager.SecondName + " " + x.Manager.FirstName + " " + x.Manager.LastName + " (" + x.Manager.Login + ")", GroupName = x.GroupName }).ToList();

            return new JsonResult { Data = new SelectList(model, "GroupId", "GroupName") };            
        }

        [HttpPost]
        public ActionResult AjaxDocTemplateList()
        {
            var repTemplates = DependencyResolver.Current.GetService<IDocTemplateRepository>();
            var model = repTemplates.GetAllDocTeplates().Where(x => x.Status != (int)DocTemplateStatus.Deleted).ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList();

            return new JsonResult { Data = new SelectList(model, "DocTemplateId", "DocTemplateName") };
        }

        [HttpPost]
        public ActionResult AjaxUserTagsList(string text)
        {
            var data = _userRepository.GetAllUserTags();
            if (!String.IsNullOrEmpty(text))
            {
                data = data.Where(p => p.Name != null && p.Name.ToLower().Contains(text.ToLower())).Take(20);
            }
            return new JsonResult { Data = new SelectList(data.ToList().Select(x => new { Id = x.Id, Name = x.Name}), "Id", "Name") };
        }
    }
}
