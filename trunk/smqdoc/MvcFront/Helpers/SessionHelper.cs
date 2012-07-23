using System.Web;
using System.Xml.Serialization;
using System.IO;
using MvcFront.Entities;

namespace MvcFront.Helpers
{
    public static class SessionHelper
    {
        private const string Smqsessiondatastore = "SMQSESSIONDATASTOPE";

        /// <summary>
        /// Сохраняем данные о пользователе в сессию
        /// </summary>
        /// <param name="ses"></param>
        /// <param name="data"></param>
        public static void SetUserSessionData(HttpSessionStateBase ses, UserSessionData data)
        {
            var xSer = new XmlSerializer(typeof(UserSessionData));
            using (var stream =  new MemoryStream())
            {
                xSer.Serialize(stream, data);
                ses[Smqsessiondatastore] = stream.ToArray();
            }
        }

        /// <summary>
        /// Читаем пользовательские данные из сессии
        /// </summary>
        /// <param name="ses"></param>
        /// <returns></returns>
        public static UserSessionData GetUserSessionData(HttpSessionStateBase ses)
        {
            UserSessionData retVal = null;
            var xSer = new XmlSerializer(typeof(UserSessionData));
            using (var ms = new MemoryStream())
            {
                var data = (byte[])ses[Smqsessiondatastore]; 
                if (data != null)
                {
                    ms.Write(data,0,data.Length);
                    ms.Position = 0;
                    ms.Seek(0, SeekOrigin.Begin);
                    retVal = (UserSessionData)xSer.Deserialize(ms);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Чистит данные в сессии
        /// </summary>
        /// <param name="ses"></param>
        public static void ClearUserSessionData(HttpSessionStateBase ses)
        {
            ses[Smqsessiondatastore] = null;
        }

        /// <summary>
        /// Саздает код пользовательского профиля (нужно для хранения в сессии и для входа в прошлый пльзовательский профиль )
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="isManager"></param>
        /// <returns></returns>
        public static string GenerateUserProfileCode(int? groupId,bool isManager)
        {
            if (groupId == null)
                if (isManager)
                    return "0";
                else
                    return "1";
            return groupId + ";" + (isManager ? "0" : "1");
        }

        /// <summary>
        /// Парсит код пользовательского профиля
        /// </summary>
        /// <param name="code">Код профиля</param>
        /// <param name="groupId">ID группы</param>
        /// <param name="isManager">показывает менеджер ли ?</param>
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
                isManager = true;
                return;
            }
            //Просто пользователь
            if (code == "1")
            {
                return;
            }

            var ids = code.Split(';');
            int parsedGId;
            if(int.TryParse(ids[0],out parsedGId))
                groupId = parsedGId;
            isManager = ids[1] == "0";
        }

    }
}