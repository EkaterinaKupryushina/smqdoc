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
            return View(new DocTemplateListEditModel(_templateRepository.GetDocTemplateById(0)));
        } 

        //
        // POST: /DocTemplate/Create

        [HttpPost]
        public ActionResult Create(DocTemplateListEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetDocTemplateById(0);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveDocTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Шаблона");
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
        // GET: /DocTemplate/Edit/5
 
        public ActionResult Edit(long id)
        {
            return View(new DocTemplateListEditModel(_templateRepository.GetDocTemplateById(id)));
        }

        //
        // POST: /DocTemplate/Edit/5

        [HttpPost]
        public ActionResult Edit(DocTemplateListEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetDocTemplateById(model.DocTemplateId);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveDocTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Шаблона");
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
        // GET: /DocTemplate/Delete/5
 
        public ActionResult Delete(long id)
        {
            return View(_templateRepository.GetDocTemplateById(id));
        }

        //
        // POST: /DocTemplate/Delete/5

        [HttpPost]
        public ActionResult Delete(long id, FormCollection collection)
        {
            try
            {
                _templateRepository.DeleteDocTemplate(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ошибка", ex.Message);
                return View();
            }
        }

        public ActionResult DocTemplateFieldsManagment(long id)
        {
            return View(_templateRepository.GetDocTemplateById(id));
        }

        public JsonResult DeleteField(long id,long FieldTemplateId)
        {
            _templateRepository.DeleteFieldTemplate(FieldTemplateId);
            return Json(new { result = true });
        }

        [HttpGet]
        public ActionResult AddField(long id)
        {
            var field = _templateRepository.GetFieldTemplateById(0);
            field.DocTemplate_docteplateid = id;
            var fModel = new FieldTemplateListEditModel(field);
            return View(fModel);
        }

        [HttpPost]
        public ActionResult AddField(FieldTemplateListEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetFieldTemplateById(0);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveFieldTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Шаблона");
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


        [HttpGet]
        public ActionResult EditField(long id)
        {
            return View(new FieldTemplateListEditModel(_templateRepository.GetFieldTemplateById(id)));
        }

        [HttpPost]
        public ActionResult EditField(FieldTemplateListEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetFieldTemplateById(model.FieldTemplateId);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveFieldTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Шаблона");
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
