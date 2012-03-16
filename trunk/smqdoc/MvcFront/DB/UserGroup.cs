
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MvcFront.Helpers;

namespace MvcFront.DB
{
    public enum UserGroupStatus
    {
        Active,
        Unactive,
        Deleted
    }
    [MetadataType(typeof(UserGroupMetadata))]
    public partial class UserGroup
    {
        [Display(Name="Статус")]
        public UserGroupStatus GroupStatus
        {
            get
            {
                return (UserGroupStatus)this.Status;
            }
            set
            {
                this.Status = (int)value;
            }
        }
        public string GroupStatusText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(UserGroupStatus), this.Status);
            }
        }
    }

    public class UserGroupMetadata
    {
        [Display(Name="ID группы")]
        [Required]
        public int usergroupid { get; set; }

        [Display(Name="Краткое имя группы")]
        [Required]
        public string GroupName { get; set; }

        [Display(Name = "Полное имя группы")]
        [Required]
        public string FullGroupName { get; set; }

        [Display(Name = "Менеджер группы")]
        public UserAccount Manager { get; set; }

        [Display(Name = "Менеджер кгруппы")]
        [UIHint("UserAccountFilter")]
        [Required]
        public int Managerid { get; set; }
    }
}