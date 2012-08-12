using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Models
{
    public class DocAppointmentEditModel :IValidatableObject
    {
        public long DocAppointmentId { get; set; }
        public long DocTemplateId { get; set; }
       
        [Display(Name = "Название Формы")]
        public string TemplateName { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Дата начала заполнения планируемых данных")]
        public DateTime PlanedStartDate { get; set; }
        [Required]
        [Display(Name = "Дата окончания заполнения планируемых данных")]
        public DateTime PlanedEndDate { get; set; }
        [Required]
        [Display(Name = "Дата начала заполнения фактических данных")]
        public DateTime ActualStartDate { get; set; }
        [Required]
        [Display(Name = "Дата окончания заполнения фактических данных")]
        public DateTime ActualEndDate { get; set; }
        [Display(Name = "Содержит планируемые поля")]
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
            PlanedEndDate = entity.PlanedEndDate.HasValue ? entity.PlanedEndDate.Value : DateTime.Now.AddDays(1);
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ActualStartDate > ActualEndDate)
            {
                yield return new ValidationResult("Проверьте период заполнения фактических значений");
            }
            if(NeedPlanDates)
            {
                if (PlanedStartDate > PlanedEndDate)
                {
                    yield return new ValidationResult("Проверьте период заполнения плинируемых значений");
                }
                if (PlanedEndDate> ActualStartDate)
                {
                    yield return new ValidationResult("Проверьте период заполнения плинируемых и фактических значений");
                }
            }
        }
    }
}