using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class FileLibraryFolderEditModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public Int32 AssetFolderId { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Корневая папка")]
        public bool IsRootFolder { get; set; }

        [Display(Name = "Папка")]
        public int? ParentAssetFolderId { get; set; }

        public FileLibraryFolderEditModel()
        {
        }

        public FileLibraryFolderEditModel(FileLibraryFolder folder)
        {
            AssetFolderId = folder.filelibraryfolderid;
            Name = folder.Name;
            IsRootFolder = folder.FileLibraryFolder_filelibraryfolderid == null;
            ParentAssetFolderId = folder.FileLibraryFolder_filelibraryfolderid;
        }

        public FileLibraryFolder Update(FileLibraryFolder folder)
        {
            folder.Name = Name;
            if(IsRootFolder || !ParentAssetFolderId.HasValue)
            {
                folder.FileLibraryFolder_filelibraryfolderid = null;
            }
            else
            {
                folder.FileLibraryFolder_filelibraryfolderid = ParentAssetFolderId.Value;
            }
            return folder;
        }
    }
}