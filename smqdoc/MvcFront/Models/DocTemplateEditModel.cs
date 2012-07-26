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
        [Display(Name = "Разрешить самоназначение")]
        public bool AllowUserUse { get; set; }
         [Display(Name = "Разрешить много документов")]
        public bool AllowMultyDocs { get; set; }

        public DocTemplateEditModel()
        {
        }
        
        public DocTemplateEditModel(DocTemplate templ)
        {
            DocTemplateId = templ.docteplateid;
            DocTemplateName = templ.TemplateName;
            Comment = templ.Comment;
            if(templ.DocTemplatesForUser != null)
            {
                AllowUserUse = true;
                AllowMultyDocs = templ.DocTemplatesForUser.AllowManyInstances;
            }
        }
        
        public DocTemplate Update(DocTemplate templ)
        {
            templ.docteplateid = DocTemplateId;
            templ.TemplateName = DocTemplateName;
            templ.Comment = Comment;
            if(AllowUserUse)
            {
               if(templ.DocTemplatesForUser == null)
               {
                   templ.DocTemplatesForUser = new DocTemplatesForUser();
               }
                templ.DocTemplatesForUser.AllowManyInstances = AllowMultyDocs;
            }
            else
            {
                templ.DocTemplatesForUser = null;
            }
            return templ;
        }
    }
}