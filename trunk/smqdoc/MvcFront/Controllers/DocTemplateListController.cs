using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.Enums;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using MvcFront.DB;
using MvcFront.Models;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    [AdminAuthorize]
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
            return View();
        }

        /// <summary>
        /// Просмотр свойств формы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(long id)
        {
            try
            {
                return View(_templateRepository.GetDocTemplateById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.Details()", ex);
                return View(new DocTemplate());
            }
        }

        /// <summary>
        /// Создание формы
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                return View(new DocTemplateEditModel(_templateRepository.GetDocTemplateById(0)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.Create()", ex);
                return View(new DocTemplateEditModel());
            }
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
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.Create()", ex);
                return View(model);
            }
        }
        
        /// <summary>
        /// Редактирование Формы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(long id)
        {
            try
            {
                return View(new DocTemplateEditModel(_templateRepository.GetDocTemplateById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.Edit()", ex);
                return View(new DocTemplateEditModel());
            }
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
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.Edit()", ex);
                return View(model);
            }
            
        }

        /// <summary>
        /// Удаление формы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(long id)
        {
            try
            {
                return View(_templateRepository.GetDocTemplateById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.Delete()", ex);
                return View(new DocTemplate());
            }
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
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.Delete()", ex);
                return View(new DocTemplate());
            }
        }

        #region JSon
        
        /// <summary>
        /// Меняет стутс формы 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult ChangeState(long id)
        {
            try
            {
                _templateRepository.ChangeDocTemplateState(id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.ChangeState()", ex);
                return new JsonResult { Data = false };
            }
        }

        #endregion

        #region GridActions

        /// <summary>
        /// Возвращает список  форм
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _DocTemplatesList()
        {
            try
            {
                var data =
                    _templateRepository.GetAllDocTeplates().Where(x => x.Status != (int) DocTemplateStatus.Deleted).
                        ToList()
                        .ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList();
                return View(new GridModel<DocTemplateListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController._DocTemplatesList()", ex);
                return View(new GridModel<DocTemplateListViewModel> { Data = new List<DocTemplateListViewModel>() });
            }
        }

        #endregion

        #endregion

        #region FieldTemplates

        /// <summary>
        /// страниция редактирования списка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DocTemplateFieldsManagment(long id)
        {
            try
            {
                return View(_templateRepository.GetDocTemplateById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.DocTemplateFieldsManagment()", ex);
                return View(new DocTemplate());
            }
        }

        /// <summary>
        /// Добавление поля в форму
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddField(long id)
        {
            try
            {
                var field = _templateRepository.GetFieldTemplateById(0);

                field.DocTemplate_docteplateid = id;
                field.DocTemplate = _templateRepository.GetDocTemplateById(id);
                var fModel = new FieldTemplateEditModel(field);
                return View(fModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.AddField()", ex);
                return View(new FieldTemplateEditModel());
            }
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
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.AddField()", ex);
                return View(model);
            }
        }

        /// <summary>
        /// Редактирование поля
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditField(long id)
        {
            try
            {
                return View(new FieldTemplateEditModel(_templateRepository.GetFieldTemplateById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.EditField()", ex);
                return View(new FieldTemplateEditModel());
            }
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
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.EditField()", ex);
                return View(model);
            }
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
            try
            {
                _templateRepository.DeleteFieldTemplate(fieldTemplateId);
                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.DeleteField()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Ajax добавление поля в список вычислимых
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldTemplateId"></param>
        /// <returns></returns>
        public JsonResult AddFieldToCalc(long id, long fieldTemplateId)
        {
            try
            {
                var entity = _templateRepository.GetFieldTemplateById(id);
                entity.ComputableFieldTemplateParts.Add(new ComputableFieldTemplateParts
                                                            {
                                                                FieldTemplate = entity,
                                                                fkCalculatedFieldTemplateID = fieldTemplateId
                                                            });
                _templateRepository.SaveFieldTemplate(entity);

                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.AddFieldToCalc()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Ajax удаление поля из списка вычислимых
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldTemplateId"></param>
        /// <returns></returns>
        public JsonResult DeleteFieldFromCalc(long id, long fieldTemplateId)
        {
            try
            {
                var entity = _templateRepository.GetFieldTemplateById(id);
                _templateRepository.SaveFieldTemplate(entity);
                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.DeleteFieldFromCalc()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Поднять в списке поле 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpField(long id)
        {
            try
            {
                var entity = _templateRepository.GetFieldTemplateById(id);
                if (entity.OrderNumber > 1)
                {
                    _templateRepository.SetFieldTemplateNumber(entity.fieldteplateid, entity.OrderNumber - 1);
                }
                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.UpField()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Опустить поле в списке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DownField(long id)
        {
            try
            {
                var entity = _templateRepository.GetFieldTemplateById(id);
                _templateRepository.SetFieldTemplateNumber(entity.fieldteplateid, entity.OrderNumber + 1);
                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController.DownField()", ex);
                return new JsonResult { Data = false };
            }
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
            try
            {
                var data =
                    _templateRepository.GetDocTemplateById(docTemplID).FieldTeplates.Where(x =>
                                                                                           (x.FiledType !=
                                                                                            (int)
                                                                                            FieldTemplateType.Planned &&
                                                                                            !x.PlanFieldTemplates.Any()) ||
                                                                                           (factFieldId.HasValue &&
                                                                                            x.fieldteplateid ==
                                                                                            factFieldId));

                return new JsonResult
                           {
                               Data =
                                   new SelectList(
                                   data.ToList().Select(x => new {Id = x.fieldteplateid, Name = x.FieldName}), "Id",
                                   "Name", factFieldId)
                           };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocTemplateListController._FactFieldTemplatesList()", ex);
                return new JsonResult { Data = false };
            }
        }

        #endregion

        #region GridActions

        /// <summary>
        /// Возращает список полей формы
        /// </summary>
        /// <param name="templId"></param>
        /// <returns></returns>
        [GridAction]
        public ActionResult _FieldTemplateList(long templId)
        {
            try
            {
                var data = _templateRepository.GetDocTemplateById(templId).FieldTeplates.OrderBy(x => x.OrderNumber).
                    ToList()
                    .ConvertAll(FieldTemplateListViewModel.FieldToModelConverter);
                return View(new GridModel<FieldTemplateListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController._FieldTemplateList()", ex);
                return View(new GridModel<FieldTemplateListViewModel> { Data = new List<FieldTemplateListViewModel>() });
            }
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
            try
            {
                var tpl = _templateRepository.GetFieldTemplateById(fieldTemplID);
                var lst =
                    _templateRepository.GetAllComputableFieldTempalteParts().Where(
                        x => x.FieldTemplate_fieldteplateid == tpl.fieldteplateid).ToList();
                var userTemplatesIDs = lst.Select(item => item.fkCalculatedFieldTemplateID).ToList();

                var data = _templateRepository.GetDocTemplateById(docTemplID).FieldTeplates.Where(
                    x => userTemplatesIDs.Contains((int) x.fieldteplateid)).OrderBy(x => x.OrderNumber).ToList()
                    .ConvertAll(FieldTemplateListViewModel.FieldToModelConverter);
                return View(new GridModel<FieldTemplateListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController._FieldTemplateListUsedForCalc()", ex);
                return View(new GridModel<FieldTemplateListViewModel> { Data = new List<FieldTemplateListViewModel>() });
            }
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
            try
            {
                var tpl = _templateRepository.GetFieldTemplateById(fieldTemplID);
                var lst =
                    _templateRepository.GetAllComputableFieldTempalteParts().Where(
                        x => x.FieldTemplate_fieldteplateid == tpl.fieldteplateid).ToList();
                var userTemplatesIDs = lst.Select(item => item.fkCalculatedFieldTemplateID).ToList();

                var data = _templateRepository.GetDocTemplateById(docTemplID).FieldTeplates.Where(
                    x =>
                    !userTemplatesIDs.Contains((int) x.fieldteplateid) && x.FiledType == (int) FieldTemplateType.Number)
                    .OrderBy(x => x.OrderNumber).ToList()
                    .ConvertAll(FieldTemplateListViewModel.FieldToModelConverter);

                return View(new GridModel<FieldTemplateListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DictionaryController._FieldTemplateListNotUsedForCalc()", ex);
                return View(new GridModel<FieldTemplateListViewModel> { Data = new List<FieldTemplateListViewModel>() });
            }
        }

        #endregion

        #endregion
    }
}
