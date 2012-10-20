using System.Web.Mvc;
using MvcFront.Helpers;
using MvcFront.Interfaces;
using MvcFront.Services;

namespace MvcFront.Controllers
{
    public class UserReportController : Controller
    {
        private readonly IDocReportRepository _docReportRepository;
        
        public UserReportController(IDocReportRepository docReportRepository)
        {
            _docReportRepository = docReportRepository;
        }

        public ActionResult PrintUserReport(int reportId)
        {
            var reportService = new ReportService();
            var docReport = _docReportRepository.GetDocReportById(reportId);
            var sessData = SessionHelper.GetUserSessionData(Session);
            var report = reportService.GenerateReport(docReport, sessData.UserId, sessData.UserGroupId);
            return View(report);
        }

    }
}
