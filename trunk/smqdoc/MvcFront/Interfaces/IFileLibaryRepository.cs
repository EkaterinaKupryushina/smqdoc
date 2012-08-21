using System.Linq;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IFileLibaryRepository
    {
        /// <summary>
        /// Возвращает asset по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileLibraryAsset GetAssetById(int id);

        /// <summary>
        /// Возвращает assetFolder по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileLibraryFolder GetAssetFolderById(int id);

        /// <summary>
        /// Возвращает список всех доступных asset
        /// </summary>
        /// <returns></returns>
        IQueryable<FileLibraryAsset> GetAllAssets();

        /// <summary>
        /// Возвращает список всех доступных assetFolder
        /// </summary>
        /// <returns></returns>
        IQueryable<FileLibraryFolder> GetAllAssetFolders();

        /// <summary>
        /// Создать или сохранить asses
        /// </summary>
        /// <param name="asset"></param>
        void SaveAsset(FileLibraryAsset asset);

        /// <summary>
        /// Удалить asset
        /// </summary>
        /// <param name="assetId"></param>
        void DeleteAsset(int assetId);

        /// <summary>
        /// Сохранить AssetFolder
        /// </summary>
        /// <param name="assetFolder"></param>
        void SaveAssetFolder(FileLibraryFolder assetFolder);

        /// <summary>
        /// Удалить assetFolder
        /// </summary>
        /// <param name="assetFolderId"></param>
        void DeleteAssetFolder(int assetFolderId);
    }
}