using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.Interfaces;
using MvcFront.DB;
using MvcFront.Models;

namespace MvcFront.Controllers
{
    public class UserGroupListController : Controller
    {
        private readonly IUserAccountRepository _userRepository;
        private readonly IUserGroupRepository _groupRepository;
        public UserGroupListController(IUserAccountRepository userRepository, IUserGroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }
        //
        // GET: /UserGroup/

        public ActionResult Index()
        {
            return View(_groupRepository.GetAll().Where(x => x.Status != (int)UserGroupStatus.Deleted)
                .Select(x => new UserGroupListViewModel() { GroupId = x.usergroupid, Manager =  x.Manager.SecondName +" "+ x.Manager.FirstName +" " + x.Manager.LastName + " ("+ x.Manager.Login+")", GroupName = x.GroupName }).ToList());
        }

        //
        // GET: /UserGroup/Details/5

        public ActionResult Details(int id)
        {
            return View(_groupRepository.GetById(id));
        }

        //
        // GET: /UserGroup/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /UserGroup/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /UserGroup/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /UserGroup/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /UserGroup/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(_groupRepository.GetById(id));
        }

        //
        // POST: /UserGroup/Delete/5

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
                ModelState.AddModelError("Ошибка", ex.Message);
                return View();
            }
        }
        public ActionResult ChangeState(int id)
        {
            return View(_groupRepository.GetById(id));
        }

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
                ModelState.AddModelError("Ошибка", ex.Message);
                return View();
            }
        }
    }
}
