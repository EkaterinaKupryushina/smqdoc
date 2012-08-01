using System;


namespace MvcFront.Helpers
{
    /// <summary>
    /// Хелпер для получения текста по значению конкретного enum
    /// </summary>
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

                                case 3:
                                    {
                                        retVal = "Вычислимое";
                                        break;
                                    }
                                case 4:
                                    {
                                        retVal = "Планируемое";
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
                                        retVal = "Редактируемый (План)";
                                        break;
                                    }
                                case 1:
                                    {
                                        retVal = "Отправлен на рассмотрение (План)";
                                        break;
                                    }
                                case 2:
                                    {
                                        retVal = "Редактируемый (Факт)";
                                        break;
                                    }
                                case 3:
                                    {
                                        retVal = "Отправлен на рассмотрение (Факт)";
                                        break;
                                    }
                                case 4:
                                    {
                                        retVal = "Одобрен";
                                        break;
                                    }
                                case 5:
                                    {
                                        retVal = "Удален";
                                        break;
                                    }
                            }

                            break;
                        }
                    case "DocAppointmentStatus":
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