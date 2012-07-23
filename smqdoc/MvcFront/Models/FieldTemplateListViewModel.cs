using System.ComponentModel.DataAnnotations;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;

namespace MvcFront.Models
{
    public class FieldTemplateListViewModel
    {
        [Display(Name = "ID")]
        public long FieldTemplateId { get; set; }
        [Display(Name = "№")]
        public long OrderNumber { get; set; }
        [Display(Name = "Название шаблона")]
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