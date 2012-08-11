using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class ReportFieldListViewModel
    {
        [Display(Name = "ID")]
        public long FieldId { get; set; }
        [Display(Name = "№")]
        public long OrderNumber { get; set; }
        [Display(Name = "Название поля")]
        public string Name { get; set; }

        public ReportFieldListViewModel()
        {
        }

        public ReportFieldListViewModel(ReportField fld)
        {
            FieldId = fld.reportfieldid;
            OrderNumber = fld.OrderNumber;
            Name = fld.FieldTemplate.FieldName;
        }

        public static ReportFieldListViewModel FieldToModelConverter(ReportField templ)
        {
            return new ReportFieldListViewModel(templ);
        }
    }
}