using System;
using System.Data.Objects;
using System.Linq;
using MvcFront.DB;
using MvcFront.Enums;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssetRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Asset GetAssetById(int id)
        {
            if (id == 0)
                return new Asset();
            return _unitOfWork.DbModel.Assets.SingleOrDefault(x => x.assetid == id);
        }

        public AssetFolder GetAssetFolderById(int id)
        {
            if (id == 0)
                return new AssetFolder();
            return _unitOfWork.DbModel.AssetFolders.SingleOrDefault(x => x.assetfolderid == id);
        }

        public IQueryable<Asset> GetAllAssets()
        {
            return _unitOfWork.DbModel.Assets;
        }

        public IQueryable<AssetFolder> GetAllAssetFolders()
        {
            return _unitOfWork.DbModel.AssetFolders;
        }

        public void SaveAsset(Asset asset)
        {
            asset.LastEditDate = DateTime.Now;
            if (asset.assetid == 0)
            {
                _unitOfWork.DbModel.Assets.AddObject(asset);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, asset);
            }
            else
            {
                _unitOfWork.DbModel.Assets.ApplyCurrentValues(asset);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, asset);
            }
        }

        public void DeleteAsset(int assetId)
        {
            var asset = GetAssetById(assetId);
            _unitOfWork.DbModel.Assets.DeleteObject(asset);
            _unitOfWork.DbModel.SaveChanges();
        }

        public void SaveAssetFolder(AssetFolder assetFolder)
        {
            if (assetFolder.assetfolderid == 0)
            {
                _unitOfWork.DbModel.AssetFolders.AddObject(assetFolder);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, assetFolder);
            }
            else
            {
                _unitOfWork.DbModel.AssetFolders.ApplyCurrentValues(assetFolder);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, assetFolder);
            }
        }

        public void DeleteAssetFolder(int assetFolderId)
        {
            var assetFolder = GetAssetFolderById(assetFolderId);
            for (var i = 0; i < assetFolder.Assets.Count; i++)
            {
                _unitOfWork.DbModel.Assets.DeleteObject(assetFolder.Assets.ElementAt(i));
            }
            _unitOfWork.DbModel.AssetFolders.DeleteObject(assetFolder);
            _unitOfWork.DbModel.SaveChanges();
        }

    }
}