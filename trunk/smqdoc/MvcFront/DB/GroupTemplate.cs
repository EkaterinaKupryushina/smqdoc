using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using MvcFront.Helpers;

namespace MvcFront.DB
{
    public enum GroupTemplateStatus
    {
        Active,
        Unactive,
        Deleted
    }

    [MetadataType(typeof(GroupTemplateMetadata))]
    public partial class GroupTemplate
    {
        [Display(Name = "Статус Связи")]
        public GroupTemplateStatus GroupTemplateStatus
        {
            get
            {
                return (GroupTemplateStatus)Status;
            }
            set
            {
                Status = (int)value;
            }
        }
        public string GroupTemplateStatusText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(GroupTemplateStatus), Status);
            }
        }
    }
    public class GroupTemplateMetadata
    {
        [Required]
        [UIHint("Hidden")]
        public long grouptemplateid { get; set; }
        [Required]
        [Display(Name = "Наименование назначения шаблона документа для группы")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Дата начала периода заполенния документов")]
        public DateTime DateStart { get; set; }
        [Required]
        [Display(Name = "Дата окончания периода заполенния документов")]
        public DateTime DateEnd { get; set; }
        [Required]
        [Display(Name = "Код статуса")]
        public int Status { get; set; }
        [Display(Name = "Статус Связи")]
        public GroupTemplateStatus TemplateStatus { get; set; }
        [Display(Name = "Родительский шаблон")]
        public DocTemplate DocTemplate { get; set; }
        [Display(Name = "Родительская группа")]
        public UserGroup UserGroup { get; set; }        
    }

}