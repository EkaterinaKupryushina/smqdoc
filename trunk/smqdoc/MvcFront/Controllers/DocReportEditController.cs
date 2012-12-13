using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using MvcFront.Models;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    [AdminAuthorize]
    public class DocReportEditController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;
        private readonly IMainDocReportRepository _mainDocReportRepository;
        private readonly IDocTemplateRepository _docTemplateRepository;
        private readonly IUserTagRepository _userTagRepository;

        public DocReportEditController(IDocReportRepository docReportRepository, IDocTemplateRepository docTemplateRepository, IUserTagRepository userTagRepository, IMainDocReportRepository mainDocReportRepository)
        {
            _docReportRepository = docReportRepository;
            _docTemplateRepository = docTemplateRepository;
            _userTagRepository = userTagRepository;
            _mainDocReportRepository = mainDocReportRepository;
        }

        #region DocReport
        /// <summary>
        /// Список всех DocReport в таблице
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// страница выбора DocTemplate для отчета
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectDocTemplate()
        {
            try
            {
                return
                    View(
                        _docTemplateRepository.GetAllDocTeplates().Where(
                            x => x.Status != (int) DocTemplateStatus.Deleted).ToList().ConvertAll(
                                DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.SelectDocTemplate()", ex);
                return View(new List<DocTemplateListViewModel>());
            }
        }

        /// <summary>
        /// Создание Очета
        /// </summary>
        /// <param name="docTemplateId"></param>
        /// <returns></returns>
        public ActionResult CreateDocReport(long docTemplateId)
        {
            try
            {
                return View(new DocReportEditModel {DocTemplateId = docTemplateId});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.CreateDocReport()", ex);
                return View(new DocReportEditModel());
            }
        }

        /// <summary>
        /// Создания отчета
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDocReport(DocReportEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var docReport = model.Update(new DocReport());
                    _docReportRepository.SaveDocReport(docReport);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.CreateDocReport()", ex);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Создание Очета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditDocReport(int id)
        {
            try
            {
                return View(new DocReportEditModel(_docReportRepository.GetDocReportById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.EditDocReport()", ex);
                return View(new DocReportEditModel());
            }
        }

        /// <summary>
        /// Создания отчета
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDocReport(DocReportEditModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var docReport = model.Update(_docReportRepository.GetDocReportById(model.DocReportId));
                    _docReportRepository.SaveDocReport(docReport);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.EditDocReport()", ex);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #region JSon

        /// <summary>
        /// Удаляем отчет
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteDocReport(int id)
        {
           try
            {
                _docReportRepository.DeleteDocReport(id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.DeleteDocReport()", ex);
               return new JsonResult { Data = false };
            }
            
        }

        #endregion

        #region Grid Actions

        /// <summary>
        /// Список всех отчетов системы
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _DocReportList()
        {
            try
            {
                var data =
                    _docReportRepository.GetAllDocReports().ToList()
                        .ConvertAll(DocReportListViewModel.DocReportToModelConverter).ToList();
                return View(new GridModel<DocReportListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController._DocReportList()", ex);
                return View(new GridModel<DocReportListViewModel> { Data = new List<DocReportListViewModel>() });
            }
        }

        #endregion

        #endregion

        #region DocReportFields

        /// <summary>
        /// страниция редактирования списка
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DocReportFieldsManagment(int id)
        {
            try
            {
                return View(_docReportRepository.GetDocReportById(id));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.DocReportFieldsManagment()", ex);
                return View(new DocReport());
            }
        }

        /// <summary>
        /// Добавление поля в форму
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddField(int id)
        {
            try
            {
                var docReport = _docReportRepository.GetDocReportById(id);
                var reportField = new ReportField
                                      {
                                          DocReport = docReport,
                                          reportfieldid = 0
                                      };
                var fModel = new ReportFieldEditModel(reportField);

                return View(fModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.AddField()", ex);
                return View(new ReportFieldEditModel());
            }
        }

        /// <summary>
        /// Добавление поля в форму
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddField(ReportFieldEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fld = model.Update(new ReportField());
                    _docReportRepository.SaveReportField(fld);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("DocReportFieldsManagment", new { id = model.DocReportId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.AddField()", ex);
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
                return View(new ReportFieldEditModel(_docReportRepository.GetReportFieldById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.EditField()", ex);
                return View(new ReportFieldEditModel());
            }
        }

        /// <summary>
        /// Редактирования поля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditField(ReportFieldEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fld = _docReportRepository.GetReportFieldById(model.DocReportId);
                    fld = model.Update(fld);
                    _docReportRepository.SaveReportField(fld);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
                return RedirectToAction("DocReportFieldsManagment", new { id = model.DocReportId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.EditField()", ex);
                return View(model);
            }
        }

        #region Json

        /// <summary>
        /// Ajax  удаление поля
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteField(long id)
        {
            try
            {
                _docReportRepository.DeleteReportField(id);
                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.DeleteField()", ex);
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
                var entity = _docReportRepository.GetReportFieldById(id);
                if (entity.OrderNumber > 1)
                {
                    _docReportRepository.SetFieldTemplateNumber(entity.reportfieldid, entity.OrderNumber - 1);
                }
                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.UpField()", ex);
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
                var entity = _docReportRepository.GetReportFieldById(id);

                _docReportRepository.SetFieldTemplateNumber(entity.reportfieldid, entity.OrderNumber + 1);

                return Json(new {result = true});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.DownField()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Запрос списка для отчета доступных полей 
        /// </summary>
        /// <param name="docReportId"></param>
        /// <param name="fieldTemplateId"> </param>
        /// <param name="allowNonNumber"> </param>
        /// <returns></returns>
        public JsonResult _DocTemplateReportFieldsList(int docReportId, long fieldTemplateId, bool allowNonNumber)
        {
            try
            {
                var data = _docReportRepository.GetDocReportById(docReportId).DocTemplate.FieldTeplates.AsQueryable();
                if (!allowNonNumber)
                {
                    data = data.Where(x =>
                                      (x.FiledType == (int) FieldTemplateType.Number
                                       || x.FiledType == (int) FieldTemplateType.Calculated
                                       ||
                                       (x.FiledType == (int) FieldTemplateType.Planned) && x.FactFieldTemplate != null &&
                                       (x.FactFieldTemplate.FiledType == (int) FieldTemplateType.Number ||
                                        x.FactFieldTemplate.FiledType == (int) FieldTemplateType.Calculated)));
                }

                return new JsonResult
                           {
                               Data =
                                   new SelectList(
                                   data.ToList().Select(x => new {Id = x.fieldteplateid, Name = x.FieldName}), "Id",
                                   "Name", fieldTemplateId)
                           };
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController._DocTemplateReportFieldsList()", ex);
                return new JsonResult { Data = false };
            }
        }


        #endregion

        #region GridActions

        /// <summary>
        /// Возращает список полей отчета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [GridAction]
        public ActionResult _ReportFieldList(int id)
        {
            try
            {
                var data = _docReportRepository.GetDocReportById(id).ReportFields.OrderBy(x => x.OrderNumber).ToList()
                    .ConvertAll(ReportFieldListViewModel.FieldToModelConverter);
                return View(new GridModel<ReportFieldListViewModel> {Data = data});
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController._ReportFieldList()", ex);
                return View(new GridModel<ReportFieldListViewModel> { Data = new List<ReportFieldListViewModel>() });
            }
        }

        #endregion

        #endregion

        #region UserTags

        public JsonResult RemoveUserTag(int id, int tagId)
        {
            try
            {
                return Json(new { result = _userTagRepository.RemoveUserTagFromDocReport(id, tagId) });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.RemoveUserTag()", ex);
                return new JsonResult { Data = false };
            }
        }

        public JsonResult AddUserTag(int docreportId, int tagId)
        {
            try
            {
                return Json(new { result = _userTagRepository.AddUserTagToDocReport(docreportId, tagId) });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.AddUserTag()", ex);
                return new JsonResult { Data = false };
            }
        }

        //список меток пользователя
        [GridAction]
        public ActionResult _UserTagList(int docreportId)
        {
            try
            {
                var data = _docReportRepository.GetDocReportById(docreportId).UserTags.ToList()
                    .ConvertAll(UserTagsListViewModel.UserTagNamesToModelConverter).ToList();

                return View(new GridModel<UserTagsListViewModel> { Data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController._UserTagList()", ex);
                return View(new GridModel<UserTagsListViewModel> { Data = new List<UserTagsListViewModel>() });
            }
        }

        #endregion

        #region MainDocReport

        /// <summary>
        /// Создание Очета
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMainDocReport()
        {
            try
            {
                return View(new MainDocReportEditModel());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.CreateMainDocReport()", ex);
                return View(new MainDocReportEditModel());
            }
        }

        /// <summary>
        /// Создания отчета
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateMainDocReport(MainDocReportEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mainDocReport = model.Update(new MainDocReport());
                    _mainDocReportRepository.SaveMainDocReport(mainDocReport);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.CreateMainDocReport()", ex);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Создание Очета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditMainDocReport(int id)
        {
            try
            {
                return View(new MainDocReportEditModel(_mainDocReportRepository.GetMainDocReportById(id)));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.EditMainDocReport()", ex);
                return View(new MainDocReportEditModel());
            }
        }

        /// <summary>
        /// Создания отчета
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditMainDocReport(MainDocReportEditModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var mainDocReport = model.Update(_mainDocReportRepository.GetMainDocReportById(model.MainDocReportId));
                    _mainDocReportRepository.SaveMainDocReport(mainDocReport);
                }
                else
                {
                    throw new Exception("Проверьте введенные данные");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.EditMainDocReport()", ex);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #region JSon

        /// <summary>
        /// Удаляем отчет
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteMainDocReport(int id)
        {
            try
            {
                _mainDocReportRepository.DeleteMainDocReport(id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.DeleteMainDocReport()", ex);
                return new JsonResult { Data = false };
            }

        }

        #endregion

        #region Grid Actions

        /// <summary>
        /// Список всех отчетов системы
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _MainDocReportList()
        {
            try
            {
                var data =
                    _mainDocReportRepository.GetAllMainDocReports().ToList()
                        .ConvertAll(MainDocReportListViewModel.MainDocReportToModelConverter).ToList();
                return View(new GridModel<MainDocReportListViewModel> { Data = data });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController._MainDocReportList()", ex);
                return View(new GridModel<MainDocReportListViewModel> { Data = new List<MainDocReportListViewModel>() });
            }
        }

        #endregion


        #endregion

        #region DocInMainReportsOrder

        ///<summary>
        /// страница выбора DocReport для суммарного отчета
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDimReport(int id)
        {
            try
            {
                return
                    View(
                        _docReportRepository.GetAllDocReports().Where(x => x.IsActive).ToList().ConvertAll(
                                DocReportListViewModel.DocReportToModelConverter).ToList());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.AddDimReport()", ex);
                return View(new List<DocReportListViewModel>());
            }
        }

        ///<summary>
        /// страница выбора DocReport для суммарного отчета
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDimReportComplete(int id, int newReportId)
        {
            try
            {
                var newOrder = _mainDocReportRepository.GetAllDocInMainReportsOrdersByMainReportId(id).Count() + 1;
                _mainDocReportRepository.SaveDocInMainReportsOrder(
                    new DocInMainReportsOrder
                        {
                            DocReport_docreportid = newReportId, 
                            MainDocReport_maindocreportid = id, 
                            OrderNumber = newOrder
                        });
                return RedirectToAction("EditMainDocReport", new {id});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.AddDimReportComplete()", ex);
                return RedirectToAction("EditMainDocReport", new { id });
            }
        }

        #region Json

        /// <summary>
        /// Ajax  удаление поля
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteDimReport(int id)
        {
            try
            {
                _mainDocReportRepository.DeleteDocInMainReportsOrder(id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.DeleteDimReport()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Поднять в списке поле 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpDimReport(int id)
        {
            try
            {
                var entity = _mainDocReportRepository.GetDocInMainReportsOrderById(id);
                if (entity.OrderNumber > 1)
                {
                    _mainDocReportRepository.SetDimReportNumber(entity.docinmainreportsorderid, entity.OrderNumber - 1);
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.UpDimReport()", ex);
                return new JsonResult { Data = false };
            }
        }

        /// <summary>
        /// Опустить поле в списке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DownDimReport(int id)
        {
            try
            {
                var entity = _mainDocReportRepository.GetDocInMainReportsOrderById(id);

                _mainDocReportRepository.SetDimReportNumber(entity.docinmainreportsorderid, entity.OrderNumber + 1);

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController.DownDimReport()", ex);
                return new JsonResult { Data = false };
            }
        }
        #endregion

        #region GridActions

        [GridAction]
        public ActionResult _DimReportList(int id)
        {
            try
            {
                var data = _mainDocReportRepository.GetAllDocInMainReportsOrdersByMainReportId(id).ToList()
                    .ConvertAll(DocInMainReportsOrderListViewModel.DocReportToModelConverter).ToList();

                return View(new GridModel<DocInMainReportsOrderListViewModel> { Data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "DocReportEditController._DimReportList()", ex);
                return View(new GridModel<DocInMainReportsOrderListViewModel> { Data = new List<DocInMainReportsOrderListViewModel>() });
            }
        }

        #endregion

        #endregion
    }
}
