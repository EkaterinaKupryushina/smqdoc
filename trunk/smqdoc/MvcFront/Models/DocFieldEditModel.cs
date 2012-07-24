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
    public class DocFieldEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long FieldId { get; set; }
        
        [Display(Name = "�������� ����")]
        public string FieldName { get; set; }
        
        [Display(Name = "���������� �����")]
        public int OrderNumber { get; set; }
        
        [Display(Name = "��� ����")]
        public int FieldType { get; set; }
        
        [Display(Name = "��������")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string StringValue { get; set; }
        
        [Display(Name = "��������")]
        [DataType("Number")]
        [Required]
        public double? DoubleValue { get; set; }
        
        [Display(Name = "��������")]
        [DataType("Number")]
        [Required]
        public int? IntegerValue { get; set; }
        
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
        [Required]
        public bool BoolValue { get; set; }


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
            FieldType = item.FieldTemplate.FiledType;
            IsRestricted = item.FieldTemplate.Restricted.HasValue && item.FieldTemplate.Restricted.Value;
            MaxVal = item.FieldTemplate.MaxVal;
            MinVal = item.FieldTemplate.MinVal;
            StringValue = item.StringValue;
            BoolValue = item.BoolValue ?? false;
            DoubleValue = item.DoubleValue;
            OrderNumber = item.FieldTemplate.OrderNumber;
            IsInteger = item.FieldTemplate.Integer.HasValue && item.FieldTemplate.Integer.Value;
            if(IsInteger && item.DoubleValue.HasValue)
            {
                IntegerValue = (int) item.DoubleValue.Value;
            }
        }
        public DocField Update(DocField item, Document doc = null)
        {
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
                        DoubleValue = CalculateValue(doc, item);

                    item.BoolValue = null;
                    item.StringValue = null;
                    item.DoubleValue = DoubleValue;
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
    }
}