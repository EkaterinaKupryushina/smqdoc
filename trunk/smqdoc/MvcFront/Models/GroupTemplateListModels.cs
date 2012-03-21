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


    public class GroupTemplateEditViewModel
    {
        [Required]
        [UIHint("Hidden")]
        public long ID { get; set; }
        [Required]
        [Display(Name = "Наименование назначения ")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Начала заполнения")]
        public DateTime DateStart { get; set; }
        [Required]
        [Display(Name = "Окончание заполнения")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Родительский шаблон")]
        [UIHint("DocTemplateComboBox")]
        public DocTemplateListViewModel DocTemplate { get; set; }
        [Display(Name = "Родительская группа")]
        [UIHint("UserGroupComboBox")]
        public UserGroupListViewModel UserGroup { get; set; }

        
        //public List<DocTemplateListViewModel> DocTemplateLst { get; set; }
        
        //public List<UserGroupListViewModel> UserGroupLst { get; set; }

        public GroupTemplateEditViewModel()
        {
        }

        public GroupTemplateEditViewModel(GroupTemplate tpl)
        {
            ID = tpl.grouptemplateid;
            Name = tpl.Name;
            DateStart = tpl.DateStart;
            DateEnd = tpl.DateEnd;
            if (tpl.grouptemplateid != 0)
            {
                UserGroup = new UserGroupListViewModel(tpl.UserGroup);
                DocTemplate = new DocTemplateListViewModel(tpl.DocTemplate);
            }
            //if (tpl.DocTemplate_docteplateid != 0)
            //    DocTemplate = new DocTemplateListViewModel(tpl.DocTemplate);
            //if (tpl.UserGroup_usergroupid != 0)
            //    UserGroup = new UserGroupListViewModel(tpl.UserGroup);
        }

        public GroupTemplate Update(GroupTemplate tpl)
        {
            tpl.grouptemplateid = ID;
            tpl.Name = Name;
            tpl.DateStart = DateStart;
            tpl.DateEnd = DateEnd;
            tpl.UserGroup_usergroupid = UserGroup.GroupId;
            tpl.DocTemplate_docteplateid = DocTemplate.DocTemplateId;

            return tpl;
        }        
    }    
}