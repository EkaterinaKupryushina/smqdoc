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
                return SecondName + " " + FirstName + " " + LastName;
            }
        }
        public UserAccountStatus UserStatus
        {
            get
            {
                return (UserAccountStatus)Status;
            }
            set
            {
                Status = (int)value;
            }
        }
        public string UserStatusText
        {
            get
            {
                return DictionaryHelper.GetEnumText(typeof(UserAccountStatus),Status);
            }
        }
    }
}