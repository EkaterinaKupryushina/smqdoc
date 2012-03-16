using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.Interfaces;
using MvcFront.Models;
using MvcFront.DB;

namespace MvcFront.Controllers
{
    public class UserAccountListController : Controller
    {
        private readonly IUserAccountRepository _userRepository;

        public UserAccountListController(IUserAccountRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //
        // GET: /UserAccountList/

        public ActionResult Index()
        {
            return View(_userRepository.GetAll().Where(x=>x.Status != (int)UserAccountStatus.Deleted).ToList()
                .ConvertAll(new Converter<UserAccount,UserAccountListViewModel>(UserAccountListViewModel.UserAccountToModelConverter)).ToList());
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
            return View(new UserAccountEditViewModel(_userRepository.GetById(0)));
        } 

        //
        // POST: /UserAccountList/Create

        [HttpPost]
        public ActionResult Create(UserAccountEditViewModel model)
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
            return View(new UserAccountEditViewModel(_userRepository.GetById(id)));
        }

        //
        // POST: /UserAccountList/Edit/5

        [HttpPost]
        public ActionResult Edit(UserAccountEditViewModel model)
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
    }
}
