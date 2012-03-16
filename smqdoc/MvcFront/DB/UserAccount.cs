using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront.Helpers;

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
                return DictionaryHelper.GetEnumText(typeof(UserAccountStatus),this.Status);
            }
        }
    }
}