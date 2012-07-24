using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Models
{
    public class DocumentListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        
        [Display(Name = "Название документа")]
        public string Name { get; set; }
        
        [Display(Name = "Статус документа")]
        public string DocumentStatusText { get; set; }
        
        [Display(Name = "Последний комментарий")]
        public string LastComment { get; set; }
        
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }
        
        [Display(Name = "Окончание заполенния")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Только Чтение")]
        [UIHint("Hidden")]
        public bool IsReadOnly { get; set; }
        
        [Display(Name = "Coloring")]
        [UIHint("Hidden")]
        public bool IsRed { get; set; }
        
        public DocumentListViewModel()
        {
        }
        public DocumentListViewModel(Document templ)
        {
            DocumentId = templ.documentid;
            LastEditDate = templ.LastEditDate;
            LastComment = templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            IsReadOnly = templ.DocStatus != DocumentStatus.Editing;

            //TODO сделать заполенние
            if (templ.DocAppointment.GroupTemplate != null)
            {
                //DateEnd = templ.DocAppointment.GroupTemplate.DateEnd;
            
            //IsRed = templ.GroupTemplate.DateEnd < DateTime.Now.AddDays(SmqSettings.Instance.DocumentsDedlineWarning) &&
                   // templ.Status == (int) DocumentStatus.Editing;
            }

        Name = templ.DocAppointment.Name;

        }
        public static DocumentListViewModel DocumentToModelConverter(Document templ)
        {
            return new DocumentListViewModel(templ);
        }
    }
}