using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;
using MvcFront.Infrastructure;

namespace MvcFront.Models
{
    public class DocAppointmentListViewModel 
    {
        public long ID { get; set; }

        [Display(Name = "Название Формы")]
        public string TemplateName { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Начало заполнения данных")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Окончание заполнения данных")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Статус")]
        public string StatusText{ get; set; }
        [Display(Name = "Coloring")]
        [UIHint("Hidden")]
        public bool IsRed { get; set; }

        public DocAppointmentListViewModel()
        {
        }

        public DocAppointmentListViewModel(DocAppointment entity)
        {
            ID = entity.docappointmentid;
            Name = entity.Name;
            StatusText = entity.DocAppointmentStatusText;
            TemplateName = entity.DocTemplate.TemplateName;
            StartDate = entity.PlanedStartDate.HasValue
                         ? entity.PlanedStartDate.Value
                         : entity.ActualStartDate;
            EndDate = entity.ActualEndDate;
            var currIterationEndDate = entity.PlanedEndDate.HasValue
                         ? entity.PlanedEndDate.Value
                         : entity.ActualEndDate;
            IsRed = DateTime.Now.AddDays(SmqSettings.Instance.DocumentsDedlineWarning) >= currIterationEndDate;
        }

        public static DocAppointmentListViewModel DocAppointmentToModelConverter(DocAppointment templ)
        {
            return new DocAppointmentListViewModel(templ);
        }
    }
}