using System.Web.Mvc;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Services;

namespace MvcFront.Controllers
{
    public class ManagerReportController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;

        public ManagerReportController(IDocReportRepository docReportRepository)
        {
            _docReportRepository = docReportRepository;
        }

        public ActionResult PrintUserReport(int reportId, int userId)
        {
            var reportService = new ReportService();
            var docReport = _docReportRepository.GetDocReportById(reportId);
            var sessData = SessionHelper.GetUserSessionData(Session);
            var report = reportService.GenerateReport(docReport, userId, sessData.UserGroupId);
            return View(report);
        }

        public ActionResult PrintGroupReport(int reportId)
        {
            var reportService = new ReportService();
            var docReport = _docReportRepository.GetDocReportById(reportId);
            var sessData = SessionHelper.GetUserSessionData(Session);
            var report = reportService.GenerateReport(docReport, null, sessData.UserGroupId);
            return View(report);
        }
    }
}
