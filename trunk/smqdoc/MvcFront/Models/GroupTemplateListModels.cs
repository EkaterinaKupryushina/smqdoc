using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{

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
        [UIHint("Hidden")]
        public bool IsRed { get; set; }
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
            //IsRed = tpl.DateEnd + new TimeSpan(2, 0, 0, 0) > DateTime.Now &&
            //        tpl.Status == (int)DocumentStatus.Editing;        
            IsRed = tpl.DateEnd < DateTime.Now.AddDays(2) &&
                    tpl.Status == (int)GroupTemplateStatus.Active;
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
        [Display(Name = "Наименование назначения")]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Начала заполнения")]
        [DataType(DataType.DateTime)]
        public DateTime DateStart { get; set; }
        [Required]
        [Display(Name = "Окончание заполнения")]
        [DataType(DataType.DateTime)]
        public DateTime DateEnd { get; set; }

        [Required]
        [Display(Name = "Родительский шаблон")]
        [UIHint("DocTemplateComboBox")]
        //public DocTemplateListViewModel DocTemplate { get; set; }
        public long DocTemplateID { get; set; }
        [Required]
        [Display(Name = "Родительская группа")]
        [UIHint("UserGroupComboBox")]
        //public UserGroupListViewModel UserGroup { get; set; }
        public int UserGroupID { get; set; }
                

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
                //UserGroup = new UserGroupListViewModel(tpl.UserGroup);
                //DocTemplate = new DocTemplateListViewModel(tpl.DocTemplate);

                UserGroupID = tpl.UserGroup_usergroupid;
                DocTemplateID = tpl.DocTemplate_docteplateid;
            }
            
        }

        public GroupTemplate Update(GroupTemplate tpl)
        {
            tpl.grouptemplateid = ID;
            tpl.Name = Name;
            tpl.DateStart = DateStart;
            tpl.DateEnd = DateEnd;
            tpl.UserGroup_usergroupid = UserGroupID;
            tpl.DocTemplate_docteplateid = DocTemplateID;

            //tpl.UserGroup_usergroupid = UserGroup.GroupId;
            //tpl.DocTemplate_docteplateid = DocTemplate.DocTemplateId;

            return tpl;
        }        
    }    
}