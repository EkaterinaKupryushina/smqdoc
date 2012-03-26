namespace MvcFront.Infrastructure
{
    public class SmqSettings
    {
        private static SmqSettings _instance;
        private string _applicationName = "SmqDoc";
        private int _deadlineDays = 7;
        private SmqSettings()
        {
        }

        public static SmqSettings Instance
        {
            get { return _instance ?? (_instance = new SmqSettings()); }
        }

        public string ApplicationName { get { return _applicationName; } set { _applicationName = value; } }
        public int DocumentsDedlineWarning
        {
            get { return _deadlineDays; }
            set { _deadlineDays = value; }
        }
    }
}