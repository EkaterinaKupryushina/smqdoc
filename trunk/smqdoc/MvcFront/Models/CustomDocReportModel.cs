﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class CustomDocReportModel : IValidatableObject
    {
        [Required]
        [UIHint("Hidden")]
        public int DocReportId { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Пользователи")]
        public List<BoolIntSetting> Users { get; set; }
        
        [Display(Name = "Отчет")]
        public ReportTableViewModel ReportTableView { get; set; }

        public CustomDocReportModel()
        {
            Users = new List<BoolIntSetting>();
        }

        public CustomDocReportModel(ReportTableViewModel entity):base()
        {
            Name = entity.Name;
            DocReportId = entity.DocReport.docreportid;
            ReportTableView = entity;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Users.Count == 0)
            {
                yield return new ValidationResult("Выберите хотябы одного пользователя");
            }
        }
    }
}