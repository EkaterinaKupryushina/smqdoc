using System;
using System.Collections.Generic;
using System.IO;
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

        private int rptId = 1;

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

                var mainReport = new LocalReport { ReportPath = Server.MapPath("~/Content/Reports/SmallReport.rdlc") };

                var reportDataSource = new ReportDataSource("MainDataSet", reportService.ConvertReportForRPV(report));
                mainReport.DataSources.Clear();
                mainReport.DataSources.Add(reportDataSource);

                const string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;

                Warning[] warnings;
                string[] streams;

                //Render the report
                var renderedBytes = mainReport.Render(
                    reportType,
                    null,
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

        //public ActionResult DownloadGroupReport(int reportId)
        //{
        //    try
        //    {
        //        var reportService = new ReportService();
        //        var docReport = _docReportRepository.GetDocReportById(reportId);
        //        var sessData = SessionHelper.GetUserSessionData(Session);
        //        var report = reportService.GenerateReport(docReport, null, sessData.UserGroupId);

        //        var mainReport = new LocalReport { ReportPath = Server.MapPath("~/Content/Reports/MainReport.rdlc") };
        //        var subReport = new StreamReader(Server.MapPath("~/Content/Reports/SmallReport.rdlc"));

        //        mainReport.LoadSubreportDefinition("SmallSubreport1", subReport);
        //        mainReport.LoadSubreportDefinition("SmallSubreport2", subReport);
        //        mainReport.SubreportProcessing += SetSubDataSource;


        //        const string reportType = "PDF";
        //        string mimeType;
        //        string encoding;
        //        string fileNameExtension;

        //        //The DeviceInfo settings should be changed based on the reportType
        //        //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
        //        //const string deviceInfo = "<DeviceInfo>" +
        //        //                          "  <OutputFormat>PDF</OutputFormat>" +
        //        //                          //"  <PageWidth>2870mm</PageWidth>" +
        //        //                          //"  <PageHeight>2100mm</PageHeight>" +
        //        //                          //"  <MarginTop>10mm</MarginTop>" +
        //        //                          //"  <MarginLeft>20mm</MarginLeft>" +
        //        //                          //"  <MarginRight>10mm</MarginRight>" +
        //        //                          //"  <MarginBottom>10mm</MarginBottom>" +
        //        //                          "</DeviceInfo>";

        //        Warning[] warnings;
        //        string[] streams;

        //        //Render the report
        //        var renderedBytes = mainReport.Render(
        //            reportType,
        //            null,
        //            out mimeType,
        //            out encoding,
        //            out fileNameExtension,
        //            out streams,
        //            out warnings);
        //        //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);
        //        return File(renderedBytes, mimeType);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Произошла ошибка");
        //        LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController.DownloadGroupReport()", ex);
        //        return null;
        //    }

        //}
        //public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        //{
        //    //TODO test only

        //    var reportService = new ReportService();
            
        //    var sessData = SessionHelper.GetUserSessionData(Session);
        //    var docReport = _docReportRepository.GetDocReportById(rptId);
        //    var report = reportService.GenerateReport(docReport, null, sessData.UserGroupId);
        //    e.DataSources.Add(new ReportDataSource("MainDataSet", reportService.ConvertReportForRPV(report)));
        //    rptId = 2;
        //    //e.Parameters.SetParameters(new ReportParameter("ReportName", report.Name));
        //    //localReport.SetParameters(new ReportParameter("ReportDescription", report.Legend));

        //}

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
