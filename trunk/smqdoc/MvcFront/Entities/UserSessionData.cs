using System;
using MvcFront.Enums;

namespace MvcFront.Entities
{
    /// <summary>
    /// Хранит информацию о текущем профиле пользователя
    /// </summary>
    [Serializable]
    public class UserSessionData 
    {   
        public string UserName { get; set; }
        public int UserId { get; set; }
        public UserProfileTypes UserType { get; set; }
        public string UserGroupName { get; set; }
        public int UserGroupId { get; set; }
        public string CurrentProfileName { get; set; }
    }
}