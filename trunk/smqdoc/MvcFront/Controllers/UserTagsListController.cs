using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.Models;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Controllers
{
    public class UserTagsListController : Controller
    {
        private readonly IUserAccountRepository _userRepository;

        public UserTagsListController(IUserAccountRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //
        // GET: /UserAccountList/

        public ActionResult Index()
        {
            return View(_userRepository.GetAllUserTags().Where(x=>x.Status != (int)UserTagStatus.Deleted).ToList()
                .ConvertAll(UserTagsListViewModel.UserTagNamesToModelConverter).ToList());
        }
        
        //
        // GET: /UserAccountList/Create

        public ActionResult Create()
        {
            return View(new UserTagsEditModel(_userRepository.GetUserTagByID(0)));
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
                    var tag = _userRepository.GetUserTagByID(0);
                    tag = model.Update(tag);
                    if (!_userRepository.SaveUserTag(tag))
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
            return View(new UserTagsEditModel(_userRepository.GetUserTagByID(id)));
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
                    var tag = _userRepository.GetUserTagByID(model.UserTagNameId);
                    tag = model.Update(tag);
                    if (!_userRepository.SaveUserTag(tag))
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
                ModelState.AddModelError("Ошибка при сохранении", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return View();
        }

        //
        // GET: /UserAccountList/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(new UserTagsEditModel(_userRepository.GetUserTagByID(id)));
        }

        //
        // POST: /UserAccountList/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _userRepository.DeleteUserTag(id);
 
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Ошибка", ex.Message);
                return View();
            }
        }


    }
}
