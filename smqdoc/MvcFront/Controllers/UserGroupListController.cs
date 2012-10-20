using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using MvcFront.Models;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{   
    public class UserGroupListController : Controller
    {
        private readonly IUserGroupRepository _groupRepository;
        public UserGroupListController( IUserGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        //
        // GET: /UserGroup/
        [AdminAuthorize]
        public ActionResult Index()
        {
            try
            {
                return View(_groupRepository.GetAll().Where(x => x.Status != (int) UserGroupStatus.Deleted).ToList()
                                .Select(
                                    x =>
                                    new UserGroupListViewModel
                                        {
                                            GroupId = x.usergroupid,
                                            Manager =
                                                x.Manager.SecondName + " " + x.Manager.FirstName + " " + x.Manager.LastName +
                                                " (" + x.Manager.Login + ")",
                                            GroupName = x.GroupName,
                                            Status = x.GroupStatusText
                                        }).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Index()", ex);
                return View(new List<UserGroupListViewModel>());
            }
        }

        //Возращает список пользователей группы
        [GridAction]
        public ActionResult _GroupUsersList(int groupId)
        {
            try
            {
                var data =
                    _groupRepository.GetById(groupId).Members.Where(x => x.Status != (int) UserAccountStatus.Deleted).
                        ToList()
                        .ConvertAll(UserAccountListViewModel.UserAccountToModelConverter).ToList();
                return View(new GridModel<UserAccountListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController._GroupUsersList()", ex);
                return View(new GridModel<UserAccountListViewModel> { Data = new List<UserAccountListViewModel>() });
            }
        }

        // GET: /UserGroup/Details/5
        [GroupManagerAuthorize]
        public ActionResult DetailsForManager()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                return View(_groupRepository.GetById(sessData.UserGroupId));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.DetailsForManager()", ex);
                return View(new UserGroup());
            }
        }

        //
        // GET: /UserGroup/Details/5
        [AdminAuthorize]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_groupRepository.GetById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Details()", ex);
                return View(new UserGroup());
            }
        }

        //
        // GET: /UserGroup/Create
        [AdminAuthorize]
        public ActionResult Create()
        {
            try
            {
                return View(new UserGroupEditModel(_groupRepository.GetById(0)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Create()", ex);
                return View(new UserGroupEditModel());
            }
        } 

        //
        // POST: /UserGroup/Create
        [AdminAuthorize]
        [HttpPost]
        public ActionResult Create(UserGroupEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var group = _groupRepository.GetById(0);
                    group = model.Update(group);
                    if (!_groupRepository.Save(group))
                    {
                        throw new Exception("Ошибка сохранения группы");
                    }
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Create()", ex);
                return View(model);
            }
        }
        
        //
        // GET: /UserGroup/Edit/5
        [AdminAuthorize]
        public ActionResult Edit(int id)
        {
            try
            {
                return View(new UserGroupEditModel(_groupRepository.GetById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Edit()", ex);
                return View(new UserGroupEditModel());
            }
        }

        //
        // POST: /UserGroup/Edit/5
        [AdminAuthorize]
        [HttpPost]
        public ActionResult Edit(UserGroupEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var group = _groupRepository.GetById(model.GroupId);
                    group = model.Update(group);
                    if (!_groupRepository.Save(group))
                    {
                        throw new Exception("Ошибка сохранения группы");
                    }
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Edit()", ex);
                return View(model);
            }
        }

        //
        // GET: /UserGroup/Delete/5
        [AdminAuthorize]
        public ActionResult Delete(int id)
        {
            try
            {
                return View(_groupRepository.GetById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Delete()", ex);
                return View(new UserGroup());
            }
        }

        //
        // POST: /UserGroup/Delete/5
        [AdminAuthorize]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _groupRepository.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.Delete()", ex);
                return View(new UserGroup());
            }
        }
        public ActionResult ChangeState(int id)
        {
            return View(_groupRepository.GetById(id));
        }

        [AdminAuthorize]
        [HttpPost]
        public ActionResult ChangeState(int id, FormCollection collection)
        {
            try
            {
                _groupRepository.ChangeState(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.ChangeState()", ex);
                return View(new UserGroup());
            }
        }

        [AdminAuthorize]
        public ActionResult GroupUsersManagment(int id)
        {
            try
            {
                return View(_groupRepository.GetById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.GroupUsersManagment()", ex);
                return View(new UserGroup());
            }
        }

        [AdminAuthorize]
        public JsonResult DeleteUser(int id, int UserId)
        {
            try
            {
                return Json(new {result = _groupRepository.RemoveMember(id, UserId)});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.DeleteUser()", ex);
                return new JsonResult{Data = false};
            }
        }

        [AdminAuthorize]
        public JsonResult AddUser(int groupId, int userId)
        {
            try
            {
                return Json(new {result = _groupRepository.AddMember(groupId, userId)});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserGroupListController.AddUser()", ex);
                return new JsonResult { Data = false };
            }
        }
    }
}
