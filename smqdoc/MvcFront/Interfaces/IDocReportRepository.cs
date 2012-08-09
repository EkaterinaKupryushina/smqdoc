﻿using System.Linq;
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
        ReportField GetReportFieldById(int id);

        /// <summary>
        /// Сохранить изменения в поле отчета
        /// </summary>
        /// <param name="entity"></param>
        void SaveReportField(ReportField entity);

        /// <summary>
        /// Удалить поле отчета
        /// </summary>
        /// <param name="id"></param>
        void DeleteReportField(int id);
    }
}