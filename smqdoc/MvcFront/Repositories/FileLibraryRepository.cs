using System;
using System.Data.Objects;
using System.Linq;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class FileLibraryRepository : IFileLibaryRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileLibraryRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public FileLibraryAsset GetAssetById(long id)
        {
            if (id == 0)
                return new FileLibraryAsset();
            return _unitOfWork.DbModel.FileLibraryAssets.SingleOrDefault(x => x.filelibraryassetid == id);
        }

        public FileLibraryFolder GetAssetFolderById(int id)
        {
            if (id == 0)
                return new FileLibraryFolder();
            return _unitOfWork.DbModel.FileLibraryFolders.SingleOrDefault(x => x.filelibraryfolderid == id);
        }

        public IQueryable<FileLibraryAsset> GetAllAssets()
        {
            return _unitOfWork.DbModel.FileLibraryAssets;
        }

        public IQueryable<FileLibraryFolder> GetAllAssetFolders()
        {
            return _unitOfWork.DbModel.FileLibraryFolders;
        }

        public void SaveAsset(FileLibraryAsset asset)
        {
            asset.LastEditDate = DateTime.Now;
            if (asset.filelibraryassetid == 0)
            {
                _unitOfWork.DbModel.FileLibraryAssets.AddObject(asset);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, asset);
            }
            else
            {
                _unitOfWork.DbModel.FileLibraryAssets.ApplyCurrentValues(asset);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, asset);
            }
        }

        public void DeleteAsset(long assetId)
        {
            var asset = GetAssetById(assetId);
            _unitOfWork.DbModel.FileLibraryAssets.DeleteObject(asset);
            _unitOfWork.DbModel.SaveChanges();
        }

        public void SaveAssetFolder(FileLibraryFolder assetFolder)
        {
            if (assetFolder.filelibraryfolderid == 0)
            {
                _unitOfWork.DbModel.FileLibraryFolders.AddObject(assetFolder);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, assetFolder);
            }
            else
            {
                _unitOfWork.DbModel.FileLibraryFolders.ApplyCurrentValues(assetFolder);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, assetFolder);
            }
        }

        public void DeleteAssetFolder(int assetFolderId)
        {
            var assetFolder = GetAssetFolderById(assetFolderId);
            for (var i = 0; i < assetFolder.FileLibraryAssets.Count; i++)
            {
                _unitOfWork.DbModel.FileLibraryAssets.DeleteObject(assetFolder.FileLibraryAssets.ElementAt(i));
            }
            _unitOfWork.DbModel.FileLibraryFolders.DeleteObject(assetFolder);
            _unitOfWork.DbModel.SaveChanges();
        }

    }
}