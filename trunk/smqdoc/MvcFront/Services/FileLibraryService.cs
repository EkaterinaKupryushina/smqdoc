using System.IO;
using System.Linq;
using System.Web;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Services
{
    public class FileLibraryService
    {
        private readonly IFileLibaryRepository _fileLibaryRepository;
        //Имя папки внутри папки хранения асетов
        private const string FolderName = "Library";

        public FileLibraryService(IFileLibaryRepository fileLibaryRepository)
        {
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
            var newFileName = (new AssetService()).CreateNewAsset(file, FolderName);
            var newFileAsset = new FileLibraryAsset
                {
                    filelibraryassetid = 0,
                    FileName = newFileName,
                    Name = Path.GetFileName(file.FileName),
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
            var asserService = new AssetService();
            var folder = _fileLibaryRepository.GetAssetFolderById(folderId);
            if(folder != null)
            {
                var assets = folder.FileLibraryAssets.ToList();
                foreach (var libraryAsset in assets)
                {
                    _fileLibaryRepository.DeleteAsset(libraryAsset.filelibraryassetid);
                    asserService.DeleteAsset(libraryAsset.FileName);
                }
                _fileLibaryRepository.DeleteAssetFolder(folder.filelibraryfolderid);
            }
        }

        /// <summary>
        /// Удаляет папку из библиотеки
        /// </summary>
        /// <param name="assetId"></param>
        public void DeleteFileAsset(long assetId)
        {
            var asserService = new AssetService();
            var asset = _fileLibaryRepository.GetAssetById(assetId);
            if (asset != null)
            {
                _fileLibaryRepository.DeleteAsset(asset.filelibraryassetid);
                asserService.DeleteAsset(asset.FileName);
            }
        }
    }
}