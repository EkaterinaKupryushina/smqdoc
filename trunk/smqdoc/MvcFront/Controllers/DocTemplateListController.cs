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
        
       /// <summary>
       /// Страница со списком форм
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            return View(_templateRepository.GetAllDocTeplates().Where(x => x.Status != (int)DocTemplateStatus.Deleted).ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
        }

        /// <summary>
        /// Просмотр свойств формы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(long id)
        {
            return View(_templateRepository.GetDocTemplateById(id));
        }

        /// <summary>
        /// Создание формы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View(new DocTemplateEditModel(_templateRepository.GetDocTemplateById(0)));
        } 

        /// <summary>
        /// Создание формы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Редактирование Формы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(long id)
        {
            return View(new DocTemplateEditModel(_templateRepository.GetDocTemplateById(id)));
        }

        /// <summary>
        /// Редактирование формы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаление формы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(long id)
        {
            return View(_templateRepository.GetDocTemplateById(id));
        }

        /// <summary>
        /// Удаление формы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
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

        #endregion

        #region FieldTemplates

        /// <summary>
        /// страниция редактирования списка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DocTemplateFieldsManagment(long id)
        {
            return View(_templateRepository.GetDocTemplateById(id));
        }

        /// <summary>
        /// Добавление поля в форму
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddField(long id)
        {
            var field = _templateRepository.GetFieldTemplateById(0);

            field.DocTemplate_docteplateid = id;
            field.DocTemplate = _templateRepository.GetDocTemplateById(id);
            var fModel = new FieldTemplateEditModel(field);
            return View(fModel);
        }

        /// <summary>
        /// Добавление поля в форму
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Редактирование поля
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditField(long id)
        {
            return View(new FieldTemplateEditModel(_templateRepository.GetFieldTemplateById(id)));
        }

        /// <summary>
        /// Редактирования поля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Ajax  удаление поля
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldTemplateId"></param>
        /// <returns></returns>
        public JsonResult DeleteField(long id, long fieldTemplateId)
        {
            _templateRepository.DeleteFieldTemplate(fieldTemplateId);
            return Json(new { result = true });
        }

        /// <summary>
        /// Ajax добавление поля в список вычислимых
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldTemplateId"></param>
        /// <returns></returns>
        public JsonResult AddFieldToCalc(long id, long fieldTemplateId)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);
            entity.ComputableFieldTemplateParts.Add(new ComputableFieldTemplateParts { FieldTemplate = entity, fkCalculatedFieldTemplateID = fieldTemplateId });
            _templateRepository.SaveFieldTemplate(entity);

            return Json(new { result = true });
        }

        /// <summary>
        /// Ajax удаление поля из списка вычислимых
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldTemplateId"></param>
        /// <returns></returns>
        public JsonResult DeleteFieldFromCalc(long id, long fieldTemplateId)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);
            _templateRepository.SaveFieldTemplate(entity);
            return Json(new { result = true });
        }

        /// <summary>
        /// Поднять в списке поле 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpField(long id)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);
            if (entity.OrderNumber > 1)
            {
                _templateRepository.SetFieldTemplateNumber(entity.fieldteplateid, entity.OrderNumber - 1);
            }
            return Json(new { result = true });
        }

        /// <summary>
        /// Опустить поле в списке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DownField(long id)
        {
            var entity = _templateRepository.GetFieldTemplateById(id);

            _templateRepository.SetFieldTemplateNumber(entity.fieldteplateid, entity.OrderNumber + 1);

            return Json(new { result = true });
        }

        /// <summary>
        /// Возращает список полей формы
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

            var data = _templateRepository.GetDocTemplateById(docTemplID).FieldTeplates.Where(x => !userTemplatesIDs.Contains((int)x.fieldteplateid) && x.FiledType == (int)FieldTemplateType.Number).OrderBy(x => x.OrderNumber).ToList()
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
                    (x.FiledType != (int)FieldTemplateType.Planned && !x.PlanFieldTemplates.Any()) || (factFieldId.HasValue && x.fieldteplateid == factFieldId));

            return new JsonResult { Data = new SelectList(data.ToList().Select(x => new { Id = x.fieldteplateid, Name = x.FieldName }), "Id", "Name", factFieldId) };
        }

        #endregion

        #endregion
    }
}
