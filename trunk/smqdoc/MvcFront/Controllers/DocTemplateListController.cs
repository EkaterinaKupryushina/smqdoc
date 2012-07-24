using System;
using System.Linq;
using System.Web.Mvc;
using MvcFront.Enums;
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
        
        #region DocTemplates
        
        //
        // GET: /DocTemplate/
        public ActionResult Index()
        {
            return View(_templateRepository.GetAllDocTeplates().Where(x => x.Status != (int)DocTemplateStatus.Deleted).ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
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
            return View(new DocTemplateEditModel(_templateRepository.GetDocTemplateById(0)));
        } 

        //
        // POST: /DocTemplate/Create
        [HttpPost]
        public ActionResult Create(DocTemplateEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetDocTemplateById(0);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveDocTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Формы");
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
            return View(new DocTemplateEditModel(_templateRepository.GetDocTemplateById(id)));
        }

        //
        // POST: /DocTemplate/Edit/5
        [HttpPost]
        public ActionResult Edit(DocTemplateEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetDocTemplateById(model.DocTemplateId);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveDocTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Формы");
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

        #region JSon

        /// <summary>
        /// Возращает список полей форма
        /// </summary>
        /// <param name="templId"></param>
        /// <returns></returns>
        [GridAction]
        public ActionResult _FieldTemplateList(long templId)
        {
            var data = _templateRepository.GetDocTemplateById(templId).FieldTeplates.OrderBy(x => x.OrderNumber).ToList()
                .ConvertAll(FieldTemplateListViewModel.FieldToModelConverter);
            return View(new GridModel<FieldTemplateListViewModel> { Data = data });
        }

        /// <summary>
        /// Возращает список полей формы использованных для создания вычислимого поля
        /// </summary>
        /// <param name="docTemplID"></param>
        /// <param name="fieldTemplID"></param>
        /// <returns></returns>
        [GridAction]
        public ActionResult _FieldTemplateListUsedForCalc(long docTemplID, long fieldTemplID)
        {
            var tpl = _templateRepository.GetFieldTemplateById(fieldTemplID);
            var lst = _templateRepository.GetAllComputableFieldTempalteParts().Where(x => x.FieldTemplate_fieldteplateid == tpl.fieldteplateid).ToList();
            var userTemplatesIDs = lst.Select(item => item.fkCalculatedFieldTemplateID).ToList();

            var data = _templateRepository.GetDocTemplateById(docTemplID).FieldTeplates.Where(x => userTemplatesIDs.Contains((int)x.fieldteplateid)).OrderBy(x => x.OrderNumber).ToList()
                .ConvertAll(FieldTemplateListViewModel.FieldToModelConverter);
            return View(new GridModel<FieldTemplateListViewModel> { Data = data });
        }

        /// <summary>
        /// Возращает список полей формы не использованных доступных для создания вычислимого поля
        /// </summary>
        /// <param name="docTemplID"></param>
        /// <param name="fieldTemplID"></param>
        /// <returns></returns>
        [GridAction]
        public ActionResult _FieldTemplateListNotUsedForCalc(long docTemplID, long fieldTemplID)
        {

            var tpl = _templateRepository.GetFieldTemplateById(fieldTemplID);
            var lst = _templateRepository.GetAllComputableFieldTempalteParts().Where(x => x.FieldTemplate_fieldteplateid == tpl.fieldteplateid).ToList();
            var userTemplatesIDs = lst.Select(item => item.fkCalculatedFieldTemplateID).ToList();

            var data = _templateRepository.GetDocTemplateById(docTemplID).FieldTeplates.Where(x => !userTemplatesIDs.Contains((int)x.fieldteplateid) && x.FiledType == (int)FieldTemplateType.Number ).OrderBy(x => x.OrderNumber).ToList()
                .ConvertAll(FieldTemplateListViewModel.FieldToModelConverter);

            return View(new GridModel<FieldTemplateListViewModel> { Data = data });
        }

        /// <summary>
        /// Запрос списка доступных полей для которых можно сделать планируемое поле
        /// </summary>
        /// <param name="docTemplID"></param>
        /// <param name="factFieldId"> </param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _FactFieldTemplatesList(long docTemplID, long? factFieldId)
        {
            var data =
                _templateRepository.GetDocTemplateById(docTemplID).FieldTeplates.Where(x =>
                    (x.FiledType != (int)FieldTemplateType.Planned && x.PlanFieldTemplates.Any()) || (factFieldId.HasValue && x.fieldteplateid == factFieldId));

            return new JsonResult { Data = new SelectList(data.ToList().Select(x => new { Id = x.fieldteplateid, Name = x.FieldName }), "Id", "Name", factFieldId) };
        }

        #endregion

        #endregion

        #region FieldTemplates

        public ActionResult DocTemplateFieldsManagment(long id)
        {
            return View(_templateRepository.GetDocTemplateById(id));
        }

        [HttpGet]
        public ActionResult AddField(long id)
        {
            var field = _templateRepository.GetFieldTemplateById(0);

            field.DocTemplate_docteplateid = id;
            field.DocTemplate = _templateRepository.GetDocTemplateById(id);
            var fModel = new FieldTemplateEditModel(field);
            return View(fModel);
        }

        [HttpPost]
        public ActionResult AddField(FieldTemplateEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetFieldTemplateById(0);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveFieldTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Формы");
                    }
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("DocTemplateFieldsManagment", new { id = model.DocTemplateId });
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
            return View(new FieldTemplateEditModel(_templateRepository.GetFieldTemplateById(id)));
        }

        [HttpPost]
        public ActionResult EditField(FieldTemplateEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var templ = _templateRepository.GetFieldTemplateById(model.FieldTemplateId);
                    templ = model.Update(templ);
                    if (!_templateRepository.SaveFieldTemplate(templ))
                    {
                        throw new Exception("Ошибка сохранения Формы");
                    }
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("DocTemplateFieldsManagment", new { id = model.DocTemplateId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ошибка при сохранении", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return View();
        }

        #region JSon

        public JsonResult DeleteField(long id, long fieldTemplateId)
        {
            _templateRepository.DeleteFieldTemplate(fieldTemplateId);
            return Json(new { result = true });
        }

        public JsonResult AddFieldToCalc(long id, long fieldTemplateId)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);
            entity.ComputableFieldTemplateParts.Add(new ComputableFieldTemplateParts { FieldTemplate = entity, fkCalculatedFieldTemplateID = fieldTemplateId });
            _templateRepository.SaveFieldTemplate(entity);

            return Json(new { result = true });
        }

        public JsonResult DeleteFieldFromCalc(long id, long fieldTemplateId)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);
            _templateRepository.SaveFieldTemplate(entity);
            return Json(new { result = true });
        }

        public JsonResult UpField(long id)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);
            if (entity.OrderNumber > 1)
            {
                _templateRepository.SetFieldTemplateNumber(entity.fieldteplateid, entity.OrderNumber - 1);
            }
            return Json(new { result = true });
        }

        public JsonResult DownField(long id)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);

            _templateRepository.SetFieldTemplateNumber(entity.fieldteplateid, entity.OrderNumber + 1);

            return Json(new { result = true });
        }

        #endregion

        #endregion
    }
}
