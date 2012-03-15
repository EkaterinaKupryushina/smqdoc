using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;

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
        INTEGER,
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
                return (DocTemplateStatus)this.Status;
            }
            set
            {
                this.Status = (int)value;
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
        public EntityCollection<FieldTeplate>  FieldTeplates { get; set; }
    }

    [MetadataType(typeof(FieldTeplateMetadata))]
    public partial class FieldTeplate
    {
        [Display(Name = "Статус поля шаблона")]
        public FieldTemplateStatus TemplateStatus
        {
            get
            {
                return (FieldTemplateStatus)this.Status;
            }
            set
            {
                this.Status = (int)value;
            }
        }
        [Display(Name = "Тип поля шаблона")]
        public FieldTemplateType TemplateType
        {
            get
            {
                return (FieldTemplateType)this.FiledType;
            }
            set
            {
                this.FiledType = (int)value;
            }
        }
    }
    public class FieldTeplateMetadata
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