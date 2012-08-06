﻿using System;
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
        /// <param name="name"></param>
        /// <param name="folderId"></param>
        public void CreateNewAsset(HttpPostedFile file, string name, int folderId)
        {
            var newFileName = string.Format("{0}_{1}", DateTime.Now.ToString("ssmmHHMMddyyyy"), file.FileName);
            file.SaveAs(VirtualPathUtility.Combine(SmqSettings.Instance.AssetFolder, newFileName));
            var asset = new Asset
                            {
                                assetid = 0, 
                                Name = name, 
                                FileName = newFileName, 
                                AssetFolder_assetfolderid = folderId
                            };
            _assetRepository.SaveAsset(asset);
        }

        /// <summary>
        /// Удаляет Asset
        /// </summary>
        /// <param name="asset"></param>
        public void DeleteAsset(Asset asset)
        {
            var file = new FileInfo(asset.FileName);
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
                    var assFile = new FileInfo(VirtualPathUtility.Combine(SmqSettings.Instance.AssetFolder, asset.FileName));
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