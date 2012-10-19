using System.Configuration;

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

        /// <summary>
        /// Папка хранений файлов
        /// </summary>
        public string AssetFolder
        {
            get { return ConfigurationManager.AppSettings["AssetFolder"]; }
        }

        /// <summary>
        /// папка для хранения файлов для библиотеки
        /// </summary>
        public string FileLibraryFolderName
        {
            get { return "Library"; }
        }

        /// <summary>
        /// папка для хранения файлов для докуменитов
        /// </summary>
        public string DocumentAttachmentsFolderName
        {
            get { return "DocumentFiles"; }
        }

        public string DoubleFormatStr
        {
            get { return "{0:0.##}"; }
        }
    }
}