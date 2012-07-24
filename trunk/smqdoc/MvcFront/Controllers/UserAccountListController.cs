﻿using System;
using System.Linq;
using System.Web.Mvc;
using MvcFront.Enums;
using MvcFront.Interfaces;
using MvcFront.Models;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
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
            return View(_userRepository.GetAll().Where(x=>x.Status != (int)UserAccountStatus.Deleted).ToList()
                .ConvertAll(UserAccountListViewModel.UserAccountToModelConverter).ToList());
        }

        //
        // GET: /UserAccountList/Details/5

        public ActionResult Details(int id)
        {
            return View(_userRepository.GetById(id));
        }

        //
        // GET: /UserAccountList/Create

        public ActionResult Create()
        {
            return View(new UserAccountEditModel(_userRepository.GetById(0)));
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
            catch(Exception ex)
            {
                ModelState.AddModelError("Ошибка при сохранении", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return View();
        }
        
        //
        // GET: /UserAccountList/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(new UserAccountEditModel(_userRepository.GetById(id)));
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
                ModelState.AddModelError("Ошибка при сохранении", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return View();
        }

        //
        // GET: /UserAccountList/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(_userRepository.GetById(id));
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
            catch(Exception ex)
            {
                ModelState.AddModelError("Ошибка", ex.Message);
                return View();
            }
        }

        public ActionResult ChangeState(int id)
        {
            return View(_userRepository.GetById(id));
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
                ModelState.AddModelError("Ошибка", ex.Message);
                return View();
            }
        }


        public ActionResult TagsUserManagement(int id)
        {
            return View( _userRepository.GetById(id));
        }

        public JsonResult RemoveUserTag(int id, int tagId)
        {            
            return Json(new { result = _userTagRepository.RemoveUserTag(id, tagId) });
        }

        public JsonResult AddUserTag(int userId, int tagId)
        {            
            return Json(new { result = _userTagRepository.AddUserTag(userId, tagId) });
        }

        //список меток пользователя
        [GridAction]
        public ActionResult _UserTagList(int userId)
        {            
            var data = _userRepository.GetById(userId).UserTags.Where(x => x.Status != (int)UserTagStatus.Deleted).ToList()
                .ConvertAll(UserTagsListViewModel.UserTagNamesToModelConverter).ToList();

            return View(new GridModel<UserTagsListViewModel> { Data = data });
        }
    }
}