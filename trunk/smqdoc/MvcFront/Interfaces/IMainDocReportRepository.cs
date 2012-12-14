using System.Collections.Generic;
using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IMainDocReportRepository
    {
        /// <summary>
        /// Поулчить список всех полных репортов
        /// </summary>
        /// <returns></returns>
        IQueryable<MainDocReport> GetAllMainDocReports();

        /// <summary>
        /// Возвращает список сумаарных отчетов которые доступны пользователю (те на которые уже созданы назначения)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<MainDocReport> GetMainDocReportsAvailableForUser(int userId);

        /// <summary>
        /// Возвращает список сумарных отчетов которые доступны менеджеру группы (те на которые уже созданы назначения)
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IEnumerable<MainDocReport> GetMainDocReportsAvailableForGroupManager(int groupId);

        /// <summary>
        /// Получить полный отчет по его ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MainDocReport GetMainDocReportById(int id);

        /// <summary>
        /// Сохранить отчет
        /// </summary>
        /// <param name="entity"></param>
        void SaveMainDocReport(MainDocReport entity);

        /// <summary>
        /// Удалить отчет и системы
        /// </summary>
        /// <param name="id"></param>
        void DeleteMainDocReport(int id);

        /// <summary>
        /// Поулчить список всех полных репортов
        /// </summary>
        /// <returns></returns>
        IQueryable<DocInMainReportsOrder> GetAllDocInMainReportsOrders();

        /// <summary>
        /// Получить полный отчет по его ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DocInMainReportsOrder GetDocInMainReportsOrderById(int id);

        /// <summary>
        /// Поулчить список всех полных репортов
        /// </summary>
        /// <returns></returns>
        IQueryable<DocInMainReportsOrder> GetAllDocInMainReportsOrdersByMainReportId(int id);

        /// <summary>
        /// Сохранить отчет
        /// </summary>
        /// <param name="entity"></param>
        void SaveDocInMainReportsOrder(DocInMainReportsOrder entity);

        /// <summary>
        /// Переместить Отчет внутри суммарного
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ordernumber"></param>
        void SetDimReportNumber(int id, int ordernumber);

        /// <summary>
        /// Удалить отчет и системы
        /// </summary>
        /// <param name="id"></param>
        void DeleteDocInMainReportsOrder(int id);
    }
}