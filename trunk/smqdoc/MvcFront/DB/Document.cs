using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcFront.Helpers;

namespace MvcFront.DB
{
    public enum DocumentStatus
    {
        Editing,
        Sended,
        Submited,
        Deleted
    }

    [MetadataType(typeof(DocumentMetadata))]
    public partial class Document
    {
        [Display(Name="Статус документа")]
        public DocumentStatus DocStatus
        {
            get { return (DocumentStatus) Status; }
            set { Status = (int)value; }
        }
        [Display(Name = "Статус документа")]
        public string DocStatusText
        {
            get { return DictionaryHelper.GetEnumText(typeof(DocumentStatus), Status); }
        }
    }

    public partial class DocumentMetadata
    {
        [Display(Name = "Id документа")]
        [UIHint("Hidden")]
        public Int64 documentid { get; set; }
        [Display(Name = "Дата создания")]
        [Required]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Дата последнего изменения")]
        [Required]
        public DateTime LastEditDate { get; set; }
        [Display(Name = "Код статуса")]
        [Required]
        public Int32 Status { get; set; }
        [Display(Name = "Послений коментарий")]
        [DataType(DataType.MultilineText)]
        public String LastComment { get; set; }
        //[Display(Name = "Название документа")]
        //public String DocumentName { get; set; }
    }

    public partial class DocField
    {
        public string GetValueString()
        {
            switch(FieldTemplate.TemplateType)
            {
                case FieldTemplateType.BOOL:
                    return BoolValue == null ? "" : BoolValue.Value ? "Да" : "Нет";
                    break;
                case FieldTemplateType.NUMBER:
                    return DoubleValue == null ? "" : string.Format("{0}",DoubleValue);
                    break;
                case FieldTemplateType.STRING:
                    return StringValue ?? "";
                    break;
                case FieldTemplateType.CALCULATED:
                    return DoubleValue == null ? "" : string.Format("{0}", DoubleValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}