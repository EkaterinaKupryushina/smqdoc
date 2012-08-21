using System.Web;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Services
{
    public class FileLibraryService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IFileLibaryRepository _fileLibaryRepository;

        public FileLibraryService(IAssetRepository assetRepository, IFileLibaryRepository fileLibaryRepository)
        {
            _assetRepository = assetRepository;
            _fileLibaryRepository = fileLibaryRepository;
        }

        /// <summary>
        /// Создает новый файл в библиотеке
        /// </summary>
        /// <param name="file"></param>
        /// <param name="parentFolderId"></param>
        /// <param name="comment"></param>
        public void CreateNewAsset(HttpPostedFileBase file, int parentFolderId, string comment)
        {
            var newAsset = (new AssetService(_assetRepository)).CreateNewAsset(file);
            var newFileAsset = new FileLibraryAsset
                {
                    filelibraryassetid = 0,
                    Asset = newAsset,
                    FileLibraryFolder_filelibraryfolderid = parentFolderId,
                    Comment = comment
                };
            _fileLibaryRepository.SaveAsset(newFileAsset);
        }

        /// <summary>
        /// Удаляет папку из библиотеки
        /// </summary>
        /// <param name="folderId"></param>
        public void DeleteFileAssetFolder(int folderId)
        {
            var asserService = new AssetService(_assetRepository);
            var folder = _fileLibaryRepository.GetAssetFolderById(folderId);
            if(folder != null)
            {
                foreach (var libraryAsset in folder.FileLibraryAssets)
                {
                    asserService.DeleteAsset(libraryAsset.Asset);
                }
                _fileLibaryRepository.DeleteAssetFolder(folder.filelibraryfolderid);
            }
        }
    }
}