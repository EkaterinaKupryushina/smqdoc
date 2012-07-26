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
        [Display(Name = "Статус")]
        public string Status { get; set; }

        public UserGroupListViewModel()
        {
        }
        
        public UserGroupListViewModel(UserGroup grr)
        {
            GroupId = grr.usergroupid;
            Manager = string.Format("{1} {0} {2} ({3})", grr.Manager.FirstName, grr.Manager.SecondName, grr.Manager.LastName, grr.Manager.Login);
            GroupName = grr.GroupName;
            Status = grr.GroupStatusText;
        }
    }
}