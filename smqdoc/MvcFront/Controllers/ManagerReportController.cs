using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Models;
using MvcFront.Services;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    public class ManagerReportController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;

        public ManagerReportController(IDocReportRepository docReportRepository)
        {
            _docReportRepository = docReportRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CustomReport(int reportId)
        {
            return View(new DocReportEditModel(_docReportRepository.GetDocReportById(reportId)));
        }

        [HttpPost]
        public ActionResult CustomReport(int reportId,DocReportEditModel model)
        {
            return RedirectToAction("Index");
        }

        public ActionResult PrintUserReport(int reportId, int userId)
        {
            try
            {
            var reportService = new ReportService();
            var docReport = _docReportRepository.GetDocReportById(reportId);
            var sessData = SessionHelper.GetUserSessionData(Session);
            var report = reportService.GenerateReport(docReport, userId, sessData.UserGroupId);
            return View(report);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserReportController.PrintUserReport()", ex);
                return View();
            }
        }

        public ActionResult PrintGroupReport(int reportId)
        {
            try{

            var reportService = new ReportService();
            var docReport = _docReportRepository.GetDocReportById(reportId);
            var sessData = SessionHelper.GetUserSessionData(Session);
            var report = reportService.GenerateReport(docReport, null, sessData.UserGroupId);
            return View(report);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserReportController.PrintGroupReport()", ex);
                return View();
            }
        }


        #region GridActions

        /// <summary>
        /// Список документов пользователя
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _ManagerDocReportsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _docReportRepository.GetDocReportsAvailableForGroupManager(sessData.UserGroupId).ToList()
                        .ConvertAll(DocReportListViewModel.DocReportToModelConverter).ToList();
                return View(new GridModel<DocReportListViewModel> { Data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserReportController._ManagerDocReportsList()", ex);
                return View(new GridModel<DocReport> { Data = new List<DocReport>() });
            }
        }

        #endregion
    }
}
