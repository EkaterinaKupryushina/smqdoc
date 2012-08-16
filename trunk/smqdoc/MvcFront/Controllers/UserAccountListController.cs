using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using MvcFront.Models;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    [AdminAuthorize]
    public class UserAccountListController : Controller
    {
        private readonly IUserAccountRepository _userRepository;
        private readonly IUserTagRepository _userTagRepository;
        public UserAccountListController(IUserAccountRepository userRepository, IUserTagRepository userTagRepository)
        {
            _userRepository = userRepository;
            _userTagRepository = userTagRepository;
        }
        //
        // GET: /UserAccountList/

        public ActionResult Index()
        {
            try
            {
                return View(_userRepository.GetAll().Where(x => x.Status != (int) UserAccountStatus.Deleted).ToList()
                                .ConvertAll(UserAccountListViewModel.UserAccountToModelConverter).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Index()", ex);
                return View(new List<UserAccountListViewModel>());
            }
        }

        //
        // GET: /UserAccountList/Details/5

        public ActionResult Details(int id)
        {
            try
            {
                return View(_userRepository.GetById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Details()", ex);
                return View(new UserAccount());
            }
        }

        //
        // GET: /UserAccountList/Create

        public ActionResult Create()
        {
            try
            {
                return View(new UserAccountEditModel(_userRepository.GetById(0)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Create()", ex);
                return View(new UserAccountEditModel());
            }
        } 

        //
        // POST: /UserAccountList/Create

        [HttpPost]
        public ActionResult Create(UserAccountEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userRepository.GetById(0);
                    user = model.Update(user);
                    if (!_userRepository.Save(user))
                    {
                        throw new Exception("Ошибка сохранения пользователя");
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Create()", ex);
                return View(model);
            }
        }
        
        //
        // GET: /UserAccountList/Edit/5
 
        public ActionResult Edit(int id)
        {
            try
            {
                return View(new UserAccountEditModel(_userRepository.GetById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Edit()", ex);
                return View(new UserAccountEditModel());
            }
        }

        //
        // POST: /UserAccountList/Edit/5

        [HttpPost]
        public ActionResult Edit(UserAccountEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userRepository.GetById(model.UserId);
                    model.Update(user);
                    if (!_userRepository.Save(user))
                    {
                        throw new Exception("Ошибка сохранения пользователя");
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Edit()", ex);
                return View(model);
            }
        }

        //
        // GET: /UserAccountList/Delete/5
 
        public ActionResult Delete(int id)
        {
            try
            {
                return View(_userRepository.GetById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Delete()", ex);
                return View(new UserAccount());
            }
        }

        //
        // POST: /UserAccountList/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _userRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.Delete()", ex);
                return View(new UserAccount());
            }
        }

        public ActionResult ChangeState(int id)
        {
            try
            {
                return View(_userRepository.GetById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.ChangeState()", ex);
                return View(new UserAccount());
            }
        }

        [HttpPost]
        public ActionResult ChangeState(int id, FormCollection collection)
        {
            try
            {
                _userRepository.ChangeState(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.ChangeState()", ex);
                return View(new UserAccount());
            }
        }


        public ActionResult TagsUserManagement(int id)
        {
            try
            {
                return View(_userRepository.GetById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.TagsUserManagement()", ex);
                return View(new UserAccount());
            }
        }

        public JsonResult RemoveUserTag(int id, int tagId)
        {
            try
            {
                return Json(new {result = _userTagRepository.RemoveUserTag(id, tagId)});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.RemoveUserTag()", ex);
                return new JsonResult { Data = false };
            }
        }

        public JsonResult AddUserTag(int userId, int tagId)
        {
            try
            {
                return Json(new {result = _userTagRepository.AddUserTag(userId, tagId)});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController.AddUserTag()", ex);
                return new JsonResult { Data = false };
            }
        }

        //список меток пользователя
        [GridAction]
        public ActionResult _UserTagList(int userId)
        {
            try
            {
                var data = _userRepository.GetById(userId).UserTags.ToList()
                    .ConvertAll(UserTagsListViewModel.UserTagNamesToModelConverter).ToList();

                return View(new GridModel<UserTagsListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserAccountListController._UserTagList()", ex);
                return View(new GridModel<UserTagsListViewModel> {Data = new List<UserTagsListViewModel>()});
            }
        }
    }
}
