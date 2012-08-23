using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocTemplateEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Display(Name = "Название Формы")]
        public string DocTemplateName { get; set; }
        [Display(Name = "Описание Формы")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        [Display(Name = "Разрешить приложения")]
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