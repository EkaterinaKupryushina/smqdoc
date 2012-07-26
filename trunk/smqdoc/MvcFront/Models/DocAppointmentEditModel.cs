using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Models
{
    public class DocAppointmentEditModel
    {
        public long DocAppointmentId { get; set; }
        public long DocTemplateId { get; set; }
       
        [Display(Name = "Название Формы")]
        public string TemplateName { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Начало заполнения Планируемых данных")]
        public DateTime PlanedStartDate { get; set; }
        [Required]
        [Display(Name = "Окончание заполнения Планируемых данных")]
        public DateTime PlanedEndDate { get; set; }
        [Required]
        [Display(Name = "Начало заполнения Фактических данных")]
        public DateTime ActualStartDate { get; set; }
        [Required]
        [Display(Name = "Окончание заполнения Фактических данных")]
        public DateTime ActualEndDate { get; set; }
        [Display(Name = "Содержит планируемо(ые) поле(я)")]
        public bool NeedPlanDates { get; set; }
        [Display(Name = "Статус")]
        public string StatusText{ get; set; }

        public DocAppointmentEditModel()
        {
        }

        public DocAppointmentEditModel(DocAppointment entity)
        {
            DocAppointmentId = entity.docappointmentid;
            Name = entity.Name;
            StatusText = entity.DocAppointmentStatusText;
            DocTemplateId = entity.DocTemplate_docteplateid;
            TemplateName = entity.DocTemplate.TemplateName;
            ActualStartDate = entity.ActualStartDate;
            ActualEndDate = entity.ActualEndDate;
            NeedPlanDates = entity.DocTemplate.FieldTeplates.Any(x => x.FiledType == (int)FieldTemplateType.Planned);
            PlanedEndDate = entity.PlanedEndDate.HasValue ? entity.PlanedEndDate.Value : DateTime.Now;
            PlanedStartDate = entity.PlanedStartDate.HasValue ? entity.PlanedStartDate.Value : DateTime.Now;
        }

        public DocAppointment Update(DocAppointment entity)
        {
            entity.Name = Name;
            entity.ActualStartDate = ActualStartDate;
            entity.ActualEndDate = ActualEndDate;

            entity.PlanedEndDate = NeedPlanDates ? PlanedEndDate : (DateTime?)null;
            entity.PlanedStartDate = NeedPlanDates ? PlanedStartDate : (DateTime?)null;

            entity.DocTemplate_docteplateid = DocTemplateId;
            return entity;
        }
    }
}