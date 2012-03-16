namespace MvcFront.Infrastructure
{
    public class SmqSettings
    {
        private static SmqSettings _instance;
        private string _applicationName = "SmqDoc";
        private SmqSettings()
        {
        }

        public static SmqSettings Instance
        {
            get { return _instance ?? (_instance = new SmqSettings()); }
        }

        public string ApplicationName { get { return _applicationName; } set { _applicationName = value; } }
    }
}