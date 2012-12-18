using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
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
    [GroupUserAuthorize]
    public class UserReportController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;
        private readonly IMainDocReportRepository _mainDocReportRepository;
        private const string SmallReportMatrix = "SmallSubReport_";
        private SortedList<int, ReportTableViewModel> MainReportData { get; set; }

        public UserReportController(IDocReportRepository docReportRepository, IMainDocReportRepository mainDocReportRepository)
        {
            _docReportRepository = docReportRepository;
            _mainDocReportRepository = mainDocReportRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewUserReport(int reportId)
        {
            try
            {
                var reportService = new ReportService();
                var docReport = _docReportRepository.GetDocReportById(reportId);
                var sessData = SessionHelper.GetUserSessionData(Session);
                var report = reportService.GenerateReport(docReport, new List<int>{sessData.UserId}, sessData.UserGroupId);
                return View(report);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserReportController.PrintUserReport()", ex);
                return View();
            }
        }

        public ActionResult DownloadUserReport(int reportId)
        {
            try
            {
                var reportService = new ReportService();
                var docReport = _docReportRepository.GetDocReportById(reportId);
                var sessData = SessionHelper.GetUserSessionData(Session);
                var report = reportService.GenerateReport(docReport, new List<int> { sessData.UserId }, sessData.UserGroupId);

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

        /// <summary>
        /// Скачать PDF отчета
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ActionResult DownloadMainUserReport(int reportId)
        {
            try
            {
                var reportService = new ReportService();
                var mainDocReport = _mainDocReportRepository.GetMainDocReportById(reportId);
                var sessData = SessionHelper.GetUserSessionData(Session);
                MainReportData = reportService.GenerateMainReport(mainDocReport, new List<int>{sessData.UserId}, sessData.UserGroupId);

                var mainReport = new LocalReport();

                //Формирует суммарный отчет на лету
                var mainXml = new XmlDocument();
                const string ns = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";
                var nsmanager = new XmlNamespaceManager(mainXml.NameTable);
                nsmanager.AddNamespace("ns", ns);
                mainXml.Load(Server.MapPath("~/Content/Reports/MainReport.rdlc"));

                //Поток для записи отредактирвоанного отчета (Размер это апроксимация)
                using (var mainOutputStream = new MemoryStream(mainXml.OuterXml.Length + 300 * MainReportData.Count))
                {
                    var subItemsNode = mainXml.SelectSingleNode("/ns:Report/ns:Body/ns:ReportItems", nsmanager);
                    if (subItemsNode == null)
                    {
                        throw new NullReferenceException("Не удалось найти SmallReport в MainReport");
                    }
                    subItemsNode.RemoveChild(subItemsNode.FirstChild);


                    var repNumber = 0.0;
                    foreach (var model in MainReportData)
                    {
                        var smItemNode = mainXml.CreateElement("Subreport", ns);
                        subItemsNode.AppendChild(smItemNode);

                        var smItemNameAttr = mainXml.CreateAttribute("Name");
                        smItemNameAttr.InnerText = SmallReportMatrix + model.Key;
                        smItemNode.Attributes.Append(smItemNameAttr);

                        var smReportNameNode = mainXml.CreateElement("ReportName", ns);
                        smReportNameNode.InnerText = SmallReportMatrix + model.Key;
                        smItemNode.AppendChild(smReportNameNode);

                        var smTopNode = mainXml.CreateElement("Top", ns);
                        smTopNode.InnerText = string.Format("{0}cm",
                                                            (repNumber * 10.6).ToString(CultureInfo.InvariantCulture));
                        smItemNode.AppendChild(smTopNode);

                        var smHeightNode = mainXml.CreateElement("Height", ns);
                        smHeightNode.InnerText = "10.6cm";
                        smItemNode.AppendChild(smHeightNode);

                        var smWidthNode = mainXml.CreateElement("Width", ns);
                        smWidthNode.InnerText = "26.7cm";
                        smItemNode.AppendChild(smWidthNode);

                        var smStyleNode = mainXml.CreateElement("Style", ns);
                        smItemNode.AppendChild(smStyleNode);

                        var smBorderNode = mainXml.CreateElement("Border", ns);
                        smStyleNode.AppendChild(smBorderNode);

                        var smBorderStyleNode = mainXml.CreateElement("Style", ns);
                        smBorderStyleNode.InnerText = "None";
                        smBorderNode.AppendChild(smBorderStyleNode);


                        repNumber++;

                        var subReport = new StreamReader(Server.MapPath("~/Content/Reports/SmallReport.rdlc"));
                        mainReport.LoadSubreportDefinition(SmallReportMatrix + model.Key, subReport);
                        mainReport.SubreportProcessing += SetSubDataSource;

                    }

                    //Пишем результат обраотки в стрим
                    mainXml.Save(mainOutputStream);
                    mainOutputStream.Position = 0;
                    mainReport.LoadReportDefinition(mainOutputStream);

                }


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
                return File(renderedBytes, mimeType);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "UserReportController.DownloadMainUserReport()", ex);
                return null;
            }

        }


        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            if (MainReportData != null)
            {
                var reportService = new ReportService();
                var rNumber = e.ReportPath.Substring(SmallReportMatrix.Length);
                ReportTableViewModel report;
                MainReportData.TryGetValue(int.Parse(rNumber), out report);
                e.DataSources.Add(new ReportDataSource("MainDataSet", reportService.ConvertReportForRPV(report)));
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
                var data = _docReportRepository.GetDocReportsAvailableForUser(sessData.UserId, sessData.UserGroupId).ToList()
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

        /// <summary>
        /// Список документов пользователя
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _UserMainDocReportsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _mainDocReportRepository.GetMainDocReportsAvailableForUser(sessData.UserId, sessData.UserGroupId).ToList().ConvertAll(MainDocReportListViewModel.MainDocReportToModelConverter).ToList();
                return View(new GridModel<MainDocReportListViewModel> { Data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка");
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController._ManagerMainDocReportsList()", ex);
                return View(new GridModel<MainDocReportListViewModel> { Data = new List<MainDocReportListViewModel>() });
            }
        }
        #endregion

    }
}
