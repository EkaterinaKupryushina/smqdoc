using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocReportListViewModel
    {
        
        public int DocReportId { get; set; }

        [Display(Name="Название")]
        public string Name { get; set; }

        [Display(Name = "Форма")]
        public string DocTemplateName { get; set; }

        [Display(Name = "Статус")]
        public string IsActive { get; set; }

        public DocReportListViewModel()
        {
        }

        public DocReportListViewModel(DocReport entity )
        {
            DocReportId = entity.docreportid;
            Name = entity.Name;
            IsActive = entity.IsActive ? "Активный" : "Отключен";
            DocTemplateName = entity.DocTemplate.TemplateName;
        }

        public static DocReportListViewModel DocReportToModelConverter(DocReport templ)
        {
            return new DocReportListViewModel(templ);
        }
    }
}