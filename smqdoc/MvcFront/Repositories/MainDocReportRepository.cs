using System.Linq;
using MvcFront.Interfaces;
using MvcFront.DB;

namespace MvcFront.Repositories
{
    public class MainDocReportRepository : IMainDocReportRepository
    {
        readonly IUnitOfWork _unitOfWork;
        public MainDocReportRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IQueryable<MainDocReport> GetAllMainDocReports()
        {
            return _unitOfWork.DbModel.MainDocReports;
        }

        public MainDocReport GetMainDocReportById(int id)
        {
            return _unitOfWork.DbModel.MainDocReports.First(x => x.maindocreportid == id);
        }


        public void SaveMainDocReport(MainDocReport entity)
        {
            if (entity.maindocreportid == 0)
            {
                _unitOfWork.DbModel.MainDocReports.AddObject(entity);
            }
            else
            {
                _unitOfWork.DbModel.MainDocReports.ApplyCurrentValues(entity);
            }
            _unitOfWork.DbModel.SaveChanges();
        }

        public void DeleteMainDocReport(int id)
        {
            var entity = GetMainDocReportById(id);
            if (entity != null)
            {
                if (entity.DocInMainReportsOrders != null)
                {
                    for (var i = 0; i < entity.DocInMainReportsOrders.Count; i++)
                    {
                        _unitOfWork.DbModel.DocInMainReportsOrders.DeleteObject(entity.DocInMainReportsOrders.ElementAt(i));
                    }
                }
            }
            _unitOfWork.DbModel.MainDocReports.DeleteObject(entity);
            _unitOfWork.DbModel.SaveChanges();
        }

        public IQueryable<DocInMainReportsOrder> GetAllDocInMainReportsOrders()
        {
            return _unitOfWork.DbModel.DocInMainReportsOrders;
        }

        public DocInMainReportsOrder GetDocInMainReportsOrderById(int id)
        {
            return _unitOfWork.DbModel.DocInMainReportsOrders.First(x => x.docinmainreportsorderid == id);
        }

        public IQueryable<DocInMainReportsOrder> GetAllDocInMainReportsOrdersByMainReportId(int id)
        {
            return _unitOfWork.DbModel.DocInMainReportsOrders.Where(x => x.MainDocReport_maindocreportid == id).OrderBy(x => x.OrderNumber);
        }

        public void SaveDocInMainReportsOrder(DocInMainReportsOrder entity)
        {
            if (entity.docinmainreportsorderid == 0)
            {
                _unitOfWork.DbModel.DocInMainReportsOrders.AddObject(entity);
            }
            else
            {
                _unitOfWork.DbModel.DocInMainReportsOrders.ApplyCurrentValues(entity);
            }
            _unitOfWork.DbModel.SaveChanges();
            ReoderFields(entity.MainDocReport_maindocreportid);
        }

        public void DeleteDocInMainReportsOrder(int id)
        {
            var entity = GetDocInMainReportsOrderById(id);
            _unitOfWork.DbModel.DocInMainReportsOrders.DeleteObject(entity);
            _unitOfWork.DbModel.SaveChanges();
        }

        public void SetDimReportNumber(int id, int newNumber)
        {
            var enFirst = GetDocInMainReportsOrderById(id);
            if (enFirst != null)
            {
                int oldNumber = enFirst.OrderNumber;

                var enSecond = GetAllDocInMainReportsOrders().FirstOrDefault(x => x.OrderNumber == newNumber);
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
                ReoderFields(enFirst.MainDocReport_maindocreportid);
            }
        }

        private void ReoderFields(int maindocrepotId)
        {
            var docReports = GetAllDocInMainReportsOrdersByMainReportId(maindocrepotId);
            if (docReports != null)
            {
                var items = docReports.OrderBy(x => x.OrderNumber).ToArray();
                for (int i = 0; i < items.Count(); i++)
                {
                    items.ElementAt(i).OrderNumber = i + 1;
                }
                _unitOfWork.DbModel.SaveChanges();
            }
        }
    }
}