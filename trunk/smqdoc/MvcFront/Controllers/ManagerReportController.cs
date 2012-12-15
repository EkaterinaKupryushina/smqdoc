using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using Microsoft.Reporting.WebForms;
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
        private readonly IMainDocReportRepository _mainDocReportRepository;
        private const string SmallReportMatrix = "SmallSubReport_";
        private SortedList<int, ReportTableViewModel> MainReportData { get; set; }

        public ManagerReportController(IDocReportRepository docReportRepository, IUserGroupRepository userGroupRepository, IMainDocReportRepository mainDocReportRepository)
        {
            _docReportRepository = docReportRepository;
            _userGroupRepository = userGroupRepository;
            _mainDocReportRepository = mainDocReportRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Страница настраиваемого отчета
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Отдельная страница для просмотра отчета
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Скачать PDF отчета
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Скачать PDF отчета
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ActionResult DownloadMainGroupReport(int reportId)
        {
            try
            {
                var reportService = new ReportService();
                var mainDocReport = _mainDocReportRepository.GetMainDocReportById(reportId);
                var sessData = SessionHelper.GetUserSessionData(Session);
                MainReportData = reportService.GenerateMainReport(mainDocReport, null, sessData.UserGroupId);

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
                        smReportNameNode.InnerText = "SmallReport.rdlc";
                        smItemNode.AppendChild(smReportNameNode);

                        var smTopNode = mainXml.CreateElement("Top", ns);
                        smTopNode.InnerText = string.Format("{0}cm",
                                                            (repNumber*10.6).ToString(CultureInfo.InvariantCulture));
                        smItemNode.AppendChild(smTopNode);

                        var smHeightNode = mainXml.CreateElement("Height", ns);
                        smHeightNode.InnerText = "10.6cm";
                        smItemNode.AppendChild(smHeightNode);

                        var smWidthNode = mainXml.CreateElement("Width", ns);
                        smWidthNode.InnerText = "16.51cm";
                        smItemNode.AppendChild(smWidthNode);

                        var smStyleNode = mainXml.CreateElement("Style", ns);
                        smItemNode.AppendChild(smStyleNode);

                        var smBorderNode = mainXml.CreateElement("Border", ns);
                        smStyleNode.AppendChild(smBorderNode);

                        var smBorderStyleNode = mainXml.CreateElement("Style", ns);
                        smBorderStyleNode.InnerText = "None";
                        smBorderNode.AppendChild(smBorderStyleNode);


                        repNumber++;
                    }

                    //Пишем результат обраотки в стрим
                    mainXml.Save(mainOutputStream);


                    mainOutputStream.Position = 0;

                    mainReport.LoadReportDefinition(mainOutputStream);
                }
                var subReport = new StreamReader(Server.MapPath("~/Content/Reports/SmallReport.rdlc"));

                foreach (var reportTvm in MainReportData)
                {
                    mainReport.LoadSubreportDefinition(SmallReportMatrix + reportTvm.Key, subReport);
                    mainReport.SubreportProcessing += SetSubDataSource;
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
                LogManager.GetCurrentClassLogger().LogException(LogLevel.Fatal, "ManagerReportController.DownloadGroupReport()", ex);
                return null;
            }

        }


        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
          if(MainReportData != null)
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
                return View(new GridModel<DocReportListViewModel> { Data = new List<DocReportListViewModel>() });
            }
        }

        /// <summary>
        /// Список документов пользователя
        /// </summary>
        /// <returns></returns>
        [GridAction]
        public ActionResult _ManagerMainDocReportsList()
        {
            try
            {
                var sessData = SessionHelper.GetUserSessionData(Session);
                var data = _mainDocReportRepository.GetMainDocReportsAvailableForGroupManager(sessData.UserGroupId).ToList().ConvertAll(MainDocReportListViewModel.MainDocReportToModelConverter).ToList();
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
