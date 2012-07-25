using System;
using MvcFront.Enums;

namespace MvcFront.DB
{
    public partial class DocField
    {
        public string GetValueString()
        {
            switch(FieldTemplate.TemplateType)
            {
                case FieldTemplateType.Bool:
                    return BoolValue == null ? "" : BoolValue.Value ? "Да" : "Нет";
                case FieldTemplateType.Number:
                    return DoubleValue == null ? "" : string.Format("{0}",DoubleValue);
                case FieldTemplateType.String:
                    return StringValue ?? "";
                case FieldTemplateType.Calculated:
                    return DoubleValue == null ? "" : string.Format("{0}", DoubleValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}