using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class MainDocReportEditModel
    {
        [Required]
        [UIHint("Hidden")]
        public int MainDocReportId { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }


        public MainDocReportEditModel()
        {
        }

        public MainDocReportEditModel(MainDocReport entity)
        {
            MainDocReportId = entity.maindocreportid;
            Name = entity.Name;
        }

        public MainDocReport Update(MainDocReport entity)
        {
            entity.Name = Name;
            return entity;
        }
    }
}