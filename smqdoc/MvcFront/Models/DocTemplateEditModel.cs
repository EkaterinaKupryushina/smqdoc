using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class DocTemplateEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Display(Name = "Название шаблона")]
        public string DocTemplateName { get; set; }
        [Display(Name = "Описание шаблона")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        
        public DocTemplateEditModel()
        {
        }
        
        public DocTemplateEditModel(DocTemplate templ)
        {
            DocTemplateId = templ.docteplateid;
            DocTemplateName = templ.TemplateName;
            Comment = templ.Comment;
        }
        
        public DocTemplate Update(DocTemplate templ)
        {
            templ.docteplateid = DocTemplateId;
            templ.TemplateName = DocTemplateName;
            templ.Comment = Comment;
            return templ;
        }
    }
}