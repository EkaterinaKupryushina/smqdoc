using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ELW.Library.Math.Tools;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Helpers;

namespace MvcFront.Models
{
    public class DocFieldEditModel :IValidatableObject
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long FieldId { get; set; }
        
        [Display(Name = "�������� ����")]
        public string FieldName { get; set; }
        
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

        [Display(Name = "��������")]
        [DataType(DataType.MultilineText)]
        public string StringValue { get; set; }
        
        [Display(Name = "��������")]
        [DataType("Number")]
        public double? DoubleValue { get; set; }
        
        [Display(Name = "��������")]
        [DataType("Number")]
        public int? IntegerValue { get; set; }

        [Display(Name = "��������")]
        public bool BoolValue { get; set; }

        [Display(Name = "IsEditable")]
        [UIHint("Hidden")]
        public bool IsReadOnly { get; set; }

        public DocFieldEditModel()
        {
            StringValue = "";
            BoolValue = false;
            DoubleValue = 0;
        }

        public DocFieldEditModel(DocField item)
        {
            FieldId = item.docfieldid;
            FieldName = item.FieldTemplate.FieldName;
            
            
            if(item.FieldTemplate.TemplateType == FieldTemplateType.Planned)
            {
                var factItem = item.FieldTemplate.FactFieldTemplate;
                IsRestricted = factItem.Restricted.HasValue && factItem.Restricted.Value;
                MaxVal = factItem.MaxVal;
                MinVal = factItem.MinVal;
                IsInteger = factItem.Integer.HasValue && factItem.Integer.Value;
                FieldType = factItem.FiledType;
                IsReadOnly = item.Document.DocStatus != DocumentStatus.PlanEditing;
            }
            else
            {
                IsRestricted = item.FieldTemplate.Restricted.HasValue && item.FieldTemplate.Restricted.Value;
                MaxVal = item.FieldTemplate.MaxVal;
                MinVal = item.FieldTemplate.MinVal;
                IsInteger = item.FieldTemplate.Integer.HasValue && item.FieldTemplate.Integer.Value;
                FieldType = item.FieldTemplate.FiledType;
                IsReadOnly = item.FieldTemplate.PlanFieldTemplates != null
                                        && item.Document.DocStatus != DocumentStatus.FactEditing;
            }

            StringValue = item.StringValue;
            BoolValue = item.BoolValue ?? false;
            DoubleValue = item.DoubleValue;
            if(IsInteger && item.DoubleValue.HasValue)
            {
                IntegerValue = (int) item.DoubleValue.Value;
            }
        }

        public DocField Update(DocField item, Document doc = null)
        {
            switch (item.FieldTemplate.TemplateType)
            {
                case FieldTemplateType.Bool:
                    item.BoolValue = BoolValue;
                    item.StringValue = null;
                    item.DoubleValue = null;
                    break;
                case FieldTemplateType.Number:
                    item.BoolValue = null;
                    item.StringValue = null;
                    if (item.FieldTemplate.Integer.HasValue && item.FieldTemplate.Integer.Value)
                    {
                        item.DoubleValue = IntegerValue;
                    }
                    else
                    {
                        item.DoubleValue = DoubleValue;
                    }
                    break;
                case FieldTemplateType.String:
                    item.BoolValue = null;
                    item.StringValue = StringValue;
                    item.DoubleValue = null;
                    break;
                case FieldTemplateType.Calculated:
                    if (doc != null)
                        DoubleValue = CalculateValue(doc, item);

                    item.BoolValue = null;
                    item.StringValue = null;
                    item.DoubleValue = DoubleValue;
                    break;
                case FieldTemplateType.Planned:
                    switch ((FieldTemplateType)FieldType)
                    {
                        case FieldTemplateType.Bool:
                             item.BoolValue = BoolValue;
                             item.StringValue = null;
                             item.DoubleValue = null;
                            break;
                        case FieldTemplateType.Number:
                             item.BoolValue = null;
                             item.StringValue = null;
                             if (item.FieldTemplate.Integer.HasValue && item.FieldTemplate.Integer.Value)
                             {
                                 item.DoubleValue = IntegerValue;
                             }
                             else
                             {
                                item.DoubleValue = DoubleValue;
                             }
                            break;
                        case FieldTemplateType.String:
                             item.BoolValue = null;
                             item.StringValue = StringValue;
                             item.DoubleValue = null;
                            break;
                        case FieldTemplateType.Calculated:
                             if (doc != null)
                             {
                                 DoubleValue = CalculateValue(doc, item);
                             }
                             item.BoolValue = null;
                             item.StringValue = null;
                             item.DoubleValue = DoubleValue;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return item;
        }

        private Double? CalculateValue(Document doc, DocField field)
        {
            Double? res = 0;
            List<long> lstFieldTemplateIDs = field.FieldTemplate.ComputableFieldTemplateParts
                .Where(x => x.FieldTemplate_fieldteplateid == field.FieldTemplate_fieldteplateid)
                .Select(x => x.fkCalculatedFieldTemplateID).ToList();

            var lstValues = doc.DocFields
                .Where(x => lstFieldTemplateIDs.Contains(x.FieldTemplate.fieldteplateid) && x.DoubleValue.HasValue)
                .Select(x => 
                        x.DoubleValue.HasValue? new VariableValue ( 
                                                    x.DoubleValue.Value, 
                                                    string.Format("{{{0}}}",
                                                                  x.FieldTemplate.fieldteplateid) ) : null).ToList();
            try
            {

                res = CalculationHelper.CalculateExpression(field.FieldTemplate.OperationExpression,lstValues);
            }
            catch{}

            return res;
        }

        public static DocFieldEditModel FieldToModelConverter(DocField templ)
        {
            return new DocFieldEditModel(templ);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsReadOnly)
            {
                yield break;
            }

            switch ((FieldTemplateType) FieldType)
            {
                case FieldTemplateType.Number:
                    if (IsInteger && !IntegerValue.HasValue)
                    {
                        yield return new ValidationResult("���������� ������ �������� (�����)");
                    }
                    if (!IsInteger && !DoubleValue.HasValue)
                    {
                        yield return new ValidationResult("���������� ������ �������� (�����)");
                    }
                    break;
                case FieldTemplateType.String:
                    if (string.IsNullOrWhiteSpace(StringValue))
                    {
                        yield return new ValidationResult("���������� ������ �������� (�����)");
                    }
                    break;
            }
        }
    }
}