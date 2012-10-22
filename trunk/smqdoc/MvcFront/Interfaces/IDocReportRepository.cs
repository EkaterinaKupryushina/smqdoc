using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IDocReportRepository
    {
        /// <summary>
        /// Поулчить список всех репортов
        /// </summary>
        /// <returns></returns>
        IQueryable<DocReport> GetAllDocReports();

        /// <summary>
        /// Получить отчет по его ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DocReport GetDocReportById(int id);


        /// <summary>
        /// Создает копию Instance объект отчет
        /// </summary>
        /// <param name="uw"></param>
        /// <param name="docreportId"></param>
        /// <returns></returns>
        DocReport Copy(IUnitOfWork uw, int docreportId);

        /// <summary>
        /// Сохранить отчет
        /// </summary>
        /// <param name="entity"></param>
        void SaveDocReport(DocReport entity);

        /// <summary>
        /// Удалить отчет и системы
        /// </summary>
        /// <param name="id"></param>
        void DeleteDocReport(int id);

        /// <summary>
        /// Плучить список всех полей отчетов
        /// </summary>
        /// <returns></returns>
        IQueryable<ReportField> GetAllReportFields();

        /// <summary>
        /// Получить поле отчета по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ReportField GetReportFieldById(long id);

        /// <summary>
        /// Сохранить изменения в поле отчета
        /// </summary>
        /// <param name="entity"></param>
        void SaveReportField(ReportField entity);

        /// <summary>
        /// Установить полю номер
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newNumber"></param>
        void SetFieldTemplateNumber(long id, int newNumber);

        /// <summary>
        /// Удалить поле отчета
        /// </summary>
        /// <param name="id"></param>
        void DeleteReportField(long id);
    }
}