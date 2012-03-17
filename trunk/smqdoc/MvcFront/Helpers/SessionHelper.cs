using System;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using MvcFront.DB;

namespace MvcFront.Helpers
{
    /// <summary>
    /// Тип профиля пользователя
    /// </summary>
    public enum SmqUserProfileType
    {
        Anonymous,
        User,
        Groupuser,
        Groupmanager,
        Systemadmin 
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
        public static string GenerateUserProfileCode(int? groupId,bool isManager)
        {
            if (groupId == null)
                if (isManager)
                    return "0";
                else
                    return "1";
            return groupId + ";" + (isManager ? "0" : "1");
        }
        public static void ParseUserProfileCode(string code, out int? groupId, out bool isManager)
        {
            
            groupId = null;
            isManager = false;
            if (code == null)
            {
                return;
            }
            //Админ
            if (code == "0")
            {
                groupId = null;
                isManager = true;
                return;
            }
            //Просто пользователь
            if (code == "1")
            {
                groupId = null;
                isManager = false;
                return;
            }
            var ids = code.Split(';');
            var parsedGId = 0;
            if(int.TryParse(ids[0],out parsedGId))
                groupId = parsedGId;
            isManager = ids[1] == "0";
        }

    }
}