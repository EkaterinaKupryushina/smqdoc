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
        /// <summary>
        /// Возвращает True если форма может быть отредактирована
        /// </summary>
        public bool IsEditable 
        {
            get 
            {
                if (docteplateid > 0)
                {

                    return !DocAppointments.Any( da => da.Documents.Any());
                }
                return true;
            }
        }

        /// <summary>
        /// Возвращает True если форма содержит поля типа Planned
        /// </summary>
        public bool IsPlanneble
        {
            get { return FieldTeplates.Any(x => x.FiledType == (int)FieldTemplateType.Planned); }
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
    }