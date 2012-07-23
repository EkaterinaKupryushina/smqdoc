using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    /// <summary>
    /// Редактирование всех пользовательских данных (для админа)
    /// </summary>
    public class UserAccountEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 UserId { get; set; }
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }
        [Required]
        [Display(Name = "Отчество")]
        public string LastName { get; set; }
        [Display(Name = "Администратор?")]
        public bool IsAdmin { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        public UserAccountEditModel()
        {
        }

        public UserAccountEditModel(UserAccount acc)
        {
            UserId = acc.userid;
            Login = acc.Login;
            FirstName = acc.FirstName;
            SecondName = acc.SecondName;
            LastName = acc.LastName;
            IsAdmin = acc.IsAdmin;
            Email = acc.Email;
            Password = acc.Password;            
        }

        public UserAccount Update(UserAccount acc)
        {
            acc.Login = Login;
            acc.FirstName = FirstName;
            acc.SecondName = SecondName;
            acc.LastName = LastName;
            acc.IsAdmin = IsAdmin;
            acc.Email = Email;
            acc.Password = Password;
            acc.userid = UserId;            
            return acc;
        }
    }
}