using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcFront;
using MvcFront.DB;


namespace MvcFront.Helpers
{
    public static class DictionaryHelper
    {
        public static string GetEnumText(Type typeOfEnum, int item)
        {
            string retVal = "";
            try
            {
                switch (typeOfEnum.Name)
                {
                    case "DocTemplateStatus":
                        {
                            switch (item)
                            {
                                case 0:
                                    {
                                        retVal = "Активный";
                                        break;
                                    }
                                case 1:
                                    {
                                        retVal = "Отключенный";
                                        break;
                                    }
                                case 2:
                                    {
                                        retVal = "Удаленый";
                                        break;
                                    }
                            }
                            break;
                        }
                    case "FieldTemplateStatus":
                        {
                            switch (item)
                            {
                                case 0:
                                    {
                                        retVal = "Активно";
                                        break;
                                    }
                                case 1:
                                    {
                                        retVal = "Отключено";
                                        break;
                                    }
                                case 2:
                                    {
                                        retVal = "Удалено";
                                        break;
                                    }
                            }
                            break;
                        }
                    case "FieldTemplateType":
                        {
                            switch (item)
                            {
                                case 0:
                                    {
                                        retVal = "Да/Нет";
                                        break;
                                    }
                                case 1:
                                    {
                                        retVal = "Число";
                                        break;
                                    }
                                case 2:
                                    {
                                        retVal = "Строка";
                                        break;
                                    }
                            }
                            break;
                        }
                    case "UserAccountStatus":
                        {
                            switch (item)
                            {
                                case 0:
                                    {
                                        retVal = "Активный";
                                        break;
                                    }
                                case 1:
                                    {
                                        retVal = "Отключен";
                                        break;
                                    }
                                case 2:
                                    {
                                        retVal = "Удален";
                                        break;
                                    }
                            }
                            break;
                        }
                    case "UserGroupStatus":
                        {
                            switch (item)
                            {
                                case 0:
                                    {
                                        retVal = "Активная";
                                        break;
                                    }
                                case 1:
                                    {
                                        retVal = "Отключена";
                                        break;
                                    }
                                case 2:
                                    {
                                        retVal = "Удалена";
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
            catch (Exception)
            {
            }
            return retVal;
        }
    }
}