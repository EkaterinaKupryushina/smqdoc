using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcFront.DB;
using MvcFront.Entities;
using MvcFront.Enums;
using MvcFront.Helpers;
using MvcFront.Infrastructure;
using MvcFront.Interfaces;
using MvcFront.Models;
using ReportExportGenerator;

namespace MvcFront.Services
{
    public class ReportService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IDocAppointmentRepository _docAppointmentRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public ReportService(IDocumentRepository documentRepository = null, IUserAccountRepository userAccountRepository = null, 
            IDocAppointmentRepository docAppointmentRepository = null, IUserGroupRepository userGroupRepository = null)
        {
            _documentRepository = documentRepository ??  DependencyResolver.Current.GetService<IDocumentRepository>();
            _userAccountRepository = userAccountRepository ??  DependencyResolver.Current.GetService<IUserAccountRepository>();
            _docAppointmentRepository = docAppointmentRepository ??  DependencyResolver.Current.GetService<IDocAppointmentRepository>();
            _userGroupRepository = userGroupRepository ?? DependencyResolver.Current.GetService<IUserGroupRepository>();
        }

        #region public
        /// <summary>
        /// Генерирует отчет по пользователю
        /// </summary>
        /// <param name="report"></param>
        /// <param name="userIds"></param>
        /// <param name="groupId"> </param>
        /// <returns></returns>
        public ReportTableViewModel GenerateReport(DocReport report, List<int> userIds, int groupId)
        {
            var result = new ReportTableViewModel 
            { 
                DocReport = report
            };

            var query = _documentRepository.GetUserInGroupDocumentsByGroupId(groupId, DocumentStatus.Submited);

            var userWithTagsIds = GetUserIdsListForReport(report, groupId);
            var userIdsForFilter = userIds ?? (report.UserTags.Count > 0 ? userWithTagsIds : null);
            query = ApplyFilders(report.FilterStartDate, report.FilterEndDate, report.ReportAppointmentType, userIdsForFilter , query);

            var docGroups = GroupDocuments(report.ReportGroupType, query);

            //имена строк
            var names = new Dictionary<long, string>();

            switch (report.ReportGroupType)
            {
                case DocReportGroupType.None:
                    foreach (var documentGroup in docGroups)
                    {
                        names.Add(documentGroup.EntityId, string.Empty);
                    }
                    break;
                case DocReportGroupType.DocAppointment:
                    foreach (var documentGroup in docGroups)
                    {
                        names.Add(documentGroup.EntityId, _docAppointmentRepository.GetDocAppointmentById(documentGroup.EntityId).Name);
                    }
                    break;
                case DocReportGroupType.User:
                    {
                        foreach (var documentGroup in docGroups)
                        {
                            //Convert.ToInt32 - сделан преднамернно для того что бы засунуть весь код генерации отчетов в 1 метод
                            names.Add(documentGroup.EntityId, _userAccountRepository.GetById(Convert.ToInt32(documentGroup.EntityId)).FullName);
                        }
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            var repFields = report.ReportFields.ToList();
            
            foreach (var documentGroup in docGroups)
            {
                string name;
                names.TryGetValue(documentGroup.EntityId, out name);
                result.Rows.Add(GenerateRow(repFields, documentGroup, name));
            }
            if (report.ReportGroupType != DocReportGroupType.None)
            {
                result.TotalRow = GenerateTotalRow(repFields, result.Rows, "Итого");
            }
            return result;
        }

        /// <summary>
        /// Возвращает список Id пользователей данных которых будут учтены в отчете
        /// </summary>
        /// <param name="report"></param>
        /// <param name="groupId"> </param>
        /// <returns></returns>
        public List<int> GetUserIdsListForReport(DocReport report, int groupId)
        {
            if(report.UserTags != null && report.UserTags.Count > 0)
            {
                return report.UserTags.SelectMany(x => x.UserAccounts.Select(y => y.userid)).Distinct().ToList();
            }
            return _userGroupRepository.GetById(groupId).Members.Select(x => x.userid).ToList();
        }

        /// <summary>
        /// Конвертирует из Модели данных для таблицы отчета в матричное представление
        /// </summary>
        /// <param name="reportTableViewModel"></param>
        /// <returns></returns>
        public IEnumerable<ReportDataFieldForRPV> ConvertReportForRPV(ReportTableViewModel reportTableViewModel)
        {
            var result = new List<ReportDataFieldForRPV>();
            var rowNumber = 0;
            foreach (var reportRow in reportTableViewModel.Rows)
            {
                rowNumber++;
                result.AddRange(from val in reportRow.Values
                                let col = reportTableViewModel.DocReport.ReportFields.Single(x => x.reportfieldid == val.Key)
                                select new ReportDataFieldForRPV
                                    { 
                                        Column = string.Format("{0} {1}",col.FieldTemplate.FieldName, DictionaryHelper.GetEnumText(typeof (ReportFieldOperationType), col.ReportOperationType)), 
                                        ColumnNumber = col.OrderNumber, 
                                        Value = val.Value, 
                                        RowNumber = rowNumber,
                                        Row = string.Format("{0:000}. {1}", rowNumber, reportRow.Name),
                                        Legend = reportTableViewModel.Legend,
                                        ReportName = reportTableViewModel.Name
                                    });
            }

            if(reportTableViewModel.TotalRow != null)
            {
                rowNumber++;
                result.AddRange(from val in reportTableViewModel.TotalRow.Values
                                let col = reportTableViewModel.DocReport.ReportFields.Single(x => x.reportfieldid == val.Key)
                                select new ReportDataFieldForRPV
                                    {
                                        Column = string.Format("{0} {1}",col.FieldTemplate.FieldName, DictionaryHelper.GetEnumText(typeof (ReportFieldOperationType), col.ReportOperationType)),
                                        ColumnNumber = col.OrderNumber, 
                                        Value = val.Value, 
                                        RowNumber = rowNumber,
                                        Row = string.Format("{0:000}. {1}", rowNumber, reportTableViewModel.TotalRow.Name),
                                        Legend = reportTableViewModel.Legend,
                                        ReportName = reportTableViewModel.Name
                                    });
            }
            return result;
        }

        /// <summary>
        /// Создает главный отчет 
        /// </summary>
        /// <param name="report"> Суммарный отчет</param>
        /// <param name="userIds"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public SortedList<int, ReportTableViewModel>  GenerateMainReport(MainDocReport report, List<int> userIds, int groupId)
        {
            var retVal = new SortedList<int, ReportTableViewModel>();
            foreach (var reportsOrder in report.DocInMainReportsOrders)
            {
                retVal.Add(reportsOrder.OrderNumber, GenerateReport(reportsOrder.DocReport, userIds, groupId));
            }
            return retVal;
        }

        #endregion

        #region Misc

        /// <summary>
        /// накладыввает фильтры на запрос
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="reportAppointmentType"></param>
        /// <param name="usrIds"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private IQueryable<Document> ApplyFilders(DateTime startDateTime, DateTime endDateTime,
            DocReportAppointmentType reportAppointmentType, List<int> usrIds, IQueryable<Document> query)
        {
            //Start date time
            query = query.Where(
                x =>
                (x.DocAppointment.PlanedStartDate != null && x.DocAppointment.PlanedStartDate >= startDateTime) ||
                (x.DocAppointment.PlanedStartDate == null && x.DocAppointment.ActualStartDate >= startDateTime));
            //End date time
            query = query.Where(
                x => x.DocAppointment.ActualEndDate <= endDateTime);

            //Applointment type
            switch (reportAppointmentType)
            {
                case DocReportAppointmentType.User:
                    query =
                        query.Where(
                            x =>
                            x.DocAppointment.UserAccount_userid != null);
                    break;
                case DocReportAppointmentType.Group:
                    query =
                        query.Where(
                            x =>
                            x.DocAppointment.UserAccount_userid == null);
                    break;
                case DocReportAppointmentType.Both:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("reportAppointmentType");
            }

            //UserIds
            if (usrIds != null)
            {
                query = query.Where(x => usrIds.Contains(x.UserAccount_userid));
            }

            return query;
        }

        /// <summary>
        /// Группирует все документы  в зависмости от вида отчета
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<DocumentGroup> GroupDocuments(DocReportGroupType groupType, IQueryable<Document> query)
        {
            var result = new List<DocumentGroup>();
            switch (groupType)
            {
                case DocReportGroupType.None:
                    result.AddRange(query.Select(x => new DocumentGroup
                    {
                        EntityId = x.documentid,
                        Documents = new List<Document> { x }
                    }));
                    break;
                case DocReportGroupType.User:
                    var userIds = query.Select(x => x.UserAccount_userid).Distinct().ToList();
                    result.AddRange(userIds.Select(userId => new DocumentGroup
                    {
                        EntityId = userId,
                        Documents = query.Where(x => x.UserAccount_userid == userId).ToList()
                    }));
                    break;
                case DocReportGroupType.DocAppointment:
                    var docAppIds = query.Select(x => x.DocAppointment_docappointmentid).Distinct().ToList();
                    result.AddRange(docAppIds.Select(docAppId => new DocumentGroup
                    {
                        EntityId = docAppId,
                        Documents = query.Where(x => x.DocAppointment_docappointmentid == docAppId).ToList()
                    }));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("groupType");
            }
            return result;
        }

        /// <summary>
        /// Вычисляет строку из группы документов
        /// </summary>
        /// <param name="reportFileds"></param>
        /// <param name="documentGroup"></param>
        /// <param name="rowName"> </param>
        /// <returns></returns>
        private ReportDataRow GenerateRow(List<ReportField> reportFileds, DocumentGroup documentGroup, string rowName)
        {
            var result = new ReportDataRow {Name = rowName};
            //если документов нет - то строка пустая
            if (documentGroup.Documents.Count == 0)
            {
                reportFileds.ForEach(x => result.Values.Add(x.reportfieldid, string.Empty));
            }
            //Если документов 1 - то просто выводим этот документ в строку
            if (documentGroup.Documents.Count == 1)
            {
                var doc = documentGroup.Documents.ElementAt(0);
                reportFileds.ForEach(x => result.Values.Add(
                    x.reportfieldid, 
                    doc.DocFields.Single(y => 
                        y.FieldTemplate_fieldteplateid == x.FieldTemplate_fieldteplateid)
                        .GetValueString()));
            }
            //Если документов больше 1, то нужно вычсилять каждое значение как среднее или сумму
            if (documentGroup.Documents.Count > 1)
            {
                foreach (var reportFiled in reportFileds)
                {
                    //Получаем список полей данного типа из всех документов
                    var fields = new List<DocField>();
                    documentGroup.Documents.ForEach(x => fields.Add(x.DocFields.Single(y => y.FieldTemplate_fieldteplateid == reportFiled.FieldTemplate_fieldteplateid)));
                    result.Values.Add(
                        reportFiled.reportfieldid,
                        reportFiled.ReportFieldOperationType == ReportFieldOperationType.Midle
                            ? string.Format(SmqSettings.Instance.DoubleFormatStr, fields.Average(x => x.DoubleValue))
                            : string.Format(SmqSettings.Instance.DoubleFormatStr, fields.Sum(x => x.DoubleValue)));
                }
               
            }
            return result;
        }

        /// <summary>
        /// Вычисляет строку из группы документов
        /// Надо обдумать как оптимизировать (убрать string to double)
        /// </summary>
        /// <param name="reportFileds"></param>
        /// <param name="rows"> </param>
        /// <param name="rowName"> </param>
        /// <returns></returns>
        private ReportDataRow GenerateTotalRow(List<ReportField> reportFileds,List<ReportDataRow> rows, string rowName)
        {
            var result = new ReportDataRow { Name = rowName };
            //если строк нет - то строка пустая
            if (rows.Count == 0)
            {
                reportFileds.ForEach(x => result.Values.Add(x.reportfieldid, string.Empty));
            }
            //Если строк 1 - то просто выводим этот документ в строку
            if (rows.Count == 1)
            {
                foreach (var value in rows.ElementAt(0).Values)
                {
                    result.Values.Add(value.Key, value.Value);
                }
            }
            //Если документов больше 1, то нужно вычсилять каждое значение как среднее или сумму
            if (rows.Count > 1)
            {
                foreach (var reportFiled in reportFileds)
                {
                    //Получаем список полей данного типа из всех документов
                    var strFields = new List<string>();
                    rows.ForEach(x => strFields.Add(x.Values.Single(y => y.Key == reportFiled.reportfieldid).Value));
                    var fields = strFields.Select(Convert.ToDouble).ToList();
                    result.Values.Add(
                        reportFiled.reportfieldid,
                        reportFiled.ReportFieldOperationType == ReportFieldOperationType.Midle
                            ? string.Format(SmqSettings.Instance.DoubleFormatStr, fields.Average())
                            : string.Format(SmqSettings.Instance.DoubleFormatStr, fields.Sum()));
                }

            }
            return result;
        }
        #endregion
      
    }

    #region Assist classes
    /// <summary>
    /// Класс помщник формирования
    /// </summary>
    internal class DocumentGroup
    {
        /// <summary>
        /// Идентификтор сущности по которйо сгруппированы записи
        /// </summary>
        public long EntityId { get; set; }
        /// <summary>
        /// Список документов в группировке
        /// </summary>
        public List<Document> Documents { get; set; } 
    }
    #endregion
    
}