using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class EditUserAccountForUserModel
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Отчество")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public EditUserAccountForUserModel() { }

        public EditUserAccountForUserModel(UserAccount user)
        {
            if (user != null)
            {
                LastName = user.LastName;
                FirstName = user.FirstName;
                SecondName = user.SecondName;
                Email = user.Email;
            }
        }
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore UnusedAutoPropertyAccessor.Global
    }
}