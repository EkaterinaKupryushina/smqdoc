using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class GroupTemplateListModels
    {
    }

    public class GroupTemplateListViewModel
    {
        [Required]
        [UIHint("Hidden")]
        public long ID { get; set; }
        [Required]
        [Display(Name = "Наименование назначения ")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Начала заполенния")]
        public DateTime DateStart { get; set; }
        [Required]
        [Display(Name = "Окончание заполенния")]
        public DateTime DateEnd { get; set; }
        [Required]
        [Display(Name = "Статус Связи")]
        public String Status { get; set; }       
        [Display(Name = "Родительский шаблон")]
        public String DocTemplateName { get; set; }
        [Display(Name = "Родительская группа")]
        public String UserGroupName { get; set; }        

        public GroupTemplateListViewModel()
        {
        }
        public GroupTemplateListViewModel(GroupTemplate tpl)
        {
            ID = tpl.grouptemplateid;
            Name = tpl.Name;
            DateStart = tpl.DateStart;
            DateEnd = tpl.DateEnd;
            Status = tpl.GroupTemplateStatusText;
            DocTemplateName = tpl.DocTemplate.TemplateName;
            UserGroupName = tpl.UserGroup.GroupName;                        
        }
        public static GroupTemplateListViewModel GroupTemplateToModelConverter(GroupTemplate tpl)
        {
            return new GroupTemplateListViewModel(tpl);
        }
    }

    /*
    public class UserGroupEditViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 GroupId { get; set; }
        [Required]
        [Display(Name = "Имя группы")]
        public string GroupName { get; set; }
        [Required]
        [Display(Name = "Полное имя группы")]
        public string FullGroupName { get; set; }
        //[Required]
        //[Display(Name = "Менеджер")]
        //[UIHint("UserAccountFilter")]
        //public int? Managerid { get; set; }
        [Required]
        [Display(Name = "Менеджер")]
        [UIHint("UserAccountFilter")]
        public UserAccountListViewModel Manager { get; set; }

        public UserGroupEditViewModel()
        {
        }
        public UserGroupEditViewModel(UserGroup grr)
        {
            GroupId = grr.usergroupid;
            GroupName = grr.GroupName;
            FullGroupName = grr.FullGroupName;
            if (grr.usergroupid != 0)
                Manager = new UserAccountListViewModel(grr.Manager);
        }
        public UserGroup Update(UserGroup grr)
        {
            grr.usergroupid = GroupId;
            grr.GroupName = GroupName;
            grr.FullGroupName = FullGroupName;
            grr.Managerid = Manager.UserId;
            return grr;
        }
    }*/
}