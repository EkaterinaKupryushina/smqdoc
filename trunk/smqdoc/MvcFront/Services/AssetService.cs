using System;
using System.IO;
using System.Web;
using MvcFront.DB;
using MvcFront.Infrastructure;
using MvcFront.Interfaces;

namespace MvcFront.Services
{
    public class AssetService
    {
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        /// <summary>
        /// Содает новый asset (создает запись в базе и файл на диске)
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName"> </param>
        public Asset CreateNewAsset(HttpPostedFileBase file, string folderName)
        {
            var newFileName = string.Format("{0}_{1}", DateTime.Now.ToString("ssmmHHMMddyyyy"), Path.GetFileName(file.FileName));
            var assetFolder = new DirectoryInfo(Path.Combine(SmqSettings.Instance.AssetFolder, folderName));
            if (!assetFolder.Exists)
            {
                assetFolder.Create();
            }
            file.SaveAs(Path.Combine(SmqSettings.Instance.AssetFolder, folderName, newFileName));
            var asset = new Asset
                            {
                                assetid = 0, 
                                FileName =  Path.Combine(folderName, newFileName)
                            };
            _assetRepository.SaveAsset(asset);
            return asset;
        }

        /// <summary>
        /// Удаляет Asset
        /// </summary>
        /// <param name="asset"></param>
        public void DeleteAsset(Asset asset)
        {
            var file = new FileInfo(Path.Combine(SmqSettings.Instance.AssetFolder, asset.FileName));
            file.Delete(); 
            _assetRepository.DeleteAsset(asset.assetid);
        }
    }
}