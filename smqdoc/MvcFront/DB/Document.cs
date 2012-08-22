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
            get
            {
                var text = DictionaryHelper.GetEnumText(typeof (DocumentStatus), Status);
                if (IsReadOnly && DocStatus == DocumentStatus.FactEditing)
                {
                    text = "Ожидает начала периода редактирования " + DocAppointment.ActualStartDate;
                }
                return text;
            }
        }
        public bool IsReadOnly
        {
            get { return (DocStatus != DocumentStatus.PlanEditing && DocStatus != DocumentStatus.FactEditing) || (DocStatus == DocumentStatus.FactEditing && DocAppointment.ActualStartDate > DateTime.Now); }
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
        [Display(Name = "Приложение")]
        public string DisplayFileName { get; set; }
        [Display(Name = "Последний комментарий")]
        [DataType(DataType.MultilineText)]
        public String LastComment { get; set; }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
}