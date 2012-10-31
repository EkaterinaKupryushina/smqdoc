using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Helpers;
using MvcFront.Infrastructure.Security;
using MvcFront.Interfaces;
using MvcFront.Models;
using MvcFront.Services;
using NLog;
using Telerik.Web.Mvc;

namespace MvcFront.Controllers
{
    [GroupManagerAuthorize]
    public class ManagerReportController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public ManagerReportController(IDocReportRepository docReportRepository, IUserGroupRepository userGroupRepository)
        {
            _docReportRepository = docReportRepository;
            _userGroupRepository = userGroupRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CustomReport(int reportId)
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var reportService = new ReportService();
                var docReport = _docReportRepository.GetDocReportById(reportId);
                //получаем список используемых в отчете пользователей(для отметки их по умолчанию)
                var usedUsers = reportService.GetUserIdsListForReport(docReport, sessData.UserGroupId);

                var report = reportService.GenerateReport(docReport, usedUsers, sessData.UserGroupId);

                return View(new CustomDocReportModel(report, _userGroupRepository.GetById(sessData.UserGroupId).Members, usedUsers));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController.CustomReport()", ex);
                return View();
            }
        }

        [HttpPost]
        public ActionResult CustomReport(int reportId, CustomDocReportModel model)
        {
            try
            {
                var reportService = new ReportService();
                var docReport = _docReportRepository.GetDocReportById(reportId);
                var sessData = SessionHelper.GetUserSessionData(Session);
                model.ReportTableView = reportService.GenerateReport(docReport, model.Users.Where(x => x.Value).Select(x => x.IntCode).ToList(), sessData.UserGroupId);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController.CustomReport()", ex);
                return View();
            }
        }

        public ActionResult PrintGroupReport(int reportId)
        {
            try
            {
                var reportService = new ReportService();
                var docReport = _docReportRepository.GetDocReportById(reportId);
                var sessData = SessionHelper.GetUserSessionData(Session);
                var report = reportService.GenerateReport(docReport, null, sessData.UserGroupId);
                return View(report);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController.PrintGroupReport()", ex);
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController._ManagerDocReportsList()", ex);
                return View(new GridModel<DocReport> { Data = new List<DocReport>() });
            }
        }

        #endregion
    }
}
