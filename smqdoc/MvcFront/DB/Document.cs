using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.Enums;
using MvcFront.Helpers;

namespace MvcFront.DB
{
    [MetadataType(typeof(DocumentMetadata))]
    public partial class Document
    {
        [Display(Name="Статус")]
        public DocumentStatus DocStatus
        {
            get { return (DocumentStatus) Status; }
            set { Status = (int)value; }
        }
        [Display(Name = "Статус")]
        public string DocStatusText
        {
            get { return DictionaryHelper.GetEnumText(typeof(DocumentStatus), Status); }
        }
    }

    public class DocumentMetadata
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
        [Display(Name = "Id документа")]
        [UIHint("Hidden")]
        public Int64 documentid { get; set; }
        [Display(Name = "Дата создания")]
        [Required]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Последнее изменение")]
        [Required]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "Код статуса")]
        [Required]
        public Int32 Status { get; set; }
        [Display(Name = "Последний коментарий")]
        [DataType(DataType.MultilineText)]
        public String LastComment { get; set; }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
}