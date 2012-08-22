using System;
using System.IO;
using System.Web;
using MvcFront.Infrastructure;

namespace MvcFront.Services
{
    public class AssetService
    {
        /// <summary>
        /// Содает новый asset (создает запись в базе и файл на диске)
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName"> </param>
        public string CreateNewAsset(HttpPostedFileBase file, string folderName)
        {
            var newFileName = Path.Combine(folderName, string.Format("{0}_{1}", DateTime.Now.ToString("ssmmHHddMMyyyy"), Path.GetFileName(file.FileName)));
            var assetFolder = new DirectoryInfo(Path.Combine(SmqSettings.Instance.AssetFolder, folderName));
            if (!assetFolder.Exists)
            {
                assetFolder.Create();
            }
            file.SaveAs(Path.Combine(SmqSettings.Instance.AssetFolder, newFileName));
            return newFileName;
        }

        /// <summary>
        /// Удаляет Asset
        /// </summary>
        /// <param name="fileName"></param>
        public void DeleteAsset(string fileName)
        {
            var file = new FileInfo(Path.Combine(SmqSettings.Instance.AssetFolder, fileName));
            file.Delete(); 
        }
    }
}