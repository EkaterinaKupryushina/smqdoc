using System.ComponentModel.DataAnnotations;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Models
{
    public class ReportFieldEditModel
    {

        public long ReportFieldId { get; set; }
        public int DocReportId { get; set; }
        public bool IsGrouped { get; set; }
        [Display(Name = "Поле документа")]
        public long FiledTemplateId { get; set; }

        [Display(Name = "Операция")]
        public int OperationType { get; set; }

        public ReportFieldEditModel()
        {
        }

        public ReportFieldEditModel(ReportField fld)
        {
            ReportFieldId = fld.reportfieldid;
            DocReportId = fld.DocReport_reportid;
            FiledTemplateId = fld.FieldTemplate_fieldteplateid;
            IsGrouped = fld.DocReport.ReportGroupType != DocReportGroupType.None;
            OperationType = fld.ReportOperationType;
        }

        public ReportField Update(ReportField fld)
        {
            fld.ReportOperationType = OperationType;
            fld.DocReport_reportid = DocReportId;
            fld.FieldTemplate_fieldteplateid = FiledTemplateId;
            return fld;
        }

    }
}