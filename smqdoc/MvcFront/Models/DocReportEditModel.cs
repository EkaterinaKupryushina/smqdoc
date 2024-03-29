﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocReportEditModel :IValidatableObject
    {
        [Required]
        [UIHint("Hidden")]
        public int DocReportId { get; set; }

        [Required]
        public long DocTemplateId { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Начало периода")]
        public DateTime FilterStartDate { get; set; }

        [Display(Name = "Конец периода")]
        public DateTime FilterEndDate { get; set; }

        [Display(Name = "Тип назначения")]
        public int ReportAppointmentType { get; set; }

        [Display(Name = "Группировка строк")]
        public int ReportGroupType { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Легенда")]
        public string Legend { get; set; }

        [Display(Name = "Включен?")]
        public bool IsActive { get; set; }

        public bool IsGroupChangeAvailable { get; set; }

        public DocReportEditModel()
        {
            IsGroupChangeAvailable = true;
        }

        public DocReportEditModel(DocReport entity )
        {
            DocReportId = entity.docreportid;
            DocTemplateId = entity.DocTemplate_docteplateid;

            Name = entity.Name;
            FilterStartDate = entity.FilterStartDate;
            FilterEndDate = entity.FilterEndDate;
            ReportAppointmentType = entity.DocAppointmetType;
            ReportGroupType = entity.GroupType;
            IsActive = entity.IsActive;
            IsGroupChangeAvailable = entity.ReportFields== null || entity.ReportFields.Count == 0;
            Legend = entity.Legend;
        }

        public DocReport Update(DocReport entity)
        {
            entity.Name = Name;
            entity.DocTemplate_docteplateid = DocTemplateId;
            entity.FilterStartDate = FilterStartDate;
            entity.FilterEndDate = FilterEndDate;
            entity.DocAppointmetType = ReportAppointmentType;
            entity.GroupType = ReportGroupType;
            entity.IsActive = IsActive;
            entity.Legend = Legend;

            return entity;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if( FilterStartDate >= FilterEndDate)
            {
                yield return new ValidationResult("Проверьте период докуметов в отчете");
            }
        }
    }


}