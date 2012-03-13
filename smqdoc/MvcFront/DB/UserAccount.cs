using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcFront.DB
{
    public enum UserAccountStatus
    {
        Active,
        Unactive,
        Deleted
    }
    public partial class UserAccount
    {
        public string FullName
        {
            get
            {
                return this.SecondName + " " + this.FirstName + " " + this.LastName;
            }
        }
        public UserAccountStatus UserStatus
        {
            get
            {
                return (UserAccountStatus)this.Status;
            }
            set
            {
                this.Status = (int)value;
            }
        }
        public string UserStatusText
        {
            get
            {
                switch (this.Status)
                {
                    case 0:
                        {
                            return "Активный";
                        }
                    case 1:
                        {
                            return "Отключен";
                        }
                    case 2:
                        {
                            return "Удален";
                        }
                    default:
                        {
                            return "ХЗ";
                        }
                }
            }
        }
    }
}