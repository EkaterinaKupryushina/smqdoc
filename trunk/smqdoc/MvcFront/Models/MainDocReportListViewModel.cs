using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class MainDocReportListViewModel
    {
        
        public int MainDocReportId { get; set; }

        [Display(Name="Название")]
        public string Name { get; set; }


        public MainDocReportListViewModel()
        {
        }

        public MainDocReportListViewModel(MainDocReport entity )
        {
            MainDocReportId = entity.maindocreportid;
            Name = entity.Name;
        }

        public static MainDocReportListViewModel MainDocReportToModelConverter(MainDocReport templ)
        {
            return new MainDocReportListViewModel(templ);
        }
    }
}