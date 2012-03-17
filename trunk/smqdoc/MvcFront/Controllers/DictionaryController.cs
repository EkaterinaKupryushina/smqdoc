using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Interfaces;
using MvcFront.Helpers;

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
        public ActionResult AjaxUserAccountProfiles(int userId)
        {
            var user = _userRepository.GetById(userId);
            var profDicts = new Dictionary<string, string>();
            if(user.IsAdmin) profDicts.Add(SessionHelper.GenerateUserProfileCode(null,true),"Администратор");
            foreach (var mgroup in user.ManagedGroups.Where(x=>x.Status == (int)UserGroupStatus.Active))
            {
                profDicts.Add(SessionHelper.GenerateUserProfileCode(mgroup.usergroupid, true), "Менеджер " + mgroup.GroupName);
            }
            foreach (var mgroup in user.MemberGroups.Where(x => x.Status == (int)UserGroupStatus.Active))
            {
                profDicts.Add(SessionHelper.GenerateUserProfileCode(mgroup.usergroupid, false), "Участник " + mgroup.GroupName);
            }
            if (profDicts.Count == 0) profDicts.Add(SessionHelper.GenerateUserProfileCode(null, false), "Просто пользователь");
            return new JsonResult { Data = new SelectList(profDicts.Select(x => new { Id = x.Key, Name = x.Value}), "Code", "Name")};
        }
        #endregion
    }
}
