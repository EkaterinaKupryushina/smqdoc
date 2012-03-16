using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcFront.Interfaces;
using MvcFront.DB;
using MvcFront.Models;
using Telerik.Web.Mvc;

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
        //Возращает список пользователей группы
        [GridAction]
        public ActionResult _GroupUsersList(int groupId)
        {
            var data = _groupRepository.GetById(groupId).Members.Select(x => new UserAccountListViewModel() { UserId = x.userid, Login = x.Login, FullName = x.FirstName, LastLogin = x.LastAccess,CompositeId = x.userid+":"+groupId }).ToList();
            return View(new GridModel<UserAccountListViewModel> { Data = data });
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
            return View(new UserGroupEditViewModel(_groupRepository.GetById(0)));
        } 

        //
        // POST: /UserGroup/Create

        [HttpPost]
        public ActionResult Create(UserGroupEditViewModel model)
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
                ModelState.AddModelError("Ошибка при сохранении", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return View();
        }
        
        //
        // GET: /UserGroup/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View(new UserGroupEditViewModel(_groupRepository.GetById(id)));
        }

        //
        // POST: /UserGroup/Edit/5

        [HttpPost]
        public ActionResult Edit(UserGroupEditViewModel model)
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
                ModelState.AddModelError("Ошибка при сохранении", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return View();
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
        public ActionResult GroupUsersManagment(int id)
        {
            return View(_groupRepository.GetById(id));
        }
        public JsonResult DeleteUser(int id, int UserId)
        {
            //int userId = int.Parse(CompositeId.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString());
            //int groupId = int.Parse(CompositeId.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString());
            return Json(new { result = _groupRepository.RemoveMember(id, UserId) });
        }

        public JsonResult AddUser(int groupId, int userId)
        {
            //int userId = int.Parse(CompositeId.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString());
            //int groupId = int.Parse(CompositeId.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString());
            return Json(new { result = _groupRepository.AddMember(groupId, userId) });
        }
    }
}
