using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.Infrastructure.Security;
using MvcFront.Models;
using MvcFront.Interfaces;
using NLog;

namespace MvcFront.Controllers
{
    [AdminAuthorize]
    public class UserTagsListController : Controller
    {
        private readonly IUserTagRepository _userTagRepository;
        public UserTagsListController( IUserTagRepository userTagRepository)
        {
            _userTagRepository = userTagRepository;
        }
        //
        // GET: /UserAccountList/

        public ActionResult Index()
        {
            try
            {
                return View(_userTagRepository.GetAllUserTags().ToList()
                                .ConvertAll(UserTagsListViewModel.UserTagNamesToModelConverter).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserTagsListController.Index()", ex);
                return View(new List<UserTagsListViewModel>());
            }
        }
        
        //
        // GET: /UserAccountList/Create

        public ActionResult Create()
        {
            try
            {
                return View(new UserTagsEditModel(_userTagRepository.GetUserTagById(0)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserTagsListController.Create()", ex);
                return View(new UserTagsEditModel());
            }
        } 

        //
        // POST: /UserAccountList/Create

        [HttpPost]
        public ActionResult Create(UserTagsEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tag = _userTagRepository.GetUserTagById(0);
                    tag = model.Update(tag);
                    if (!_userTagRepository.SaveUserTag(tag))
                    {
                        throw new Exception("Ошибка сохранения метки");
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserTagsListController.Create()", ex);
                return View(model);
            }
        }
        
        //
        // GET: /UserAccountList/Edit/5
 
        public ActionResult Edit(int id)
        {
            try
            {
                return View(new UserTagsEditModel(_userTagRepository.GetUserTagById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserTagsListController.Edit()", ex);
                return View(new UserTagsEditModel());
            }
        }

        //
        // POST: /UserAccountList/Edit/5

        [HttpPost]
        public ActionResult Edit(UserTagsEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tag = _userTagRepository.GetUserTagById(model.UserTagNameId);
                    tag = model.Update(tag);
                    if (!_userTagRepository.SaveUserTag(tag))
                    {
                        throw new Exception("Ошибка сохранения метки");
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserTagsListController.Edit()", ex);
                return View(model);
            }
        }

        //
        // GET: /UserAccountList/Delete/5
 
        public ActionResult Delete(int id)
        {
            try
            {
                return View(new UserTagsEditModel(_userTagRepository.GetUserTagById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserTagsListController.Delete()", ex);
                return View(new UserTagsEditModel());
            }
        }

        //
        // POST: /UserAccountList/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _userTagRepository.DeleteUserTag(id);
 
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserTagsListController.Delete()", ex);
                return View(new UserTagsEditModel());
            }
        }


    }
}
