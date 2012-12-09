using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
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
                return View(new CustomDocReportModel());
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
                return View(model);
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

        public ActionResult DownloadGroupReport(int reportId)
        {
            try
            {
                var reportService = new ReportService();
                var docReport = _docReportRepository.GetDocReportById(reportId);
                var sessData = SessionHelper.GetUserSessionData(Session);
                var report = reportService.GenerateReport(docReport, null, sessData.UserGroupId);

                var localReport = new LocalReport {ReportPath = Server.MapPath("~/Content/Reports/SmallReport.rdlc")};
                var reportDataSource = new ReportDataSource("MainDataSet", reportService.ConvertReportForRPV(report));
                localReport.SetParameters(new ReportParameter("ReportName",report.Name));
                localReport.SetParameters(new ReportParameter("ReportDescription", report.Legend));
                
                localReport.DataSources.Add(reportDataSource);
                const string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                //The DeviceInfo settings should be changed based on the reportType
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                const string deviceInfo = "<DeviceInfo>" +
                                          "  <OutputFormat>PDF</OutputFormat>" +
                                          //"  <PageWidth>8.5in</PageWidth>" +
                                          //"  <PageHeight>11in</PageHeight>" +
                                          //"  <MarginTop>0.5in</MarginTop>" +
                                          //"  <MarginLeft>1in</MarginLeft>" +
                                          //"  <MarginRight>1in</MarginRight>" +
                                          //"  <MarginBottom>0.5in</MarginBottom>" +
                                          "</DeviceInfo>";

                Warning[] warnings;
                string[] streams;

                //Render the report
                var renderedBytes = localReport.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);
                //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);
                return File(renderedBytes, mimeType);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController.DownloadGroupReport()", ex);
                return null;
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
