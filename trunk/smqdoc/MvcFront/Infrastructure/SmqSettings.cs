namespace MvcFront.Infrastructure
{
    /// <summary>
    /// Класс хренения настроек и параметров системы
    /// </summary>
    public class SmqSettings
    {
        private static SmqSettings _instance;
        private int _deadlineDays = 7;
        private SmqSettings()
        {
        }

        public static SmqSettings Instance
        {
            get { return _instance ?? (_instance = new SmqSettings()); }
        }

        public int DocumentsDedlineWarning
        {
            get { return _deadlineDays; }
            set { _deadlineDays = value; }
        }
    }
}