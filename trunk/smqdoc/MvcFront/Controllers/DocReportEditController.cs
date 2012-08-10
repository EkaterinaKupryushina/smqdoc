using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}
