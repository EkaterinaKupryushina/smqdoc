using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class UserGroupEditModel
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
        
        [Required]
        [Display(Name = "Менеджер")]
        [UIHint("UserAccountFilter")]
        public UserAccountListViewModel Manager { get; set; }

        public UserGroupEditModel()
        {
        }
        public UserGroupEditModel(UserGroup grr)
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