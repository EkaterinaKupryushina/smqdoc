using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcFront.DB;
using MvcFront.Helpers;

namespace MvcFront.Models
{
    public class DocumentListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        [Display(Name = "Названире документа")]
        public string DocumentName { get; set; }
        [Display(Name = "Статус документа")]
        public string DocumentStatusText { get; set; }
        [Display(Name = "Последний комментарий")]
        public string LastComment { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }
        public DocumentListViewModel()
        {
        }
        public DocumentListViewModel(Document templ)
        {
            DocumentId = templ.documentid;
            DocumentName = templ.DocumentName;
            LastEditDate = templ.LastEditDate;
            LastComment = templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
        }
        public static DocumentListViewModel DocumentToModelConverter(Document templ)
        {
            return new DocumentListViewModel(templ);
        }
    }
}