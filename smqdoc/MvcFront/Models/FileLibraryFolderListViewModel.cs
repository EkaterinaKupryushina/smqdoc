using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class FileLibraryFolderListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public int AssetFolderId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Родитель")]
        public string Parent { get; set; }

        public FileLibraryFolderListViewModel()
        {
        }

        public FileLibraryFolderListViewModel(FileLibraryFolder folder)
        {
            AssetFolderId = folder.filelibraryfolderid;
            Name = folder.Name;
            Parent = folder.ParentFileLibraryFolder != null ? folder.ParentFileLibraryFolder.Name : string.Empty;
        }

        public static FileLibraryFolderListViewModel AssetFolderToModelConverter(FileLibraryFolder templ)
        {
            return new FileLibraryFolderListViewModel(templ);
        }
    }
}