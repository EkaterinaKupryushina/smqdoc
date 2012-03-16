using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;
using MvcFront.Helpers;

namespace MvcFront.Models
{
    public class DocTemplateListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Display(Name = "Названире шаблона")]
        public string DocTemplateName { get; set; }
        [Display(Name = "Дата последнего изменения")]
        public DateTime LastEditDate { get; set; }
        public DocTemplateListViewModel()
        {
        }
        public DocTemplateListViewModel(DocTemplate templ)
        {
            DocTemplateId = templ.docteplateid;
            DocTemplateName = templ.TemplateName;
            LastEditDate = templ.LastEditDate;
        }
        public static DocTemplateListViewModel DocTemplateToModelConverter(DocTemplate templ)
        {
            return new DocTemplateListViewModel(templ);
        }
    }

    public class DocTemplateListEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Display(Name = "Названире шаблона")]
        public string DocTemplateName { get; set; }
        [Display(Name = "Описание шаблона")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public DocTemplateListEditModel()
        {
        }
        public DocTemplateListEditModel(DocTemplate templ)
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
    public class FieldTemplateListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long FieldTemplateId { get; set; }
        [Display(Name = "№")]
        public long OrderNumber { get; set; }
        [Display(Name = "Названире шаблона")]
        public string FieldTemplateName { get; set; }
        [Display(Name = "Тип поля")]
        public string FieldTypeText { get; set; }
        [Display(Name = "Статус поля")]
        public string FieldStatusText { get; set; }

        public FieldTemplateListViewModel()
        {
        }
        public FieldTemplateListViewModel(FieldTemplate templ)
        {
            FieldTemplateId = templ.fieldteplateid;
            OrderNumber = templ.OrderNumber;
            FieldTemplateName = templ.FieldName;
            FieldStatusText = DictionaryHelper.GetEnumText(typeof(FieldTemplateStatus),templ.Status);
            FieldTypeText = DictionaryHelper.GetEnumText(typeof(FieldTemplateType),templ.FiledType);
        }
        public static FieldTemplateListViewModel FieldToModelConverter(FieldTemplate templ)
        {
            return new FieldTemplateListViewModel(templ);
        }
    }
    

}