 using System;
using System.Linq;
 using System.ComponentModel.DataAnnotations;
 using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocumentAppointmentListViewModel
    {
        [Required]
        [UIHint("Hidden")]
        public long ID { get; set; }

        [Required]
        [Display(Name = "Наименование назначения ")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Начала заполенния")]
        public DateTime DateStart { get; set; }

        [Required]
        [Display(Name = "Окончание заполенния")]
        public DateTime DateEnd { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public String Status { get; set; }

        [Display(Name = "Родительский шаблон")]
        public String DocTemplateName { get; set; }

        [Display(Name = "Док-ты")]
        public int DocCount { get; set; }

        public DocumentAppointmentListViewModel()
        {
        }

        public DocumentAppointmentListViewModel(GroupTemplate tpl)
        {
            ID = tpl.grouptemplateid;
            Name = tpl.Name;
            DateStart = tpl.DateStart;
            DateEnd = tpl.DateEnd;
            Status = tpl.GroupTemplateStatusText;
            DocTemplateName = tpl.DocTemplate.TemplateName;
            DocCount = tpl.Documents.Count(x=>x.Status != (int)DocumentStatus.Deleted);
        }

        public static DocumentAppointmentListViewModel GroupTemplateToModelConverter(GroupTemplate tpl)
        {
            return new DocumentAppointmentListViewModel(tpl);
        }
    }

    public class UserDocumentListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        [Display(Name = "Названире документа")]
        public string DocumentName { get; set; }
        [Display(Name = "Имя автора")]
        public string UserName { get; set; }
        [Display(Name = "Статус документа")]
        public string DocumentStatusText { get; set; }
        [Display(Name = "Последний комментарий")]
        public string LastComment { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }

        [Display(Name = "Status")]
        [UIHint("Hidden")]
        public bool IsReadOnly { get; set; }
        public UserDocumentListViewModel()
        {
        }
        public UserDocumentListViewModel(Document templ)
        {
            DocumentId = templ.documentid;            
            LastEditDate = templ.LastEditDate;
            LastComment = templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            UserName = templ.UserAccount.FullName;
            IsReadOnly = templ.DocStatus == DocumentStatus.Submited;
        }
        public static UserDocumentListViewModel DocumentToModelConverter(Document templ)
        {
            return new UserDocumentListViewModel(templ);
        }


    }

    public class GroupDocumentListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        [Display(Name = "Название")]
        public string GroupTemplateName { get; set; }
        [Display(Name = "Статус")]
        public string DocumentStatusText { get; set; }
        [Display(Name = "Последний комментарий")]
        public string LastComment { get; set; }
        [Display(Name = "Последнее изменение")]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "Окончание заполенния")]
        public DateTime DateEnd { get; set; }
        [Display(Name = "Сотрудник")]
        public string UserName { get; set; }

        [Display(Name = "Status")]
        [UIHint("Hidden")]
        public bool IsReadOnly { get; set; }
        [Display(Name = "Coloring")]
        [UIHint("Hidden")]
        public bool IsRed { get; set; }
        public GroupDocumentListViewModel()
        {
        }
        public GroupDocumentListViewModel(Document templ)
        {
            DocumentId = templ.documentid;
            LastEditDate = templ.LastEditDate;
            LastComment = templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            IsReadOnly = templ.DocStatus != DocumentStatus.Editing;
            DateEnd = templ.GroupTemplate.DateEnd;
            IsRed = templ.GroupTemplate.DateEnd < DateTime.Now.AddDays(2) &&
                    templ.Status == (int)DocumentStatus.Editing;

            GroupTemplateName = templ.GroupTemplate.Name;
            UserName = templ.UserAccount.FullName;
        }
        public static GroupDocumentListViewModel DocumentToModelConverter(Document templ)
        {
            return new GroupDocumentListViewModel(templ);
        }
    }
}