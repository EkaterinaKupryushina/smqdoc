using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class UserTagsEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 UserTagNameId { get; set; }
        
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public UserTagsEditModel()
        {
        }
        
        public UserTagsEditModel(UserTags tag)
        {
            UserTagNameId = tag.Id;
            Name = tag.Name;
        }
        
        public UserTags Update(UserTags tag)
        {
            tag.Name = Name;
            return tag;
        }
    }
}