using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.Enums;
using MvcFront.Helpers;


namespace MvcFront.DB
{
    [MetadataType(typeof(DocAppointmentMetadata))]
    public partial class DocAppointment
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
        [Display(Name = "Статус Связи")]
        public DocAppointmentStatus DocAppointmentStatus
        {
            get
            {
                return (DocAppointmentStatus)Status;
            }
            set
            {
                Status = (int)value;
            }
        }
        [Display(Name = "Статус Связи")]
        public string DocAppointmentStatusText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(DocAppointmentStatus), Status);
            }
        }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }
    public class DocAppointmentMetadata
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming
        [Required]
        [UIHint("Hidden")]
        public long docappointmentid { get; set; }
        [Required]
        [Display(Name = "Наименование назначения Формы")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Код статуса")]
        public int Status { get; set; }
        [Display(Name = "Родительская форма")]
        public DocTemplate DocTemplate { get; set; }
        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedMember.Global
    }

}