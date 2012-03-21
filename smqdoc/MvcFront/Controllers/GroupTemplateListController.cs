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
    public class GroupTemplateListController : Controller
    {
        private readonly IGroupTemplateRepository _groupTemplateRepository;
        public GroupTemplateListController(IGroupTemplateRepository groupTemplateRepository)
        {
            _groupTemplateRepository = groupTemplateRepository;
        }

        // GET: /GroupTemplate/

        public ActionResult Index()
        {            
            return View(_groupTemplateRepository.GetAllGroupTemplates().Where(x => x.Status != (int)GroupTemplateStatus.Deleted).ToList().ConvertAll(GroupTemplateListViewModel.GroupTemplateToModelConverter).ToList());
        }
                 
        public ActionResult Delete(int id)
        {
            _groupTemplateRepository.DeleteGroupTemplate(id);            
            return RedirectToAction("Index");
        }



        public ActionResult Create()
        {
            var model = new GroupTemplateEditViewModel(_groupTemplateRepository.GetGroupTemplateById(0));
            //var repGroup = DependencyResolver.Current.GetService<IUserGroupRepository>();
            //var repTemplates = DependencyResolver.Current.GetService<IDocTemplateRepository>();
            
            //model.DocTemplateLst = repTemplates.GetAllDocTeplates().Where(x => x.Status != (int)DocTemplateStatus.Deleted).ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList();
            //model.UserGroupLst = repGroup.GetAll().Where(x => x.Status != (int)UserGroupStatus.Deleted)
            //            .Select(x => new UserGroupListViewModel { GroupId = x.usergroupid, Manager = x.Manager.SecondName + " " + x.Manager.FirstName + " " + x.Manager.LastName + " (" + x.Manager.Login + ")", GroupName = x.GroupName }).ToList();

            return View(model);
        }

        //
        // POST: /UserGroup/Create
        
        [HttpPost]
        public ActionResult Create(GroupTemplateEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var groupTemplate = _groupTemplateRepository.GetGroupTemplateById(0);
                    groupTemplate = model.Update(groupTemplate);
                    if (!_groupTemplateRepository.SaveGroupTemplate(groupTemplate))
                    {
                        throw new Exception("Ошибка сохранения связи");
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
    }
}
