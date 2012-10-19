using System;
using MvcFront.Enums;
using MvcFront.Infrastructure;

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
                    return DoubleValue == null ? "" : string.Format(SmqSettings.Instance.DoubleFormatStr, DoubleValue);
                case FieldTemplateType.String:
                    return StringValue ?? "";
                case FieldTemplateType.Calculated:
                    return DoubleValue == null ? "" : string.Format(SmqSettings.Instance.DoubleFormatStr, DoubleValue);
                case FieldTemplateType.Planned:
                    switch (FieldTemplate.FactFieldTemplate.TemplateType)
                    {
                        case FieldTemplateType.Bool:
                            return BoolValue == null ? "" : BoolValue.Value ? "Да" : "Нет";
                        case FieldTemplateType.Number:
                            return DoubleValue == null ? "" : string.Format(SmqSettings.Instance.DoubleFormatStr, DoubleValue);
                        case FieldTemplateType.String:
                            return StringValue ?? "";
                        case FieldTemplateType.Calculated:
                            return DoubleValue == null ? "" : string.Format(SmqSettings.Instance.DoubleFormatStr, DoubleValue);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}