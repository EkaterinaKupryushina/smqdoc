using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocTemplateEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Display(Name = "�������� �����")]
        public string DocTemplateName { get; set; }
        [Display(Name = "�������� �����")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        [Display(Name = "��������� ����������")]
        public bool AllowAttachment { get; set; }

        public DocTemplateEditModel()
        {
        }
        
        public DocTemplateEditModel(DocTemplate templ)
        {
            DocTemplateId = templ.docteplateid;
            DocTemplateName = templ.TemplateName;
            Comment = templ.Comment;
            AllowAttachment = templ.AllowAttachment;
        }
        
        public DocTemplate Update(DocTemplate templ)
        {
            templ.docteplateid = DocTemplateId;
            templ.TemplateName = DocTemplateName;
            templ.Comment = Comment;
            templ.AllowAttachment = AllowAttachment;
            return templ;
        }
    }
}