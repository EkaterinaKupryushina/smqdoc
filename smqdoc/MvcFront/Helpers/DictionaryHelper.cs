using System;


namespace MvcFront.Helpers
{
    public static class DictionaryHelper
    {
        public static string GetEnumText(Type typeOfEnum, int item)
        {
            var retVal = "";
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
                    case "DocumentStatus":
                        {
                            switch (item)
                            {
                                case 0:
                                    {
                                        retVal = "Редактируемый";
                                        break;
                                    }
                                case 1:
                                    {
                                        retVal = "Отправлен на рассмотрение";
                                        break;
                                    }
                                case 2:
                                    {
                                        retVal = "Одобрен";
                                        break;
                                    }
                                case 3:
                                    {
                                        retVal = "Удален";
                                        break;
                                    }
                            }
                            
                         break;   
                        }

                    case "GroupTemplateStatus":
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
                }
            }
            catch (Exception)
            {
                return retVal;
            }
            return retVal;
        }
    }
}