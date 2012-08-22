using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MvcFront.Enums;
using MvcFront.Infrastructure;
using MvcFront.Interfaces;
using MvcFront.Helpers;
using MvcFront.Models;
using NLog;

namespace MvcFront.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly IUserAccountRepository _userRepository;
        private readonly IUserTagRepository _userTagRepository;
        private readonly IDocumentRepository _documentRepository;

        public DictionaryController(IUserAccountRepository userRepository, IUserTagRepository userTagRepository,IDocumentRepository documentRepository)
        {
            _userRepository = userRepository;
            _userTagRepository = userTagRepository;
            _documentRepository = documentRepository;
        }

        #region Пользователи

        /// <summary>
        ///   Загрузка списка пользователей по заданной маске.
        /// </summary>
        /// <param name="text"> Маска для поиска по названию. </param>
        [HttpPost]
        public JsonResult AjaxUserAccountList(string text)
        {
            try
            {
                var data = _userRepository.GetAll();
                if (!String.IsNullOrEmpty(text))
                {
                    data = data.Where(p => p.Login != null && p.Login.ToLower().Contains(text.ToLower())
                                           || p.FirstName.Contains(text.ToLower()) ||
                                           p.LastName.Contains(text.ToLower()) || p.SecondName.Contains(text.ToLower()))
                        .Take(20);
                }
                return new JsonResult
                           {
                               Data =
                                   new SelectList(
                                   data.ToList().Select(
                                       x => new { Id = x.userid, Name = x.FullName + " (" + x.Login + ")" }), "Id", "Name")
                           };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController.AjaxUserAccountList()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Загрузка списка доступных пользователю профилей
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxUserAccountProfiles()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                if (sessData == null)
                    return new HttpUnauthorizedResult();
                var user = _userRepository.GetById(sessData.UserId);

                var profDicts = new Dictionary<string, string>();
                if (user.IsAdmin && sessData.UserType != UserProfileTypes.Systemadmin) profDicts.Add(SessionHelper.GenerateUserProfileCode(null, true), "Администратор");
                foreach (var mgroup in user.ManagedGroups.Where(x => x.Status == (int)UserGroupStatus.Active))
                {
                    if (!(mgroup.usergroupid == sessData.UserGroupId && sessData.UserType == UserProfileTypes.Groupmanager))
                        profDicts.Add(SessionHelper.GenerateUserProfileCode(mgroup.usergroupid, true), "Менеджер " + mgroup.GroupName);
                }
                foreach (var mgroup in user.MemberGroups.Where(x => x.Status == (int)UserGroupStatus.Active))
                {
                    if (!(mgroup.usergroupid == sessData.UserGroupId && sessData.UserType == UserProfileTypes.Groupuser))
                        profDicts.Add(SessionHelper.GenerateUserProfileCode(mgroup.usergroupid, false), "Участник " + mgroup.GroupName);
                }

                if (profDicts.Count == 0) profDicts.Add(SessionHelper.GenerateUserProfileCode(null, false), "Пользователь");
                return new JsonResult { Data = new SelectList(profDicts.Select(x => new { Id = x.Key, Name = x.Value }).ToList(), "Id", "Name") };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController.AjaxUserAccountProfiles()", ex);
                return new JsonResult { Data = false };
            }
        }

        #endregion

        /// <summary>
        /// Запрос списка групп
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxUserGroupList()
        {
            try
            {
                var groupRepository = DependencyResolver.Current.GetService<IUserGroupRepository>();
                var model = groupRepository.GetAll().Where(x => x.Status != (int) UserGroupStatus.Deleted)
                    .Select(
                        x =>
                        new UserGroupListViewModel
                            {
                                GroupId = x.usergroupid,
                                Manager =
                                    x.Manager.SecondName + " " + x.Manager.FirstName + " " + x.Manager.LastName + " (" +
                                    x.Manager.Login + ")",
                                GroupName = x.GroupName
                            }).ToList();

                return new JsonResult {Data = new SelectList(model, "GroupId", "GroupName")};
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController.AjaxUserGroupList()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Запрос списка всех неудаленных DocTemplates
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxDocTemplateList()
        {
            try
            {
                var docTemplateRepository = DependencyResolver.Current.GetService<IDocTemplateRepository>();
                var model =
                    docTemplateRepository.GetAllDocTeplates().Where(x => x.Status != (int) DocTemplateStatus.Deleted).
                        ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList();

                return new JsonResult {Data = new SelectList(model, "DocTemplateId", "DocTemplateName")};
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController.AjaxDocTemplateList()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Запрос списка Тэгов пользователей содержащих текст
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxUserTagsList(string text)
        {
            try
            {
                var data = _userTagRepository.GetAllUserTags();
                if (!String.IsNullOrEmpty(text))
                {
                    data = data.Where(p => p.Name != null && p.Name.ToLower().Contains(text.ToLower())).Take(20);
                }
                return new JsonResult
                           {Data = new SelectList(data.ToList().Select(x => new {x.Id, x.Name}), "Id", "Name")};
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController.AjaxUserTagsList()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Возвращает Файл ассета
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public FileStreamResult DownloadDocAttachment(long documentId)
        {
            try
            {
                var document = _documentRepository.GetDocumentById(documentId);
                return
                    File(new FileStream(Path.Combine(SmqSettings.Instance.AssetFolder, document.StoredFileName), FileMode.Open),
                        "application", document.DisplayFileName);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController.Download()", ex);
                return null;
            }
        }

    }
}
