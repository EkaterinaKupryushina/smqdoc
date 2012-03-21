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

        public JsonResult DeleteLink(long id)
        {
            _groupTemplateRepository.DeleteGroupTemplate(id);      
            return Json(new { result = true });
        }

        public ActionResult Create()
        {
            var model = new GroupTemplateEditViewModel(_groupTemplateRepository.GetGroupTemplateById(0));           
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


        [GridAction]
        public ActionResult _GroupTemplateList()
        {
            var data = _groupTemplateRepository.GetAllGroupTemplates().Where(x => x.Status != (int)GroupTemplateStatus.Deleted).ToList().ConvertAll(GroupTemplateListViewModel.GroupTemplateToModelConverter).ToList()
                ;
            //var data = _templateRepository.GetDocTemplateById(templId).FieldTeplates.Where(x => x.Status != (int)FieldTemplateStatus.Deleted).OrderBy(x => x.OrderNumber).ToList()
            //    .ConvertAll(FieldTemplateListViewModel.FieldToModelConverter);
            return View(new GridModel<GroupTemplateListViewModel> { Data = data });
        }
    }
}
