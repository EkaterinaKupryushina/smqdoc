using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class PersonalDocTemplateListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public int PersonalDocTemplateId { get; set; }
        [Display(Name = "Название Формы")]
        public string DocTemplateName { get; set; }
        [Display(Name = "Описание формы")]
        public string Comment { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "Статус")]
        public string StatusText { get; set; }

        public PersonalDocTemplateListViewModel()
        {
        }

        public PersonalDocTemplateListViewModel(PersonalDocTemplate templ)
        {
            PersonalDocTemplateId = templ.personaldoctemplateid;
            DocTemplateName = templ.DocTemplate.TemplateName;
            LastEditDate = templ.DocTemplate.LastEditDate;
            Comment = templ.DocTemplate.Comment;
            StatusText = templ.DocTemplate.TemplateStatusText;
        }

        public static PersonalDocTemplateListViewModel PersonalDocTemplateListViewModelToModelConverter(PersonalDocTemplate templ)
        {
            return new PersonalDocTemplateListViewModel(templ);
        }
    }
}