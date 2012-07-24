using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using MvcFront.Enums;
using MvcFront.Helpers;

    namespace MvcFront.DB
    {
    [MetadataType(typeof(DocTemplateMetadata))]
    public partial class DocTemplate
    {
        public Boolean IsEditable 
        {
            get 
            {
                if (docteplateid > 0)
                {

                    return !DocAppointments.Any( da => da.Documents.Any(doc => doc.Status != (int) DocumentStatus.Deleted));
                }
                return true;
            }
        }

        [Display(Name = "Статус Формы")]
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
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
        [Required]
        [UIHint("Hidden")]
        public long docteplateid { get; set; }
        [Required]
        [Display(Name = "Название Формы")]
        public string TemplateName { get; set; }
        [Required]
        [Display(Name = "Описание Формы")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        [Required]
        [Display(Name = "Дата последнего редактирования")]
        public DateTime LastEditDate { get; set; }
        [Required]
        [Display(Name = "Код статуса")]
        public int Status { get; set; }
        [Display(Name = "Статус Формы")]
        public DocTemplateStatus TemplateStatus { get; set; }
        [Display(Name = "Список полей Формы")]
        public EntityCollection<FieldTemplate>  FieldTeplates { get; set; }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }

    [MetadataType(typeof(FieldTemplateMetadata))]
    public partial class FieldTemplate
    {
        [Display(Name = "Тип поля Формы")]
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
        [Display(Name = "Тип поля Формы")]
        public string TemplateTypeText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(FieldTemplateType),FiledType);
            }
        }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
    public class FieldTemplateMetadata
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
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
        [Display(Name = "Ограничено?")]
        public bool Restricted { get; set; }
        [Display(Name = "Максимальное значение поля")]
        public int MaxVal { get; set; }
        [Display(Name = "Минимальное значение поля")]
        public int MinVal { get; set; }
        [Required]
        [Display(Name = "Порядковый номер в форме")]
        public int OrderNumber { get; set; }
        [Required]
        [Display(Name = "Статус поля")]
        public int Status { get; set; }
        [Display(Name = "Родительская Форма")]
        public DocTemplate DocTemplate { get; set; }
        [Display(Name = "Операция для вычисления значения")]
        public int OperationType { get; set; }
        [Required]
        [Display(Name = "Целое?")]
        public bool Integer { get; set; }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
}