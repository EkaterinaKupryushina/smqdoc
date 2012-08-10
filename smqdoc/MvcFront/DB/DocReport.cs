using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.Enums;

namespace MvcFront.DB
{
    [MetadataType(typeof(DocReportMetadata))]
    public partial class DocReport
    {
        /// <summary>
        /// Тип назначения к которому применим отчет
        /// </summary>
        public DocReportAppointmentType ReportAppointmentType
        {
            get { return (DocReportAppointmentType) DocAppointmetType; }
            set { DocAppointmetType = (int) value; }
        }

        /// <summary>
        /// Тип группировки используемый для формирования строк
        /// </summary>
        public DocReportGroupType ReportGroupType
        {
            get { return (DocReportGroupType) GroupType; }
            set { GroupType = (int) value; }
        }

    }

    public class DocReportMetadata
    {
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Начало периода")]
        public DateTime FilterStartDate { get; set; }

        [Display(Name = "Конец периода")]
        public DateTime FilterEndDate { get; set; }

        [Display(Name = "Тип назначения")]
        public DocReportAppointmentType ReportAppointmentType { get; set; }

        [Display(Name = "Группировка строк")]
        public DocReportGroupType ReportGroupType { get; set; }

        [Display(Name = "Статус отчета")]
        public bool IsActive { get; set; }
    }
}