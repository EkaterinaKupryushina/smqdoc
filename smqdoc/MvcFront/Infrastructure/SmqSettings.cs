using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcFront.Infrastructure
{
    public class SmqSettings
    {
        private static SmqSettings _instance = null;
        private string _applicationName = "SmqDoc";
        private SmqSettings()
        {
        }

        public static SmqSettings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SmqSettings();
                return _instance;
            }
        }

        public string ApplicationName { get { return _applicationName; } set { _applicationName = value; } }
    }
}