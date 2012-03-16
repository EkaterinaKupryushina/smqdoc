using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using MvcFront.Helpers;

namespace MvcFront.DB
{
    public enum DocTemplateStatus
    {
        Active,
        Unactive,
        Deleted
    }
    public enum FieldTemplateType
    {
        BOOL,
        NUMBER,
        STRING
    }
    public enum FieldTemplateStatus
    {
        Active,
        Unactive,
        Deleted
    }
    [MetadataType(typeof(DocTemplateMetadata))]
    public partial class DocTemplate
    {
        [Display(Name = "Статус шаблона")]
        public DocTemplateStatus TemplateStatus
        {
            get
            {
                return (DocTemplateStatus)Status;
            }
            set
            {
                Status = (int)value;
            }
        }
        public string TemplateStatusText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(DocTemplateStatus), Status);
            }
        }
    }
    public class DocTemplateMetadata
    {
        [Required]
        [UIHint("Hidden")]
        public long docteplateid { get; set; }
        [Required]
        [Display(Name = "Название шаблона")]
        public string TemplateName { get; set; }
        [Required]
        [Display(Name = "Описание шаблона")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        [Required]
        [Display(Name = "Дата последнего редактирования")]
        public DateTime LastEditDate { get; set; }
        [Required]
        [Display(Name = "Код статуса")]
        public int Status { get; set; }
        [Display(Name = "Статус шаблона")]
        public DocTemplateStatus TemplateStatus { get; set; }
        [Display(Name = "Список полей шаблона")]
        public EntityCollection<FieldTemplate>  FieldTeplates { get; set; }
    }

    [MetadataType(typeof(FieldTemplateMetadata))]
    public partial class FieldTemplate
    {
        [Display(Name = "Статус поля шаблона")]
        public FieldTemplateStatus TemplateStatus
        {
            get
            {
                return (FieldTemplateStatus)Status;
            }
            set
            {
                Status = (int)value;
            }
        }
        [Display(Name = "Статус поля шаблона")]
        public string TemplateStatusText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(FieldTemplateStatus), Status);
            }
        }
        [Display(Name = "Тип поля шаблона")]
        public FieldTemplateType TemplateType
        {
            get
            {
                return (FieldTemplateType)FiledType;
            }
            set
            {
                FiledType = (int)value;
            }
        }
        [Display(Name = "Тип поля шаблона")]
        public string TemplateTypeText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(FieldTemplateType), Status);
            }
        }
    }
    public class FieldTemplateMetadata
    {
        [Required]
        [UIHint("Hidden")]
        public long fieldteplateid { get; set; }
        [Required]
        [Display(Name = "Название поля")]
        public string FieldName { get; set; }
        [Required]
        [Display(Name = "Тип поля")]
        public int FiledType { get; set; }
        [Required]
        [Display(Name = "Огрничено поле?")]
        public bool Restricted { get; set; }
        [Display(Name = "Максимальное значение поля")]
        public int MaxVal { get; set; }
        [Display(Name = "Минимальное значение поля")]
        public int MinVal { get; set; }
        [Required]
        [Display(Name = "Порядковый номер в шаблоне")]
        public int OrderNumber { get; set; }
        [Required]
        [Display(Name = "Статус поля")]
        public int Status { get; set; }
        [Display(Name = "Родительский шаблон")]
        public DocTemplate DocTemplate { get; set; }
    }
}