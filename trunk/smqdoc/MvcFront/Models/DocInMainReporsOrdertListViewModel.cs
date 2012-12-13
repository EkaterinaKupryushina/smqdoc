using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocInMainReportsOrderListViewModel
    {

        public int DocInMainReportsOrderId { get; set; }

        [Display(Name="Название")]
        public string Name { get; set; }

        [Display(Name = "Форма")]
        public string DocTemplateName { get; set; }

        [Display(Name = "Статус")]
        public string IsActive { get; set; }

        public DocInMainReportsOrderListViewModel()
        {
        }

        public DocInMainReportsOrderListViewModel(DocInMainReportsOrder entity)
        {
            DocInMainReportsOrderId = entity.docinmainreportsorderid;
            Name = entity.DocReport.Name;
            IsActive = entity.DocReport.IsActive ? "Активный" : "Отключен";
            DocTemplateName = entity.DocReport.DocTemplate.TemplateName;
        }

        public static DocInMainReportsOrderListViewModel DocReportToModelConverter(DocInMainReportsOrder templ)
        {
            return new DocInMainReportsOrderListViewModel(templ);
        }
    }
}