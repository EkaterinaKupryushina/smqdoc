using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.Enums;
using MvcFront.Helpers;


namespace MvcFront.DB
{
    [MetadataType(typeof(GroupTemplateMetadata))]
    public partial class GroupTemplate
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
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
        [Display(Name = "Статус Связи")]
        public string GroupTemplateStatusText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(GroupTemplateStatus), Status);
            }
        }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
    public class GroupTemplateMetadata
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
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
        [Display(Name = "Родительский шаблон")]
        public DocTemplate DocTemplate { get; set; }
        [Display(Name = "Родительская группа")]
        public UserGroup UserGroup { get; set; }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }

}