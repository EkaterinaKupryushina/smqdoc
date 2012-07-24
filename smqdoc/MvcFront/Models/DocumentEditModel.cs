using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Models
{
    public class DocumentEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocumentId { get; set; }
        
        [Display(Name = "�������� ���������")]
        public string Name { get; set; }
        
        [Display(Name = "������ ���������")]
        public string DocumentStatusText { get; set; }
        
        [Display(Name = "��������� �����������")]
        public string LastComment { get; set; }
        
        [Display(Name = "���� ���������� ���������")]
        public DateTime LastEditDate { get; set; }

        public List<DocFieldEditModel> Fields { get; set; }
        
        public DocumentEditModel()
        {
            Fields = new List<DocFieldEditModel>();
        }
        public DocumentEditModel(Document templ)
            : this()
        {
            DocumentId = templ.documentid;
            Name = templ.DocAppointment.Name;
            LastEditDate = templ.LastEditDate;
            LastComment = templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            Fields = templ.DocFields.OrderBy(x => x.FieldTemplate.OrderNumber).ToList().ConvertAll(DocFieldEditModel.FieldToModelConverter).ToList();
        }
        public static DocumentEditModel DocumentToModelConverter(Document templ)
        {
            return new DocumentEditModel(templ);
        }

    }
}