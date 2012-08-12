using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Infrastructure;

namespace MvcFront.Models
{
    public class DocumentListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        
        [Display(Name = "Название")]
        public string Name { get; set; }
        
        [Display(Name = "Статус")]
        public string DocumentStatusText { get; set; }
        
        [Display(Name = "Последний комментарий")]
        public string LastComment { get; set; }

        [Display(Name = "Владелец")]
        public string Owner { get; set; }

        [Display(Name = "Последнее изменение")]
        public DateTime LastEditDate { get; set; }
        
        [Display(Name = "Окончание заполнения")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Только Чтение")]
        [UIHint("Hidden")]
        public bool IsReadOnly { get; set; }
        
        [Display(Name = "Coloring")]
        [UIHint("Hidden")]
        public bool IsRed    { get; set; }
        
        public DocumentListViewModel()
        {
        }

        public DocumentListViewModel(Document templ)
        {
            DocumentId = templ.documentid;
            Name = templ.DocAppointment.Name;
            LastEditDate = templ.LastEditDate;
            LastComment = string.IsNullOrWhiteSpace(templ.LastComment) ? "-" : templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            IsReadOnly = templ.DocStatus != DocumentStatus.PlanEditing && templ.DocStatus != DocumentStatus.FactEditing;
            DateEnd = (templ.DocStatus == DocumentStatus.PlanEditing || templ.DocStatus == DocumentStatus.PlanSended) && templ.DocAppointment.PlanedEndDate.HasValue
                          ? templ.DocAppointment.PlanedEndDate.Value
                          : templ.DocAppointment.ActualEndDate;
            IsRed = DateTime.Now.AddDays(SmqSettings.Instance.DocumentsDedlineWarning) >= DateEnd;
            Owner = templ.UserAccount.FullName;
        }
        public static DocumentListViewModel DocumentToModelConverter(Document templ)
        {
            return new DocumentListViewModel(templ);
        }
    }
}