using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Models
{
    public class UserDocAppointmentListViewModel
    {
        [Required]
        [UIHint("Hidden")]
        public long ID { get; set; }

        [Required]
        [Display(Name = "Наименование документа")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public String Status { get; set; }

        [Display(Name = "Форма")]
        public String DocTemplateName { get; set; }

        [Display(Name = "Док-ты")]
        public int DocCount { get; set; }

        public UserDocAppointmentListViewModel()
        {
        }

        public UserDocAppointmentListViewModel(DocAppointment tpl)
        {
            ID = tpl.docappointmentid;
            Name = tpl.Name;
            Status = tpl.DocAppointmentStatusText;
            DocTemplateName = tpl.DocTemplate.TemplateName;
            DocCount = tpl.Documents.Count(x=>x.Status != (int)DocumentStatus.Deleted);
        }

        public static UserDocAppointmentListViewModel DocAppointmentToModelConverter(DocAppointment tpl)
        {
            return new UserDocAppointmentListViewModel(tpl);
        }
    }
}