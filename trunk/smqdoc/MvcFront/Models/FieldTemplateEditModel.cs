using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Models
{
    public class FieldTemplateEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long FieldTemplateId { get; set; }
        [Display(Name = "DocTemplateId")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Required]
        [Display(Name = "Название поля")]
        public string FieldTemplateName { get; set; }
        [Required]
        [Display(Name = "Тип поля")]
        public int FieldType { get; set; }
        [Display(Name = "Поле ограничено?")]
        public bool IsRestricted { get; set; }
        [Display(Name = "Поле Целое?")]
        public bool IsInteger { get; set; }
        [Display(Name = "Максимальное значение")]
        [DataType("Number")]
        public double? MaxVal { get; set; }
        [DataType("Number")]
        [Display(Name = "Минимальное значение")]
        public double? MinVal { get; set; }
        [Display(Name = "Формула")]
        public string OperationExpression { get; set; }
        [Display(Name = "Фактическое значение")]
        public long? FactFieldTemplateId { get; set; }

        public bool CanBePlaned { get; set; }

        public FieldTemplateEditModel()
        {
        }
        
        public FieldTemplateEditModel(FieldTemplate templ)
        {
            FieldTemplateId = templ.fieldteplateid;
            DocTemplateId = templ.DocTemplate_docteplateid;
            FieldTemplateName = templ.FieldName;
            FieldType = templ.FiledType;

            IsRestricted = templ.Restricted.HasValue && templ.Restricted.Value ;
            IsInteger = templ.Integer.HasValue && templ.Integer.Value;
            MaxVal = templ.MaxVal;
            MinVal = templ.MinVal;
            OperationExpression = templ.OperationExpression;
            FactFieldTemplateId = templ.FactFieldTemplate_fieldteplateid;
            CanBePlaned = templ.DocTemplate.FieldTeplates.Any(x => (x.FiledType != (int)FieldTemplateType.Planned && !x.PlanFieldTemplates.Any() && x.fieldteplateid > 0 )
                || (FactFieldTemplateId.HasValue && x.fieldteplateid == FactFieldTemplateId));
            
        }
        
        public FieldTemplate Update(FieldTemplate templ)
        {
            templ.fieldteplateid = FieldTemplateId;
            templ.DocTemplate_docteplateid = DocTemplateId;
            templ.FieldName = FieldTemplateName;
            templ.FiledType = FieldType;

            templ.Restricted = IsRestricted;
            templ.Integer = IsInteger;
            templ.MaxVal = MaxVal;
            templ.MinVal = MinVal;

            templ.OperationExpression = OperationExpression;
            templ.FactFieldTemplate_fieldteplateid = templ.TemplateType == FieldTemplateType.Planned
                                                         ? FactFieldTemplateId
                                                         : null;

            return templ;
        }
    }
}