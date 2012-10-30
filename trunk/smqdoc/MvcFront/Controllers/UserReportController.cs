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
    public class UserReportController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;
        
        public UserReportController(IDocReportRepository docReportRepository)
        {
            _docReportRepository = docReportRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrintUserReport(int reportId)
        {
            try
            {
            var reportService = new ReportService();
            var docReport = _docReportRepository.GetDocReportById(reportId);
            var sessData = SessionHelper.GetUserSessionData(Session);
            var report = reportService.GenerateReport(docReport, sessData.UserId, sessData.UserGroupId);
            return View(report);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserReportController.PrintUserReport()", ex);
                return View();
            }
        }



        #region GridActions
       
        /// <summary>
        /// Список документов пользователя
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _UserDocReportsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _docReportRepository.GetDocReportsAvailableForUser(sessData.UserId).ToList()
                        .ConvertAll(DocReportListViewModel.DocReportToModelConverter).ToList();
                return View(new GridModel<DocReportListViewModel> { Data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserReportController._UserDocReportsList()", ex);
                return View(new GridModel<DocReport> { Data = new List<DocReport>() });
            }
        }

        #endregion

    }
}
