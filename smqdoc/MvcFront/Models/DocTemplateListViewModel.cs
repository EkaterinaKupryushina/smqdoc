using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocTemplateListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Display(Name = "Название Формы")]
        public string DocTemplateName { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }
        
        public DocTemplateListViewModel()
        {
        }
        
        public DocTemplateListViewModel(DocTemplate templ)
        {
            DocTemplateId = templ.docteplateid;
            DocTemplateName = templ.TemplateName;
            LastEditDate = templ.LastEditDate;
        }
        
        public static DocTemplateListViewModel DocTemplateToModelConverter(DocTemplate templ)
        {
            return new DocTemplateListViewModel(templ);
        }
    }
}