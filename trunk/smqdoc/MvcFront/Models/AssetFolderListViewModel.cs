using System;
using System.ComponentModel.DataAnnotations;
using MvcFront.DB;

namespace MvcFront.Models
{
    public class AssetFolderListViewModel
    {
        [Display(Name = "ID")]
        [UIHint("Hidden")]
        public int AssetFolderId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Родитель")]
        public string Parent { get; set; }

        public AssetFolderListViewModel()
        {
        }

        public AssetFolderListViewModel(AssetFolder folder)
        {
            AssetFolderId = folder.assetfolderid;
            Name = folder.Name;
            Parent = folder.ParentAssetFolder != null ? folder.ParentAssetFolder.Name : string.Empty;
        }

        public static AssetFolderListViewModel AssetFolderToModelConverter(AssetFolder templ)
        {
            return new AssetFolderListViewModel(templ);
        }
    }
}