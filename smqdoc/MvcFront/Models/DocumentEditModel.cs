using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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

        [Display(Name = "������ ��� ������")]
        [UIHint("Hidden")]
        public bool IsReadOnly { get; set; }

        [Display(Name = "�������� ����������")]
        public string AttachmentName { get; set; }

        [Display(Name = "����������")]
        public IEnumerable<HttpPostedFileBase> Files { get; set; }

        public List<DocFieldEditModel> Fields { get; set; }

        public bool AllowAttachments { get; set; }

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
            LastComment = string.IsNullOrWhiteSpace(templ.LastComment) ? "-" : templ.LastComment;
            DocumentStatusText = templ.DocStatusText;
            Fields = templ.DocFields.OrderBy(x => x.FieldTemplate.OrderNumber).ToList().ConvertAll(DocFieldEditModel.FieldToModelConverter).ToList();
            AllowAttachments = templ.DocAppointment.DocTemplate.AllowAttachment;
            if(AllowAttachments)
            {
                AttachmentName = templ.DisplayFileName;
            }
            IsReadOnly = templ.DocStatus != DocumentStatus.PlanEditing && templ.DocStatus != DocumentStatus.FactEditing;
        }

    }
}