using System;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Interfaces;
using MvcFront.Models;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    public class DocReportEditController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;
        private readonly IDocTemplateRepository _docTemplateRepository;

        public DocReportEditController(IDocReportRepository docReportRepository,IDocTemplateRepository docTemplateRepository)
        {
            _docReportRepository = docReportRepository;
            _docTemplateRepository = docTemplateRepository;
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
            return View(_docTemplateRepository.GetAllDocTeplates().Where(x => x.Status != (int)DocTemplateStatus.Deleted).ToList().ConvertAll(DocTemplateListViewModel.DocTemplateToModelConverter).ToList());
        }

        /// <summary>
        /// Создание Очета
        /// </summary>
        /// <param name="docTemplateId"></param>
        /// <returns></returns>
        public ActionResult CreateDocReport(long docTemplateId)
        {
            return View(new DocReportEditModel {DocTemplateId = docTemplateId});
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
                ModelState.AddModelError("Ошибка Сохранения",ex);
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

            return View(new DocReportEditModel(_docReportRepository.GetDocReportById(id)));
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
                ModelState.AddModelError("Ошибка сохранения", ex);
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
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Ошибка", ex.Message);
            }
            return Json(new { result = true });
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
            var data =
              _docReportRepository.GetAllDocReports().ToList()
                  .ConvertAll(DocReportListViewModel.DocReportToModelConverter).ToList();
            return View(new GridModel<DocReportListViewModel> { Data = data });
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
            return View(_docReportRepository.GetDocReportById(id));
        }

        /// <summary>
        /// Добавление поля в форму
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddField(int id)
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
            return View(new ReportFieldEditModel(_docReportRepository.GetReportFieldById(id)));
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
                ModelState.AddModelError("Ошибка при сохранении", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            return View();
        }

        #region Json

        /// <summary>
        /// Ajax  удаление поля
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteField(long id)
        {
            _docReportRepository.DeleteReportField(id);
            return Json(new { result = true });
        }

        /// <summary>
        /// Поднять в списке поле 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpField(long id)
        {
            var entity = _docReportRepository.GetReportFieldById(id);
            if (entity.OrderNumber > 1)
            {
                _docReportRepository.SetFieldTemplateNumber(entity.reportfieldid, entity.OrderNumber - 1);
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
            var entity = _docReportRepository.GetReportFieldById(id);

            _docReportRepository.SetFieldTemplateNumber(entity.reportfieldid, entity.OrderNumber + 1);

            return Json(new { result = true });
        }

        /// <summary>
        /// Запрос списка доступных полей для которых можно сделать планируемое поле
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="fieldId"> </param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DocTemplateReportFieldsList(int docId, long fieldId)
        {
            var data =
                _docReportRepository.GetDocReportById(docId).DocTemplate.FieldTeplates.Where(x =>
                    (x.FiledType == (int)FieldTemplateType.Number
                    || x.FiledType == (int)FieldTemplateType.Calculated
                    || (x.FiledType == (int)FieldTemplateType.Planned) && x.FactFieldTemplate != null &&
                        (x.FactFieldTemplate.FiledType == (int)FieldTemplateType.Number || x.FactFieldTemplate.FiledType == (int)FieldTemplateType.Calculated)));

            return new JsonResult { Data = new SelectList(data.ToList().Select(x => new { Id = x.fieldteplateid, Name = x.FieldName }), "Id", "Name", fieldId) };
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
            var data = _docReportRepository.GetDocReportById(id).ReportFields.OrderBy(x => x.OrderNumber).ToList()
                .ConvertAll(ReportFieldListViewModel.FieldToModelConverter);
            return View(new GridModel<ReportFieldListViewModel> { Data = data });
        }

        #endregion

        #endregion
    }
}
