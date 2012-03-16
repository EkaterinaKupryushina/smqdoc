using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.DB;
using System.ComponentModel.DataAnnotations;

namespace MvcFront.Models
{
    public class UserAccountListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 UserId { get; set; }
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Display(Name = "Имя")]
        public string FullName { get; set; }
        [Display(Name = "Последний вход")]
        [UIHint("DateTime")]
        public DateTime? LastLogin { get; set; }
        [Display(Name = "CompositeId")]
        [UIHint("Hidden")]
        public string CompositeId { get; set; }

        public UserAccountListViewModel()
        {
        }
        public UserAccountListViewModel(UserAccount acc)
        {
            Login = acc.Login;
            FullName = acc.FullName;
            LastLogin = acc.LastAccess;
        }
        public static UserAccountListViewModel UserAccountToModelConverter(UserAccount templ)
        {
            return new UserAccountListViewModel(templ);
        }
    }
    public class UserAccountEditViewModel
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

        public UserAccountEditViewModel()
        {
        }
        public UserAccountEditViewModel(UserAccount acc)
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