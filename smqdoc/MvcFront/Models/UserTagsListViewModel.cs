using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class UserTagsListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 UserTagNameId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }        
        [Display(Name = "Статус")]
        [UIHint("Hidden")]
        public Int32 Status { get; set; }
        
        public UserTagsListViewModel()
        {
        }

        public UserTagsListViewModel(UserTags tag)
        {
            if (tag != null)
            {
                UserTagNameId = tag.Id;
                Name = tag.Name;
                Status = tag.Status;                
            }
        }

        public static UserTagsListViewModel UserTagNamesToModelConverter(UserTags templ)
        {
            return new UserTagsListViewModel(templ);
        }
    }
}