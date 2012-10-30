using System.Linq;
using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Repositories
{
    public class DocReportRepository : IDocReportRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public DocReportRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IQueryable<DocReport> GetAllDocReports()
        {
            return _unitOfWork.DbModel.DocReports;
        }

        public DocReport GetDocReportById(int id)
        {
            return _unitOfWork.DbModel.DocReports.First(x => x.docreportid == id);
        }


        public IQueryable<DocReport> GetDocReportsAvailableForUser(int userId)
        {
            return
                _unitOfWork.DbModel.DocReports.Where(
                    x => x.DocTemplate.DocAppointments.Any(y => y.Documents.Any(z => z.UserAccount_userid == userId)));
        }


        public IQueryable<DocReport> GetDocReportsAvailableForGroupManager(int groupId)
        {
            return
                _unitOfWork.DbModel.DocReports.Where(
                    x => x.DocTemplate.DocAppointments.Any(y => y.UserGroup_usergroupid == groupId));
        }

        public void SaveDocReport(DocReport entity)
        {
            if (entity.docreportid == 0)
            {
                _unitOfWork.DbModel.DocReports.AddObject(entity);
            }
            else
            {
                _unitOfWork.DbModel.DocReports.ApplyCurrentValues(entity);
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public void DeleteDocReport(int id)
        {
            var entity = GetDocReportById(id);
            if (entity != null)
            {
                if (entity.ReportFields != null)
                {
                    for (var i = 0; i < entity.ReportFields.Count; i++)
                    {
                        _unitOfWork.DbModel.ReportFields.DeleteObject(entity.ReportFields.ElementAt(i));
                    }
                }
            }
            _unitOfWork.DbModel.DocReports.DeleteObject(entity);
            _unitOfWork.DbModel.SaveChanges();
        }

        public IQueryable<ReportField> GetAllReportFields()
        {
            return _unitOfWork.DbModel.ReportFields;
        }

        public ReportField GetReportFieldById(long id)
        {
            return _unitOfWork.DbModel.ReportFields.First(x => x.reportfieldid == id);
        }

        public void SaveReportField(ReportField entity)
        {
            if (entity.reportfieldid == 0)
            {
                var docReport = GetDocReportById(entity.DocReport_reportid);
                if (docReport != null)
                {
                    entity.OrderNumber = docReport.ReportFields.Count + 1;
                    _unitOfWork.DbModel.ReportFields.AddObject(entity);
                }
            }
            else
            {
                _unitOfWork.DbModel.ReportFields.ApplyCurrentValues(entity);
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public void DeleteReportField(long id)
        {
            var entity = _unitOfWork.DbModel.ReportFields.First(x => x.reportfieldid == id);
            _unitOfWork.DbModel.ReportFields.DeleteObject(entity);
            _unitOfWork.DbModel.SaveChanges();
            if (entity != null) ReoderFields(entity.DocReport_reportid);
        }

        public void SetFieldTemplateNumber(long id, int newNumber)
        {
            var enFirst = GetReportFieldById(id);
            if (enFirst != null)
            {
                int oldNumber = enFirst.OrderNumber;

                var enSecond = enFirst.DocReport.ReportFields.FirstOrDefault(x => x.OrderNumber == newNumber);
                if (enSecond == null)
                {
                    enFirst.OrderNumber = newNumber;
                }
                else
                {
                    enSecond.OrderNumber = oldNumber;
                    enFirst.OrderNumber = newNumber;
                }
                _unitOfWork.DbModel.SaveChanges();
                ReoderFields(enFirst.DocReport_reportid);
            }
        }

        private void ReoderFields(int docrepotId)
        {
            var docReport = GetDocReportById(docrepotId);
            if (docReport != null)
            {
                var items = docReport.ReportFields.OrderBy(x => x.OrderNumber);
                for (int i = 0; i < items.Count(); i++)
                {
                    items.ElementAt(i).OrderNumber = i + 1;
                }
                _unitOfWork.DbModel.SaveChanges();
            }
        }


        public DocReport Copy(IUnitOfWork uw, int docreportId)
        {
            if (docreportId == 0)
                return new DocReport();
            return uw.DbModel.DocReports.SingleOrDefault(x => x.docreportid == docreportId);
        }
    }
}