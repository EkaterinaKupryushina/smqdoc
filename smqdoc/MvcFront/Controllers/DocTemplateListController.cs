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
    public class DocTemplateListController : Controller
    {
        private readonly IDocTemplateRepository _templateRepository;
        public DocTemplateListController(IDocTemplateRepository templateRepository)
        {
            _templateRepository = templateRepository;
        }
        //
        // GET: /DocTemplate/

        public ActionResult Index()
        {
            return View(_templateRepository.GetAllDocTeplates().Where(x => x.Status != (int)DocTemplateStatus.Deleted).ToList().ConvertAll(new Converter<DocTemplate,DocTemplateListViewModel>(DocTemplateListViewModel.DocTemplateToModelConverter)).ToList());
        }

        //Возращает список полей шаблона
        [GridAction]
        public ActionResult _FieldTemplateList(long templId)
        {
            var data = _templateRepository.GetDocTemplateById(templId).FieldTeplates.Where(x => x.Status != (int)FieldTemplateStatus.Deleted).ToList()
                .ConvertAll(new Converter<FieldTemplate, FieldTemplateListViewModel>(FieldTemplateListViewModel.FieldToModelConverter));
            return View(new GridModel<FieldTemplateListViewModel> { Data = data });
        }

        //
        // GET: /DocTemplate/Details/5

        public ActionResult Details(long id)
        {
            return View(_templateRepository.GetDocTemplateById(id));
        }

        //
        // GET: /DocTemplate/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /DocTemplate/Create

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
        // GET: /DocTemplate/Edit/5
 
        public ActionResult Edit(long id)
        {
            return View();
        }

        //
        // POST: /DocTemplate/Edit/5

        [HttpPost]
        public ActionResult Edit(long id, FormCollection collection)
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
        // GET: /DocTemplate/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View();
        }

        //
        // POST: /DocTemplate/Delete/5

        [HttpPost]
        public ActionResult Delete(long id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
