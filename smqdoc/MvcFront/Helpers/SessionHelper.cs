using System;
using System.Web;
using System.Xml.Serialization;
using System.IO;

namespace MvcFront.Helpers
{
    /// <summary>
    /// Тип профиля пользователя
    /// </summary>
    public enum SmqUserProfileType
    {
        SYSTEMADMIN,//0
        GROUPMANAGER,//1
        GROUPUSER,//2
        USER//3
    }
    /// <summary>
    /// Хранит информацию о текущем профиле пользователя
    /// </summary>
    [Serializable]
    public class SmqUserSessionData
    {   
        public string UserName { get; set; }
        public int UserId { get; set; }
        public SmqUserProfileType UserType { get; set; }
        public string UserGroupName { get; set; }
        public int UserGroupId { get; set; }
        public string CurrentProfileName { get; set; }
    }

    public static class SessionHelper
    {
        private const string Smqsessiondatastore = "SMQSESSIONDATASTOPE";

        public static void SetUserSessionData(HttpSessionStateBase ses, SmqUserSessionData data)
        {
            var xSer = new XmlSerializer(typeof(SmqUserSessionData));
            using (var stream =  new MemoryStream())
            {
                xSer.Serialize(stream, data);
                ses[Smqsessiondatastore] = stream.ToArray();
            }
        }
        public static SmqUserSessionData GetUserSessionData(HttpSessionStateBase ses)
        {
            SmqUserSessionData retVal = null;
            var xSer = new XmlSerializer(typeof(SmqUserSessionData));
            using (var ms = new MemoryStream())
            {
                var data = (byte[])ses[Smqsessiondatastore]; 
                if (data != null)
                {
                    ms.Write(data,0,data.Length);
                    ms.Position = 0;
                    ms.Seek(0, SeekOrigin.Begin);
                    retVal = (SmqUserSessionData)xSer.Deserialize(ms);
                }
            }
            return retVal;
        }
        public static void ClearUserSessionData(HttpSessionStateBase ses)
        {
            ses[Smqsessiondatastore] = null;
        }
    }
}