using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
    }

    public partial class DocumentMetadata
    {
        [Display(Name = "Id документа")]
        [UIHint("Hidden")]
        public Int64 documentid { get; set; }
        [Display(Name = "Дата создания")]
        [Required]
        private DateTime CreationDate { get; set; }
        [Display(Name = "Дата последнего изменения")]
        [Required]
        private DateTime LastEditDate { get; set; }
        [Display(Name = "Код статуса")]
        [Required]
        private Int32 Status { get; set; }
        [Display(Name = "Послений коментарий")]
        [DataType(DataType.MultilineText)]
        private String LastComment { get; set; }
    }
}