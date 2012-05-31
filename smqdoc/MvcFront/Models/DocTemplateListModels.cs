using System;
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
        [Display(Name = "Название шаблона")]
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

    //public class DocTemplateListViewModelConverter : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {

    //        if (sourceType == typeof(string))
    //        {
    //            return true;
    //        }
    //        return base.CanConvertFrom(context, sourceType);
    //    }
    //    // Overrides the ConvertFrom method of TypeConverter.
    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        if (value is string)
    //        {
    //            return new DocTemplateListViewModel { DocTemplateId = Convert.ToInt32(value) };
    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }
    //    // Overrides the ConvertTo method of TypeConverter.
    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (destinationType == typeof(string))
    //        {
    //            return ((DocTemplateListViewModel)value).DocTemplateId;
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }
    //}

    public class DocTemplateListEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public long DocTemplateId { get; set; }
        [Display(Name = "Название шаблона")]
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
    public class FieldTemplateListEditModel
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
        [Display(Name = "Максимальное значение")]
        [DataType("Number")]
        public double? MaxVal { get; set; }
        [DataType("Number")]
        [Display(Name = "Минимальное значение")]
        public double? MinVal { get; set; }
        [Display(Name = "Формула")]
        public string OperationExpression { get; set; }

        public FieldTemplateListEditModel()
        {
        }
        public FieldTemplateListEditModel(FieldTemplate templ)
        {
            FieldTemplateId = templ.fieldteplateid;
            DocTemplateId = templ.DocTemplate_docteplateid;
            FieldTemplateName = templ.FieldName;
            FieldType = templ.FiledType;

            IsRestricted = templ.Restricted.HasValue && templ.Restricted.Value ;
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
            templ.MaxVal = MaxVal;
            templ.MinVal = MinVal;

            templ.OperationExpression = OperationExpression;
            return templ;
        }
        public static FieldTemplateListEditModel FieldToModelConverter(FieldTemplate templ)
        {
            return new FieldTemplateListEditModel(templ);
        }
    }

}