using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IAssetRepository
    {
        /// <summary>
        /// Возвращает asset по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Asset GetAssetById(int id);

        /// <summary>
        /// Возвращает assetFolder по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AssetFolder GetAssetFolderById(int id);

        /// <summary>
        /// Возвращает список всех доступных asset
        /// </summary>
        /// <returns></returns>
        IQueryable<Asset> GetAllAssets();

        /// <summary>
        /// Возвращает список всех доступных assetFolder
        /// </summary>
        /// <returns></returns>
        IQueryable<AssetFolder> GetAllAssetFolders();

        /// <summary>
        /// Создать или сохранить asses
        /// </summary>
        /// <param name="asset"></param>
        void SaveAsset(Asset asset);

        /// <summary>
        /// Удалить asset
        /// </summary>
        /// <param name="assetId"></param>
        void DeleteAsset(int assetId);

        /// <summary>
        /// Сохранить AssetFolder
        /// </summary>
        /// <param name="assetFolder"></param>
        void SaveAssetFolder(AssetFolder assetFolder);

        /// <summary>
        /// Удалить assetFolder
        /// </summary>
        /// <param name="assetFolderId"></param>
        void DeleteAssetFolder(int assetFolderId);
    }
}