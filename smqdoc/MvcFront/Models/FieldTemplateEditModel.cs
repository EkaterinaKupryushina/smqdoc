using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

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
        [Display(Name = "�������� ����")]
        public string FieldTemplateName { get; set; }
        [Required]
        [Display(Name = "��� ����")]
        public int FieldType { get; set; }
        [Display(Name = "���� ����������?")]
        public bool IsRestricted { get; set; }
        [Display(Name = "���� �����?")]
        public bool IsInteger { get; set; }
        [Display(Name = "������������ ��������")]
        [DataType("Number")]
        public double? MaxVal { get; set; }
        [DataType("Number")]
        [Display(Name = "����������� ��������")]
        public double? MinVal { get; set; }
        [Display(Name = "�������")]
        public string OperationExpression { get; set; }

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
            return templ;
        }
        
        public static FieldTemplateEditModel FieldToModelConverter(FieldTemplate templ)
        {
            return new FieldTemplateEditModel(templ);
        }
    }
}