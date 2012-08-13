using System;
using System.IO;
using System.Web;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Infrastructure
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
        /// <param name="folderId"></param>
        /// <param name="comment"></param>
        public void CreateNewAsset(HttpPostedFileBase file, int folderId, string comment)
        {
            var newFileName = string.Format("{0}_{1}", DateTime.Now.ToString("ssmmHHMMddyyyy"), Guid.NewGuid());
            file.SaveAs(Path.Combine(SmqSettings.Instance.AssetFolder, newFileName));
            var asset = new Asset
                            {
                                assetid = 0, 
                                Name = Path.GetFileName(file.FileName), 
                                FileName = newFileName, 
                                AssetFolder_assetfolderid = folderId,
                                Comment = comment
                            };
            _assetRepository.SaveAsset(asset);
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

        //Удаляет папку вместе с содержимым
        public void DeleteAssetFolder(AssetFolder folder)
        {
            foreach (var asset in folder.Assets)
            {
                try
                {
                    var assFile = new FileInfo(Path.Combine(SmqSettings.Instance.AssetFolder, asset.FileName));
                    assFile.Delete();
                }
                catch (Exception)
                {
                }
                
            }
            _assetRepository.DeleteAssetFolder(folder.assetfolderid);
        }
    }
}