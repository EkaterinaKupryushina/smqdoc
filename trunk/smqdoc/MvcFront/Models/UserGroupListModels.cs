using System;
using MvcFront.DB;
using System.ComponentModel.DataAnnotations;

namespace MvcFront.Models
{
    public class UserGroupListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 GroupId { get; set; }
        [Display(Name = "Менеджер")]
        public string Manager { get; set; }
        [Display(Name = "Имя группы")]
        public string GroupName { get; set; }

        public UserGroupListViewModel()
        {
        }
        public UserGroupListViewModel(UserGroup grr)
        {
            GroupId = grr.usergroupid;
            Manager = string.Format("{1} {0} {2} ({3})", grr.Manager.FirstName, grr.Manager.SecondName, grr.Manager.LastName, grr.Manager.Login);
            GroupName = grr.GroupName;
        }
        public static UserGroupListViewModel UserGroupToModelConverter(UserGroup templ)
        {
            return new UserGroupListViewModel(templ);
        }
    }
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
            if(grr.usergroupid != 0)
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
    }
}