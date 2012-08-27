using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class EditUserAccountForUserModel
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        
        [Required]
        [Display(Name = "�������")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "���")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "��������")]
        public string SecondName { get; set; }

        public EditUserAccountForUserModel() { }

        public EditUserAccountForUserModel(UserAccount user)
        {
            if (user != null)
            {
                LastName = user.LastName;
                FirstName = user.FirstName;
                SecondName = user.SecondName;
            }
        }
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}